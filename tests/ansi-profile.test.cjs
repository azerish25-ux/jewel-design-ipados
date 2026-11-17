'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),profile=require('../scripts/prepare-ansi-profile.cjs');
test('portable ANSI profile is the complete native-derived800-byte CP1256 mapping',()=>{const r=profile.verify();assert.equal(r.bytes,800);assert.equal(r.sha256,'3a88c315e6ffe2092d9e4f923ecab1a9b8fdf4976bcc438c636c0a899da2324d');assert.equal(r.decodeMappings,256);assert.equal(r.lowerMappings,256);});
test('portable ANSI profile rejects changed header, decode and lower mappings',()=>{for(const at of [0,16,32,544,799]){const changed=Buffer.from(profile.generate());changed[at]^=1;assert.throws(()=>profile.verify(changed));}});

test('decoder helper manifest retains optimized five-argument ABI without claiming remote PE verification',()=>{const r=profile.verifyDecoderSource();assert.equal(r.functions,2);assert.equal(r.totalBytes,1828);assert.equal(r.originalBytesChecked,false);assert.equal(r.originalExecuted,false);});
