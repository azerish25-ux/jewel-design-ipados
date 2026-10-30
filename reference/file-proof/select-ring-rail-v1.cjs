const fs=require("node:fs"),path=require("node:path"),crypto=require("node:crypto");
const {parse}=require("./csg-v4.cjs");
const {ScalarCursor}=require("./scalar-v1.cjs");
const SELECT="e10a6966-719f-4ba9-9efe-b7ad0378b5f3";
function readSelectRails(bytes){
 const graph=parse(bytes),rails=[];
 for(const node of graph.nodes){
  if(node.guid!==SELECT||node.major!==1||node.minor!==0)continue;
  const r=new ScalarCursor(bytes,node.payload+8,node.crcOffset);
  const identifier=r.text(),flags=r.i()>>>0,legacyLayer=r.i(),userData=r.userData(),objects=r.ints(),channels=r.ints();
  if(objects.length!==channels.length||r.p!==node.crcOffset)throw Error("Selector header not fully consumed or mismatched links");
  rails.push({index:node.index,guid:node.guid,identifier,flags,legacyLayer,userData,objects,channels,fullyConsumed:true});
 }
 return rails;
}
if(require.main===module){
 if(!process.argv.includes("--record"))throw Error("Use --record");
 const root=path.resolve(__dirname,"../.."),bytes=fs.readFileSync(path.join(root,"reference/fixtures/library-item-4.csg"));
 const fixtureSha256=crypto.createHash("sha256").update(bytes).digest("hex");
 if(fixtureSha256!=="73e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd")throw Error("Unexpected fixture");
 const source="recovery/canonical/cad/CSBlocks.Rails/CSSelectRingRail.cs";
 const report={schemaVersion:1,status:"reference_reader_only",fixtureSha256,rails:readSelectRails(bytes),sources:[{path:source,sha256:crypto.createHash("sha256").update(fs.readFileSync(path.join(root,source))).digest("hex")}],scope:"Exact stored selector1.0 header only; execution and original PC parity not claimed."};
 fs.writeFileSync(path.join(root,"docs/evidence/audit/select-ring-rail-reference.json"),JSON.stringify(report,null,2)+"\n");
 console.log(JSON.stringify(report.rails));
}
module.exports={readSelectRails};
