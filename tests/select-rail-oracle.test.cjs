const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const {build,verify}=require('../scripts/verify-select-rail-oracle.cjs');
test('selector oracle preserves exact source Update',()=>assert.equal(verify(fs.readFileSync(path.join(__dirname,'../',build().target),'utf8')),true));
test('selector reversed-choice tamper is rejected',()=>assert.throws(()=>verify(build().code.replace('value ? data2 : data','value ? data : data2')),/differs/));
test('selector null versus empty tamper is rejected',()=>assert.throws(()=>verify(build().code.replace('data == null && data2 == null','data == null || data2 == null')),/differs/));
