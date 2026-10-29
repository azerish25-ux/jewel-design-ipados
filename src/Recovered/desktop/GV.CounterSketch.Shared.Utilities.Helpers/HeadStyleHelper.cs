using System.Collections.Generic;

namespace GV.CounterSketch.Shared.Utilities.Helpers;

public static class HeadStyleHelper
{
	public static IEnumerable<HeadStyle> GetHeadStyles(string shape)
	{
		string text = shape.ToLower();
		if ((text.StartsWith("baguette") && text.Contains("_")) || text.StartsWith("baguettetapered"))
		{
			text = "baguette tapered";
		}
		if (text.StartsWith("emerald") && text.Contains("_"))
		{
			text = "emerald";
		}
		return text switch
		{
			"baguette straight" => new HeadStyle[10]
			{
				HeadStyle.HeadStyleBaguetteStraight1,
				HeadStyle.HeadStyleBaguetteStraight2,
				HeadStyle.HeadStyleBaguetteStraight3,
				HeadStyle.HeadStyleBaguetteStraight4,
				HeadStyle.HeadStyleBaguetteStraight5,
				HeadStyle.HeadStyleBaguetteStraight6,
				HeadStyle.HeadStyleBaguetteStraight7,
				HeadStyle.HeadStyleBaguetteStraight8,
				HeadStyle.HeadStyleBaguetteStraight9,
				HeadStyle.HeadStyleBaguetteStraight10
			}, 
			"baguette tapered" => new HeadStyle[10]
			{
				HeadStyle.HeadStyleBaguetteTapered1,
				HeadStyle.HeadStyleBaguetteTapered2,
				HeadStyle.HeadStyleBaguetteTapered3,
				HeadStyle.HeadStyleBaguetteTapered4,
				HeadStyle.HeadStyleBaguetteTapered5,
				HeadStyle.HeadStyleBaguetteTapered6,
				HeadStyle.HeadStyleBaguetteTapered7,
				HeadStyle.HeadStyleBaguetteTapered8,
				HeadStyle.HeadStyleBaguetteTapered9,
				HeadStyle.HeadStyleBaguetteTapered10
			}, 
			"bullet" => new HeadStyle[10]
			{
				HeadStyle.HeadStyleBullet1,
				HeadStyle.HeadStyleBullet2,
				HeadStyle.HeadStyleBullet3,
				HeadStyle.HeadStyleBullet4,
				HeadStyle.HeadStyleBullet5,
				HeadStyle.HeadStyleBullet6,
				HeadStyle.HeadStyleBullet7,
				HeadStyle.HeadStyleBullet8,
				HeadStyle.HeadStyleBullet9,
				HeadStyle.HeadStyleBullet10
			}, 
			"calf" => new HeadStyle[9]
			{
				HeadStyle.HeadStyleCalf1,
				HeadStyle.HeadStyleCalf2,
				HeadStyle.HeadStyleCalf3,
				HeadStyle.HeadStyleCalf4,
				HeadStyle.HeadStyleCalf5,
				HeadStyle.HeadStyleCalf6,
				HeadStyle.HeadStyleCalf7,
				HeadStyle.HeadStyleCalf8,
				HeadStyle.HeadStyleCalf9
			}, 
			"coffin" => new HeadStyle[9]
			{
				HeadStyle.HeadStyleCoffin1,
				HeadStyle.HeadStyleCoffin2,
				HeadStyle.HeadStyleCoffin3,
				HeadStyle.HeadStyleCoffin4,
				HeadStyle.HeadStyleCoffin5,
				HeadStyle.HeadStyleCoffin6,
				HeadStyle.HeadStyleCoffin7,
				HeadStyle.HeadStyleCoffin8,
				HeadStyle.HeadStyleCoffin9
			}, 
			"cushion" => new HeadStyle[10]
			{
				HeadStyle.HeadStyleCushion1,
				HeadStyle.HeadStyleCushion2,
				HeadStyle.HeadStyleCushion3,
				HeadStyle.HeadStyleCushion4,
				HeadStyle.HeadStyleCushion5,
				HeadStyle.HeadStyleCushion6,
				HeadStyle.HeadStyleCushion7,
				HeadStyle.HeadStyleCushion8,
				HeadStyle.HeadStyleCushion9,
				HeadStyle.HeadStyleCushion10
			}, 
			"cushion square" => new HeadStyle[9]
			{
				HeadStyle.HeadStyleCushionSquare1,
				HeadStyle.HeadStyleCushionSquare2,
				HeadStyle.HeadStyleCushionSquare3,
				HeadStyle.HeadStyleCushionSquare4,
				HeadStyle.HeadStyleCushionSquare5,
				HeadStyle.HeadStyleCushionSquare6,
				HeadStyle.HeadStyleCushionSquare7,
				HeadStyle.HeadStyleCushionSquare8,
				HeadStyle.HeadStyleCushionSquare9
			}, 
			"emerald" => new HeadStyle[9]
			{
				HeadStyle.HeadStyleEmerald1,
				HeadStyle.HeadStyleEmerald2,
				HeadStyle.HeadStyleEmerald3,
				HeadStyle.HeadStyleEmerald4,
				HeadStyle.HeadStyleEmerald5,
				HeadStyle.HeadStyleEmerald6,
				HeadStyle.HeadStyleEmerald7,
				HeadStyle.HeadStyleEmerald8,
				HeadStyle.HeadStyleEmerald9
			}, 
			"emerald square" => new HeadStyle[9]
			{
				HeadStyle.HeadStyleEmeraldSquare1,
				HeadStyle.HeadStyleEmeraldSquare2,
				HeadStyle.HeadStyleEmeraldSquare3,
				HeadStyle.HeadStyleEmeraldSquare4,
				HeadStyle.HeadStyleEmeraldSquare5,
				HeadStyle.HeadStyleEmeraldSquare6,
				HeadStyle.HeadStyleEmeraldSquare7,
				HeadStyle.HeadStyleEmeraldSquare8,
				HeadStyle.HeadStyleEmeraldSquare9
			}, 
			"half moon" => new HeadStyle[10]
			{
				HeadStyle.HeadStyleHalfMoon1,
				HeadStyle.HeadStyleHalfMoon2,
				HeadStyle.HeadStyleHalfMoon3,
				HeadStyle.HeadStyleHalfMoon4,
				HeadStyle.HeadStyleHalfMoon5,
				HeadStyle.HeadStyleHalfMoon6,
				HeadStyle.HeadStyleHalfMoon7,
				HeadStyle.HeadStyleHalfMoon8,
				HeadStyle.HeadStyleHalfMoon9,
				HeadStyle.HeadStyleHalfMoon10
			}, 
			"heart" => new HeadStyle[8]
			{
				HeadStyle.HeadStyleHeart1,
				HeadStyle.HeadStyleHeart2,
				HeadStyle.HeadStyleHeart3,
				HeadStyle.HeadStyleHeart4,
				HeadStyle.HeadStyleHeart5,
				HeadStyle.HeadStyleHeart6,
				HeadStyle.HeadStyleHeart7,
				HeadStyle.HeadStyleHeart8
			}, 
			"hexagon" => new HeadStyle[8]
			{
				HeadStyle.HeadStyleHexagon1,
				HeadStyle.HeadStyleHexagon2,
				HeadStyle.HeadStyleHexagon3,
				HeadStyle.HeadStyleHexagon4,
				HeadStyle.HeadStyleHexagon5,
				HeadStyle.HeadStyleHexagon6,
				HeadStyle.HeadStyleHexagon7,
				HeadStyle.HeadStyleHexagon8
			}, 
			"marquise" => new HeadStyle[8]
			{
				HeadStyle.HeadStyleMarquise1,
				HeadStyle.HeadStyleMarquise2,
				HeadStyle.HeadStyleMarquise3,
				HeadStyle.HeadStyleMarquise4,
				HeadStyle.HeadStyleMarquise5,
				HeadStyle.HeadStyleMarquise6,
				HeadStyle.HeadStyleMarquise7,
				HeadStyle.HeadStyleMarquise8
			}, 
			"octagon" => new HeadStyle[9]
			{
				HeadStyle.HeadStyleOctagon1,
				HeadStyle.HeadStyleOctagon2,
				HeadStyle.HeadStyleOctagon3,
				HeadStyle.HeadStyleOctagon4,
				HeadStyle.HeadStyleOctagon5,
				HeadStyle.HeadStyleOctagon6,
				HeadStyle.HeadStyleOctagon7,
				HeadStyle.HeadStyleOctagon8,
				HeadStyle.HeadStyleOctagon9
			}, 
			"oval" => new HeadStyle[10]
			{
				HeadStyle.HeadStyleOval1,
				HeadStyle.HeadStyleOval2,
				HeadStyle.HeadStyleOval3,
				HeadStyle.HeadStyleOval4,
				HeadStyle.HeadStyleOval5,
				HeadStyle.HeadStyleOval6,
				HeadStyle.HeadStyleOval7,
				HeadStyle.HeadStyleOval8,
				HeadStyle.HeadStyleOval9,
				HeadStyle.HeadStyleOval10
			}, 
			"pear" => new HeadStyle[9]
			{
				HeadStyle.HeadStylePear1,
				HeadStyle.HeadStylePear2,
				HeadStyle.HeadStylePear3,
				HeadStyle.HeadStylePear4,
				HeadStyle.HeadStylePear5,
				HeadStyle.HeadStylePear6,
				HeadStyle.HeadStylePear7,
				HeadStyle.HeadStylePear8,
				HeadStyle.HeadStylePear9
			}, 
			"princess" => new HeadStyle[9]
			{
				HeadStyle.HeadStylePrincess1,
				HeadStyle.HeadStylePrincess2,
				HeadStyle.HeadStylePrincess3,
				HeadStyle.HeadStylePrincess4,
				HeadStyle.HeadStylePrincess5,
				HeadStyle.HeadStylePrincess6,
				HeadStyle.HeadStylePrincess7,
				HeadStyle.HeadStylePrincess8,
				HeadStyle.HeadStylePrincess9
			}, 
			"radiant" => new HeadStyle[10]
			{
				HeadStyle.HeadStyleRadiant1,
				HeadStyle.HeadStyleRadiant2,
				HeadStyle.HeadStyleRadiant3,
				HeadStyle.HeadStyleRadiant4,
				HeadStyle.HeadStyleRadiant5,
				HeadStyle.HeadStyleRadiant6,
				HeadStyle.HeadStyleRadiant7,
				HeadStyle.HeadStyleRadiant8,
				HeadStyle.HeadStyleRadiant9,
				HeadStyle.HeadStyleRadiant10
			}, 
			"radiant square" => new HeadStyle[10]
			{
				HeadStyle.HeadStyleRadiantSquare1,
				HeadStyle.HeadStyleRadiantSquare2,
				HeadStyle.HeadStyleRadiantSquare3,
				HeadStyle.HeadStyleRadiantSquare4,
				HeadStyle.HeadStyleRadiantSquare5,
				HeadStyle.HeadStyleRadiantSquare6,
				HeadStyle.HeadStyleRadiantSquare7,
				HeadStyle.HeadStyleRadiantSquare8,
				HeadStyle.HeadStyleRadiantSquare9,
				HeadStyle.HeadStyleRadiantSquare10
			}, 
			"round" => new HeadStyle[10]
			{
				HeadStyle.HeadStyleRound1,
				HeadStyle.HeadStyleRound2,
				HeadStyle.HeadStyleRound3,
				HeadStyle.HeadStyleRound4,
				HeadStyle.HeadStyleRound5,
				HeadStyle.HeadStyleRound6,
				HeadStyle.HeadStyleRound7,
				HeadStyle.HeadStyleRound8,
				HeadStyle.HeadStyleRound9,
				HeadStyle.HeadStyleRound10
			}, 
			"square" => new HeadStyle[9]
			{
				HeadStyle.HeadStyleSquare1,
				HeadStyle.HeadStyleSquare2,
				HeadStyle.HeadStyleSquare3,
				HeadStyle.HeadStyleSquare4,
				HeadStyle.HeadStyleSquare5,
				HeadStyle.HeadStyleSquare6,
				HeadStyle.HeadStyleSquare7,
				HeadStyle.HeadStyleSquare8,
				HeadStyle.HeadStyleSquare9
			}, 
			"triangle" => new HeadStyle[9]
			{
				HeadStyle.HeadStyleTriangle1,
				HeadStyle.HeadStyleTriangle2,
				HeadStyle.HeadStyleTriangle3,
				HeadStyle.HeadStyleTriangle4,
				HeadStyle.HeadStyleTriangle5,
				HeadStyle.HeadStyleTriangle6,
				HeadStyle.HeadStyleTriangle7,
				HeadStyle.HeadStyleTriangle8,
				HeadStyle.HeadStyleTriangle9
			}, 
			"trillion" => new HeadStyle[9]
			{
				HeadStyle.HeadStyleTrillion1,
				HeadStyle.HeadStyleTrillion2,
				HeadStyle.HeadStyleTrillion3,
				HeadStyle.HeadStyleTrillion4,
				HeadStyle.HeadStyleTrillion5,
				HeadStyle.HeadStyleTrillion6,
				HeadStyle.HeadStyleTrillion7,
				HeadStyle.HeadStyleTrillion8,
				HeadStyle.HeadStyleTrillion9
			}, 
			"trillion curved" => new HeadStyle[9]
			{
				HeadStyle.HeadStyleTrillionCurved1,
				HeadStyle.HeadStyleTrillionCurved2,
				HeadStyle.HeadStyleTrillionCurved3,
				HeadStyle.HeadStyleTrillionCurved4,
				HeadStyle.HeadStyleTrillionCurved5,
				HeadStyle.HeadStyleTrillionCurved6,
				HeadStyle.HeadStyleTrillionCurved7,
				HeadStyle.HeadStyleTrillionCurved8,
				HeadStyle.HeadStyleTrillionCurved9
			}, 
			"trillion straight" => new HeadStyle[9]
			{
				HeadStyle.HeadStyleTrillionStraight1,
				HeadStyle.HeadStyleTrillionStraight2,
				HeadStyle.HeadStyleTrillionStraight3,
				HeadStyle.HeadStyleTrillionStraight4,
				HeadStyle.HeadStyleTrillionStraight5,
				HeadStyle.HeadStyleTrillionStraight6,
				HeadStyle.HeadStyleTrillionStraight7,
				HeadStyle.HeadStyleTrillionStraight8,
				HeadStyle.HeadStyleTrillionStraight9
			}, 
			_ => new List<HeadStyle>(), 
		};
	}
}
