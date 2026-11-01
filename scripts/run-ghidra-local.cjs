'use strict';
const fs=require('node:fs'),cp=require('node:child_process'),path=require('node:path').win32;
const version='11.1.2',relativeTool='.local/tools/ghidra/ghidra_11.1.2_PUBLIC';
function configuration({root,javaExe,baseEnv={},vmArgs=[],args=[]}){
 root=path.resolve(root);
 if(!/^d:[\\/]/i.test(root)||root===path.parse(root).root)throw Error('Ghidra workspace must be a named D: folder');
 const prefix=root.toLowerCase()+path.sep;
 function confined(value){if(typeof value!=='string'||!value)throw Error('Missing Ghidra path');const result=path.resolve(root,value);if(result.toLowerCase()!==root.toLowerCase()&&!result.toLowerCase().startsWith(prefix))throw Error('Ghidra writable path leaves workspace: '+value);return result;}
 if(args.length<2)throw Error('Supply headless project directory and project name');
 confined(args[0]);if(!/^[A-Za-z0-9_-][A-Za-z0-9_.-]*$/.test(args[1]))throw Error('Invalid Ghidra project name');
 for(let i=2;i<args.length;i++)if(['-log','-scriptlog','-scriptPath','-propertiesPath'].includes(args[i])){const value=args[++i];if(!value)throw Error('Missing headless path argument');for(const entry of value.split(';'))confined(entry);}
 const runtime=path.join(root,'.local/ghidra-runtime'),locations={};
 for(const name of ['home','settings','cache','temp','jython','appdata','localappdata'])locations[name]=path.join(runtime,name);
 const env={...baseEnv,USERPROFILE:locations.home,APPDATA:locations.appdata,LOCALAPPDATA:locations.localappdata,
  TEMP:locations.temp,TMP:locations.temp,TMPDIR:locations.temp,XDG_CONFIG_HOME:locations.settings,XDG_CACHE_HOME:locations.cache,
  GHIDRA_USER_HOME:locations.home,JAVA_TOOL_OPTIONS:'',JDK_JAVA_OPTIONS:'',_JAVA_OPTIONS:''};
 const properties={'user.home':locations.home,'java.io.tmpdir':locations.temp,'application.settingsdir':locations.settings,
  'application.cachedir':locations.cache,'application.tempdir':locations.temp,'python.cachedir':locations.jython};
 const toolRoot=path.join(root,relativeTool),classpath=path.join(toolRoot,'Ghidra/Framework/Utility/lib/Utility.jar');
 const javaArgs=[...vmArgs,'-Xmx2G','-XX:ParallelGCThreads=2','-XX:CICompilerCount=2','-Djava.awt.headless=true','-Dcpu.core.limit=2',
  ...Object.entries(properties).map(([key,value])=>'-D'+key+'='+value),'-cp',classpath,'ghidra.Ghidra','ghidra.app.util.headless.AnalyzeHeadless',...args];
 return {root,version,javaExe,javaArgs,env,dirs:Object.values(locations),toolRoot};
}
function headlessOutputHasError(text){return /^\s*(?:\d{4}-\d{2}-\d{2}[^\n]*?\s)?ERROR\b/m.test(text);}
function executionExitCode(status,stdout='',stderr=''){if(status!==0)return Number.isInteger(status)?status:1;return headlessOutputHasError(stdout+'\n'+stderr)?1:0;}
function main(){
 if(process.platform!=='win32')throw Error('This launcher is for the local Windows D: workspace');
 const local=require('./local-env.cjs');
 const toolRoot=path.join(local.root,relativeTool),properties=fs.readFileSync(path.join(toolRoot,'support/launch.properties'),'utf8');
 const application=fs.readFileSync(path.join(toolRoot,'Ghidra/application.properties'),'utf8');
 if(!/^application.version=11\.1\.2\r?$/m.test(application))throw Error('Pinned Ghidra version differs');
 const vmArgs=[...properties.matchAll(/^VMARGS(?:_WINDOWS)?=(.+)\r?$/gm)].map(m=>m[1].trim());
 const config=configuration({root:local.root,javaExe:process.env.GHIDRA_JAVA||'C:/Program Files/Microsoft/jdk-17.0.20.101-hotspot/bin/java.exe',baseEnv:local.env,vmArgs,args:process.argv.slice(2)});
 const realRoot=fs.realpathSync(config.root).toLowerCase();
 if(!/^d:[\\/]/i.test(realRoot))throw Error('Resolved workspace leaves D:');
 for(const dir of config.dirs){fs.mkdirSync(dir,{recursive:true});if(!fs.realpathSync(dir).toLowerCase().startsWith(realRoot+path.sep))throw Error('Resolved Ghidra directory leaves workspace');}
 if(!fs.existsSync(config.javaExe))throw Error('Existing JDK missing; set GHIDRA_JAVA to its java executable');
 const child=cp.spawn(config.javaExe,config.javaArgs,{cwd:config.root,env:config.env,stdio:['inherit','pipe','pipe']});
 let failure=false;const tails={stdout:'',stderr:''};
 function relay(name,destination,chunk){const text=chunk.toString('utf8');failure=failure||headlessOutputHasError(tails[name]+text);tails[name]=(tails[name]+text).slice(-1024);destination.write(chunk);}
 child.stdout.on('data',chunk=>relay('stdout',process.stdout,chunk));child.stderr.on('data',chunk=>relay('stderr',process.stderr,chunk));
 child.on('error',error=>{process.stderr.write(error.message+'\n');process.exitCode=1;});
 child.on('close',status=>{if(status===0&&failure)process.stderr.write('Ghidra exited0 but logged ERROR; refusing success.\n');process.exitCode=executionExitCode(status,failure?'ERROR Ghidra logged a failure':'','');});
}
if(require.main===module)main();
module.exports={configuration,executionExitCode};
