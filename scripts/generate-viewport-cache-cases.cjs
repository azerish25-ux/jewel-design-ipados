'use strict';
const fs=require('node:fs'),path=require('node:path'),cp=require('node:child_process'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),fixture=path.join(root,'tests/Port/Geometry.Candidate.Contracts/Fixtures/viewport-cache-source-cases.json');
function inputs(){
 const id=[1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1];
 const frames=[[0,0,80,1,0,0,0,1,0,0,0,1],[1e200,-2e200,3e200,2,.2,0,0,1,.5,.1,0,1],[0,-80,0,1,0,0,0,0,1,0,-1,0]];
 const bounds=[[-1,1,-1,1,1,1000],[-3,2,-4,5,.1,100],[1,1,-1,1,1,1000],[2,-3,4,-5,100,.1],['nan',1,-1,1,1,1000]];
 const mods=[id,[1,.2,0,3,0,1,.3,-2,.1,0,1,4,0,0,0,1],[1,0,0,.1,0,2,0,0,0,0,1,0,.01,0,0,1]];
 const extras=[id,[2,.2,0,3,0,1,0,-2,0,0,.5,5,0,0,0,1]];const result=[];
 for(let flags=0;flags<4;flags++)for(const projection of [0,1,2])for(const frame of frames)for(const b of bounds)for(const mod of mods)for(const extra of extras)result.push([flags,projection,...frame,...b,...mod,...extra].map(String));
 for(const value of ['nan','inf','-inf','-0']){const extra=[...id];extra[3]=value;result.push([3,2,...frames[0],...bounds[0],...id,...extra].map(String));}
 return result;
}
function number(s){if(s==='nan')return NaN;if(s==='inf')return Infinity;if(s==='-inf')return -Infinity;if(typeof s!=='string'||s.trim()===''||!Number.isFinite(Number(s)))throw Error('Invalid cache number');return Number(s);}
function verify(a,b){if(a.length!==b.length)throw Error('Cache count differs');for(let i=0;i<a.length;i++){const x=a[i],y=b[i];if(JSON.stringify(x.args)!==JSON.stringify(y.args)||x.ok!==y.ok||x.identity!==y.identity)throw Error('Cache input/status differs '+i);if(x.values.length!==16||y.values.length!==16)throw Error('Cache matrix size differs');for(let j=0;j<16;j++){const p=number(x.values[j]),q=number(y.values[j]);if(!(Object.is(p,q)||(Number.isNaN(p)&&Number.isNaN(q))||(Number.isFinite(p)&&Number.isFinite(q)&&Math.abs(p-q)<=2e-13*Math.max(1,Math.abs(p),Math.abs(q)))))throw Error('Cache coefficient differs '+i+':'+j);}}return true;}
function run(executable,write){const args=inputs();const r=cp.spawnSync(path.resolve(executable),[],{input:args.map(x=>x.join(' ')).join('\n')+'\n',encoding:'utf8',maxBuffer:16*1024*1024});if(r.status!==0)throw Error(r.stderr||'Cache reference failed');const lines=r.stdout.trim().split(/\r?\n/);if(lines.length!==args.length)throw Error('Cache output count differs');const cases=lines.map((l,i)=>{const a=l.split('\t');if(a.length!==18||!['0','1'].includes(a[0])||!['0','1'].includes(a[1]))throw Error('Malformed cache output');return {args:args[i],ok:a[0]==='1',identity:a[1]==='1',values:a.slice(2)};});if(write)fs.writeFileSync(fixture,'{"schemaVersion":1,"sourcePin":"f2dc7fba67017475b8d530835a40fcfb5372134a","scope":"Published full GetXform world-to-clip plus extra product; original5 static policy separate","cases":[\n'+cases.map(x=>JSON.stringify(x)).join(',\n')+'\n]}\n');else verify(cases,JSON.parse(fs.readFileSync(fixture,'utf8')).cases);return {cases:cases.length,verified:!write,fixtureSha256:crypto.createHash('sha256').update(fs.readFileSync(fixture)).digest('hex')};}
if(require.main===module)try{console.log(JSON.stringify(run(process.argv[2],process.argv.includes('--write')),null,2));}catch(e){console.error(e.message);process.exitCode=1;}module.exports={inputs,verify,run};
