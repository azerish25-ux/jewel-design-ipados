'use strict';
const fs = require('node:fs');
const path = require('node:path');
const crypto = require('node:crypto');
const root = path.resolve(__dirname, '..');
const sha = bytes => crypto.createHash('sha256').update(bytes).digest('hex');
const check = (value, message) => { if (!value) throw Error(message); };
function verify(supplied) {
  const bytes = supplied ?? fs.readFileSync(path.join(root, 'src/Port/Rendering.Candidate/Fixtures/windows-nls1-comparison-inputs-60502.bin'));
  check(bytes.length === 265088 && sha(bytes) === '79d2e430fb8e0ee7b9558cdc3119dc609e2f1798392c50a63a4895bbc87533dd', 'Comparison input resource digest');
  check(bytes.toString('ascii', 0, 8) === 'JDNLSI01' && bytes.readUInt32LE(8) === 1 && bytes.readUInt32LE(12) === 0x60502 && bytes.readUInt32LE(16) === 65536 && bytes.readUInt32LE(20) === 730, 'Comparison input header');
  const weights = bytes.subarray(24, 24 + 65536 * 4), expansions = bytes.subarray(24 + 65536 * 4);
  check(sha(weights) === '42ef9f244f091136a6a94ddf747b3d13e4cb1bf7b8367956abcdf0bb42affba0' && sha(expansions) === '9d68be72650078da8ea7e72785b37abc8ffaaa87fcdd4f8ae507d0dcf258aa3e', 'Complete active native input digests');
  let markers = 0;
  for (let unit = 0; unit < 65536; unit++) {
    const value = weights.readUInt32LE(unit * 4);
    if (((value >>> 8) & 255) === 2) { check((value >>> 16) < 730, 'Expansion marker escaped native order'); markers++; }
  }
  return { schemaVersion: 1, characterWeights: 65536, orderedExpansions: 730, expansionMarkers: markers, completeActiveWeightSha256: sha(weights), completeActiveExpansionSha256: sha(expansions), productResourceSha256: sha(bytes), scope: 'Exact fixed-profile engine inputs only. Full direct comparison/Jamo/wrapper/CRT/ANSI/state/document/PC/physical remain open.' };
}
module.exports = { verify };
if (require.main === module) process.stdout.write(JSON.stringify(verify(), null, 2) + '\n');
