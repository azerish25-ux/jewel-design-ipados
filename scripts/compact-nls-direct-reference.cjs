'use strict';
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto'),zlib=require('node:zlib');
const root=path.resolve(__dirname,'..'),mode=process.argv[2],input=path.resolve(root,process.argv[3]||'');
const sha=b=>crypto.createHash('sha256').update(b).digest('hex');
if(!['exhaustive','jamo','stress'].includes(mode)||!input.toLowerCase().startsWith(root.toLowerCase()+path.sep))throw Error('Approved mode/repository input required');
if(process.platform==='win32'&&!/^D:/i.test(root))throw Error('Windows writes must remain on D:');
const json=fs.readFileSync(input),report=JSON.parse(json);
if(report.OriginalTargetExecuted!==false||report.Locale!==1||report.Flags!==0x20001||report.Version.NlsVersion!==0x60502)throw Error('Native profile mismatch');
const outputs=[['reference/fixtures/nls-direct-'+mode+'-60502.json.br',json]];
if(mode==='stress'){
 if(report.Comparisons!==103280||report.Failures||report.RepeatMismatches||report.Pairs.length!==103280)throw Error('Stress capture coverage');
 for(const p of report.Pairs)if(![1,2,3].includes(p.NativeResult)||p.NativeResult!==p.RepeatNativeResult)throw Error('Stress native comparison failure');
}else{
 const raw=fs.readFileSync(input+'.native-results.bin');
 const expected=mode==='exhaustive'?4456448:6507500;
 if(report.Comparisons!==expected||report.NativeRecordsBytes!==raw.length||raw.length!==expected||sha(raw)!==report.NativeRecordsSha256||report.CompareFailures||report.LowerFailures||report.Failures)throw Error('Exhaustive capture coverage/digest');
 for(const value of raw)if(value<1||value>3)throw Error('Invalid native result byte');
 outputs.push(['reference/fixtures/nls-direct-'+mode+'-results-60502.bin.br',raw]);
}
for(const [name,bytes]of outputs){const packed=zlib.brotliCompressSync(bytes,{params:{[zlib.constants.BROTLI_PARAM_QUALITY]:11}});fs.writeFileSync(path.join(root,name),packed);console.log(JSON.stringify({path:name,bytes:packed.length,sha256:sha(packed),decodedBytes:bytes.length,decodedSha256:sha(bytes)}));}
