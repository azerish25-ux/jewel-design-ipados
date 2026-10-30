const fs = require("fs"),
    crypto = require("crypto"),
    zlib = require("zlib");
const RANGE = "56b84606-7f1d-41fb-b81f-f521d9a9577d",
    RING = "dd5c2342-d89a-440e-a76e-624a1faf2b68",
    MARKER = "b237ee6c-ac7e-4725-86ee-54bccda34fea";
const hash = (b) => crypto.createHash("sha256").update(b).digest("hex");
class Cursor {
    constructor(b, p = 0, limit = b.length) {
        this.b = b;
        this.p = p;
        this.limit = limit;
    }
    need(n) {
        if (!Number.isSafeInteger(n) || n < 0 || this.p + n > this.limit)
            throw Error("Out-of-range read at " + this.p);
    }
    i() {
        this.need(4);
        const x = this.b.readInt32LE(this.p);
        this.p += 4;
        return x;
    }
    u() {
        return this.i() >>> 0;
    }
    d() {
        this.need(8);
        const x = this.b.readDoubleLE(this.p);
        this.p += 8;
        if (!Number.isFinite(x)) throw Error("Non-finite double");
        return x;
    }
    count(max = 100000) {
        const n = this.i();
        if (n < 0 || n > max) throw Error("Invalid count at " + (this.p - 4));
        return n;
    }
    text() {
        const n = this.count(1000000);
        this.need(n * 2);
        if (n > 0 && this.b.readUInt16LE(this.p + n * 2 - 2) !== 0)
            throw Error("Unterminated UTF-16 string");
        const s = this.b
            .toString("utf16le", this.p, this.p + n * 2)
            .replace(/\0$/, "");
        this.p += n * 2;
        return s;
    }
    guid() {
        this.need(16);
        const s = this.b.subarray(this.p, this.p + 16);
        this.p += 16;
        return [
            Buffer.from(s.subarray(0, 4)).reverse().toString("hex"),
            Buffer.from(s.subarray(4, 6)).reverse().toString("hex"),
            Buffer.from(s.subarray(6, 8)).reverse().toString("hex"),
            s.subarray(8, 10).toString("hex"),
            s.subarray(10).toString("hex"),
        ].join("-");
    }
    ints() {
        const n = this.count(10000),
            a = [];
        for (let i = 0; i < n; i++) a.push(this.i());
        return a;
    }
    userData() {
        const n = this.count(10000),
            a = [];
        for (let i = 0; i < n; i++)
            a.push({ type: this.text(), key: this.text(), value: this.text() });
        return a;
    }
    chunk(expected) {
        const start = this.p,
            code = this.u(),
            length = this.i();
        if (code & 0x80000000 || length < 4)
            throw Error("Expected bounded long chunk at " + start);
        this.need(length);
        if (expected !== undefined && code !== expected)
            throw Error("Unexpected chunk type at " + start);
        return {
            offset: start,
            code,
            length,
            payload: this.p,
            end: this.p + length,
            crcOffset: this.p + length - 4,
        };
    }
}
function checkCrc(b, c, start = c.payload, end = c.crcOffset) {
    const stored = b.readUInt32LE(c.crcOffset),
        actual = zlib.crc32(b.subarray(start, end));
    if (stored !== actual) throw Error("CRC mismatch at " + c.offset);
    return { stored, actual, valid: true };
}
function numeric(b, node) {
    const q = new Cursor(b, node.payload, node.crcOffset),
        major = q.i(),
        minor = q.i();
    const wanted = node.guid === RANGE ? 1 : 2;
    if (major !== 1 || minor !== wanted)
        throw Error("Unsupported numeric record version");
    const identifier = q.text(),
        flags = q.u(),
        legacyLayer = q.i(),
        userData = q.userData(),
        objects = q.ints(),
        channels = q.ints(),
        categories = q.ints();
    if (objects.length !== channels.length) throw Error("Link array mismatch");
    let legacyUiInteger = null;
    if (major === 1 && minor > 0) legacyUiInteger = q.i();
    const valueOffset = q.p,
        value = q.d(),
        min = q.d(),
        max = q.d();
    let step = null;
    if (node.guid === RANGE) step = q.d();
    if (q.p !== node.crcOffset)
        throw Error("Numeric record not fully decoded: " + identifier);
    return {
        identifier,
        flags,
        legacyLayer,
        userData: userData.map((x) => ({
            type: x.type,
            key: x.key,
            valueCharacters: x.value.length,
            valueHash: hash(Buffer.from(x.value, "utf8")),
        })),
        objects,
        channels,
        categories,
        legacyUiInteger,
        valueOffset,
        value,
        min,
        max,
        step,
        fullyConsumed: true,
    };
}
function parse(b) {
    if (
        b.length < 32 ||
        b.toString("ascii", 0, 32) !== "3D Geometry File Format        4"
    )
        throw Error("Proof supports selected archive-4 sample only");
    const r = new Cursor(b, 32),
        envelope = [];
    for (const expected of [1, 0x10000014, 0x10000015]) {
        const c = r.chunk(expected);
        envelope.push(c);
        r.p = c.end;
    }
    const containerGuid = r.guid();
    if (containerGuid !== MARKER) throw Error("Unsupported container UUID");
    const info = r.chunk(0x40008000);
    const fileInfoVersion = [r.i(), r.i()];
    r.text();
    r.text();
    const build = { year: r.i(), month: r.i(), day: r.i(), revision: r.i() };
    const provenance = [];
    for (let n = r.count(10000); n > 0; n--) provenance.push(r.guid());
    if (r.p !== info.crcOffset) throw Error("File info trailing data");
    checkCrc(b, info);
    r.p = info.end;
    const udStart = r.p,
        ud = r.userData(),
        udEnd = r.p;
    const gem = r.chunk(0x40008000),
        gemVersion = [r.i(), r.i()],
        gemCount = r.count();
    r.p = gem.end;
    const category = r.chunk(0x40008000),
        categoryVersion = [r.i(), r.i()],
        categoryNames = [];
    for (let n = r.count(10000); n > 0; n--) categoryNames.push(r.text());
    if (r.p !== category.crcOffset) throw Error("Category table trailing data");
    checkCrc(b, category);
    r.p = category.end;
    const graph = r.chunk(0x40008000),
        graphVersion = [r.i(), r.i()];
    if (graphVersion.join(".") !== "1.0")
        throw Error("Unsupported graph version");
    const count = r.count(),
        guids = [];
    for (let i = 0; i < count; i++) guids.push(r.guid());
    const directoryEnd = r.p;
    checkCrc(b, graph, graph.payload, directoryEnd);
    const nodes = [];
    for (let i = 0; i < count; i++) {
        const c = r.chunk(0x40008000);
        const n = {
            index: i,
            guid: guids[i],
            ...c,
            major: r.i(),
            minor: r.i(),
            rawHash: hash(b.subarray(c.offset, c.end)),
            crc: checkCrc(b, c),
        };
        if (n.guid === RANGE || n.guid === RING) n.numeric = numeric(b, n);
        nodes.push(n);
        r.p = c.end;
    }
    if (r.p !== graph.crcOffset)
        throw Error("Graph tail does not match scoped CRC position");
    r.p = graph.end;
    const eof = {
        offset: r.p,
        code: r.u(),
        length: r.i(),
        archiveLength: r.u(),
    };
    if (
        eof.code !== 0x7fff ||
        eof.length !== 4 ||
        eof.archiveLength !== b.length ||
        r.p !== b.length
    )
        throw Error("Invalid EOF record");
    const regions = [
        ...envelope.map((c, i) => ({
            name: ["comment", "properties-and-preview", "settings"][i],
            offset: c.offset,
            bytes: c.end - c.offset,
        })),
        {
            name: "container-and-file-info",
            offset: envelope[2].end,
            bytes: info.end - envelope[2].end,
        },
        { name: "document-user-data", offset: udStart, bytes: udEnd - udStart },
        {
            name: "gem-definitions",
            offset: gem.offset,
            bytes: gem.end - gem.offset,
        },
        {
            name: "categories",
            offset: category.offset,
            bytes: category.end - category.offset,
        },
        {
            name: "graph-directory",
            offset: graph.offset,
            bytes: directoryEnd - graph.offset,
        },
        {
            name: "graph-scoped-crc-and-eof",
            offset: graph.crcOffset,
            bytes: b.length - graph.crcOffset,
        },
    ].map((x) => ({
        ...x,
        sha256: hash(b.subarray(x.offset, x.offset + x.bytes)),
    }));
    return {
        size: b.length,
        sha256: hash(b),
        header: b.toString("ascii", 0, 32),
        containerGuid,
        fileInfoVersion,
        build,
        provenanceGuids: provenance,
        documentUserData: ud.map((x) => ({
            type: x.type,
            key: x.key,
            characters: x.value.length,
            valueHash: hash(Buffer.from(x.value, "utf8")),
        })),
        gemVersion,
        gemCount,
        categoryVersion,
        categoryNames,
        graphVersion,
        count,
        numericCount: nodes.filter((x) => x.numeric).length,
        nodes,
        regions,
        eof,
        crcCoverage: {
            fileInfo: true,
            categories: true,
            graphDirectoryScoped: true,
            leafNodes: nodes.length,
            nestedGemAndPreviewSubchunks:
                "Preserved opaque; their nested CRCs not independently walked.",
        },
    };
}
module.exports = { parse, hash, RANGE, RING };
