using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlocks.Gems.Obsolete;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves.Obsolete;

[Obsolete]
[CSBlock("ChannelBorderFromGems_V1", "{BDF9F00E-D0DE-45ec-9016-22ACACF7DFFA}")]
public sealed class CSChannelBorderFromGems1 : CSBlock
{
	public CSInputChannel iSortedGemArray { get; }

	public CSInputChannel iAmount { get; }

	public CSInputChannel iEndAmount { get; }

	public CSInputChannel iEndAmount2 { get; }

	public CSInputChannel iExtType { get; }

	public CSInputChannel iZOffset { get; }

	public CSOutputChannel oBrep { get; }

	public CSChannelBorderFromGems1(string identifier)
		: base(identifier)
	{
		iSortedGemArray = new CSInputChannel(GetPropertyName(() => iSortedGemArray), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iAmount = new CSInputChannel(GetPropertyName(() => iAmount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEndAmount = new CSInputChannel(GetPropertyName(() => iEndAmount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEndAmount2 = new CSInputChannel(GetPropertyName(() => iEndAmount2), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iExtType = new CSInputChannel(GetPropertyName(() => iExtType), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZOffset = new CSInputChannel(GetPropertyName(() => iZOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		if (!(iSortedGemArray.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Missing Gems");
		}
		if (!(cSDataArray.Duplicate() is CSDataArray cSDataArray2))
		{
			return UpdateMissing("Missing Gems Channel Gems Data");
		}
		CSSortGems1 cSSortGems = iSortedGemArray.OutputChannelBlock as CSSortGems1;
		if (cSSortGems?.iGemArray?.Data != null)
		{
			CSDataHelper.SetSettingType(cSSortGems.iGemArray.Data, GemSettingType.CH);
		}
		Curve curve = CSUtilGemRails.RailRc(cSDataArray2)?.DuplicateCurve();
		Curve curve2 = CSUtilGemRails.RightRc(cSDataArray2, curve)?.DuplicateCurve();
		Curve curve3 = CSUtilGemRails.LeftRc(cSDataArray2, curve)?.DuplicateCurve();
		if (curve == null || curve2 == null || curve3 == null)
		{
			return UpdateMissing("Missing Curves On Gems");
		}
		double value = 0.0;
		iAmount.GetDouble(ref value);
		double value2 = 0.0;
		iZOffset.GetDouble(ref value2);
		double value3 = value;
		iEndAmount.GetDouble(ref value3);
		double value4 = value3;
		iEndAmount2.GetDouble(ref value4);
		CurveExtensionStyle style = CurveExtensionStyle.Line;
		double value5 = 0.0;
		if (iExtType.GetDouble(ref value5))
		{
			if (value5 > 0.5 && value5 <= 1.5)
			{
				style = CurveExtensionStyle.Arc;
			}
			if (value5 > 1.5 && value5 < 2.5)
			{
				style = CurveExtensionStyle.Smooth;
			}
		}
		curve3.Extend(CurveEnd.None, value3 + CSUtilGemRails.SizeStart(cSDataArray2) + value, style);
		curve3.Extend(CurveEnd.Start, value4 + CSUtilGemRails.SizeEnd(cSDataArray2) + value, style);
		curve2.Extend(CurveEnd.None, value3 + CSUtilGemRails.SizeStart(cSDataArray2) + value, style);
		curve2.Extend(CurveEnd.Start, value4 + CSUtilGemRails.SizeEnd(cSDataArray2) + value, style);
		LineCurve item = new LineCurve(curve3.PointAtStart, curve2.PointAtStart);
		LineCurve item2 = new LineCurve(curve3.PointAtEnd, curve2.PointAtEnd);
		List<Curve> list = new List<Curve>();
		list.Add(item);
		list.Add(curve3);
		list.Add(item2);
		list.Add(curve2);
		if (!CSUtilGemRails.IsClosed(cSDataArray2.Array))
		{
			Curve curve4 = Curve.JoinCurves(list.ToArray())[0];
			if (curve4 != null)
			{
				oBrep.SetData(new CSOnGeometryData(curve4));
				return base.Update();
			}
			return UpdateError("Unable To Calculate Border");
		}
		CSDataArray cSDataArray3 = new CSDataArray();
		cSDataArray3.Append(new CSOnGeometryData(curve3));
		cSDataArray3.Append(new CSOnGeometryData(curve2));
		oBrep.SetData(cSDataArray3);
		return base.Update();
	}
}
