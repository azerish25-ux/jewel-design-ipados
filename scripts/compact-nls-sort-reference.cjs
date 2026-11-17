'use strict';
const fs = require('node:fs');
const path = require('node:path');
const crypto = require('node:crypto');
const zlib = require('node:zlib');
const root = path.resolve(__dirname, '..');
const sha = bytes => crypto.createHash('sha256').update(bytes).digest('hex');
if (process.platform === 'win32' && !/^D:/i.test(root)) throw Error('Windows output must remain on D:');
const input = path.resolve(root, process.argv[2] || '.local/evidence/nls-sort/windows-05.json');
if (!input.toLowerCase().startsWith(root.toLowerCase() + path.sep)) throw Error('Input leaves repository');
const json = fs.readFileSync(input), report = JSON.parse(json), raw = fs.readFileSync(input + '.native-keys.bin');
if (report.OriginalTargetExecuted !== false || report.Locale !== 1 || report.SortFlags !== 0x20401 || report.CompareFlags !== 0x20001 || report.Version.NlsVersion !== 0x60502 || report.NativeComparisonRepeatMismatches !== 0 || sha(raw) !== report.NativeKeyRecordsSha256 || raw.length !== report.NativeKeyRecordsBytes) throw Error('Unapproved native source/profile');
let cursor = 0, cases = 0;
for (let plane = 0; plane < 17; plane++) {
  const start = cursor;
  for (let i = 0; i < 65536; i++) {
    if (cursor + 8 > raw.length) throw Error('Truncated native record');
    const count = raw.readInt32LE(cursor), error = raw.readInt32LE(cursor + 4);
    if (count < 1 || count > 128 || error !== 0 || cursor + 8 + count > raw.length || raw[cursor + 7 + count] !== 0) throw Error('Invalid native key record');
    cursor += 8 + count; cases++;
  }
  if (report.Planes[plane].Index !== plane || report.Planes[plane].Cases !== 65536 || report.Planes[plane].Errors !== 0 || sha(raw.subarray(start, cursor)) !== report.Planes[plane].NativeSha256) throw Error('Native plane digest mismatch');
}
if (cursor !== raw.length || cases !== 1114112 || report.UnitScalarCases !== cases || report.Strings.length !== 15547 || report.Pairs.length !== 40663) throw Error('Native coverage mismatch');
let mismatches = 0;
for (const p of report.Pairs) {
  const a = report.Strings[p.Left], b = report.Strings[p.Right];
  if (!a || !b || p.NativeResult < 1 || p.NativeResult > 3 || p.NativeResult !== p.RepeatNativeResult || a.KeyError || b.KeyError) throw Error('Native comparison failed/repeat changed');
  const sign = Math.sign(Buffer.compare(Buffer.from(a.SortKeyBase64, 'base64'), Buffer.from(b.SortKeyBase64, 'base64')));
  if (sign !== p.SortKeySign) throw Error('Recorded key comparison differs');
  if (sign !== p.NativeResult - 2) mismatches++;
}
if (mismatches !== 213 || report.SortKeyCompareMismatches !== mismatches || report.NaiveUnitGroupCompositionMismatches !== 9407) throw Error('Rejected shortcut controls changed');
const outputs = [
  ['reference/fixtures/windows-nls1-sort-60502.json.br', json],
  ['reference/fixtures/windows-nls1-sort-keys-60502.bin.br', raw]
];
for (const [name, bytes] of outputs) {
  const packed = zlib.brotliCompressSync(bytes, { params: { [zlib.constants.BROTLI_PARAM_QUALITY]: 11 } });
  fs.writeFileSync(path.join(root, name), packed);
  console.log(JSON.stringify({ path: name, bytes: packed.length, sha256: sha(packed), decodedBytes: bytes.length, decodedSha256: sha(bytes) }));
}
