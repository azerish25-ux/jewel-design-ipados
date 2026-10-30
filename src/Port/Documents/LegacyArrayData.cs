namespace JewelDesign.Documents;

/// <summary>Pure immutable DTO semantics. Native curve references require a separate owner/snapshot adapter.</summary>
public static class LegacyArrayData
{
    public const int MaximumDepth=256,MaximumItems=65536;
    public static LegacyChannelValue Flatten(LegacyChannelValue array)
    {
        ArgumentNullException.ThrowIfNull(array);if(array.Array is null)throw new ArgumentException("Expected array data.",nameof(array));
        var result=new List<LegacyChannelValue>();var stack=new Stack<(LegacyChannelValue Value,int Depth)>();
        for(int i=array.Array.Count-1;i>=0;i--)stack.Push((array.Array[i],1));int visited=0;
        while(stack.Count>0){
            var (value,depth)=stack.Pop();if(depth>MaximumDepth||++visited>MaximumItems)throw new NotSupportedException("Array traversal exceeds explicit candidate resource limits.");
            if(value.Array is{} items){
                if(items.Count>MaximumItems-visited-stack.Count)throw new NotSupportedException("Pending array traversal exceeds explicit candidate item limit.");
                for(int i=items.Count-1;i>=0;i--)stack.Push((items[i],depth+1));
            }
            else result.Add(value.IsMissing?LegacyChannelValue.Empty:value);
        }
        return LegacyChannelValue.FromArray(result);
    }
    public static LegacyChannelValue Duplicate(LegacyChannelValue array)
    {
        ArgumentNullException.ThrowIfNull(array);if(array.Array is null)throw new ArgumentException("Expected array data.",nameof(array));int visited=0;
        LegacyChannelValue Copy(LegacyChannelValue value,int depth){
            if(depth>MaximumDepth||(depth>0&&++visited>MaximumItems))throw new NotSupportedException("Array copy exceeds explicit candidate resource limits.");
            if(value.IsMissing)return LegacyChannelValue.Empty;
            return value.Array is{} items?LegacyChannelValue.FromArray(items.Select(x=>Copy(x,depth+1))):value;
        }
        return Copy(array,0);
    }
}
public readonly record struct LegacyArrayAddResult(LegacyChannelValue Value,LegacyChannelValue Negated,LegacyChannelValue OneMinus);
public static class LegacyArrayArithmetic
{
    public static LegacyArrayAddResult Add(LegacyChannelValue first,LegacyChannelValue second)
    {
        ArgumentNullException.ThrowIfNull(first);ArgumentNullException.ThrowIfNull(second);
        int a=first.Array?.Count??0,b=second.Array?.Count??0;
        double x=0,y=0;
        if(a==0&&b==0){first.GetDouble(ref x);second.GetDouble(ref y);double n=x+y;return new(LegacyChannelValue.FromNumber(n),LegacyChannelValue.FromNumber(0.0-n),LegacyChannelValue.FromNumber(1.0-n));}
        long count=a>0&&b>0?(long)a*b:Math.Max(a,b);
        if(count>LegacyArrayData.MaximumItems)throw new NotSupportedException("Array arithmetic exceeds explicit candidate item limit.");
        var values=new List<LegacyChannelValue>((int)count);var negative=new List<LegacyChannelValue>((int)count);var oneMinus=new List<LegacyChannelValue>((int)count);
        void ReadItem(LegacyChannelValue item,ref double value){if(item.IsMissing)throw new NullReferenceException("Legacy CSAdd null array item.");item.GetDouble(ref value);}
        void Append(){double n=x+y;values.Add(LegacyChannelValue.FromNumber(n));negative.Add(LegacyChannelValue.FromNumber(0.0-n));oneMinus.Add(LegacyChannelValue.FromNumber(1.0-n));}
        if(a>0&&b>0){
            foreach(var item in first.Array!){ReadItem(item,ref x);foreach(var other in second.Array!){ReadItem(other,ref y);Append();}}
        }else if(b>0){first.GetDouble(ref x);foreach(var item in second.Array!){ReadItem(item,ref y);Append();}}
        else{second.GetDouble(ref y);foreach(var item in first.Array!){ReadItem(item,ref x);Append();}}
        return new(LegacyChannelValue.FromArray(values),LegacyChannelValue.FromArray(negative),LegacyChannelValue.FromArray(oneMinus));
    }
}
