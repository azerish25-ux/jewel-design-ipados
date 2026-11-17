'use strict';
const fs = require('node:fs');
const path = require('node:path');
const crypto = require('node:crypto');
const zlib = require('node:zlib');
const root = path.resolve(__dirname, '..');
const sha = bytes => crypto.createHash('sha256').update(bytes).digest('hex');
const check = (ok, why) => { if (!ok) throw Error(why); };
const names = ['windows-nls1-sort-60502.json.br', 'windows-nls1-sort-keys-60502.bin.br', 'windows-nls1-published-weights-60502.bin.br'];
const hashes = ['556b9b2f9d6cfb092e66d4627721bb5fd6fc521f1ecf64715df31f1c2f2bd5ac', '1283c48f6da7a0a7e20a0d2e100febece1f6ae6190d769bcd092aba54c989a06', 'fb5b2c256e881c9254c1e2465ea1affa3e21fcbba0d7f9303f0a9e32407d4533'];
const decodedHashes = ['ce851aef6ab2e7e1a2c4a8126153825406d3d9662d9cc43d4e4101152f3daefa', 'e51e5d9459614f85ce7af2ea01a5cacadeb9c2afd941b3eb3582df4f847b66a0', '1093cb47b48a7dcf6f9153bba427add95858d1f6025fc821997f5bc1a08405f4'];
function verify(supplied) {
  const data = names.map((name, i) => {
    const packed = supplied?.[i] ?? fs.readFileSync(path.join(root, 'reference/fixtures', name));
    check(sha(packed) === hashes[i], 'NLS sort packed artifact digest ' + name);
    const bytes = zlib.brotliDecompressSync(packed, { maxOutputLength: 32 * 1024 * 1024 });
    check(sha(bytes) === decodedHashes[i], 'Decoded NLS sort artifact digest');
    return bytes;
  });
  const report = JSON.parse(data[0]), raw = data[1], weights = data[2];
  check(report.OriginalTargetExecuted === false && report.Locale === 1 && report.SortFlags === 0x20401 && report.CompareFlags === 0x20001 && report.CTerminated === true && report.Version.NlsVersion === 0x60502 && report.Version.EffectiveId === 1025, 'Native NLS sort profile');
  check(report.UnitScalarCases === 1114112 && report.UnitScalarKeyErrors === 0 && report.StringKeyErrors === 0 && report.CompareErrors === 0 && report.NativeComparisonRepeatMismatches === 0 && report.TargetedVariantPairs === 3400 && report.LengthBoundaryPairs === 72 && report.Strings.length === 15547 && report.Pairs.length === 40663 && report.Planes.length === 17, 'Native comparison coverage');
  const unitKeys = [], offsets = new Uint32Array(1114112);
  let cursor = 0, count = 0, max = 0;
  for (let plane = 0; plane < 17; plane++) {
    const start = cursor;
    for (let i = 0; i < 65536; i++) {
      check(cursor + 8 <= raw.length, 'Truncated native key');
      const size = raw.readInt32LE(cursor), error = raw.readInt32LE(cursor + 4);
      check(size >= 1 && size <= 128 && error === 0 && cursor + 8 + size <= raw.length && raw[cursor + 7 + size] === 0, 'Native key buffer/error');
      offsets[count] = cursor;
      if (plane === 0) unitKeys.push(raw.subarray(cursor + 8, cursor + 8 + size));
      max = Math.max(max, size); cursor += 8 + size; count++;
    }
    const p = report.Planes[plane];
    check(p.Index === plane && p.Cases === 65536 && p.Errors === 0 && sha(raw.subarray(start, cursor)) === p.NativeSha256, 'Full native plane key digest');
  }
  check(cursor === raw.length && cursor === report.NativeKeyRecordsBytes && count === 1114112 && max === report.MaximumSingleKeyBytes && sha(raw) === report.NativeKeyRecordsSha256, 'Complete native key stream');
  const strings = report.Strings.map(s => {
    const input = Buffer.from(s.InputUtf16Base64, 'base64'), key = Buffer.from(s.SortKeyBase64, 'base64');
    check(input.length % 2 === 0 && key.length > 0 && key.at(-1) === 0 && s.KeyError === 0, 'Native full-string storage');
    return { input, key };
  });
  let mismatchCount = 0;
  for (const p of report.Pairs) {
    check(Number.isInteger(p.Left) && Number.isInteger(p.Right) && strings[p.Left] && strings[p.Right] && [1, 2, 3].includes(p.NativeResult) && p.NativeResult === p.RepeatNativeResult, 'Native comparison/repetition');
    const sign = Math.sign(Buffer.compare(strings[p.Left].key, strings[p.Right].key));
    check(sign === p.SortKeySign, 'Independent native-key ordering');
    if (sign !== p.NativeResult - 2) mismatchCount++;
  }
  check(mismatchCount === 213 && report.SortKeyCompareMismatches === mismatchCount, 'Rejected sort-key comparator control changed');
  const unitGroups = unitKeys.map(key => {
    const groups = Array.from({ length: 5 }, () => []);
    let index = 0;
    for (const value of key) {
      if (value === 0) break;
      if (value === 1) { if (++index > 4) return null; }
      else groups[index].push(value);
    }
    return groups;
  });
  let naiveMismatches = 0, individualStringMatches = 0;
  for (const item of strings) {
    const groups = Array.from({ length: 5 }, () => []);
    let end = 0, valid = true;
    for (; end < item.input.length && item.input.readUInt16LE(end) !== 0; end += 2) {
      const unit = item.input.readUInt16LE(end), parts = unitGroups[unit];
      if (parts === null) { valid = false; break; }
      for (let i = 0; i < 5; i++) groups[i].push(...parts[i]);
    }
    const naive = valid ? Buffer.from(groups.flatMap((group, i) => [...group, i === 4 ? 0 : 1])) : null;
    if (!naive || !naive.equals(item.key)) naiveMismatches++;
    let keyIndex;
    if (end === 0) keyIndex = 0;
    else if (end === 2) keyIndex = item.input.readUInt16LE(0);
    else if (end === 4) {
      const a = item.input.readUInt16LE(0), b = item.input.readUInt16LE(2);
      if (a >= 0xd800 && a <= 0xdbff && b >= 0xdc00 && b <= 0xdfff) keyIndex = 65536 + ((a - 0xd800) << 10) + b - 0xdc00;
    }
    if (keyIndex !== undefined) {
      const at = offsets[keyIndex], size = raw.readInt32LE(at);
      check(raw.subarray(at + 8, at + 8 + size).equals(item.key), 'Individual-symbol/full-string native binding');
      individualStringMatches++;
    }
  }
  check(naiveMismatches === 9407 && report.NaiveUnitGroupCompositionMismatches === naiveMismatches, 'Rejected per-unit composition control changed');
  check(weights.length === 372238 && weights.toString('ascii', 0, 8) === 'JDNLSW01' && weights.readUInt32LE(8) === 0x60502 && weights.readUInt32LE(12) === 60669 && weights.readUInt32LE(16) === 730 && weights.readUInt32LE(20) === 955, 'Official parsed table format');
  let ordinaryMatches = 0, previous = -1;
  cursor = 24;
  for (let i = 0; i < 60669; i++, cursor += 6) {
    const unit = weights.readUInt16LE(cursor), sm = weights[cursor + 2], aw = weights[cursor + 3], dw = weights[cursor + 4];
    check(unit > previous, 'Official default row ordering'); previous = unit;
    if (sm < 7 || sm >= 34 || dw < 2) continue;
    const expected = Buffer.from([sm, aw, 1, ...(dw === 2 ? [] : [dw]), 1, 1, 1, 0]);
    check(expected.equals(unitKeys[unit]), 'Ordinary published/native single-key alignment'); ordinaryMatches++;
  }
  check(ordinaryMatches === 6029, 'Published ordinary-key coverage');
  for (const [rows, width] of [[730, 6], [955, 4]]) {
    previous = -1;
    for (let i = 0; i < rows; i++, cursor += width) { const unit = weights.readUInt16LE(cursor); check(unit > previous, 'Published expansion/compatibility ordering'); previous = unit; }
  }
  check(cursor === weights.length, 'Published table end');
  return { schemaVersion: 1, unitScalarKeys: count, fullStringKeys: strings.length, repeatedNativeComparisons: report.Pairs.length, targetedVariantPairs: 3400, lengthBoundaryPairs: 72, nativeErrors: 0, nativeRepeatMismatches: 0, rejectedSortKeyComparisonCases: mismatchCount, rejectedNaiveCompositionCases: naiveMismatches, individualStringBindings: individualStringMatches, defaultWeightRows: 60669, expansionRows: 730, compatibilityRows: 955, ordinaryPublishedNativeKeyMatches: ordinaryMatches, version: report.Version, scope: 'Native/public reference only. Full direct NLS comparator, special weight/Jamo algorithms, original wrapper/CRT/ANSI/document and PC/physical acceptance remain open.' };
}
module.exports = { verify, names };
if (require.main === module) process.stdout.write(JSON.stringify(verify(), null, 2) + '\n');
