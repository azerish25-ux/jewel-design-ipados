'use strict';
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto'),cp=require('node:child_process');
const OCCT_PIN='b8f597c677811d1f9f4d8a97f5ae2825c0353a42';
const targets={'iossimulator-arm64':'ios-sim-all.a','ios-arm64':'ios-device-all.a'};
const sha=bytes=>crypto.createHash('sha256').update(bytes).digest('hex');
function checkIdentity(value){
 if(!/^[1-9][0-9]*$/.test(value.runId))throw Error('Invalid runId');
 if(!Object.hasOwn(targets,value.platform))throw Error('Invalid platform');
 if(!/^[a-f0-9]{40}$/.test(value.sourceCommit))throw Error('Invalid sourceCommit');
 if(!/^[a-f0-9]{64}$/.test(value.nativeInputSha256))throw Error('Invalid nativeInputSha256');
 if(typeof value.xcode!=='string'||!value.xcode.startsWith('Xcode 26.6\nBuild version '))throw Error('Invalid xcode');
}
function checkArchive(bytes){if(bytes.length<8||bytes.subarray(0,8).toString('ascii')!=='!<arch>\n')throw Error('Invalid archive format');}
function pack(bytes,identity){
 checkIdentity(identity);checkArchive(bytes);
 return {schemaVersion:1,occtPin:OCCT_PIN,runId:identity.runId,sourceCommit:identity.sourceCommit,platform:identity.platform,nativeInputSha256:identity.nativeInputSha256,xcode:identity.xcode,archiveSha256:sha(bytes),archiveBytes:bytes.length};
}
function validate(bytes,manifest,expected){
 checkIdentity(expected);checkArchive(bytes);
 if(manifest.schemaVersion!==1)throw Error('Unsupported schemaVersion');
 if(manifest.occtPin!==OCCT_PIN)throw Error('Mismatched occtPin');
 checkIdentity(manifest);
 for(const key of ['runId','platform','nativeInputSha256','xcode'])if(manifest[key]!==expected[key])throw Error('Mismatched '+key);
 if(manifest.archiveSha256!==sha(bytes))throw Error('Mismatched archiveSha256');
 if(manifest.archiveBytes!==bytes.length)throw Error('Mismatched archiveBytes');
 return manifest;
}
function restore(root,bytes,manifest,expected){
 validate(bytes,manifest,expected);
 const target=path.join(root,'.local/native',targets[expected.platform]);
 fs.mkdirSync(path.dirname(target),{recursive:true});fs.writeFileSync(target,bytes);
 return target;
}
function nativeFingerprint(root){
 const files=cp.execFileSync('git',['ls-files','-z','--','native/GeometryBridge/CandidateProbe','scripts/prepare-rotation-reference.cjs','scripts/prepare-plane-reference.cjs','.github/workflows/native-geometry.yml','.github/workflows/native-device.yml'],{cwd:root,encoding:'utf8'}).split('\0').filter(Boolean).sort();
 if(files.length<10)throw Error('Missing native input inventory');
 const hash=crypto.createHash('sha256');
 for(const file of files){hash.update(file);hash.update('\0');hash.update(fs.readFileSync(path.join(root,file)));hash.update('\0');}
 return hash.digest('hex');
}
function main(args){
 const [command,platform,folder,producerRun]=args,root=path.resolve(__dirname,'..');
 const sourceCommit=cp.execFileSync('git',['rev-parse','HEAD'],{cwd:root,encoding:'utf8'}).trim();
 const xcode=fs.readFileSync(path.join(root,'artifacts/xcode.txt'),'utf8').trim();
 const identity={runId:command==='pack'?process.env.GITHUB_RUN_ID:producerRun,sourceCommit,platform,nativeInputSha256:nativeFingerprint(root),xcode};
 checkIdentity(identity);
 if(command==='pack'){
  if(fs.readFileSync(path.join(root,'artifacts/native-source.txt'),'utf8').trim()!==OCCT_PIN)throw Error('Built OCCT source differs from pin');
  const bytes=fs.readFileSync(path.join(root,'.local/native',targets[platform]));
  const dir=path.join(root,'artifacts/native-kernel-'+platform),manifest=pack(bytes,identity);
  fs.mkdirSync(dir,{recursive:true});fs.writeFileSync(path.join(dir,'candidate.a'),bytes);fs.writeFileSync(path.join(dir,'manifest.json'),JSON.stringify(manifest,null,2)+'\n');
  console.log(JSON.stringify(manifest,null,2));
 }else if(command==='restore'){
  const dir=path.resolve(root,folder),bytes=fs.readFileSync(path.join(dir,'candidate.a')),manifest=JSON.parse(fs.readFileSync(path.join(dir,'manifest.json'),'utf8'));
  const target=restore(root,bytes,manifest,identity);
  const proof={...manifest,consumerCommit:sourceCommit,consumerRunId:process.env.GITHUB_RUN_ID,scope:'Validated native archive reuse only; app compile/runtime still required.'};
  fs.mkdirSync(path.join(root,'artifacts'),{recursive:true});fs.writeFileSync(path.join(root,'artifacts/native-archive-reuse.json'),JSON.stringify(proof,null,2)+'\n');
  console.log('Validated native archive restored: '+path.relative(root,target));
 }else throw Error('Use pack PLATFORM or restore PLATFORM FOLDER PRODUCER_RUN');
}
if(require.main===module)try{main(process.argv.slice(2));}catch(error){console.error(error.message);process.exitCode=1;}
module.exports={pack,validate,restore,nativeFingerprint};
