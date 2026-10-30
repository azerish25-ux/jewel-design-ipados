"use strict";
const fs=require("node:fs"),path=require("node:path"),crypto=require("node:crypto");
const {extract}=require("./verify-value-channel-oracle.cjs");
const root=path.resolve(__dirname,"..");
const source="recovery/canonical/cad/CSBlocks.Rails/CSSelectRingRail.cs";
const target="tests/Port/Geometry.Candidate.Contracts/Recovered/LegacySelectRingRail.cs";
function build(){
 const bytes=fs.readFileSync(path.join(root,source)),body=extract(bytes.toString("utf8"),"public override bool Update()");
 const code="#nullable disable\n// Exact source Update with test-only channel/data observation stubs.\nusing JewelDesign.Documents;\nnamespace CandidateGeometryContractChecks.Recovered.SelectRails;\ninternal sealed class CSData{internal LegacyChannelValue Value;internal CSData(LegacyChannelValue value){Value=value;}}\ninternal sealed class Input{internal CSData Data;internal Input(LegacyChannelValue value){Data=value.IsMissing?null:new(value);}internal bool GetBoolean(ref bool value)=>Data?.Value.GetBoolean(ref value)??false;}\ninternal sealed class Output{internal CSData Data;internal void SetData(CSData data){Data=data;}}\ninternal class CSBlock{public virtual bool Update()=>true;}\ninternal sealed class CSSelectRingRail:CSBlock\n{\nprivate readonly Input iRingRail1,iRingRail2,iUseRingRail2;\ninternal readonly Output oRingRail=new();\ninternal CSSelectRingRail(LegacyChannelValue first,LegacyChannelValue second,LegacyChannelValue control){iRingRail1=new(first);iRingRail2=new(second);iUseRingRail2=new(control);}\n"+body+"\n}\n";
 return {code,source,sourceSha256:crypto.createHash("sha256").update(bytes).digest("hex"),bodySha256:crypto.createHash("sha256").update(body).digest("hex"),target};
}
function verify(code){if(code.replaceAll("\r\n","\n")!==build().code)throw Error("Selector source body differs from canonical");return true;}
if(require.main===module){const result=build();if(process.argv.includes("--write")){fs.mkdirSync(path.dirname(path.join(root,target)),{recursive:true});fs.writeFileSync(path.join(root,target),result.code);}verify(fs.readFileSync(path.join(root,target),"utf8"));console.log(JSON.stringify({source:result.source,sourceSha256:result.sourceSha256,bodySha256:result.bodySha256,target},null,2));}
module.exports={build,verify};
