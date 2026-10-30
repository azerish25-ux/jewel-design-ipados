using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CSDatas.Interfaces;
using CSMaterials;
using Rhino.Display;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSDatas;

public sealed class CSDataArray : CSData, ICSDisplayObject, ICSDocumentObject, ICSMeshable, ICSRenderObject, ICSTransform, ICSTransformable, ICSArray, ICSFlippable
{
	public List<CSData> Array { get; } = new List<CSData>();

	public CSDataArray()
	{
	}

	public CSDataArray(CSData data)
		: base(data)
	{
		Array.Add(data);
	}

	public CSDataArray(IEnumerable<CSData> inputArray)
	{
		inputArray?.ToList().ForEach(delegate(CSData input)
		{
			Array.Add(input.Duplicate());
		});
	}

	private CSDataArray(CSDataArray input)
		: base(input)
	{
		foreach (CSData item in input.Array)
		{
			Array.Add((item == null) ? new CSData() : item.Duplicate());
		}
	}

	public bool IsIndexValid(int index)
	{
		if (index >= 0)
		{
			return index < Array.Count;
		}
		return false;
	}

	public int GetArrayCount()
	{
		return Array.Count;
	}

	public CSData GetArrayItem(int index)
	{
		if (!IsIndexValid(index))
		{
			return new CSData();
		}
		return Array[index];
	}

	public CSDataArray GetFlattenArray()
	{
		CSDataArray cSDataArray = new CSDataArray();
		foreach (CSData item in Array.ToList())
		{
			if (item == null)
			{
				cSDataArray.Append(new CSData());
				continue;
			}
			if (!(item is ICSArray iCSArray))
			{
				cSDataArray.Append(item);
				continue;
			}
			CSDataArray flattenArray = iCSArray.GetFlattenArray();
			if (flattenArray == null)
			{
				cSDataArray.Append(new CSData());
				continue;
			}
			foreach (CSData item2 in flattenArray.Array.ToList())
			{
				cSDataArray.Append(item2);
			}
		}
		return cSDataArray;
	}

	public List<CSData> GetFlattenedList()
	{
		List<CSData> flatData = new List<CSData>();
		Array.ForEach(delegate(CSData _)
		{
			if (!(_ is ICSArray iCSArray))
			{
				flatData.Add(_);
			}
			else
			{
				flatData.AddRange(iCSArray.GetFlattenedList());
			}
		});
		return flatData;
	}

	public bool Display(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions, CSDisplayMaterial csDisplayMaterial = null)
	{
		bool flag = true;
		foreach (ICSDisplayObject item in GetFlattenArray().Array.OfType<ICSDisplayObject>())
		{
			flag = ((csDisplayMaterial == null) ? (flag & item.Display(displayPipeline, displayOptions)) : (flag & item.Display(displayPipeline, displayOptions, csDisplayMaterial)));
		}
		return flag;
	}

	public bool AssignCurrentMaterialGroup(MaterialGroup materialGroup)
	{
		return GetFlattenArray().Array.OfType<ICSDisplayObject>().Aggregate(seed: true, (bool current, ICSDisplayObject csDisplayObject) => current & csDisplayObject.AssignCurrentMaterialGroup(materialGroup));
	}

	public void AddToDocument()
	{
		foreach (CSData item in Array)
		{
			(item as ICSDocumentObject)?.AddToDocument();
		}
	}

