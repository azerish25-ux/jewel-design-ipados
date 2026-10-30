const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const {build,verify}=require('../scripts/verify-array-query-oracle.cjs');
test('exact array transform/bounds/flatten/first-data source queries remain intact',()=>assert.equal(verify(fs.readFileSync(path.join(__dirname,'../',build().target),'utf8')),true));
test('first-item selection tamper rejected',()=>assert.throws(()=>verify(build().code.replace('iCSArray.GetArrayItem(0)','iCSArray.GetArrayItem(1)')),/differ/));
test('aggregate-bounds grow behavior tamper rejected',()=>assert.throws(()=>verify(build().code.replace('doGrow: false, transform','doGrow: true, transform')),/differ/));
test('nested array transform exclusion tamper rejected',()=>assert.throws(()=>verify(build().code.replace('data is CSDataArray','data is not CSDataArray')),/differ/));
