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
		Brep[] array2 = Brep.CreatePlanarBreps(array);
		if (array2 == null)
		{
			return UpdateError("CreatePlanarBreps returned null");
		}
		if (array2.Length == 0)
		{
			return UpdateError("CreatePlanarBreps returned empty");
		}
		double offsetDistance = num / 2.0;
		List<Brep> list = new List<Brep>();
		Brep[] array3 = array2;
		for (int i = 0; i < array3.Length; i++)
		{
			Brep brep = Brep.CreateFromOffsetFace(array3[i].Faces.First(), offsetDistance, CSApplicationHelper.Application.Tolerance, bothSides: true, createSolid: true);
			if (brep != null && brep.IsValid)
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
				BoundingBox boundingBox = item.GetBoundingBox(accurate: true);
				unset.Union(boundingBox);
			}
			Transform xform = Transform.Translation(Point3d.Origin - unset.Center);
			foreach (Brep item2 in list)
			{
				item2.Transform(xform);
			}
		}
		CSDataArray cSDataArray = new CSDataArray();
		foreach (Brep item3 in list)
		{
			cSDataArray.Append(new CSOnGeometryData(item3));
		}
		OutTextObjects.SetData(cSDataArray);
		return base.Update();
	}
}
