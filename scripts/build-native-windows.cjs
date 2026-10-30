const cp=require('child_process'),path=require('path');
const root=path.resolve(__dirname,'..'),wrapper=path.join(root,'scripts/run-local.cjs');
if(process.platform!=='win32'||!path.parse(root).root.toUpperCase().startsWith('D:'))throw Error('This local Windows helper requires the D: workspace; use macOS CI for Apple builds.');
function run(command,args){const result=cp.spawnSync(process.execPath,[wrapper,command,...args],{cwd:root,stdio:'inherit'});if(result.error)throw result.error;if(result.status!==0)throw Error(command+' failed with exit '+result.status);}
const build=path.join(root,'.local/native/windows-gnu'),source=path.join(root,'.local/native/occt-source');
run('g++',['--version']);
run('cmake',['-S','native/GeometryBridge/CandidateProbe','-B',build,'-G','Ninja','-DCMAKE_BUILD_TYPE=Release','-DOCCT_SOURCE_DIR='+source]);
run('cmake',['--build',build,'--target','jewel_occt_candidate_objects','--parallel','2']);
run('cmake',['--build',build,'--target','occt_candidate_test','--parallel','4']);
run(path.join(build,'occt_candidate_test.exe'),[]);
