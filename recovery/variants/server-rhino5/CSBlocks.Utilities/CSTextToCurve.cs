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
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		string output = "Text";
		string output2 = "Arial";
		if (iFont != null)
		{
			iFont.GetString(ref output2);
		}
		iText.GetString(ref output);
		OnPlane val = new OnPlane(RhUtil.RhinoActiveCPlane());
		val.SetOrigin(OnUtil.On_origin);
		OnTextEntity2 annotation = new OnTextEntity2();
		MRhinoAnnotationText val2 = new MRhinoAnnotationText();
		val2.SetAnnotation((IOnTextEntity2)(object)annotation);
		val2.SetTextHeight(2.0);
		if (output != null)
		{
			val2.SetString(output);
		}
		else
		{
			val2.SetString("Empty");
		}
		((MRhinoAnnotationObject)val2).SetPlane((IOnPlane)(object)val);
		int fontIndex = RhinoDoc.ActiveDoc.Fonts.FindOrCreate(output2, bold: false, italic: false);
		val2.SetFontIndex(fontIndex);
		RhUtil.RhinoApp().ActiveDoc().AddObject((MRhinoObject)(object)val2);
		OnCurve[] array = default(OnCurve[]);
		val2.Explode(ref array);
		if (array != null)
		{
			CSDataArray cSDataArray = new CSDataArray();
			for (int i = 0; i < array.Length; i++)
			{
				cSDataArray.Append(new CSOnGeometryData(array[i].Convert()));
			}
			oGeometry.SetData(cSDataArray);
		}
		RhUtil.RhinoApp().ActiveDoc().DeleteObject((IRhinoObjRef)new MRhinoObjRef(((IOn3dmObjectAttributes)((MRhinoObject)val2).Attributes()).m_uuid));
		return base.Update();
	}
}
