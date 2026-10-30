const cp = require("child_process");
const { root, env, dotnetPath } = require("./local-env.cjs");
const [command, ...args] = process.argv.slice(2);
if (!command)
    throw Error("Usage: node scripts/run-local.cjs <command> [arguments]");
if (command === "dotnet" && !dotnetPath)
    throw Error(
        "Install the SDK inside .local/dotnet using scripts/bootstrap-dotnet.cjs; global Windows fallback is disabled",
    );
const result = cp.spawnSync(command === "dotnet" ? dotnetPath : command, args, {
    cwd: root,
    env,
    stdio: "inherit",
});
if (result.error) throw result.error;
process.exitCode = result.status ?? 1;
