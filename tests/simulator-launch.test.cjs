const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const cp=require('node:child_process');
const root=path.resolve(__dirname,'..');
const bash=process.platform==='win32'?'C:/Program Files/Git/bin/bash.exe':'bash';
// These fixtures simulate simctl scheduling/file publication, never iOS or geometry.
// Every fixture, mock output and shell temp stays beneath the repository.
function scenario(name){
 fs.mkdirSync(path.join(root,'.local'),{recursive:true});
 const dir=fs.mkdtempSync(path.join(root,'.local/simulator-regression-'));
 fs.mkdirSync(path.join(dir,'artifacts'));
 fs.mkdirSync(path.join(dir,'data/Documents'),{recursive:true});
 fs.mkdirSync(path.join(dir,'tests/iOS/bin/Debug/Smoke.app'),{recursive:true});
 fs.writeFileSync(path.join(dir,'tests/iOS/bin/Debug/Smoke.app/Info.plist'),'fixture');
 fs.mkdirSync(path.join(dir,'scripts'));
 fs.copyFileSync(path.join(root,'scripts/ios-device.cjs'),path.join(dir,'scripts/ios-device.cjs'));
 if(name==='stale'){
  fs.writeFileSync(path.join(dir,'data/Documents/reuse-result.txt'),'PASS 16 stale report');
  fs.writeFileSync(path.join(dir,'data/Documents/reuse-result.txt.family'),'ipad');
 }
 const script=path.join(root,'scripts/run-simulator.sh').replaceAll('\\','/');
 const mock=[
  'set -euo pipefail',
  'COUNT=0',
  'xcrun(){',
  ' case "$2" in',
  '  list) printf \'%s\' \'{"devices":{"com.apple.CoreSimulator.SimRuntime.iOS-26-5":[{"name":"iPad test","udid":"test-device","isAvailable":true}]}}\';;',
  '  boot|bootstatus|install|terminate) return 0;;',
  '  get_app_container) printf \'%s/data\\n\' "$PWD";;',
  '  launch)',
  '   if [[ "$*" == *--console-pty* ]]; then echo "Console launch not supported in regression" >&2; return 17; fi',
  '   if [[ "$SCENARIO" == launch-failure ]]; then return 18; fi',
  '   return 0;;',
  '  spawn) return 0;;',
  '  *) echo "Unexpected simctl call: $*" >&2; return 19;;',
  ' esac',
  '}',
  'sleep(){',
  ' COUNT=$((COUNT+1))',
  ' if [[ "$SCENARIO" != stale && "$COUNT" == 75 ]]; then',
  '  printf \'%s\' ipad > data/Documents/reuse-result.txt.family',
  '  if [[ "$SCENARIO" == wrong-family ]]; then printf \'%s\' iphone > data/Documents/reuse-result.txt.family; fi',
  '  if [[ "$SCENARIO" == failed-check ]]; then printf \'%s\' "FAIL contract assertion" > data/Documents/reuse-result.txt',
  '  else printf \'%s\' "PASS 16 fresh report" > data/Documents/reuse-result.txt; fi',
  ' fi',
  '}',
  'export -f xcrun sleep',
  'source "$SCRIPT"',
 ].join('\n');
 return cp.spawnSync(bash,['--noprofile','--norc','-c',mock],{
  cwd:dir,encoding:'utf8',timeout:15000,
  env:{...process.env,SCENARIO:name,SCRIPT:script,SIMULATOR_FAMILY:'ipad',MSYS_NO_PATHCONV:'1'},
 });
}
test('waits for delayed fresh launch beyond the previous 60 polls',()=>{
 const r=scenario('delayed');assert.ifError(r.error);assert.equal(r.status,0,r.stdout+r.stderr);assert.match(r.stdout,/PASS 16 fresh report/);
});
test('rejects a stale passing report when this launch produces no result',()=>{
 const r=scenario('stale');assert.ifError(r.error);assert.notEqual(r.status,0,r.stdout+r.stderr);
});
test('rejects actual family mismatch',()=>{const r=scenario('wrong-family');assert.ifError(r.error);assert.notEqual(r.status,0);});
test('rejects a fresh failing check',()=>{const r=scenario('failed-check');assert.ifError(r.error);assert.notEqual(r.status,0);});
test('propagates simctl launch failure',()=>{const r=scenario('launch-failure');assert.ifError(r.error);assert.notEqual(r.status,0);});
