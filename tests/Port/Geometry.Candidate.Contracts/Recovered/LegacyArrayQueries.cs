#nullable disable
// Exact source array query bodies with observed finite bounds/transform capabilities, not original native geometry.
using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks.Recovered.ArrayQueries;
internal readonly record struct Transform(LegacyAffineTransform Matrix,bool IsValid){internal static Transform Identity=>new(LegacyAffineTransform.Identity,true);internal static Transform Unset=>new(default,false);}
internal struct BoundingBox
{
    internal CandidatePoint3 Min,Max;internal bool IsValid;
    internal static BoundingBox Unset=>default;
    internal BoundingBox(CandidateCurveBounds b){Min=b.Minimum;Max=b.Maximum;IsValid=true;}
    internal bool MakeValid()=>IsValid;
    internal void Union(BoundingBox b){if(!IsValid){this=b;return;}Min=new(Math.Min(Min.X,b.Min.X),Math.Min(Min.Y,b.Min.Y),Math.Min(Min.Z,b.Min.Z));Max=new(Math.Max(Max.X,b.Max.X),Math.Max(Max.Y,b.Max.Y),Math.Max(Max.Z,b.Max.Z));}
}
internal interface ICSArray{int GetArrayCount();CSData GetArrayItem(int index);List<CSData> GetFlattenedList();}
internal interface ICSTransform{bool GetTransform(out Transform value);}
internal class CSData{public virtual bool GetTightBoundingBox(ref BoundingBox bounds,bool doGrow,Transform transform)=>false;}
internal sealed class Leaf:CSData,ICSTransform
{
    internal LegacyChannelValue Value;private readonly CandidateCurveBounds? bounds;private readonly LegacyAffineTransform? matrix;
    internal Leaf(LegacyChannelValue value,CandidateCurveBounds? bounds,LegacyAffineTransform? matrix){Value=value;this.bounds=bounds;this.matrix=matrix;}
    public bool GetTransform(out Transform value){value=matrix is{} m?new(m,m.IsFinite):Transform.Unset;return value.IsValid;}
    public override bool GetTightBoundingBox(ref BoundingBox result,bool doGrow,Transform transform){if(bounds is not{} b)return false;result=new(b);return true;}
}
internal sealed class CSDataArray:CSData,ICSArray,ICSTransform
{
    internal readonly List<CSData> Array=new();public int GetArrayCount()=>Array.Count;public CSData GetArrayItem(int index)=>Array[index];
public List<CSData> GetFlattenedList()
	{
		List<CSData> flatData = new List<CSData>();
		Array.ForEach(delegate(CSData _)
		{
			if (!(_ is ICSArray iCSArray))
			{
				flatData.Add(_);
			}
			else
			{
				flatData.AddRange(iCSArray.GetFlattenedList());
			}
		});
		return flatData;
	}
public bool GetTransform(out Transform value)
	{
		if (GetArrayCount() > 0)
		{
			return CSUtilMath.GetTransformNoArray(Array[0], out value);
		}
		value = Transform.Unset;
		return false;
	}
public override bool GetTightBoundingBox(ref BoundingBox boundingBox, bool doGrow, Transform transform)
	{
		BoundingBox boundingBox2 = BoundingBox.Unset;
		foreach (CSData flattened in GetFlattenedList())
		{
			if (flattened == null)
			{
				continue;
			}
			BoundingBox boundingBox3 = BoundingBox.Unset;
			if (flattened.GetTightBoundingBox(ref boundingBox3, doGrow: false, transform))
			{
				if (boundingBox2.IsValid)
				{
					boundingBox2.Union(boundingBox3);
				}
				else
				{
					boundingBox2 = boundingBox3;
				}
			}
		}
		if (!boundingBox2.MakeValid())
		{
			return false;
		}
		if (boundingBox.IsValid && doGrow)
		{
			boundingBox.Union(boundingBox2);
		}
		else
		{
			boundingBox = boundingBox2;
		}
		return true;
	}
}
internal static class CSUtilMath{
public static bool GetTransformNoArray(CSData data, out Transform value)
	{
		value = Transform.Unset;
		if (data == null)
		{
			return false;
		}
		if (data is CSDataArray)
		{
			return false;
		}
		if (data is ICSTransform iCSTransform)
		{
			return iCSTransform.GetTransform(out value);
		}
		return false;
	}
}
internal sealed class CSOutputChannel{internal CSData Data;internal CSOutputChannel(CSData data){Data=data;}}
internal sealed class Block{private readonly CSData value;internal Block(CSData value){this.value=value;}internal CSOutputChannel GetOutputChannel(int index)=>index==0?new(value):null;}
internal sealed class CSInputChannel{internal Block OutputChannelBlock;internal int OutputChannelIndex;internal CSInputChannel(CSData value){OutputChannelBlock=new(value);OutputChannelIndex=0;}
private CSData GetFirstData
	{
		get
		{
			if (OutputChannelBlock == null)
			{
				return null;
			}
			CSOutputChannel outputChannel = OutputChannelBlock.GetOutputChannel(OutputChannelIndex);
			if (outputChannel == null)
			{
				return null;
			}
			CSData cSData = outputChannel.Data;
			while (cSData is ICSArray)
			{
				ICSArray iCSArray = cSData as ICSArray;
				cSData = ((iCSArray.GetArrayCount() == 0) ? null : iCSArray.GetArrayItem(0));
			}
			return cSData;
		}
	}
internal CSData Observe()=>GetFirstData;
}
internal static class Values{internal static CSData From(LegacyChannelValue value,Func<LegacyChannelValue,CandidateCurveBounds?> bounds,Func<LegacyChannelValue,LegacyAffineTransform?> matrix){if(value.IsMissing)return null;if(value.Array is{} items){var a=new CSDataArray();foreach(var child in items)a.Array.Add(From(child,bounds,matrix));return a;}return new Leaf(value,bounds(value),matrix(value));}
internal static LegacyChannelValue First(CSData value)=>new CSInputChannel(value).Observe() is Leaf leaf?leaf.Value:LegacyChannelValue.Missing;
}
