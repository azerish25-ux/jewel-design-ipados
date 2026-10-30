const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');const {build,verify,target}=require('../scripts/verify-cap-placement-contracts.cjs');
test('Cap placement source method hashes and policies match',()=>assert.equal(verify(JSON.parse(fs.readFileSync(path.join(__dirname,'..',target),'utf8'))),true));
test('Cap placement inverse ordering tamper rejected',()=>{const value=build();value.policy.inverse='inverse(basis * translation)';assert.throws(()=>verify(value),/differs/);});
test('Cap placement source provenance tamper rejected',()=>{const value=build();value.methods[0].bodySha256='0'.repeat(64);assert.throws(()=>verify(value),/differs/);});
