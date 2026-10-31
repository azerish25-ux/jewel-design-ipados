const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),zlib=require('node:zlib');
const {decodePng,verifyFrames}=require('../scripts/verify-metal-artifacts.cjs');
const root=path.resolve(__dirname,'..'),fixture=path.join(root,'docs/evidence/apple/metal-viewport');
test('real Metal PNG pixels reproduce independently recorded GPU readback metrics',()=>{
 const result=verifyFrames(fixture);assert.equal(result.foregroundPixels,16109);assert.equal(result.editedPixels,12377);assert.equal(result.orbitPixels,19835);assert.equal(result.originalBgraSha256,'047cb13264fcfaafc45204662c1fe3c07f6fab17f166e1417c1d190e18ded012');
});
test('reject corrupted PNG data and unsafe dimensions',()=>{
 const original=fs.readFileSync(path.join(fixture,'original.png')),bad=Buffer.from(original);bad[100]^=1;assert.throws(()=>decodePng(bad),/CRC/);
 const wide=Buffer.from(original);wide.writeUInt32BE(4097,16);wide.writeUInt32BE(zlib.crc32(wide.subarray(12,29)),29);assert.throws(()=>decodePng(wide),/dimensions/);
 assert.throws(()=>decodePng(Buffer.from('not png')),/signature/);
});
test('reject forged GPU hash/count reports for actual captured frames',()=>{
 fs.mkdirSync(path.join(root,'.local'),{recursive:true});const dir=fs.mkdtempSync(path.join(root,'.local/metal-artifact-test-'));
 for(const file of ['original.png','edited.png','orbit.png','empty.png'])fs.copyFileSync(path.join(fixture,file),path.join(dir,file));
 const report=JSON.parse(fs.readFileSync(path.join(fixture,'report.json'),'utf8'));
 for(const [field,value] of Object.entries({originalBgraSha256:'0'.repeat(64),foregroundPixels:1,editedPixels:1,orbitPixels:1,orderPixels:33,width:321})){
  fs.writeFileSync(path.join(dir,'report.json'),JSON.stringify({...report,[field]:value}));assert.throws(()=>verifyFrames(dir),new RegExp(field));
 }
});
