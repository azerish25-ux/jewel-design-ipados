const fs=require('fs'),path=require('path'),crypto=require('crypto');
const {parse}=require('./csg-v4.cjs');
const TYPES=new Map([
 ['38455005-5ef2-43db-8fb9-60e447d75d59','CS.CSValues.CSValue'],
 ['a5d6b280-42fe-4188-8ff0-9ac6caed7831','CS.CSValues.CSValueOnOff'],
 ['6c85fe71-927a-4d28-9335-8f1eb8713979','CS.CSValues.CSValueChooser']
]);
class ScalarCursor {
 constructor(bytes,start,end){this.bytes=bytes;this.p=start;this.end=end;}
 need(n){if(!Number.isSafeInteger(n)||n<0||this.p>this.end-n)throw Error('Scalar read outside bound');}
 i(){this.need(4);const value=this.bytes.readInt32LE(this.p);this.p+=4;return value;}
 count(max){const n=this.i();if(n<0||n>max)throw Error('Scalar count outside bound');return n;}
 text(){const n=this.count(1000000);this.need(n*2);if(n===0)return '';if(this.bytes.readUInt16LE(this.p+n*2-2)!==0)throw Error('Scalar UTF16 string not terminated');const value=this.bytes.toString('utf16le',this.p,this.p+(n-1)*2);this.p+=n*2;return value;}
 ints(){const n=this.count(10000);this.need(n*4);return Array.from({length:n},()=>this.i());}
 userData(){const n=this.count(10000);this.need(n*12);return Array.from({length:n},()=>({type:this.text(),key:this.text(),value:this.text()}));}
}
function readAdditionalScalars(bytes){
 if(!Buffer.isBuffer(bytes)||bytes.length>64*1024*1024)throw Error('Scalar input byte bound');
 const original=parse(bytes),extras=[];
 for(const node of original.nodes){
  const type=TYPES.get(node.guid);if(!type||node.major!==1||node.minor!==1)continue;
  const r=new ScalarCursor(bytes,node.payload+8,node.crcOffset);
  const identifier=r.text(),flags=r.i()>>>0,legacyLayer=r.i(),userData=r.userData(),objects=r.ints(),channelCountOffset=r.p,channels=r.ints();
  if(objects.length!==channels.length)throw Error('Scalar link array mismatch');
  const categories=r.ints(),legacyUiInteger=r.i(),valueOffset=r.p;r.need(8);
  const value=bytes.readDoubleLE(r.p),valueBits=bytes.readBigUInt64LE(r.p).toString(16).padStart(16,'0');r.p+=8;
  if(r.p!==node.crcOffset)throw Error('Scalar body did not consume bounded region');
  const valueKind=Number.isNaN(value)?'nan':value===Infinity?'positive-infinity':value===-Infinity?'negative-infinity':Object.is(value,-0)?'negative-zero':'finite';
  extras.push({index:node.index,guid:node.guid,type,major:node.major,minor:node.minor,identifier,flags,legacyLayer,userData,objects,channels,categories,legacyUiInteger,
   valueOffset,channelCountOffset,valueBits,valueNumber:Number.isFinite(value)?value:null,valueKind,fullyConsumed:true});
 }
 return {fixtureSha256:original.sha256,existingNumericCount:original.numericCount,extras};
}
if(require.main===module){
 if(!process.argv.includes('--record'))throw Error('Use --record for the pinned reference fixture');
 const root=path.resolve(__dirname,'../..'),fixture='reference/fixtures/library-item-4.csg';
 const report=readAdditionalScalars(fs.readFileSync(path.join(root,fixture)));
 if(report.fixtureSha256!=='73e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd')throw Error('Unexpected reference fixture');
 const sources=['CS.CSObjects/CSObject.cs','CS.CSObjects.CSBlocks/CSBlock.cs','CS.CSValues/CSValueBase.cs','CS.CSValues/CSValue.cs','CS.CSValues/CSValueOnOff.cs','CS.CSValues/CSValueChooser.cs'];
 const artifact={schemaVersion:1,status:'reference_reader_only',scope:'Strict independent inspection of three stored-double record types at version1.1. Production codec, UI OnOff/chooser behavior and PC/native regeneration are not established.',
  sources:sources.map(p=>{const source='recovery/canonical/cad/'+p;return {path:source,sha256:crypto.createHash('sha256').update(fs.readFileSync(path.join(root,source))).digest('hex')};}),...report};
 fs.writeFileSync(path.join(root,'docs/evidence/audit/plain-scalar-reference.json'),JSON.stringify(artifact,null,2)+'\n');
 const counts={};for(const item of report.extras)counts[item.type]=(counts[item.type]||0)+1;
 console.log(JSON.stringify({existingNumeric:report.existingNumericCount,additionalScalars:report.extras.length,types:counts},null,2));
}
module.exports={readAdditionalScalars,ScalarCursor};