	public bool AddToDocumentExport(ref ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, Transform transform, CSOptionsExport exportOptions)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		bool flag = true;
		foreach (CSData item in GetFlattenArray().Array.ToList())
		{
			if (item is ICSDocumentObject iCSDocumentObject)
			{
				flag &= iCSDocumentObject.AddToDocumentExport(ref rhinoRenderObjects, transform, exportOptions);
			}
		}
		return flag;
	}

	public bool GetMeshableObjects(ref ConcurrentBag<GeometryBase> meshableObjects, CSOptionsExport exportOptions)
	{
		bool flag = true;
		foreach (CSData flattened in GetFlattenedList())
		{
			if (flattened is ICSMeshable iCSMeshable)
			{
				flag &= iCSMeshable.GetMeshableObjects(ref meshableObjects, exportOptions);
			}
		}
		return flag;
	}

	public bool AddToDocumentRender(ref ConcurrentBag<RhinoRenderObject> renderObjects, Transform transform)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		bool flag = true;
		foreach (CSData item in GetFlattenArray().Array)
		{
			if (item is ICSRenderObject iCSRenderObject)
			{
				flag &= iCSRenderObject.AddToDocumentRender(ref renderObjects, transform);
			}
		}
		return flag;
	}

	public bool GetTransform(out Transform value)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		if (GetArrayCount() > 0)
		{
			return CSUtilMath.GetTransformNoArray(Array[0], out value);
		}
		value = Transform.Unset;
		return false;
	}

	public virtual bool ApplyTransform(Transform transform)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		bool result = false;
		foreach (CSData item in Array)
		{
			if (item is ICSTransformable iCSTransformable && iCSTransformable.ApplyTransform(transform))
			{
				result = true;
			}
		}
		return result;
	}

	public virtual void Append(CSData data)
	{
		Array.Add(data);
	}

	public override CSData Duplicate()
	{
		return new CSDataArray(this);
	}

	public void Flip()
	{
		foreach (CSData item in Array)
		{
			(item as ICSFlippable)?.Flip();
		}
	}

	public override bool GetAllMaterial(List<CSData> metal, Transform transform, MaterialClassification whicMaterialClassification, MaterialClassification searchMaterialClassification)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		if (whicMaterialClassification == MaterialClassification.Undefined || whicMaterialClassification == MaterialClassification.UseOriginal)
		{
			whicMaterialClassification = base.DefaultDisplayMaterialClassification;
		}
		bool result = false;
		foreach (CSData item in Array)
		{
			if (item != null && item.GetAllMaterial(metal, transform, whicMaterialClassification, searchMaterialClassification))
			{
				result = true;
			}
		}
		return result;
	}

	public override bool GetBoundingBox(ref BoundingBox boundingBox)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		BoundingBox val = BoundingBox.Unset;
		foreach (CSData flattened in GetFlattenedList())
		{
			if (flattened == null)
			{
				continue;
			}
			BoundingBox boundingBox2 = BoundingBox.Unset;
			if (flattened.GetBoundingBox(ref boundingBox2))
			{
				if (((BoundingBox)(ref val)).IsValid)
				{
					((BoundingBox)(ref val)).Union(boundingBox2);
				}
				else
				{
					val = boundingBox2;
				}
			}
		}
		if (!((BoundingBox)(ref val)).MakeValid())
		{
			return false;
		}
		boundingBox = val;
		return true;
	}

	[Obsolete]
	public override bool GetTightBoundingBox(ref BoundingBox boundingBox)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return GetTightBoundingBox(ref boundingBox, doGrow: false, Transform.Identity);
	}

	[Obsolete]
	public override bool GetTightBoundingBox(ref BoundingBox boundingBox, bool doGrow)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return GetTightBoundingBox(ref boundingBox, doGrow, Transform.Identity);
	}

	[Obsolete]
	public override bool GetTightBoundingBox(ref BoundingBox boundingBox, bool doGrow, Transform transform)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		BoundingBox val = BoundingBox.Unset;
		foreach (CSData flattened in GetFlattenedList())
		{
			if (flattened == null)
			{
				continue;
			}
			BoundingBox boundingBox2 = BoundingBox.Unset;
			if (flattened.GetTightBoundingBox(ref boundingBox2, doGrow: false, transform))
			{
				if (((BoundingBox)(ref val)).IsValid)
				{
					((BoundingBox)(ref val)).Union(boundingBox2);
				}
				else
				{
					val = boundingBox2;
				}
			}
		}
		if (!((BoundingBox)(ref val)).MakeValid())
		{
			return false;
		}
		if (((BoundingBox)(ref boundingBox)).IsValid && doGrow)
		{
			((BoundingBox)(ref boundingBox)).Union(val);
		}
		else
		{
			boundingBox = val;
		}
		return true;
	}

	public override bool GetTightBoundingBoxNew(ref BoundingBox boundingBox)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return GetTightBoundingBoxNew(ref boundingBox, doGrow: false, Transform.Identity);
	}

	public override bool GetTightBoundingBoxNew(ref BoundingBox boundingBox, bool doGrow)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return GetTightBoundingBoxNew(ref boundingBox, doGrow, Transform.Identity);
	}

	public override bool GetTightBoundingBoxNew(ref BoundingBox boundingBox, bool doGrow, Transform transform)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		BoundingBox val = BoundingBox.Unset;
		foreach (CSData flattened in GetFlattenedList())
		{
			if (flattened == null)
			{
				continue;
			}
			BoundingBox boundingBox2 = BoundingBox.Unset;
			if (!(flattened is CSOnGeometryData))
			{
				if (!flattened.GetTightBoundingBox(ref boundingBox2, doGrow: false, transform))
				{
					continue;
				}
			}
			else if (!flattened.GetTightBoundingBoxNew(ref boundingBox2, doGrow: false, transform))
			{
				continue;
			}
			if (((BoundingBox)(ref val)).IsValid)
			{
				((BoundingBox)(ref val)).Union(boundingBox2);
			}
			else
			{
				val = boundingBox2;
			}
		}
		if (!((BoundingBox)(ref val)).MakeValid())
		{
			return false;
		}
		if (((BoundingBox)(ref boundingBox)).IsValid && doGrow)
		{
			((BoundingBox)(ref boundingBox)).Union(val);
		}
		else
		{
			boundingBox = val;
		}
		return true;
	}

	public override bool GetUserString(string key, ref string value)
	{
		if (Array.Count > 0)
		{
			return Array[0].GetUserString(key, ref value);
		}
		return false;
	}

	public override Mesh MeshObject(MeshingParameters mp, bool productionMeshes = false, bool useDisplayMesh = true)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		Mesh val = new Mesh();
		foreach (CSData flattened in GetFlattenedList())
		{
			Mesh val2 = flattened?.MeshObject(mp, productionMeshes, useDisplayMesh);
			if (val2 != null)
			{
				val.Append(val2);
			}
		}
		if (!((CommonObject)val).IsValid)
		{
			return null;
		}
		return val;
	}

	public override bool Morph(SpaceMorph spaceMorph)
	{
		bool result = true;
		(from _ in GetFlattenedList()
			where _ != null
			select _).ToList().ForEach(delegate(CSData _)
		{
			result &= _.Morph(spaceMorph);
		});
		return result;
	}

	public void RemoveAt(int index)
	{
		Array.RemoveAt(index);
	}

	public override void SetMaterialClassificationOnDisplay(MaterialClassification materialClassification)
	{
		if (base.DefaultDisplayMaterialClassification == MaterialClassification.Undefined)
		{
			base.DefaultDisplayMaterialClassification = materialClassification;
			(from _ in GetFlattenedList()
				where _ != null
				select _).ToList().ForEach(delegate(CSData _)
			{
				_.SetMaterialClassificationOnDisplay(materialClassification);
			});
		}
	}

	public override void SetOutputObject()
	{
		base.SetOutputObject();
		(from _ in GetFlattenedList()
			where _ != null
			select _).ToList().ForEach(delegate(CSData _)
		{
			_.SetOutputObject();
		});
	}

	protected override void Dispose(bool disposing)
	{
		if (_isDisposed)
		{
			return;
		}
		try
		{
			if (disposing)
			{
				foreach (CSData item in Array)
				{
					IDisposable disposable = item;
					if (disposable != null && (item is CSDataArray || item is CSOnGeometryData))
					{
						disposable.Dispose();
					}
				}
			}
		}
		finally
		{
			base.Dispose(disposing);
		}
		_isDisposed = true;
	}

	~CSDataArray()
	{
		Dispose(disposing: false);
	}
}
