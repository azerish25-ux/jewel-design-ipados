using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.SMI;

[CSBlock("SMIPosaluxCuttingDepth", "{41008634-A8C9-487d-B17A-F12E6B8AA8FF}")]
public sealed class CSSMIPosaluxCuttingDepth : CSBlock
{
	public CSInputChannel iBand { get; }

	public CSInputChannel iCutter { get; }

	public CSInputChannel iRadius { get; }

	public CSInputChannel iCuttingDepth { get; }

	public CSOutputChannel oObject { get; }

	public CSSMIPosaluxCuttingDepth(string identifier)
		: base(identifier)
	{
		iBand = new CSInputChannel(GetPropertyName(() => iBand), this, ChannelDataType.Any, ChannelArrayType.Any);
		iCutter = new CSInputChannel(GetPropertyName(() => iCutter), this, ChannelDataType.Any, ChannelArrayType.Any);
		iRadius = new CSInputChannel(GetPropertyName(() => iRadius), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iCuttingDepth = new CSInputChannel(GetPropertyName(() => iCuttingDepth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObject = new CSOutputChannel(GetPropertyName(() => oObject), this, ChannelDataType.Any, ChannelArrayType.Any);
	}

	public override bool Update()
	{
		CSData data = iBand.Data;
		if (data == null)
		{
			return UpdateMissing("Missing Band Data");
		}
		OnBrep onBrep = data.GetBrep().Convert();
		if (onBrep == null)
		{
			return UpdateMissing("Missing Band");
		}
		CSData data2 = iCutter.Data;
		if (data2 == null)
		{
			return UpdateMissing("Missing Cutter Data");
		}
		double value = 76.2;
		iRadius.GetDouble(ref value);
		double value2 = 0.0;
		iCuttingDepth.GetDouble(ref value2);
		CSDataHelper.GetTransformNoArray(data2, out var value3);
		if (!value3.IsValid)
		{
			return UpdateError("Invalid Xform");
		}
		value3.TryGetInverse(out var inverseTransform);
		if (!inverseTransform.IsValid)
		{
			return UpdateError("Invalid Xform");
		}
		On3dPoint on3dPoint = new On3dPoint(0.0, value, 0.0);
		on3dPoint.Transform(value3.Convert());
		On3dPoint a = new On3dPoint(0.0, -100.0, 100.0);
		On3dPoint b = new On3dPoint(0.0, 100.0, 100.0);
		On3dPoint c = new On3dPoint(0.0, 100.0, -100.0);
		On3dPoint d = new On3dPoint(0.0, -100.0, -100.0);
		OnBrep onBrep2 = RhUtil.RhinoCreateSurfaceFromCorners(a, b, ref c, ref d).BrepForm();
		onBrep2.Transform(value3.Convert());
		if (!RhUtil.RhinoIntersectBreps(onBrep, onBrep2, CSApplicationHelper.Application.Tolerance, out var curves, out var points))
		{
			return UpdateError("RhinoIntersectBreps failed");
		}
		if (curves.Length == 0 && points.Count() <= 0)
		{
			return UpdateError("RhinoIntersectBreps failed");
		}
		double num = 0.0;
		bool flag = false;
		OnCurve[] array = curves;
		foreach (OnCurve onCurve in array)
		{
			double t = 0.0;
			if (onCurve.GetClosestPoint(on3dPoint, ref t))
			{
				double num2 = on3dPoint.DistanceTo(onCurve.PointAt(t));
				if (num2 <= num || !flag)
				{
					num = num2;
					flag = true;
				}
			}
		}
		foreach (On3dPoint item in points)
		{
			double num3 = on3dPoint.DistanceTo(item);
			if (num3 <= num || !flag)
			{
				num = num3;
				flag = true;
			}
		}
		num -= value;
		OnXform onXform = new OnXform();
		onXform.Translation(0.0, 0.0 - num + value2, 0.0);
		CSData data3 = data2.Duplicate();
		CSDataHelper.Transform(data3, value3 * onXform.Convert() * inverseTransform);
		oObject.SetData(data3);
		return base.Update();
	}
}
