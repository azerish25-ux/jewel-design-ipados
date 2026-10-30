const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),zlib=require('node:zlib');
const {parse}=require('../reference/file-proof/csg-v4.cjs');
const {readSelectRails}=require('../reference/file-proof/select-ring-rail-v1.cjs');
const bytes=fs.readFileSync(path.join(__dirname,'../reference/fixtures/library-item-4.csg'));
function crc(data,node){data.writeUInt32LE(zlib.crc32(data.subarray(node.payload,node.crcOffset)),node.crcOffset);}
test('stored selector1.0 fully consumes three source input links',()=>{const rails=readSelectRails(bytes);assert.equal(rails.length,1);assert.equal(rails[0].index,16);assert.deepEqual(rails[0].objects,[4,13,15]);assert.deepEqual(rails[0].channels,[0,0,0]);assert.equal(rails[0].fullyConsumed,true);});
test('selector future version stays unrecognized and malformed known bounds fail',()=>{const node=parse(bytes).nodes[16],future=Buffer.from(bytes);future.writeInt32LE(99,node.payload);crc(future,node);assert.equal(readSelectRails(future).length,0);const malformed=Buffer.from(bytes);malformed.writeInt32LE(0x7fffffff,node.payload+8);crc(malformed,node);assert.throws(()=>readSelectRails(malformed),/bound/);});
test('selector link arrays require matching complete body',()=>{const node=parse(bytes).nodes[16],malformed=Buffer.from(bytes);malformed.writeInt32LE(2,node.crcOffset-16);crc(malformed,node);assert.throws(()=>readSelectRails(malformed),/mismatched/);});
