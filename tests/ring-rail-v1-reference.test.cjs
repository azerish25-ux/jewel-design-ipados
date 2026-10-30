const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),zlib=require('node:zlib');
const {parse}=require('../reference/file-proof/csg-v4.cjs');
const {readRingRails}=require('../reference/file-proof/ring-rail-v1.cjs');
const bytes=fs.readFileSync(path.join(__dirname,'../reference/fixtures/library-item-4.csg'));
function crc(data,node){data.writeUInt32LE(zlib.crc32(data.subarray(node.payload,node.crcOffset)),node.crcOffset);}
test('library RingRail_V1 header is completely source-defined and linked to stored radius',()=>{
 const rails=readRingRails(bytes);assert.equal(rails.length,1);const rail=rails[0];
 assert.equal(rail.index,4);assert.deepEqual(rail.objects,[2]);assert.deepEqual(rail.channels,[0]);assert.equal(rail.fullyConsumed,true);
 const radius=parse(bytes).nodes[rail.objects[0]];assert.equal(radius.guid,'dd5c2342-d89a-440e-a76e-624a1faf2b68');assert.equal(radius.major,1);assert.equal(radius.minor,2);assert.equal(radius.numeric.value,8.667196228921199);
});
test('future ring-rail version is skipped; known-schema string bounds enforced',()=>{
 const node=parse(bytes).nodes[4],future=Buffer.from(bytes);future.writeInt32LE(99,node.payload);crc(future,node);assert.equal(readRingRails(future).length,0);
 const invalid=Buffer.from(bytes);invalid.writeInt32LE(0x7fffffff,node.payload+8);crc(invalid,node);assert.throws(()=>readRingRails(invalid),/bound/);
});
