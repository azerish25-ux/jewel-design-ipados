'use strict';
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');const {extract}=require('./verify-value-channel-oracle.cjs');const root=path.resolve(__dirname,'..');
const target='tests/Port/Documents.Contracts/Recovered/LegacyArrayData.cs';
const specs=[
 ['CSDatas/CSNumberData.cs','public override bool GetDouble(ref double value)'],
 ['CSDatas/CSDataArray.cs','private CSDataArray(CSDataArray input)'],
 ['CSDatas/CSDataArray.cs','public bool IsIndexValid(int index)'],
 ['CSDatas/CSDataArray.cs','public CSData GetArrayItem(int index)'],
 ['CSDatas/CSDataArray.cs','public CSDataArray GetFlattenArray()'],
 ['CSDatas/CSDataArray.cs','public override CSData Duplicate()'],
 ['CS.CSDataUtilities/CSDataHelper.cs','public static int GetArrayCount(CSData data)'],
 ['CS.CSDataUtilities/CSDataHelper.cs','public static CSData GetArrayItem(CSData data, int index)'],
 ['CSBlocks.Maths/CSAdd.cs','public override bool Update()']
];
function build(){
 const entries=specs.map(([file,signature])=>{const source='recovery/canonical/cad/'+file,bytes=fs.readFileSync(path.join(root,source)),body=extract(bytes.toString('utf8'),signature);return {source,signature,body,sourceSha256:crypto.createHash('sha256').update(bytes).digest('hex'),bodySha256:crypto.createHash('sha256').update(body).digest('hex')};});
 const code="#nullable disable\n// Exact source array/copy/arithmetic bodies with observation-only base values and channels.\nusing JewelDesign.Documents;\nnamespace DocumentContractChecks.Recovered.Arrays;\ninternal interface ICSArray{int GetArrayCount();CSData GetArrayItem(int index);CSDataArray GetFlattenArray();}\ninternal class CSData\n{\n    internal LegacyChannelValue Stored;\n    internal CSData(){Stored=LegacyChannelValue.Empty;}\n    internal CSData(LegacyChannelValue value){Stored=value;}\n    protected CSData(CSData copy){Stored=copy.Stored;}\n    public virtual bool GetDouble(ref double value)=>Stored.GetDouble(ref value);\n    public virtual CSData Duplicate()=>new CSData(this);\n}\ninternal sealed class CSNumberData:CSData\n{\n    private readonly double _numberData;internal CSNumberData(double value):base(LegacyChannelValue.FromNumber(value)){_numberData=value;}\n    public override CSData Duplicate()=>new CSNumberData(_numberData);\n"+entries[0].body+'\n}\ninternal sealed class CSDataArray:CSData,ICSArray{\ninternal List<CSData> Array{get;}=new();public CSDataArray(){}public int GetArrayCount()=>Array.Count;public void Append(CSData data){Array.Add(data);}\n'+entries.slice(1,6).map(x=>x.body).join('\n')+'\n}\ninternal static class CSDataHelper{\n'+entries.slice(6,8).map(x=>x.body).join('\n')+'\n}\ninternal sealed class Input{internal CSData Data;internal Input(CSData data){Data=data;}}\ninternal sealed class Output{internal CSData Data;internal void SetData(CSData data){Data=data;}}\ninternal class CSBlock{public virtual bool Update()=>true;}\ninternal sealed class CSAdd:CSBlock{\nprivate readonly Input iStart,iValue;internal readonly Output oValue=new(),oNegated=new(),oOneMinus=new();\ninternal CSAdd(CSData first,CSData second){iStart=new(first);iValue=new(second);}\n'+entries[8].body+'\n}\ninternal static class Values{\ninternal static CSData From(LegacyChannelValue value){if(value.IsMissing)return null;if(value.Array is{} items){var result=new CSDataArray();foreach(var child in items)result.Append(From(child));return result;}double n=0;return value.GetDouble(ref n)?new CSNumberData(n):new CSData(value);}\ninternal static LegacyChannelValue To(CSData value){if(value is null)return LegacyChannelValue.Missing;if(value is CSDataArray array)return LegacyChannelValue.FromArray(array.Array.Select(To));return value.Stored;}\n}\n';
 return {target,code,entries};
}
function verify(code){if(code.replaceAll('\r\n','\n')!==build().code)throw Error('Array source bodies differ from canonical');return true;}
if(require.main===module){const result=build();if(process.argv.includes('--write')){fs.mkdirSync(path.dirname(path.join(root,target)),{recursive:true});fs.writeFileSync(path.join(root,target),result.code);}verify(fs.readFileSync(path.join(root,target),'utf8'));console.log(JSON.stringify({target,entries:result.entries.map(({body,...x})=>x)},null,2));}
module.exports={build,verify};
