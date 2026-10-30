using System.Collections.Generic;
using System.Linq;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Shared.Utilities.Helpers;
using Rhino;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSBlocks.Tools;

[CSBlock("TextObjects", "{B39DC635-910F-4802-98D3-2D6E9465EA62}")]
public sealed class CSTextObjects : CSBlock
{
	public CSInputChannel InValue { get; }

	public CSInputChannel InFont { get; }

	public CSInputChannel InIsFontFaceBold { get; }

	public CSInputChannel InIsFontFaceItalics { get; }

	public CSInputChannel InFontSize { get; }

	public CSInputChannel InFontDepth { get; }

	public CSInputChannel InDoCenterText { get; }

	public CSOutputChannel OutTextObjects { get; }

	public CSTextObjects(string identifier)
		: base(identifier)
	{
		InValue = new CSInputChannel(this, ChannelDataType.Boolean | ChannelDataType.Double | ChannelDataType.String, ChannelArrayType.Individual);
		InFont = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, Font.BookmanOldStyle);
		InFontSize = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 12.0);
		InFontDepth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		InIsFontFaceBold = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InIsFontFaceItalics = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InDoCenterText = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		OutTextObjects = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		string text = InValue.String;
		Font font = InFont.Font;
		double textHeight = InFontSize.Double;
		double num = InFontDepth.Double;
		bool boolean = InIsFontFaceBold.Boolean;
		bool boolean2 = InIsFontFaceItalics.Boolean;
		bool boolean3 = InDoCenterText.Boolean;
		if (font == Font.Unknown)
		{
			return UpdateError("Invalid font value");
		}
		string enumDescriptionFromValue = EnumHelper.GetEnumDescriptionFromValue(font);
		if (string.IsNullOrEmpty(enumDescriptionFromValue))
		{
			return UpdateError("Invalid font name");
		}
		int num2 = RhinoDoc.ActiveDoc.Fonts.FindOrCreate(enumDescriptionFromValue, boolean, boolean2);
		if (num2 < 0 || num2 >= RhinoDoc.ActiveDoc.Fonts.Count)
		{
			return UpdateError("Invalid font face index (Rhino)");
		}
		Curve[] array = new TextEntity
		{
			FontIndex = num2,
			Text = text,
			TextHeight = textHeight
		}.Explode();
		if (array == null)
		{
			return UpdateError("Explode returned null");
		}
		if (array.Length == 0)
		{
			return UpdateError("Explode returned empty");
		}
		Brep[] array2 = Brep.CreatePlanarBreps((IEnumerable<Curve>)array);
		if (array2 == null)
		{
			return UpdateError("CreatePlanarBreps returned null");
		}
		if (array2.Length == 0)
		{
			return UpdateError("CreatePlanarBreps returned empty");
		}
		double num3 = num / 2.0;
		List<Brep> list = new List<Brep>();
		Brep[] array3 = array2;
		for (int i = 0; i < array3.Length; i++)
		{
			Brep brep = Brep.CreateFromOffsetFace(((IEnumerable<BrepFace>)array3[i].Faces).First(), num3, CSApplicationHelper.Application.Tolerance, true, true);
			if (brep != null && ((CommonObject)brep).IsValid)
			{
				CSUtilGeometry.FixBrepSolidOrientation(ref brep);
				list.Add(brep);
			}
		}
		if (boolean3)
		{
			BoundingBox unset = BoundingBox.Unset;
			foreach (Brep item in list)
			{
				BoundingBox boundingBox = ((GeometryBase)item).GetBoundingBox(true);
				((BoundingBox)(ref unset)).Union(boundingBox);
			}
			Transform val = Transform.Translation(Point3d.Origin - ((BoundingBox)(ref unset)).Center);
			foreach (Brep item2 in list)
			{
				((GeometryBase)item2).Transform(val);
			}
		}
		CSDataArray cSDataArray = new CSDataArray();
		foreach (Brep item3 in list)
		{
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)item3));
		}
		OutTextObjects.SetData(cSDataArray);
		return base.Update();
	}
}
