const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');const {build,verify,target}=require('../scripts/verify-legacy-join-contracts.cjs');
test('Actual140 binds legacy OnBrep overload and repair/null behavior',()=>assert.equal(verify(JSON.parse(fs.readFileSync(path.join(__dirname,'..',target),'utf8'))),true));
test('Modern filtered overload cannot be substituted for legacy140',()=>{const r=build();r.selectedOverload='IEnumerable<Brep> modern';assert.throws(()=>verify(r),/differs/);});
test('Legacy null-output retention rule tamper rejected',()=>{const r=build();r.sourcePolicy.nullResult='Replace output with empty';assert.throws(()=>verify(r),/differs/);});
