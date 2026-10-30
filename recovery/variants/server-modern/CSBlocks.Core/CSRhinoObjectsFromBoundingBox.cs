using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("RhinoObjectsFromBoundingBox", "{16358839-FDC6-44c0-A62E-B2E872A76697}")]
public sealed class CSRhinoObjectsFromBoundingBox : CSBlock, ICSBlockObjectWatcher
{
	private readonly List<GeometryBase> _objects = new List<GeometryBase>();

	private bool _useObjects;

	public CSInputChannel iBoundingBox { get; }

	public CSInputChannel iGrowBox { get; }

	public CSOutputChannel oObjects { get; }

	public CSRhinoObjectsFromBoundingBox(string identifier)
		: base(identifier)
	{
		iBoundingBox = new CSInputChannel(GetPropertyName(() => iBoundingBox), this, ChannelDataType.Any, ChannelArrayType.Individual);
		iGrowBox = new CSInputChannel(GetPropertyName(() => iGrowBox), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObjects = new CSOutputChannel(GetPropertyName(() => oObjects), this, ChannelDataType.Any, ChannelArrayType.Array);
	}

	public bool CheckForChangeOnDelete(MRhinoObject obj)
	{
		return CheckForChangeOnAdd(obj);
	}

	public bool CheckForChangeOnAdd(MRhinoObject obj)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		if (_useObjects)
		{
			return false;
		}
		if (iBoundingBox == null)
		{
			return false;
		}
		if (iBoundingBox.Data == null)
		{
			return false;
		}
		BoundingBox boundingBox = BoundingBox.Unset;
		iBoundingBox.Data.GetBoundingBox(ref boundingBox);
		if (iGrowBox != null)
		{
			double value = 0.0;
			iGrowBox.GetDouble(ref value);
			((BoundingBox)(ref boundingBox)).Max = ((BoundingBox)(ref boundingBox)).Max + new Vector3d(value, value, value);
			((BoundingBox)(ref boundingBox)).Min = ((BoundingBox)(ref boundingBox)).Min - new Vector3d(value, value, value);
		}
		Point3d val = ((BoundingBox)(ref boundingBox)).Min;
		double x = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double y = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		Point3d pt = new Point3d(x, y, ((Point3d)(ref val)).Z - 1000.0);
		val = ((BoundingBox)(ref boundingBox)).Max;
		double x2 = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double y2 = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Max;
		Point3d pt2 = default(Point3d);
		((Point3d)(ref pt2))._002Ector(x2, y2, ((Point3d)(ref val)).Z + 1000.0);
		OnBoundingBox val2 = new OnBoundingBox((IOn3dPoint)(object)CSConvert.Convert(pt), (IOn3dPoint)(object)pt2.Convert());
		OnBoundingBox val3 = new OnBoundingBox();
		obj.GetTightBoundingBox(ref val3);
		if (val3.Min().DistanceTo((IOn3dPoint)(object)((BoundingBox)(ref boundingBox)).Min.Convert()) < 0.1 && val3.Max().DistanceTo((IOn3dPoint)(object)((BoundingBox)(ref boundingBox)).Max.Convert()) < 0.1)
		{
			Change();
			return true;
		}
		if (!val2.Includes((IOnBoundingBox)(object)val3))
		{
			return false;
		}
		Change();
		return true;
	}

	public bool CheckForChangeOnReplace(MRhinoObject oldObject, MRhinoObject obj)
	{
		return CheckForChangeOnAdd(oldObject);
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		base.Read(ref archive, graph, major, minor);
		uint num = 0u;
		int num2 = 0;
		if (!archive.BeginRead3dmChunk(ref num, ref num2))
		{
			return false;
		}
		_objects.Clear();
		_useObjects = true;
		On3dmObjectAttributes val = new On3dmObjectAttributes();
		if (num == 268435475)
		{
			OnObject val2 = default(OnObject);
			while (archive.Read3dmObject(ref val2, ref val, 0u) != 0 && val2 != null)
			{
				_objects.Add(val2.Convert());
			}
		}
		archive.EndRead3dmChunk();
		return true;
	}

