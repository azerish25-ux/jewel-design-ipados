const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const {build,verify,target}=require('../scripts/verify-brep-data-contracts.cjs');
test('BRep source conversion, transform and bounds contracts match canonical bodies',()=>assert.equal(verify(JSON.parse(fs.readFileSync(path.join(__dirname,'..',target),'utf8'))),true));
test('BRep method provenance tampering is rejected',()=>{const report=build();report.methods[0].bodySha256='0'.repeat(64);assert.throws(()=>verify(report),/differs/);});
test('Fast/accurate source policy tampering is rejected',()=>{const report=build();report.observations.oldTightBrepBox='doAccurate:true';assert.throws(()=>verify(report),/differs/);});
