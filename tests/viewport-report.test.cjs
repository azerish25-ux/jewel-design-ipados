const test=require('node:test'),assert=require('node:assert/strict');
const {verify}=require('../scripts/check-viewport-report.cjs');
// These fixtures test report integrity, never native GPU or screenshot behavior.
function fixture(){return {SchemaVersion:1,Frames:['original','orbit','edited','empty','resized','final'].map((Stage,i)=>({Stage,FrameId:i+1,Revision:i+1,MeshUploadId:[1,1,2,3,4,1][i],Width:i===4?600:800,Height:1000,PresentedTime:100+i})),BeforeDisposal:{PeakInFlightFrames:3,InFlightFrames:2},AfterDisposal:{IsDisposed:true,InFlightFrames:0,LiveMeshResources:0,LiveMeshBuffers:0,LastError:null},FinalView:{IsDisposed:false,InFlightFrames:0,LiveMeshBuffers:2,LastError:null,PresentedFrames:1},DisposalInFlight:2};}
test('report gate accepts complete timing/resource evidence',()=>assert.equal(verify(fixture()).stages.length,6));
test('report gate rejects source/presentation/retirement violations',()=>{
 for(const change of [r=>r.Frames[0].PresentedTime=0,r=>r.Frames[1].MeshUploadId=2,r=>r.AfterDisposal.LiveMeshBuffers=2,r=>r.FinalView.LastError='GPU failure',r=>r.Frames[4].Width=800]){const r=fixture();change(r);assert.throws(()=>verify(r));}
});
test('report gate rejects an over-capacity or negative outstanding-frame snapshot',()=>{
 for(const count of [-1,4]){const r=fixture();r.BeforeDisposal.InFlightFrames=count;assert.throws(()=>verify(r),/frame bound/);}
});
