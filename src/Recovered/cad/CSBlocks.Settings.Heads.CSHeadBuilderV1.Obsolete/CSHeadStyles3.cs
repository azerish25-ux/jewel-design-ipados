using System;
using System.Collections.Generic;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

[Obsolete]
public static class CSHeadStyles3
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
		public double Overlap;

		public int ProngParamIndex;

		public int Mirror;

		public EHeadProngArrangment Arrangment;

		public EHeadProngType Type;
	}

	public static List<HeadProngInfo> GetHeadStyle(string shape, int style)
	{
		List<HeadProngInfo> list = new List<HeadProngInfo>();
		switch (shape.ToLower())
		{
		case "baguette straight":
			switch (style)
			{
			case 0:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				break;
			case 1:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				break;
			case 2:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 9,
					Mirror = -2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 15,
					Mirror = 2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 3:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 4:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 2,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 4,
					Mirror = -2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 8,
					Mirror = 3,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 10,
					Mirror = -4,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 14,
					Mirror = 4,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 16,
					Mirror = -3,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 20,
					Mirror = 2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 22,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			}
			break;
		case "calf":
			switch (style)
			{
			case 0:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 8,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 16,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 1:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 4,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 20,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				break;
			case 2:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 4,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 8,
					Mirror = 2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 16,
					Mirror = -2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 20,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 3:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 5,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 11,
					Mirror = 2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 13,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 19,
					Mirror = 3,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -3,
					Type = EHeadProngType.Straight
				});
				break;
			case 4:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 2,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 4,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 3,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = 4,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 20,
					Mirror = 5,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 22,
					Mirror = -5,
					Type = EHeadProngType.Straight
				});
				break;
			}
			break;
		case "cushion":
		case "cushion rotated 90":
		case "cushion square":
		case "cushion square rotated 45":
			switch (style)
			{
			case 0:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				break;
			case 1:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				break;
			case 2:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 3:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 4:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 9,
					Mirror = -2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 15,
					Mirror = 2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 5:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 2,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 10,
					Mirror = 3,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 14,
					Mirror = -3,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = -2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 22,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 6:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 4,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 8,
					Mirror = -2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 16,
					Mirror = 2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 20,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			}
			break;
		case "emerald":
		case "emerald rotated 90":
		case "emerald square":
		case "emerald square rotated 45":
		case "octagon":
			switch (style)
			{
			case 0:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				break;
			case 1:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				break;
			case 2:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 9,
					Mirror = -2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 15,
					Mirror = 2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 3:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 4:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 2,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 4,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 8,
					Mirror = 2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 10,
					Mirror = -2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 14,
					Mirror = 3,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 16,
					Mirror = -3,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 20,
					Mirror = 4,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 22,
					Mirror = -4,
					Type = EHeadProngType.Straight
				});
				break;
			}
			break;
		case "princess":
			switch (style)
			{
			case 0:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				break;
			case 1:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				break;
			case 2:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 9,
					Mirror = -2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 15,
					Mirror = 2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 3:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 4:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 2,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 4,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 8,
					Mirror = 2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 10,
					Mirror = -2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 14,
					Mirror = 3,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 16,
					Mirror = -3,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 20,
					Mirror = 4,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 22,
					Mirror = -4,
					Type = EHeadProngType.Straight
				});
				break;
			}
			break;
		case "radiant":
		case "radiant square":
			switch (style)
			{
			case 0:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				break;
			case 1:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				break;
			case 2:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 9,
					Mirror = -2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 15,
					Mirror = 2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 3:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 4:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 2,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 4,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 8,
					Mirror = 2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 10,
					Mirror = -2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 14,
					Mirror = 3,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 16,
					Mirror = -3,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 20,
					Mirror = 4,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 22,
					Mirror = -4,
					Type = EHeadProngType.Straight
				});
				break;
			}
			break;
		case "half moon":
			if (style == 0)
			{
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 0,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
			}
			break;
		case "heart":
			switch (style)
			{
			case 0:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				break;
			case 1:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 2:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Type = EHeadProngType.Round
				});
				break;
			case 3:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 10,
					Mirror = 2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 14,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -2,
					Type = EHeadProngType.Straight
				});
				break;
			case 4:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 1,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = -1,
					Type = EHeadProngType.Round
				});
				break;
			case 5:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 8,
					Mirror = -2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 16,
					Mirror = 2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			}
			break;
		case "marquise":
		case "marquise rotated 90":
			switch (style)
			{
			case 0:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				break;
			case 1:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 2:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 9,
					Mirror = -2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 15,
					Mirror = 2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 3:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 4:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 4,
					Mirror = 1,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 8,
					Mirror = -2,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 16,
					Mirror = 2,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 20,
					Mirror = -1,
					Type = EHeadProngType.Round
				});
				break;
			case 5:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 9,
					Mirror = -2,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 15,
					Mirror = 2,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Type = EHeadProngType.Round
				});
				break;
			case 6:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 4,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 8,
					Mirror = -2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 16,
					Mirror = 2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 20,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 7:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 9,
					Mirror = -2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 15,
					Mirror = 2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			}
			break;
		case "pear":
			switch (style)
			{
			case 0:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				break;
			case 1:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 1,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = -1,
					Type = EHeadProngType.Round
				});
				break;
			case 2:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Type = EHeadProngType.Round
				});
				break;
			case 3:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 1,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = -1,
					Type = EHeadProngType.Round
				});
				break;
			case 4:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 2,
					Mirror = 1,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 10,
					Mirror = -2,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 14,
					Mirror = 2,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 22,
					Mirror = -1,
					Type = EHeadProngType.Round
				});
				break;
			case 5:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 11,
					Mirror = -2,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 13,
					Mirror = 2,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Type = EHeadProngType.Round
				});
				break;
			}
			break;
		case "round":
		case "oval":
		case "oval rotated 90":
			switch (style)
			{
			case 0:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				break;
			case 1:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 2:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 4,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 20,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 3:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 8,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 16,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 4:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 5:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 9,
					Mirror = -2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 15,
					Mirror = 2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 6:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 4,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 8,
					Mirror = -2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 16,
					Mirror = 2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 20,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			case 7:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 9,
					Mirror = -2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 15,
					Mirror = 2,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Type = EHeadProngType.Straight
				});
				break;
			}
			break;
		case "triangle":
		case "triangle 2":
		case "trillion":
		case "trillion curved":
		case "trillion curved 2":
		case "trillion straight":
		case "trillion straight 2":
			switch (style)
			{
			case 0:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				break;
			case 1:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 4,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 20,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				break;
			case 2:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 4,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 8,
					Mirror = 0,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 16,
					Mirror = 0,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 20,
					Mirror = 0,
					Type = EHeadProngType.Straight
				});
				break;
			case 3:
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 5,
					Mirror = -1,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 11,
					Mirror = 2,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 13,
					Mirror = -1,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 19,
					Mirror = 3,
					Type = EHeadProngType.Round
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -3,
					Type = EHeadProngType.Round
				});
				break;
			}
			break;
		}
		return list;
	}

	public static List<HeadProngInfo> GetHeadStyleGameChanger(string shape, int style)
	{
		List<HeadProngInfo> list = new List<HeadProngInfo>();
		switch (style)
		{
		case 0:
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
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 9,
					Mirror = -2,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 15,
					Mirror = 2,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				break;
			case "pear":
			case "heart":
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				break;
			case "marquise":
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				break;
			case "marquise rotated 90":
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				break;
			}
			break;
		case 1:
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
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Double,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 9,
					Mirror = -2,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Double,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 15,
					Mirror = 2,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Double,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Double,
					Type = EHeadProngType.Straight
				});
				break;
			case "pear":
			case "heart":
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Double,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Double,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Double,
					Type = EHeadProngType.Straight
				});
				break;
			case "marquise":
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Double,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Double,
					Type = EHeadProngType.Straight
				});
				break;
			case "marquise rotated 90":
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 0,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Double,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = 0,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Double,
					Type = EHeadProngType.Straight
				});
				break;
			}
			break;
		case 2:
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
			case "marquise":
			case "marquise rotated 90":
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = -1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				break;
			case "pear":
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = -2,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Double,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				break;
			case "heart":
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 9,
					Mirror = -2,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 15,
					Mirror = 2,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				break;
			}
			break;
		case 3:
			switch (shape.ToLower())
			{
			case "round":
			case "oval":
			case "cushion rotated 90":
			case "marquise":
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 4,
					Mirror = 1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 8,
					Mirror = -2,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 16,
					Mirror = 2,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 20,
					Mirror = -1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				break;
			case "oval rotated 90":
			case "marquise rotated 90":
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 2,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 10,
					Mirror = -2,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 14,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = 2,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 22,
					Mirror = -1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				break;
			case "baguette straight":
			case "princess":
			case "princess rotated 45":
			case "emerald":
			case "emerald rotated 90":
			case "emerald square":
			case "emerald square rotated 45":
			case "radiant":
			case "radiant square":
			case "cushion square":
			case "cushion square rotated 45":
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 2,
					Mirror = 1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 4,
					Mirror = 1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 8,
					Mirror = -2,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 10,
					Mirror = -2,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 14,
					Mirror = 2,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 16,
					Mirror = 2,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 20,
					Mirror = -1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 22,
					Mirror = -1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				break;
			case "pear":
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 9,
					Mirror = -2,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 15,
					Mirror = 2,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				break;
			case "cushion":
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 9,
					Mirror = -2,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 15,
					Mirror = 2,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				break;
			case "heart":
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 2,
					Mirror = 1,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 4,
					Mirror = 1,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 11,
					Mirror = 0,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 13,
					Mirror = 0,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 20,
					Mirror = -1,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 22,
					Mirror = -1,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				break;
			}
			break;
		case 4:
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
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Triple,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 9,
					Mirror = -2,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Triple,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 15,
					Mirror = 2,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Triple,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Triple,
					Type = EHeadProngType.Straight
				});
				break;
			case "baguette straight":
			case "princess":
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Triple,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 9,
					Mirror = -2,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Triple,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 15,
					Mirror = 2,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Triple,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Triple,
					Type = EHeadProngType.Straight
				});
				break;
			case "pear":
			case "heart":
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 3,
					Mirror = 1,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Triple,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Triple,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 21,
					Mirror = -1,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Triple,
					Type = EHeadProngType.Straight
				});
				break;
			case "marquise":
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 0,
					Mirror = 0,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Triple,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 12,
					Mirror = 0,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Triple,
					Type = EHeadProngType.Straight
				});
				break;
			case "marquise rotated 90":
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 6,
					Mirror = 0,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Triple,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 18,
					Mirror = 0,
					Overlap = 0.25,
					Arrangment = EHeadProngArrangment.Triple,
					Type = EHeadProngType.Straight
				});
				break;
			}
			break;
		case 5:
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
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 2,
					Mirror = 1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 4,
					Mirror = 1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 8,
					Mirror = -2,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 10,
					Mirror = -2,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 14,
					Mirror = 2,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 16,
					Mirror = 2,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 20,
					Mirror = -1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 22,
					Mirror = -1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				break;
			case "pear":
			case "heart":
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 2,
					Mirror = 1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 4,
					Mirror = 1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 11,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 13,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 20,
					Mirror = -1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 22,
					Mirror = -1,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				break;
			case "marquise":
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 1,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 11,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 13,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 23,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				break;
			case "marquise rotated 90":
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 5,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 7,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 17,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				list.Add(new HeadProngInfo
				{
					ProngParamIndex = 19,
					Mirror = 0,
					Overlap = 0.0,
					Arrangment = EHeadProngArrangment.Single,
					Type = EHeadProngType.Straight
				});
				break;
			}
			break;
		}
		return list;
	}
}
