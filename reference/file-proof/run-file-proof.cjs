const fs = require("fs"),
    path = require("path"),
    zlib = require("zlib"),
    assert = require("assert/strict"),
    { parse, hash, RANGE } = require("./csg-v4.cjs");
const root = path.resolve(__dirname, "../../.local/file-proof"),
    source = path.resolve(__dirname, "../fixtures/library-item-4.csg"),
    expected =
        "73e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd";
function save(name, data) {
    const p = path.resolve(root, name);
    if (!p.startsWith(root + path.sep))
        throw Error("Output outside test folder");
    fs.mkdirSync(path.dirname(p), { recursive: true });
    const temp = p + ".partial";
    fs.writeFileSync(temp, data);
    fs.renameSync(temp, p);
    return p;
}
const input = fs.readFileSync(source);
assert.equal(hash(input), expected, "Unexpected input fixture");
const before = parse(input);
const matches = before.nodes.filter(
    (n) => n.guid === RANGE && n.numeric?.identifier === "Top Thickness",
);
assert.equal(matches.length, 2, "Fixture display-label ambiguity changed");
const node = matches.find((n) => n.index === 8);
assert(node, "Selected graph index missing");
const oldValue = node.numeric.value,
    newValue = 4.48;
assert.equal(oldValue, 4.38);
assert.equal(node.major, 1);
assert.equal(node.minor, 1);
assert(newValue >= node.numeric.min && newValue <= node.numeric.max);
assert(
    Math.abs(
        (newValue - node.numeric.min) / node.numeric.step -
            Math.round((newValue - node.numeric.min) / node.numeric.step),
    ) < 1e-8,
);
const files = {
    sourceCopy: save("cases/00-source-copy.csg", input),
    unchanged: save("cases/01-unchanged-save.csg", Buffer.from(input)),
};
const edited = Buffer.from(input);
edited.writeDoubleLE(newValue, node.numeric.valueOffset);
edited.writeUInt32LE(
    zlib.crc32(edited.subarray(node.payload, node.crcOffset)),
    node.crcOffset,
);
files.edited = save("cases/02-top-thickness-4.48.csg", edited);
const after = parse(fs.readFileSync(files.edited));
assert.equal(after.nodes[node.index].numeric.value, newValue);
assert.equal(after.count, before.count);
assert.equal(after.numericCount, before.numericCount);
const unchangedNodes = before.nodes.filter(
    (n) => n.index !== node.index && n.rawHash === after.nodes[n.index].rawHash,
);
assert.equal(unchangedNodes.length, before.count - 1);
assert.deepEqual(before.regions, after.regions);
assert.deepEqual(before.provenanceGuids, after.provenanceGuids);
assert.deepEqual(before.documentUserData, after.documentUserData);
assert.deepEqual(before.categoryNames, after.categoryNames);
const beforeOther = before.nodes
        .filter((n) => n.numeric && n.index !== node.index)
        .map((n) => n.numeric),
    afterOther = after.nodes
        .filter((n) => n.numeric && n.index !== node.index)
        .map((n) => n.numeric);
assert.deepEqual(beforeOther, afterOther);
const fieldsBefore = { ...node.numeric };
delete fieldsBefore.value;
const fieldsAfter = { ...after.nodes[node.index].numeric };
delete fieldsAfter.value;
assert.deepEqual(fieldsBefore, fieldsAfter);
const allowed = [
        { kind: "stored double", offset: node.numeric.valueOffset, bytes: 8 },
        { kind: "leaf CRC32", offset: node.crcOffset, bytes: 4 },
    ],
    changed = [];
for (let i = 0; i < input.length; i++)
    if (input[i] !== edited[i]) changed.push(i);
assert(changed.length > 0);
assert(
    changed.every((i) =>
        allowed.some((r) => i >= r.offset && i < r.offset + r.bytes),
    ),
);
const restored = Buffer.from(fs.readFileSync(files.edited));
restored.writeDoubleLE(oldValue, node.numeric.valueOffset);
restored.writeUInt32LE(
    zlib.crc32(restored.subarray(node.payload, node.crcOffset)),
    node.crcOffset,
);
files.restored = save("cases/03-restored-4.38.csg", restored);
parse(fs.readFileSync(files.restored));
assert(restored.equals(input));
assert(fs.readFileSync(files.unchanged).equals(input));
const bad = Buffer.from(edited);
bad[node.numeric.valueOffset] ^= 1;
files.badCrc = save("negative-controls/bad-leaf-crc.csg", bad);
let crcRejected = false,
    crcError = "";
