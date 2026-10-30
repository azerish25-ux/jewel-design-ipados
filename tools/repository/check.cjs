const fs = require("fs"),
    path = require("path"),
    cp = require("child_process");
const root = path.resolve(__dirname, "../..");
const { discover } = require("./security.cjs");
function walk(d) {
    return fs.readdirSync(d, { withFileTypes: true }).flatMap((e) => {
        const f = path.join(d, e.name);
        return e.isDirectory()
            ? [".git", ".local", "bin", "obj", "artifacts"].includes(e.name)
                ? []
                : walk(f)
            : [f];
    });
}
function checks() {
    const results = {
        recovery: require("../../scripts/recovery.cjs").verify(),
        planning: require("../../scripts/porting.cjs").check(),
    };
    results.contracts = require("./contracts.cjs").check();
    let links = 0,
        scanned = 0;
    const bad = [];
    const files = walk(root);
    const textExt = new Set([
        ".cs",
        ".csproj",
        ".cjs",
        ".json",
        ".md",
        ".xaml",
        ".config",
        ".resx",
        ".yml",
        ".xml",
        ".manifest",
        ".csdl",
        ".msl",
        ".ssdl",
        ".txt",
    ]);
    for (const f of files) {
        const relative = path.relative(root, f).replaceAll("\\", "/");
        if (fs.statSync(f).size > 95 * 1024 * 1024)
            bad.push("Oversize Git blob " + relative);
        if (!textExt.has(path.extname(f))) continue;
        const text = fs.readFileSync(f, "utf8");
        if (relative !== "tools/repository/security.test.cjs") {
            const findings = discover(text, f);
            for (const q of findings)
                bad.push(
                    "Credential pattern " +
                        relative +
                        ":" +
                        text.slice(0, q.start).split("\n").length +
                        " (" +
                        q.kind +
                        ")",
                );
            scanned++;
        }
        if (
            f.endsWith(".md") &&
            (relative.startsWith("docs/") ||
                ["README.md", "AGENTS.md", "recovery/README.md"].includes(
                    relative,
                ))
        ) {
            const withoutCode = text
                .replace(/~~~[\s\S]*?~~~/g, "")
                .replace(/\x60\x60\x60[\s\S]*?\x60\x60\x60/g, "");
            for (const m of withoutCode.matchAll(
                /\[[^\]\n]*\]\((<[^>]+>|[^)\n]+)\)/g,
            )) {
                let target = m[1].replace(/^<|>$/g, "").split("#")[0];
                if (!target || /^(https?:|mailto:)/.test(target)) continue;
                try {
                    target = decodeURIComponent(target);
                } catch {}
                if (!fs.existsSync(path.resolve(path.dirname(f), target)))
                    bad.push("Broken link " + relative + " -> " + target);
                links++;
            }
        }
    }
    if (bad.length) throw Error(bad.join("\n"));
    results.navigation = { localLinksChecked: links };
    results.security = {
        textFilesScanned: scanned,
        noCredentialFindings: true,
    };
    return results;
}
try {
    const result = checks();
    console.log(JSON.stringify(result, null, 2));
    if (process.argv.includes("--record")) {
        const folder = path.join(root, ".local/evidence");
        fs.mkdirSync(folder, { recursive: true });
        fs.writeFileSync(
            path.join(folder, "repository-check.json"),
            JSON.stringify(result, null, 2),
        );
    }
} catch (error) {
    console.error(error.message);
    process.exitCode = 1;
}
