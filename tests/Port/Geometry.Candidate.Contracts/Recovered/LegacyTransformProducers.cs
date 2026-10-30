#nullable disable
// Exact recovered Update bodies; channel/bounds/matrix storage stubs are observation aids only.
using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks.Recovered.Transforms;
internal sealed class Plane{internal static readonly Plane WorldXY=new();}
internal readonly record struct Transform(LegacyAffineTransform Matrix)
{
    internal static Transform Identity=>new(LegacyAffineTransform.Scale(1,1,1));
    internal static Transform Translation(double x,double y,double z)=>new(LegacyAffineTransform.Translation(x,y,z));
    internal static Transform Scale(Plane plane,double x,double y,double z){if(plane!=Plane.WorldXY)throw new Exception("Unexpected source plane");return new(LegacyAffineTransform.Scale(x,y,z));}
}
internal readonly record struct BoundingBox(CandidatePoint3 Min,CandidatePoint3 Max){internal static BoundingBox Unset=>default;}
internal class CSData
{
    internal LegacyChannelValue Value;private readonly CandidateCurveBounds? bounds;
    internal CSData(LegacyChannelValue value,CandidateCurveBounds? bounds=null){Value=value;this.bounds=bounds;}
    internal bool GetDouble(ref double value)=>Value.GetDouble(ref value);
    internal bool GetTightBoundingBox(ref BoundingBox result,bool doGrow,Transform transform){if(bounds is not{} b)return false;result=new(b.Minimum,b.Maximum);return true;}
}
internal sealed class CSXformData:CSData{internal CSXformData(Transform matrix):base(LegacyChannelValue.FromTransform(matrix.Matrix)){}}
internal sealed class CSNumberData:CSData{internal CSNumberData(double number):base(LegacyChannelValue.FromNumber(number)){}}
internal sealed class Input
{
    internal CSData Data;
    internal Input(LegacyChannelValue value){Data=value.IsMissing?null:new(value);}
    internal Input(CandidateCurveBounds bounds){Data=new(LegacyChannelValue.Empty,bounds);}
    internal bool GetDouble(ref double value)=>Data?.GetDouble(ref value)??false;
}
internal sealed class Output{internal CSData Data;internal void SetData(CSData data){Data=data;}}
internal class CSBlock{public virtual bool Update()=>true;protected bool UpdateMissing(string message)=>false;}
// Array source branches are retained but deliberately unavailable in this scalar observation harness.
internal sealed class CSDataArray:CSData{internal CSDataArray():base(LegacyChannelValue.Empty){}internal void Append(CSData value)=>throw new NotSupportedException("Array oracle unavailable");}
internal static class CSDataHelper{internal static int GetArrayCount(CSData value)=>0;internal static CSData GetArrayItem(CSData value,int index)=>throw new NotSupportedException("Array oracle unavailable");}
internal sealed class TranslateSource:CSBlock
{
    private readonly Input iX,iY,iZ,iMirror;internal readonly Output oXform=new();
    internal TranslateSource(LegacyChannelValue x,LegacyChannelValue y,LegacyChannelValue z,LegacyChannelValue mirror){iX=new(x);iY=new(y);iZ=new(z);iMirror=new(mirror);}
public override bool Update()
	{
		double value = 0.0;
		if (iX != null)
		{
			iX.GetDouble(ref value);
		}
		double value2 = 0.0;
		if (iY != null)
		{
			iY.GetDouble(ref value2);
		}
		double value3 = 0.0;
		if (iZ != null)
		{
			iZ.GetDouble(ref value3);
		}
		double value4 = 0.0;
		if (iMirror != null)
		{
			iMirror.GetDouble(ref value4);
		}
		if (value4 > 0.5)
		{
			value = 0.0 - value;
			value3 = 0.0 - value3;
		}
		Transform transform = Transform.Translation(value, value2, value3);
		oXform.SetData(new CSXformData(transform));
		return base.Update();
	}
}
internal sealed class ScaleSource:CSBlock
{
    private readonly Input iObj,iX,iY,iZ;internal readonly Output oXform=new();
    internal ScaleSource(CandidateCurveBounds bounds,LegacyChannelValue x,LegacyChannelValue y,LegacyChannelValue z){iObj=new(bounds);iX=new(x);iY=new(y);iZ=new(z);}
public override bool Update()
	{
		CSData data = iObj.Data;
		if (data == null)
		{
			return UpdateMissing("Object");
		}
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!data.GetTightBoundingBox(ref boundingBox, doGrow: true, Transform.Identity))
		{
			return UpdateMissing("Bounding Box");
		}
		double num = boundingBox.Max.X - boundingBox.Min.X;
		double num2 = boundingBox.Max.Y - boundingBox.Min.Y;
		double num3 = boundingBox.Max.Z - boundingBox.Min.Z;
		if (num < 0.01)
		{
			num = 1.0;
		}
		if (num2 < 0.01)
		{
			num2 = 1.0;
		}
		if (num3 < 0.01)
		{
			num3 = 1.0;
		}
		double value = num;
		iX.GetDouble(ref value);
		double value2 = num2;
		iY.GetDouble(ref value2);
		double value3 = num3;
		iZ.GetDouble(ref value3);
		Transform transform = Transform.Scale(Plane.WorldXY, value / num, value2 / num2, value3 / num3);
		oXform.SetData(new CSXformData(transform));
		return base.Update();
	}
}
internal sealed class AddSource:CSBlock
{
    private readonly Input iStart,iValue;internal readonly Output oValue=new(),oNegated=new(),oOneMinus=new();
    internal AddSource(LegacyChannelValue x,LegacyChannelValue y){iStart=new(x);iValue=new(y);}
public override bool Update()
	{
		CSData data = iStart.Data;
		CSData data2 = iValue.Data;
		double value = 0.0;
		double value2 = 0.0;
		if (CSDataHelper.GetArrayCount(data) > 0 || CSDataHelper.GetArrayCount(data2) > 0)
		{
			CSDataArray cSDataArray = new CSDataArray();
			CSDataArray cSDataArray2 = new CSDataArray();
			CSDataArray cSDataArray3 = new CSDataArray();
			if (CSDataHelper.GetArrayCount(data) > 0 && CSDataHelper.GetArrayCount(data2) > 0)
			{
				for (int i = 0; i < CSDataHelper.GetArrayCount(data); i++)
				{
					CSDataHelper.GetArrayItem(data, i).GetDouble(ref value);
					for (int j = 0; j < CSDataHelper.GetArrayCount(data2); j++)
					{
						CSDataHelper.GetArrayItem(data2, j).GetDouble(ref value2);
						double num = value + value2;
						cSDataArray.Append(new CSNumberData(num));
						cSDataArray2.Append(new CSNumberData(0.0 - num));
						cSDataArray3.Append(new CSNumberData(1.0 - num));
					}
				}
			}
			else if (CSDataHelper.GetArrayCount(data) <= 0 && CSDataHelper.GetArrayCount(data2) > 0)
			{
				data?.GetDouble(ref value);
				for (int k = 0; k < CSDataHelper.GetArrayCount(data2); k++)
				{
					CSDataHelper.GetArrayItem(data2, k).GetDouble(ref value2);
					double num = value + value2;
					cSDataArray.Append(new CSNumberData(num));
					cSDataArray2.Append(new CSNumberData(0.0 - num));
					cSDataArray3.Append(new CSNumberData(1.0 - num));
				}
			}
			else if (CSDataHelper.GetArrayCount(data) > 0 && CSDataHelper.GetArrayCount(data2) <= 0)
			{
				data2?.GetDouble(ref value2);
				for (int l = 0; l < CSDataHelper.GetArrayCount(data); l++)
				{
					CSDataHelper.GetArrayItem(data, l).GetDouble(ref value);
					double num = value + value2;
					cSDataArray.Append(new CSNumberData(num));
					cSDataArray2.Append(new CSNumberData(0.0 - num));
					cSDataArray3.Append(new CSNumberData(1.0 - num));
				}
			}
			oValue.SetData(cSDataArray);
			oNegated.SetData(cSDataArray2);
			oOneMinus.SetData(cSDataArray3);
		}
		else
		{
			data?.GetDouble(ref value);
			data2?.GetDouble(ref value2);
			double num = value + value2;
			oValue.SetData(new CSNumberData(num));
			oNegated.SetData(new CSNumberData(0.0 - num));
			oOneMinus.SetData(new CSNumberData(1.0 - num));
		}
		return base.Update();
	}
}
