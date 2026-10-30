const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const {build,verify}=require('../scripts/verify-collection-execution-oracle.cjs');
test('Array/Unarray Update bodies match canonical source',()=>assert.equal(verify(fs.readFileSync(path.join(__dirname,'../',build().target),'utf8')),true));
test('all-output wildcard tamper rejected',()=>assert.throws(()=>verify(build().code.replace('OutputChannelIndex == -1','OutputChannelIndex < -1')),/differ/));
test('Unarray one-level expansion tamper rejected',()=>assert.throws(()=>verify(build().code.replace('data2.GetType() == typeof(CSDataArray)','data2.GetType() != typeof(CSDataArray)')),/differ/));
test('source trailing empty array tamper rejected',()=>assert.throws(()=>verify(build().code.replace('SetData(data);','SetData(data2);')),/differ/));
