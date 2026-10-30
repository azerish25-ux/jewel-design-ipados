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
		OnNurbsCurve nurbsCurve = new OnNurbsCurve();
		if (!GetVarFromCurve(ref nurbsCurve))
		{
			return null;
		}
		Curve curve = Interop.FromOnCurve(nurbsCurve);
		if (curve == null)
		{
			return null;
		}
		if (curve.HasNurbsForm() > 0)
		{
			return curve.ToNurbsCurve();
		}
		return null;
	}

	public bool GetVarFromCurve(ref OnNurbsCurve nurbsCurve)
	{
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
			RMA.OpenNURBS.On4dPoint vertex_value = new RMA.OpenNURBS.On4dPoint
			{
				x = points[i].x,
				y = points[i].y,
				z = points[i].z,
				w = points[i].w
			};
			flag &= nurbsCurve.SetCV(i, vertex_value);
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
			RMA.OpenNURBS.On4dPoint vertex_value = new RMA.OpenNURBS.On4dPoint();
			nurbsCurve.GetCV(j, ref vertex_value);
			points[j] = new On4dPoint
			{
				x = vertex_value.x,
				y = vertex_value.y,
				z = vertex_value.z,
				w = vertex_value.w
			};
		}
	}
}
