const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
function checkPlacement(text) {
 const jobs = [...text.replace(/\r/g, '').matchAll(/^  ([a-z][a-z0-9_-]*):\n([\s\S]*?)(?=^  [a-z][a-z0-9_-]*:|$(?![\s\S]))/gm)];
 const metal = jobs.filter(x => x[2].includes('dotnet build tests/iOS.Metal.Compile'));
 assert.equal(metal.length, 1, 'Exactly one Metal preflight job');
 assert.match(metal[0][2], /runs-on: macos-26/, 'Metal preflight requires macOS');
 assert.match(metal[0][2], /dotnet workload install ios --version 10\.0\.302\.0/, 'Pinned iOS workload required');
 assert.ok(metal[0][2].indexOf('dotnet workload install ios') < metal[0][2].indexOf('dotnet build tests/iOS.Metal.Compile'), 'Install workload before preflight');
}
test('Native Metal preflight runs on prepared Apple runner', () => checkPlacement(fs.readFileSync(path.join(root, '.github/workflows/native-geometry.yml'), 'utf8')));
test('Regression rejects Metal preflight moved into Linux host job', () => {
 const text = fs.readFileSync(path.join(root, '.github/workflows/native-geometry.yml'), 'utf8');
 const step = '      - name: Compile Metal renderer bindings before kernel build\n        run: dotnet build tests/iOS.Metal.Compile -c Debug -r iossimulator-arm64\n';
 const broken = text.replace(/\r/g, '').replace(step, '').replace('      - name: Configure genuine modeling kernel\n', step + '      - name: Configure genuine modeling kernel\n');
 assert.throws(() => checkPlacement(broken), /requires macOS/);
});

test('Metal preflight does not hide missing imports with implicit global usings', () => {
 const project = fs.readFileSync(path.join(root, 'tests/iOS.Metal.Compile/Metal.Compile.csproj'), 'utf8');
 assert.match(project, /<ImplicitUsings>disable<\/ImplicitUsings>/);
 const harness = fs.readFileSync(path.join(root, 'tests/iOS.Native/MetalViewportChecks.cs'), 'utf8');
 assert.match(harness, /^using System;$/m);
 assert.match(harness, /^using System\.IO;$/m);
});
