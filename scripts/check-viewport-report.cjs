'use strict';
const fs=require('node:fs');
function verify(report){
 if(report.SchemaVersion!==1)throw Error('Unsupported viewport schema');
 const names=['original','orbit','edited','empty','resized','final'];
 if(!Array.isArray(report.Frames)||report.Frames.length!==names.length)throw Error('Viewport stage count mismatch');
 for(let i=0;i<names.length;i++){
  const f=report.Frames[i];if(f.Stage!==names[i])throw Error('Viewport stage order mismatch');
  for(const key of ['FrameId','Revision','MeshUploadId','Width','Height'])if(!Number.isSafeInteger(f[key])||f[key]<=0)throw Error('Invalid viewport '+key);
  if(!Number.isFinite(f.PresentedTime)||f.PresentedTime<=0)throw Error('Native drawable presentation time missing');
 }
 const [original,orbit,edited,empty,resized,final]=report.Frames;
 if(original.MeshUploadId!==orbit.MeshUploadId||edited.MeshUploadId<=original.MeshUploadId||empty.MeshUploadId<=edited.MeshUploadId)throw Error('Source edit/orbit upload identity mismatch');
 if(original.Width===resized.Width&&original.Height===resized.Height)throw Error('Resize did not change drawable dimensions');
 const before=report.BeforeDisposal,after=report.AfterDisposal,last=report.FinalView;
 if(!Number.isInteger(before.InFlightFrames)||before.InFlightFrames<0||before.InFlightFrames>3||before.PeakInFlightFrames<1||before.PeakInFlightFrames>3||before.InFlightFrames>before.PeakInFlightFrames)throw Error('Visible frame bound failed');
 if(!after.IsDisposed||after.InFlightFrames!==0||after.LiveMeshResources!==0||after.LiveMeshBuffers!==0||after.LastError!=null)throw Error('Disposed viewport retained work/failure');
 if(last.IsDisposed||last.InFlightFrames!==0||last.LiveMeshBuffers!==2||last.LastError!=null||last.PresentedFrames<1)throw Error('Final visible viewport invalid');
 if(!Number.isInteger(report.DisposalInFlight)||report.DisposalInFlight<0||report.DisposalInFlight>3)throw Error('Invalid observed disposal timing');
 return {stages:names,peakInFlight:before.PeakInFlightFrames,disposalInFlight:report.DisposalInFlight,retiredBuffers:after.LiveMeshBuffers,actualPresentation:true,scope:'Native timing/resource report gates only; actual screenshots and offscreen pixel proof inspected separately.'};
}
if(require.main===module)try{console.log(JSON.stringify(verify(JSON.parse(fs.readFileSync(process.argv[2],'utf8'))),null,2));}catch(error){console.error(error.message);process.exitCode=1;}
module.exports={verify};
