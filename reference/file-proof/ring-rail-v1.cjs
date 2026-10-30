const fs=require('fs'),path=require('path'),crypto=require('crypto');
const {parse}=require('./csg-v4.cjs');
const {ScalarCursor}=require('./scalar-v1.cjs');
const RING_RAIL='65aa45c6-d1cd-4c45-92bb-b5a9a6a2ec47';
function readRingRails(bytes){
 if(!Buffer.isBuffer(bytes)||bytes.length>64*1024*1024)throw Error('Ring rail byte bound');
 const graph=parse(bytes),results=[];
 for(const node of graph.nodes){
  if(node.guid!==RING_RAIL||node.major!==1||node.minor!==0)continue;
  const cursor=new ScalarCursor(bytes,node.payload+8,node.crcOffset);
  const identifier=cursor.text(),flags=cursor.i()>>>0,legacyLayer=cursor.i(),userData=cursor.userData(),objects=cursor.ints(),channels=cursor.ints();
  if(objects.length!==channels.length)throw Error('Ring rail link array mismatch');
  if(cursor.p!==node.crcOffset)throw Error('Ring rail body not fully consumed');
  results.push({index:node.index,guid:node.guid,major:node.major,minor:node.minor,identifier,flags,legacyLayer,userData,objects,channels,fullyConsumed:true});
 }
 return results;
}
if(require.main===module){
 if(!process.argv.includes('--record'))throw Error('Use --record');
 const root=path.resolve(__dirname,'../..'),bytes=fs.readFileSync(path.join(root,'reference/fixtures/library-item-4.csg')),graph=parse(bytes),rails=readRingRails(bytes);
 if(graph.sha256!=='73e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd')throw Error('Unexpected fixture');
 const report={schemaVersion:1,status:'reference_reader_only',fixtureSha256:graph.sha256,rails,
  recordedInputCandidates:rails.map(rail=>{const source=graph.nodes[rail.objects[0]];return {railIndex:rail.index,sourceIndex:rail.objects[0],sourceChannel:rail.channels[0],sourceTypeGuid:source?.guid,sourceRecordVersion:source?source.major+'.'+source.minor:null,storedValue:source?.numeric?.value??null,units:source?.guid==='dd5c2342-d89a-440e-a76e-624a1faf2b68'&&source.major===1&&source.minor===2?'stored radius mm':'not established'};}),
  scope:'Read-only exact RingRail_V1 header/input references. Stored scalar candidate is not an evaluated channel output; activation/override/missing-input/default/graph/CAD behavior remains unexecuted.',
  sources:['CSBlocks.Rails.Obsolete/CSRingRail1.cs','CS.CSObjects.CSBlocks/CSBlock.cs','CS.CSObjects/CSObject.cs','CS.CSValues/CSValueRingSize.cs'].map(p=>{const source='recovery/canonical/cad/'+p;return {path:source,sha256:crypto.createHash('sha256').update(fs.readFileSync(path.join(root,source))).digest('hex')};})};
 fs.writeFileSync(path.join(root,'docs/evidence/audit/ring-rail-input-reference.json'),JSON.stringify(report,null,2)+'\n');
 console.log(JSON.stringify({rails:rails.length,recordedInputCandidates:report.recordedInputCandidates},null,2));
}
module.exports={readRingRails};
