const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),zlib=require('node:zlib');
const {parse}=require('../reference/file-proof/csg-v4.cjs');
const {readOutsideRails}=require('../reference/file-proof/outside-rail-v1.cjs');
const bytes=fs.readFileSync(path.join(__dirname,'../reference/fixtures/library-item-4.csg'));
function crc(data,node){data.writeUInt32LE(zlib.crc32(data.subarray(node.payload,node.crcOffset)),node.crcOffset);}
test('record13 outside rail header has six source-ordered stored references',()=>{
 const rails=readOutsideRails(bytes);assert.equal(rails.length,1);const rail=rails[0];
 assert.equal(rail.index,13);assert.equal(rail.identifier,'OutsideRingRail');assert.equal(rail.fullyConsumed,true);
 assert.deepEqual(rail.objects,[2,6,8,9,10,11]);assert.deepEqual(rail.channels,[0,3,0,0,0,0]);
});
test('unknown version remains unclaimed; known-schema bounds and channel count reject',()=>{
 const node=parse(bytes).nodes[13],future=Buffer.from(bytes);future.writeInt32LE(99,node.payload);crc(future,node);assert.equal(readOutsideRails(future).length,0);
 const invalid=Buffer.from(bytes);invalid.writeInt32LE(0x7fffffff,node.payload+8);crc(invalid,node);assert.throws(()=>readOutsideRails(invalid),/bound/);
 const count=Buffer.from(bytes);count.writeInt32LE(5,node.crcOffset-28);crc(count,node);assert.throws(()=>readOutsideRails(count),/six|consumed/);
});
