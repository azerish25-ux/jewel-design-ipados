namespace CSRingSizes.RingSize;

public abstract class RingSize
{
	public abstract int Count { get; }

	public abstract string RingSizeTypeAbrv { get; }

	public abstract ERingSizeOption RingSizeType { get; }

	public abstract int GetIndex(double d);

	public virtual bool GetIndexTxt(string size, ref int index)
	{
		int count = Count;
		for (int i = 0; i < count; i++)
		{
			if (TxtSize(i).CompareTo(size) == 0)
			{
				index = i;
				return true;
			}
		}
		return false;
	}

	public virtual UniversalRingSize GetSize(double diameter)
	{
		UniversalRingSize result = new UniversalRingSize
		{
			RingSizeTypeAbrv = RingSizeTypeAbrv
		};
		int index = GetIndex(diameter);
		result.diameterMM = MmDiameter(index);
		result.Size = TxtSize(index);
		result.radiusMM = result.diameterMM / 2.0;
		result.index = index;
		return result;
	}

	public virtual UniversalRingSize GetSizeIndex(double index)
	{
		UniversalRingSize result = new UniversalRingSize
		{
			RingSizeTypeAbrv = RingSizeTypeAbrv
		};
		int num = (int)index;
		result.diameterMM = MmDiameter(num);
		result.Size = TxtSize(num);
		result.radiusMM = result.diameterMM / 2.0;
		result.index = num;
		return result;
	}

	public virtual UniversalRingSize GetSizeRadius(double radius)
	{
		return GetSize(radius * 2.0);
	}

	public virtual UniversalRingSize GetSizeTxt(string txt)
	{
		UniversalRingSize result = new UniversalRingSize
		{
			RingSizeTypeAbrv = RingSizeTypeAbrv,
			ringSizeType = RingSizeType
		};
		int index = 0;
		if (!GetIndexTxt(txt, ref index))
		{
			return result;
		}
		result.diameterMM = MmDiameter(index);
		result.Size = TxtSize(index);
		result.radiusMM = result.diameterMM / 2.0;
		result.index = index;
		return result;
	}

	public abstract double MmDiameter(int n);

	public abstract string TxtSize(int n);
}
