const { test } = require("node:test");
const assert = require("node:assert/strict");
const { discover, redact, MARKER } = require("./security.cjs");
test("does not mistake comparisons and reader expressions for credentials", () => {
    assert.equal(
        discover(
            'if (x.Password == null) return; x.Password = reader["Password"].ToString();',
        ).length,
        0,
    );
});
test("preserves public names, masked UI values and API paths", () => {
    for (const s of [
        'HiddenPassword = "HiddenPassword";',
        'HiddenPassword = "************";',
        'AUTHTOKENENDPOINT = "/auth/get-token";',
        'ProviderManifestToken="2012.Azure"',
    ])
        assert.equal(discover(s).length, 0);
});
test("redacts developer credential value while preserving source structure", () => {
    const s =
        'namespace CounterSketch; string devKey = "01234567-0000-4000-8000-012345678900";';
    const r = redact(s);
    assert.equal(
        r.text,
        'namespace CounterSketch; string devKey = "' + MARKER + '";',
    );
    assert.equal(r.findings.length, 1);
});
test("redacts whole remote connection string, not namespace identifiers", () => {
    const r = redact(
        'namespace CounterSketch; var c = new SqlConnection("Server=example.invalid;Password=CounterSketch;User ID=demo;");',
    );
    assert.equal(
        r.text,
        'namespace CounterSketch; var c = new SqlConnection("' + MARKER + '");',
    );
});
test("keeps documented local archive format constants", () => {
    assert.equal(
        redact(
            'const string CsBackupPassword="local-format-constant";',
            "BackupHelper.cs",
        ).findings.length,
        0,
    );
});
test("overlapping findings do not corrupt replacements", () => {
    const uri = "https://" + "hooks.slack.com/services/EXAMPLE/EXAMPLE/EXAMPLE";
    const r = redact('string UrlWithAccessToken="' + uri + '";');
    assert.equal(r.text, 'string UrlWithAccessToken="' + MARKER + '";');
});
test("redaction is idempotent", () => {
    const s = 'string apiKey="01234567-0000-4000-8000-012345678900";';
    const one = redact(s).text;
    assert.equal(redact(one).text, one);
    assert.equal(discover(one).length, 0);
});

test("does not whitelist credentials beginning with a label word", () => {
  assert.equal(discover('string apiKey = "password123456";').length, 1);
});
