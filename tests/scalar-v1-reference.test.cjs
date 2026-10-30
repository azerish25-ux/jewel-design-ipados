const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),zlib=require('node:zlib');
const {parse}=require('../reference/file-proof/csg-v4.cjs');
const {readAdditionalScalars}=require('../reference/file-proof/scalar-v1.cjs');
const fixture=fs.readFileSync(path.resolve(__dirname,'../reference/fixtures/library-item-4.csg'));
function crc(bytes,node){bytes.writeUInt32LE(zlib.crc32(bytes.subarray(node.payload,node.crcOffset)),node.crcOffset);}
test('real source-defined scalar bodies are fully consumed without a Boolean encoding',()=>{
 const result=readAdditionalScalars(fixture);assert.equal(result.existingNumericCount,74);assert.equal(result.extras.length,26);
 const counts={};for(const item of result.extras){counts[item.type]=(counts[item.type]||0)+1;assert.equal(item.major,1);assert.equal(item.minor,1);assert.equal(item.fullyConsumed,true);assert.equal(item.valueBits.length,16);}
 assert.deepEqual(counts,{'CS.CSValues.CSValueChooser':7,'CS.CSValues.CSValueOnOff':15,'CS.CSValues.CSValue':4});
});
test('future scalar versions remain undecoded and corrupt CRC is rejected',()=>{
 const selected=readAdditionalScalars(fixture).extras[0],node=parse(fixture).nodes[selected.index];
 const future=Buffer.from(fixture);future.writeInt32LE(99,node.payload);crc(future,node);assert.equal(readAdditionalScalars(future).extras.length,25);
 const corrupt=Buffer.from(fixture);corrupt[node.payload+10]^=1;assert.throws(()=>readAdditionalScalars(corrupt),/CRC mismatch/);
});
test('scalar strings and paired link arrays are bounded',()=>{
 const selected=readAdditionalScalars(fixture).extras[0],node=parse(fixture).nodes[selected.index];
 const text=Buffer.from(fixture);text.writeInt32LE(0x7fffffff,node.payload+8);crc(text,node);assert.throws(()=>readAdditionalScalars(text),/bound/);
 const links=Buffer.from(fixture);links.writeInt32LE(selected.channels.length+1,selected.channelCountOffset);crc(links,node);assert.throws(()=>readAdditionalScalars(links),/mismatch/);
});
test('raw nonfinite and negative-zero bits survive reference inspection',()=>{
 const selected=readAdditionalScalars(fixture).extras[0],node=parse(fixture).nodes[selected.index];
 for(const [bits,kind]of [[0x7ff8000000000042n,'nan'],[0x8000000000000000n,'negative-zero'],[0x7ff0000000000000n,'positive-infinity']]){
  const bytes=Buffer.from(fixture);bytes.writeBigUInt64LE(bits,selected.valueOffset);crc(bytes,node);const actual=readAdditionalScalars(bytes).extras.find(x=>x.index===selected.index);
  assert.equal(actual.valueBits,bits.toString(16).padStart(16,'0'));assert.equal(actual.valueKind,kind);
 }
});
