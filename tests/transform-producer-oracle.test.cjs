const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const {build,verify}=require('../scripts/verify-transform-producer-oracle.cjs');
test('three transform/scalar Add source Update bodies remain exact',()=>assert.equal(verify(fs.readFileSync(path.join(__dirname,'../',build().target),'utf8')),true));
test('translation mirror threshold tamper is rejected',()=>assert.throws(()=>verify(build().code.replace('value4 > 0.5','value4 >= 0.5')),/differ/));
test('scale extent floor tamper is rejected',()=>assert.throws(()=>verify(build().code.replace('num < 0.01','num <= 0.01')),/differ/));
test('scalar addition tamper is rejected',()=>assert.throws(()=>verify(build().code.replaceAll('value + value2','value - value2')),/differ/));
