using System;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

[Obsolete]
public static class CSHeadStyles1
{
	public static void GetHeadData1(string shape, int style, out int[] position, out int[] type, out int[] mirror)
	{
		position = null;
		type = null;
		mirror = null;
		string text = shape.ToLower();
		if (text == null)
		{
			return;
		}
		switch (text)
		{
		case "baguette straight":
			switch (style)
			{
			case 0:
				position = new int[2] { 0, 8 };
				type = new int[2];
				mirror = new int[2];
				break;
			case 1:
				position = new int[2] { 4, 12 };
				type = new int[2];
				mirror = new int[2];
				break;
			case 2:
				position = new int[4] { 2, 6, 10, 14 };
				type = new int[4] { 1, 1, 1, 1 };
				mirror = new int[4] { 1, -2, 2, -1 };
				break;
			case 3:
				position = new int[4] { 0, 4, 8, 12 };
				type = new int[4];
				mirror = new int[4] { 0, 1, 0, -1 };
				break;
			case 4:
				position = new int[8] { 1, 3, 5, 7, 9, 11, 13, 15 };
				type = new int[8];
				mirror = new int[8] { 1, -2, 3, -4, 4, -3, 2, -1 };
				break;
			}
			break;
		case "calf":
			switch (style)
			{
			case 0:
				position = new int[3] { 0, 6, 10 };
				type = new int[3];
				mirror = new int[3] { 0, 1, -1 };
				break;
			case 1:
				position = new int[3] { 3, 8, 13 };
				type = new int[3];
				mirror = new int[3];
				break;
			case 2:
				position = new int[6] { 0, 3, 6, 8, 10, 13 };
				type = new int[6];
				mirror = new int[6] { 0, 1, 2, 0, -2, -1 };
				break;
			case 3:
				position = new int[6] { 2, 4, 7, 9, 12, 14 };
				type = new int[6] { 1, 1, 1, 1, 1, 1 };
				mirror = new int[6] { 1, -1, 2, -1, 3, -3 };
				break;
			case 4:
				position = new int[7] { 1, 3, 5, 8, 11, 13, 15 };
				type = new int[7];
				mirror = new int[7] { 1, -1, 2, 3, 4, 5, -5 };
				break;
			}
			break;
		case "cushion":
		case "cushion square":
			switch (style)
			{
			case 0:
				position = new int[2] { 0, 8 };
				type = new int[2];
				mirror = new int[2];
				break;
			case 1:
				position = new int[2] { 4, 12 };
				type = new int[2];
				mirror = new int[2];
				break;
			case 2:
				position = new int[3] { 2, 8, 14 };
				type = new int[3];
				mirror = new int[3] { 1, 0, -1 };
				break;
			case 3:
				position = new int[4] { 0, 4, 8, 12 };
				type = new int[4];
				mirror = new int[4] { 0, 1, 0, -1 };
				break;
			case 4:
				position = new int[6] { 1, 4, 7, 9, 12, 15 };
				type = new int[6];
				mirror = new int[6] { 1, 2, 3, -3, -2, -1 };
				break;
			case 5:
				position = new int[6] { 0, 3, 5, 8, 11, 13 };
				type = new int[6];
				mirror = new int[6] { 0, 1, -2, 0, 2, -1 };
				break;
			}
			break;
		case "emerald":
		case "emerald square":
		case "radiant":
		case "princess":
			switch (style)
			{
			case 0:
				position = new int[2] { 0, 8 };
				type = new int[2];
				mirror = new int[2];
				break;
			case 1:
				position = new int[2] { 4, 12 };
				type = new int[2];
				mirror = new int[2];
				break;
			case 2:
				position = new int[4] { 2, 6, 10, 14 };
				type = new int[4];
				mirror = new int[4] { 1, -2, 2, -1 };
				break;
			case 3:
				position = new int[4] { 0, 4, 8, 12 };
				type = new int[4];
				mirror = new int[4] { 0, 1, 0, -1 };
				break;
			case 4:
				position = new int[8] { 1, 3, 5, 7, 9, 11, 13, 15 };
				type = new int[8] { 1, 1, 1, 1, 1, 1, 1, 1 };
				mirror = new int[8] { 1, -1, 2, -2, 3, -3, 4, -4 };
				break;
			}
			break;
		case "half moon":
			if (style == 0)
			{
				position = new int[3] { 0, 1, 2 };
				type = new int[3] { 1, 0, 1 };
				mirror = new int[3];
			}
			break;
		case "heart":
			switch (style)
			{
			case 0:
				position = new int[2] { 0, 7 };
				type = new int[2] { 1, 1 };
				mirror = new int[2];
				break;
			case 1:
				position = new int[2] { 3, 11 };
				type = new int[2];
				mirror = new int[2] { 1, -1 };
				break;
			case 2:
				position = new int[3] { 2, 7, 12 };
				type = new int[3] { 0, 1, 0 };
				mirror = new int[3] { 1, 0, -1 };
				break;
			case 3:
				position = new int[4] { 2, 6, 8, 12 };
				type = new int[4];
				mirror = new int[4] { 1, 2, -1, -2 };
				break;
			case 4:
				position = new int[4] { 0, 3, 7, 11 };
				type = new int[4] { 1, 0, 1, 0 };
				mirror = new int[4] { 0, 1, 0, -1 };
				break;
			case 5:
				position = new int[5] { 2, 4, 7, 10, 12 };
				type = new int[5];
				mirror = new int[5] { 1, -2, 0, 2, -1 };
				break;
			}
			break;
		case "marquise":
			switch (style)
			{
			case 0:
				position = new int[2] { 0, 6 };
				type = new int[2] { 1, 1 };
				mirror = new int[2];
				break;
			case 1:
				position = new int[2] { 3, 9 };
				type = new int[2];
				mirror = new int[2] { 1, -1 };
				break;
			case 2:
				position = new int[4] { 1, 5, 7, 11 };
				type = new int[4];
				mirror = new int[4] { 1, -2, 2, -1 };
				break;
			case 3:
				position = new int[4] { 0, 3, 6, 9 };
				type = new int[4] { 1, 1, 1, 1 };
				mirror = new int[4] { 0, 1, 0, -1 };
				break;
			case 4:
				position = new int[6] { 0, 2, 4, 6, 8, 10 };
				type = new int[6] { 1, 0, 0, 1, 0, 0 };
				mirror = new int[6] { 0, 1, -2, 0, 2, -1 };
				break;
			case 5:
				position = new int[6] { 0, 1, 5, 6, 7, 11 };
				type = new int[6] { 1, 0, 0, 1, 0, 0 };
				mirror = new int[6] { 0, 1, -2, 0, 2, -1 };
				break;
			}
			break;
		case "pear":
			switch (style)
			{
			case 0:
				position = new int[2] { 0, 6 };
				type = new int[2] { 0, 1 };
				mirror = new int[2];
				break;
			case 1:
				position = new int[2] { 3, 9 };
				type = new int[2];
				mirror = new int[2] { 1, -1 };
				break;
			case 2:
				position = new int[3] { 2, 6, 10 };
				type = new int[3] { 0, 1, 0 };
				mirror = new int[3] { 1, 0, -1 };
				break;
			case 3:
				position = new int[4] { 0, 3, 6, 9 };
				type = new int[4] { 0, 0, 1, 0 };
				mirror = new int[4] { 0, 1, 0, -1 };
				break;
			case 4:
				position = new int[5] { 1, 4, 6, 8, 11 };
				type = new int[5] { 0, 0, 1, 0, 0 };
				mirror = new int[5] { 1, -2, 0, 2, -1 };
				break;
			case 5:
				position = new int[6] { 0, 2, 5, 6, 7, 10 };
				type = new int[6] { 0, 0, 0, 1, 0, 0 };
				mirror = new int[6] { 0, 1, -2, 0, 2, -1 };
				break;
			}
			break;
		case "bullet":
		case "round":
		case "oval":
			switch (style)
			{
			case 0:
				position = new int[2] { 0, 6 };
				type = new int[2];
				mirror = new int[2];
				break;
			case 1:
				position = new int[2] { 3, 9 };
				type = new int[2];
				mirror = new int[2] { 1, -1 };
				break;
			case 2:
				position = new int[3] { 2, 6, 10 };
				type = new int[3];
				mirror = new int[3] { 1, 0, -1 };
				break;
			case 3:
				position = new int[3] { 0, 4, 8 };
				type = new int[3];
				mirror = new int[3] { 0, 1, -1 };
				break;
			case 4:
				position = new int[4] { 0, 3, 6, 9 };
				type = new int[4];
				mirror = new int[4] { 0, 1, 0, -1 };
				break;
			case 5:
				position = new int[4] { 1, 5, 7, 11 };
				type = new int[4];
				mirror = new int[4] { 1, -2, 2, -1 };
				break;
			case 6:
				position = new int[6] { 0, 2, 4, 6, 8, 10 };
				type = new int[6];
				mirror = new int[6] { 0, 1, -2, 0, 2, -1 };
				break;
			case 7:
				position = new int[8] { 0, 1, 3, 5, 6, 7, 9, 11 };
				type = new int[8];
				mirror = new int[8] { 0, 1, 0, -2, 0, 2, 0, -1 };
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
				position = new int[2] { 0, 6 };
				type = new int[2] { 0, 1 };
				mirror = new int[2];
				break;
			case 1:
				position = new int[3] { 2, 6, 10 };
				type = new int[3] { 1, 1, 1 };
				mirror = new int[3];
				break;
			case 2:
				position = new int[6] { 0, 2, 4, 6, 8, 10 };
				type = new int[6] { 0, 1, 0, 1, 0, 1 };
				mirror = new int[6];
				break;
			case 3:
				position = new int[6] { 1, 3, 5, 7, 9, 11 };
				type = new int[6];
				mirror = new int[6] { 1, -1, 2, -1, 3, -3 };
				break;
			}
			break;
		}
	}
}
