using System.Drawing;
using System.Linq;
using CS.Parametric;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CS.CSObjects.CSTreeObjects;

[CSBlock("SuperViewGroup", "{B5A0AA97-4201-49FA-B309-29CB0BF7ACD6}")]
public sealed class CSSuperViewGroup : CSTreeObject
{
	public bool workingGroup;

	public override int InputChannelSVDisplayCount => InputChannelCount + (base.Expanded ? 1 : 0);

	public override int OutputChannelSVDisplayCount => OutputChannelCount + (base.Expanded ? 1 : 0);

	public override int SizeXSmall
	{
		get
		{
			int num = (int)GetIdentSize();
			int inputChannelCount = InputChannelCount;
			int outputChannelCount = OutputChannelCount;
			int num2 = 106 + num;
			if (inputChannelCount * 12 + 56 > num2)
			{
				num2 = inputChannelCount * 12 + 56;
			}
			if (outputChannelCount * 12 + 58 > num2)
			{
				num2 = outputChannelCount * 12 + 58;
			}
			return num2;
		}
	}

	public override int SizeYSmall => 107;

	public override Rectangle MainBoxMouseHit => new Rectangle(11, 10, XSize - 12, YSize - 20);

	public override ArrowDir NubInDirection => ArrowDir.Up;

	public override ArrowDir NubOutDirection => ArrowDir.Down;

	public override ArrowDir InternalSocketInDirection => ArrowDir.Down;

	public override ArrowDir InternalSocketOutDirection => ArrowDir.Up;

	public override Rectangle SizeDragHit
	{
		get
		{
			if (!base.Expanded)
			{
				return Rectangle.Empty;
			}
			return new Rectangle(XSize - 27, YSize - 35, 18, 18);
		}
	}

	public CSSuperViewGroup(string identifier)
		: base(identifier)
	{
	}

	public override bool Delete(bool checkOutputs = true)
	{
		if (checkOutputs && !IsBlockDeleteable())
		{
			return false;
		}
		foreach (CSTreeObject item in base.Children.OfType<CSTreeObject>())
		{
			item.Delete(checkOutputs: false);
		}
		int index = CSApplicationHelper.Application.Document.Graph.GraphList.FindIndex((CSObject check) => Equals(check));
		CSApplicationHelper.Application.Document.Graph.GraphList[index].Deleted = true;
		return true;
	}

	public override Point InternalSocketInLocation(int index)
	{
		return new Point(36 + index * 12 + 5, 10);
	}

	public override Point InternalSocketOutLocation(int index)
	{
		return new Point(XSize - 33 - (OutputChannelSVDisplayCount - index) * 12, YSize - 10);
	}

	public override Point NubInLocation(int index)
	{
		return new Point(base.XPosLocal + 41 + index * 12, base.YPosLocal + 4);
	}

	public override Rectangle NubInMouseHit(int index)
	{
		return new Rectangle(41 + index * 12 - 5, 0, 10, 9);
	}

	public override Point NubOutLocation(int index)
	{
		return new Point(base.XPosLocal + XSize - 33 - (OutputChannelSVDisplayCount - index) * 12, base.YPosLocal + YSize - 4);
	}

	public override Rectangle NubOutMouseHit(int index)
	{
		return new Rectangle(XSize - 33 - (OutputChannelSVDisplayCount - index) * 12 - 5, YSize - 10, 10, 9);
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		bool result = base.Read(ref archive, graph, major, minor);
		GetStaticUserData(graph);
		return result;
	}

	public override bool ReadJson(JObject jObject, CSParaGraph graph, int major, int minor)
	{
		bool result = base.ReadJson(jObject, graph, major, minor);
		GetStaticUserData(graph);
		return result;
	}

	private bool IsBlockDeleteable()
	{
		int outputChannelCount = OutputChannelCount;
		for (int i = 0; i < outputChannelCount; i++)
		{
			if (GetOutputChannel(i).OutputChannelBlock != null)
			{
				return false;
			}
		}
		return true;
	}
}
