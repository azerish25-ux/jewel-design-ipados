const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const {specs,build}=require('../scripts/prepare-rotation-reference.cjs');
const folder=path.join(__dirname,'../.local/reference/opennurbs6-rotation');
const files=Object.fromEntries(specs.map(x=>[x.file,fs.readFileSync(path.join(folder,x.file))]));
test('published reference files and extracted bodies match recorded immutable pins',()=>{const result=build(files);assert.equal(result.report.generatedSha256,'40fdcbece9ffcf20b50c02472869a337a96235f0327fd31b5301ed1751b09560');assert.equal(result.report.methods.length,2);assert.ok(result.code.includes('Copyright (c)'));});
test('modified published rotation source is rejected before extraction',()=>assert.throws(()=>build({...files,'opennurbs_xform.cpp':Buffer.concat([files['opennurbs_xform.cpp'],Buffer.from(' ')])}),/hash mismatch/));
test('changed comparison epsilon constants are rejected',()=>assert.throws(()=>build({...files,'opennurbs_defines.h':Buffer.from(files['opennurbs_defines.h'].toString().replace('1.490116119385000000e-8','1.490116119385000000e-6'))}),/hash mismatch/));
test('missing or changed comparison notice cannot be accepted',()=>assert.throws(()=>build({...files,LICENSE:Buffer.from('altered')}),/hash mismatch/));
