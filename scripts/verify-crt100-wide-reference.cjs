'use strict';
const fs=require('node:fs'),path=require('node:path'),zlib=require('node:zlib'),crypto=require('node:crypto');
const sha=b=>crypto.createHash('sha256').update(b).digest('hex');
function load(file=path.join(__dirname,'../reference/fixtures/windows-crt100-wide.json.br')) {
 const packed=fs.readFileSync(file);if(sha(packed)!=='d985a9cf166f485665b673f8df10ba7287de9ee8c3a6059b88a89a0b2116da54')throw Error('CRT100 wide packed identity');
 const raw=zlib.brotliDecompressSync(packed,{maxOutputLength:2500000});if(raw.length!==1965539||sha(raw)!=='6b74a96076c12140d18f981f58721811c8bc58b3128729080ed8c419ccbd1f84')throw Error('CRT100 wide decoded identity');return JSON.parse(raw);
}
function reconstructComparisons(items,count,key,base) {
 const bytes=Buffer.alloc(count*4);let previous=-1;
 for(const row of items){const index=row[key]-base;if(!Number.isInteger(index)||index<0||index>=count||index<=previous||!Number.isInteger(row.Result)||row.Result===0)throw Error('CRT100 nonzero comparison record');bytes.writeInt32LE(row.Result,index*4);previous=index;}
 return bytes;
}
function lowerContext(input,map,changes) {
 const output=Buffer.from(input);
 for(let at=0;at<input.length;at+=2){const unit=input.readUInt16LE(at);if(unit===0)break;
  if(unit>=0xd800&&unit<=0xdbff&&at+2<input.length){const tail=input.readUInt16LE(at+2);if(tail>=0xdc00&&tail<=0xdfff){const scalar=0x10000+((unit-0xd800)<<10)+tail-0xdc00,changed=changes.get(scalar);if(changed){output.writeUInt16LE(changed.OutputFirstUtf16,at);output.writeUInt16LE(changed.OutputSecondUtf16,at+2);}at+=2;continue;}}
  output.writeUInt16LE(map.readUInt16LE(unit*2),at);
 }
 return output;
}
function verify(r) {
 if(r.SchemaVersion!==1||r.Kind!=='installed_microsoft_crt100_current_wide_entrypoint_reference'||r.OriginalTargetExecuted!==false||r.LocaleRestored!==true||r.RuntimeSha256!=='2b7ab898b2f64f4cd1ae651c93d7eb9556208fd8d8d40d425e856c83d86d2346'||r.Architecture!=='X64'||r.Profiles.length!==3)throw Error('CRT100 wide profile identity');
 const prior=require('./verify-crt100-reference.cjs').load();let lowerCount=0,pairCount=0,contextCount=0;const profileResults=[];
 for(let index=0;index<3;index++) {
  const p=r.Profiles[index],old=prior.Profiles[index],unitMap=Buffer.from(old.LowerUtf16Base64,'base64'),map=Buffer.from(p.LowerUtf16Base64,'base64');
  if(p.Locale!==old.Locale||map.length!==131072||sha(map)!==p.LowerSha256||p.LowerFailures!==0||p.ExplicitPairDifferences!==0||p.Comparisons.length!==1156||p.LowerContexts.length!==49||p.SupplementaryUnits!==1048576||p.SupplementaryFailures.length!==0)throw Error('CRT100 wide coverage');
  let unitDiff=0;for(let u=0;u<65536;u++){const expected=index===2&&u===221?253:unitMap.readUInt16LE(u*2);if(map.readUInt16LE(u*2)!==expected)throw Error('CRT100 whole lower unit');if(expected!==unitMap.readUInt16LE(u*2))unitDiff++;}if(unitDiff!==p.ExplicitLowerDifferences||unitDiff!==(index===2?1:0))throw Error('CRT100 lower distinction');
  const supplementary=Buffer.alloc(1048576*4),changes=new Map();let prev=0;
  for(const row of p.SupplementaryChanges){if(row.InputScalar<=prev||row.InputScalar<0x10000||row.InputScalar>0x10ffff||row.OutputFirstUtf16<0||row.OutputFirstUtf16>65535||row.OutputSecondUtf16<0||row.OutputSecondUtf16>65535)throw Error('CRT100 supplementary record');changes.set(row.InputScalar,row);prev=row.InputScalar;}
  if(changes.size!==(index===0?0:40))throw Error('CRT100 supplementary coverage');
  for(let scalar=0x10000;scalar<=0x10ffff;scalar++){const value=scalar-0x10000,at=value*4,changed=changes.get(scalar);let high=0xd800+(value>>>10),low=0xdc00+(value&1023);if(changed){if(scalar<0x10400||scalar>0x10427||changed.OutputFirstUtf16!==0xd801||changed.OutputSecondUtf16!==0xdc28+scalar-0x10400)throw Error('CRT100 Deseret distinction');high=changed.OutputFirstUtf16;low=changed.OutputSecondUtf16;}supplementary.writeUInt16LE(high,at);supplementary.writeUInt16LE(low,at+2);}
  if(sha(supplementary)!==p.SupplementarySha256)throw Error('CRT100 complete native supplementary digest');
  const unitCompare=reconstructComparisons(p.UnitWholeCompareNonzero,65536,'InputUtf16',0),scalarCompare=reconstructComparisons(p.SupplementaryCompareNonzero,1048576,'InputScalar',0x10000);
  if(sha(unitCompare)!==p.UnitWholeCompareSha256||sha(scalarCompare)!==p.SupplementaryCompareSha256||p.UnitWholeCompareNonzero.length!==(index===2?1:0)||p.SupplementaryCompareNonzero.length!==(index===0?0:40))throw Error('CRT100 complete native comparison digest');
  if(index===2&&(p.UnitWholeCompareNonzero[0].InputUtf16!==221||p.UnitWholeCompareNonzero[0].Result!==-32))throw Error('CRT100 Turkish lower/compare disagreement');for(let i=0;i<p.SupplementaryCompareNonzero.length;i++){const row=p.SupplementaryCompareNonzero[i];if(row.InputScalar!==0x10400+i||row.Result!==-40)throw Error('CRT100 supplementary lower/compare disagreement');}
  for(let i=0;i<1156;i++){const pair=p.Comparisons[i],oldPair=old.Comparisons[i];if(pair.Left!==oldPair.Left||pair.Right!==oldPair.Right||pair.Result!==oldPair.Result||pair.Repeat!==pair.Result)throw Error('CRT100 current/explicit comparison agreement');}
  let contextDifferences=0;for(let i=0;i<49;i++){const c=p.LowerContexts[i],source=Buffer.from(c.InputUtf16Base64,'base64'),actual=Buffer.from(c.OutputUtf16Base64,'base64');if((source.length&1)||!c.Success||source.length!==actual.length||!lowerContext(source,map,changes).equals(actual)||c.InputUtf16Base64!==r.Profiles[0].LowerContexts[i].InputUtf16Base64)throw Error('CRT100 whole context output');const perUnit=lowerContext(source,unitMap,new Map());if(!actual.equals(perUnit))contextDifferences++;if(c.PerUnitEqual!==actual.equals(perUnit))throw Error('CRT100 whole-vs-unit diagnostic');}
  if(contextDifferences!==p.WholeBufferPerUnitDifferences||contextDifferences!==[0,1,2][index])throw Error('CRT100 context distinction coverage');
  lowerCount+=1114112;pairCount+=1114112+1156;contextCount+=49;profileResults.push({locale:p.Locale,wholeLowerUnitDifferences:unitDiff,supplementaryChanges:changes.size,lowerCompareDisagreements:p.UnitWholeCompareNonzero.length+p.SupplementaryCompareNonzero.length,wholeVsUnitContexts:contextDifferences});
 }
 return {profiles:3,wholeLowerInputs:lowerCount,repeatedComparePairs:pairCount,wholeBuffers:contextCount,localeRestored:true,originalTargetExecuted:false,profileResults};
}
module.exports={load,verify,lowerContext};if(require.main===module)console.log(JSON.stringify(verify(load()),null,2));
