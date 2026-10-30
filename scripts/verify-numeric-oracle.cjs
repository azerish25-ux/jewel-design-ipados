"use strict";
const fs = require("node:fs"), path = require("node:path"), crypto = require("node:crypto");
const root = path.resolve(__dirname, "..");
const canonical = "recovery/canonical/cad/CS.CSValues/CSValue.cs";
const harness = "tests/Port/Domain.Contracts/Recovered/LegacyNumericValue.cs";
function extract(source) {
    const bodies = [...source.replaceAll("\r\n", "\n").matchAll(/public bool SetValue\(double newValue\)\s*\{[\s\S]*?\n\t\}/g)];
    if (bodies.length !== 1) throw Error("Expected one exact scalar setter body");
    return bodies[0][0];
}
function verifyPair(source, harnessText) {
    if (crypto.createHash("sha256").update(source).digest("hex") !== "a7ccbf583e3576daf067140b5af52b21941bfc316e905cfebb48402b0ece6210")
        throw Error("Numeric oracle canonical source changed");
    const expected = extract(source.toString());
    const actual = extract(harnessText);
    if (actual !== expected) throw Error("Test oracle scalar method differs from canonical source");
    return { canonical, harness, methodSha256: crypto.createHash("sha256").update(actual).digest("hex"), lineEndings: "normalized LF", scope: "method body only; test callbacks are observation instrumentation" };
}
function verify() {
    return verifyPair(fs.readFileSync(path.join(root, canonical)), fs.readFileSync(path.join(root, harness), "utf8"));
}
module.exports = { verify, verifyPair };
if (require.main === module) console.log(JSON.stringify(verify(), null, 2));
