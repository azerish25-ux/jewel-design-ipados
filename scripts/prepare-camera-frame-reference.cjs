'use strict';
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const {extract}=require('./verify-value-channel-oracle.cjs');
const root=path.resolve(__dirname,'..'),spec={
  "pin": "f2dc7fba67017475b8d530835a40fcfb5372134a",
  "files": [
    {
      "local": ".local/reference/opennurbs6-camera/opennurbs_viewport.cpp",
      "remote": "opennurbs_viewport.cpp",
      "sha256": "2403be6f12b9f3fa7c723da44793eda28c78e1e06d2ae7f2c529241c5b0498ce"
    },
    {
      "local": ".local/reference/opennurbs6-plane/opennurbs_point.cpp",
      "remote": "opennurbs_point.cpp",
      "sha256": "4aac860b778a37de2942f0bc665c655bed8f98e51e0c94beb542752a08558eeb"
    },
    {
      "local": ".local/reference/opennurbs6-rotation/opennurbs_defines.h",
      "remote": "opennurbs_defines.h",
      "sha256": "773d75ecf3dab2d792de490da99a175be548c394a81f11cd9a04cec5c1fe7bd6"
    },
    {
      "local": ".local/reference/opennurbs6-camera/opennurbs_math.h",
      "remote": "opennurbs_math.h",
      "sha256": "b3ecf4b86cf6b38af3fdf67f78ee5648136b3ceaffd5792620a647e2c879d673"
    }
  ],
  "pointMethods": [
    {
      "signature": "bool ON_IsValid(double x)",
      "sha256": "18c69d3b924f27669bf7659cf62b74c976c011290eed4dee123f276efb0ae26b"
    },
    {
      "signature": "ON_3dVector ON_3dVector::operator-() const",
      "sha256": "e92a1852852e0f05160d9d18e59496ad1ba4f2b2ee0df0df04eef027e7983f6d"
    },
    {
      "signature": "double ON_3dVector::operator*( const ON_3dVector& v ) const",
      "sha256": "b1987163d9e95badef2991c963a093d5c583675dc7ed1e1c88083362cc581ff5"
    },
    {
      "signature": "ON_3dVector ON_3dVector::operator+( const ON_3dVector& v ) const",
      "sha256": "ec68d16363a4ba87a89d8c05a6b3962fa59a22489b1071785497e937809b0ffd"
    },
    {
      "signature": "ON_3dVector ON_3dVector::operator-( const ON_3dVector& v ) const",
      "sha256": "14bddeb92336154ed100b42b3b3f1861b4f3e4d1ea43668a759a4313cea269b4"
    },
    {
      "signature": "ON_3dVector operator*(double d, const ON_3dVector& v)",
      "sha256": "50c2005bf10bedaef66d8bd9f6b73b81e06aaacab0383f57f3a22ef07866f5ab"
    },
    {
      "signature": "ON_3dVector ON_CrossProduct( const ON_3dVector& a , const ON_3dVector& b )",
      "sha256": "1c8afbc4915a584622ec7d8e1121dc0bb40f9f8bdec54d83b2ea7bb2b79db019"
    },
    {
      "signature": "bool ON_3dVector::operator==( const ON_3dVector& v ) const",
      "sha256": "494cf19f5ae199b232e9d5ef436b90937622d63199f2f692d19d62686c159b8c"
    },
    {
      "signature": "bool ON_3dPoint::operator==( const ON_3dPoint& v ) const",
      "sha256": "b7c27a269073e00b2bb49b1684cf775c755694ce9b0d439c231aeddb4318f1e9"
    },
    {
      "signature": "double ON_Length3d(",
      "sha256": "8fa6a44f3f91bb7beee5890ac084e663891390f796ae4fa69f3e4c4671043f03"
    },
    {
      "signature": "double ON_3dVector::Length() const",
      "sha256": "c4bcde221dcd92aafb76e865bca21fe57fa69eb59b890db2963101a02289058b"
    },
    {
      "signature": "bool ON_3dVector::Unitize()",
      "sha256": "d5b1ebb8bce26d3b7285273900d24c603179f06446298f5d5996c7222db3dfd2"
    },
    {
      "signature": "bool ON_3dVector::IsValid() const",
      "sha256": "931ef3ca8b0f801fae18d6ed5e2a14d49b8a893a0ccf717e98a554951a2cbd89"
    },
    {
      "signature": "bool ON_3dPoint::IsValid() const",
      "sha256": "c4c3100752811710752629fe7b307c416dfb29daa990080863af19fc7067111c"
    },
    {
      "signature": "double ON_3dVector::MaximumCoordinate() const",
      "sha256": "d69b465f3160883525f7985ea7d59d613f161ffced74d0ddc36d919c6ac36871"
    },
    {
      "signature": "double ON_3dPoint::MaximumCoordinate() const",
      "sha256": "266f9d126fb9082fb7153eb6a14fd5810738e95b4832c4cf1a108581c326e285"
    }
  ],
  "viewportMethods": [
    {
      "signature": "bool ON__IsCameraFrameUnitVectorHelper(",
      "sha256": "81a106c33fa9f25491f87a67fa1e667a9149f5933dcfb3fa30700d8631f96c48"
    },
    {
      "signature": "bool ON__IsCameraFramePerpindicular(",
      "sha256": "a64dfe86b84ba97c1a2bf4046c4269143f06e8fdcf092258399360f274115e2f"
    },
    {
      "signature": "bool ON_Viewport::IsValidCameraLocation(",
      "sha256": "fef57f919cded1f3b08fdfc61b0fb50441fd7074e61713c214d3ecdf92a7271d"
    },
    {
      "signature": "bool ON_Viewport::IsValidCameraUpOrDirection(",
      "sha256": "4662003b7dd8cf15de783c7a85e2ccea37adeb639e7c9b514bee7c82460daca6"
    },
    {
      "signature": "bool ON_Viewport::SetCameraLocation(",
      "sha256": "dca45147e80bb69dfa2abbc7dfce0a5e350f789b1d52e3cfd54c65558ff39605"
    },
    {
      "signature": "bool ON_Viewport::SetCameraDirection(",
      "sha256": "6608151a36808f5ba4b0c900d111c7591380d38ecb47f9e8e7aaba58f88e2d86"
    },
    {
      "signature": "bool ON_Viewport::SetCameraUp(",
      "sha256": "64adf4b03146a9103ef0210478670d280909ad50a3421ae358ff2257d58bb53f"
    },
    {
      "signature": "static bool Internal_SetCameraFameFailed()",
      "sha256": "f325897a5658a8bfe9924f8dececbde8dbbeba2183290574272ee4b1a41a68a5"
    },
    {
      "signature": "bool ON_Viewport::SetCameraFrame()",
      "sha256": "c4a746af569c8fb6db35d4f145f5af6a6fea019c4e27fb18d9fea30345ccded2"
    }
  ]
};
const sha=b=>crypto.createHash('sha256').update(b).digest('hex');
async function prepare(acquire){
 for(const f of spec.files){const file=path.join(root,f.local);if(!fs.existsSync(file)){if(!acquire)throw Error('Pinned frame source missing: '+f.local);const response=await fetch('https://raw.githubusercontent.com/mcneel/opennurbs/'+spec.pin+'/'+f.remote);if(!response.ok)throw Error('Source acquisition failed '+response.status);const bytes=Buffer.from(await response.arrayBuffer());if(sha(bytes)!==f.sha256)throw Error('Acquired frame source hash differs');fs.mkdirSync(path.dirname(file),{recursive:true});fs.writeFileSync(file,bytes);}if(sha(fs.readFileSync(file))!==f.sha256)throw Error('Pinned frame source bytes differ: '+f.local);}
 const texts=spec.files.map(f=>fs.readFileSync(path.join(root,f.local),'utf8').replaceAll('\r\n','\n'));
 const macros=['ON_DBL_MIN','ON_ZERO_TOLERANCE','ON_UNSET_POSITIVE_VALUE','ON_UNSET_VALUE','ON_NONSENSE_WORLD_COORDINATE_VALUE'].map(name=>texts[2].split('\n').find(x=>x.startsWith('#define '+name+' ')));
 macros.push(texts[3].split('\n').find(x=>x.startsWith('#define ON_IS_FINITE(x)')&&x.includes('+ 3')),texts[3].split('\n').find(x=>x.startsWith('#define ON_IS_VALID(x)')));if(macros.some(x=>!x))throw Error('Required frame macro missing');
 function bodies(list,text){return list.map(m=>{const body=extract(text,m.signature);if(sha(body)!==m.sha256)throw Error('Frame source body differs: '+m.signature);return body;}).join('\n');}
 const code=macros.join('\n')+'\n'+bodies(spec.pointMethods,texts[1])+'\nconst ON_3dPoint ON_3dPoint::UnsetPoint(ON_UNSET_VALUE,ON_UNSET_VALUE,ON_UNSET_VALUE);\nconst ON_3dVector ON_3dVector::UnsetVector(ON_UNSET_VALUE,ON_UNSET_VALUE,ON_UNSET_VALUE);\n'+bodies(spec.viewportMethods,texts[0])+'\n';
 const dir=path.join(root,'.local/reference/camera-frame');fs.mkdirSync(dir,{recursive:true});fs.writeFileSync(path.join(dir,'camera-frame-methods.inc'),code);return {...spec,includeSha256:sha(code),scope:'Unchanged published frame/setter/vector methods; observation-only containers/cache/diagnostic counters. No private viewport/GPU/PC claim.'};
}
if(require.main===module)prepare(process.argv.includes('--acquire')).then(x=>console.log(JSON.stringify(x,null,2))).catch(e=>{console.error(e.message);process.exitCode=1;});module.exports={prepare};
