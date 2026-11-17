'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),reference=require('../scripts/verify-ansi-source-contract.cjs');
test('original conversion contract retains nine pinned whole functions without claiming original execution',()=>{const r=reference.verify();assert.equal(r.functions,9);assert.ok(r.chainedRegions>20);assert.equal(r.originalBytesChecked,false);assert.equal(r.originalExecuted,false);});
