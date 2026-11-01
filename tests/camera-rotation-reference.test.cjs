'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const {inputs,verify}=require('../scripts/generate-camera-rotation-cases.cjs');
const root=path.resolve(__dirname,'..'),fixture=JSON.parse(fs.readFileSync(path.join(root,'tests/Port/Geometry.Candidate.Contracts/Fixtures/camera-rotation-source-cases.json'),'utf8'));
test('rotation fixture preserves deterministic independent inputs and difficult branches',()=>{
 assert.equal(fixture.cases.length,608);assert.deepEqual(fixture.cases.map(x=>x.args),inputs());
 assert(fixture.cases.some(x=>x.diagnostics===1));assert(fixture.cases.some(x=>x.args.includes('1e+308')));
 assert(fixture.cases.some(x=>x.args.includes('5e-324')));assert(fixture.cases.every(x=>x.vector.length===3));
});
test('Apple preflight regenerates independent reference and executes production assertions',()=>{
 const workflow=fs.readFileSync(path.join(root,'.github/workflows/metal-preflight.yml'),'utf8');
 for(const required of ['scripts/prepare-camera-rotation-reference.cjs --acquire','-ffp-contract=off','native/Rendering/CameraRotationReference/reference.cpp','scripts/generate-camera-rotation-cases.cjs .local/native/camera-rotation-reference','dotnet run --project tests/Port/Rendering.Tests'])assert(workflow.includes(required),required);
 assert(fs.readFileSync(path.join(root,'tests/iOS.Viewport/Main.cs'),'utf8').includes('Rhino5RotationAssertions.Run()'));
});

test('reference comparison rejects altered inputs, diagnostics, values and malformed outputs',()=>{
 for(const mutate of [r=>r.args[1]='.123',r=>r.diagnostics++,r=>r.vector[0]='100',r=>r.vector.pop(),r=>r.vector[0]='garbage',r=>r.vector[0]='nan',r=>r.vector[0]='']){
  const rows=structuredClone(fixture.cases);mutate(rows[0]);assert.throws(()=>verify(rows,fixture.cases));
 }
 assert(verify(fixture.cases,fixture.cases));
});
