const fs = require("fs"),
    path = require("path"),
    crypto = require("crypto"),
    cp = require("child_process");
const { pipeline } = require("stream/promises"),
    { Readable } = require("stream");
const { root, local, env } = require("./local-env.cjs");
async function main() {
    const version = JSON.parse(
        fs.readFileSync(path.join(root, "global.json"), "utf8"),
    ).sdk.version;
    const target = path.join(local, "dotnet"),
        executable = path.join(
            target,
            process.platform === "win32" ? "dotnet.exe" : "dotnet",
        );
    if (
        fs.existsSync(executable) &&
        fs.existsSync(path.join(target, "sdk", version))
    ) {
        console.log("Local SDK " + version + " already present: " + target);
        return;
    }
    if (process.argv.includes("--check"))
        throw Error("Local SDK missing: run this script without --check");
    const os = { win32: "win", darwin: "osx", linux: "linux" }[
        process.platform
    ];
    if (!os || !["x64", "arm64"].includes(process.arch))
        throw Error("Unsupported SDK platform");
    const rid = os + "-" + process.arch,
        metadata = await (
            await fetch(
                "https://builds.dotnet.microsoft.com/dotnet/release-metadata/10.0/releases.json",
            )
        ).json();
    const sdk = metadata.releases
        .flatMap((r) => r.sdks || [r.sdk])
        .find((s) => s?.version === version);
    if (!sdk) throw Error("Pinned SDK absent from official metadata");
    const extension = os === "win" ? ".zip" : ".tar.gz",
        file = sdk.files.find(
            (f) => f.rid === rid && f.name.endsWith(extension),
        );
    if (!file) throw Error("SDK archive missing");
    if (
        !/^https:\/\/(builds\.dotnet\.microsoft\.com|download\.visualstudio\.microsoft\.com)\//.test(
            file.url,
        )
    )
        throw Error("Unexpected SDK download host");
    const folder = path.join(local, "downloads");
    fs.mkdirSync(folder, { recursive: true });
    const archive = path.join(
        folder,
        "dotnet-sdk-" + version + "-" + rid + extension,
    );
    if (!fs.existsSync(archive)) {
        const response = await fetch(file.url);
        if (!response.ok) throw Error("SDK download HTTP " + response.status);
        await pipeline(
            Readable.fromWeb(response.body),
            fs.createWriteStream(archive + ".partial"),
        );
        fs.renameSync(archive + ".partial", archive);
    }
    const digest = crypto.createHash("sha512");
    for await (const chunk of fs.createReadStream(archive))
        digest.update(chunk);
    if (digest.digest("hex").toLowerCase() !== file.hash.toLowerCase())
        throw Error("SDK SHA512 mismatch; archive not extracted");
    fs.mkdirSync(target, { recursive: true });
    const extraction = cp.spawnSync("tar", ["-xf", archive, "-C", target], {
        cwd: root,
        env,
        stdio: "inherit",
    });
    if (extraction.error || extraction.status !== 0)
        throw Error("Archive extraction failed; keep all files in .local");
    if (!fs.existsSync(executable))
        throw Error("SDK executable missing after extraction");
    fs.writeFileSync(
        path.join(folder, "sdk-provenance.json"),
        JSON.stringify(
            { version, rid, url: file.url, sha512: file.hash },
            null,
            2,
        ),
    );
    console.log(
        "Verified SDK installed under " +
            target +
            "; no global installation performed",
    );
}
main().catch((e) => {
    console.error(e.message);
    process.exitCode = 1;
});
