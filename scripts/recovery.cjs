const fs = require("fs"),
    path = require("path"),
    crypto = require("crypto");
const root = path.resolve(__dirname, "..");
const load = (n) =>
    JSON.parse(
        fs.readFileSync(
            path.join(root, "recovery/manifests", n + ".json"),
            "utf8",
        ),
    );
const sha = (b) => crypto.createHash("sha256").update(b).digest("hex");
function inside(relative) {
    const f = path.resolve(root, relative);
    if (!f.startsWith(root + path.sep)) throw Error("Path escapes repository");
    return f;
}
function verify() {
    const files = load("source-files"),
        resources = load("resource-files");
    const unique = new Map([...files, ...resources].map((f) => [f.path, f]));
    for (const f of unique.values()) {
        const bytes = fs.readFileSync(inside(f.path));
        if (bytes.length !== f.bytes || sha(bytes) !== f.sha256)
            throw Error("Recovery hash mismatch: " + f.path);
    }
    return {
        sourceTextFiles: files.length,
        csharpFiles: files.filter((f) => f.path.endsWith(".cs")).length,
        uniqueResources: new Set(resources.map((f) => f.path)).size,
        resourceMappings: resources.length,
        uniqueFilesVerified: unique.size,
    };
}
function main(args) {
    const [command, q = ""] = args;
    if (command === "verify")
        return console.log(JSON.stringify(verify(), null, 2));
    if (command === "summary")
        return console.log(JSON.stringify(load("summary"), null, 2));
    if (command === "find" || command === "assets") {
        const list = load(
            command === "find" ? "source-files" : "resource-files",
        )
            .filter((f) =>
                (f.path + " " + f.origin)
                    .toLowerCase()
                    .includes(q.toLowerCase()),
            )
            .sort(
                (a, b) =>
                    Number(!a.path.includes("/canonical/")) -
                        Number(!b.path.includes("/canonical/")) ||
                    a.origin.localeCompare(b.origin),
            );
        for (const f of list.slice(0, 40))
            console.log(
                f.origin +
                    " -> " +
                    f.path +
                    (f.redacted ? " [credential redaction]" : ""),
            );
        console.log("Matches: " + list.length + "; showing at most 40");
        return;
    }
    if (command === "materialize") {
        if (!/^[A-Za-z0-9_-]+$/.test(q))
            throw Error("Specify one original recovery root");
        const prefix = "analysis/decompiled/" + q + "/";
        const list = [
            ...load("source-files"),
            ...load("resource-files"),
        ].filter((f) => f.origin.startsWith(prefix));
        if (!list.length) throw Error("Unknown snapshot");
        const out = inside(".local/materialized/" + q);
        for (const f of list) {
            const relative = f.origin.slice(prefix.length),
                target = path.resolve(out, relative);
            if (!target.startsWith(out + path.sep))
                throw Error("Unsafe source path");
            const bytes = fs.readFileSync(inside(f.path));
            if (sha(bytes) !== f.sha256) throw Error("Input hash mismatch");
            fs.mkdirSync(path.dirname(target), { recursive: true });
            fs.writeFileSync(target, bytes);
        }
        console.log(
            "Materialized " +
                list.length +
                " files under " +
                out +
                "; this does not install or run the original software",
        );
        return;
    }
    throw Error(
        "Usage: recovery.cjs summary|verify|find <query>|assets <name>|materialize <original-root>",
    );
}
if (require.main === module) main(process.argv.slice(2));
module.exports = { verify, inside, load };
