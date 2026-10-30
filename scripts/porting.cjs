const fs = require("fs"),
    path = require("path");
const root = path.resolve(__dirname, "..");
const read = (f) => JSON.parse(fs.readFileSync(path.join(root, f), "utf8"));
function data() {
    const phases = read("docs/porting/phases.json").phases,
        rows = ["ui", "cad", "pc", "serializer", "builder"].flatMap(
            (k) => read("docs/porting/backlog/" + k + ".json").rows,
        );
    return { phases, rows };
}
function check() {
    const { phases, rows } = data(),
        ids = new Set(phases.map((p) => p.id)),
        tasks = new Map(phases.flatMap((p) => p.tasks.map((t) => [t.id, t]))),
        states = new Set([
            "not_started",
            "in_progress",
            "blocked",
            "implemented",
            "verified",
            "complete",
        ]);
    if (
        ids.size !== phases.length ||
        new Set(rows.map((r) => r.id)).size !== rows.length
    )
        throw Error("Duplicate phase/requirement ID");
    const seen = new Set(),
        active = new Set();
    function visit(id) {
        if (active.has(id)) throw Error("Cyclic phases");
        if (seen.has(id)) return;
        active.add(id);
        const p = phases.find((p) => p.id === id);
        if (!p) throw Error("Unknown phase " + id);
        for (const d of p.depends) visit(d);
        active.delete(id);
        seen.add(id);
    }
    for (const p of phases) {
        visit(p.id);
        if (!states.has(p.status)) throw Error("Invalid phase status");
        if (!fs.existsSync(path.join(root, p.plan)))
            throw Error("Missing phase plan " + p.id);
        for (const t of p.tasks) {
            if (!states.has(t.status)) throw Error("Invalid task state");
            for (const d of t.depends)
                if (!tasks.has(d)) throw Error("Missing task dependency");
            if (
                ["verified", "complete"].includes(t.status) &&
                (!t.evidence.length || !t.outputs.length)
            )
                throw Error("Completed task lacks evidence " + t.id);
        }
        if (
            p.status === "complete" &&
            (p.depends.some(
                (d) => phases.find((x) => x.id === d).status !== "complete",
            ) ||
                p.tasks.some(
                    (t) => !["verified", "complete"].includes(t.status),
                ))
        )
            throw Error("Premature phase completion " + p.id);
    }
    for (const r of rows) {
        if (!ids.has(r.phase) || !states.has(r.status))
            throw Error("Invalid requirement state " + r.id);
        for (const s of r.sourceEvidence || [])
            if (s.path && !fs.existsSync(path.join(root, s.path)))
                throw Error("Missing source evidence " + r.id + ": " + s.path);
        if (
            r.status === "verified" &&
            (!r.fixtureIds.length ||
                !r.testCommands.length ||
                !r.evidence.length ||
                !r.review)
        )
            throw Error("Verified row lacks evidence/review " + r.id);
    }
    return {
        phases: phases.length,
        tasks: tasks.size,
        requirementRows: rows.length,
        verifiedRequirements: rows.filter((r) => r.status === "verified")
            .length,
    };
}
function main(args) {
    const { phases, rows } = data(),
        [cmd, id, n = "10"] = args;
    if (cmd === "check") return console.log(JSON.stringify(check(), null, 2));
    if (cmd === "next") {
        const ready = phases.filter(
            (p) =>
                p.status !== "complete" &&
                p.status !== "blocked" &&
                p.depends.every(
                    (d) => phases.find((x) => x.id === d).status === "complete",
                ),
        );
        for (const p of ready) {
            const t = p.tasks.find(
                (t) =>
                    !["verified", "complete", "blocked"].includes(t.status) &&
                    t.depends.every((d) =>
                        ["verified", "complete"].includes(
                            p.tasks.find((x) => x.id === d)?.status,
                        ),
                    ),
            );
            console.log(
                p.id +
                    " " +
                    p.title +
                    " -> " +
                    (t
                        ? t.id + " " + t.title
                        : "waiting on tasks or phase review"),
            );
        }
        for (const p of phases.filter((p) => p.status === "blocked"))
            console.log("BLOCKED " + p.id + ": " + p.blockers.join("; "));
        return;
    }
    if (cmd === "show") {
        const p = phases.find((p) => p.id === id);
        if (!p) throw Error("Unknown phase");
        return console.log(fs.readFileSync(path.join(root, p.plan), "utf8"));
    }
    if (cmd === "backlog")
        return console.log(
            JSON.stringify(
                rows
                    .filter((r) => r.phase === id)
                    .slice(0, Math.min(100, Math.max(1, Number(n) || 10))),
                null,
                2,
            ),
        );
    if (cmd === "card") {
        const r = rows.find((r) => r.id === id);
        if (!r) throw Error("Unknown requirement");
        const match = r.guid
            ? rows.find(
                  (x) =>
                      x.kind === "serializer" &&
                      x.guid?.toLowerCase() === r.guid.toLowerCase(),
              )
            : null;
        return console.log(
            JSON.stringify(
                {
                    ...r,
                    serializerRequirement: match?.id,
                    requiredEvidence: [
                        "read source and effective inherited serializers",
                        "normal/non-default/boundary/invalid cases",
                        "fixture input SHA256 and PC build",
                        "real iOS output and independent comparison",
                        "review and supported-version record",
                    ],
                },
                null,
                2,
            ),
        );
    }
    throw Error(
        "Usage: porting.cjs next|show <phase>|backlog <phase> [limit]|card <requirement>|check",
    );
}
if (require.main === module) main(process.argv.slice(2));
module.exports = { check, data };
