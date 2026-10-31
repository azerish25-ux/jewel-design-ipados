'use strict';
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto'),zlib=require('node:zlib');
const hash=bytes=>crypto.createHash('sha256').update(bytes).digest('hex');
function adler32(bytes){let a=1,b=0;for(const value of bytes){a=(a+value)%65521;b=(b+a)%65521;}return ((b<<16)|a)>>>0;}
const reportFile=path.resolve(process.argv[2]||'.local/evidence/compression/boundary-report.json');
const report=JSON.parse(fs.readFileSync(reportFile,'utf8')),checks=[];
for(const entry of report.cases.filter(x=>x.stage==='recovered.compress'&&x.status==='observed_stage_success')){
 const bytes=fs.readFileSync(path.join(path.dirname(reportFile),'packed',entry.length+'-'+entry.pattern+'.zlib'));
 if(hash(bytes)!==entry.outputSha256)throw Error('Captured output hash differs');
 if(bytes.length===0){checks.push({length:entry.length,pattern:entry.pattern,packedSha256:hash(bytes),status:'missing_zlib_frame'});continue;}
 const raw=zlib.inflateRawSync(bytes.subarray(2,-4),{maxOutputLength:100001});
 if(raw.length!==entry.length||hash(raw)!==entry.inputSha256)throw Error('Raw DEFLATE payload differs from captured input hash');
 const stored=bytes.readUInt32BE(bytes.length-4),expected=adler32(raw);
 checks.push({length:entry.length,pattern:entry.pattern,packedBytes:bytes.length,packedSha256:hash(bytes),inputSha256:entry.inputSha256,rawPayloadMatches:true,storedAdler32:stored.toString(16).padStart(8,'0'),expectedAdler32:expected.toString(16).padStart(8,'0'),checksumMatches:stored===expected});
}
console.log(JSON.stringify({schemaVersion:1,scope:'Actual captured recovered outputs checked independently; no original binary execution or compatibility pass',managedStages:report.cases.length,managedFailures:report.cases.filter(x=>x.status==='failure').map(x=>({length:x.length,pattern:x.pattern,stage:x.stage,error:x.error.split('\n')[0].trim()})),checks},null,2));
