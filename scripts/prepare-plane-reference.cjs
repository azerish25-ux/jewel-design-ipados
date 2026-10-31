'use strict';
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');const {extract}=require('./verify-value-channel-oracle.cjs');const root=path.resolve(__dirname,'..'),folder=path.join(root,'.local/reference/opennurbs6-plane'),pin='f2dc7fba67017475b8d530835a40fcfb5372134a';
const specs=[
  {
    "file": "opennurbs_point.cpp",
    "sha256": "4aac860b778a37de2942f0bc665c655bed8f98e51e0c94beb542752a08558eeb",
    "methods": [
      {
        "signature": "double ON_Length3d("
      },
      {
        "signature": "double ON_3dVector::Length() const"
      },
      {
        "signature": "bool ON_3dVector::Unitize()"
      },
      {
        "signature": "double ON_Length2d("
      },
      {
        "signature": "double ON_2dVector::Length() const"
      },
      {
        "signature": "bool ON_2dVector::Unitize()"
      },
      {
        "signature": "bool ON_3dVector::PerpendicularTo( const ON_3dVector& v )"
      },
      {
        "signature": "ON_3dVector::PerpendicularTo( \n      const ON_3dPoint& P0",
        "prefix": "bool\n"
      }
    ]
  },
  {
    "file": "opennurbs_plane.cpp",
    "sha256": "a33e6973e8b36fabb65d7534dcc83b3637f23d05bce776c4ea28359b9c786748",
    "methods": [
      {
        "signature": "bool ON_Plane::CreateFromFrame("
      },
      {
        "signature": "bool ON_Plane::CreateFromPoints("
      }
    ]
  },
  {
    "file": "opennurbs_line.cpp",
    "sha256": "1453dba5195c8dd94ca0c3c3ee3aff6fa483b8007f70f69cb817e41101528adb",
    "methods": [
      {
        "signature": "bool ON_Line::InPlane("
      }
    ]
  },
  {
    "file": "opennurbs_nurbscurve.cpp",
    "sha256": "9dfe9df60d6703bc8bfe636e46b6c82095694d7aca713e471f49e0432cc6c11a",
    "methods": [
      {
        "signature": "ON_NurbsCurve::IsLinear(",
        "prefix": "bool\n"
      },
      {
        "signature": "ON_NurbsCurve::IsPlanar(",
        "prefix": "bool\n"
      }
    ]
  },
  {
    "file": "LICENSE",
    "sha256": "de437cd429817f50999710543b9d3e743b17b35b4c96842393b9d39f7bb9b3bd",
    "methods": []
  }
];
const sha=b=>crypto.createHash('sha256').update(b).digest('hex');
function validate(file,bytes){const spec=specs.find(x=>x.file===file);if(!spec||sha(bytes)!==spec.sha256)throw Error('Pinned plane reference hash mismatch: '+file);return true;}
function build(files){const methods=[];for(const spec of specs){validate(spec.file,files[spec.file]);const text=files[spec.file].toString('utf8').replaceAll('\r\n','\n');for(const m of spec.methods){const body=extract(text,m.signature);methods.push({file:spec.file,signature:m.signature,body,prefix:m.prefix??'',sha256:sha(body)});}}const first=files['opennurbs_point.cpp'].toString('utf8').replaceAll('\r\n','\n');const code=first.slice(0,first.indexOf('#include'))+'\n// Unmodified published comparison methods; observation types are test-only.\n'+methods.map(m=>m.prefix+m.body).join('\n\n')+'\n';return {code,report:{schemaVersion:1,pin,sourceRepository:'https://github.com/mcneel/opennurbs',files:specs.map(({methods,...s})=>({...s,url:'https://raw.githubusercontent.com/mcneel/opennurbs/'+pin+'/'+s.file})),methods:methods.map(({body,prefix,...m})=>m),generatedSha256:sha(code),scope:'Test-only published6 method comparison, not original Rhino5 runtime;3D inputs/positive1e-12 tolerance only.'}};}
async function prepare(){if(process.platform==='win32'&&!path.parse(root).root.toUpperCase().startsWith('D:'))throw Error('Local reference cache must stay D:');fs.mkdirSync(folder,{recursive:true});const files={};for(const spec of specs){const file=path.join(folder,spec.file);if(fs.existsSync(file))files[spec.file]=fs.readFileSync(file);else{const r=await fetch('https://raw.githubusercontent.com/mcneel/opennurbs/'+pin+'/'+spec.file,{signal:AbortSignal.timeout(30000)});if(!r.ok)throw Error('Plane acquisition HTTP '+r.status);const b=Buffer.from(await r.arrayBuffer());if(b.length>2*1024*1024)throw Error('Plane reference size bound');validate(spec.file,b);fs.writeFileSync(file,b);files[spec.file]=b;}}const result=build(files);fs.writeFileSync(path.join(folder,'plane-methods.inc'),result.code);fs.writeFileSync(path.join(folder,'plane-source-manifest.json'),JSON.stringify(result.report,null,2)+'\n');return result;}
if(require.main===module)prepare().then(x=>console.log(JSON.stringify(x.report,null,2))).catch(e=>{console.error(e.message);process.exitCode=1;});module.exports={pin,specs,validate,build,prepare};
