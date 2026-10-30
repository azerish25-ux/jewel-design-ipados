const fs = require("fs"),
    path = require("path");
const root = path.resolve(__dirname, ".."),
    local = path.join(root, ".local");
if (process.platform === "win32" && !/^d:[\\/]/i.test(root)) throw Error("Windows checkout must stay on D:; no cache directories were created");
for (const n of [
    "temp",
    "dotnet-home",
    "nuget",
    "nuget-http",
    "nuget-plugins",
    "gh",
    "gh-cache",
    "cache",
])
    fs.mkdirSync(path.join(local, n), { recursive: true });
const nativeDotnet = path.join(
    local,
    "dotnet",
    process.platform === "win32" ? "dotnet.exe" : "dotnet",
);
const dotnetPath = fs.existsSync(nativeDotnet)
    ? nativeDotnet
    : process.platform === "win32"
      ? null
      : "dotnet";
const env = {
    ...process.env,
    TEMP: path.join(local, "temp"),
    TMP: path.join(local, "temp"),
    TMPDIR: path.join(local, "temp"),
    DOTNET_CLI_HOME: path.join(local, "dotnet-home"),
    NUGET_PACKAGES: path.join(local, "nuget"),
    NUGET_HTTP_CACHE_PATH: path.join(local, "nuget-http"),
    NUGET_PLUGINS_CACHE_PATH: path.join(local, "nuget-plugins"),
    GH_CONFIG_DIR: path.join(local, "gh"),
    GH_CACHE_DIR: path.join(local, "gh-cache"),
    XDG_CACHE_HOME: path.join(local, "cache"),
    DOTNET_GENERATE_ASPNET_CERTIFICATE: "false",
    DOTNET_MULTILEVEL_LOOKUP: "0",
    DOTNET_CLI_TELEMETRY_OPTOUT: "1",
    DOTNET_CLI_WORKLOAD_UPDATE_NOTIFY_DISABLE: "true",
    DOTNET_CLI_USE_MSBUILD_SERVER: "0",
    MSBUILDDISABLENODEREUSE: "1",
    GIT_OPTIONAL_LOCKS: "0",
};
if (fs.existsSync(nativeDotnet)) env.DOTNET_ROOT = path.dirname(nativeDotnet);
module.exports = { root, local, env, dotnetPath };
