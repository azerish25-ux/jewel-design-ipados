'use strict';
const fs=require('node:fs'),path=require('node:path'),cp=require('node:child_process');
const {root,local,env}=require('./local-env.cjs');
const [command,...args]=process.argv.slice(2);if(!command)throw Error('Expected oracle command');
const profile=path.join(local,'archive-oracle-profile'),roaming=path.join(profile,'AppData','Roaming'),appLocal=path.join(profile,'AppData','Local'),extensions=path.join(appLocal,'Microsoft','MSBuild');
for(const dir of [profile,roaming,appLocal,extensions])fs.mkdirSync(dir,{recursive:true});
const childEnv={...env,VSCMD_SKIP_SENDTELEMETRY:'1'};
if(process.platform==='win32')Object.assign(childEnv,{USERPROFILE:profile,APPDATA:roaming,LOCALAPPDATA:appLocal,MSBuildUserExtensionsPath:extensions});
const result=cp.spawnSync(command,args,{cwd:root,env:childEnv,stdio:'inherit'});if(result.error)throw result.error;process.exitCode=result.status??1;
