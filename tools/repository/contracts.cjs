const fs = require("fs");
const path = require("path");
const crypto = require("crypto");
const root = path.resolve(__dirname, "../..");
const read = (file) =>
    JSON.parse(fs.readFileSync(path.join(root, file), "utf8"));

function check() {
    const { phases, rows } = require("../../scripts/porting.cjs").data();
    const builders = rows.filter((row) => row.kind === "builder");
    const serializers = new Map(
        rows
            .filter((row) => row.kind === "serializer")
            .map((row) => [row.id, row]),
    );
    const batches = read("docs/porting/batches/builders.json").batches;
    const assigned = batches.flatMap((batch) => batch.builderIds);
    if (
        builders.length !== 619 ||
        assigned.length !== 619 ||
        new Set(assigned).size !== 619
    )
        throw Error(
            "Builder batches must cover every one of 619 builders exactly once",
        );
    const builderIds = new Set(builders.map((row) => row.id));
    for (const batch of batches) {
        if (
            !batch.builderIds.length ||
            batch.builderIds.length > 20 ||
            batch.builderIds.some((id) => !builderIds.has(id))
        )
            throw Error("Invalid builder batch " + batch.id);
    }
    for (const row of builders) {
        const serializer = serializers.get(row.serializerId);
        if (
            !serializer ||
            serializer.guid.toLowerCase() !== row.guid.toLowerCase()
        )
            throw Error("Missing serializer cross-reference: " + row.id);
    }
    let completedTasks = 0;
    for (const phase of phases)
        for (const task of phase.tasks) {
            if (!["verified", "complete"].includes(task.status)) continue;
            const record = read("docs/porting/evidence/" + task.id + ".json");
            if (
                record.taskId !== task.id ||
                !record.outputs.length ||
                !record.checks.length ||
                !record.review
            )
                throw Error(
                    "Completed task lacks an evidence/review record: " +
                        task.id,
                );
            for (const output of record.outputs) {
                if (
                    !/^https:\/\//.test(output) &&
                    !fs.existsSync(path.join(root, output))
                )
                    throw Error("Missing evidence output: " + output);
            }
            for (const run of record.checks) {
                if (
                    typeof run.command !== "string" ||
                    !Number.isInteger(run.exitCode) ||
                    run.exitCode !== run.expectedExitCode ||
                    !run.artifact
                )
                    throw Error("Invalid execution evidence: " + task.id);
            }
            completedTasks++;
        }
    const fixture = read("reference/fixtures/library-item-4.provenance.json");
    const hash = crypto
        .createHash("sha256")
        .update(
            fs.readFileSync(
                path.join(root, "reference/fixtures/library-item-4.csg"),
            ),
        )
        .digest("hex");
    if (hash !== fixture.sha256) throw Error("Library fixture changed");
    return {
        builderBatches: batches.length,
        buildersAssigned: assigned.length,
        serializerCrossReferences: builders.length,
        completedTaskRecordsChecked: completedTasks,
        fixtureHashVerified: true,
    };
}
module.exports = { check };
