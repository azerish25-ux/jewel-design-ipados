const MARKER = "REDACTED_EMBEDDED_CREDENTIAL";
const formatPaths = /(?:BackupHelper|LibraryPacker|NameFreeHandParts)\.cs$/i;
function discover(text, filename = "") {
    const out = [],
        add = (start, end, value, kind) => {
            if (value && !value.includes(MARKER))
                out.push({ start, end, value, kind });
        };
    for (const m of text.matchAll(/"(?:\\.|[^"\\\r\n])*"/g)) {
        const v = m[0].slice(1, -1),
            b = text.slice(Math.max(0, m.index - 180), m.index),
            n = /\b([A-Za-z_]\w*)\s*=\s*(?:@|\$)?$/.exec(b)?.[1];
        if (
            /\b(?:Password|Pwd)\s*=\s*(?!@|\{)[^;\s"<>]+/i.test(v) &&
            !formatPaths.test(filename)
        )
            add(
                m.index + 1,
                m.index + m[0].length - 1,
                v,
                "connection-password",
            );
        if (
            n &&
            /(?:password|secret|devkey|api_?key|credential|token|authorization|webhook)/i.test(
                n,
            ) &&
            !/(?:PublicKeyToken|ProviderManifestToken|ENDPOINT)$/i.test(n) &&
            v.length >= 6 &&
            !/^[*•●]+$/.test(v) &&
            v.toLowerCase() !== n.toLowerCase() &&
            !/^(?:null|password|secret|token|authorization|true|false|default)$/i.test(
                v,
            ) &&
            !/\{[^}]+\}/.test(v) &&
            !(formatPaths.test(filename) && /password/i.test(n))
        )
            add(
                m.index + 1,
                m.index + m[0].length - 1,
                v,
                "embedded-credential",
            );
    }
    for (const m of text.matchAll(
        /\bNetworkCredential\s*\(\s*"[^"\r\n]*"\s*,\s*"([^"\r\n]+)"/g,
    )) {
        const at = m.index + m[0].lastIndexOf(m[1]);
        if (m[1].length >= 4)
            add(at, at + m[1].length, m[1], "network-password");
    }
    for (const m of text.matchAll(
        /(?:key|name)\s*=\s*"([^"\r\n]*(?:password|secret|devkey|api_?key|credential|token|webhook)[^"\r\n]*)"[^>\r\n]*?\bvalue\s*=\s*"([^"\r\n]+)"/gi,
    )) {
        if (m[2].toLowerCase() === m[1].toLowerCase()) continue;
        const at = m.index + m[0].lastIndexOf(m[2]);
        add(at, at + m[2].length, m[2], "configuration-credential");
    }
    for (const m of text.matchAll(
        /(?:gh[pousr]_[A-Za-z0-9_]{20,}|github_pat_[A-Za-z0-9_]{20,}|AKIA[A-Z0-9]{16}|https:\/\/(?:hooks\.slack\.com\/services\/|discord(?:app)?\.com\/api\/webhooks\/)[^\s"<>]+)/g,
    ))
        add(m.index, m.index + m[0].length, m[0], "credential-pattern");
    return out
        .sort((a, b) => a.start - b.start || b.end - a.end)
        .filter(
            (q, i, a) =>
                !a
                    .slice(0, i)
                    .some((x) => x.start <= q.start && x.end >= q.end),
        );
}
function redact(text, filename = "") {
    const matches = discover(text, filename);
    let result = text;
    for (const m of [...matches].sort((a, b) => b.start - a.start))
        result = result.slice(0, m.start) + MARKER + result.slice(m.end);
    return {
        text: result,
        findings: matches.map((m) => ({
            kind: m.kind,
            line: text.slice(0, m.start).split("\n").length,
        })),
        values: matches.map((m) => m.value),
    };
}
module.exports = { MARKER, discover, redact };
