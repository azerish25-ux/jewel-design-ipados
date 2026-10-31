const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');const {verify,target}=require('../scripts/verify-legacy-empty-join.cjs');const report=()=>JSON.parse(fs.readFileSync(path.join(__dirname,'..',target),'utf8'));
test('Locked original SDK5 empty branch proves null without executing native code',()=>assert.equal(verify(report()),true));
test('Inverted native zero-count branch cannot pass proof',()=>{const r=report(),b=Buffer.from(r.emptyCase.bytesHex,'hex');b[37]=0x74;r.emptyCase.bytesHex=b.toString('hex');assert.throws(()=>verify(r),/differs/);});
test('Wrong native array Count field cannot pass proof',()=>{const r=report();r.emptyCase.arrayCountFieldOffset=20;assert.throws(()=>verify(r),/differs/);});
