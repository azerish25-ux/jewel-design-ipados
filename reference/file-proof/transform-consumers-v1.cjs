const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const {parse}=require('./csg-v4.cjs');const {ScalarCursor}=require('./scalar-v1.cjs');
const GUID='27f4537a-6fc3-404f-9abd-eb53b7b3d8a1';
function layout(objects,channels,count){
 const none=()=>({objectIndex:-1,channelIndex:-1}),runtime=[none(),none()];
 function expand(){const last=runtime.at(-1);if(last.objectIndex>=0&&last.channelIndex>=0)runtime.push(none());return runtime.length-1;}
 for(let i=0;i<objects.length;i++){const valid=objects[i]>=0&&objects[i]<count;const slot=i<runtime.length?i:expand();runtime[slot]=valid?{objectIndex:objects[i],channelIndex:channels[i]}:none();}
 expand();return {runtimeChannels:runtime,operationChannels:runtime.slice(0,-1)};
}
function readTransformConsumers(bytes){const graph=parse(bytes),blocks=[];for(const node of graph.nodes){
 if(node.guid!==GUID||node.major!==1||node.minor!==0)continue;
 const r=new ScalarCursor(bytes,node.payload+8,node.crcOffset),identifier=r.text(),flags=r.i()>>>0,legacyLayer=r.i(),userData=r.userData(),objects=r.ints(),channels=r.ints();
 if(objects.length!==channels.length||r.p!==node.crcOffset)throw Error('Consumer body incomplete or mismatched links');
 blocks.push({index:node.index,guid:node.guid,identifier,flags,legacyLayer,userData,objects,channels,...layout(objects,channels,graph.count),fullyConsumed:true});
}return blocks;}
if(require.main===module){
 if(!process.argv.includes('--record'))throw Error('Use --record');
 const root=path.resolve(__dirname,'../..'),bytes=fs.readFileSync(path.join(root,'reference/fixtures/library-item-4.csg')),fixtureSha256=crypto.createHash('sha256').update(bytes).digest('hex');
 if(fixtureSha256!=='73e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd')throw Error('Unexpected fixture');
 const sources=['CSBlocks.Transforms/CSTransformObject.cs','CS.CSObjects.CSBlocks/CSBlock.cs','CS.CSObjects.CSBlocks/CSBlockExpandableInput.cs'].map(p=>{const file='recovery/canonical/cad/'+p;return {path:file,sha256:crypto.createHash('sha256').update(fs.readFileSync(path.join(root,file))).digest('hex')};});
 const report={schemaVersion:1,status:'reference_reader_only',fixtureSha256,blocks:readTransformConsumers(bytes),sources,scope:'Exact stored1.0 headers plus independently reconstructed sockets. No native geometry or original-PC execution claim.'};
 fs.writeFileSync(path.join(root,'docs/evidence/audit/transform-consumer-reference.json'),JSON.stringify(report,null,2)+'\n');console.log(JSON.stringify({blocks:report.blocks.length,indices:report.blocks.map(x=>x.index)}));
}
module.exports={readTransformConsumers,layout};
