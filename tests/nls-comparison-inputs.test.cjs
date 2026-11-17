'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const { verify } = require('../scripts/verify-nls-comparison-inputs.cjs');
test('complete native packed character inputs and original expansion order remain exact', () => {
  const value = verify();
  assert.equal(value.characterWeights, 65536);
  assert.equal(value.orderedExpansions, 730);
  assert.ok(value.expansionMarkers > 0);
});
test('changed packed weight or reordered expansion is rejected', () => {
  const original = fs.readFileSync(path.join(__dirname, '../src/Port/Rendering.Candidate/Fixtures/windows-nls1-comparison-inputs-60502.bin'));
  for (const offset of [24 + 65 * 4, 24 + 65536 * 4 + 12]) {
    const changed = Buffer.from(original); changed[offset] ^= 1;
    assert.throws(() => verify(changed), /resource digest/);
  }
});
