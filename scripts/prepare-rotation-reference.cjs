'use strict';
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const {extract}=require('./verify-value-channel-oracle.cjs');
const root=path.resolve(__dirname,'..'),folder=path.join(root,'.local/reference/opennurbs6-rotation');
const pin='f2dc7fba67017475b8d530835a40fcfb5372134a';
const specs=[
 {file:'opennurbs_xform.cpp',sha256:'7b922be97b2d05c50cd5865e32826d87fc2f4926304f55f21bf9344478580258'},
 {file:'opennurbs_defines.h',sha256:'773d75ecf3dab2d792de490da99a175be548c394a81f11cd9a04cec5c1fe7bd6'},
 {file:'LICENSE',sha256:'de437cd429817f50999710543b9d3e743b17b35b4c96842393b9d39f7bb9b3bd'}
];
const sha=b=>crypto.createHash('sha256').update(b).digest('hex');
function validate(file,bytes){const spec=specs.find(x=>x.file===file);if(!spec||sha(bytes)!==spec.sha256)throw Error('Pinned rotation comparison hash mismatch: '+file);return true;}
function build(files){
 for(const spec of specs)validate(spec.file,files[spec.file]);
 const source=files['opennurbs_xform.cpp'].toString('utf8').replaceAll('\r\n','\n');
 const signatures=['void ON_Xform::Rotation( \n        double angle,','void ON_Xform::Rotation(  \n        double sin_angle,'];
 const methods=signatures.map(signature=>extract(source,signature));
 const header=source.slice(0,source.indexOf('#include'));
 const code=header+'\n// Unmodified published comparison methods; test observation types are declared by rotation_reference.cpp.\n'+methods.join('\n\n')+'\n';
 const report={schemaVersion:1,pin,sourceRepository:'https://github.com/mcneel/opennurbs',files:specs.map(x=>({...x,url:'https://raw.githubusercontent.com/mcneel/opennurbs/'+pin+'/'+x.file})),methods:signatures.map((signature,i)=>({signature,sha256:sha(methods[i])})),generatedSha256:sha(code),scope:'Published openNURBS6 comparison only, not original Rhino5 binary/runtime parity. No production runtime dependency.'};
 return {code,report};
}
async function prepare(){
 if(process.platform==='win32'&&!path.parse(root).root.toUpperCase().startsWith('D:'))throw Error('Local reference cache must stay on D:');
 fs.mkdirSync(folder,{recursive:true});const files={};
 for(const spec of specs){
  const file=path.join(folder,spec.file);
  if(fs.existsSync(file))files[spec.file]=fs.readFileSync(file);
  else{const response=await fetch('https://raw.githubusercontent.com/mcneel/opennurbs/'+pin+'/'+spec.file,{signal:AbortSignal.timeout(30000)});if(!response.ok)throw Error('Reference acquisition HTTP '+response.status);const bytes=Buffer.from(await response.arrayBuffer());if(bytes.length>2*1024*1024)throw Error('Reference size bound');validate(spec.file,bytes);fs.writeFileSync(file,bytes);files[spec.file]=bytes;}
 }
 const result=build(files);fs.writeFileSync(path.join(folder,'rotation-methods.inc'),result.code);fs.writeFileSync(path.join(folder,'source-manifest.json'),JSON.stringify(result.report,null,2)+'\n');return result;
}
if(require.main===module)prepare().then(x=>console.log(JSON.stringify(x.report,null,2))).catch(error=>{console.error(error.message);process.exitCode=1;});
module.exports={pin,specs,validate,build,prepare};
