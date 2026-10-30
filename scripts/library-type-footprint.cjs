const fs=require('fs'),path=require('path'),crypto=require('crypto');
const root=path.resolve(__dirname,'..');
const {parse,hash}=require('../reference/file-proof/csg-v4.cjs');
const {readAdditionalScalars}=require('../reference/file-proof/scalar-v1.cjs');
const {readMatrixProfiles}=require('../reference/file-proof/matrix-profiles-v1.cjs');
const {readRingRails}=require('../reference/file-proof/ring-rail-v1.cjs');
const {readOutsideRails}=require('../reference/file-proof/outside-rail-v1.cjs');
const {readSelectRails}=require('../reference/file-proof/select-ring-rail-v1.cjs');
const read=name=>fs.readFileSync(path.join(root,name));
const sha=bytes=>crypto.createHash('sha256').update(bytes).digest('hex');
function build(){
 const fixture='reference/fixtures/library-item-4.csg', bytes=read(fixture);
 if(hash(bytes)!=='73e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd')throw Error('Unexpected fixture');
 const parsed=parse(bytes);
 const additional=new Set(readAdditionalScalars(bytes).extras.map(x=>x.index));
 const profileGroups=readMatrixProfiles(bytes).groups,profiles=new Set(profileGroups.map(x=>x.index));
 const blocks=new Set([...readRingRails(bytes),...readOutsideRails(bytes),...readSelectRails(bytes)].map(x=>x.index));
 if(parsed.count!==241)throw Error('Unexpected serialized node table');
 const matrixPath='docs/evidence/audit/serialization-matrix.json',inventoryPath='docs/evidence/audit/cad-type-inventory.json';
 const matrix=JSON.parse(read(matrixPath)),inventory=JSON.parse(read(inventoryPath));
 const byGuid=new Map(),byName=new Map(),groups=new Map();
 for(const row of matrix.types){const key=row.guid.toLowerCase();if(!byGuid.has(key))byGuid.set(key,[]);byGuid.get(key).push(row);}
 for(const row of inventory.types){if(!byName.has(row.qualifiedName))byName.set(row.qualifiedName,[]);byName.get(row.qualifiedName).push(row);}
 for(const node of parsed.nodes){if(!groups.has(node.guid))groups.set(node.guid,[]);groups.get(node.guid).push(node);}
 const types=[];
 for(const [guid,nodes]of groups){
  const registrations=[];
  for(const row of byGuid.get(guid)||[]){
   const source=read(row.path),sourceSha256=sha(source);
   if(!source.toString('utf8').toLowerCase().includes(guid))throw Error('Registration anchor absent '+row.path);
   const sourceRows=(byName.get(row.type)||[]).filter(x=>x.path===row.path);
   for(const t of sourceRows)if(t.sourceSha256!==sourceSha256)throw Error('Stale inventory source hash '+row.path);
   registrations.push({serializerId:row.id,type:row.type,path:row.path,sourceSha256,legacy:row.legacy,
    inventoryMatches:sourceRows.length,baseChain:row.baseChain,resolutionIssues:row.resolutionIssues,
    directStaticCalls:sourceRows.flatMap(x=>x.kernelStaticCalls||[]),
    serializerSources:Object.fromEntries(Object.entries(row.effectiveMethods).map(([method,items])=>[method,items.map(x=>({path:x.path,line:x.line,end:x.end,declaredBy:x.type}))]))});
  }
  types.push({guid,nodeIndices:nodes.map(x=>x.index),recordVersions:[...new Set(nodes.map(x=>x.major+'.'+x.minor))].sort(),
   numericDecoded:nodes.filter(x=>x.numeric).length,scalarDecoded:nodes.filter(x=>x.numeric||additional.has(x.index)).length,railHeadersDecoded:nodes.filter(x=>blocks.has(x.index)).length,matrixProfilesDecoded:nodes.filter(x=>profiles.has(x.index)).length,opaquePayloads:nodes.filter(x=>!x.numeric&&!additional.has(x.index)&&!profiles.has(x.index)&&!blocks.has(x.index)).length,
   registrationAmbiguous:registrations.length>1,registrations});
 }
 types.sort((a,b)=>a.nodeIndices[0]-b.nodeIndices[0]);
 const callMap=new Map();
 for(const group of types)for(const registration of group.registrations)for(const call of registration.directStaticCalls){
  if(!callMap.has(call.call))callMap.set(call.call,[]);
  callMap.get(call.call).push({type:registration.type,path:registration.path,line:call.line,guid:group.guid});
 }
 return {schemaVersion:1,status:'source_inventory_only',
  scope:'Pinned library serialized type footprint, not executed graph reachability, transitive dependency closure, complete field/runtime decoding or geometry parity',
  inputs:[fixture,matrixPath,inventoryPath,'reference/file-proof/csg-v4.cjs','reference/file-proof/scalar-v1.cjs','src/Port/Documents/LegacyCsg4Codec.cs','src/Port/Documents/LegacyCsg4Scalar.cs','src/Port/Documents/LegacyCsg4MatrixProfiles.cs','reference/file-proof/matrix-profiles-v1.cjs','src/Port/Documents/LegacyCsg4SimpleBlock.cs','reference/file-proof/ring-rail-v1.cjs','reference/file-proof/outside-rail-v1.cjs','reference/file-proof/select-ring-rail-v1.cjs'].map(p=>({path:p,normalization:p.endsWith('.csg')?'none':'utf8-lf',sha256:sha(p.endsWith('.csg')?read(p):Buffer.from(read(p).toString('utf8').replaceAll('\r\n','\n')))})),
  counts:{serializedNodes:parsed.count,registrationGuids:types.length,numericDecoded:parsed.numericCount,scalarDecoded:parsed.numericCount+additional.size,
   railBlockHeadersDecoded:blocks.size,matrixProfileGroupsDecoded:profiles.size,embeddedProfileCurves:profileGroups.reduce((n,g)=>n+g.curves.length,0),
   opaquePayloads:parsed.count-parsed.numericCount-additional.size-profiles.size-blocks.size,unknownGuids:types.filter(x=>!x.registrations.length).length,
   ambiguousGuids:types.filter(x=>x.registrationAmbiguous).length,
   legacyNodes:types.filter(x=>x.registrations.length===1&&x.registrations[0].legacy).reduce((sum,x)=>sum+x.nodeIndices.length,0),
   uniqueDirectStaticCalls:callMap.size},types,
  directStaticCallFootprint:[...callMap].sort((a,b)=>a[0].localeCompare(b[0])).map(([call,evidence])=>({call,evidence})),
  limits:['All serialized nodes included; active outputs and graph edge reachability not decoded for opaque records.',
   'Stored-double decoding covers only five exact observed profiles: Range1.1/RingSize1.2 and Value/OnOff/Chooser1.1. Full value runtime behavior and other versions remain open.',
   'Matrix profiles decode known group1.0/1.1 and direct NURBS1.0 data only; no actual chooser/native curve generation or complete group UI/runtime parity.',
   'Direct lexical static calls omit instance calls, dispatch, base methods and transitive helpers. Zero calls is not geometry-free.',
   'Registration GUID/source mapping and serializer locations do not prove supported record versions or runtime parity.',
   'Nested preview/gem checksums remain opaque as recorded by the independent reader.',
   'Current candidate handles/booleans do not regenerate this design. Native rails, profiles, sweeps, caps, gem data, morphing, legacy semantics and PC oracle remain required.']};
}
function markdown(report){
 const c=report.counts;
 const lines=['# Available library serialized type footprint','',
  'Generated by node scripts/library-type-footprint.cjs --write. [Machine-readable report](library-type-footprint.json).',
  '',c.serializedNodes+' serialized nodes use '+c.registrationGuids+' registration GUIDs; '+c.scalarDecoded+' scalar payloads are decoded (including '+c.numericDecoded+' bounded numeric), plus '+c.matrixProfileGroupsDecoded+' matrix-profile groups with '+c.embeddedProfileCurves+' stored curves and '+c.railBlockHeadersDecoded+' exact rail consumer headers; '+c.opaquePayloads+' remain opaque. '+c.legacyNodes+' nodes map to legacy registrations. This is a serialized footprint, not an executed dependency graph or a regenerated jewelry design.','',
  '| Registered type | Nodes | Serialized versions | Legacy | Scalar decoded | Direct static call sites |',
  '| --- | ---: | --- | --- | ---: | ---: |'];
 for(const group of report.types){
  const label=group.registrations.map(r=>'['+r.type+'](../../../'+r.path+')').join(', ')||'UNKNOWN '+group.guid;
  lines.push('| '+label+' | '+group.nodeIndices.length+' | '+group.recordVersions.join(', ')+' | '+group.registrations.map(r=>String(r.legacy)).join(', ')+' | '+group.scalarDecoded+' | '+group.registrations.reduce((n,r)=>n+r.directStaticCalls.length,0)+' |');
 }
 lines.push('','## Interpretation','',...report.limits.map(x=>'- '+x),'',
  'Use exact registered source paths and serializer locations to define the next decoding and native-operation fixtures. Do not infer that modern similarly named nodes can replace the legacy GUIDs. The prototype box/boolean tests are infrastructure proof; this fixture still requires real curve/profile/rail/sweep/cap/gem/morph operations and the original-PC comparison.','');
 return lines.join('\n');
}
if(require.main===module){
 const report=build(),json=JSON.stringify(report,null,2)+'\n',md=markdown(report);
 const jsonPath=path.join(root,'docs/porting/kernel/library-type-footprint.json'),mdPath=path.join(root,'docs/porting/kernel/LIBRARY_TYPE_FOOTPRINT.md');
 if(process.argv.includes('--write')){fs.writeFileSync(jsonPath,json);fs.writeFileSync(mdPath,md);}
 else if(process.argv.includes('--check')){if(fs.readFileSync(jsonPath,'utf8')!==json||fs.readFileSync(mdPath,'utf8')!==md)throw Error('Stale library type footprint');}
 else throw Error('Use --write or --check');
 console.log(JSON.stringify(report.counts,null,2));
}
module.exports={build};
