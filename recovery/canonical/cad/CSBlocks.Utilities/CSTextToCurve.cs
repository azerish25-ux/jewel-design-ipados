using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino;

namespace CSBlocks.Utilities;

[CSBlock("TextToCurve", "{18E4F41B-5369-47b4-A797-ACA8ED3D97D2}")]
public sealed class CSTextToCurve : CSBlock
{
	public CSInputChannel iText { get; }

	public CSInputChannel iFont { get; }

	public CSOutputChannel oGeometry { get; }

	public CSTextToCurve(string identifier)
		: base(identifier)
	{
		iText = new CSInputChannel(GetPropertyName(() => iText), this, ChannelDataType.String, ChannelArrayType.Individual);
		iFont = new CSInputChannel(GetPropertyName(() => iFont), this, ChannelDataType.String, ChannelArrayType.Individual);
		oGeometry = new CSOutputChannel(GetPropertyName(() => oGeometry), this, ChannelDataType.Curve, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		string output = "Text";
		string output2 = "Arial";
		if (iFont != null)
		{
			iFont.GetString(ref output2);
		}
		iText.GetString(ref output);
		OnPlane onPlane = new OnPlane(RhUtil.RhinoActiveCPlane());
		onPlane.SetOrigin(OnUtil.On_origin);
		OnTextEntity2 annotation = new OnTextEntity2();
		MRhinoAnnotationText mRhinoAnnotationText = new MRhinoAnnotationText();
		mRhinoAnnotationText.SetAnnotation(annotation);
		mRhinoAnnotationText.SetTextHeight(2.0);
		if (output != null)
		{
			mRhinoAnnotationText.SetString(output);
		}
		else
		{
			mRhinoAnnotationText.SetString("Empty");
		}
		mRhinoAnnotationText.SetPlane(onPlane);
		int fontIndex = RhinoDoc.ActiveDoc.Fonts.FindOrCreate(output2, bold: false, italic: false);
		mRhinoAnnotationText.SetFontIndex(fontIndex);
		RhUtil.RhinoApp().ActiveDoc().AddObject(mRhinoAnnotationText);
		mRhinoAnnotationText.Explode(out var output_curves);
		if (output_curves != null)
		{
			CSDataArray cSDataArray = new CSDataArray();
			for (int i = 0; i < output_curves.Length; i++)
			{
				cSDataArray.Append(new CSOnGeometryData(output_curves[i].Convert()));
			}
			oGeometry.SetData(cSDataArray);
		}
		RhUtil.RhinoApp().ActiveDoc().DeleteObject(new MRhinoObjRef(mRhinoAnnotationText.Attributes().m_uuid));
		return base.Update();
	}
}
