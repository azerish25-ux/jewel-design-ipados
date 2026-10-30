const fs=require('fs');
function selectDevice(inventory,family){
 if(!['ipad','iphone'].includes(family))throw Error('Unsupported device family');
 if(!inventory?.devices||typeof inventory.devices!=='object')throw Error('Malformed simulator inventory');
 const candidates=[];
 for(const[runtime,devices]of Object.entries(inventory.devices)){
  const version=/(?:^|[.])iOS[-.]([\d.-]+)$/.exec(runtime);if(!version||!Array.isArray(devices))continue;
  for(const device of devices)if(device.isAvailable===true&&typeof device.udid==='string'&&
   (family==='ipad'?/^iPad\b/:/^iPhone\b/).test(device.name||''))candidates.push({family,runtime,name:device.name,udid:device.udid,version:version[1].split(/[.-]/).map(Number)});
 }
 candidates.sort((a,b)=>{for(let i=0;i<Math.max(a.version.length,b.version.length);i++){const difference=(b.version[i]||0)-(a.version[i]||0);if(difference)return difference;}return a.name.localeCompare(b.name)||a.udid.localeCompare(b.udid);});
 if(!candidates.length)throw Error('No available '+family+' simulator in iOS inventory');
 const{version,...selected}=candidates[0];return selected;
}
if(require.main===module){const[file,family]=process.argv.slice(2);console.log(JSON.stringify(selectDevice(JSON.parse(fs.readFileSync(file,'utf8')),family),null,2));}
module.exports={selectDevice};
