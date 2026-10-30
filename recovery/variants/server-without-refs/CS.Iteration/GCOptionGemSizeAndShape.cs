using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.CSValues;
using CSDatas;
using Newtonsoft.Json;

namespace CS.Iteration;

[Serializable]
public sealed class GCOptionGemSizeAndShape : GCOption
{
	public struct GCOptionGem
	{
		public string type;

		public string shape;

		public double height;

		public double width;

		public double depth;

		public double percentage;

		public string Name => type + "_" + shape;

		public GCOptionGem(string t, string s, double w, double h, double d, double p)
		{
			width = w;
			height = h;
			depth = d;
			percentage = p;
			shape = s;
			type = t;
		}

		public void SwapHeightWidth()
		{
			double num = height;
			height = width;
			width = num;
		}
	}

	[XmlIgnore]
	[JsonIgnore]
	public CSValueGem gem;

	public List<GCOptionGem> values = new List<GCOptionGem>();

	public GCOptionGem original;

	public GCOptionGemSizeAndShape()
	{
	}

	public GCOptionGemSizeAndShape(CSValueGem g)
		: base(g)
	{
		gem = g;
		StoreOrginalFromObject();
	}

	public bool AddShape(string type, string shape, double width, double height, double d, double p)
	{
		if (!values.Any((GCOptionGem _) => _.type == type && _.shape == shape && Math.Abs(_.width - width) < 0.001 && Math.Abs(_.height - height) < 0.001))
		{
			GCOptionGem item = new GCOptionGem(type, shape, width, height, d, p);
			values.Add(item);
			return true;
		}
		return false;
	}

	public string GetCurrentValueString(int index)
	{
		if (index < 0 || index >= values.Count)
		{
			return "";
		}
		GCOptionGem gCOptionGem = values[index];
		return string.Format(CultureInfo.InvariantCulture, "Gem:{0:0.00}:{1:0.00}:{2:0.00}:{3}:{4}", gCOptionGem.height, gCOptionGem.width, gCOptionGem.depth, gCOptionGem.type, gCOptionGem.shape);
	}

	public override int GetIterationCount()
	{
		return values.Count;
	}

	public static bool ParseCurrentValueString(string input, ref double height, ref double width, ref double depth, ref string type, ref string shape)
	{
		string[] array = input.Split(':');
		if (array == null)
		{
			return false;
		}
		if (array.Length < 6)
		{
			return false;
		}
		if (!array[0].Equals("Gem"))
		{
			return false;
		}
		int result = (int)(1u & (double.TryParse(array[1], NumberStyles.Any, CultureInfo.InvariantCulture, out height) ? 1u : 0u) & (double.TryParse(array[2], NumberStyles.Any, CultureInfo.InvariantCulture, out width) ? 1u : 0u)) & (double.TryParse(array[3], NumberStyles.Any, CultureInfo.InvariantCulture, out depth) ? 1 : 0);
		type = array[4];
		shape = array[5];
		return (byte)result != 0;
	}

	public override bool RestoreOrginalToObject()
	{
		if (gem == null)
		{
			return false;
		}
		Set(original);
		return true;
	}

	public bool Set(GCOptionGem gemValue)
	{
		if (gem == null)
		{
			return false;
		}
		CSBlock chooserSource = gem.GetChooserSource();
		int num = -1;
		if (chooserSource != null)
		{
			for (int i = 0; i < chooserSource.OutputChannelCount; i++)
			{
				if (chooserSource.GetOutputChannelData(i) is CSGemData cSGemData && cSGemData.GemType == gemValue.type && cSGemData.GemShape == gemValue.shape)
				{
					num = i;
					break;
				}
			}
		}
		bool flag = true;
		flag = num >= 0 && (flag & gem.SetValue(num, doOverridePrevious: true));
		gem.PreChange("Set");
		gem.Width = gemValue.width;
		gem.Depth = gemValue.depth;
		gem.Height = gemValue.height;
		gem.Percentage = gemValue.percentage;
		gem.Change();
		return flag;
	}

	public override bool SetFromIteration(int index)
	{
		if (index < 0 || index >= values.Count)
		{
			return false;
		}
		if (gem == null)
		{
			return false;
		}
		CurrentValueString = GetCurrentValueString(index);
		return Set(values[index]);
	}

	public override void SetGuid(Dictionary<Guid, CSObject> guidDictionary)
	{
		base.SetGuid(guidDictionary);
		gem = obj as CSValueGem;
	}

	public override bool StoreOrginalFromObject()
	{
		if (gem == null)
		{
			return false;
		}
		CSBlock chooserSource = gem.GetChooserSource();
		if (chooserSource != null && chooserSource.GetOutputChannelData((int)gem.Value) is CSGemData cSGemData)
		{
			original.shape = cSGemData.GemShape;
			original.type = cSGemData.GemType;
		}
		original.width = gem.Width;
		original.depth = gem.Depth;
		original.height = gem.Height;
		original.percentage = gem.Percentage;
		return true;
	}

	public int SwapHeightWidth()
	{
		int num = 0;
		for (int i = 0; i < values.Count; i++)
		{
			values[i] = new GCOptionGem(values[i].type, values[i].shape, values[i].height, values[i].width, values[i].depth, values[i].percentage);
			if (Math.Abs(values[i].height - values[i].width) > 0.01)
			{
				num++;
			}
		}
		return num;
	}

	private CSBlock GetGroupOfGems(CSValueGem vGem)
	{
		if (vGem == null)
		{
			return null;
		}
		if (vGem.InGems.OutputChannelBlock == null)
		{
			return null;
		}
		return vGem.InGems.OutputChannelBlock;
	}
}
