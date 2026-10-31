const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const {build,verify}=require('../scripts/verify-camera-preset-oracle.cjs');
const root=path.resolve(__dirname,'..');
test('camera extraction matches immutable recovered body and enum',()=>{const r=build();assert.equal(verify(fs.readFileSync(path.join(root,r.target),'utf8')),true);});
test('camera source checker rejects changed pose and enum state',()=>{const r=build();assert.throws(()=>verify(r.code.replace('0.0, 0.0, 80.0','0.0, 0.0, 81.0')),/differs/);assert.throws(()=>verify(r.code.replace('CameraView.Top','CameraView.Front')),/differs/);});
