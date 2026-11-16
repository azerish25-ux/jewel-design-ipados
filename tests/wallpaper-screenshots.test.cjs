'use strict';
// Synthetic buffers test rejection rules only; product evidence must come from actual simulator captures.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),zlib=require('node:zlib');
const {verify}=require('../scripts/verify-wallpaper-screenshots.cjs');
function png(data){const c=(name,b)=>{const t=Buffer.from(name),n=Buffer.alloc(4),crc=Buffer.alloc(4);n.writeUInt32BE(b.length);crc.writeUInt32BE(zlib.crc32(Buffer.concat([t,b]))>>>0);return Buffer.concat([n,t,b,crc]);};const h=Buffer.alloc(13);h.writeUInt32BE(128);h.writeUInt32BE(128,4);h[8]=8;h[9]=6;const rows=Buffer.alloc(513*128);for(let y=0;y<128;y++)for(let x=0;x<128;x++){const i=(y*128+x)*4,p=y*513+1+x*4;rows[p]=data[i+2];rows[p+1]=data[i+1];rows[p+2]=data[i];rows[p+3]=255;}return Buffer.concat([Buffer.from('89504e470d0a1a0a','hex'),c('IHDR',h),c('IDAT',zlib.deflateSync(rows)),c('IEND',Buffer.alloc(0))]);}
function fixture(change){
 const root=path.resolve(__dirname,'../.local/evidence/visible-wallpaper/node-tests');fs.mkdirSync(root,{recursive:true});const dir=fs.mkdtempSync(path.join(root,'case-'));
 try{
  const names=['paper-original','paper-green','paper-blue','paper-clear','paper-resized','paper-final'],frames={},marks=[];
  const baseline=Buffer.alloc(128*128*4);for(let y=0;y<128;y++)for(let x=0;x<128;x++){const i=(y*128+x)*4,c=x>=54&&x<74&&y>=34&&y<94?[100,100,100]:[15,11,9];baseline[i]=c[0];baseline[i+1]=c[1];baseline[i+2]=c[2];baseline[i+3]=255;}
  for(const name of names){const data=Buffer.from(baseline),size=name==='paper-green'?48:name==='paper-blue'?96:0,left=(128-size)/2,top=left;
   if(size)for(let y=top;y<top+size;y++)for(let x=left;x<left+size;x++){const i=(y*128+x)*4;if(data[i]!==15)continue;const c=name==='paper-green'?[90,120,35]:[180,70,35];data[i]=c[0];data[i+1]=c[1];data[i+2]=c[2];}
   frames[name]=data;marks.push({Stage:name,Width:name==='paper-resized'?100:128,Height:128,Left:left,Top:top,ImageWidth:size,ImageHeight:size});
  }
  const report={SchemaVersion:1,Frames:marks,AfterDisposal:{IsDisposed:true,InFlightFrames:0,LiveMeshResources:0,LiveMeshBuffers:0,LiveImageTextures:0,LiveSceneResources:0,LastError:null},AfterClear:{LiveImageTextures:0,LiveMeshBuffers:2},BeforeDisposal:{PeakInFlightFrames:3},DisposalInFlight:2};
  change?.(frames,report);for(const n of names)fs.writeFileSync(path.join(dir,n+'.png'),png(frames[n]));fs.writeFileSync(path.join(dir,'wallpaper-report.json'),JSON.stringify(report));return verify(dir);
 }finally{const actual=fs.realpathSync(dir),base=fs.realpathSync(root);if(!actual.startsWith(base+path.sep))throw Error('Test cleanup leaves owned directory');fs.rmSync(actual,{recursive:true,force:true});}
}
test('Visible wallpaper verifier validates image background and unchanged CAD/clear/final',()=>{const r=fixture();assert.equal(r.differences['paper-clear'],0);assert.equal(r.retiredImages,0);});
test('Visible wallpaper verifier rejects image overwriting real CAD pixels',()=>assert.throws(()=>fixture(f=>{const i=(64*128+64)*4;f['paper-green'][i]=90;f['paper-green'][i+1]=120;f['paper-green'][i+2]=35;}),/overwrote CAD/));
test('Visible wallpaper verifier rejects wrong rectangle and leaked textures',()=>{assert.throws(()=>fixture((f,r)=>r.Frames[1].Left+=20),/pixel mismatch|overwrote CAD/);assert.throws(()=>fixture((f,r)=>r.AfterDisposal.LiveImageTextures=1),/retained/);});
