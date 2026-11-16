'use strict';
const {createHash}=require('node:crypto');
const fs=require('node:fs');
const path=require('node:path');
const audit=JSON.parse(fs.readFileSync(path.join(__dirname,'../docs/evidence/audit/rhino5-wallpaper-raster-source.json'),'utf8'));
const plane=Buffer.alloc(256*256*3),hash=createHash('sha256');
for(let r=0;r<256;r++){
 let k=0;
 for(let b=0;b<256;b++)for(let g=0;g<256;g++){
  const gray=Math.trunc((r*0.299+g*0.589)+b*0.111);
  plane[k++]=gray;plane[k++]=gray;plane[k++]=gray;
 }
 hash.update(plane);
}
const sha256=hash.digest('hex');
if(sha256!==audit.golden.sha256)throw new Error('Wallpaper raster golden drift');
console.log(JSON.stringify({schemaVersion:1,triples:16777216,sha256,originalTargetExecuted:false,scope:'Source-derived ordered arithmetic, not original runtime/GDI pixel oracle'},null,2));
