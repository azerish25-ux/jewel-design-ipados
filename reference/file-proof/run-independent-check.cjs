const fs = require("fs"),
    path = require("path"),
    cp = require("child_process"),
    assert = require("assert/strict"),
    { env, dotnetPath } = require("../../scripts/local-env.cjs");
const root = path.resolve(__dirname, "../../.local/file-proof"),
    report = JSON.parse(
        fs.readFileSync(path.join(root, "file-roundtrip-result.json"), "utf8"),
    );
fs.mkdirSync(path.join(root, "independent"), { recursive: true });
function run(label, args, expected) {
    const log = path.join(root, "independent", label + ".log"),
        fd = fs.openSync(log, "w");
    const r = cp.spawnSync(dotnetPath, args, {
        env,
        cwd: root,
        stdio: ["ignore", fd, fd],
        timeout: 120000,
    });
    fs.closeSync(fd);
    assert.equal(
        r.status,
        expected,
        "Unexpected verifier/build exit: " +
            label +
            " " +
            fs.readFileSync(log, "utf8"),
    );
    return { label, exitCode: r.status, expectedExitCode: expected, log };
}
const build = run(
    "build",
    [
        "build",
        path.resolve(
            __dirname,
            "IndependentVerifier/IndependentVerifier.csproj",
        ),
        "-o",
        path.join(root, "verifier"),
        "--nologo",
        "-v:minimal",
        "-p:UseSharedCompilation=false",
        "-nr:false",
    ],
    0,
);
const exe = path.join(root, "verifier/IndependentVerifier.dll"),
    runs = [],
    snapshots = {};
for (const [name, input] of [
    ["source", report.files.sourceCopy],
    ["unchanged", report.files.unchanged],
    ["edited", report.files.edited],
    ["restored", report.files.restored],
]) {
    const out = path.join(root, "independent", name + ".json");
    runs.push(run(name, [exe, input, out, root], 0));
    snapshots[name] = JSON.parse(fs.readFileSync(out, "utf8"));
}
const badOut = path.join(root, "independent/bad-crc.json");
if (fs.existsSync(badOut)) fs.unlinkSync(badOut);
runs.push(run("bad-crc", [exe, report.files.badCrc, badOut, root], 1));
assert(!fs.existsSync(badOut), "Invalid input produced a success report");
assert(fs.readFileSync(runs.at(-1).log, "utf8").includes("CRC mismatch"));
const before = JSON.parse(fs.readFileSync(report.snapshots.before, "utf8")),
    after = JSON.parse(fs.readFileSync(report.snapshots.after, "utf8"));
for (const [name, model] of [
    ["source", before],
    ["edited", after],
]) {
    const c = snapshots[name];
    assert.equal(c.NodeCount, model.count);
    assert.equal(c.NumericNodes.length, model.numericCount);
    assert.equal(c.Sha256, model.sha256);
    for (const n of model.nodes) {
        const d = c.Nodes[n.index];
        assert.equal(d.TypeGuid, n.guid);
        assert.equal(d.Major, n.major);
        assert.equal(d.Minor, n.minor);
        assert.equal(d.RawSha256, n.rawHash);
        assert(d.CrcValid);
        if (n.numeric) {
            const v = c.NumericNodes.find((x) => x.Index === n.index);
            assert(v);
            assert.equal(v.Identifier, n.numeric.identifier);
            assert.equal(v.Value, n.numeric.value);
            assert.equal(v.Minimum, n.numeric.min);
            assert.equal(v.Maximum, n.numeric.max);
            assert.equal(v.Step, n.numeric.step);
            assert.equal(v.ValueOffset, n.numeric.valueOffset);
            assert.equal(v.Flags, n.numeric.flags);
            assert.equal(v.Layer, n.numeric.legacyLayer);
            assert.deepEqual(v.Objects, n.numeric.objects);
            assert.deepEqual(v.Channels, n.numeric.channels);
            assert.deepEqual(v.Categories, n.numeric.categories);
            assert.equal(v.LegacyUiInteger, n.numeric.legacyUiInteger);
        }
    }
}
assert.equal(snapshots.source.Sha256, report.sourceSha256);
assert.equal(snapshots.unchanged.Sha256, report.sourceSha256);
assert.equal(snapshots.edited.Sha256, report.editSha256);
assert.equal(snapshots.restored.Sha256, report.sourceSha256);
assert.equal(
    snapshots.edited.NumericNodes.find((n) => n.Index === 8).Value,
    4.48,
);
assert.equal(
    snapshots.restored.NumericNodes.find((n) => n.Index === 8).Value,
    4.38,
);
for (const k of [
    "DocumentUserDataHash",
    "GemTableHash",
    "CategoryTableHash",
    "GraphDirectoryCrc",
])
    assert.equal(snapshots.source[k], snapshots.edited[k]);
const result = {
    status: "PASS for bounded file-format proof only",
    build,
    runs,
    crossLanguage: {
        beforeAndAfterNodeRecords: before.count,
        perFileNumericRecords: before.numericCount,
        decodedNumericFieldsAndLinkArraysMatch: true,
    },
    sourceAndControls: {
        sourceCopyOriginalHash: true,
        unchangedSaveOriginalHash: true,
        editedExpectedHash: true,
        restoredOriginalHash: true,
        negativeCrcRejectedByIndependentReader: true,
    },
    selected: {
        graphIndex: 8,
        name: "Top Thickness",
        before: 4.38,
        savedAndReread: 4.48,
        restoredAndReread: 4.38,
    },
    nativeCounterSketchReopen:
        "NOT RUN: original native application writes outside D are not contained",
    geometryRegeneration: "NOT RUN",
    ipadRoundTrip: "NOT RUN",
    scope: "Independent managed reader plus Node editor; neither loads any CounterSketch/Rhino assembly.",
};
const p = path.join(root, "independent-verification.json");
fs.writeFileSync(p + ".partial", JSON.stringify(result, null, 2));
fs.renameSync(p + ".partial", p);
console.log(
    JSON.stringify(
        {
            status: result.status,
            runs: runs.map((x) => ({ label: x.label, exitCode: x.exitCode })),
            crossLanguage: result.crossLanguage,
            selected: result.selected,
            nativeCounterSketchReopen: result.nativeCounterSketchReopen,
        },
        null,
        2,
    ),
);
