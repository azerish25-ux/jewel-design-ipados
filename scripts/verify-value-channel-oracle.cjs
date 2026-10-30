"use strict";
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..');
function extract(source,signature){
 const s=source.replaceAll('\r\n','\n'),start=s.indexOf(signature);
 if(start<0||s.indexOf(signature,start+signature.length)>=0)throw Error('Expected unique signature '+signature);
 const open=s.indexOf('{',start);let depth=0;
 for(let p=open;p<s.length;p++){if(s[p]==='{')depth++;else if(s[p]==='}'&&!--depth)return s.slice(start,p+1);}
 throw Error('Unclosed source body');
}
const specs=[
 ['recovery/canonical/cad/CSDatas/CSNumberData.cs','public override bool GetDouble(ref double value)'],
 ['recovery/canonical/cad/CSDatas/CSNumberData.cs','public override bool GetBoolean(ref bool value)'],
 ['recovery/canonical/cad/CS.CSValues/CSValue.cs','public double Value'],
 ['recovery/canonical/cad/CS.CSValues/CSValue.cs','protected bool IsOn()'],
 ['recovery/canonical/cad/CS.CSValues/CSValue.cs','public override bool Update()'],
 ['recovery/canonical/cad/CS.CSValues/CSValue.cs','public override CSData GetOutputChannelData(int outputChannelIndex)'],
 ['recovery/canonical/cad/CS.CSValues/CSValueOnOff.cs','public override CSData GetOutputChannelData(int outputChannelIndex)']
];
function build(){
 const entries=specs.map(([file,signature])=>{const bytes=fs.readFileSync(path.join(root,file)),body=extract(bytes.toString('utf8'),signature);return {file,signature,body,sourceSha256:crypto.createHash('sha256').update(bytes).digest('hex'),bodySha256:crypto.createHash('sha256').update(body).digest('hex')};});
 const code="// Generated test-only source extraction; observation stubs are not native/graph substitutes.\nnamespace DocumentContractChecks.Recovered.ValueChannels;\ninternal class CSData\n{\n    public virtual bool GetDouble(ref double value)=>false;\n    public virtual bool GetBoolean(ref bool value)=>false;\n}\ninternal sealed class CSNumberData:CSData\n{\n    private readonly double _numberData;\n    internal CSNumberData(double value){_numberData=value;}\n"+entries.slice(0,2).map(x=>x.body).join('\n')+"}\ninternal sealed class Input\n{\n    private readonly CSData data;\n    internal Input(CSData data){this.data=data;}\n    internal bool GetDouble(ref double value)=>data.GetDouble(ref value);\n    internal bool GetBoolean(ref bool value)=>data.GetBoolean(ref value);\n}\ninternal sealed class CSOutputChannel{}\ninternal class CSBlock\n{\n    protected readonly List<CSOutputChannel> OutputChannels=new();\n    public virtual bool Update()=>true;\n    public virtual CSData GetOutputChannelData(int index)=>new CSData();\n}\ninternal static class CSApplicationHelper\n{\n    internal sealed class App{public bool IsCamValueMode;}\n    internal static readonly App Application=new();\n}\ninternal class CSValue:CSBlock\n{\n    protected double _doubleValue,_previousValue;\n    private readonly bool _absoluteCAM;\n    private readonly double _doubleCAMAbsolute,_doubleCAMDelta;\n    protected readonly Input InOn,InOverride;\n    protected readonly CSOutputChannel OutValue=new(),OutNegated=new(),OutOneMinus=new();\n    internal double Real=>_doubleValue;\n    internal double Previous=>_previousValue;\n    internal CSValue(double value,double previous,bool cam,bool absolute,double absoluteValue,double delta,CSData on,CSData over)\n    {\n        _doubleValue=value;_previousValue=previous;_absoluteCAM=absolute;_doubleCAMAbsolute=absoluteValue;_doubleCAMDelta=delta;\n        CSApplicationHelper.Application.IsCamValueMode=cam;InOn=new(on);InOverride=new(over);\n        OutputChannels.AddRange(new[]{OutValue,OutNegated,OutOneMinus});\n    }\n"+entries.slice(2,6).map(x=>x.body).join('\n')+"}\ninternal sealed class CSValueOnOff:CSValue\n{\n    private readonly CSOutputChannel OutOnOff=new(),OutGroupOnOff=new();\n    internal CSValueOnOff(double value,double previous,bool cam,bool absolute,double absoluteValue,double delta,CSData on,CSData over)\n        :base(value,previous,cam,absolute,absoluteValue,delta,on,over)\n    {OutputChannels.AddRange(new[]{OutOnOff,OutGroupOnOff});}\n"+entries[6].body+'\n}\n';
 return {code,entries:entries.map(({body,...x})=>x)};
}
const target='tests/Port/Documents.Contracts/Recovered/LegacyValueChannels.cs';
function verify(harness){if(harness.replaceAll('\r\n','\n')!==build().code)throw Error('Extracted value-channel bodies differ from canonical source');return true;}
if(require.main===module){const result=build();if(process.argv.includes('--write')){fs.mkdirSync(path.dirname(path.join(root,target)),{recursive:true});fs.writeFileSync(path.join(root,target),result.code);}
 if(fs.readFileSync(path.join(root,target),'utf8').replaceAll('\r\n','\n')!==result.code)throw Error('Extracted value-channel bodies differ from canonical source');
 console.log(JSON.stringify({target,methods:result.entries.length,entries:result.entries},null,2));}
module.exports={build,extract,verify};
