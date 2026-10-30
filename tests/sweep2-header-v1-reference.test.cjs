const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),zlib=require('node:zlib');
const {parse}=require('../reference/file-proof/csg-v4.cjs'),{readSweep2}=require('../reference/file-proof/sweep2-header-v1.cjs');
const bytes=fs.readFileSync(path.join(__dirname,'../reference/fixtures/library-item-4.csg'));
function crc(b,n){b.writeUInt32LE(zlib.crc32(b.subarray(n.payload,n.crcOffset)),n.crcOffset);}
test('original134 exact1.0 sweep header fully consumed with seven saved links',()=>{const rows=readSweep2(bytes);assert.equal(rows.length,1);assert.equal(rows[0].index,134);assert.deepEqual(rows[0].objects,[4,13,131,132,133,129,130]);assert.deepEqual(rows[0].channels,[0,0,0,0,0,0,0]);assert.equal(rows[0].fullyConsumed,true);});
test('future sweep versions remain opaque',()=>{const n=parse(bytes).nodes[134],b=Buffer.from(bytes);b.writeInt32LE(99,n.payload);crc(b,n);assert.equal(readSweep2(b).length,0);});
test('known sweep corrupt bounded header rejected',()=>{const n=parse(bytes).nodes[134],b=Buffer.from(bytes);b.writeInt32LE(0x7fffffff,n.payload+8);crc(b,n);assert.throws(()=>readSweep2(b),/bound/);});
