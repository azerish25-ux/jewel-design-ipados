const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');const {build,verify}=require('../scripts/verify-dome-profile-oracle.cjs');
test('Rhino5 dome oracle preserves exact recovered body',()=>assert.equal(verify(fs.readFileSync(path.join(__dirname,'..',build().target),'utf8')),true));
test('Dome threshold tamper rejected',()=>assert.throws(()=>verify(build().code.replace('Math.Abs(domeAmount) < 0.005','Math.Abs(domeAmount) < 0.006')),/differs/));
test('Dome weight tamper rejected',()=>assert.throws(()=>verify(build().code.replace('0.70710678','0.7071')),/differs/));
