'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),profiles=require('../scripts/prepare-crt100-profiles.cjs');
test('portable CRT resources reconstruct separate whole-lower and comparison data from native captures',()=>{const r=profiles.verify();assert.equal(r.bytes,16476);assert.equal(r.sha256,'7aad75e02af1cab78bc7fc4d9a71bcec227a1fbc116cb0069a81e817691133cd');assert.deepEqual(r.profiles.map(p=>[p.compareRows,p.lowerRows,p.supplementaryRows]),[[26,26,0],[973,973,40],[972,973,40]]);});
test('portable CRT data rejects changed header or lower/comparison records',()=>{for(const position of [0,24,32,16475]){const copy=Buffer.from(profiles.generate().bytes);copy[position]^=1;assert.throws(()=>profiles.verify(copy));}});
