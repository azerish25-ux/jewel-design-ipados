'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const { verify, names } = require('../scripts/verify-nls-sort-reference.cjs');
test('complete native sort streams, actual repeated comparisons and published ordinary weights remain bound', () => {
  const report = verify();
  assert.equal(report.unitScalarKeys, 1114112);
  assert.equal(report.fullStringKeys, 15547);
  assert.equal(report.repeatedNativeComparisons, 40663);
  assert.equal(report.rejectedSortKeyComparisonCases, 213);
  assert.equal(report.rejectedNaiveCompositionCases, 9407);
  assert.equal(report.ordinaryPublishedNativeKeyMatches, 6029);
});
test('altered native key, native comparison or published weight artifact is rejected', () => {
  const packed = names.map(name => fs.readFileSync(path.join(__dirname, '../reference/fixtures', name)));
  for (let i = 0; i < packed.length; i++) {
    const altered = packed.map(bytes => Buffer.from(bytes));
    altered[i][100] ^= 1;
    assert.throws(() => verify(altered), /packed artifact digest/);
  }
});
