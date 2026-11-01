'use strict';
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const {extract}=require('./verify-value-channel-oracle.cjs');
const root=path.resolve(__dirname,'..'),spec={
  "pin": "f2dc7fba67017475b8d530835a40fcfb5372134a",
  "files": [
    {
      "file": ".local/reference/opennurbs6-camera/opennurbs_viewport.cpp",
      "remote": "opennurbs_viewport.cpp",
      "sha256": "2403be6f12b9f3fa7c723da44793eda28c78e1e06d2ae7f2c529241c5b0498ce"
    },
    {
      "file": ".local/reference/opennurbs6-plane/opennurbs_point.cpp",
      "remote": "opennurbs_point.cpp",
      "sha256": "4aac860b778a37de2942f0bc665c655bed8f98e51e0c94beb542752a08558eeb"
    },
    {
      "file": ".local/reference/opennurbs6-rotation/opennurbs_defines.h",
      "remote": "opennurbs_defines.h",
      "sha256": "773d75ecf3dab2d792de490da99a175be548c394a81f11cd9a04cec5c1fe7bd6"
    },
    {
      "file": ".local/reference/opennurbs6-camera/opennurbs_math.h",
      "remote": "opennurbs_math.h",
      "sha256": "b3ecf4b86cf6b38af3fdf67f78ee5648136b3ceaffd5792620a647e2c879d673"
    }
  ],
  "methods": [
    {
      "signature": "bool ON_Viewport::IsTwoPointPerspectiveProjection() const",
      "sha256": "7e7179c250755df41880edaf6179b301b5626080ec57ca955f9d55d58f22ffee"
    },
    {
      "signature": "bool ON_Viewport::SetFrustum(\n      double frus_left,",
      "sha256": "c451a695bfa893dbd05b2587a3f9f53c3dbb16bfdbf6cc54bf68563ba85295ba"
    },
    {
      "signature": "bool ON_Viewport::GetFrustum(\n",
      "sha256": "34b04b2b03a2772757cc7348dcff69b88033cc5d9be06a5d1f20d8bb6b253f43"
    },
    {
      "signature": "bool ON_Viewport::GetFrustumAspect( double& frustum_aspect ) const",
      "sha256": "d0e8e229270ae87067cb0b031e42bd827cb945340c3c0c278786e4374ce44307"
    },
    {
      "signature": "bool ON_Viewport::SetCameraAngle( double angle )",
      "sha256": "47d17abbb284f2d6d8895f79205bda8f47bf1253c4315c06ef5f7fde14fbc371"
    },
    {
      "signature": "bool ON_Viewport::GetCamera35mmLensLength( double* lens_length ) const",
      "sha256": "f34c479b847af842fa36fb930118f5960753d345038d07168ff2e790617a2183"
    },
    {
      "signature": "bool ON_Viewport::SetCamera35mmLensLength( double lens_length )",
      "sha256": "89a65f0815e77b3e6d28c412a46c514fa3d566fce333e1fdf7ba5b8e49b72c6a"
    }
  ]
};
const sha=b=>crypto.createHash('sha256').update(b).digest('hex');
async function prepare(acquire){
 for(const file of spec.files){const target=path.join(root,file.file);if(!fs.existsSync(target)){if(!acquire)throw Error('Pinned source missing: '+file.file);const response=await fetch('https://raw.githubusercontent.com/mcneel/opennurbs/'+spec.pin+'/'+file.remote);if(!response.ok)throw Error('Source acquisition failed: '+response.status);const bytes=Buffer.from(await response.arrayBuffer());if(sha(bytes)!==file.sha256)throw Error('Acquired source hash differs');fs.mkdirSync(path.dirname(target),{recursive:true});fs.writeFileSync(target,bytes);}if(sha(fs.readFileSync(target))!==file.sha256)throw Error('Pinned source hash differs: '+file.file);}
 const texts=spec.files.map(f=>fs.readFileSync(path.join(root,f.file),'utf8').replaceAll('\r\n','\n'));
 const macros=[texts[2].split('\n').find(x=>x.startsWith('#define ON_PI ')&&x.includes('3.14159')),...['ON_SQRT_EPSILON','ON_UNSET_POSITIVE_VALUE','ON_UNSET_VALUE','ON_NONSENSE_WORLD_COORDINATE_VALUE'].map(name=>texts[2].split('\n').find(x=>x.startsWith('#define '+name+' '))),texts[3].split('\n').find(x=>x.startsWith('#define ON_IS_FINITE(x)')&&x.includes('+ 3')),texts[3].split('\n').find(x=>x.startsWith('#define ON_IS_VALID(x)'))];
 if(macros.some(x=>!x))throw Error('Expected source macro missing');
 const methods=spec.methods.map(m=>{const body=extract(texts[0],m.signature);if(sha(body)!==m.sha256)throw Error('Published camera body differs');return body;});
 const point=extract(texts[1],'bool ON_IsValid(double x)');
 const code=macros.join('\n')+'\n'+point+'\n'+methods.join('\n')+'\n';
 const dir=path.join(root,'.local/reference/frustum');fs.mkdirSync(dir,{recursive:true});fs.writeFileSync(path.join(dir,'frustum-methods.inc'),code);
 return {pin:spec.pin,sourceFiles:spec.files,methods:spec.methods,includeSha256:sha(code),finitePolicy:'Exact source little-endian IEEE macro, compiled with -fno-strict-aliasing; both unset sentinels retained.'};
}
if(require.main===module)prepare(process.argv.includes('--acquire')).then(x=>console.log(JSON.stringify(x,null,2))).catch(e=>{console.error(e.message);process.exitCode=1;});
module.exports={prepare};
