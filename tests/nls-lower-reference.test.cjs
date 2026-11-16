'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const { verify } = require('../scripts/verify-nls-lower-reference.cjs');
test('complete public Windows NLS1 lower corpus and portable resource match native output digests', () => {
  const report = verify();
  assert.equal(report.unitScalarCases, 1114112);
  assert.equal(report.nativeContexts, 3329);
  assert.equal(report.changedMappings, 1013);
  assert.equal(report.portableInvariantDifferences, 419);
});
test('native corpus and production mapping reject altered bytes', () => {
  const fixture = fs.readFileSync(path.join(__dirname, '../reference/fixtures/windows-nls1-lower-60502.json.br'));
  const table = fs.readFileSync(path.join(__dirname, '../src/Port/Rendering.Candidate/Fixtures/windows-nls1-lower-60502.bin'));
  const changedFixture = Buffer.from(fixture), changedTable = Buffer.from(table);
  changedFixture[100] ^= 1; changedTable[100] ^= 1;
  assert.throws(() => verify(changedFixture, table), /fixture digest/);
  assert.throws(() => verify(fixture, changedTable), /Product mapping digest/);
});
