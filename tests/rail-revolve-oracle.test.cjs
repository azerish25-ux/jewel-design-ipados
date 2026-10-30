const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const {build,verify}=require('../scripts/verify-rail-revolve-oracle.cjs');
test('Rail-revolve oracle preserves recovered tensor math',()=>assert.equal(verify(fs.readFileSync(path.join(__dirname,'..',build().target),'utf8')),true));
test('Rail-revolve weight product tamper rejected',()=>{const code=build().code;const changed=code.replace('onNurbsCurve.Weight(i) * onNurbsCurve2.Weight(j)','onNurbsCurve.Weight(i) + onNurbsCurve2.Weight(j)');assert.notEqual(changed,code);assert.throws(()=>verify(changed),/differs/);});
test('Rail-revolve NaN policy tamper rejected',()=>{const code=build().code;const changed=code.replace('double.IsNaN(num4)','double.IsInfinity(num4)');assert.notEqual(changed,code);assert.throws(()=>verify(changed),/differs/);});
test('Rail-revolve UV routing tamper rejected',()=>{const code=build().code;const changed=code.replace('SetKnot(1 - direction','SetKnot(direction');assert.notEqual(changed,code);assert.throws(()=>verify(changed),/differs/);});
