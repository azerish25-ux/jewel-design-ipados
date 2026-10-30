using System;
using System.IO;
using System.Xml.Serialization;
using RMA.OpenNURBS;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CS.Profile;

[Serializable]
[XmlRoot("NurbsCurve")]
public class CSNurbsCurveXML
{
	public double[] knots;

	public On4dPoint[] points;

	private double version;

	private string name;

	private int dimension;

	private int isRational;

	private int order;

	private int knotCount;

	private int cvCount;

	[XmlAttribute]
	public double Version
	{
		get
		{
			return version;
		}
		set
		{
			version = value;
		}
	}

	[XmlAttribute]
	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	[XmlAttribute]
	public int Dimension
	{
		get
		{
			return dimension;
		}
		set
		{
			dimension = value;
		}
	}

	[XmlAttribute]
	public int IsRational
	{
		get
		{
			return isRational;
		}
		set
		{
			isRational = value;
		}
	}

	[XmlAttribute]
	public int Order
	{
		get
		{
			return order;
		}
		set
		{
			order = value;
		}
	}

	[XmlAttribute]
	public int KnotCount
	{
		get
		{
			return knotCount;
		}
		set
		{
			knotCount = value;
		}
	}

	[XmlAttribute]
	public int CvCount
	{
		get
		{
			return cvCount;
		}
		set
		{
			cvCount = value;
		}
	}

	public NurbsCurve GetNurbsCurve()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		OnNurbsCurve nurbsCurve = new OnNurbsCurve();
		if (!GetVarFromCurve(ref nurbsCurve))
		{
			return null;
		}
		Curve val = Interop.FromOnCurve((object)nurbsCurve);
		if (val == null)
		{
			return null;
		}
		if (val.HasNurbsForm() > 0)
		{
			return val.ToNurbsCurve();
		}
		return null;
	}

	public bool GetVarFromCurve(ref OnNurbsCurve nurbsCurve)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		if (version != 1.0)
		{
			return false;
		}
		if (!nurbsCurve.Create(dimension, isRational == 1, order, cvCount))
		{
			return false;
		}
		bool flag = true;
		if (points.Length != nurbsCurve.CVCount())
		{
			return false;
		}
		if (knots.Length != nurbsCurve.KnotCount())
		{
			return false;
		}
		for (int i = 0; i < points.Length; i++)
		{
			On4dPoint val = new On4dPoint
			{
				x = points[i].x,
				y = points[i].y,
				z = points[i].z,
				w = points[i].w
			};
			flag &= nurbsCurve.SetCV(i, (IOn4dPoint)(object)val);
		}
		for (int j = 0; j < knots.Length; j++)
		{
			flag &= nurbsCurve.SetKnot(j, knots[j]);
		}
		return flag;
	}

	public static CSNurbsCurveXML ReadFromFile(string fileName)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(CSNurbsCurveXML));
		FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
		CSNurbsCurveXML cSNurbsCurveXML = null;
		cSNurbsCurveXML = (CSNurbsCurveXML)xmlSerializer.Deserialize(fileStream);
		fileStream.Close();
		if (cSNurbsCurveXML.Name == null)
		{
			cSNurbsCurveXML.Name = fileName;
		}
		return cSNurbsCurveXML;
	}

	public bool SaveToFile(string fileName)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(GetType());
		FileStream fileStream = new FileStream(fileName, FileMode.Create);
		xmlSerializer.Serialize(fileStream, this);
		fileStream.Close();
		return true;
	}

	public void SetVarFromCurve(OnNurbsCurve nurbsCurve)
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		version = 1.0;
		dimension = nurbsCurve.m_dim;
		isRational = nurbsCurve.m_is_rat;
		order = nurbsCurve.m_order;
		cvCount = nurbsCurve.m_cv_count;
		knotCount = nurbsCurve.KnotCount();
		knots = new double[knotCount];
		for (int i = 0; i < knotCount; i++)
		{
			knots[i] = nurbsCurve.Knot(i);
		}
		points = new On4dPoint[cvCount];
		for (int j = 0; j < cvCount; j++)
		{
			On4dPoint val = new On4dPoint();
			nurbsCurve.GetCV(j, ref val);
			points[j] = new On4dPoint
			{
				x = val.x,
				y = val.y,
				z = val.z,
				w = val.w
			};
		}
	}
}
