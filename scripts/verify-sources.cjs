const fs = require("fs"),
    p = require("path"),
    crypto = require("crypto");
let n = 0;
for (const c of ["desktop", "cad", "geometry"]) {
    const m = JSON.parse(fs.readFileSync("docs/reuse/" + c + "-manifest.json"));
    for (const f of m.retained) {
        const s = fs
            .readFileSync(p.join("src/Recovered", c, f.path), "utf8")
            .replace(/^﻿/, "");
        const h = crypto
            .createHash("sha256")
            .update(s)
            .digest("hex")
            .toUpperCase();
        if (h !== f.sha256) throw Error("Changed source " + c + "/" + f.path);
        n++;
    }
}
console.log("Verified " + n + " unchanged recovered files");
