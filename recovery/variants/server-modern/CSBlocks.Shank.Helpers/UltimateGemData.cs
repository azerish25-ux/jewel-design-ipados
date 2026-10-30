using CSDatas;
using CSDatas.Interfaces;
using Rhino.Geometry;

namespace CSBlocks.Shank.Helpers;

internal sealed class UltimateGemData
{
	public double BaseParam = double.NaN;

	public Transform GemTransform;

	public double GemZMax = double.NaN;

	public double GemZMin = double.NaN;

	public double NormParam;

	public double Param = double.NaN;

	public double ParamLeft;

	public double ParamRight;

	public Curve ProfileCurve;

	public Transform ProfileTransform;

	public double SizeX;

	public double SizeXRotated;

	public double SizeY;

	public double SizeZ;

	public double SizeYPlusWall;

	public double Spacing;

	public double ZPlacement;

	public double vY;

	public double MinCuletMovement;

	public Point3d GemPoint
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			Point3d origin = Point3d.Origin;
			((Point3d)(ref origin)).Transform(GemTransform);
			return origin;
		}
	}

	public CSDataArray AddToArray()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray cSDataArray = new CSDataArray();
		cSDataArray.Append(new CSXformData(GemTransform));
		cSDataArray.Append(new CSNumberData(SizeX));
		cSDataArray.Append(new CSNumberData(SizeY));
		cSDataArray.Append(new CSNumberData(SizeZ));
		cSDataArray.Append(new CSNumberData(NormParam));
		cSDataArray.Append(new CSNumberData(Param));
		cSDataArray.Append(new CSNumberData(BaseParam));
		cSDataArray.Append(new CSXformData(ProfileTransform));
		cSDataArray.Append(CSOnGeometryData.FromCurve(ProfileCurve));
		cSDataArray.Append(new CSNumberData(ParamLeft));
		cSDataArray.Append(new CSNumberData(ParamRight));
		cSDataArray.Append(new CSNumberData(ZPlacement));
		cSDataArray.Append(new CSNumberData(GemZMax));
		cSDataArray.Append(new CSNumberData(GemZMin));
		cSDataArray.Append(new CSNumberData(vY));
		cSDataArray.Append(new CSNumberData(MinCuletMovement));
		return cSDataArray;
	}

	public UltimateGemData Duplicate()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		UltimateGemData obj = new UltimateGemData
		{
			SizeX = SizeX,
			SizeY = SizeY,
			SizeZ = SizeZ,
			NormParam = NormParam,
			Param = Param,
			ProfileTransform = ProfileTransform
		};
		Curve profileCurve = ProfileCurve;
		obj.ProfileCurve = ((profileCurve != null) ? profileCurve.DuplicateCurve() : null);
		obj.ParamLeft = ParamLeft;
		obj.ParamRight = ParamRight;
		obj.ZPlacement = ZPlacement;
		obj.SizeXRotated = SizeXRotated;
		obj.GemZMax = GemZMax;
		obj.GemZMin = GemZMin;
		obj.vY = vY;
		obj.MinCuletMovement = MinCuletMovement;
		return obj;
	}

	public static UltimateGemData ReadFromArray(CSDataArray gemInfo)
	{
		UltimateGemData ultimateGemData = new UltimateGemData();
		((CSXformData)gemInfo.GetArrayItem(0)).GetTransform(out ultimateGemData.GemTransform);
		gemInfo.GetArrayItem(1).GetDouble(ref ultimateGemData.SizeX);
		gemInfo.GetArrayItem(2).GetDouble(ref ultimateGemData.SizeY);
		gemInfo.GetArrayItem(3).GetDouble(ref ultimateGemData.SizeZ);
		gemInfo.GetArrayItem(4).GetDouble(ref ultimateGemData.NormParam);
		gemInfo.GetArrayItem(5).GetDouble(ref ultimateGemData.Param);
		gemInfo.GetArrayItem(6).GetDouble(ref ultimateGemData.BaseParam);
		((CSXformData)gemInfo.GetArrayItem(7)).GetTransform(out ultimateGemData.ProfileTransform);
		ultimateGemData.ProfileCurve = (gemInfo.GetArrayItem(8) as ICSCurve)?.GetCurve();
		gemInfo.GetArrayItem(9).GetDouble(ref ultimateGemData.ParamLeft);
		gemInfo.GetArrayItem(10).GetDouble(ref ultimateGemData.ParamRight);
		gemInfo.GetArrayItem(11).GetDouble(ref ultimateGemData.ZPlacement);
		gemInfo.GetArrayItem(12).GetDouble(ref ultimateGemData.GemZMax);
		gemInfo.GetArrayItem(13).GetDouble(ref ultimateGemData.GemZMin);
		gemInfo.GetArrayItem(14).GetDouble(ref ultimateGemData.vY);
		gemInfo.GetArrayItem(15).GetDouble(ref ultimateGemData.MinCuletMovement);
		return ultimateGemData;
	}
}
