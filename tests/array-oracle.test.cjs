const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const {build,verify}=require('../scripts/verify-array-oracle.cjs');
test('array copy/flatten/helper/arithmetic bodies match canonical source',()=>assert.equal(verify(fs.readFileSync(path.join(__dirname,'../',build().target),'utf8')),true));
test('array arithmetic operation tamper rejected',()=>assert.throws(()=>verify(build().code.replaceAll('value + value2','value - value2')),/differ/));
test('null-to-empty copy behavior tamper rejected',()=>assert.throws(()=>verify(build().code.replace('(item == null) ? new CSData() : item.Duplicate()','(item == null) ? null : item.Duplicate()')),/differ/));
test('flatten leaf order tamper rejected',()=>assert.throws(()=>verify(build().code.replace('Array.ToList()','Array.AsEnumerable().Reverse().ToList()')),/differ/));
