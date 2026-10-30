const fs=require('fs'),path=require('path'),assert=require('assert/strict');
const {parse}=require('./csg-v4.cjs');
const {readAdditionalScalars}=require('./scalar-v1.cjs');
const root=path.resolve(__dirname,'../..');
function verify(folder){
 const source=fs.readFileSync(path.join(folder,'source.csg')),original=parse(source),scalars=readAdditionalScalars(source);
 assert.equal(original.sha256,'73e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd');
 const cases=JSON.parse(fs.readFileSync(path.join(folder,'cases.json'),'utf8'));assert.equal(cases.length,3);
 const seen=new Set(),results=[];
 for(const item of cases){
  assert.match(item.file,/^scalar-[0-9]+[.]csg$/);assert.equal(path.basename(item.file),item.file);
  const before=scalars.extras.find(x=>x.index===item.index);assert(before,'Export target not in additional scalar profile');
  assert.equal(before.valueBits,item.beforeBits);seen.add(before.guid);
  const bytes=fs.readFileSync(path.join(folder,item.file)),after=parse(bytes),decoded=readAdditionalScalars(bytes);
  assert.equal(after.count,241);assert.equal(after.numericCount,74);assert.equal(decoded.extras.length,26);
  assert.equal(decoded.extras.find(x=>x.index===item.index).valueBits,item.afterBits);
  let preserved=0;
  for(const node of original.nodes)if(node.index!==item.index){assert.equal(after.nodes[node.index].rawHash,node.rawHash);preserved++;}
  assert.equal(preserved,240);assert.deepEqual(after.regions,original.regions);
  for(const record of scalars.extras)if(record.index!==item.index)assert.deepEqual(decoded.extras.find(x=>x.index===record.index),record);
  results.push({file:item.file,index:item.index,type:before.type,beforeBits:item.beforeBits,afterBits:item.afterBits,otherRecordsPreserved:preserved,sha256:after.sha256});
 }
 assert.equal(seen.size,3);
 return {scope:'Independent Node CRC/full-body/bit/header/other-record verification of production CSharp structural outputs; no PC application or geometry regeneration claim',sourceSha256:original.sha256,cases:results};
}
if(require.main===module){
 const result=verify(path.join(root,'.local/file-proof/production/additional-scalars'));
 const output=path.join(root,'.local/evidence/additional-scalar-crosscheck.json');fs.mkdirSync(path.dirname(output),{recursive:true});fs.writeFileSync(output,JSON.stringify(result,null,2)+'\n');
 console.log(JSON.stringify(result,null,2));
}
module.exports={verify};
