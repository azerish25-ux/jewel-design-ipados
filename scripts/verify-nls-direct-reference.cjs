'use strict';
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto'),zlib=require('node:zlib');const root=path.resolve(__dirname,'..'),sha=b=>crypto.createHash('sha256').update(b).digest('hex');const check=(v,m)=>{if(!v)throw Error(m);};
const pins=[
 ['nls-direct-exhaustive-60502.json.br','e8d236e1fe287db21fc39f8ce07e023df9db78c579d8ab289dde1b02497434ff','759923e108c71a2f9f9b177b32aed63262938010bf436bde1bce8b6213ac97a1'],
 ['nls-direct-exhaustive-results-60502.bin.br','021daf2c52872203ef9044aa8b022c77da80615134928bb42a96d9fdc5cee007','6ac1e2a2a4f3d9d18c00a59e60f2d0f9c97db81f949f21a4619de56c7c84a066'],
 ['nls-direct-jamo-60502.json.br','1f06d5caf6337b5b6876e03ea94c5dcc00b40118ddbe175c944ad8360cb05129','b82701dfe82798b573cfae5ea20f236469c9393d888e939033606a5e76a69ebd'],
 ['nls-direct-jamo-results-60502.bin.br','924bcbf9a7be6a4762e8fd67f37617bb95632ae4e04ed34ff085c230518ad4be','2ca0885e9a14387c0f7ed17daca9cf926d2c0d8fb045e1fea57fba408a879928'],
 ['nls-direct-stress-60502.json.br','0ea4cd7503a20a7ad2f0f7f6650bce01122c87c09864239c75f6c8ef2228b75e','f8ce541a0671d44d2f5a7cdf9807c75147a90518004772c140764ea2a27dda38']
];
function verify(supplied){const decoded=pins.map(([name,packedHash,rawHash],i)=>{const packed=supplied?.[i]??fs.readFileSync(path.join(root,'reference/fixtures',name));check(sha(packed)===packedHash,'Direct packed native fixture changed: '+name);const raw=zlib.brotliDecompressSync(packed,{maxOutputLength:64*1024*1024});check(sha(raw)===rawHash,'Direct decoded native fixture changed');return raw;});
 const exhaustive=JSON.parse(decoded[0]),jamo=JSON.parse(decoded[2]),stress=JSON.parse(decoded[4]);
 for(const r of [exhaustive,jamo,stress])check(r.OriginalTargetExecuted===false&&r.Locale===1&&r.Flags===0x20001&&r.Version.NlsVersion===0x60502&&r.Version.EffectiveId===1025,'Direct native profile changed');
 check(exhaustive.UnitScalarInputs===1114112&&exhaustive.Comparisons===4456448&&!exhaustive.CompareFailures&&!exhaustive.LowerFailures&&decoded[1].length===4456448&&sha(decoded[1])===exhaustive.NativeRecordsSha256,'Direct exhaustive coverage');
 for(let plane=0;plane<17;plane++){const p=exhaustive.Planes[plane],chunk=decoded[1].subarray(plane*65536*4,(plane+1)*65536*4);check(p.Index===plane&&p.Inputs===65536&&p.Comparisons===262144&&sha(chunk)===p.NativeSha256,'Complete native direct plane digest');}
 check(jamo.Leading.length===125&&jamo.Vowels.length===95&&jamo.Trailing.length===137&&jamo.TripleInputs===1626875&&jamo.Comparisons===6507500&&!jamo.Failures&&decoded[3].length===6507500&&sha(decoded[3])===jamo.NativeRecordsSha256,'Complete native Jamo triple coverage');
 for(const r of [decoded[1],decoded[3]])for(const value of r)check(value>=1&&value<=3,'Native comparison failure byte');
 check(stress.Comparisons===103280&&!stress.Failures&&!stress.RepeatMismatches&&stress.Pairs.length===103280,'Stress pair coverage');const groups={};
 for(const p of stress.Pairs){check([1,2,3].includes(p.NativeResult)&&p.NativeResult===p.RepeatNativeResult&&Buffer.from(p.FirstUtf16Base64,'base64').length%2===0&&Buffer.from(p.SecondUtf16Base64,'base64').length%2===0,'Repeated native stress pair');groups[p.Group]=(groups[p.Group]??0)+1;}
 check(groups['all-public-mapping-contexts']===53920&&groups['long-mark-mutation']===104&&groups['long-leading-mark']===104,'Targeted public mapping/boundary coverage');
 const jamoInput=fs.readFileSync(path.join(root,'src/Port/Rendering.Candidate/Fixtures/windows-nls1-jamo-60502.bin'));check(jamoInput.length===2856&&sha(jamoInput)==='4f8e7acefeeead6cd25f22cb801db55603813760bb9072c84cf2d31ed95a5f86','Native/public Jamo input table');
 return {schemaVersion:1,exhaustiveComparisons:4456448,jamoComparisons:6507500,jamoTriples:1626875,stressComparisons:103280,stressGroups:groups,nativeErrors:0,nativeRepeatMismatches:0,jamoInputRows:357,scope:'Independent retained native references. Managed implementation/platform proofs separate; original wrapper/CRT/ANSI/state/document/PC/physical remain open.'};}
module.exports={verify,pins};if(require.main===module)process.stdout.write(JSON.stringify(verify(),null,2)+'\n');
