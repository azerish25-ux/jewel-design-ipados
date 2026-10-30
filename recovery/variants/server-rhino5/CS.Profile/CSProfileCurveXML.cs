using System;
using System.IO;
using System.Xml.Serialization;
using RMA.OpenNURBS;

namespace CS.Profile;

[Serializable]
[XmlRoot("ProfileCurve")]
public sealed class CSProfileCurveXML : CSNurbsCurveXML
{
	private double[] sweepParam;

	private double center;

	public double[] SweepParam
	{
		get
		{
			return sweepParam;
		}
		set
		{
			sweepParam = value;
		}
	}

	public double Center
	{
		get
		{
			return center;
		}
		set
		{
			center = value;
		}
	}

	public CSProfileCurveXML()
	{
		sweepParam = new double[4];
		center = 0.5;
		sweepParam[0] = 1.0 / 6.0;
		sweepParam[1] = 1.0 / 3.0;
		sweepParam[2] = 2.0 / 3.0;
		sweepParam[3] = 5.0 / 6.0;
	}

	public CSProfileCurveXML(OnNurbsCurve crv)
		: this()
	{
		SetVarFromCurve(crv);
	}

	public CSProfileCurveXML(OnNurbsCurve crv, double[] param)
	{
		SetVarFromCurve(crv);
		sweepParam = (double[])param.Clone();
	}

	public new static CSProfileCurveXML ReadFromFile(string fileName)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(CSProfileCurveXML));
		FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
		object obj = xmlSerializer.Deserialize(fileStream);
		fileStream.Close();
		if (obj != null && obj is CSProfileCurveXML cSProfileCurveXML)
		{
			if (cSProfileCurveXML.Name == null)
			{
				cSProfileCurveXML.Name = fileName;
			}
			return cSProfileCurveXML;
		}
		return null;
	}
}
