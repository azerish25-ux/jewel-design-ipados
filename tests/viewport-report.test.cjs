const test=require('node:test'),assert=require('node:assert/strict');
const {verify}=require('../scripts/check-viewport-report.cjs');
// These fixtures test report integrity, never native GPU or screenshot behavior.
function fixture(){return {SchemaVersion:2,Frames:['original','orbit','edited','empty','resized','final'].map((Stage,i)=>({Stage,FrameId:i+1,Revision:i+1,MeshUploadId:[1,1,2,3,4,1][i],Width:i===4?600:800,Height:1000,PresentedTime:100+i,SupportsNativePresentationTiming:true,EvidenceKind:"native-presented"})),BeforeDisposal:{PeakInFlightFrames:3,InFlightFrames:2},AfterDisposal:{IsDisposed:true,InFlightFrames:0,LiveMeshResources:0,LiveMeshBuffers:0,LastError:null},FinalView:{IsDisposed:false,InFlightFrames:0,LiveMeshBuffers:2,LastError:null,PresentedFrames:1,CompletedFrames:1},DisposalInFlight:2};}
test('report gate accepts complete timing/resource evidence',()=>assert.equal(verify(fixture()).stages.length,6));
test('report gate rejects source/presentation/retirement violations',()=>{
 for(const change of [r=>r.Frames[0].PresentedTime=0,r=>r.Frames[1].MeshUploadId=2,r=>r.AfterDisposal.LiveMeshBuffers=2,r=>r.FinalView.LastError='GPU failure',r=>r.Frames[4].Width=800]){const r=fixture();change(r);assert.throws(()=>verify(r));}
});
test('report gate rejects an over-capacity or negative outstanding-frame snapshot',()=>{
 for(const count of [-1,4]){const r=fixture();r.BeforeDisposal.InFlightFrames=count;assert.throws(()=>verify(r),/frame bound/);}
});

test('unavailable native timing stays explicitly unavailable and requires screen evidence',()=>{const r=fixture();for(const f of r.Frames){f.SupportsNativePresentationTiming=false;f.PresentedTime=null;f.EvidenceKind='gpu-completed';}r.FinalView.PresentedFrames=0;const proof=verify(r);assert.equal(proof.nativePresentationTimingVerified,false);assert.equal(proof.screenEvidenceRequired,true);assert.equal(proof.actualPresentation,undefined);});
test('GPU completion cannot impersonate a native presentation timestamp',()=>{for(const corrupt of [f=>f.PresentedTime=123,f=>f.EvidenceKind='native-presented']){const r=fixture();for(const f of r.Frames){f.SupportsNativePresentationTiming=false;f.PresentedTime=null;f.EvidenceKind='gpu-completed';}corrupt(r.Frames[0]);assert.throws(()=>verify(r));}});
