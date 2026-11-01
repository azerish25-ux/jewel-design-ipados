'use strict';
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const {extract}=require('./verify-value-channel-oracle.cjs'),root=path.resolve(__dirname,'..');
const signatures=['ON_Xform::ON_Xform()','ON_Xform ON_Xform::operator*( const ON_Xform& rhs ) const','void ON_Xform::WorldToCamera(','void ON_Xform::CameraToWorld(','bool ON_Xform::CameraToClip(','bool ON_Xform::ClipToCamera(','bool ON_Xform::ClipToScreen(','bool ON_Xform::ScreenToClip(','bool ON_Xform::IsIdentity('];
async function prepare(acquire){
 const matrix=await require('./prepare-camera-matrix-reference.cjs').prepare(acquire),frame=await require('./prepare-camera-frame-reference.cjs').prepare(acquire);
 const xf=fs.readFileSync(path.join(root,'.local/reference/opennurbs6-rotation/opennurbs_xform.cpp'),'utf8').replaceAll('\r\n','\n'),vp=fs.readFileSync(path.join(root,'.local/reference/opennurbs6-camera/opennurbs_viewport.cpp'),'utf8');
 const defs=fs.readFileSync(path.join(root,'.local/reference/opennurbs6-rotation/opennurbs_defines.h'),'utf8');const macro=defs.split('\n').find(x=>x.startsWith('#define ON_UNSET_POSITIVE_VALUE '));if(!macro)throw Error('Missing identity macro');
 const methods=signatures.map(s=>extract(xf,s));methods.push(extract(vp,'bool ON_Viewport::IsPerspectiveProjection() const'),extract(vp,'bool ON_Viewport::GetXform('));
 const code=xf.slice(0,xf.indexOf('#include'))+'\n'+macro+'\n'+methods.join('\n')+'\n',sha=x=>crypto.createHash('sha256').update(x).digest('hex');
 const report={schemaVersion:1,pin:matrix.pin,xformSourceSha256:matrix.sourceSha256,viewportSource:frame.files[0],methods:[...signatures,'bool ON_Viewport::IsPerspectiveProjection() const','bool ON_Viewport::GetXform('].map((signature,i)=>({signature,sha256:sha(methods[i])})),includeSha256:sha(code),scope:'Unchanged full published6 GetXform/matrix arithmetic; reference exercises world-to-clip/cache extra product only. Original5 static policy separately reviewed; no original runtime execution.'};
 const dir=path.join(root,'.local/reference/viewport-cache');fs.mkdirSync(dir,{recursive:true});fs.writeFileSync(path.join(dir,'viewport-cache-methods.inc'),code);fs.writeFileSync(path.join(dir,'source-manifest.json'),JSON.stringify(report,null,2)+'\n');return report;
}
if(require.main===module)prepare(process.argv.includes('--acquire')).then(x=>console.log(JSON.stringify(x,null,2))).catch(e=>{console.error(e.message);process.exitCode=1;});module.exports={prepare};
