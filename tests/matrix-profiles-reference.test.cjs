const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),zlib=require('node:zlib');
const {parse}=require('../reference/file-proof/csg-v4.cjs');
const {readMatrixProfiles}=require('../reference/file-proof/matrix-profiles-v1.cjs');
const bytes=fs.readFileSync(path.join(__dirname,'../reference/fixtures/library-item-4.csg'));
function crc(data,node){data.writeUInt32LE(zlib.crc32(data.subarray(node.payload,node.crcOffset)),node.crcOffset);}
test('decodes six groups and the five-curve outside-rail subset with homogeneous controls',()=>{
 const report=readMatrixProfiles(bytes);assert.equal(report.groups.reduce((n,g)=>n+g.curves.length,0),155);assert.equal(report.groups.reduce((n,g)=>n+g.curves.reduce((m,c)=>m+c.controlVertexCount,0),0),4012);assert.equal(report.groups.length,6);const g=report.groups[0];
 assert.equal(g.index,7);assert.equal(g.fullyConsumed,true);assert.deepEqual(g.curves.map(x=>x.controlVertexCount),[26,36,38,21,35]);
 assert.equal(g.curves.filter(x=>x.rational).length,2);assert.equal(g.curves.reduce((n,c)=>n+c.controlVertexCount,0),156);
 assert.equal(g.names.length,5);assert.equal(g.sweepBits.length,5);
 for(const c of g.curves){assert.equal(c.order,4);assert.deepEqual(c.domain,[0,1]);assert.equal(c.controlBits.length,c.controlVertexCount*(c.dimension+(c.rational?1:0)));}
});
test('unknown outer or nested version remains unclaimed; malformed counts fail',()=>{
 const node=parse(bytes).nodes[7],g=readMatrixProfiles(bytes).groups[0],future=Buffer.from(bytes);
 future.writeInt32LE(99,node.payload);crc(future,node);assert.equal(readMatrixProfiles(future).groups.some(g=>g.index===7),false);
 const nested=Buffer.from(bytes);nested[g.curves[0].start]=0x11;crc(nested,node);assert.equal(readMatrixProfiles(nested).groups.some(g=>g.index===7),false);
 const invalid=Buffer.from(bytes);invalid.writeInt32LE(0x7fffffff,g.curves[0].knotCountOffset);crc(invalid,node);assert.throws(()=>readMatrixProfiles(invalid),/bound|count/);
 const mismatch=Buffer.from(bytes);mismatch.writeInt32LE(25,g.curves[0].controlCountOffset);crc(mismatch,node);assert.throws(()=>readMatrixProfiles(mismatch),/control count/);
});
