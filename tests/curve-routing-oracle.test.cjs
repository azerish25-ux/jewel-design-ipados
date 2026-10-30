const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const {build,verify}=require('../scripts/verify-curve-routing-oracle.cjs');
test('exact source property, direct method and flatten helper remain distinct',()=>assert.equal(verify(fs.readFileSync(path.join(__dirname,'../',build().target),'utf8')),true));
test('curve property selection tamper rejected',()=>assert.throws(()=>verify(build().code.replace('iCSArray.GetArrayItem(0)','iCSArray.GetArrayItem(1)')),/differ/));
