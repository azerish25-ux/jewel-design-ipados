const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const {cases,verify}=require('../scripts/generate-frustum-cases.cjs');
const fixture=JSON.parse(fs.readFileSync(path.resolve(__dirname,'../tests/Port/Geometry.Candidate.Contracts/Fixtures/frustum-source-cases.json'),'utf8')).cases;
test('frustum reference fixture covers current deterministic inputs',()=>{assert.equal(fixture.length,1245);assert.deepEqual(fixture.map(({expected,...input})=>input),cases());});
test('frustum reference comparison rejects forged return/numeric/signed-zero outcomes',()=>{
 const badReturn=structuredClone(fixture);badReturn[0].expected.ok=!badReturn[0].expected.ok;assert.throws(()=>verify(fixture,badReturn));
 const badValue=structuredClone(fixture);badValue[0].expected.scalar='999';assert.throws(()=>verify(fixture,badValue));
 const negative=fixture.findIndex(x=>x.expected.bounds.includes('-0'));assert.ok(negative>=0);const zero=structuredClone(fixture);zero[negative].expected.bounds=zero[negative].expected.bounds.map(x=>x==='-0'?'0':x);assert.throws(()=>verify(fixture,zero));
});
