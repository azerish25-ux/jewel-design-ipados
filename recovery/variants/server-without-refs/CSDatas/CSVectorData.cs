using CSDatas.Interfaces;
using Rhino.Geometry;

namespace CSDatas;

public sealed class CSVectorData : CSData, ICSVector
{
	public Vector3d VectorStore { get; set; }

	public CSVectorData(Vector3d vectorStore)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		VectorStore = vectorStore;
	}

	public CSVectorData(CSVectorData copy)
		: base(copy)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		VectorStore = copy.VectorStore;
	}

	public bool GetVector(out Vector3d value)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		value = VectorStore;
		return true;
	}

	public bool HasVector()
	{
		return true;
	}

	public override CSData Duplicate()
	{
		return new CSVectorData(this);
	}

	public override string Report()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		if (!(VectorStore == Vector3d.Unset))
		{
			Vector3d vectorStore = VectorStore;
			object arg = ((Vector3d)(ref vectorStore)).X;
			vectorStore = VectorStore;
			object arg2 = ((Vector3d)(ref vectorStore)).Y;
			vectorStore = VectorStore;
			return $"{arg}:{arg2}:{((Vector3d)(ref vectorStore)).Z}";
		}
		return "Vector (empty)";
	}

	protected override void Dispose(bool disposing)
	{
		if (!_isDisposed)
		{
			try
			{
			}
			finally
			{
				base.Dispose(disposing);
			}
			_isDisposed = true;
		}
	}

	~CSVectorData()
	{
		Dispose(disposing: false);
	}
}
