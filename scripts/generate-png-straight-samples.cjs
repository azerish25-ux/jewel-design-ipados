'use strict';
const fs=require('node:fs'),path=require('node:path'),zlib=require('node:zlib'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..');
const chunk=(name,data)=>{const t=Buffer.from(name),s=Buffer.alloc(4),c=Buffer.alloc(4);s.writeUInt32BE(data.length);c.writeUInt32BE(zlib.crc32(Buffer.concat([t,data]))>>>0);return Buffer.concat([s,t,data,c]);};
const passes=[[0,0,8,8],[4,0,8,8],[0,4,4,8],[2,0,4,4],[0,2,2,4],[1,0,2,2],[0,1,1,2]];
const paeth=(a,b,c)=>{const p=a+b-c,x=Math.abs(p-a),y=Math.abs(p-b),z=Math.abs(p-c);return x<=y&&x<=z?a:y<=z?b:c;};
const cases=[];
for(const [type,depth] of [[0,1],[0,2],[0,4],[0,8],[2,8],[3,1],[3,2],[3,4],[3,8],[4,8],[6,8]])for(const interlace of [0,1]){
 const width=9,height=7,channels={0:1,2:3,3:1,4:2,6:4}[type],mask=(1<<depth)-1,expected=[],samples=[],palette=Buffer.alloc(3*(mask+1)),alpha=Buffer.alloc(mask+1);
 for(let i=0;i<=mask;i++){palette[i*3]=i*37%256;palette[i*3+1]=i*73%256;palette[i*3+2]=i*19%256;alpha[i]=i*29%256;}
 for(let y=0;y<height;y++)for(let x=0;x<width;x++){
  const p=y*width+x,r=(17+p*37)%256,g=(31+p*73)%256,b=(77+p*19)%256,a=p*43%256,k=p%(mask+1);
  const value=type===0?[k]:type===2?[r,g,b]:type===3?[k]:type===4?[r,a]:[r,g,b,a];samples.push(value);
  expected.push(...(type===0?[k*255/mask,k*255/mask,k*255/mask,k===0?0:255]:type===2?[r,g,b,p===0?0:255]:type===3?[palette[k*3],palette[k*3+1],palette[k*3+2],alpha[k]]:type===4?[r,r,r,a]:value));
 }
 const streams=[];let passIndex=0;
 for(const [sx,sy,dx,dy]of(interlace?passes:[[0,0,1,1]])){
  const pw=Math.max(0,Math.ceil((width-sx)/dx)),ph=Math.max(0,Math.ceil((height-sy)/dy));if(!pw||!ph)continue;
  const rowBytes=Math.ceil(pw*channels*depth/8),bpp=Math.max(1,Math.ceil(channels*depth/8));let previous=Buffer.alloc(rowBytes);
  for(let y=0;y<ph;y++){
   const row=Buffer.alloc(rowBytes);let bit=0;
   for(let x=0;x<pw;x++)for(const v of samples[(sy+y*dy)*width+sx+x*dx]){row[bit>>3]|=v<<(8-depth-(bit%8));bit+=depth;}
   const filter=(y+passIndex)%5,encoded=Buffer.alloc(rowBytes);
   for(let i=0;i<rowBytes;i++){const left=i>=bpp?row[i-bpp]:0,up=previous[i],ul=i>=bpp?previous[i-bpp]:0;const predictor=[0,left,up,Math.floor((left+up)/2),paeth(left,up,ul)][filter];encoded[i]=(row[i]-predictor+256)%256;}
   streams.push(Buffer.from([filter]),encoded);previous=row;
  }passIndex++;
 }
 const h=Buffer.alloc(13);h.writeUInt32BE(width);h.writeUInt32BE(height,4);h[8]=depth;h[9]=type;h[12]=interlace;
 const compressed=zlib.deflateSync(Buffer.concat(streams)),parts=[Buffer.from('89504e470d0a1a0a','hex'),chunk('IHDR',h)];
 if(type===3)parts.push(chunk('PLTE',palette),chunk('tRNS',alpha));
 if(type===0)parts.push(chunk('tRNS',Buffer.from([0,0])));
 if(type===2){const t=Buffer.alloc(6);t.writeUInt16BE(17,0);t.writeUInt16BE(31,2);t.writeUInt16BE(77,4);parts.push(chunk('tRNS',t));}
 const split=Math.max(1,Math.floor(compressed.length/2));parts.push(chunk('IDAT',compressed.subarray(0,split)),chunk('IDAT',compressed.subarray(split)),chunk('IEND',Buffer.alloc(0)));
 const bytes=Buffer.concat(parts);cases.push({name:'type'+type+'-depth'+depth+'-adam'+interlace,width,height,type,depth,interlace,png:bytes.toString('base64'),rgba:Buffer.from(expected).toString('base64'),rgbaSha256:crypto.createHash('sha256').update(Buffer.from(expected)).digest('hex')});
}
const partsOf=bytes=>{const parts=[];for(let p=8;p<bytes.length;){const n=bytes.readUInt32BE(p);parts.push({name:bytes.subarray(p+4,p+8).toString("ascii"),data:Buffer.from(bytes.subarray(p+8,p+8+n))});p+=12+n;}return parts;};
const pack=parts=>Buffer.concat([Buffer.from("89504e470d0a1a0a","hex"),...parts.map(p=>chunk(p.name,p.data))]);
const base=partsOf(Buffer.from(cases.find(c=>c.type===6&&c.interlace===0).png,"base64"));
const idat=Buffer.concat(base.filter(p=>p.name==="IDAT").map(p=>p.data)),raw=zlib.inflateSync(idat);
const replaceData=data=>[base[0],{name:"IDAT",data},base.at(-1)];
const invalid=[];const bad=(name,parts,error="InvalidData")=>invalid.push({name,png:pack(parts).toString("base64"),error});
let b=Buffer.from(raw);b[0]=5;bad("invalid-filter",replaceData(zlib.deflateSync(b)));
bad("truncated-inflation",replaceData(zlib.deflateSync(raw.subarray(0,raw.length-1))));
bad("extra-inflation",replaceData(zlib.deflateSync(Buffer.concat([raw,Buffer.from([0])]))));
b=Buffer.from(idat);b[b.length-1]^=1;bad("invalid-adler-with-valid-chunk-crc",replaceData(b));
b=Buffer.from(base[0].data);b[8]=16;bad("16-bit-not-silently-reduced",[{name:"IHDR",data:b},...base.slice(1)],"NotSupported");
bad("unknown-critical",[base[0],{name:"ABCD",data:Buffer.alloc(0)},...base.slice(1)],"NotSupported");
bad("apng-frame-selection",[base[0],{name:"acTL",data:Buffer.from([0,0,0,1,0,0,0,0])},...base.slice(1)],"NotSupported");
bad("duplicate-header",[base[0],base[0],...base.slice(1)]);
bad("nonconsecutive-idat",[base[0],base[1],{name:"tEXt",data:Buffer.from("note\0value")},...base.slice(2)]);
b=Buffer.from(base[0].data);b.writeUInt32BE(0xffffffff,0);bad("dimension-overflow",[{name:"IHDR",data:b},...base.slice(1)]);
const indexed=partsOf(Buffer.from(cases.find(c=>c.type===3&&c.depth===1&&c.interlace===0).png,"base64"));
bad("missing-palette",indexed.filter(p=>p.name!=="PLTE"&&p.name!=="tRNS"));
bad("out-of-range-palette-index",indexed.map(p=>p.name==="PLTE"?{...p,data:p.data.subarray(0,3)}:p.name==="tRNS"?{...p,data:p.data.subarray(0,1)}:p));
const rgbParts=partsOf(Buffer.from(cases.find(c=>c.type===2&&c.interlace===0).png,"base64"));
bad("palette-after-transparency",[rgbParts[0],rgbParts.find(p=>p.name==="tRNS"),{name:"PLTE",data:Buffer.from([0,0,0])},...rgbParts.filter(p=>p.name==="IDAT"||p.name==="IEND")]);
const target=path.join(root,'tests/Port/Geometry.Candidate.Contracts/Fixtures/png-straight-samples.json');
fs.writeFileSync(target,JSON.stringify({schemaVersion:1,source:'W3C PNG integer sample fixtures; independent encoder cycles all five filters and Adam7 passes; never CAD preview substitutes.',cases,invalid},null,2)+'\n');
console.log(JSON.stringify({cases:cases.length,target}));
