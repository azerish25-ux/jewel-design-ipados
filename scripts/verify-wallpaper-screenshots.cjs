'use strict';
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const {decodePng}=require('./verify-metal-artifacts.cjs');
function verify(folder){
 const report=JSON.parse(fs.readFileSync(path.join(folder,'wallpaper-report.json'),'utf8'));
 const names=['paper-original','paper-green','paper-blue','paper-clear','paper-resized','paper-final'];
 if(report.SchemaVersion!==1||report.Frames.length!==6||names.some((n,i)=>report.Frames[i].Stage!==n))throw Error('Wallpaper stage report mismatch');
 const frames={},hashes={};for(const name of names){const bytes=fs.readFileSync(path.join(folder,name+'.png'));hashes[name]=crypto.createHash('sha256').update(bytes).digest('hex');frames[name]=decodePng(bytes);}
 const first=frames['paper-original'],w=first.width,h=first.height;
 for(const f of Object.values(frames))if(f.width!==w||f.height!==h)throw Error('Wallpaper screenshot dimensions changed');
 const bounds={left:Math.floor(w/8),right:Math.floor(w*7/8),top:Math.floor(h/8),bottom:Math.floor(h*7/8)};
 const close=(a,i,b,j)=>Math.abs(a[i]-b[j])<=3&&Math.abs(a[i+1]-b[j+1])<=3&&Math.abs(a[i+2]-b[j+2])<=3;
 const background=Buffer.from([15,11,9]),results=[];
 for(const [name,color]of[['paper-green',[90,120,35]],['paper-blue',[180,70,35]]]){
  const mark=report.Frames.find(x=>x.Stage===name),data=frames[name].bgra;if(mark.Width!==w||mark.Height!==h)throw Error('Full-view drawable does not match screenshot');
  let checkedBackground=0,checkedCad=0,colored=0;
  for(let y=bounds.top;y<bounds.bottom;y++)for(let x=bounds.left;x<bounds.right;x++){
   const i=(y*w+x)*4,inside=x>=mark.Left&&x<mark.Left+mark.ImageWidth&&y>=mark.Top&&y<mark.Top+mark.ImageHeight;
   if(inside&&close(first.bgra,i,background,0)){checkedBackground++;if(!close(data,i,color,0))throw Error(name+' visible source rectangle/background pixel mismatch');}
   else{checkedCad++;if(!close(data,i,first.bgra,i))throw Error(name+' overwrote CAD geometry or pixels outside source rectangle');}
   if(close(data,i,color,0))colored++;
  }
  if(checkedBackground<1000||colored<1000||checkedCad<1000)throw Error('Insufficient actual image/CAD comparison');
  results.push({stage:name,checkedBackground,checkedCad,colored,rectangle:{left:mark.Left,top:mark.Top,width:mark.ImageWidth,height:mark.ImageHeight}});
 }
 const differences={};for(const name of ['paper-clear','paper-final']){let count=0;for(let y=bounds.top;y<bounds.bottom;y++)for(let x=bounds.left;x<bounds.right;x++){const i=(y*w+x)*4;if(!close(frames[name].bgra,i,first.bgra,i))count++;}if(count>32)throw Error(name+' did not restore actual CAD pixels');differences[name]=count;}
 const after=report.AfterDisposal;if(!after.IsDisposed||after.InFlightFrames!==0||after.LiveMeshResources!==0||after.LiveMeshBuffers!==0||after.LiveImageTextures!==0||after.LiveSceneResources!==0||after.LastError!==null)throw Error('Disposed combined scene retained work/resources/failure');
 if(report.AfterClear.LiveImageTextures!==0||report.AfterClear.LiveMeshBuffers!==2||report.BeforeDisposal.PeakInFlightFrames>3)throw Error('Image clear/combined bound failed');
 if(report.Frames[4].Width===report.Frames[0].Width&&report.Frames[4].Height===report.Frames[0].Height)throw Error('Wallpaper viewport did not resize');
 return {schemaVersion:1,width:w,height:h,bounds,results,differences,disposalInFlight:report.DisposalInFlight,retiredImages:after.LiveImageTextures,retiredMeshBuffers:after.LiveMeshBuffers,pngSha256:hashes,scope:'Actual visible supplied resized wallpaper pixels and regenerated CAD composition. Original resampling/GL/trace/document/PC/physical acceptance separate.'};
}
if(require.main===module)try{console.log(JSON.stringify(verify(process.argv[2]),null,2));}catch(e){console.error(e.message);process.exitCode=1;}
module.exports={verify};
