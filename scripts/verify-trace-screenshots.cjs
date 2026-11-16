'use strict';
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto'),{decodePng}=require('./verify-metal-artifacts.cjs');
function verify(folder){
 const report=JSON.parse(fs.readFileSync(path.join(folder,'trace-report.json'),'utf8')),names=['trace-original','trace-image','trace-orbit','trace-hidden','trace-moved','trace-final'];
 if(report.SchemaVersion!==1||report.Frames.length!==6||names.some((n,i)=>report.Frames[i].Stage!==n))throw Error('Trace stages invalid');
 const frames={},hashes={};for(const n of names){const b=fs.readFileSync(path.join(folder,n+'.png'));frames[n]=decodePng(b);hashes[n]=crypto.createHash('sha256').update(b).digest('hex');}
 const {width:w,height:h}=frames['trace-original'];if(Object.values(frames).some(f=>f.width!==w||f.height!==h))throw Error('Trace capture dimensions differ');
 const box={left:Math.floor(w/8),right:Math.floor(w*7/8),top:Math.floor(h/8),bottom:Math.floor(h*7/8)};
 const close=(a,b,i)=>Math.abs(a[i]-b[i])<=3&&Math.abs(a[i+1]-b[i+1])<=3&&Math.abs(a[i+2]-b[i+2])<=3;
 const green=(a,i)=>Math.abs(a[i]-90)<=3&&Math.abs(a[i+1]-120)<=3&&Math.abs(a[i+2]-35)<=3;
 const counts={};for(const n of ['trace-image','trace-orbit','trace-moved']){let count=0;for(let y=box.top;y<box.bottom;y++)for(let x=box.left;x<box.right;x++)if(green(frames[n].bgra,(y*w+x)*4))count++;if(count<1000)throw Error(n+' world image not visible');counts[n]=count;}
 let worldMotion=0,moved=0;const differences={};for(let y=box.top;y<box.bottom;y++)for(let x=box.left;x<box.right;x++){const i=(y*w+x)*4;if(green(frames['trace-image'].bgra,i)!==green(frames['trace-orbit'].bgra,i))worldMotion++;if(green(frames['trace-image'].bgra,i)!==green(frames['trace-moved'].bgra,i))moved++;}
 if(worldMotion<1000||moved<1000)throw Error('Trace did not follow world camera/pose changes');
 for(const n of ['trace-hidden','trace-final']){let count=0;for(let y=box.top;y<box.bottom;y++)for(let x=box.left;x<box.right;x++){const i=(y*w+x)*4;if(!close(frames[n].bgra,frames['trace-original'].bgra,i))count++;}if(count>32)throw Error(n+' did not reproduce actual no-image CAD');differences[n]=count;}
 if(report.Hidden.LiveImageTextures!==1)throw Error('Hidden state did not retain loaded trace');
 const after=report.AfterDisposal;if(!after.IsDisposed||after.InFlightFrames!==0||after.LiveSceneResources!==0||after.LiveImageTextures!==0||after.LiveMeshResources!==0||after.LiveMeshBuffers!==0||after.LastError!==null)throw Error('Disposed trace scene retained resources/error');
 return {schemaVersion:1,width:w,height:h,bounds:box,counts,worldMotionPixels:worldMotion,movedPixels:moved,differences,disposalInFlight:report.DisposalInFlight,pngSha256:hashes,scope:'Actual prepared world-plane pixels/candidate camera/mesh composition and metadata/ownership. Original source camera/cache/mipmap/filtering/failure diagnostics/document/PC/physical parity separate.'};
}
if(require.main===module)try{console.log(JSON.stringify(verify(process.argv[2]),null,2));}catch(e){console.error(e.message);process.exitCode=1;}
module.exports={verify};
