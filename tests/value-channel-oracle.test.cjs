const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const {build,extract,verify}=require('../scripts/verify-value-channel-oracle.cjs');
test('all seven channel oracle bodies match the exact canonical source extraction',()=>{
 const result=build();assert.equal(result.entries.length,7);
 assert.equal(verify(fs.readFileSync(path.join(__dirname,'../tests/Port/Documents.Contracts/Recovered/LegacyValueChannels.cs'),'utf8')),true);
});
test('source arithmetic and malformed/ambiguous extraction cannot silently drift',()=>{
 const result=build();assert.throws(()=>verify(result.code.replace('0.0 - Value','-Value')),/differ/);
 assert.throws(()=>extract('public void A(){','public void A()'),/Unclosed/);
 assert.throws(()=>extract('public void A(){} public void A(){}','public void A()'),/unique/);
});
