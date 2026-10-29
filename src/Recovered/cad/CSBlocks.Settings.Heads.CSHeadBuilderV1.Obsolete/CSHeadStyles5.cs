using System.Collections.Generic;
using GV.CounterSketch.Shared;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

public static class CSHeadStyles5
{
	public enum EHeadProngArrangment
	{
		Single,
		Double,
		Triple
	}

	public enum EHeadProngType
	{
		Round,
		Straight
	}

	public struct HeadProngInfo
	{
		public double Overlap { get; }

		public int ProngParamIndex { get; }

		public EHeadProngArrangment Arrangment { get; }

		public EHeadProngType Type { get; }

		public int MoveDir { get; }

		public HeadProngInfo(int prongParamIndex, double overlap, EHeadProngArrangment arrangment, EHeadProngType type, int moveDir)
		{
			ProngParamIndex = prongParamIndex;
			Overlap = overlap;
			Arrangment = arrangment;
			Type = type;
			MoveDir = moveDir;
		}
	}

	public static List<HeadProngInfo> GetHeadStyle(string shape, ValueListItem whichStyle)
	{
		if ((shape.ToLower().StartsWith("baguette") && shape.ToLower().Contains("_")) || shape.ToLower().StartsWith("baguettetapered"))
		{
			shape = "baguette straight";
		}
		if (shape.ToLower().StartsWith("emerald") && shape.ToLower().Contains("_"))
		{
			shape = "emerald";
		}
		List<HeadProngInfo> list = new List<HeadProngInfo>();
		switch (whichStyle)
		{
		case ValueListItem.HeadStyleCenter1:
			switch (shape.ToLower())
			{
			case "emerald":
			case "emerald rotated 90":
			case "emerald square":
			case "emerald square rotated 45":
			case "round":
			case "oval":
			case "oval rotated 90":
			case "cushion":
			case "cushion rotated 90":
			case "cushion square":
			case "cushion square rotated 45":
			case "octagon":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "baguette straight":
			case "princess":
			case "princess rotated 45":
			case "radiant":
			case "radiant square":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				list.Add(new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				break;
			case "half moon":
				list.Add(new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "pear":
			case "heart":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "marquise":
				list.Add(new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				break;
			case "marquise rotated 90":
				list.Add(new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(18, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				break;
			case "triangle":
			case "trillion curved":
			case "trillion straight":
			case "calf":
				list.Add(new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				break;
			}
			break;
		case ValueListItem.HeadStyleCenter2:
			switch (shape.ToLower())
			{
			case "round":
			case "baguette straight":
			case "oval":
			case "oval rotated 90":
			case "emerald":
			case "emerald rotated 90":
			case "emerald square":
			case "emerald square rotated 45":
			case "radiant":
			case "radiant square":
			case "cushion":
			case "cushion rotated 90":
			case "cushion square":
			case "cushion square rotated 45":
			case "octagon":
				list.Add(new HeadProngInfo(3, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(21, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1));
				break;
			case "princess":
			case "princess rotated 45":
				list.Add(new HeadProngInfo(3, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, -1));
				list.Add(new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(21, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, -1));
				break;
			case "half moon":
				list.Add(new HeadProngInfo(0, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(4, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(8, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(12, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1));
				break;
			case "pear":
			case "heart":
				list.Add(new HeadProngInfo(3, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(12, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(21, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1));
				break;
			case "marquise":
				list.Add(new HeadProngInfo(0, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(12, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 0));
				break;
			case "marquise rotated 90":
				list.Add(new HeadProngInfo(6, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 0));
				list.Add(new HeadProngInfo(18, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 0));
				break;
			case "triangle":
			case "trillion curved":
			case "trillion straight":
			case "calf":
				list.Add(new HeadProngInfo(4, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(12, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 0));
				list.Add(new HeadProngInfo(20, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1));
				break;
			}
			break;
		case ValueListItem.HeadStyleCenter3:
			switch (shape.ToLower())
			{
			case "round":
			case "oval":
			case "oval rotated 90":
			case "cushion":
			case "cushion rotated 90":
			case "cushion square":
			case "cushion square rotated 45":
			case "baguette straight":
			case "princess":
			case "princess rotated 45":
			case "emerald":
			case "emerald rotated 90":
			case "emerald square":
			case "emerald square rotated 45":
			case "radiant":
			case "radiant square":
			case "octagon":
				list.Add(new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0));
				list.Add(new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0));
				list.Add(new HeadProngInfo(18, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "marquise":
			case "marquise rotated 90":
				list.Add(new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(18, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "half moon":
				list.Add(new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "pear":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(12, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "heart":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "triangle":
			case "trillion curved":
			case "trillion straight":
			case "calf":
				list.Add(new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				break;
			}
			break;
		case ValueListItem.HeadStyleCenter4:
			switch (shape.ToLower())
			{
			case "round":
			case "oval":
			case "cushion":
			case "cushion rotated 90":
				list.Add(new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0));
				list.Add(new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0));
				list.Add(new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "marquise":
				list.Add(new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "half moon":
				list.Add(new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				list.Add(new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(18, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				break;
			case "oval rotated 90":
			case "marquise rotated 90":
				list.Add(new HeadProngInfo(2, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				list.Add(new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(18, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(22, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				break;
			case "emerald square":
			case "emerald square rotated 45":
			case "emerald":
			case "emerald rotated 90":
			case "octagon":
				list.Add(new HeadProngInfo(2, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(22, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "baguette straight":
			case "princess":
			case "princess rotated 45":
			case "radiant":
			case "radiant square":
			case "cushion square":
			case "cushion square rotated 45":
				list.Add(new HeadProngInfo(2, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(22, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "pear":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "heart":
				list.Add(new HeadProngInfo(2, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(11, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(13, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(22, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "triangle":
			case "trillion curved":
			case "trillion straight":
			case "calf":
				list.Add(new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				list.Add(new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				list.Add(new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				break;
			}
			break;
		case ValueListItem.HeadStyleCenter5:
			switch (shape.ToLower())
			{
			case "round":
			case "oval":
			case "oval rotated 90":
			case "emerald":
			case "emerald rotated 90":
			case "emerald square":
			case "emerald square rotated 45":
			case "radiant":
			case "radiant square":
			case "cushion":
			case "cushion rotated 90":
			case "cushion square":
			case "cushion square rotated 45":
			case "octagon":
				list.Add(new HeadProngInfo(3, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(21, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1));
				break;
			case "half moon":
				list.Add(new HeadProngInfo(0, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(4, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(8, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(12, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1));
				break;
			case "baguette straight":
			case "princess":
			case "princess rotated 45":
				list.Add(new HeadProngInfo(3, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, -1));
				list.Add(new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(21, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, -1));
				break;
			case "pear":
			case "heart":
				list.Add(new HeadProngInfo(3, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(12, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(21, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1));
				break;
			case "marquise":
				list.Add(new HeadProngInfo(0, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(12, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, 0));
				break;
			case "marquise rotated 90":
				list.Add(new HeadProngInfo(6, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 0));
				list.Add(new HeadProngInfo(18, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 0));
				break;
			case "triangle":
			case "trillion curved":
			case "trillion straight":
			case "calf":
				list.Add(new HeadProngInfo(4, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(12, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 0));
				list.Add(new HeadProngInfo(20, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1));
				break;
			}
			break;
		case ValueListItem.HeadStyleCenter6:
			switch (shape.ToLower())
			{
			case "round":
			case "baguette straight":
			case "princess":
			case "princess rotated 45":
			case "oval":
			case "oval rotated 90":
			case "emerald":
			case "emerald rotated 90":
			case "emerald square":
			case "emerald square rotated 45":
			case "radiant":
			case "radiant square":
			case "cushion":
			case "cushion rotated 90":
			case "cushion square":
			case "cushion square rotated 45":
			case "octagon":
				list.Add(new HeadProngInfo(2, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(22, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "half moon":
				list.Add(new HeadProngInfo(23, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(1, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(5, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(7, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(11, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(13, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "pear":
			case "heart":
				list.Add(new HeadProngInfo(2, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(11, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(13, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(22, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "marquise":
				list.Add(new HeadProngInfo(1, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(11, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(13, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(23, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "marquise rotated 90":
				list.Add(new HeadProngInfo(5, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(7, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(17, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(19, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "triangle":
			case "trillion curved":
			case "trillion straight":
			case "calf":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(5, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(11, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(13, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(19, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			}
			break;
		case ValueListItem.HeadStyleSide1:
			switch (shape.ToLower())
			{
			case "round":
			case "baguette straight":
			case "oval":
			case "oval rotated 90":
			case "emerald":
			case "emerald rotated 90":
			case "emerald square":
			case "emerald square rotated 45":
			case "radiant":
			case "radiant square":
			case "cushion":
			case "cushion rotated 90":
			case "cushion square":
			case "cushion square rotated 45":
			case "octagon":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "princess":
			case "princess rotated 45":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				list.Add(new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				break;
			case "half moon":
				list.Add(new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "pear":
			case "heart":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "marquise":
				list.Add(new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				break;
			case "marquise rotated 90":
				list.Add(new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(18, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				break;
			case "triangle":
			case "trillion curved":
			case "trillion straight":
			case "calf":
				list.Add(new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				break;
			}
			break;
		case ValueListItem.HeadStyleSide2:
			switch (shape.ToLower())
			{
			case "round":
			case "baguette straight":
			case "oval":
			case "oval rotated 90":
			case "emerald":
			case "emerald rotated 90":
			case "emerald square":
			case "emerald square rotated 45":
			case "radiant":
			case "radiant square":
			case "cushion":
			case "cushion rotated 90":
			case "cushion square":
			case "cushion square rotated 45":
			case "octagon":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "princess":
			case "princess rotated 45":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, -1));
				list.Add(new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				break;
			case "half moon":
				list.Add(new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(4, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(8, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				break;
			case "pear":
			case "heart":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(12, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "marquise":
				list.Add(new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(12, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 0));
				break;
			case "marquise rotated 90":
				list.Add(new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(18, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 0));
				break;
			case "triangle":
			case "trillion curved":
			case "trillion straight":
			case "calf":
				list.Add(new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(12, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				break;
			}
			break;
		case ValueListItem.HeadStyleSide3:
			switch (shape.ToLower())
			{
			case "round":
			case "baguette straight":
			case "princess":
			case "princess rotated 45":
			case "oval":
			case "oval rotated 90":
			case "emerald":
			case "emerald rotated 90":
			case "emerald square":
			case "emerald square rotated 45":
			case "radiant":
			case "radiant square":
			case "cushion":
			case "cushion rotated 90":
			case "cushion square":
			case "cushion square rotated 45":
			case "octagon":
				list.Add(new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0));
				list.Add(new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0));
				list.Add(new HeadProngInfo(18, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "marquise":
			case "marquise rotated 90":
				list.Add(new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(18, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "half moon":
				list.Add(new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "pear":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "heart":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "triangle":
			case "trillion curved":
			case "trillion straight":
			case "calf":
				list.Add(new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				break;
			}
			break;
		case ValueListItem.HeadStyleSide4:
			switch (shape.ToLower())
			{
			case "round":
			case "oval":
			case "oval rotated 90":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0));
				list.Add(new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "baguette straight":
			case "emerald":
			case "emerald rotated 90":
			case "emerald square":
			case "emerald square rotated 45":
			case "radiant":
			case "radiant square":
			case "cushion":
			case "cushion rotated 90":
			case "cushion square":
			case "cushion square rotated 45":
			case "octagon":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0));
				list.Add(new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "princess":
			case "princess rotated 45":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0));
				list.Add(new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				break;
			case "half moon":
				list.Add(new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0));
				list.Add(new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "marquise":
			case "marquise rotated 90":
				list.Add(new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "pear":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "heart":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "triangle":
			case "trillion curved":
			case "trillion straight":
			case "calf":
				list.Add(new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				break;
			}
			break;
		case ValueListItem.HeadStyleSide5:
			switch (shape.ToLower())
			{
			case "round":
			case "baguette straight":
			case "oval":
			case "oval rotated 90":
			case "emerald":
			case "emerald rotated 90":
			case "emerald square":
			case "emerald square rotated 45":
			case "radiant":
			case "radiant square":
			case "cushion":
			case "cushion rotated 90":
			case "cushion square":
			case "cushion square rotated 45":
			case "octagon":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "princess":
			case "princess rotated 45":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, -1));
				list.Add(new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				break;
			case "half moon":
				list.Add(new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(4, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(8, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				break;
			case "pear":
			case "heart":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(12, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "marquise":
				list.Add(new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(12, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, 0));
				break;
			case "marquise rotated 90":
				list.Add(new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0));
				list.Add(new HeadProngInfo(18, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, 0));
				break;
			case "triangle":
			case "trillion curved":
			case "trillion straight":
			case "calf":
				list.Add(new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(12, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 0));
				list.Add(new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				break;
			}
			break;
		case ValueListItem.HeadStyleSide6:
			switch (shape.ToLower())
			{
			case "round":
			case "baguette straight":
			case "oval":
			case "oval rotated 90":
			case "emerald":
			case "emerald rotated 90":
			case "emerald square":
			case "emerald square rotated 45":
			case "radiant":
			case "radiant square":
			case "cushion":
			case "cushion rotated 90":
			case "cushion square":
			case "cushion square rotated 45":
			case "octagon":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "princess":
			case "princess rotated 45":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1));
				list.Add(new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1));
				break;
			case "half moon":
				list.Add(new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(5, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(7, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "pear":
			case "heart":
				list.Add(new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(11, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(13, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "marquise":
				list.Add(new HeadProngInfo(1, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(11, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(13, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(23, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "marquise rotated 90":
				list.Add(new HeadProngInfo(5, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(7, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(17, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(19, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			case "triangle":
			case "trillion curved":
			case "trillion straight":
			case "calf":
				list.Add(new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(11, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				list.Add(new HeadProngInfo(13, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1));
				list.Add(new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1));
				break;
			}
			break;
		}
		return list;
	}
}
