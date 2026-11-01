'use strict';
const fs=require('node:fs'),path=require('node:path'),cp=require('node:child_process'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),fixture=path.join(root,'tests/Port/Geometry.Candidate.Contracts/Fixtures/frustum-source-cases.json');
const token=x=>Number.isNaN(x)?'nan':x===Infinity?'inf':x===-Infinity?'-inf':Object.is(x,-0)?'-0':String(x);
function cases(){
 const rows=[];function add(op,flags,bounds,args=[]){rows.push({id:rows.length,op,flags,before:bounds.map(token),args:args.map(token)});}
 const bases=[[-2,4,-1,3,1,1000],[-.6,.6,-.4,.4,1,1000]];
 for(let flags=0;flags<32;flags++)for(const bounds of bases){
  add(3,flags,bounds);add(4,flags,bounds);
  for(const lens of [0,1,12,30,50,200,NaN,Infinity])add(1,flags,bounds,[lens]);
  for(const angle of [0,.1,.5,1.2,Math.PI/2])add(2,flags,bounds,[angle]);
  for(const request of [[-2,4,-1,3,1,1000],[-1,1,-1,1,1e-9,1],[-1,1,-1,1,1,1e100]])add(0,flags,bounds,request);
 }
 add(0,3,[-1,1,-1,1,1,100],[-0,1,-0,1,1,10]);
 add(3,3,[-0,1,-0,1,1,10]);
 const valid=[-1,1,-1,1,1,100];
 for(let field=0;field<6;field++)for(const value of [NaN,Infinity,-Infinity,-1.23432101234321e308,1.23432101234321e308]){const invalid=[...valid];invalid[field]=value;add(0,3,valid,invalid);}
 for(const bound of [1e100,1e100*(1-Number.EPSILON),-1e100,-1e100*(1-Number.EPSILON)])add(0,3,valid,[-1,bound,-1,1,1,100]);
 for(const bounds of [[0,0,-1,1,1,10],[-1,1,0,0,1,10],[-1,1,-1,1,0,10],[NaN,1,-1,1,1,10],[-1,1,-1,1,NaN,10]])for(const op of [1,2,3,4])add(op,3,bounds,op===1?[50]:op===2?[.3]:[]);
 for(const delta of [0,9.99999e-7,1e-6,1.000001e-6,-9.99999e-7,-1e-6,-1.000001e-6])add(1,3,[-.6,.6,-.4,.4,1,100],[30/(1+delta)]);
 for(const field of [0,1,2,3,5])for(const scale of [1-Number.EPSILON,1,1+Number.EPSILON]){const request=[...valid];request[field]=(field===0||field===2?-1:1)*1e100*scale;add(0,3,valid,request);}
 for(const near of [1e-8*(1-Number.EPSILON),1e-8,1e-8*(1+Number.EPSILON)])add(0,3,valid,[-1,1,-1,1,near,1]);
 for(const far of [1.0001e8*(1-Number.EPSILON),1.0001e8,1.0001e8*(1+Number.EPSILON)])add(0,3,valid,[-1,1,-1,1,1,far]);
 for(const lens of [-1.23432101234321e308,1.23432101234321e308,Number.MIN_VALUE,Number.MAX_VALUE])add(1,3,valid,[lens]);
 for(const angle of [NaN,Infinity,-.1,.5*Math.PI*(1-1.490116119385e-8),.5*Math.PI*(1-1.490116119385e-8)*(1-Number.EPSILON)])add(2,3,valid,[angle]);
 return rows;
}
function numeric(s){return s==='nan'?NaN:s==='inf'?Infinity:s==='-inf'?-Infinity:Number(s);}
function same(a,b){if(a===b)return true;const x=numeric(a),y=numeric(b);if(x===0&&y===0)return Object.is(x,y);return Number.isNaN(x)&&Number.isNaN(y)||Number.isFinite(x)&&Number.isFinite(y)&&Math.abs(x-y)<=1e-13*Math.max(1,Math.abs(x),Math.abs(y));}
function run(exe){
 const rows=cases(),input=rows.map(r=>[r.id,r.op,r.flags,...r.before,...r.args].join(' ')).join('\n')+'\n';
 const result=cp.spawnSync(path.resolve(root,exe),[],{cwd:root,input,encoding:'utf8',maxBuffer:8*1024*1024,timeout:30000});
 if(result.error)throw result.error;if(result.status!==0)throw Error(result.stderr||'Native frustum reference failed');
 const lines=result.stdout.trim().split(/\r?\n/);if(lines.length!==rows.length)throw Error('Native case count differs');
 return rows.map((row,index)=>{const f=lines[index].split('\t');if(f.length!==13||Number(f[0])!==row.id)throw Error('Native case shape differs');return {...row,expected:{ok:f[1]==='1',scalar:f[2],bounds:f.slice(3,9),valid:f[9]==='1',cacheReset:f[10]==='1',diagnostics:Number(f[11]),twoPoint:f[12]==='1'}};});
}
function verify(expected,actual){if(expected.length!==actual.length)throw Error('Fixture count stale');for(let i=0;i<expected.length;i++){const a=expected[i],b=actual[i];if(JSON.stringify({...a,expected:null})!==JSON.stringify({...b,expected:null}))throw Error('Fixture input stale at '+i);for(const key of ['ok','valid','cacheReset','diagnostics','twoPoint'])if(a.expected[key]!==b.expected[key])throw Error('Native outcome differs '+i+':'+key);if(!same(a.expected.scalar,b.expected.scalar)||a.expected.bounds.some((v,j)=>!same(v,b.expected.bounds[j])))throw Error('Native numeric outcome differs '+i);}return true;}
if(require.main===module)try{const rows=run(process.argv[2]);if(process.argv.includes('--write')){fs.mkdirSync(path.dirname(fixture),{recursive:true});fs.writeFileSync(fixture,'{"schemaVersion":1,"cases":[\n'+rows.map(r=>JSON.stringify(r)).join(',\n')+'\n]}\n');}verify(JSON.parse(fs.readFileSync(fixture,'utf8')).cases,rows);console.log(JSON.stringify({cases:rows.length,fixture:path.relative(root,fixture).replaceAll('\\','/'),fixtureSha256:crypto.createHash('sha256').update(fs.readFileSync(fixture)).digest('hex'),scope:'Compiled pinned published numerical methods; no original Rhino5/GPU/PC claim'},null,2));}catch(e){console.error(e.message);process.exitCode=1;}
module.exports={cases,run,verify};
