'use strict';
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const input=path.resolve(process.argv[2]),output=path.resolve(process.argv[3]);
const fixture=JSON.parse(fs.readFileSync(path.join(input,'manifest.json'),'utf8'));
const lines=fs.readFileSync(path.join(output,'native.jsonl'),'utf8').trim().split(/\r?\n/).map(x=>JSON.parse(x));
if(lines.length!==fixture.fixtures.length)throw Error('Provider fixture count mismatch');
const results=[];
for(const expected of fixture.fixtures){
 const encoded=fs.readFileSync(path.join(input,expected.name));
 if(crypto.createHash("sha256").update(encoded).digest("hex")!==expected.encodedSha256)throw Error("Encoded fixture hash drift");
 const item=lines.find(x=>x.file===expected.name);
 if(!item||item.width!==expected.width||item.height!==expected.height||item.sourceStatus!==0)throw Error('Provider metadata/status differs');
 const raw=fs.readFileSync(path.join(output,item.file+'.provider.bin'));
 if(raw.length!==item.providerBytes||!Number.isSafeInteger(item.bytesPerRow)||item.bytesPerRow<1)throw Error('Provider length/stride invalid');
 let directRgbaPreserved=null,directRgbPreserved=null;
 if(expected.expectedRgba&&item.bitsPerComponent===8){
  const wanted=Buffer.from(expected.expectedRgba);
  if(item.bitsPerPixel===32&&item.bytesPerRow>=item.width*4&&raw.length>=item.bytesPerRow*item.height){
   const packed=Buffer.concat(Array.from({length:item.height},(_,y)=>raw.subarray(y*item.bytesPerRow,y*item.bytesPerRow+item.width*4)));
   directRgbaPreserved=packed.equals(wanted);
  }
  if(item.bitsPerPixel===24&&item.bytesPerRow>=item.width*3&&raw.length>=item.bytesPerRow*item.height){
   const packed=Buffer.concat(Array.from({length:item.height},(_,y)=>raw.subarray(y*item.bytesPerRow,y*item.bytesPerRow+item.width*3)));
   const rgb=Buffer.from(expected.expectedRgba.filter((_,i)=>i%4!==3));directRgbPreserved=packed.equals(rgb);
  }
 }
 results.push({...item,encodedSha256:expected.encodedSha256,providerSha256:crypto.createHash('sha256').update(raw).digest('hex'),sampleHex:raw.subarray(0,64).toString('hex'),directRgbaPreserved,directRgbPreserved});
}
console.log(JSON.stringify({schemaVersion:1,status:'native_provider_observation_only',results,scope:'Native macOS ImageIO provider bytes, not original LEADTOOLS conversion or iOS provider/production adapter parity. Null comparisons mean unsupported observation layout, not successful conversion.'},null,2));
