const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const {parse}=require('./csg-v4.cjs');const {ScalarCursor}=require('./scalar-v1.cjs');
const GUID='fc789225-3f3b-41f5-9eb0-6c25d4fc54b8';
function readProfilePlacements(bytes){const blocks=[];for(const node of parse(bytes).nodes){
 if(node.guid!==GUID||node.major!==1||node.minor!==0)continue;
 const r=new ScalarCursor(bytes,node.payload+8,node.crcOffset),identifier=r.text(),flags=r.i()>>>0,legacyLayer=r.i(),userData=r.userData(),objects=r.ints(),channels=r.ints();
 if(objects.length!==channels.length||r.p!==node.crcOffset)throw Error('Placement body incomplete or mismatched links');
 blocks.push({index:node.index,guid:node.guid,identifier,flags,legacyLayer,userData,objects,channels,fullyConsumed:true});
}return blocks;}
if(require.main===module){
 if(!process.argv.includes('--record'))throw Error('Use --record');
 const root=path.resolve(__dirname,'../..'),bytes=fs.readFileSync(path.join(root,'reference/fixtures/library-item-4.csg')),fixtureSha256=crypto.createHash('sha256').update(bytes).digest('hex');
 if(fixtureSha256!=='73e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd')throw Error('Unexpected fixture');
 const source='recovery/canonical/cad/CSBlocks.Profiles.Obsolete/CSProfilePlacer2RailXform1.cs';
 const report={schemaVersion:1,status:'reference_reader_only',fixtureSha256,blocks:readProfilePlacements(bytes),sources:[{path:source,sha256:crypto.createHash('sha256').update(fs.readFileSync(path.join(root,source))).digest('hex')}],scope:'Exact legacy1.0 common header only; no graph/geometry/PC execution claim.'};
 fs.writeFileSync(path.join(root,'docs/evidence/audit/profile-placement-reference.json'),JSON.stringify(report,null,2)+'\n');console.log(JSON.stringify(report.blocks));
}
module.exports={readProfilePlacements};
