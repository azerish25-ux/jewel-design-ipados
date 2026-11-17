'use strict';
const fs = require('node:fs');
const path = require('node:path');
const crypto = require('node:crypto');
const root = path.resolve(__dirname, '..');
const sha = bytes => crypto.createHash('sha256').update(bytes).digest('hex');
const check = (ok, why) => { if (!ok) throw Error(why); };
if (process.platform === 'win32') check(/^D:/i.test(root), 'Windows output must remain on D:');
const source = fs.readFileSync(path.join(root, '.local/reference/windows-sorting/Windows 11 Sorting Weight Table.txt'));
check(sha(source) === 'f0bab873ed8901ae17afc4848c7d9e0a7de4310942effb2b54b538411b26d542', 'Approved public source changed');
const lines = source.toString('utf8').split(/\r?\n/).map(line => line.split(';')[0].trim());
function rows(label, width, count) {
  const first = lines.findIndex(line => new RegExp('^' + label + '(?:\\s|$)').test(line));
  check(first >= 0, 'Missing source section');
  const result = [];
  for (let i = first + 1; i < lines.length; i++) {
    if (!lines[i]) continue;
    if (!/^0x/.test(lines[i])) break;
    const row = lines[i].split(/\s+/).map(Number);
    check(row.length === width && row.every(v => Number.isInteger(v) && v >= 0 && v <= 65535), 'Invalid public source row');
    result.push(row);
  }
  check(result.length === count && new Set(result.map(r => r[0])).size === count, 'Source count/identity');
  return result;
}
const defaults = rows('DEFAULT', 5, 60669), expansions = rows('EXPANSION', 3, 730), aliases = rows('COMPATIBILITY', 2, 955);
const table = Buffer.alloc(65536 * 4), present = new Set();
for (const [unit, sm, aw, dw, cw] of defaults) {
  check([sm, aw, dw, cw].every(v => v <= 255), 'Non-byte packed weight');
  table.set([aw, sm, dw, cw], unit * 4); present.add(unit);
}
const pairs = Buffer.alloc(730 * 4);
for (let i = 0; i < expansions.length; i++) {
  const [unit, first, second] = expansions[i];
  pairs.writeUInt16LE(first, i * 4); pairs.writeUInt16LE(second, i * 4 + 2);
  if (!present.has(unit)) { table[unit * 4 + 1] = 2; table.writeUInt16LE(i, unit * 4 + 2); present.add(unit); }
}
let pending = aliases.filter(([unit]) => !present.has(unit));
while (pending.length) {
  const next = [];
  for (const [unit, target] of pending) {
    if (!present.has(target)) { next.push([unit, target]); continue; }
    table.set(table.subarray(target * 4, target * 4 + 4), unit * 4); present.add(unit);
  }
  check(next.length < pending.length, 'Unresolved compatibility cycle'); pending = next;
}
check(sha(table) === '42ef9f244f091136a6a94ddf747b3d13e4cb1bf7b8367956abcdf0bb42affba0', 'Rebuilt complete native weight table differs');
check(sha(pairs) === '9d68be72650078da8ea7e72785b37abc8ffaaa87fcdd4f8ae507d0dcf258aa3e', 'Rebuilt ordered expansions differ');
const nativeBase = path.join(root, '.local/evidence/nls-engine/dispatch-07.json');
check(table.equals(fs.readFileSync(nativeBase + '.weights.bin')) && pairs.equals(fs.readFileSync(nativeBase + '.expansions.bin')), 'Active native inputs differ');
const header = Buffer.alloc(24); header.write('JDNLSI01', 0, 'ascii');
[1, 0x60502, 65536, 730].forEach((v, i) => header.writeUInt32LE(v, 8 + i * 4));
const result = Buffer.concat([header, table, pairs]);
const filename = 'src/Port/Rendering.Candidate/Fixtures/windows-nls1-comparison-inputs-60502.bin';
fs.writeFileSync(path.join(root, filename), result);
console.log(JSON.stringify({ path: filename, bytes: result.length, sha256: sha(result), characterWeights: 65536, orderedExpansions: 730, fullNativeEquality: true }));
