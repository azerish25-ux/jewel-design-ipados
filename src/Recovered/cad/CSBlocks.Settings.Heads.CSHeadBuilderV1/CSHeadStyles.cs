using System.Collections.Generic;
using GV.CounterSketch.Shared;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1;

public static class CSHeadStyles
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

	public static IEnumerable<HeadProngInfo> GetHeadProngInfo(HeadStyle whichHeadStyle)
	{
		return whichHeadStyle switch
		{
			HeadStyle.HeadStyleBaguetteStraight1 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleBaguetteStraight2 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleBaguetteStraight3 => new HeadProngInfo[4]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(18, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleBaguetteStraight4 => new HeadProngInfo[8]
			{
				new HeadProngInfo(2, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(22, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleBaguetteStraight5 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, 1),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleBaguetteStraight6 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleBaguetteStraight7 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleBaguetteStraight8 => new HeadProngInfo[5]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleBaguetteStraight9 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleBaguetteStraight10 => new HeadProngInfo[6]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleCalf1 => new HeadProngInfo[3]
			{
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleCalf2 => new HeadProngInfo[3]
			{
				new HeadProngInfo(4, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 0),
				new HeadProngInfo(20, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleCalf3 => new HeadProngInfo[6]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleCalf4 => new HeadProngInfo[3]
			{
				new HeadProngInfo(4, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 0),
				new HeadProngInfo(20, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleCalf5 => new HeadProngInfo[6]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(5, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(11, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(13, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(19, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleCalf6 => new HeadProngInfo[3]
			{
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 0),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleCalf7 => new HeadProngInfo[5]
			{
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleCalf8 => new HeadProngInfo[3]
			{
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 0),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleCalf9 => new HeadProngInfo[4]
			{
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(11, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(13, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleCushion1 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleCushion2 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleCushion3 => new HeadProngInfo[4]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(18, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleCushion4 => new HeadProngInfo[6]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleCushion5 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleCushion6 => new HeadProngInfo[8]
			{
				new HeadProngInfo(2, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(22, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleCushion7 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleCushion8 => new HeadProngInfo[5]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleCushion9 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleCushion10 => new HeadProngInfo[6]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleCushionSquare1 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleCushionSquare2 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleCushionSquare3 => new HeadProngInfo[4]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(18, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleCushionSquare4 => new HeadProngInfo[8]
			{
				new HeadProngInfo(2, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(22, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleCushionSquare5 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleCushionSquare6 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleCushionSquare7 => new HeadProngInfo[5]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleCushionSquare8 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleCushionSquare9 => new HeadProngInfo[6]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleEmerald1 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleEmerald2 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleEmerald3 => new HeadProngInfo[4]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(18, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleEmerald4 => new HeadProngInfo[8]
			{
				new HeadProngInfo(2, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(22, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleEmerald5 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleEmerald6 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleEmerald7 => new HeadProngInfo[5]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleEmerald8 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleEmerald9 => new HeadProngInfo[6]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleEmeraldSquare1 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleEmeraldSquare2 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleEmeraldSquare3 => new HeadProngInfo[4]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(18, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleEmeraldSquare4 => new HeadProngInfo[8]
			{
				new HeadProngInfo(2, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(22, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleEmeraldSquare5 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleEmeraldSquare6 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleEmeraldSquare7 => new HeadProngInfo[5]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleEmeraldSquare8 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleEmeraldSquare9 => new HeadProngInfo[6]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleHalfMoon1 => new HeadProngInfo[4]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleHalfMoon2 => new HeadProngInfo[4]
			{
				new HeadProngInfo(0, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(4, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1),
				new HeadProngInfo(8, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleHalfMoon3 => new HeadProngInfo[3]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleHalfMoon4 => new HeadProngInfo[6]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1),
				new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(18, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleHalfMoon5 => new HeadProngInfo[4]
			{
				new HeadProngInfo(0, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(4, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1),
				new HeadProngInfo(8, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleHalfMoon6 => new HeadProngInfo[8]
			{
				new HeadProngInfo(23, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(1, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(5, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(7, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(11, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(13, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleHalfMoon7 => new HeadProngInfo[4]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(4, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1),
				new HeadProngInfo(8, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleHalfMoon8 => new HeadProngInfo[5]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleHalfMoon9 => new HeadProngInfo[4]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(4, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1),
				new HeadProngInfo(8, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleHalfMoon10 => new HeadProngInfo[6]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(5, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(7, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleHeart1 => new HeadProngInfo[3]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleHeart2 => new HeadProngInfo[3]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 0),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleHeart3 => new HeadProngInfo[5]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleHeart4 => new HeadProngInfo[6]
			{
				new HeadProngInfo(2, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(11, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(13, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(22, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleHeart5 => new HeadProngInfo[3]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, 0),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleHeart6 => new HeadProngInfo[3]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 0),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleHeart7 => new HeadProngInfo[3]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, 0),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleHeart8 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(11, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(13, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleMarquise1 => new HeadProngInfo[2]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0)
			}, 
			HeadStyle.HeadStyleMarquise2 => new HeadProngInfo[2]
			{
				new HeadProngInfo(0, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 0),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 0)
			}, 
			HeadStyle.HeadStyleMarquise3 => new HeadProngInfo[4]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0),
				new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0),
				new HeadProngInfo(18, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleMarquise4 => new HeadProngInfo[6]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleMarquise5 => new HeadProngInfo[2]
			{
				new HeadProngInfo(0, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, 0),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, 0)
			}, 
			HeadStyle.HeadStyleMarquise6 => new HeadProngInfo[4]
			{
				new HeadProngInfo(1, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(11, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(13, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(23, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleMarquise7 => new HeadProngInfo[2]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 0)
			}, 
			HeadStyle.HeadStyleMarquise8 => new HeadProngInfo[2]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, 0)
			}, 
			HeadStyle.HeadStyleOctagon1 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleOctagon2 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleOctagon3 => new HeadProngInfo[4]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(18, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleOctagon4 => new HeadProngInfo[8]
			{
				new HeadProngInfo(2, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(22, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleOctagon5 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleOctagon6 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleOctagon7 => new HeadProngInfo[5]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleOctagon8 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleOctagon9 => new HeadProngInfo[6]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleOval1 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleOval2 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleOval3 => new HeadProngInfo[4]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(18, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleOval4 => new HeadProngInfo[6]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleOval5 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleOval6 => new HeadProngInfo[8]
			{
				new HeadProngInfo(2, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(22, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleOval7 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleOval8 => new HeadProngInfo[5]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleOval9 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleOval10 => new HeadProngInfo[6]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStylePear1 => new HeadProngInfo[3]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStylePear2 => new HeadProngInfo[3]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 0),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStylePear3 => new HeadProngInfo[3]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 0),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStylePear4 => new HeadProngInfo[5]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStylePear5 => new HeadProngInfo[3]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, 0),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStylePear6 => new HeadProngInfo[6]
			{
				new HeadProngInfo(2, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(11, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(13, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(22, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStylePear7 => new HeadProngInfo[5]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStylePear8 => new HeadProngInfo[3]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, 0),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStylePear9 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(11, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(13, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStylePrincess1 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStylePrincess2 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 1),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStylePrincess3 => new HeadProngInfo[4]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(18, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStylePrincess4 => new HeadProngInfo[8]
			{
				new HeadProngInfo(2, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(22, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStylePrincess5 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, 1),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStylePrincess6 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStylePrincess7 => new HeadProngInfo[5]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStylePrincess8 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Straight, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStylePrincess9 => new HeadProngInfo[6]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleRadiant1 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleRadiant2 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRadiant3 => new HeadProngInfo[4]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(18, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRadiant4 => new HeadProngInfo[8]
			{
				new HeadProngInfo(2, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(22, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRadiant5 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRadiant6 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRadiant7 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRadiant8 => new HeadProngInfo[5]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRadiant9 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRadiant10 => new HeadProngInfo[6]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRadiantSquare1 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleRadiantSquare2 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRadiantSquare3 => new HeadProngInfo[4]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(18, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRadiantSquare4 => new HeadProngInfo[8]
			{
				new HeadProngInfo(2, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(22, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRadiantSquare5 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRadiantSquare6 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRadiantSquare7 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRadiantSquare8 => new HeadProngInfo[5]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRadiantSquare9 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRadiantSquare10 => new HeadProngInfo[6]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRound1 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRound2 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRound3 => new HeadProngInfo[4]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(6, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(18, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRound4 => new HeadProngInfo[6]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRound5 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRound6 => new HeadProngInfo[8]
			{
				new HeadProngInfo(2, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(22, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRound7 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRound8 => new HeadProngInfo[5]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 0),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRound9 => new HeadProngInfo[4]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(9, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1),
				new HeadProngInfo(15, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleRound10 => new HeadProngInfo[6]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(10, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(14, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleTriangle1 => new HeadProngInfo[3]
			{
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleTriangle2 => new HeadProngInfo[3]
			{
				new HeadProngInfo(4, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 0),
				new HeadProngInfo(20, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleTriangle3 => new HeadProngInfo[6]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleTriangle4 => new HeadProngInfo[3]
			{
				new HeadProngInfo(4, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 0),
				new HeadProngInfo(20, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleTriangle5 => new HeadProngInfo[6]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(5, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(11, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(13, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(19, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleTriangle6 => new HeadProngInfo[3]
			{
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 0),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleTriangle7 => new HeadProngInfo[5]
			{
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleTriangle8 => new HeadProngInfo[3]
			{
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 0),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleTriangle9 => new HeadProngInfo[4]
			{
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(11, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(13, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleTrillionCurved1 => new HeadProngInfo[3]
			{
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleTrillionCurved2 => new HeadProngInfo[3]
			{
				new HeadProngInfo(4, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 0),
				new HeadProngInfo(20, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleTrillionCurved3 => new HeadProngInfo[6]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleTrillionCurved4 => new HeadProngInfo[3]
			{
				new HeadProngInfo(4, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 0),
				new HeadProngInfo(20, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleTrillionCurved5 => new HeadProngInfo[6]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(5, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(11, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(13, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(19, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleTrillionCurved6 => new HeadProngInfo[3]
			{
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 0),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleTrillionCurved7 => new HeadProngInfo[5]
			{
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleTrillionCurved8 => new HeadProngInfo[3]
			{
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 0),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleTrillionCurved9 => new HeadProngInfo[4]
			{
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(11, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(13, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleTrillionStraight1 => new HeadProngInfo[3]
			{
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleTrillionStraight2 => new HeadProngInfo[3]
			{
				new HeadProngInfo(4, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, 0),
				new HeadProngInfo(20, 0.25, EHeadProngArrangment.Double, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleTrillionStraight3 => new HeadProngInfo[6]
			{
				new HeadProngInfo(0, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleTrillionStraight4 => new HeadProngInfo[3]
			{
				new HeadProngInfo(4, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 0),
				new HeadProngInfo(20, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleTrillionStraight5 => new HeadProngInfo[6]
			{
				new HeadProngInfo(3, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(5, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(11, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(13, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(19, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(21, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			HeadStyle.HeadStyleTrillionStraight6 => new HeadProngInfo[3]
			{
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Double, EHeadProngType.Straight, 0),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleTrillionStraight7 => new HeadProngInfo[5]
			{
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(8, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(12, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 0),
				new HeadProngInfo(16, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleTrillionStraight8 => new HeadProngInfo[3]
			{
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, 1),
				new HeadProngInfo(12, 0.25, EHeadProngArrangment.Triple, EHeadProngType.Round, 0),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Straight, -1)
			}, 
			HeadStyle.HeadStyleTrillionStraight9 => new HeadProngInfo[4]
			{
				new HeadProngInfo(4, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(11, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1),
				new HeadProngInfo(13, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, 1),
				new HeadProngInfo(20, 0.0, EHeadProngArrangment.Single, EHeadProngType.Round, -1)
			}, 
			_ => new List<HeadProngInfo>(), 
		};
	}
}
