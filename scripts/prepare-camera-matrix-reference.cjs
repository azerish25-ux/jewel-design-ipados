'use strict';
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const {extract}=require('./verify-value-channel-oracle.cjs');
const root=path.resolve(__dirname,'..'),pin='f2dc7fba67017475b8d530835a40fcfb5372134a',source='.local/reference/opennurbs6-rotation/opennurbs_xform.cpp';
const expected='7b922be97b2d05c50cd5865e32826d87fc2f4926304f55f21bf9344478580258';
const specs=[
  {
    "signature": "void ON_Xform::WorldToCamera(",
    "sha256": "577b2e5863c8336125e4e1f7cadef2e06bf173b4adaaf0c33c4a118e465a8a62"
  },
  {
    "signature": "void ON_Xform::CameraToWorld(",
    "sha256": "4dcc20033518dca86eb998475b51eb7f157d453686f4f1d67540e7c4d12589b4"
  },
  {
    "signature": "bool ON_Xform::CameraToClip(",
    "sha256": "4959c33451844d38e28eda791d8ede5b204f3d7a389e72e1d2ea2dad9cd98f3b"
  }
];
const sha=b=>crypto.createHash('sha256').update(b).digest('hex');
async function prepare(acquire){
 const file=path.join(root,source);if(!fs.existsSync(file)){if(!acquire)throw Error('Pinned xform source missing');const response=await fetch('https://raw.githubusercontent.com/mcneel/opennurbs/'+pin+'/opennurbs_xform.cpp');if(!response.ok)throw Error('Source acquisition failed '+response.status);const bytes=Buffer.from(await response.arrayBuffer());if(sha(bytes)!==expected)throw Error('Xform source hash differs');fs.mkdirSync(path.dirname(file),{recursive:true});fs.writeFileSync(file,bytes);}
 const bytes=fs.readFileSync(file);if(sha(bytes)!==expected)throw Error('Pinned xform bytes differ');
 const bodies=specs.map(s=>{const body=extract(bytes.toString('utf8'),s.signature);if(sha(body)!==s.sha256)throw Error('Camera matrix source body differs');return body;});
 const dir=path.join(root,'.local/reference/camera-matrix');fs.mkdirSync(dir,{recursive:true});const code=bodies.join('\n')+'\n';fs.writeFileSync(path.join(dir,'camera-matrix-methods.inc'),code);
 return {pin,file:'opennurbs_xform.cpp',sourceSha256:expected,methods:specs,includeSha256:sha(code),scope:'Exact published numeric methods with observation matrix/point/vector containers; no private viewport/GPU claim.'};
}
if(require.main===module)prepare(process.argv.includes('--acquire')).then(x=>console.log(JSON.stringify(x,null,2))).catch(e=>{console.error(e.message);process.exitCode=1;});module.exports={prepare};
