const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),cp=require('node:child_process');
const root=path.resolve(__dirname,'..'),bash=process.platform==='win32'?'C:/Program Files/Git/bin/bash.exe':'bash';
// Protocol/control-flow fixtures do not execute or prove native/GPU behavior.
function scenario(name){
 fs.mkdirSync(path.join(root,'.local'),{recursive:true});const dir=fs.mkdtempSync(path.join(root,'.local/viewport-driver-test-'));
 for(const folder of ['scripts','data/Documents','artifacts/viewport-ipad','tests/iOS.Viewport/bin/Debug/Test.app'])fs.mkdirSync(path.join(dir,folder),{recursive:true});
 fs.copyFileSync(path.join(root,'scripts/ios-device.cjs'),path.join(dir,'scripts/ios-device.cjs'));
 fs.writeFileSync(path.join(dir,'tests/iOS.Viewport/bin/Debug/Test.app/Info.plist'),'fixture');
 fs.writeFileSync(path.join(dir,'data/Documents/viewport-result.txt'),'PASS visible Metal viewport:6 captured stages; stale');
 fs.writeFileSync(path.join(dir,'data/Documents/viewport-result.txt.family'),'ipad');
 fs.writeFileSync(path.join(dir,'artifacts/viewport-ipad/original.png'),'stale image');
 const mock=[
 'set -euo pipefail',
 'xcrun(){',
 ' case "$2" in',
 ' list) printf \'%s\' \'{"devices":{"com.apple.CoreSimulator.SimRuntime.iOS-26-5":[{"name":"iPad test","udid":"test-device","isAvailable":true}]}}\';;',
 ' boot|bootstatus|install) return 0;;',
 ' get_app_container) printf \'%s/data\\n\' "$PWD";;',
 ' launch) if [[ "$SCENARIO" == launch-failure ]]; then return 18; fi; if [[ "$SCENARIO" == invalid-stage ]]; then printf \'%s\' "00000000000000000000000000000000|../../escape" > data/Documents/viewport-stage.txt; fi; if [[ "$SCENARIO" == screenshot-failure ]]; then printf \'%s\' "00000000000000000000000000000000|original" > data/Documents/viewport-stage.txt; fi;;',
 ' io) echo unexpected-screenshot >> unexpected.txt; return 19;;',
 ' shutdown) echo shutdown >> cleanup.txt;;',
 ' spawn) return 0;;',
 ' *) return 20;;',
 ' esac',
 '}',
 'sleep(){ return 0; }',
 'export -f xcrun sleep',
 'source "$SCRIPT" ipad'
 ].join('\n');
 const result=cp.spawnSync(bash,['--noprofile','--norc','-c',mock],{cwd:dir,encoding:'utf8',timeout:15000,env:{...process.env,SCENARIO:name,SCRIPT:path.join(root,'scripts/run-viewport-simulator.sh').replaceAll('\\','/'),MSYS_NO_PATHCONV:'1'}});
 return {dir,result};
}
test('visible driver preserves launch failure and shuts down the selected simulator',()=>{const {dir,result}=scenario('launch-failure');assert.ifError(result.error);assert.equal(result.status,18,result.stdout+result.stderr);assert.equal(fs.readFileSync(path.join(dir,'cleanup.txt'),'utf8').trim(),'shutdown');});
test('visible driver rejects untrusted stage paths before screenshot',()=>{const {dir,result}=scenario('invalid-stage');assert.ifError(result.error);assert.notEqual(result.status,0,result.stdout+result.stderr);assert.equal(fs.existsSync(path.join(dir,'unexpected.txt')),false);assert.equal(fs.existsSync(path.join(dir,'cleanup.txt')),true);});
test('visible driver rejects stale result/images when this launch produces nothing',()=>{const {dir,result}=scenario('stale');assert.ifError(result.error);assert.notEqual(result.status,0,result.stdout+result.stderr);assert.equal(fs.existsSync(path.join(dir,'data/Documents/viewport-result.txt')),false);assert.equal(fs.existsSync(path.join(dir,'artifacts/viewport-ipad/original.png')),false);assert.equal(fs.existsSync(path.join(dir,'cleanup.txt')),true);});

test('visible driver reaches a valid stage and preserves screenshot failure without acknowledgement',()=>{const {dir,result}=scenario('screenshot-failure');assert.ifError(result.error);assert.equal(result.status,19,result.stdout+result.stderr);assert.equal(fs.existsSync(path.join(dir,'unexpected.txt')),true);assert.equal(fs.existsSync(path.join(dir,'data/Documents/viewport-ack.txt')),false);assert.equal(fs.existsSync(path.join(dir,'cleanup.txt')),true);});
