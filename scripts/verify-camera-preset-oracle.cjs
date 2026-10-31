'use strict';
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const {extract}=require('./verify-value-channel-oracle.cjs');
const root=path.resolve(__dirname,'..'),source='recovery/canonical/cad/CS/CSUtil.cs',target='tests/Port/Geometry.Candidate.Contracts/Recovered/LegacyCameraPresets.cs';
const sha=x=>crypto.createHash('sha256').update(x).digest('hex');
function build(){
 const bytes=fs.readFileSync(path.join(root,source));
 if(sha(bytes)!=='421af2e1725afe8571b3c407104ea04d32eee64318857c072b1f3cef6b1881b5')throw Error('Camera preset source hash changed');
 const body=extract(bytes.toString('utf8'),'public static void DocumentSetCameraView(string cameraViewName)');
 const enumBytes=fs.readFileSync(path.join(root,'recovery/canonical/cad/CS/CameraView.cs'));
 if(sha(enumBytes)!=='1130791b078e2af7d7bf1e0db567ce7bc879486b0f49c7cc7d76b471e2a66cf3')throw Error('Camera enum source hash changed');
 const enumBody=extract(enumBytes.toString('utf8'),'public enum CameraView');
 const support=fs.readFileSync(path.join(root,'tests/Port/Geometry.Candidate.Contracts/Recovered/CameraObservationTypes.txt'),'utf8').replaceAll('\r\n','\n');
 const code='#nullable disable\n// Exact recovered wrapper with observation-only viewport types; no native projection implementation.\nusing System;\nusing System.Collections.Generic;\nusing System.Linq;\nnamespace CandidateGeometryContractChecks.Recovered.CameraPresets;\n'+enumBody+'\n'+support+'\ninternal static class CSUtil\n{\nprivate static void Redraw()=>Observation.Trace.Add("redraw");\n'+body+'\n}\n';
 return {code,source,sourceSha256:sha(bytes),bodySha256:sha(body),supportSha256:sha(support),enumSourceSha256:sha(enumBytes),target};
}
function verify(code){if(code.replaceAll('\r\n','\n')!==build().code)throw Error('Recovered camera wrapper differs from canonical');return true;}
if(require.main===module){const result=build();if(process.argv.includes('--write')){fs.mkdirSync(path.dirname(path.join(root,target)),{recursive:true});fs.writeFileSync(path.join(root,target),result.code);}verify(fs.readFileSync(path.join(root,target),'utf8'));const {code,...report}=result;console.log(JSON.stringify(report,null,2));}
module.exports={build,verify};
