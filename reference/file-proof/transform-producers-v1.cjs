const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const {parse}=require('./csg-v4.cjs');const {ScalarCursor}=require('./scalar-v1.cjs');
const TYPES=new Map([['f9af1449-1903-4d26-92df-6febc8a61427','CSTranslateObjectXform'],['e80ed2a1-d415-4ded-b1e8-b8557ac37e9c','CSScaleObjectToSizeXform'],['8793b48f-30c7-4841-9314-157a5c7bb629','CSAdd']]);
function readTransformProducers(bytes){
 const blocks=[];
 for(const node of parse(bytes).nodes){
  if(!TYPES.has(node.guid)||node.major!==1||node.minor!==0)continue;
  const r=new ScalarCursor(bytes,node.payload+8,node.crcOffset);
  const identifier=r.text(),flags=r.i()>>>0,legacyLayer=r.i(),userData=r.userData(),objects=r.ints(),channels=r.ints();
  if(objects.length!==channels.length||r.p!==node.crcOffset)throw Error('Transform producer body incomplete or mismatched links');
  blocks.push({index:node.index,type:TYPES.get(node.guid),guid:node.guid,identifier,flags,legacyLayer,userData,objects,channels,fullyConsumed:true});
 }
 return blocks;
}
if(require.main===module){
 if(!process.argv.includes('--record'))throw Error('Use --record');
 const root=path.resolve(__dirname,'../..'),bytes=fs.readFileSync(path.join(root,'reference/fixtures/library-item-4.csg')),fixtureSha256=crypto.createHash('sha256').update(bytes).digest('hex');
 if(fixtureSha256!=='73e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd')throw Error('Unexpected fixture');
 const sources=['CSBlocks.Transforms/CSTranslateObjectXform.cs','CSBlocks.Transforms/CSScaleObjectToSizeXform.cs','CSBlocks.Maths/CSAdd.cs'].map(p=>{const file='recovery/canonical/cad/'+p;return {path:file,sha256:crypto.createHash('sha256').update(fs.readFileSync(path.join(root,file))).digest('hex')};});
 const report={schemaVersion:1,status:'reference_reader_only',fixtureSha256,blocks:readTransformProducers(bytes),sources,scope:'Exact1.0 stored headers/links only; no evaluated transform, complete input closure or original-PC proof.'};
 fs.writeFileSync(path.join(root,'docs/evidence/audit/transform-producer-reference.json'),JSON.stringify(report,null,2)+'\n');console.log(JSON.stringify({blocks:report.blocks.length,indices:report.blocks.map(x=>x.index)}));
}
module.exports={readTransformProducers};