try {
    parse(fs.readFileSync(files.badCrc));
} catch (e) {
    crcRejected = true;
    crcError = e.message;
}
assert(crcRejected && crcError.includes("CRC mismatch"));
let truncationRejected = false;
try {
    parse(input.subarray(0, input.length - 3));
} catch (e) {
    truncationRejected = true;
}
assert(truncationRejected);
const wrongVersion = Buffer.from(input);
wrongVersion.writeInt32LE(99, node.payload);
wrongVersion.writeUInt32LE(
    zlib.crc32(wrongVersion.subarray(node.payload, node.crcOffset)),
    node.crcOffset,
);
let unsupportedVersionRejected = false;
try {
    parse(wrongVersion);
} catch (e) {
    unsupportedVersionRejected = e.message.includes(
        "Unsupported numeric record version",
    );
}
assert(unsupportedVersionRejected);
assert.equal(hash(fs.readFileSync(source)), expected, "Source fixture changed");
const snapshots = {
    before: save("snapshots/before.json", JSON.stringify(before, null, 2)),
    after: save("snapshots/after.json", JSON.stringify(after, null, 2)),
    restored: save(
        "snapshots/restored.json",
        JSON.stringify(parse(restored), null, 2),
    ),
};
const result = {
    scope: "Limited legacy CSG file-format proof using copy-preserving edit. Not a CounterSketch UI save, recovered modern serializer save, geometry regeneration or PC/iPad/PC acceptance.",
    source,
    sourceSha256: expected,
    sourceUnchanged: true,
    selected: {
        index: node.index,
        typeGuid: node.guid,
        identifier: node.numeric.identifier,
        matchingDisplayLabels: matches.length,
        selection:
            "Exact fixture hash plus graph index 8 and type GUID; display label alone is ambiguous",
        recordVersion: "1.1",
        before: oldValue,
        after: newValue,
        range: [node.numeric.min, node.numeric.max],
        step: node.numeric.step,
        valueOffset: node.numeric.valueOffset,
        crcOffset: node.crcOffset,
    },
    files,
    snapshots,
    fileBytes: input.length,
    editSha256: hash(edited),
    unchangedSha256: hash(input),
    restoredSha256: hash(restored),
    changedByteCount: changed.length,
    changedByteOffsets: changed,
    allowedChanges: allowed,
    preserved: {
        otherNodeRecords: unchangedNodes.length,
        numericParametersOtherThanEdited: beforeOther.length,
        totalGraphRecords: before.count,
        opaqueAndMetadataRegions: before.regions.length,
        typeGuidDirectory: true,
        provenance: true,
        documentUserData: true,
        categories: true,
        selectedRecordOtherFields: true,
    },
    integrity: after.crcCoverage,
    controls: {
        unchangedByteIdentical: true,
        reverseEditByteIdentical: true,
        badCrcRejected: crcRejected,
        badCrcError: crcError,
        truncationRejected,
        unsupportedNumericVersionRejected: unsupportedVersionRejected,
    },
    notVerified: [
        "Opening or resaving in CounterSketch/Rhino itself",
        "Geometry regeneration, metal volume updates or manufacturing output",
        "Preview/render refresh (the old preview bytes are deliberately retained)",
        "All nested gem/preview chunk CRCs or opaque geometry semantics",
        "Full portfolio CSP/history/sidecars or other archive/record versions",
        "iPad implementation or PC -> iPad -> PC round trip",
    ],
    codeReferences: [
        { path: "recovery/canonical/cad/CS.CSObjects/CSObject.cs", line: 895 },
        {
            path: "recovery/canonical/cad/CS.CSObjects.CSBlocks/CSBlock.cs",
            line: 342,
        },
        { path: "recovery/canonical/cad/CS.CSValues/CSValueBase.cs", line: 93 },
        { path: "recovery/canonical/cad/CS.CSValues/CSValue.cs", line: 295 },
        {
            path: "recovery/canonical/cad/CS.CSValues/CSValueRange.cs",
            line: 142,
        },
    ],
};
save("file-roundtrip-result.json", JSON.stringify(result, null, 2));
console.log(
    JSON.stringify(
        {
            selected: result.selected,
            bytes: input.length,
            changedByteCount: changed.length,
            preserved: result.preserved,
            controls: result.controls,
            scope: result.scope,
        },
        null,
        2,
    ),
);
