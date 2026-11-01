'use strict';
// Readiness for the dedicated CAD harness clear border only; full scene comparison remains mandatory.
const fs=require('node:fs'),crypto=require('node:crypto'),{decodePng}=require('./verify-metal-artifacts.cjs');
function check(frame){
 const {width,height,bgra}=frame||{};
 if(!Number.isInteger(width)||!Number.isInteger(height)||width<32||height<32||width>4096||height>4096||!bgra||bgra.length!==width*height*4)throw Error('Invalid capture surface');
 const left=Math.floor(width/4),right=Math.floor(width*3/4),top=Math.floor(height/8),bottom=Math.floor(height*7/8),thickness=Math.max(4,Math.floor(height/256));
 const probes=[{left,right,top,bottom:top+thickness},{left,right,top:bottom-thickness,bottom}],backgroundBgra=[15,11,9,255];
 let checkedPixels=0,blockedPixels=0;
 for(const box of probes)for(let y=box.top;y<box.bottom;y++)for(let x=box.left;x<box.right;x++){
  const i=(y*width+x)*4;checkedPixels++;
  if(Math.abs(bgra[i]-15)>3||Math.abs(bgra[i+1]-11)>3||Math.abs(bgra[i+2]-9)>3||bgra[i+3]!==255)blockedPixels++;
 }
 return {ready:blockedPixels===0,checkedPixels,blockedPixels,probes,backgroundBgra,scope:'Dedicated CAD harness clear-border readiness only; no cropping, image modification or full scene acceptance.'};
}
function verify(file){const bytes=fs.readFileSync(file);return {...check(decodePng(bytes)),pngSha256:crypto.createHash('sha256').update(bytes).digest('hex')};}
if(require.main===module)try{const result=verify(process.argv[2]);console.log(JSON.stringify(result,null,2));if(!result.ready)process.exitCode=2;}catch(e){console.error(e.message);process.exitCode=1;}module.exports={check,verify};
