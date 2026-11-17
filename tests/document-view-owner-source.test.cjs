'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),reference=require('../scripts/verify-document-view-owner-source.cjs');
test('document/view source retains complete owner/helper regions and explicit output-pointer ABIs',()=>{const r=reference.verify();assert.equal(r.functions,7);assert.equal(r.totalPinnedBytes,4260);assert.equal(r.explicitWin32Abis,3);assert.equal(r.sourceImageChecked,false);assert.equal(r.originalExecuted,false);});
