using System;
using System.Drawing;
using CS.JSonDoc;
using CS.Parametric;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CS.CSObjects.CSTreeObjects;

[CSBlock("BlockGroupStart", "{5D44163E-7E07-442b-BA86-375A93C41D1E}")]
public sealed class CSBlockGroupStart : CSTreeObject
{
	private Guid _blockgroupIdentifier;

	private uint _groupFlags;

	public override int SizeXSmall => (int)GetIdentSize() + 18;

	public override int SizeYSmall => Math.Max(InputChannelCount, OutputChannelCount) * 8 + 14;

	public override ArrowDir NubInDirection => ArrowDir.Left;

	public override ArrowDir NubOutDirection => ArrowDir.Right;

	public override ArrowDir InternalSocketInDirection => ArrowDir.Right;

	public override ArrowDir InternalSocketOutDirection => ArrowDir.Left;

	public override Rectangle MainBoxMouseHit => new Rectangle(1, 1, XSize - 2, YSize - 2);

	public override Rectangle SizeDragHit
	{
		get
		{
			if (!base.Expanded)
			{
				return Rectangle.Empty;
			}
			return new Rectangle(XSize - 25, YSize - 25, 16, 16);
		}
	}

	public Guid BlockGroupIdentifier
	{
		get
		{
			return _blockgroupIdentifier;
		}
		set
		{
			_blockgroupIdentifier = value;
		}
	}

	public int BlockGroupVersionNumber { get; set; }

	public bool MouseOverGroup { get; set; }

	public virtual bool UICollapsedGroup
	{
		get
		{
			return (_groupFlags & 1) == 1;
		}
		set
		{
			if (value)
			{
				_groupFlags |= 1u;
			}
			else
			{
				_groupFlags &= 4294967294u;
			}
		}
	}

	public CSBlockGroupStart(string identifier)
		: base(identifier)
	{
	}

	public override bool Delete(bool checkOUtputs = true)
	{
		return true;
	}

	public override Point InternalSocketInLocation(int index)
	{
		return new Point(0, (YSize - InputChannelCount * 8) / 2 + index * 8 + 5);
	}

	public override Point InternalSocketOutLocation(int index)
	{
		return new Point(XSize, (YSize - OutputChannelCount * 8) / 2 + index * 8 + 5);
	}

	public override Point NubInLocation(int index)
	{
		return new Point(base.XPosLocal + 1, base.YPosLocal + (YSize - InputChannelCount * 8) / 2 + index * 8 + 5);
	}

	public override Rectangle NubInMouseHit(int index)
	{
		return Rectangle.Empty;
	}

	public override Point NubOutLocation(int index)
	{
		return new Point(base.XPosLocal + XSize - 1, base.YPosLocal + (YSize - OutputChannelCount * 8) / 2 + index * 8 + 5);
	}

	public override Rectangle NubOutMouseHit(int index)
	{
		return Rectangle.Empty;
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		bool flag = base.Read(ref archive, graph, major, minor);
		if (flag)
		{
			if (major >= 2 || (major == 1 && minor >= 1))
			{
				archive.ReadInt(ref _groupFlags);
			}
			if (major >= 3 || (major == 2 && minor >= 3))
			{
				string empty = string.Empty;
				int blockGroupVersionNumber = 0;
				flag &= archive.ReadString(ref empty);
				flag &= archive.ReadInt(ref blockGroupVersionNumber);
				if (flag)
				{
					Guid.TryParse(empty, out _blockgroupIdentifier);
					BlockGroupVersionNumber = blockGroupVersionNumber;
				}
			}
		}
		GetStaticUserData(graph);
		return flag;
	}

	public override bool ReadJson(JObject archive, CSParaGraph graph, int major, int minor)
	{
		string value;
		bool num = base.ReadJson(archive, graph, major, minor) & archive.ReadJsonValue<uint>("GroupFlags", out _groupFlags) & archive.ReadJsonValue<string>("BlockGroupIdentifier", out value);
		_blockgroupIdentifier = new Guid(value);
		int value2;
		bool result = num & archive.ReadJsonValue<int>("BlockGroupVersionNumber", out value2);
		BlockGroupVersionNumber = value2;
		GetStaticUserData(graph);
		return result;
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		bool num = base.Write(ref archive);
		if (num)
		{
			archive.WriteInt(_groupFlags);
		}
		return num & archive.WriteString(BlockGroupIdentifier.ToString()) & archive.WriteInt(BlockGroupVersionNumber);
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		archive.Add("GroupFlags", _groupFlags);
		archive.Add("BlockGroupIdentifier", BlockGroupIdentifier);
		archive.Add("BlockGroupVersionNumber", BlockGroupVersionNumber);
	}
}
