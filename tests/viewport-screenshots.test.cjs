const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const {decodePng}=require('../scripts/verify-metal-artifacts.cjs');
const {verifyPixels}=require('../scripts/verify-viewport-screenshots.cjs');
const folder=path.resolve(__dirname,'../docs/evidence/apple/metal-viewport');
function frames(){const read=name=>decodePng(fs.readFileSync(path.join(folder,name+'.png')));return {original:read('original'),orbit:read('orbit'),edited:read('edited'),empty:read('empty'),resized:read('orbit'),final:read('original')};}
// Prior actual CAD PNGs exercise comparison logic only, not an on-screen/runtime claim.
test('screen comparison detects real CAD changes and reproducible final output',()=>{const p=verifyPixels(frames());assert.ok(p.foregroundPixels>100&&p.editedPixels>100&&p.orbitPixels>100&&p.resizedPixels>100);assert.equal(p.finalDifferencePixels,0);});
test('screen comparison rejects blank, stale, nonempty-clear and incorrect-final scenes',()=>{
 for(const change of [f=>f.original=f.empty,f=>f.edited=f.original,f=>f.orbit=f.original,f=>f.resized=f.original,f=>f.empty=f.edited,f=>f.final=f.orbit]){const f=frames();change(f);assert.throws(()=>verifyPixels(f));}
});
