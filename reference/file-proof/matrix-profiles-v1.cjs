const fs=require('fs'),path=require('path'),crypto=require('crypto');
const {parse}=require('./csg-v4.cjs');
const {ScalarCursor}=require('./scalar-v1.cjs');
const GUID='98415b72-dadb-4b9b-94a9-1e6c5865b5f2';
class UnsupportedProfile extends Error{}
function readMatrixProfiles(bytes){
 if(!Buffer.isBuffer(bytes)||bytes.length>64*1024*1024)throw Error('Matrix profile byte bound');
 const graph=parse(bytes),groups=[];
 for(const node of graph.nodes){
  if(node.guid!==GUID||node.major!==1||![0,1].includes(node.minor))continue;
  const c=new ScalarCursor(bytes,node.payload+8,node.crcOffset);
  const identifier=c.text(),flags=c.i()>>>0,legacyLayer=c.i(),userData=c.userData(),objects=c.ints(),channels=c.ints();
  if(objects.length!==channels.length)throw Error('Profile link count mismatch');
  function doubles(n){
   if(!Number.isSafeInteger(n)||n<0)throw Error('Profile array count invalid');
   c.need(n*8);const bits=[],values=[];
   for(let i=0;i<n;i++){bits.push(bytes.readBigUInt64LE(c.p).toString(16).padStart(16,'0'));values.push(bytes.readDoubleLE(c.p));c.p+=8;}
   return {bits,values};
  }
  try{
   const curveCount=c.count(10000),curves=[];
   for(let i=0;i<curveCount;i++){
    const start=c.p;c.need(1);const packedVersion=bytes[c.p++];
    if(packedVersion!==0x10)throw new UnsupportedProfile('Unknown nested curve version');
    const dimension=c.count(10000),rat=c.i(),order=c.count(10000),controlVertexCount=c.count(10000),reserved=[c.i(),c.i()];
    if(dimension<1||![0,1].includes(rat)||order<2||controlVertexCount<order)throw Error('Invalid profile NURBS header');
    const bbox=doubles(6),knotCountOffset=c.p,knotCount=c.count(10000);
    if(knotCount!==order+controlVertexCount-2)throw Error('Profile knot count mismatch');
    const knots=doubles(knotCount),controlCountOffset=c.p,storedCount=c.count(10000);
    if(storedCount!==controlVertexCount)throw Error('Profile control count mismatch');
    const controlOffset=c.p,controls=doubles(storedCount*(dimension+rat));
    curves.push({index:i,start,end:c.p,packedVersion,dimension,rational:rat===1,order,controlVertexCount,reserved,
     knotCountOffset,controlCountOffset,controlOffset,knotBits:knots.bits,controlBits:controls.bits,boundingBoxBits:bbox.bits,
     domain:[knots.values[order-2],knots.values[controlVertexCount-1]],rawSha256:crypto.createHash('sha256').update(bytes.subarray(start,c.p)).digest('hex')});
   }
   const sweepCount=c.count(10000),sweepBits=[];for(let i=0;i<sweepCount;i++)sweepBits.push(doubles(c.count(10000)).bits);
   const namesOffset=c.p,names=[];if(node.minor>=1){const count=c.count(10000);for(let i=0;i<count;i++)names.push(c.text());}
   if(c.p!==node.crcOffset)throw Error('Matrix profile body not fully consumed');
   groups.push({index:node.index,guid:node.guid,major:node.major,minor:node.minor,identifier,flags,legacyLayer,userData,objects,channels,curves,sweepBits,namesOffset,names,fullyConsumed:true});
  }catch(error){if(!(error instanceof UnsupportedProfile))throw error;}
 }
 return {fixtureSha256:graph.sha256,groups};
}
if(require.main===module){
 if(!process.argv.includes('--record'))throw Error('Use --record');
 const root=path.resolve(__dirname,'../..'),report=readMatrixProfiles(fs.readFileSync(path.join(root,'reference/fixtures/library-item-4.csg')));
 if(report.fixtureSha256!=='73e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd')throw Error('Unexpected fixture');
 const artifact={schemaVersion:1,status:'source_defined_reference_reader',scope:'Raw stored matrix-profile/NURBS arrays, no native geometry/chooser/PC execution. Serialized path labels are data only.',...report,
  sources:['CSBlocks.Core/CSGroupOfMatrixProfiles.cs','CS/CSUtilIO.cs','CS.CSObjects.CSBlocks/CSBlock.cs','CSDatas/CSProfileData.cs'].map(p=>{const source='recovery/canonical/cad/'+p;return {path:source,sha256:crypto.createHash('sha256').update(fs.readFileSync(path.join(root,source))).digest('hex')};})};
 fs.writeFileSync(path.join(root,'docs/evidence/audit/matrix-profile-reference.json'),JSON.stringify(artifact,null,2)+'\n');
 console.log(JSON.stringify({groups:report.groups.length,curves:report.groups.reduce((n,g)=>n+g.curves.length,0),controls:report.groups.reduce((n,g)=>n+g.curves.reduce((m,c)=>m+c.controlVertexCount,0),0)},null,2));
}
module.exports={readMatrixProfiles};
