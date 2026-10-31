'use strict';
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const {decodePng}=require('./verify-metal-artifacts.cjs');
const names=['original','orbit','edited','empty','resized','final'];
function verifyPixels(frames){
 const {width,height}=frames.original;
 for(const name of names)if(!frames[name]||frames[name].width!==width||frames[name].height!==height||frames[name].bgra.length!==width*height*4)throw Error('Screen dimensions differ: '+name);
 const bounds={left:Math.floor(width/8),top:Math.floor(height/8),right:Math.floor(width*7/8),bottom:Math.floor(height*7/8)};
 const background=frames.empty.bgra,first=(bounds.top*width+bounds.left)*4;
 let uniformPixels=0;
 for(let y=bounds.top;y<bounds.bottom;y++)for(let x=bounds.left;x<bounds.right;x++){
  const index=(y*width+x)*4;
  for(let c=0;c<3;c++)if(Math.abs(background[index+c]-background[first+c])>3)throw Error('Empty CAD screen region is not uniform');
  uniformPixels++;
 }
 function different(a,b){let count=0;for(let y=bounds.top;y<bounds.bottom;y++)for(let x=bounds.left;x<bounds.right;x++){const i=(y*width+x)*4;if(Math.abs(a[i]-b[i])>3||Math.abs(a[i+1]-b[i+1])>3||Math.abs(a[i+2]-b[i+2])>3)count++;}return count;}
 const original=frames.original.bgra;
 const result={width,height,bounds,uniformPixels,foregroundPixels:different(original,background),editedPixels:different(original,frames.edited.bgra),orbitPixels:different(original,frames.orbit.bgra),resizedPixels:different(original,frames.resized.bgra),finalDifferencePixels:different(original,frames.final.bgra)};
 for(const key of ['foregroundPixels','editedPixels','orbitPixels','resizedPixels'])if(result[key]<=100)throw Error('Visible scene change missing: '+key);
 if(result.finalDifferencePixels>32)throw Error('Fresh final viewport did not reproduce original CAD screen');
 return result;
}
function verify(folder){
 const frames={},pngSha256={};
 for(const name of names){const bytes=fs.readFileSync(path.join(folder,name+'.png'));frames[name]=decodePng(bytes);pngSha256[name]=crypto.createHash('sha256').update(bytes).digest('hex');}
 return {...verifyPixels(frames),pngSha256,scope:'Central screen-region scene consistency only; original-PC image quality and native presentation timestamps are separate acceptance.'};
}
if(require.main===module)try{console.log(JSON.stringify(verify(process.argv[2]),null,2));}catch(error){console.error(error.message);process.exitCode=1;}
module.exports={verifyPixels,verify};
