const {test}=require('node:test');const assert=require('node:assert/strict');const {selectDevice}=require('../scripts/ios-device.cjs');
const data={devices:{'com.apple.CoreSimulator.SimRuntime.iOS-26-5':[{name:'iPad Pro',udid:'pad',isAvailable:true},{name:'iPhone 17',udid:'phone',isAvailable:true}],'com.apple.CoreSimulator.SimRuntime.iOS-9-0':[{name:'iPhone old',udid:'old',isAvailable:true}],'com.apple.CoreSimulator.SimRuntime.tvOS-26-5':[{name:'iPhone fake',udid:'tv',isAvailable:true}]}};
test('selects requested family from newest iOS runtime',()=>{assert.equal(selectDevice(data,'iphone').udid,'phone');assert.equal(selectDevice(data,'ipad').udid,'pad');});
test('does not fall back to another device family',()=>assert.throws(()=>selectDevice({devices:{'iOS-26-5':[{name:'iPad',isAvailable:true,udid:'pad'}]}},'iphone'),/No available/));
test('ignores unavailable devices',()=>assert.throws(()=>selectDevice({devices:{'iOS-26-5':[{name:'iPhone',isAvailable:false,udid:'x'}]}},'iphone'),/No available/));
test('rejects invalid family and malformed inventory',()=>{assert.throws(()=>selectDevice(data,'mac'),/family/);assert.throws(()=>selectDevice({},'ipad'),/inventory/);});
