'use strict';
const fs=require('node:fs'),path=require('node:path'),zlib=require('node:zlib');
const root=path.resolve(__dirname,'..');
if(process.platform==='win32'&&!/^D:/i.test(root))throw Error('D output required');
const publicBytes=fs.readFileSync(path.join(root,'.local/reference/windows-sorting/Windows 11 Sorting Weight Table.txt'));if(require('node:crypto').createHash('sha256').update(publicBytes).digest('hex')!=='f0bab873ed8901ae17afc4848c7d9e0a7de4310942effb2b54b538411b26d542')throw Error('Approved source changed');
const text=publicBytes.toString('utf8').split(/\r?\n/).map(s=>s.split(';')[0].trim());
function section(name){let i=text.findIndex(s=>new RegExp('^'+name+'(?:\\s|$)').test(s)),out=[];for(i++;i<text.length;i++){if(!text[i])continue;if(!text[i].startsWith('0x'))break;out.push(text[i].split(/\s+/).map(Number));}return out;}
const expansions=section('EXPANSION'),aliases=section('COMPATIBILITY');
if(expansions.length!==730||aliases.length!==955)throw Error('Source row count');
const pairs=[],encode=s=>Buffer.from(s,'utf16le').toString('base64');
function add(a,b,group){pairs.push({FirstUtf16Base64:encode(a),SecondUtf16Base64:encode(b),Group:group});pairs.push({FirstUtf16Base64:encode(b),SecondUtf16Base64:encode(a),Group:group});}
for(const row of [...expansions,...aliases])for(const pre of ['','a','\u0301','-'])for(const post of ['','z','\u0308',"'"])add(pre+String.fromCharCode(row[0])+post,pre+String.fromCharCode(...row.slice(1))+post,'all-public-mapping-contexts');
const corpus=JSON.parse(zlib.brotliDecompressSync(fs.readFileSync(path.join(root,'reference/fixtures/windows-nls1-sort-60502.json.br')))),seeds=corpus.Strings.map(s=>Buffer.from(s.InputUtf16Base64,'base64').toString('utf16le')).filter(s=>s.length<=128);
let state=0x20261118;function next(){state^=state<<13;state^=state>>>17;state^=state<<5;return state>>>0;}
const ignore=['\u00ad','\u200d','\ufe00','\udb40\udd00'],repeat=['\u30fc','\u309d','\u309e','\u30fd','\u30fe','\u3005'],marks=['\u0300','\u0301','\u0308','\u034f'];
for(let i=0;i<4096;i++){
 const source=seeds[next()%seeds.length],at=next()%(source.length+1),pre=source.slice(0,at),post=source.slice(at);
 add(source,pre+ignore[next()%ignore.length]+post,'ignored-insertion');
 if(source.length){let index=next()%source.length;add(source,source.slice(0,index)+String.fromCharCode((source.charCodeAt(index)+1)&65535)+source.slice(index+1),'adjacent-unit-mutation');}else add(source,'\0','adjacent-unit-mutation');
 const exp=expansions[next()%expansions.length];add(pre+String.fromCharCode(exp[0])+post,pre+String.fromCharCode(exp[1],exp[2])+post,'expansion-insertion');
 add(pre+repeat[next()%repeat.length]+post,source,'repeat-insertion');
 add(source.replace(/[-']/g,''),pre+"-'"+post,'word-punctuation-mutation');
 let run=Array.from({length:8},()=>marks[next()%marks.length]);add(pre+run.join('')+post,pre+run.reverse().join('')+post,'mark-order-mutation');
}
for(let length of [127,128,129,255,256,257,1023,1024,1025,4095,4096,32767,65535])for(let mark of marks){add('a'+mark.repeat(length),'á'+mark.repeat(length-1),'long-mark-mutation');add(mark.repeat(length)+'a','a'+mark.repeat(length),'long-leading-mark');}
const output=path.join(root,'.local/evidence/nls-direct/stress-inputs.json');fs.mkdirSync(path.dirname(output),{recursive:true});fs.writeFileSync(output,JSON.stringify({SchemaVersion:1,Pairs:pairs}));console.log(JSON.stringify({pairs:pairs.length,bytes:fs.statSync(output).size,output}));
