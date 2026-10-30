using JewelDesign.Documents;
using Oracle=DocumentContractChecks.Recovered.Arrays;
namespace DocumentContractChecks;

internal static class ArrayValueAssertions
{
    private static void Check(bool v,string message){if(!v)throw new Exception(message);}
    private static void Throws<T>(Action action)where T:Exception{try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
    private static LegacyChannelValue N(double x)=>LegacyChannelValue.FromNumber(x);
    private static LegacyChannelValue A(params LegacyChannelValue[] values)=>LegacyChannelValue.FromArray(values);
    private static void Same(LegacyChannelValue a,LegacyChannelValue b)
    {
        Check(a.IsMissing==b.IsMissing&&a.HasNumber==b.HasNumber&&a.Profile==b.Profile&&a.Curve==b.Curve&&a.Transform==b.Transform&&(a.Array is null)==(b.Array is null),"Array kind/reference mismatch");
        if(a.HasNumber){double x=0,y=0;a.GetDouble(ref x);b.GetDouble(ref y);Check(BitConverter.DoubleToInt64Bits(x)==BitConverter.DoubleToInt64Bits(y),"Array numeric bits changed");}
        if(a.Array is{} items){Check(items.Count==b.Array!.Count,"Array length changed");for(int i=0;i<items.Count;i++)Same(items[i],b.Array[i]);}
    }
    private static bool AddCase(LegacyChannelValue first,LegacyChannelValue second)
    {
        var source=new Oracle.CSAdd(Oracle.Values.From(first),Oracle.Values.From(second));bool sourceNull=false;
        try{Check(source.Update(),"Source Add failed");}catch(NullReferenceException){sourceNull=true;}
        if(sourceNull){Throws<NullReferenceException>(()=>LegacyArrayArithmetic.Add(first,second));return false;}
        var actual=LegacyArrayArithmetic.Add(first,second);
        Same(actual.Value,Oracle.Values.To(source.oValue.Data));Same(actual.Negated,Oracle.Values.To(source.oNegated.Data));Same(actual.OneMinus,Oracle.Values.To(source.oOneMinus.Data));return true;
    }
    public static string Run()
    {
        var empty=LegacyChannelValue.Empty;var missing=LegacyChannelValue.Missing;var curve=LegacyChannelValue.FromCurve(4,0);
        var matrix=LegacyChannelValue.FromTransform(LegacyAffineTransform.Identity);
        var choices=new[]{missing,empty,curve,matrix,N(0),N(BitConverter.Int64BitsToDouble(long.MinValue)),N(1),N(-3),N(double.NaN),N(double.PositiveInfinity),N(double.NegativeInfinity),A(),A(N(1),empty,N(2)),A(empty,N(3),empty),A(missing),A(A(N(2)),N(5)),A(N(1),A(),curve)};
        int pairs=0,nullFailures=0,structures=0;
        foreach(var first in choices)foreach(var second in choices){if(!AddCase(first,second))nullFailures++;pairs++;}
        var arrays=new List<LegacyChannelValue>();
        foreach(var a in choices)foreach(var b in choices)arrays.Add(A(a,b));
        var random=new Random(73013);
        LegacyChannelValue Tree(int depth){
            if(depth==0||random.Next(3)==0)return choices[random.Next(11)];
            return LegacyChannelValue.FromArray(Enumerable.Range(0,random.Next(5)).Select(_=>Tree(depth-1)));
        }
        for(int i=0;i<500;i++)arrays.Add(A(Tree(4),Tree(3)));
        foreach(var array in arrays){
            var source=(Oracle.CSDataArray)Oracle.Values.From(array);
            Same(LegacyArrayData.Flatten(array),Oracle.Values.To(source.GetFlattenArray()));
            Same(LegacyArrayData.Duplicate(array),Oracle.Values.To(source.Duplicate()));
            double number=123;bool boolean=true;Check(!array.GetDouble(ref number)&&number==123&&!array.GetBoolean(ref boolean)&&boolean,"Array converted to a scalar");
            structures++;
        }
        // Failed conversions retain previous values in source loop order; nested arrays are not scalarized.
        var carried=LegacyArrayArithmetic.Add(A(N(2),empty,N(4)),A(N(10),empty,N(20)));
        Same(carried.Value,A(N(12),N(12),N(22),N(12),N(12),N(22),N(14),N(14),N(24)));
        Same(LegacyArrayArithmetic.Add(A(N(2),empty,N(4)),A(empty,N(20))).Value,A(N(2),N(22),N(22),N(22),N(24),N(24)));
        Same(LegacyArrayArithmetic.Add(A(),A()).Value,N(0));
        Same(LegacyArrayData.Flatten(A(missing,A(N(1),missing),A())),A(empty,N(1),empty));
        Same(LegacyArrayData.Duplicate(A(missing,A(missing,N(-0.0)))),A(empty,A(empty,N(-0.0))));
        var mutable=new[]{N(1),missing};var captured=LegacyChannelValue.FromArray(mutable);mutable[0]=N(9);Same(captured,A(N(1),missing));
        Throws<NotSupportedException>(()=>((IList<LegacyChannelValue>)captured.Array!)[0]=N(9));
        Same(LegacyChannelValue.FromArray(new LegacyChannelValue?[]{null,N(2)}),A(missing,N(2)));
        LegacyChannelValue deep=N(1);for(int i=0;i<LegacyArrayData.MaximumDepth;i++)deep=A(deep);
        Same(LegacyArrayData.Flatten(deep),A(N(1)));LegacyArrayData.Duplicate(deep);
        var tooDeep=A(deep);Throws<NotSupportedException>(()=>LegacyArrayData.Flatten(tooDeep));Throws<NotSupportedException>(()=>LegacyArrayData.Duplicate(tooDeep));
        var wide=LegacyChannelValue.FromArray(Enumerable.Repeat(N(1),LegacyArrayData.MaximumItems));Check(LegacyArrayData.Flatten(wide).Array!.Count==LegacyArrayData.MaximumItems,"Flatten item boundary lost");LegacyArrayData.Duplicate(wide);
        Throws<NotSupportedException>(()=>LegacyChannelValue.FromArray(Enumerable.Repeat(N(1),LegacyArrayData.MaximumItems+1)));
        var sharedTree=LegacyChannelValue.FromArray(Enumerable.Repeat(wide,LegacyArrayData.MaximumItems));Throws<NotSupportedException>(()=>LegacyArrayData.Flatten(sharedTree));
        var left=LegacyChannelValue.FromArray(Enumerable.Repeat(N(1),256));var right=LegacyChannelValue.FromArray(Enumerable.Repeat(N(2),256));Check(LegacyArrayArithmetic.Add(left,right).Value.Array!.Count==65536,"Cartesian result boundary lost");
        Throws<NotSupportedException>(()=>LegacyArrayArithmetic.Add(A(N(1),N(2)),wide));
        Throws<ArgumentException>(()=>LegacyArrayData.Flatten(N(1)));
        Throws<ArgumentException>(()=>LegacyArrayData.Duplicate(N(1)));
        return "PASS legacy array values:"+pairs+" exact-source arithmetic pairs/"+nullFailures+" null-item failures;"+structures+" source flatten/copy trees, immutable missing/empty/nested/bit fidelity, Cartesian carry/order and bounded failures; pure references only, no native snapshot/producer execution claim.";
    }
}
