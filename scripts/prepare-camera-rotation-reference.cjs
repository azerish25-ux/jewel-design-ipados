'use strict';
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const {extract}=require('./verify-value-channel-oracle.cjs');
const root=path.resolve(__dirname,'..');
const signatures=['double ON_Length2d( double x, double y )','double ON_2dVector::Length() const','bool ON_2dVector::Unitize()','double ON_Length3d(','double ON_3dVector::Length() const','bool ON_3dVector::Unitize()'];
async function prepare(acquire){
 const frame=await require('./prepare-camera-frame-reference.cjs').prepare(acquire);
 const rotation=await require('./prepare-rotation-reference.cjs').prepare();
 const point=fs.readFileSync(path.join(root,'.local/reference/opennurbs6-plane/opennurbs_point.cpp'),'utf8');
 const defs=fs.readFileSync(path.join(root,'.local/reference/opennurbs6-rotation/opennurbs_defines.h'),'utf8');
 const math=fs.readFileSync(path.join(root,'.local/reference/opennurbs6-camera/opennurbs_math.h'),'utf8');
 const macros=['ON_DBL_MIN','ON_EPSILON','ON_SQRT_EPSILON'].map(n=>defs.split('\n').find(x=>x.startsWith('#define '+n+' ')));
 macros.push(math.split('\n').find(x=>x.startsWith('#define ON_IS_FINITE(x)')&&x.includes('+ 3')));
 if(macros.some(x=>!x))throw Error('Missing source macro');
 const bodies=signatures.map(s=>extract(point,s));
 const code=macros.join('\n')+'\n'+bodies.join('\n')+'\n'+rotation.code;
 const sha=x=>crypto.createHash('sha256').update(x).digest('hex');
 const report={schemaVersion:1,pin:rotation.report.pin,pointSource:frame.files[1],rotationSource:rotation.report,methods:signatures.map((signature,i)=>({signature,sha256:sha(bodies[i])})),includeSha256:sha(code),scope:'Unchanged published6 arithmetic comparison; original5 static policy observed separately. Not original runtime evidence.'};
 const dir=path.join(root,'.local/reference/camera-rotation');fs.mkdirSync(dir,{recursive:true});fs.writeFileSync(path.join(dir,'camera-rotation-methods.inc'),code);fs.writeFileSync(path.join(dir,'source-manifest.json'),JSON.stringify(report,null,2)+'\n');return report;
}
if(require.main===module)prepare(process.argv.includes('--acquire')).then(x=>console.log(JSON.stringify(x,null,2))).catch(e=>{console.error(e.message);process.exitCode=1;});module.exports={prepare};
