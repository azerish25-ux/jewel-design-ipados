const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');const {build,verify}=require('../scripts/verify-average-plane-oracle.cjs');
test('Average-plane fallback body matches recovered source',()=>assert.equal(verify(fs.readFileSync(path.join(__dirname,'..',build().target),'utf8')),true));
test('Average-plane dot guard tamper rejected',()=>{const c=build().code,m=c.replace('num2 < 0.01','num2 < 0.02');assert.notEqual(c,m);assert.throws(()=>verify(m),/differs/);});
test('Average-plane length sample tamper rejected',()=>{const c=build().code,m=c.replace('0.25, 0.33','0.25, 0.34');assert.notEqual(c,m);assert.throws(()=>verify(m),/differs/);});
