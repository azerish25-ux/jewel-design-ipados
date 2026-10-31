const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const {pack,validate,restore}=require('../scripts/native-archive.cjs');
const root=path.resolve(__dirname,'..');
function fixture(){
 fs.mkdirSync(path.join(root,'.local'),{recursive:true});
 const dir=fs.mkdtempSync(path.join(root,'.local/native-archive-test-'));
 const bytes=Buffer.from('!<arch>\nreal archive content is tested on Apple; this is an integrity fixture');
 const identity={runId:'12345',sourceCommit:'a'.repeat(40),platform:'iossimulator-arm64',nativeInputSha256:'b'.repeat(64),xcode:'Xcode 26.6\nBuild version pinned'};
 return {dir,bytes,identity,manifest:pack(bytes,identity)};
}
test('matching archive validates and restores to fixed simulator path',()=>{
 const f=fixture();assert.doesNotThrow(()=>validate(f.bytes,f.manifest,f.identity));
 restore(f.dir,f.bytes,f.manifest,f.identity);
 assert.deepEqual(fs.readFileSync(path.join(f.dir,'.local/native/ios-sim-all.a')),f.bytes);
 assert.equal(fs.existsSync(path.join(f.dir,'.local/native/ios-device-all.a')),false);
});
test('reject mismatched producer/platform/native inputs/toolchain',()=>{
 const f=fixture();
 for(const [key,value] of Object.entries({runId:'67890',platform:'ios-arm64',nativeInputSha256:'c'.repeat(64),xcode:'Xcode different'}))
  assert.throws(()=>validate(f.bytes,f.manifest,{...f.identity,[key]:value}),new RegExp(key));
});
test('reject malformed/tampered archive and manifest provenance',()=>{
 const f=fixture();assert.throws(()=>validate(Buffer.concat([f.bytes,Buffer.from('x')]),f.manifest,f.identity),/archive/);
 assert.throws(()=>validate(Buffer.from('not an archive'),f.manifest,f.identity),/archive/);
 for(const [key,value] of Object.entries({schemaVersion:2,occtPin:'0'.repeat(40),sourceCommit:'invalid',archiveSha256:'0'.repeat(64)}))
  assert.throws(()=>validate(f.bytes,{...f.manifest,[key]:value},f.identity),new RegExp(key));
 assert.throws(()=>pack(f.bytes,{...f.identity,runId:'../other'}),/runId/);
 assert.throws(()=>pack(f.bytes,{...f.identity,platform:'../../escape'}),/platform/);
});

test('device archive restores separately; validation failure writes nothing',()=>{
 const f=fixture(),identity={...f.identity,platform:'ios-arm64'},manifest=pack(f.bytes,identity);
 restore(f.dir,f.bytes,manifest,identity);
 assert.deepEqual(fs.readFileSync(path.join(f.dir,'.local/native/ios-device-all.a')),f.bytes);
 assert.equal(fs.existsSync(path.join(f.dir,'.local/native/ios-sim-all.a')),false);
 const bad=fixture();assert.throws(()=>restore(bad.dir,bad.bytes,bad.manifest,{...bad.identity,runId:'999'}),/runId/);
 assert.equal(fs.existsSync(path.join(bad.dir,'.local/native')),false);
});
test('both producer workflows retain native archive before app build and gate retests',()=>{
 for(const [file,platform,build] of [['native-geometry.yml','iossimulator-arm64','Build signed native-execution test app'],['native-device.yml','ios-arm64','Build unsigned native device AOT test app']]){
  const w=fs.readFileSync(path.join(root,'.github/workflows',file),'utf8');
  assert.ok(w.indexOf('Retain validated native archive before app compilation')<w.indexOf(build));
  assert.ok(w.includes('name: native-kernel-'+platform));
  assert.ok(w.includes('node scripts/native-archive.cjs restore '+platform));
  assert.match(w,/native_archive_run:/);assert.match(w,/actions: read/);
  assert.ok(w.includes('!artifacts/native-kernel-*/**'));
 }
});
