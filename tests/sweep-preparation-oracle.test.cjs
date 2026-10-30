const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const {build,verify}=require('../scripts/verify-sweep-preparation-oracle.cjs');
test('exact recovered explicit sweep clamp and stable ordering remain intact',()=>assert.equal(verify(fs.readFileSync(path.join(__dirname,'../',build().target),'utf8')),true));
test('sweep preparation tamper rejected',()=>assert.throws(()=>verify(build().code.replace('int[] array3','long[] array3')),/differ/));
