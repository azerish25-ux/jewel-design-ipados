'use strict';
const fs = require('node:fs');
const path = require('node:path');
const crypto = require('node:crypto');
const zlib = require('node:zlib');
const root = path.resolve(__dirname, '..');
const source = path.join(root, '.local/reference/windows-sorting/Windows 11 Sorting Weight Table.txt');
const rawFile = path.join(root, '.local/evidence/nls-sort/windows-05.json.native-keys.bin');
const sha = bytes => crypto.createHash('sha256').update(bytes).digest('hex');
const check = (ok, why) => { if (!ok) throw Error(why); };
if (process.platform === 'win32') check(/^D:/i.test(root), 'Windows output must remain on D:');
const bytes = fs.readFileSync(source);
check(sha(bytes) === 'f0bab873ed8901ae17afc4848c7d9e0a7de4310942effb2b54b538411b26d542', 'Official Windows11 table identity');
const lines = bytes.toString('utf8').split(/\r?\n/).map(line => line.split(';')[0].trim());
const versionLine = lines.find(line => /^NLSVERSION\s/.test(line));
check(versionLine && Number(versionLine.split(/\s+/)[1]) === 0x60502, 'Official NLS version differs');
function section(label, width, expected) {
  const start = lines.findIndex(line => new RegExp('^' + label + '(?:\\s|$)').test(line));
  check(start >= 0, 'Missing official section ' + label);
  const result = [];
  for (let i = start + 1; i < lines.length; i++) {
    if (!lines[i]) continue;
    if (!/^0x/.test(lines[i])) break;
    const values = lines[i].split(/\s+/).map(Number);
    check(values.length === width && values.every(v => Number.isInteger(v) && v >= 0 && v <= 65535), 'Malformed official row ' + label);
    result.push(values);
  }
  check(result.length === expected && new Set(result.map(r => r[0])).size === expected, 'Official section count/duplicates ' + label);
  return result.sort((a, b) => a[0] - b[0]);
}
const defaults = section('DEFAULT', 5, 60669), expansion = section('EXPANSION', 3, 730), compatibility = section('COMPATIBILITY', 2, 955);
check(defaults.every(row => row.slice(1).every(v => v <= 255)), 'Weight outside byte range');
const output = Buffer.alloc(24 + defaults.length * 6 + expansion.length * 6 + compatibility.length * 4);
output.write('JDNLSW01', 0, 'ascii');
[0x60502, defaults.length, expansion.length, compatibility.length].forEach((value, i) => output.writeUInt32LE(value, 8 + i * 4));
let cursor = 24;
for (const row of defaults) { output.writeUInt16LE(row[0], cursor); for (let i = 0; i < 4; i++) output[cursor + 2 + i] = row[i + 1]; cursor += 6; }
for (const row of expansion) for (const value of row) { output.writeUInt16LE(value, cursor); cursor += 2; }
for (const row of compatibility) for (const value of row) { output.writeUInt16LE(value, cursor); cursor += 2; }
check(cursor === output.length, 'Parsed weight stream size');
const native = fs.readFileSync(rawFile);
check(sha(native) === 'e51e5d9459614f85ce7af2ea01a5cacadeb9c2afd941b3eb3582df4f847b66a0', 'Native reference stream identity');
const keys = [];
cursor = 0;
for (let unit = 0; unit < 65536; unit++) { const size = native.readInt32LE(cursor); keys.push(native.subarray(cursor + 8, cursor + 8 + size)); cursor += 8 + size; }
let ordinary = 0;
for (const [unit, sm, aw, dw] of defaults) {
  if (sm < 7 || sm >= 34 || dw < 2) continue;
  const expected = Buffer.from([sm, aw, 1, ...(dw === 2 ? [] : [dw]), 1, 1, 1, 0]);
  check(expected.equals(keys[unit]), 'Ordinary published/native weight mismatch U+' + unit.toString(16));
  ordinary++;
}
check(ordinary === 6029, 'Ordinary weight coverage changed');
const packed = zlib.brotliCompressSync(output, { params: { [zlib.constants.BROTLI_PARAM_QUALITY]: 11 } });
const name = 'reference/fixtures/windows-nls1-published-weights-60502.bin.br';
fs.writeFileSync(path.join(root, name), packed);
console.log(JSON.stringify({ path: name, bytes: packed.length, sha256: sha(packed), decodedBytes: output.length, decodedSha256: sha(output), defaultRows: defaults.length, expansionRows: expansion.length, compatibilityRows: compatibility.length, nativeOrdinaryKeysMatched: ordinary, scope: 'Parsed public reference data; validates ordinary single-key weights only. Complete direct comparer/special-key algorithms and Jamo state data remain separate.' }));
