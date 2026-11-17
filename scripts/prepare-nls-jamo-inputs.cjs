'use strict';
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');const root=path.resolve(__dirname,'..'),sha=b=>crypto.createHash('sha256').update(b).digest('hex');
if(process.platform==='win32'&&!/^D:/i.test(root))throw Error('D output required');
const source=fs.readFileSync(path.join(root,'.local/reference/windows-sorting/Windows 11 Sorting Weight Table.txt'));
if(sha(source)!=='f0bab873ed8901ae17afc4848c7d9e0a7de4310942effb2b54b538411b26d542')throw Error('Approved public source changed');
const lines=source.toString('utf8').split(/\r?\n/).map(s=>s.split(';')[0].trim()),start=lines.findIndex(s=>/^JAMOSORT\s/.test(s)),rows=[];
for(let i=start+1;i<lines.length;i++){if(!lines[i])continue;if(!lines[i].startsWith('0x'))break;let row=lines[i].split(/\s+/).map(Number);if(row.length!==6||row.some(v=>!Number.isInteger(v)||v<0||v>65535))throw Error('Jamo source row');rows.push(row);}
if(rows.length!==357)throw Error('Jamo source count');
const result=Buffer.alloc(357*8);for(let i=0;i<rows.length;i++){let c=rows[i][0],index=c>=0x1100&&c<=0x11ff?c-0x1100:c>=0xa960&&c<=0xa97c?c-0xa860:c>=0xd7b0&&c<=0xd7c6?c-0xd693:c-0xd697;if(index!==i||rows[i].slice(1).some(v=>v>255))throw Error('Jamo native row ordering');for(let j=0;j<5;j++)result[i*8+j]=rows[i][j+1];}
if(sha(result)!=='4f8e7acefeeead6cd25f22cb801db55603813760bb9072c84cf2d31ed95a5f86')throw Error('Complete native Jamo data differs');
const native=fs.readFileSync(path.join(root,'.local/reference/windows-nls/SortDefault.nls'));if(native.indexOf(result)!==3382884)throw Error('Native Jamo reference span differs');
const target='src/Port/Rendering.Candidate/Fixtures/windows-nls1-jamo-60502.bin';fs.writeFileSync(path.join(root,target),result);console.log(JSON.stringify({path:target,rows:357,bytes:result.length,sha256:sha(result),nativeByteEquality:true}));
