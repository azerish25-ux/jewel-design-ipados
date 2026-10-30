const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const {build,verify}=require('../scripts/verify-rotation-oracle.cjs');
test('recovered rotation Update and ToRadians bodies remain exact',()=>assert.equal(verify(fs.readFileSync(path.join(__dirname,'../',build().target),'utf8')),true));
test('rotation order tamper is rejected',()=>assert.throws(()=>verify(build().code.replace('transform * transform2 * transform3','transform3 * transform2 * transform')),/differs/));
test('rotation mirror threshold tamper is rejected',()=>assert.throws(()=>verify(build().code.replace('value4 > 0.5','value4 >= 0.5')),/differs/));
test('degree conversion tamper is rejected',()=>assert.throws(()=>verify(build().code.replace('On_DEGREES_TO_RADIANS * angleDegrees','On_DEGREES_TO_RADIANS / angleDegrees')),/differs/));
