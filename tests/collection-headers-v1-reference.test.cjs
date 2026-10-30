const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),zlib=require('node:zlib');
const {parse}=require('../reference/file-proof/csg-v4.cjs');const {readCollections}=require('../reference/file-proof/collection-headers-v1.cjs');
const bytes=fs.readFileSync(path.join(__dirname,'../reference/fixtures/library-item-4.csg'));
function crc(b,n){b.writeUInt32LE(zlib.crc32(b.subarray(n.payload,n.crcOffset)),n.crcOffset);}
test('13 Array and7 Unarray exact1.0 headers fully consumed',()=>{const rows=readCollections(bytes);assert.equal(rows.length,20);assert.equal(rows.filter(x=>x.type==='CSArray').length,13);assert.equal(rows.filter(x=>x.type==='CSUnarray1').length,7);assert.ok(rows.every(x=>x.fullyConsumed));});
test('future collection versions are not guessed',()=>{for(const index of [61,116]){const n=parse(bytes).nodes[index],b=Buffer.from(bytes);b.writeInt32LE(99,n.payload);crc(b,n);assert.equal(readCollections(b).length,19);}});
test('known collection bounded header corruption rejected',()=>{const n=parse(bytes).nodes[116],b=Buffer.from(bytes);b.writeInt32LE(0x7fffffff,n.payload+8);crc(b,n);assert.throws(()=>readCollections(b),/bound/);});
