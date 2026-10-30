"use strict";
const fs=require("node:fs"),path=require("node:path"),cp=require("node:child_process"),assert=require("node:assert/strict");
const {env,dotnetPath}=require("./local-env.cjs");
const {parse}=require("../reference/file-proof/csg-v4.cjs");
const root=path.resolve(__dirname,".."),testRoot=path.join(root,".local/file-proof"),folder=path.join(testRoot,"production");
const names=["source","rebuilt","edited","restored"],models={};
for(const name of names)models[name]=parse(fs.readFileSync(path.join(folder,name+".csg")));
const fixture="73e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd";
for(const name of ["source","rebuilt","restored"])assert.equal(models[name].sha256,fixture);
assert.equal(models.edited.nodes[8].numeric.value,4.48);assert.equal(models.source.nodes[8].numeric.value,4.38);
assert.equal(models.edited.count,241);assert.equal(models.edited.numericCount,74);
assert.deepEqual(models.source.regions,models.edited.regions);
const oldSelected={...models.source.nodes[8].numeric},newSelected={...models.edited.nodes[8].numeric};delete oldSelected.value;delete newSelected.value;assert.deepEqual(oldSelected,newSelected);
for(let i=0;i<241;i++)if(i!==8)assert.equal(models.source.nodes[i].rawHash,models.edited.nodes[i].rawHash);
const logs=path.join(folder,"independent");fs.mkdirSync(logs,{recursive:true});
function run(label,args){const file=path.join(logs,label+".log"),fd=fs.openSync(file,"w");let result;try{result=cp.spawnSync(dotnetPath,args,{cwd:root,env,stdio:["ignore",fd,fd],timeout:120000});}finally{fs.closeSync(fd);}assert.equal(result.status,0,"Independent verifier failure: "+label+" "+fs.readFileSync(file,"utf8"));}
const bin=path.join(folder,"verifier");
run("build",["build",path.join(root,"reference/file-proof/IndependentVerifier"),"-o",bin,"--nologo","-v:q","-p:UseSharedCompilation=false","-nr:false"]);
for(const name of names){const out=path.join(logs,name+".json");run(name,[path.join(bin,"IndependentVerifier.dll"),path.join(folder,name+".csg"),out,testRoot]);const data=JSON.parse(fs.readFileSync(out));assert.equal(data.NodeCount,241);assert.equal(data.NumericNodes.length,74);assert.equal(data.Sha256,models[name].sha256);for(const node of models[name].nodes){assert.equal(data.Nodes[node.index].RawSha256,node.rawHash);if(node.numeric){const value=data.NumericNodes.find(x=>x.Index===node.index);assert.equal(value.Value,node.numeric.value);assert.equal(value.Identifier,node.numeric.identifier);assert.deepEqual(value.Objects,node.numeric.objects);assert.deepEqual(value.Channels,node.numeric.channels);assert.deepEqual(value.Categories,node.numeric.categories);}}}
const report={scope:"Production structural writer independently read by Node/zlib and separate CSharp verifier; no original PC app or geometry regeneration",fixtureSha256:fixture,records:241,numericRecords:74,outputs:Object.fromEntries(names.map(name=>[name,{sha256:models[name].sha256,independentReaders:["Node/zlib","CSharp bitwise CRC"]}]))};
fs.writeFileSync(path.join(folder,"production-crosscheck.json"),JSON.stringify(report,null,2)+"\n");
console.log("PASS production CSG4 crosscheck: four outputs, 241 records/74 numeric, two independent readers and exact source/rebuild/reversal hashes");
