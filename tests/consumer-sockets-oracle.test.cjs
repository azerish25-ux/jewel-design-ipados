const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const {build,verify}=require('../scripts/verify-consumer-sockets-oracle.cjs');
test('socket assignment/HasLink/expansion source bodies remain exact',()=>assert.equal(verify(fs.readFileSync(path.join(__dirname,'../',build().target),'utf8')),true));
test('source negative channel link condition tamper rejected',()=>assert.throws(()=>verify(build().code.replace('OutputChannelIndex >= 0','OutputChannelIndex > 0')),/differ/));
test('source unlinked slot reuse tamper rejected',()=>assert.throws(()=>verify(build().code.replace('base.InputChannels.Last().HasLink','!base.InputChannels.Last().HasLink')),/differ/));
test('source saved assignment target tamper rejected',()=>assert.throws(()=>verify(build().code.replace('InputChannels[index].OutputChannelIndex = outputChannelIndex','InputChannels[0].OutputChannelIndex = outputChannelIndex')),/differ/));
