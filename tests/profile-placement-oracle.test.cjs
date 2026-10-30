const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const {build,verify}=require('../scripts/verify-profile-placement-oracle.cjs');
test('exact frame/helper/seam/height source fragments match canonical',()=>assert.equal(verify(fs.readFileSync(path.join(__dirname,'../',build().target),'utf8')),true));
test('frame handedness tamper is rejected',()=>assert.throws(()=>verify(build().code.replace('ON_CrossProduct(on3dVector3, arg2)','ON_CrossProduct(arg2, on3dVector3)')),/differ/));
test('normalized-fraction seam comparison cannot silently change',()=>assert.throws(()=>verify(build().code.replace('value - nurbsCurve.Domain().Max()','nativeParameter - nurbsCurve.Domain().Max()')),/differ/));
test('height floor tamper is rejected',()=>assert.throws(()=>verify(build().code.replace('num < 0.01','num < 0.1')),/differ/));
