'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),reference=require('../scripts/verify-crt100-reference.cjs');
test('actual Microsoft CRT100 locales retain complete lower units and repeated native comparisons',()=>{const r=reference.verify(reference.load());assert.equal(r.lowerUnits,196608);assert.equal(r.repeatedPairs,3468);assert.equal(r.crtCMatchesUcrt,true);});
test('CRT100 reference rejects changed data, repeats and profile identities',()=>{for(const mutate of [r=>r.RuntimeSha256='0'.repeat(64),r=>r.Profiles[0].LowerSha256='0'.repeat(64),r=>r.Profiles[1].Comparisons[0].Repeat=999,r=>r.Profiles[2].Locale='C']){const r=reference.load();mutate(r);assert.throws(()=>reference.verify(r));}});
