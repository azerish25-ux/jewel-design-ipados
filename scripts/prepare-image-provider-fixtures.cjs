'use strict';
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto'),zlib=require('node:zlib');
const root=path.resolve(__dirname,'..'),out=path.resolve(root,process.argv[2]||'.local/evidence/image-provider-input');
if(!out.startsWith(root+path.sep))throw Error('Fixture output leaves repository');
fs.mkdirSync(out,{recursive:true});
const base=JSON.parse(fs.readFileSync(path.join(root,'tests/Port/Geometry.Candidate.Contracts/Fixtures/local-image-decode.json'),'utf8'));
const chunk=(name,bytes)=>{const type=Buffer.from(name),size=Buffer.alloc(4),crc=Buffer.alloc(4);size.writeUInt32BE(bytes.length);crc.writeUInt32BE(zlib.crc32(Buffer.concat([type,bytes]))>>>0);return Buffer.concat([size,type,bytes,crc]);};
const low=[17,31,77,1,211,43,19,0,128,129,130,127],header=Buffer.alloc(13);header.writeUInt32BE(3,0);header.writeUInt32BE(1,4);header[8]=8;header[9]=6;
const png=Buffer.concat([Buffer.from('89504e470d0a1a0a','hex'),chunk('IHDR',header),chunk('IDAT',zlib.deflateSync(Buffer.from([0,...low]))),chunk('IEND',Buffer.alloc(0))]);
const fixtures=[
{name:'orientation.png',bytes:Buffer.from(base.png.base64,'base64'),width:2,height:2,expectedRgba:[255,0,0,255,0,255,0,255,0,0,255,255,255,0,0,128]},
{name:'opaque.bmp',bytes:Buffer.from(base.bmp.base64,'base64'),width:3,height:2,expectedRgba:base.bmp.rawRgba},
{name:'low-alpha.png',bytes:png,width:3,height:1,expectedRgba:low},
{name:'recovered.jpg',bytes:fs.readFileSync(path.join(root,base.jpeg.resourcePath)),width:base.jpeg.width,height:base.jpeg.height}
];
const manifest=fixtures.map(({bytes,...f})=>{fs.writeFileSync(path.join(out,f.name),bytes);return {...f,encodedSha256:crypto.createHash('sha256').update(bytes).digest('hex')};});
fs.writeFileSync(path.join(out,'manifest.json'),JSON.stringify({schemaVersion:1,fixtures:manifest,scope:'Analytical image decoder fixtures and unchanged recovered JPEG; no CAD preview substitution'},null,2)+'\n');
console.log(JSON.stringify({files:manifest.length,output:out}));
