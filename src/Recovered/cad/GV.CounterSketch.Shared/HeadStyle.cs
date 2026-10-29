using System.ComponentModel;
using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared;

[DataContract]
public enum HeadStyle
{
	[EnumMember]
	[Description("Unknown")]
	Unknown,
	[EnumMember]
	[Description("Baguette Straight Style 1")]
	HeadStyleBaguetteStraight1,
	[EnumMember]
	[Description("Baguette Straight Style 2")]
	HeadStyleBaguetteStraight2,
	[EnumMember]
	[Description("Baguette Straight Style 3")]
	HeadStyleBaguetteStraight3,
	[EnumMember]
	[Description("Baguette Straight Style 4")]
	HeadStyleBaguetteStraight4,
	[EnumMember]
	[Description("Baguette Straight Style 5")]
	HeadStyleBaguetteStraight5,
	[EnumMember]
	[Description("Baguette Straight Style 6")]
	HeadStyleBaguetteStraight6,
	[EnumMember]
	[Description("Baguette Straight Style 7")]
	HeadStyleBaguetteStraight7,
	[EnumMember]
	[Description("Baguette Straight Style 8")]
	HeadStyleBaguetteStraight8,
	[EnumMember]
	[Description("Baguette Straight Style 9")]
	HeadStyleBaguetteStraight9,
	[EnumMember]
	[Description("Baguette Straight Style 10")]
	HeadStyleBaguetteStraight10,
	[EnumMember]
	[Description("Calf Style 1")]
	HeadStyleCalf1,
	[EnumMember]
	[Description("Calf Style 2")]
	HeadStyleCalf2,
	[EnumMember]
	[Description("Calf Style 3")]
	HeadStyleCalf3,
	[EnumMember]
	[Description("Calf Style 4")]
	HeadStyleCalf4,
	[EnumMember]
	[Description("Calf Style 5")]
	HeadStyleCalf5,
	[EnumMember]
	[Description("Calf Style 6")]
	HeadStyleCalf6,
	[EnumMember]
	[Description("Calf Style 7")]
	HeadStyleCalf7,
	[EnumMember]
	[Description("Calf Style 8")]
	HeadStyleCalf8,
	[EnumMember]
	[Description("Calf Style 9")]
	HeadStyleCalf9,
	[EnumMember]
	[Description("Cushion Style 1")]
	HeadStyleCushion1,
	[EnumMember]
	[Description("Cushion Style 2")]
	HeadStyleCushion2,
	[EnumMember]
	[Description("Cushion Style 3")]
	HeadStyleCushion3,
	[EnumMember]
	[Description("Cushion Style 4")]
	HeadStyleCushion4,
	[EnumMember]
	[Description("Cushion Style 5")]
	HeadStyleCushion5,
	[EnumMember]
	[Description("Cushion Style 6")]
	HeadStyleCushion6,
	[EnumMember]
	[Description("Cushion Style 7")]
	HeadStyleCushion7,
	[EnumMember]
	[Description("Cushion Style 8")]
	HeadStyleCushion8,
	[EnumMember]
	[Description("Cushion Style 9")]
	HeadStyleCushion9,
	[EnumMember]
	[Description("Cushion Style 10")]
	HeadStyleCushion10,
	[EnumMember]
	[Description("Cushion Square Style 1")]
	HeadStyleCushionSquare1,
	[EnumMember]
	[Description("Cushion Square Style 2")]
	HeadStyleCushionSquare2,
	[EnumMember]
	[Description("Cushion Square Style 3")]
	HeadStyleCushionSquare3,
	[EnumMember]
	[Description("Cushion Square Style 4")]
	HeadStyleCushionSquare4,
	[EnumMember]
	[Description("Cushion Square Style 5")]
	HeadStyleCushionSquare5,
	[EnumMember]
	[Description("Cushion Square Style 6")]
	HeadStyleCushionSquare6,
	[EnumMember]
	[Description("Cushion Square Style 7")]
	HeadStyleCushionSquare7,
	[EnumMember]
	[Description("Cushion Square Style 8")]
	HeadStyleCushionSquare8,
	[EnumMember]
	[Description("Cushion Square Style 9")]
	HeadStyleCushionSquare9,
	[EnumMember]
	[Description("Emerald Style 1")]
	HeadStyleEmerald1,
	[EnumMember]
	[Description("Emerald Style 2")]
	HeadStyleEmerald2,
	[EnumMember]
	[Description("Emerald Style 3")]
	HeadStyleEmerald3,
	[EnumMember]
	[Description("Emerald Style 4")]
	HeadStyleEmerald4,
	[EnumMember]
	[Description("Emerald Style 5")]
	HeadStyleEmerald5,
	[EnumMember]
	[Description("Emerald Style 6")]
	HeadStyleEmerald6,
	[EnumMember]
	[Description("Emerald Style 7")]
	HeadStyleEmerald7,
	[EnumMember]
	[Description("Emerald Style 8")]
	HeadStyleEmerald8,
	[EnumMember]
	[Description("Emerald Style 9")]
	HeadStyleEmerald9,
	[EnumMember]
	[Description("Emerald Square Style 1")]
	HeadStyleEmeraldSquare1,
	[EnumMember]
	[Description("Emerald Square Style 2")]
	HeadStyleEmeraldSquare2,
	[EnumMember]
	[Description("Emerald Square Style 3")]
	HeadStyleEmeraldSquare3,
	[EnumMember]
	[Description("Emerald Square Style 4")]
	HeadStyleEmeraldSquare4,
	[EnumMember]
	[Description("Emerald Square Style 5")]
	HeadStyleEmeraldSquare5,
	[EnumMember]
	[Description("Emerald Square Style 6")]
	HeadStyleEmeraldSquare6,
	[EnumMember]
	[Description("Emerald Square Style 7")]
	HeadStyleEmeraldSquare7,
	[EnumMember]
	[Description("Emerald Square Style 8")]
	HeadStyleEmeraldSquare8,
	[EnumMember]
	[Description("Emerald Square Style 9")]
	HeadStyleEmeraldSquare9,
	[EnumMember]
	[Description("Half Moon Style 1")]
	HeadStyleHalfMoon1,
	[EnumMember]
	[Description("Half Moon Style 2")]
	HeadStyleHalfMoon2,
	[EnumMember]
	[Description("Half Moon Style 3")]
	HeadStyleHalfMoon3,
	[EnumMember]
	[Description("Half Moon Style 4")]
	HeadStyleHalfMoon4,
	[EnumMember]
	[Description("Half Moon Style 5")]
	HeadStyleHalfMoon5,
	[EnumMember]
	[Description("Half Moon Style 6")]
	HeadStyleHalfMoon6,
	[EnumMember]
	[Description("Half Moon Style 7")]
	HeadStyleHalfMoon7,
	[EnumMember]
	[Description("Half Moon Style 8")]
	HeadStyleHalfMoon8,
	[EnumMember]
	[Description("Half Moon Style 9")]
	HeadStyleHalfMoon9,
	[EnumMember]
	[Description("Half Moon Style 10")]
	HeadStyleHalfMoon10,
	[EnumMember]
	[Description("Heart Style 1")]
	HeadStyleHeart1,
	[EnumMember]
	[Description("Heart Style 2")]
	HeadStyleHeart2,
	[EnumMember]
	[Description("Heart Style 3")]
	HeadStyleHeart3,
	[EnumMember]
	[Description("Heart Style 4")]
	HeadStyleHeart4,
	[EnumMember]
	[Description("Heart Style 5")]
	HeadStyleHeart5,
	[EnumMember]
	[Description("Heart Style 6")]
	HeadStyleHeart6,
	[EnumMember]
	[Description("Heart Style 7")]
	HeadStyleHeart7,
	[EnumMember]
	[Description("Heart Style 8")]
	HeadStyleHeart8,
	[EnumMember]
	[Description("Marquise Style 1")]
	HeadStyleMarquise1,
	[EnumMember]
	[Description("Marquise Style 2")]
	HeadStyleMarquise2,
	[EnumMember]
	[Description("Marquise Style 3")]
	HeadStyleMarquise3,
	[EnumMember]
	[Description("Marquise Style 4")]
	HeadStyleMarquise4,
	[EnumMember]
	[Description("Marquise Style 5")]
	HeadStyleMarquise5,
	[EnumMember]
	[Description("Marquise Style 6")]
	HeadStyleMarquise6,
	[EnumMember]
	[Description("Marquise Style 7")]
	HeadStyleMarquise7,
	[EnumMember]
	[Description("Marquise Style 8")]
	HeadStyleMarquise8,
	[EnumMember]
	[Description("Octagon Style 1")]
	HeadStyleOctagon1,
	[EnumMember]
	[Description("Octagon Style 2")]
	HeadStyleOctagon2,
	[EnumMember]
	[Description("Octagon Style 3")]
	HeadStyleOctagon3,
	[EnumMember]
	[Description("Octagon Style 4")]
	HeadStyleOctagon4,
	[EnumMember]
	[Description("Octagon Style 5")]
	HeadStyleOctagon5,
	[EnumMember]
	[Description("Octagon Style 6")]
	HeadStyleOctagon6,
	[EnumMember]
	[Description("Octagon Style 7")]
	HeadStyleOctagon7,
	[EnumMember]
	[Description("Octagon Style 8")]
	HeadStyleOctagon8,
	[EnumMember]
	[Description("Octagon Style 9")]
	HeadStyleOctagon9,
	[EnumMember]
	[Description("Oval Style 1")]
	HeadStyleOval1,
	[EnumMember]
	[Description("Oval Style 2")]
	HeadStyleOval2,
	[EnumMember]
	[Description("Oval Style 3")]
	HeadStyleOval3,
	[EnumMember]
	[Description("Oval Style 4")]
	HeadStyleOval4,
	[EnumMember]
	[Description("Oval Style 5")]
	HeadStyleOval5,
	[EnumMember]
	[Description("Oval Style 6")]
	HeadStyleOval6,
	[EnumMember]
	[Description("Oval Style 7")]
	HeadStyleOval7,
	[EnumMember]
	[Description("Oval Style 8")]
	HeadStyleOval8,
	[EnumMember]
	[Description("Oval Style 9")]
	HeadStyleOval9,
	[EnumMember]
	[Description("Oval Style 10")]
	HeadStyleOval10,
	[EnumMember]
	[Description("Pear Style 1")]
	HeadStylePear1,
	[EnumMember]
	[Description("Pear Style 2")]
	HeadStylePear2,
	[EnumMember]
	[Description("Pear Style 3")]
	HeadStylePear3,
	[EnumMember]
	[Description("Pear Style 4")]
	HeadStylePear4,
	[EnumMember]
	[Description("Pear Style 5")]
	HeadStylePear5,
	[EnumMember]
	[Description("Pear Style 6")]
	HeadStylePear6,
	[EnumMember]
	[Description("Pear Style 7")]
	HeadStylePear7,
	[EnumMember]
	[Description("Pear Style 8")]
	HeadStylePear8,
	[EnumMember]
	[Description("Pear Style 9")]
	HeadStylePear9,
	[EnumMember]
	[Description("Princess Style 1")]
	HeadStylePrincess1,
	[EnumMember]
	[Description("Princess Style 2")]
	HeadStylePrincess2,
	[EnumMember]
	[Description("Princess Style 3")]
	HeadStylePrincess3,
	[EnumMember]
	[Description("Princess Style 4")]
	HeadStylePrincess4,
	[EnumMember]
	[Description("Princess Style 5")]
	HeadStylePrincess5,
	[EnumMember]
	[Description("Princess Style 6")]
	HeadStylePrincess6,
	[EnumMember]
	[Description("Princess Style 7")]
	HeadStylePrincess7,
	[EnumMember]
	[Description("Princess Style 8")]
	HeadStylePrincess8,
	[EnumMember]
	[Description("Princess Style 9")]
	HeadStylePrincess9,
	[EnumMember]
	[Description("Radiant Style 1")]
	HeadStyleRadiant1,
	[EnumMember]
	[Description("Radiant Style 2")]
	HeadStyleRadiant2,
	[EnumMember]
	[Description("Radiant Style 3")]
	HeadStyleRadiant3,
	[EnumMember]
	[Description("Radiant Style 4")]
	HeadStyleRadiant4,
	[EnumMember]
	[Description("Radiant Style 5")]
	HeadStyleRadiant5,
	[EnumMember]
	[Description("Radiant Style 6")]
	HeadStyleRadiant6,
	[EnumMember]
	[Description("Radiant Style 7")]
	HeadStyleRadiant7,
	[EnumMember]
	[Description("Radiant Style 8")]
	HeadStyleRadiant8,
	[EnumMember]
	[Description("Radiant Style 9")]
	HeadStyleRadiant9,
	[EnumMember]
	[Description("Radiant Style 10")]
	HeadStyleRadiant10,
	[EnumMember]
	[Description("Radiant Square Style 1")]
	HeadStyleRadiantSquare1,
	[EnumMember]
	[Description("Radiant Square Style 2")]
	HeadStyleRadiantSquare2,
	[EnumMember]
	[Description("Radiant Square Style 3")]
	HeadStyleRadiantSquare3,
	[EnumMember]
	[Description("Radiant Square Style 4")]
	HeadStyleRadiantSquare4,
	[EnumMember]
	[Description("Radiant Square Style 5")]
	HeadStyleRadiantSquare5,
	[EnumMember]
	[Description("Radiant Square Style 6")]
	HeadStyleRadiantSquare6,
	[EnumMember]
	[Description("Radiant Square Style 7")]
	HeadStyleRadiantSquare7,
	[EnumMember]
	[Description("Radiant Square Style 8")]
	HeadStyleRadiantSquare8,
	[EnumMember]
	[Description("Radiant Square Style 9")]
	HeadStyleRadiantSquare9,
	[EnumMember]
	[Description("Radiant Square Style 10")]
	HeadStyleRadiantSquare10,
	[EnumMember]
	[Description("Round Style 1")]
	HeadStyleRound1,
	[EnumMember]
	[Description("Round Style 2")]
	HeadStyleRound2,
	[EnumMember]
	[Description("Round Style 3")]
	HeadStyleRound3,
	[EnumMember]
	[Description("Round Style 4")]
	HeadStyleRound4,
	[EnumMember]
	[Description("Round Style 5")]
	HeadStyleRound5,
	[EnumMember]
	[Description("Round Style 6")]
	HeadStyleRound6,
	[EnumMember]
	[Description("Round Style 7")]
	HeadStyleRound7,
	[EnumMember]
	[Description("Round Style 8")]
	HeadStyleRound8,
	[EnumMember]
	[Description("Round Style 9")]
	HeadStyleRound9,
	[EnumMember]
	[Description("Round Style 10")]
	HeadStyleRound10,
	[EnumMember]
	[Description("Triangle Style 1")]
	HeadStyleTriangle1,
	[EnumMember]
	[Description("Triangle Style 2")]
	HeadStyleTriangle2,
	[EnumMember]
	[Description("Triangle Style 3")]
	HeadStyleTriangle3,
	[EnumMember]
	[Description("Triangle Style 4")]
	HeadStyleTriangle4,
	[EnumMember]
	[Description("Triangle Style 5")]
	HeadStyleTriangle5,
	[EnumMember]
	[Description("Triangle Style 6")]
	HeadStyleTriangle6,
	[EnumMember]
	[Description("Triangle Style 7")]
	HeadStyleTriangle7,
	[EnumMember]
	[Description("Triangle Style 8")]
	HeadStyleTriangle8,
	[EnumMember]
	[Description("Triangle Style 9")]
	HeadStyleTriangle9,
	[EnumMember]
	[Description("Trillion Curved Style 1")]
	HeadStyleTrillionCurved1,
	[EnumMember]
	[Description("Trillion Curved Style 2")]
	HeadStyleTrillionCurved2,
	[EnumMember]
	[Description("Trillion Curved Style 3")]
	HeadStyleTrillionCurved3,
	[EnumMember]
	[Description("Trillion Curved Style 4")]
	HeadStyleTrillionCurved4,
	[EnumMember]
	[Description("Trillion Curved Style 5")]
	HeadStyleTrillionCurved5,
	[EnumMember]
	[Description("Trillion Curved Style 6")]
	HeadStyleTrillionCurved6,
	[EnumMember]
	[Description("Trillion Curved Style 7")]
	HeadStyleTrillionCurved7,
	[EnumMember]
	[Description("Trillion Curved Style 8")]
	HeadStyleTrillionCurved8,
	[EnumMember]
	[Description("Trillion Curved Style 9")]
	HeadStyleTrillionCurved9,
	[EnumMember]
	[Description("Trillion Straight Style 1")]
	HeadStyleTrillionStraight1,
	[EnumMember]
	[Description("Trillion Straight Style 2")]
	HeadStyleTrillionStraight2,
	[EnumMember]
	[Description("Trillion Straight Style 3")]
	HeadStyleTrillionStraight3,
	[EnumMember]
	[Description("Trillion Straight Style 4")]
	HeadStyleTrillionStraight4,
	[EnumMember]
	[Description("Trillion Straight Style 5")]
	HeadStyleTrillionStraight5,
	[EnumMember]
	[Description("Trillion Straight Style 6")]
	HeadStyleTrillionStraight6,
	[EnumMember]
	[Description("Trillion Straight Style 7")]
	HeadStyleTrillionStraight7,
	[EnumMember]
	[Description("Trillion Straight Style 8")]
	HeadStyleTrillionStraight8,
	[EnumMember]
	[Description("Trillion Straight Style 9")]
	HeadStyleTrillionStraight9,
	[EnumMember]
	[Description("Baguette Tapered Style 1")]
	HeadStyleBaguetteTapered1,
	[EnumMember]
	[Description("Baguette Tapered Style 2")]
	HeadStyleBaguetteTapered2,
	[EnumMember]
	[Description("Baguette Tapered Style 3")]
	HeadStyleBaguetteTapered3,
	[EnumMember]
	[Description("Baguette Tapered Style 4")]
	HeadStyleBaguetteTapered4,
	[EnumMember]
	[Description("Baguette Tapered Style 5")]
	HeadStyleBaguetteTapered5,
	[EnumMember]
	[Description("Baguette Tapered Style 6")]
	HeadStyleBaguetteTapered6,
	[EnumMember]
	[Description("Baguette Tapered Style 7")]
	HeadStyleBaguetteTapered7,
	[EnumMember]
	[Description("Baguette Tapered Style 8")]
	HeadStyleBaguetteTapered8,
	[EnumMember]
	[Description("Baguette Tapered Style 9")]
	HeadStyleBaguetteTapered9,
	[EnumMember]
	[Description("Baguette Tapered Style 10")]
	HeadStyleBaguetteTapered10,
	[EnumMember]
	[Description("Bullet Style 1")]
	HeadStyleBullet1,
	[EnumMember]
	[Description("Bullet Style 2")]
	HeadStyleBullet2,
	[EnumMember]
	[Description("Bullet Style 3")]
	HeadStyleBullet3,
	[EnumMember]
	[Description("Bullet Style 4")]
	HeadStyleBullet4,
	[EnumMember]
	[Description("Bullet Style 5")]
	HeadStyleBullet5,
	[EnumMember]
	[Description("Bullet Style 6")]
	HeadStyleBullet6,
	[EnumMember]
	[Description("Bullet Style 7")]
	HeadStyleBullet7,
	[EnumMember]
	[Description("Bullet Style 8")]
	HeadStyleBullet8,
	[EnumMember]
	[Description("Bullet Style 9")]
	HeadStyleBullet9,
	[EnumMember]
	[Description("Bullet Style 10")]
	HeadStyleBullet10,
	[EnumMember]
	[Description("Square Style 1")]
	HeadStyleSquare1,
	[EnumMember]
	[Description("Square Style 2")]
	HeadStyleSquare2,
	[EnumMember]
	[Description("Square Style 3")]
	HeadStyleSquare3,
	[EnumMember]
	[Description("Square Style 4")]
	HeadStyleSquare4,
	[EnumMember]
	[Description("Square Style 5")]
	HeadStyleSquare5,
	[EnumMember]
	[Description("Square Style 6")]
	HeadStyleSquare6,
	[EnumMember]
	[Description("Square Style 7")]
	HeadStyleSquare7,
	[EnumMember]
	[Description("Square Style 8")]
	HeadStyleSquare8,
	[EnumMember]
	[Description("Square Style 9")]
	HeadStyleSquare9,
	[EnumMember]
	[Description("Trillion Style 1")]
	HeadStyleTrillion1,
	[EnumMember]
	[Description("Trillion Style 2")]
	HeadStyleTrillion2,
	[EnumMember]
	[Description("Trillion Style 3")]
	HeadStyleTrillion3,
	[EnumMember]
	[Description("Trillion Style 4")]
	HeadStyleTrillion4,
	[EnumMember]
	[Description("Trillion Style 5")]
	HeadStyleTrillion5,
	[EnumMember]
	[Description("Trillion Style 6")]
	HeadStyleTrillion6,
	[EnumMember]
	[Description("Trillion Style 7")]
	HeadStyleTrillion7,
	[EnumMember]
	[Description("Trillion Style 8")]
	HeadStyleTrillion8,
	[EnumMember]
	[Description("Trillion Style 9")]
	HeadStyleTrillion9,
	[EnumMember]
	[Description("Coffin Style 1")]
	HeadStyleCoffin1,
	[EnumMember]
	[Description("Coffin Style 2")]
	HeadStyleCoffin2,
	[EnumMember]
	[Description("Coffin Style 3")]
	HeadStyleCoffin3,
	[EnumMember]
	[Description("Coffin Style 4")]
	HeadStyleCoffin4,
	[EnumMember]
	[Description("Coffin Style 5")]
	HeadStyleCoffin5,
	[EnumMember]
	[Description("Coffin Style 6")]
	HeadStyleCoffin6,
	[EnumMember]
	[Description("Coffin Style 7")]
	HeadStyleCoffin7,
	[EnumMember]
	[Description("Coffin Style 8")]
	HeadStyleCoffin8,
	[EnumMember]
	[Description("Coffin Style 9")]
	HeadStyleCoffin9,
	[EnumMember]
	[Description("Hexagon Style 1")]
	HeadStyleHexagon1,
	[EnumMember]
	[Description("Hexagon Style 2")]
	HeadStyleHexagon2,
	[EnumMember]
	[Description("Hexagon Style 3")]
	HeadStyleHexagon3,
	[EnumMember]
	[Description("Hexagon Style 4")]
	HeadStyleHexagon4,
	[EnumMember]
	[Description("Hexagon Style 5")]
	HeadStyleHexagon5,
	[EnumMember]
	[Description("Hexagon Style 6")]
	HeadStyleHexagon6,
	[EnumMember]
	[Description("Hexagon Style 7")]
	HeadStyleHexagon7,
	[EnumMember]
	[Description("Hexagon Style 8")]
	HeadStyleHexagon8
}