	public override bool Update()
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray cSDataArray = new CSDataArray();
		if (_useObjects)
		{
			foreach (GeometryBase @object in _objects)
			{
				cSDataArray.Append(new CSOnGeometryData(@object));
			}
		}
		else
		{
			if (iBoundingBox == null)
			{
				return UpdateMissing("BoundingBox");
			}
			if (iBoundingBox.Data == null)
			{
				return UpdateError("BoundingBox Missing");
			}
			BoundingBox boundingBox = BoundingBox.Unset;
			iBoundingBox.Data.GetBoundingBox(ref boundingBox);
			if (iGrowBox != null)
			{
				double value = 0.0;
				iGrowBox.GetDouble(ref value);
				((BoundingBox)(ref boundingBox)).Max = ((BoundingBox)(ref boundingBox)).Max + new Vector3d(value, value, value);
				((BoundingBox)(ref boundingBox)).Min = ((BoundingBox)(ref boundingBox)).Min - new Vector3d(value, value, value);
			}
			Point3d val = ((BoundingBox)(ref boundingBox)).Min;
			double x = ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref boundingBox)).Min;
			double y = ((Point3d)(ref val)).Y;
			val = ((BoundingBox)(ref boundingBox)).Min;
			Point3d val2 = default(Point3d);
			((Point3d)(ref val2))._002Ector(x, y, ((Point3d)(ref val)).Z - 1000.0);
			val = ((BoundingBox)(ref boundingBox)).Max;
			double x2 = ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref boundingBox)).Max;
			double y2 = ((Point3d)(ref val)).Y;
			val = ((BoundingBox)(ref boundingBox)).Max;
			Point3d val3 = default(Point3d);
			((Point3d)(ref val3))._002Ector(x2, y2, ((Point3d)(ref val)).Z + 1000.0);
			BoundingBox val4 = default(BoundingBox);
			((BoundingBox)(ref val4))._002Ector(val2, val3);
			foreach (RhinoObject selectedObject in RhinoDoc.ActiveDoc.Objects.GetSelectedObjects(false, false))
			{
				if (selectedObject.Geometry == null)
				{
					continue;
				}
				BoundingBox boundingBox2 = selectedObject.Geometry.GetBoundingBox(selectedObject.Geometry is Curve);
				val = ((BoundingBox)(ref boundingBox2)).Min;
				if (((Point3d)(ref val)).CompareTo(((BoundingBox)(ref boundingBox)).Min) == 0)
				{
					val = ((BoundingBox)(ref boundingBox2)).Max;
					if (((Point3d)(ref val)).CompareTo(((BoundingBox)(ref boundingBox)).Max) == 0)
					{
						continue;
					}
				}
				if (((BoundingBox)(ref val4)).Contains(boundingBox2))
				{
					GeometryBase val5 = selectedObject.Geometry.Duplicate();
					if (selectedObject.Attributes.Name.Length > 0)
					{
						val5.SetUserString("CSName", selectedObject.Attributes.Name);
					}
					if (selectedObject.Attributes.LayerIndex >= 0 && selectedObject.Attributes.LayerIndex < RhinoDoc.ActiveDoc.Layers.Count)
					{
						string name = RhinoDoc.ActiveDoc.Layers[selectedObject.Attributes.LayerIndex].Name;
						val5.SetUserString("CSLayer", name);
					}
					if (val5 != null)
					{
						cSDataArray.Append(new CSOnGeometryData(val5));
					}
				}
			}
		}
		oObjects.SetData(cSDataArray);
		return base.Update();
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Expected O, but got Unknown
		base.Write(ref archive);
		archive.BeginWrite3dmChunk(268435475u, 0);
		if (_useObjects)
		{
			foreach (GeometryBase @object in _objects)
			{
				On3dmObjectAttributes val = new On3dmObjectAttributes();
				archive.Write3dmObject((IOnObject)(object)@object.Convert(), (IOn3dmObjectAttributes)(object)val);
			}
		}
		else
		{
			if (iBoundingBox == null)
			{
				return false;
			}
			if (iBoundingBox.Data == null)
			{
				return false;
			}
			BoundingBox boundingBox = BoundingBox.Unset;
			iBoundingBox.Data.GetBoundingBox(ref boundingBox);
			if (iGrowBox != null)
			{
				double value = 0.0;
				iGrowBox.GetDouble(ref value);
				((BoundingBox)(ref boundingBox)).Max = ((BoundingBox)(ref boundingBox)).Max + new Vector3d(value, value, value);
				((BoundingBox)(ref boundingBox)).Min = ((BoundingBox)(ref boundingBox)).Min - new Vector3d(value, value, value);
			}
			Point3d val2 = ((BoundingBox)(ref boundingBox)).Min;
			double x = ((Point3d)(ref val2)).X;
			val2 = ((BoundingBox)(ref boundingBox)).Min;
			double y = ((Point3d)(ref val2)).Y;
			val2 = ((BoundingBox)(ref boundingBox)).Min;
			Point3d val3 = default(Point3d);
			((Point3d)(ref val3))._002Ector(x, y, ((Point3d)(ref val2)).Z - 1000.0);
			val2 = ((BoundingBox)(ref boundingBox)).Max;
			double x2 = ((Point3d)(ref val2)).X;
			val2 = ((BoundingBox)(ref boundingBox)).Max;
			double y2 = ((Point3d)(ref val2)).Y;
			val2 = ((BoundingBox)(ref boundingBox)).Max;
			Point3d val4 = default(Point3d);
			((Point3d)(ref val4))._002Ector(x2, y2, ((Point3d)(ref val2)).Z + 1000.0);
			BoundingBox val5 = default(BoundingBox);
			((BoundingBox)(ref val5))._002Ector(val3, val4);
			foreach (RhinoObject selectedObject in RhinoDoc.ActiveDoc.Objects.GetSelectedObjects(false, false))
			{
				if (selectedObject.Geometry == null)
				{
					continue;
				}
				BoundingBox boundingBox2 = selectedObject.Geometry.GetBoundingBox(selectedObject.Geometry is Curve);
				val2 = ((BoundingBox)(ref boundingBox2)).Min;
				if (((Point3d)(ref val2)).CompareTo(((BoundingBox)(ref boundingBox)).Min) == 0)
				{
					val2 = ((BoundingBox)(ref boundingBox2)).Max;
					if (((Point3d)(ref val2)).CompareTo(((BoundingBox)(ref boundingBox)).Max) == 0)
					{
						continue;
					}
				}
				if (!((BoundingBox)(ref val5)).Contains(boundingBox2))
				{
					continue;
				}
				GeometryBase val6 = selectedObject.Geometry.Duplicate();
				if (val6 != null)
				{
					if (selectedObject.Attributes.Name.Length > 0)
					{
						val6.SetUserString("CSName", selectedObject.Attributes.Name);
					}
					if (selectedObject.Attributes.LayerIndex >= 0 && selectedObject.Attributes.LayerIndex < RhinoDoc.ActiveDoc.Layers.Count)
					{
						string name = RhinoDoc.ActiveDoc.Layers[selectedObject.Attributes.LayerIndex].Name;
						val6.SetUserString("CSLayer", name);
					}
					On3dmObjectAttributes val7 = new On3dmObjectAttributes();
					archive.Write3dmObject((IOnObject)(object)val6.Convert(), (IOn3dmObjectAttributes)(object)val7);
				}
			}
		}
		return archive.EndWrite3dmChunk();
	}
}
