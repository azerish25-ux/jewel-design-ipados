'use strict';
const fs=require('node:fs'),path=require('node:path'),cp=require('node:child_process'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),fixture=path.join(root,'tests/Port/Geometry.Candidate.Contracts/Fixtures/camera-rotation-source-cases.json');
function inputs(){
 const axes=[[0,0,0],[1,0,0],[0,1,0],[0,0,1],[1,2,3],[1e200,2e200,-3e200],[1e-300,2e-300,-3e-300],[5e-324,0,0]];
 const pairs=[[0,1],[0,-1],[1,0],[-1,0],[3,4],[0,0],[1,1e-10],[1e-10,-1],[1e-300,2e-300],[1e200,2e200],[1e308,1e308],[.6,.8]];
 const vectors=[[1,0,0],[0,1,0],[0,0,1],[2,-3,5]];const rows=[];
 for(const axis of axes)for(const pair of pairs)for(const vector of vectors)rows.push([0,...pair,...axis,...vector]);
 for(const axis of axes)for(const angle of [0,.731,-.731,Math.PI/2,Math.PI,-Math.PI/2,2*Math.PI])for(const vector of vectors)rows.push([1,angle,0,...axis,...vector]);
 return rows.map(r=>r.map(String));
}
function verify(actual,expected){
 if(actual.length!==expected.length)throw Error('Stored case count differs');
 const num=s=>{if(s==='nan')return NaN;if(s==='inf')return Infinity;if(s==='-inf')return -Infinity;if(typeof s!=='string'||s.trim()===''||!Number.isFinite(Number(s)))throw Error('Invalid reference number');return Number(s);};
 for(let i=0;i<actual.length;i++){
  const a=actual[i],b=expected[i];if(JSON.stringify(a.args)!==JSON.stringify(b.args)||a.diagnostics!==b.diagnostics)throw Error('Reference input/diagnostics differ '+i);
  if(a.vector.length!==3||b.vector.length!==3)throw Error('Reference vector size differs');
  a.vector.forEach((s,j)=>{const x=num(s),y=num(b.vector[j]);if(!(Object.is(x,y)||(Number.isNaN(x)&&Number.isNaN(y))||(Number.isFinite(x)&&Number.isFinite(y)&&Math.abs(x-y)<=2e-13*Math.max(1,Math.abs(x),Math.abs(y)))))throw Error('Reference vector differs '+i+':'+j);});
 }
 return true;
}
function run(executable,write=false){const args=inputs();const r=cp.spawnSync(path.resolve(executable),[],{input:args.map(x=>x.join(' ')).join('\n')+'\n',encoding:'utf8',maxBuffer:8*1024*1024});if(r.status!==0)throw Error(r.stderr||'Rotation reference failed');const lines=r.stdout.trim().split(/\r?\n/);if(lines.length!==args.length)throw Error('Rotation case count differs');const cases=lines.map((l,i)=>{const a=l.split('\t');if(a.length!==4||!/^\d+$/.test(a[0]))throw Error('Invalid rotation reference row');return {args:args[i],diagnostics:Number(a[0]),vector:a.slice(1)};});
 const report={schemaVersion:1,sourcePin:'f2dc7fba67017475b8d530835a40fcfb5372134a',scope:'Published-source numeric reference, original5 static policy separate; no original target execution.',cases};
 if(write)fs.writeFileSync(fixture,JSON.stringify(report,null,2)+'\n');else verify(cases,JSON.parse(fs.readFileSync(fixture,'utf8')).cases);
 return {cases:cases.length,fixtureSha256:crypto.createHash('sha256').update(fs.readFileSync(fixture)).digest('hex'),verified:!write};}
if(require.main===module){try{console.log(JSON.stringify(run(process.argv[2],process.argv.includes('--write')),null,2));}catch(e){console.error(e.message);process.exitCode=1;}}module.exports={inputs,run,verify};
