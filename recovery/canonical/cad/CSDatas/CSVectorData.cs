using CSDatas.Interfaces;
using Rhino.Geometry;

namespace CSDatas;

public sealed class CSVectorData : CSData, ICSVector
{
	public Vector3d VectorStore { get; set; }

	public CSVectorData(Vector3d vectorStore)
	{
		VectorStore = vectorStore;
	}

	public CSVectorData(CSVectorData copy)
		: base(copy)
	{
		VectorStore = copy.VectorStore;
	}

	public bool GetVector(out Vector3d value)
	{
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
		if (!(VectorStore == Vector3d.Unset))
		{
			return $"{VectorStore.X}:{VectorStore.Y}:{VectorStore.Z}";
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
