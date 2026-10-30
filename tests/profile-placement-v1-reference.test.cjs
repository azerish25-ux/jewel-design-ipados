const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),zlib=require('node:zlib');
const {parse}=require('../reference/file-proof/csg-v4.cjs');const {readProfilePlacements}=require('../reference/file-proof/profile-placement-v1.cjs');
const bytes=fs.readFileSync(path.join(__dirname,'../reference/fixtures/library-item-4.csg'));
function crc(b,n){b.writeUInt32LE(zlib.crc32(b.subarray(n.payload,n.crcOffset)),n.crcOffset);}
test('three source placement headers fully consume original input links',()=>{const blocks=readProfilePlacements(bytes);assert.deepEqual(blocks.map(x=>x.index),[110,117,124]);assert.deepEqual(blocks[0].objects,[4,13,13,94]);assert.deepEqual(blocks[1].channels,[0,0,1,2]);assert.ok(blocks.every(x=>x.fullyConsumed));});
test('future placement versions remain unknown',()=>{const n=parse(bytes).nodes[110],b=Buffer.from(bytes);b.writeInt32LE(99,n.payload);crc(b,n);assert.equal(readProfilePlacements(b).length,2);});
test('known placement malformed bounds/counts reject',()=>{const n=parse(bytes).nodes[110],b=Buffer.from(bytes);b.writeInt32LE(0x7fffffff,n.payload+8);crc(b,n);assert.throws(()=>readProfilePlacements(b),/bound/);const mismatch=Buffer.from(bytes);mismatch.writeInt32LE(3,n.crcOffset-20);crc(mismatch,n);assert.throws(()=>readProfilePlacements(mismatch),/mismatched/);});
