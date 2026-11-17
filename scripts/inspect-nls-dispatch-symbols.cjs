'use strict';
const fs = require('node:fs');
const path = require('node:path');
const crypto = require('node:crypto');
const { peImage, msf, identityMatch } = require('./windows-gdi-symbols.cjs');
const root = path.resolve(__dirname, '..');
const sha = bytes => crypto.createHash('sha256').update(bytes).digest('hex');
const check = (ok, why) => { if (!ok) throw Error(why); };
const image = fs.readFileSync(path.join(root, '.local/reference/windows-nls/Kernel32.dll'));
const pdbBytes = fs.readFileSync(path.join(root, '.local/reference/windows-nls/kernel32.pdb'));
check(sha(image) === 'e132eba4166256e2f414b91fe7d5386590bc5ea8626aee5af9c9738383cf2ae1' && sha(pdbBytes) === 'c3127b30252d8a0249bf1ebf024f77e36b207123c41264905c807711a2dd659c', 'Pinned comparison module/symbol identity');
const pe = peImage(image), pdb = msf(pdbBytes), info = pdb.stream(1), dbi = pdb.stream(3), records = pdb.stream(dbi.readUInt16LE(20));
function guid(bytes, at) { return (bytes.readUInt32LE(at).toString(16).padStart(8, '0') + bytes.readUInt16LE(at + 4).toString(16).padStart(4, '0') + bytes.readUInt16LE(at + 6).toString(16).padStart(4, '0') + bytes.subarray(at + 8, at + 16).toString('hex')).toUpperCase(); }
const identity = identityMatch(pe.codeViews[0], { guid: guid(info, 12), age: info.readUInt32LE(8) }, dbi.readUInt32LE(8));
const symbols = [];
for (let p = 0; p + 4 <= records.length;) {
  const length = records.readUInt16LE(p), end = p + 2 + length;
  check(length >= 2 && end <= records.length, 'Symbol record bounds');
  if (records.readUInt16LE(p + 2) === 0x110e) {
    const section = records.readUInt16LE(p + 12), offset = records.readUInt32LE(p + 8), zero = records.indexOf(0, p + 14);
    check(zero >= p + 14 && zero < end, 'Symbol name bound');
    if (section > 0 && section <= pe.sections.length) symbols.push({ name: records.toString('utf8', p + 14, zero), rva: pe.sections[section - 1].rva + offset });
  }
  p = end;
}
let direct = 0, stubs = 0;
for (const item of pe.exports.filter(e => !e.forwarder)) {
  const exact = symbols.filter(s => s.name === item.name), alias = symbols.filter(s => s.name === item.name + 'Stub');
  if (exact.some(s => s.rva === item.rva)) { direct++; continue; }
  if (alias.some(s => s.rva === item.rva)) { stubs++; continue; }
  if (exact.length) {
    const at = pe.at(item.rva, 5);
    check(image[at] === 0xe9 && exact.some(s => s.rva === item.rva + 5 + image.readInt32LE(at + 1)), 'Unresolved named export identity ' + item.name);
    direct++;
  }
}
check(direct + stubs >= 1000 && stubs >= 12, 'Insufficient independent export/stub identities');
const wanted = ['SortCompareString', 'LongCompareString', 'GetOldHangulWeight', 'MapOldHangulSortKey', 'SortTblPtrs'];
const selected = wanted.map(name => { const rows = symbols.filter(s => s.name === name); check(rows.length === 1, 'Missing/ambiguous comparison symbol'); return rows[0]; });
check(selected.find(s => s.name === 'SortCompareString').rva === 23616 && selected.find(s => s.name === 'LongCompareString').rva === 26304 && selected.find(s => s.name === 'SortTblPtrs').rva === 795520, 'Observed callback/data symbols changed');
const dispatch = JSON.parse(fs.readFileSync(path.join(root, '.local/evidence/nls-engine/dispatch-07.json')));
const callbacks = dispatch.NlsGlobals.flatMap(g => g.Candidates);
check(callbacks.length === 1 && callbacks[0].Callback.Rva === 23616 && callbacks[0].Field130Null && callbacks[0].Field138Null && callbacks[0].Flags140 === 0 && callbacks[0].Field150Null, 'Observed fixed-profile callback context');
const code = image.subarray(pe.at(23616), pe.at(23616) + 32);
check(code.toString('hex') === callbacks[0].Callback.First32Hex, 'Live callback bytes differ from pinned image');
const result = { schemaVersion: 1, imageSha256: sha(image), pdbSha256: sha(pdbBytes), identity, publicSymbols: symbols.length, directExportMatches: direct, explicitStubMatches: stubs, selected, liveCallbackRva: 23616, liveFirst32Sha256: sha(code), fixedProfile: { caseMap130Null: true, caseMap138Null: true, traits140: 0, compression150Null: true }, originalTargetExecuted: false, scope: 'Exact owned-process callback, symbol and byte identity. Full direct comparator still requires implementation.' };
const output = path.join(root, '.local/evidence/nls-engine/kernel32-dispatch-symbols.json');
if (process.platform === 'win32') check(/^D:/i.test(root), 'Windows writes must remain on D:');
fs.writeFileSync(output, JSON.stringify(result, null, 2) + '\n');
process.stdout.write(JSON.stringify(result, null, 2) + '\n');
