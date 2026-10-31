'use strict';
// Bounded evidence decoder: non-interlaced 8-bit RGBA only, not a product image codec.
// PNG chunk CRC and scanline filters: https://www.w3.org/TR/png-3/
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto'),zlib=require('node:zlib');
const signature=Buffer.from([137,80,78,71,13,10,26,10]);
const sha=bytes=>crypto.createHash('sha256').update(bytes).digest('hex');
function paeth(a,b,c){
 const p=a+b-c,pa=Math.abs(p-a),pb=Math.abs(p-b),pc=Math.abs(p-c);
 return pa<=pb&&pa<=pc?a:pb<=pc?b:c;
}
function decodePng(bytes){
 if(bytes.length<8||!bytes.subarray(0,8).equals(signature))throw Error('PNG signature mismatch');
 if(bytes.length>128*1024*1024)throw Error('PNG file exceeds evidence bound');
 let header=null,ended=false,dataEnded=false;const chunks=[];
 for(let offset=8;offset<bytes.length;){
  if(bytes.length-offset<12)throw Error('Truncated PNG chunk');
  const length=bytes.readUInt32BE(offset),type=bytes.toString('ascii',offset+4,offset+8),end=offset+12+length;
  if(end>bytes.length)throw Error('Truncated PNG chunk data');
  if(zlib.crc32(bytes.subarray(offset+4,end-4))!==bytes.readUInt32BE(end-4))throw Error('PNG CRC mismatch: '+type);
  const data=bytes.subarray(offset+8,end-4);
  if(type==='IHDR'){
   if(header||offset!==8||length!==13)throw Error('Invalid PNG IHDR order');
   const width=data.readUInt32BE(0),height=data.readUInt32BE(4),channels=data[9]===6?4:0;
   if(width<1||height<1||width>4096||height>4096)throw Error('PNG dimensions outside evidence bound');
   if(data[8]!==8||!channels||data[10]!==0||data[11]!==0||data[12]!==0)throw Error('Unsupported evidence PNG encoding');
   header={width,height,channels};
  }else if(type==='IDAT'){
   if(!header||dataEnded)throw Error('Invalid PNG IDAT order');chunks.push(data);
  }else if(type==='IEND'){
   if(length!==0||end!==bytes.length)throw Error('PNG IEND/trailing bytes invalid');ended=true;break;
  }else{
   if(!header)throw Error('PNG IHDR missing');
   if(['acTL','fcTL','fdAT','tRNS'].includes(type))throw Error('Unsupported animated/transparency PNG evidence');
   if(chunks.length)dataEnded=true;
   if(type[0]===type[0].toUpperCase()&&type!=='PLTE')throw Error('Unsupported critical PNG chunk '+type);
  }
  offset=end;
 }
 if(!header||!ended||!chunks.length)throw Error('Incomplete PNG image');
 const {width,height,channels}=header,rowBytes=width*channels,expected=(rowBytes+1)*height;
 const raw=zlib.inflateSync(Buffer.concat(chunks),{maxOutputLength:expected});
 if(raw.length!==expected)throw Error('PNG scanline length mismatch');
 const pixels=Buffer.alloc(rowBytes*height),bgra=Buffer.alloc(width*height*4);
 for(let y=0;y<height;y++){
  const filter=raw[y*(rowBytes+1)];if(filter>4)throw Error('Invalid PNG filter');
  for(let x=0;x<rowBytes;x++){
   const index=y*rowBytes+x,a=x>=channels?pixels[index-channels]:0,b=y?pixels[index-rowBytes]:0,c=y&&x>=channels?pixels[index-rowBytes-channels]:0;
   const prediction=filter===0?0:filter===1?a:filter===2?b:filter===3?Math.floor((a+b)/2):paeth(a,b,c);
   pixels[index]=(raw[y*(rowBytes+1)+1+x]+prediction)&255;
  }
 }
 for(let i=0;i<width*height;i++){
  bgra[i*4]=pixels[i*channels+2];bgra[i*4+1]=pixels[i*channels+1];bgra[i*4+2]=pixels[i*channels];bgra[i*4+3]=pixels[i*channels+3];
 }
 return {width,height,bgra};
}
function different(a,b){let count=0;for(let i=0;i<a.length;i+=4)if(a[i]!==b[i]||a[i+1]!==b[i+1]||a[i+2]!==b[i+2])count++;return count;}
function verifyFrames(folder){
 const report=JSON.parse(fs.readFileSync(path.join(folder,'report.json'),'utf8'));
 if(report.schemaVersion!==1)throw Error('Unsupported Metal report schemaVersion');
 const frames={},pngSha256={},bgraSha256={};
 for(const name of ['original','edited','orbit','empty']){
  const bytes=fs.readFileSync(path.join(folder,name+'.png')),frame=decodePng(bytes);
  if(frame.width!==report.width||frame.height!==report.height)throw Error('Metal frame width/height mismatch');
  for(let i=3;i<frame.bgra.length;i+=4)if(frame.bgra[i]!==255)throw Error('Non-opaque GPU evidence frame');
  frames[name]=frame.bgra;pngSha256[name]=sha(bytes);bgraSha256[name]=sha(frame.bgra);
 }
 const clear=frames.empty;
 for(let i=0;i<clear.length;i+=4)if(clear[i]!==clear[0]||clear[i+1]!==clear[1]||clear[i+2]!==clear[2])throw Error('Empty GPU image is not a uniform background');
 const result={width:report.width,height:report.height,foregroundPixels:different(frames.original,clear),editedPixels:different(frames.original,frames.edited),orbitPixels:different(frames.original,frames.orbit),originalBgraSha256:bgraSha256.original};
 for(const key of ['foregroundPixels','editedPixels','orbitPixels','originalBgraSha256'])if(result[key]!==report[key])throw Error(key+' differs from independently decoded pixels');
 if(result.foregroundPixels<=100||result.editedPixels<=100||result.orbitPixels<=100)throw Error('GPU content/edit/orbit threshold failed');
 if(!Number.isInteger(report.orderPixels)||report.orderPixels<0||report.orderPixels>32)throw Error('orderPixels outside harness acceptance');
 return {...result,orderPixels:report.orderPixels,pngSha256,bgraSha256,depthOrderScope:'Real harness raw-frame comparison; reordered PNG not retained in this checkpoint.'};
}
if(require.main===module)try{console.log(JSON.stringify(verifyFrames(path.resolve(__dirname,'..',process.argv[2]||'docs/evidence/apple/metal-viewport')),null,2));}catch(error){console.error(error.message);process.exitCode=1;}
module.exports={decodePng,verifyFrames};
