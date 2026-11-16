'use strict';
const fs = require('node:fs');
const path = require('node:path');
const crypto = require('node:crypto');
const zlib = require('node:zlib');
const root = path.resolve(__dirname, '..');
const sha = bytes => crypto.createHash('sha256').update(bytes).digest('hex');
const check = (ok, why) => { if (!ok) throw Error(why); };
function encodeScalar(value) {
  const bytes = Buffer.alloc(value > 65535 ? 4 : 2);
  if (value <= 65535) bytes.writeUInt16LE(value);
  else { value -= 65536; bytes.writeUInt16LE(0xd800 + (value >> 10)); bytes.writeUInt16LE(0xdc00 + (value & 1023), 2); }
  return bytes;
}
function verify(fixtureBytes, tableBytes, rawFile) {
  fixtureBytes ??= fs.readFileSync(path.join(root, 'reference/fixtures/windows-nls1-lower-60502.json.br'));
  tableBytes ??= fs.readFileSync(path.join(root, 'src/Port/Rendering.Candidate/Fixtures/windows-nls1-lower-60502.bin'));
  check(sha(fixtureBytes) === 'f0268c6a6b764d64d1e44cc52dde0b6712eee39b9c6417d9c8ab7d1f71ad519c', 'Native NLS lower fixture digest');
  const decoded = zlib.brotliDecompressSync(fixtureBytes, { maxOutputLength: 16 * 1024 * 1024 });
  check(sha(decoded) === '45f10d37f2950af872407236b8308008496e26e1e9d91dee8f5d6e17bbe9e6c9', 'Decoded native NLS lower digest');
  const data = JSON.parse(decoded);
  check(data.OriginalTargetExecuted === false && data.Locale === 1 && data.Flags === 256 && data.Version.NlsVersion === 0x60502 && data.Version.EffectiveId === 1025, 'NLS capture profile');
  check(data.Total === 1114112 && data.Errors === 0 && data.Planes.length === 17 && data.Mappings.length === 1013 && data.Controls.length === 3329 && data.ContextCompositionMismatches === 0 && data.PortableInvariantDifferences === 419, 'Native capture coverage');
  check(tableBytes.length === 8136 && sha(tableBytes) === '06e58c3cbbd38bcce4af8e99bb4f0693148fb5144ca9db23ef65fc9711d3ab70', 'Product mapping digest');
  check(tableBytes.toString('ascii', 0, 8) === 'JDNLSL01' && tableBytes.readUInt32LE(8) === 1 && tableBytes.readUInt32LE(12) === 256 && tableBytes.readUInt32LE(16) === 0x60502 && tableBytes.readUInt32LE(20) === 1025 && tableBytes.readUInt32LE(24) === 1013 && tableBytes.readUInt32LE(28) === 0, 'Product mapping header');
  const native = new Map(), product = new Map();
  let previous = -1;
  for (let i = 0; i < data.Mappings.length; i++) {
    const m = data.Mappings[i], bytes = Buffer.from(m.OutputUtf16Base64, 'base64');
    check(m.Input > previous && m.Input <= 0x10ffff && m.Error === 0 && m.Count === (m.Input > 65535 ? 2 : 1) && bytes.length === m.Count * 2, 'Native sparse mapping record');
    previous = m.Input;
    const key = tableBytes.readUInt32LE(32 + i * 8), value = tableBytes.readUInt32LE(36 + i * 8);
    check(key === m.Input && value <= 0x10ffff && (key > 65535) === (value > 65535) && !encodeScalar(key).equals(bytes) && encodeScalar(value).equals(bytes), 'Product/native mapping mismatch');
    native.set(m.Input, bytes); product.set(key, encodeScalar(value));
  }
  const all = crypto.createHash('sha256'), header = Buffer.alloc(8);
  let count = 0, recordBytes = 0;
  for (let plane = 0; plane <= 16; plane++) {
    const hash = crypto.createHash('sha256');
    for (let key = plane << 16; key < (plane + 1) * 65536; key++) {
      const output = native.get(key) ?? encodeScalar(key);
      header.writeInt32LE(output.length / 2);
      hash.update(header); hash.update(output); all.update(header); all.update(output);
      recordBytes += 8 + output.length; count++;
    }
    const p = data.Planes[plane];
    check(p.Index === plane && p.Cases === 65536 && p.Errors === 0 && p.NativeSha256 === hash.digest('hex'), 'Complete native plane digest');
  }
  check(count === data.Total && recordBytes === data.NativeRecordsBytes && all.digest('hex') === data.NativeRecordsSha256, 'Complete native record stream digest');
  if (rawFile) check(sha(fs.readFileSync(rawFile)) === data.NativeRecordsSha256, 'Acquired raw Windows record stream changed');
  for (const c of data.Controls) {
    const input = Buffer.from(c.InputUtf16Base64, 'base64'), chunks = [];
    check(input.length % 2 === 0 && c.Error === 0, 'Native context encoding/error');
    for (let i = 0; i < input.length; i += 2) {
      let key = input.readUInt16LE(i), width = 2;
      if (key >= 0xd800 && key <= 0xdbff && i + 3 < input.length) {
        const low = input.readUInt16LE(i + 2);
        if (low >= 0xdc00 && low <= 0xdfff) { key = 65536 + ((key - 0xd800) << 10) + low - 0xdc00; width = 4; }
      }
      chunks.push(product.get(key) ?? input.subarray(i, i + width)); i += width - 2;
    }
    const output = Buffer.from(c.OutputUtf16Base64, 'base64');
    check(Buffer.concat(chunks).equals(output) && c.Count === output.length / 2 && c.NativeCalled === (input.length > 0), 'Actual native context output');
  }
  return { schemaVersion: 1, unitScalarCases: count, nativeContexts: data.Controls.length, changedMappings: native.size, nativeErrors: 0, contextCompositionMismatches: 0, portableInvariantDifferences: 419, version: data.Version, nativeRecordsSha256: data.NativeRecordsSha256, productTableSha256: sha(tableBytes), scope: 'Exact recorded NLS1 lower mapping. Complete original filename comparison/CRT/ANSI/document and PC/physical remain separate.' };
}
module.exports = { verify };
if (require.main === module) process.stdout.write(JSON.stringify(verify(undefined, undefined, process.argv[2]), null, 2) + '\n');
