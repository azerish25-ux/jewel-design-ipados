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
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Expected O, but got Unknown
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Expected O, but got Unknown
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
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
		Curve obj = CSUtilGemRails.RailRc(cSDataArray2);
		Curve val = ((obj != null) ? obj.DuplicateCurve() : null);
		Curve obj2 = CSUtilGemRails.RightRc(cSDataArray2, val);
		Curve val2 = ((obj2 != null) ? obj2.DuplicateCurve() : null);
		Curve obj3 = CSUtilGemRails.LeftRc(cSDataArray2, val);
		Curve val3 = ((obj3 != null) ? obj3.DuplicateCurve() : null);
		if (val == null || val2 == null || val3 == null)
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
		CurveExtensionStyle val4 = (CurveExtensionStyle)0;
		double value5 = 0.0;
		if (iExtType.GetDouble(ref value5))
		{
			if (value5 > 0.5 && value5 <= 1.5)
			{
				val4 = (CurveExtensionStyle)1;
			}
			if (value5 > 1.5 && value5 < 2.5)
			{
				val4 = (CurveExtensionStyle)2;
			}
		}
		val3.Extend((CurveEnd)0, value3 + CSUtilGemRails.SizeStart(cSDataArray2) + value, val4);
		val3.Extend((CurveEnd)1, value4 + CSUtilGemRails.SizeEnd(cSDataArray2) + value, val4);
		val2.Extend((CurveEnd)0, value3 + CSUtilGemRails.SizeStart(cSDataArray2) + value, val4);
		val2.Extend((CurveEnd)1, value4 + CSUtilGemRails.SizeEnd(cSDataArray2) + value, val4);
		LineCurve item = new LineCurve(val3.PointAtStart, val2.PointAtStart);
		LineCurve item2 = new LineCurve(val3.PointAtEnd, val2.PointAtEnd);
		List<Curve> list = new List<Curve>();
		list.Add((Curve)(object)item);
		list.Add(val3);
		list.Add((Curve)(object)item2);
		list.Add(val2);
		if (!CSUtilGemRails.IsClosed(cSDataArray2.Array))
		{
			Curve val5 = Curve.JoinCurves((IEnumerable<Curve>)list.ToArray())[0];
			if (val5 != null)
			{
				oBrep.SetData(new CSOnGeometryData((GeometryBase)(object)val5));
				return base.Update();
			}
			return UpdateError("Unable To Calculate Border");
		}
		CSDataArray cSDataArray3 = new CSDataArray();
		cSDataArray3.Append(new CSOnGeometryData((GeometryBase)(object)val3));
		cSDataArray3.Append(new CSOnGeometryData((GeometryBase)(object)val2));
		oBrep.SetData(cSDataArray3);
		return base.Update();
	}
}
