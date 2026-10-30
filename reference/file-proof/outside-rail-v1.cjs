const fs=require('fs'),path=require('path'),crypto=require('crypto');
const {parse}=require('./csg-v4.cjs');
const {ScalarCursor,readAdditionalScalars}=require('./scalar-v1.cjs');
const GUID='f9f6400f-67c2-4883-8c44-a91f411f2ee3';
const inputNames=['RingRadius','OutsideCurve','TopThickness','SideThickness','BottomThickness','LockOn'];
function readOutsideRails(bytes){
 if(!Buffer.isBuffer(bytes)||bytes.length>64*1024*1024)throw Error('Outside rail byte bound');
 const graph=parse(bytes),results=[];
 for(const node of graph.nodes){
  if(node.guid!==GUID||node.major!==1||node.minor!==0)continue;
  const c=new ScalarCursor(bytes,node.payload+8,node.crcOffset);
  const identifier=c.text(),flags=c.i()>>>0,legacyLayer=c.i(),userData=c.userData(),objects=c.ints(),channels=c.ints();
  if(objects.length!==6||channels.length!==6)throw Error('Outside rail expected six input references');
  if(c.p!==node.crcOffset)throw Error('Outside rail body not fully consumed');
  results.push({index:node.index,guid:node.guid,major:node.major,minor:node.minor,identifier,flags,legacyLayer,userData,objects,channels,fullyConsumed:true});
 }
 return results;
}
function ellipseMinimum(){
 const a=3,b=7,n=2048;
 const speed=t=>Math.sqrt(a*a*Math.cos(t)**2+b*b*Math.sin(t)**2);
 function length(t){const step=t/n;let sum=speed(0)+speed(t);for(let i=1;i<n;i++)sum+=(i%2?4:2)*speed(i*step);return sum*step/3;}
 const total=length(2*Math.PI);let minimum=Infinity;
 for(let i=0;i<50;i++){let low=0,high=2*Math.PI;for(let j=0;j<50;j++){const mid=(low+high)/2;if(length(mid)<total*i/50)low=mid;else high=mid;}const t=(low+high)/2;minimum=Math.min(minimum,Math.hypot(a*Math.sin(t),b*Math.cos(t)));}
 return {semiX:a,semiZ:b,samples:50,endpointExcluded:true,method:'Independent analytic ellipse speed, composite Simpson2048 plus bisection50; no native geometry calls',sampledMinimum:Number(minimum.toFixed(12)),exactClosestDistance:3};
}
function buildReport(bytes){
 const graph=parse(bytes),extras=new Map(readAdditionalScalars(bytes).extras.map(x=>[x.index,x]));
 const rails=readOutsideRails(bytes);
 return {schemaVersion:1,status:'reference_header_and_analytic_fixture_only',fixtureSha256:graph.sha256,rails,
  storedInputCandidates:rails.map(rail=>({railIndex:rail.index,inputs:rail.objects.map((id,i)=>{const node=graph.nodes[id],extra=extras.get(id);return {name:inputNames[i],sourceIndex:id,sourceChannel:rail.channels[i],sourceTypeGuid:node?.guid,sourceVersion:node?node.major+'.'+node.minor:null,storedValue:node?.numeric?.value??extra?.valueNumber??null,meaning:i===1?'stored chooser selection, NOT evaluated curve':i===5?'stored number candidate; CSNumberData Boolean is >0.5':'stored scalar candidate, NOT evaluated channel'};})})),
  analyticEllipse:ellipseMinimum(),
  scope:'Read-only exact OutsideRail_V1 header and stored input candidates. Actual chooser curve/channel activation/override/graph/PC regeneration not executed. Analytic ellipse is independently computed test geometry, not recovered project output.',
  sources:['CSBlocks.Rails.Obsolete/CSOutsideRail1.cs','CSBlockUtil/CSUtilGeometry.cs','CSBlockUtil/CSUtilRingRail.cs','CSDatas/CSNumberData.cs'].map(p=>{const source='recovery/canonical/cad/'+p;return {path:source,sha256:crypto.createHash('sha256').update(fs.readFileSync(path.join(__dirname,'../..',source))).digest('hex')};})};
}
if(require.main===module){
 if(!process.argv.includes('--record'))throw Error('Use --record');
 const root=path.resolve(__dirname,'../..'),report=buildReport(fs.readFileSync(path.join(root,'reference/fixtures/library-item-4.csg')));
 if(report.fixtureSha256!=='73e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd')throw Error('Unexpected fixture');
 fs.writeFileSync(path.join(root,'docs/evidence/audit/outside-rail-input-reference.json'),JSON.stringify(report,null,2)+'\n');
 console.log(JSON.stringify({rails:report.rails.length,inputs:report.storedInputCandidates,analyticEllipse:report.analyticEllipse},null,2));
}
module.exports={readOutsideRails,buildReport};
