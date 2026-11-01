'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs');
const {rows,mutateCases,parseRead,same}=require('../scripts/generate-userstring-body-cases.cjs');
const fixture=JSON.parse(fs.readFileSync('tests/Port/Geometry.Candidate.Contracts/Fixtures/userstring-body-source-cases.json','utf8'));
test('native user-string corpus preserves source input coverage and all malformed reader inputs',()=>{
 assert.deepEqual(rows(),fixture.writes.map(({version,pairs})=>({version,pairs})));
 assert.deepEqual(mutateCases(fixture.writes),fixture.cases.map(({expected,...rest})=>rest));
 assert.equal(fixture.writes.length,12);assert.equal(fixture.cases.length,1076);
});
test('native reader protocol rejects malformed returns, cursors, UTF16 and pair counts',()=>{
 assert.deepEqual(parseRead('R 1 2 30 1 6b00 -'),{ok:true,crcErrors:2,consumed:30,pairs:[['6b00','-']]});
 for(const bad of ['R 2 0 0 0','R 1 NaN 0 0','R 1 0 -1 0','R 1 0 1.5 0','R 1 0 0 1','R 1 0 0 1 61 -','R 1 0 0 1 zzzz -','R 1 0 9007199254740992 0'])assert.throws(()=>parseRead(bad));
});
test('native corpus equality rejects changed bool, CRC, cursor, values, framing and version',()=>{
 for(const field of ['ok','crcErrors','consumed','pairs']){const changed=structuredClone(fixture);changed.cases[0].expected[field]=null;assert.throws(()=>same(fixture,changed));}
 for(const field of ['version','hex','pairs']){const changed=structuredClone(fixture);changed.writes[0][field]=null;assert.throws(()=>same(fixture,changed));}
 assert.equal(same(fixture,structuredClone(fixture)),true);
});
