using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared;

[DataContract]
public enum ValueListItem
{
	[EnumMember]
	[Description("Unknown")]
	Unknown = 0,
	[EnumMember]
	[Description("Profile Placer block to Center")]
	ProfilePlacerToCenter = 1,
	[EnumMember]
	[Description("Profile Placer block to 0,Y,0")]
	ProfilePlacerTo0Y0 = 2,
	[EnumMember]
	[Description("Profile Placer block Up")]
	ProfilePlacerUp = 3,
	[EnumMember]
	[Description("Profile Placer block in Rail Plane")]
	ProfilePlacerInRailPlane = 4,
	[EnumMember]
	[Description("Profile Placer block in Rail Plane Flat")]
	ProfilePlacerInRailPlaneFlat = 5,
	[EnumMember]
	[Description("Profile Placer block Down")]
	ProfilePlacerDown = 6,
	[EnumMember]
	[Description("Profile Placer block Out Y")]
	ProfilePlacerOutY = 7,
	[EnumMember]
	[Description("Profile Placer block in Rail Plane Force Up")]
	ProfilePlacerInRailPlaneForceUp = 8,
	[EnumMember]
	[Description("Profile Placer block Horizontal Curve Up")]
	ProfilePlacerHorizontalCurveUp = 9,
	[EnumMember]
	[Description("Gem spacing method: Minimum Spacing")]
	GemSpacingMethodMinimum = 10,
	[EnumMember]
	[Description("Gem spacing method: Minimum Spacing Scaled")]
	GemSpacingMethodMinimumScaled = 11,
	[EnumMember]
	[Description("Gem spacing method: Fixed Spacing")]
	GemSpacingMethodFixed = 12,
	[EnumMember]
	[Description("Gem orientation method: Surface")]
	[Obsolete]
	GemOrientationMethodSurface = 13,
	[EnumMember]
	[Description("Gem orientation method: Closest Point on Curve")]
	[Obsolete]
	GemOrientationMethodCurve = 14,
	[EnumMember]
	[Description("Gem orientation method: Point")]
	[Obsolete]
	GemOrientationMethodPoint = 15,
	[EnumMember]
	[Description("Gem orientation method: Flow along Crv, Angle to Origin")]
	GemOrientationMethodOrigin = 16,
	[EnumMember]
	[Description("Gem orientation method: Flow along Crv, Angle to Y Axis")]
	GemOrientationMethodAxisY = 17,
	[EnumMember]
	[Description("Gem orientation method: Point Down")]
	GemOrientationMethodDown = 18,
	[EnumMember]
	[Description("Sort X Positive")]
	SortAxisXPos = 20,
	[EnumMember]
	[Description("Sort X Negative")]
	SortAxisXNeg = 21,
	[EnumMember]
	[Description("Sort Y Positive")]
	SortAxisYPos = 22,
	[EnumMember]
	[Description("Sort Y Negative")]
	SortAxisYNeg = 23,
	[EnumMember]
	[Description("Sort Z Positive")]
	SortAxisZPos = 24,
	[EnumMember]
	[Description("Sort Z Negative")]
	SortAxisZNeg = 25,
	[EnumMember]
	[Description("Gem placement method: Girdle")]
	GemPlacementMethodGirdle = 30,
	[EnumMember]
	[Description("Gem placement method: Culet")]
	GemPlacementMethodCulet = 31,
	[EnumMember]
	[Description("Gem placement method: Table")]
	GemPlacementMethodTable = 32,
	[EnumMember]
	[Description("Gem placement method: Girdle Scaled")]
	GemPlacementMethodGirdleScaled = 33,
	[EnumMember]
	[Description("Gem placement method: Culet Scaled")]
	GemPlacementMethodCuletScaled = 34,
	[EnumMember]
	[Description("Gem placement method: Table Scaled")]
	GemPlacementMethodTableScaled = 35,
	[EnumMember]
	[Description("Head builder center style 1")]
	HeadStyleCenter1 = 40,
	[EnumMember]
	[Description("Head builder center style 2")]
	HeadStyleCenter2 = 41,
	[EnumMember]
	[Description("Head builder center style 3")]
	HeadStyleCenter3 = 42,
	[EnumMember]
	[Description("Head builder center style 4")]
	HeadStyleCenter4 = 43,
	[EnumMember]
	[Description("Head builder center style 5")]
	HeadStyleCenter5 = 44,
	[EnumMember]
	[Description("Head builder center style 6")]
	HeadStyleCenter6 = 45,
	[EnumMember]
	[Description("Head builder side style 1")]
	HeadStyleSide1 = 50,
	[EnumMember]
	[Description("Head builder side style 2")]
	HeadStyleSide2 = 51,
	[EnumMember]
	[Description("Head builder side style 3")]
	HeadStyleSide3 = 52,
	[EnumMember]
	[Description("Head builder side style 4")]
	HeadStyleSide4 = 53,
	[EnumMember]
	[Description("Head builder side style 5")]
	HeadStyleSide5 = 54,
	[EnumMember]
	[Description("Head builder side style 6")]
	HeadStyleSide6 = 55,
	[EnumMember]
	[Description("Cluster prong style 1")]
	ClusterProngStyle1 = 60,
	[EnumMember]
	[Description("Cluster prong style 2")]
	ClusterProngStyle2 = 61,
	[EnumMember]
	[Description("Cluster prong style 3")]
	ClusterProngStyle3 = 62,
	[EnumMember]
	[Description("Cluster prong style 4")]
	ClusterProngStyle4 = 63,
	[EnumMember]
	[Description("Cluster prong style 5")]
	ClusterProngStyle5 = 64,
	[EnumMember]
	[Description("Cluster prong style 6")]
	ClusterProngStyle6 = 65,
	[EnumMember]
	[Description("Cluster prong style 7")]
	ClusterProngStyle7 = 66,
	[EnumMember]
	[Description("Cluster prong style 8")]
	ClusterProngStyle8 = 67,
	[EnumMember]
	[Description("Cluster prong style 9")]
	ClusterProngStyle9 = 68,
	[EnumMember]
	[Description("Cluster prong style 10")]
	ClusterProngStyle10 = 69,
	[EnumMember]
	[Description("Extend curve type: Line")]
	ExtendCurveTypeLine = 70,
	[EnumMember]
	[Description("Extend curve type: Arc")]
	ExtendCurveTypeArc = 71,
	[EnumMember]
	[Description("Extend curve type: Smooth")]
	ExtendCurveTypeSmooth = 72,
	[EnumMember]
	[Description("End prong style 1")]
	ProngPlacerStyleEnd1 = 80,
	[EnumMember]
	[Description("End prong style 2")]
	ProngPlacerStyleEnd2 = 81,
	[EnumMember]
	[Description("End prong style 3")]
	ProngPlacerStyleEnd3 = 82,
	[EnumMember]
	[Description("End prong style 4")]
	ProngPlacerStyleEnd4 = 83,
	[EnumMember]
	[Description("End prong style 5")]
	ProngPlacerStyleEnd5 = 84,
	[EnumMember]
	[Description("End prong style 6")]
	ProngPlacerStyleEnd6 = 85,
	[EnumMember]
	[Description("End prong style 7")]
	ProngPlacerStyleEnd7 = 86,
	[EnumMember]
	[Description("End prong style 8")]
	ProngPlacerStyleEnd8 = 87,
	[EnumMember]
	[Description("End prong style 9")]
	ProngPlacerStyleEnd9 = 88,
	[EnumMember]
	[Description("End prong style 10")]
	ProngPlacerStyleEnd10 = 89,
	[EnumMember]
	[Description("Gem Cluster - Center Shape Override Mode: X Dimension")]
	GemClusterCenterShapeOverrideModeDimensionX = 90,
	[EnumMember]
	[Description("Gem Cluster - Center Shape Override Mode: Y Dimension")]
	GemClusterCenterShapeOverrideModeDimensionY = 91,
	[EnumMember]
	[Description("Gem Cluster - Center Shape Override Mode: Largest Dimension")]
	GemClusterCenterShapeOverrideModeDimensionLargest = 92,
	[EnumMember]
	[Description("Gem Cluster - Center Shape Override Mode: Smallest Dimension")]
	GemClusterCenterShapeOverrideModeDimensionSmallest = 93,
	[EnumMember]
	[Description("Output To Array")]
	SortAxisOutputArray = 94,
	[EnumMember]
	[Description("Output to Outputs")]
	SortAxisOutputOutputs = 95,
	[EnumMember]
	[Description("Output First Only")]
	SortAxisOutputFirstOnly = 96,
	[EnumMember]
	[Description("X Axis")]
	FlattenXAxis = 97,
	[EnumMember]
	[Description("Y Axis")]
	FlattenYAxis = 98,
	[EnumMember]
	[Description("Z Axis")]
	FlattenZAxis = 99,
	[EnumMember]
	[Description("First Curve")]
	OrientCurveFirst = 100,
	[EnumMember]
	[Description("Second Curve")]
	OrientCurveSecond = 101,
	[EnumMember]
	[Description("No Mirror")]
	MirrorNone = 102,
	[EnumMember]
	[Description("Mirror X")]
	MirrorX = 103,
	[EnumMember]
	[Description("Mirror X Opposite Y")]
	MirrorXOpp = 104,
	[EnumMember]
	[Description("Mirror Y")]
	MirrorY = 105,
	[EnumMember]
	[Description("Mirror Quad")]
	MirrorQuad = 106,
	[EnumMember]
	[Description("No Center")]
	VPCHNoCenter = 107,
	[EnumMember]
	[Description("Auto Center")]
	VPCHAutoCenter = 108,
	[EnumMember]
	[Description("Auto Center on XY")]
	VPCHAutoCenterXY = 109,
	[EnumMember]
	[Description("Resize Gem Mode: Unconstrained")]
	ResizeGemModeUnconstrained = 110,
	[EnumMember]
	[Description("Resize Gem Mode: Constrain to Full Gem Size Table")]
	ResizeGemModeConstrainedToFullGemSizeTable = 111,
	[EnumMember]
	[Description("Resize Gem Mode: Constrain to Iteration Gem Size Table")]
	ResizeGemModeConstrainedToIterationGemSizeTable = 112,
	[EnumMember]
	[Description("Continuity Mode: loose")]
	ContinuityModeLoose = 113,
	[EnumMember]
	[Description("Continuity Mode: pos")]
	ContinuityModePos = 114,
	[EnumMember]
	[Description("Continuity Mode: tan")]
	ContinuityModeTan = 115,
	[EnumMember]
	[Description("Continuity Mode: curvature")]
	ContinuityModeCurvature = 116,
	[EnumMember]
	[Description("Odd or Even")]
	GemOnCrvMirrorOddAndEven = 117,
	[EnumMember]
	[Description("Odd (Center Gem)")]
	GemOnCrvMirrorOdd = 118,
	[EnumMember]
	[Description("Even (No Center Gem)")]
	GemOnCrvMirrorEven = 119,
	[EnumMember]
	[Description("Regular Shoulder")]
	RingRailFlexShoulderStyleRegular = 120,
	[EnumMember]
	[Description("Inverted Shoulder")]
	RingRailFlexShoulderStyleInverted = 121,
	[EnumMember]
	[Description("Prong Head")]
	HeadBuilderProngHead = 122,
	[EnumMember]
	[Description("Prong Base To Point")]
	HeadBuilderProngHeadPoint = 123,
	[EnumMember]
	[Description("Prong Base Cut To Point")]
	HeadBuilderProngHeadCutToPoint = 124,
	[EnumMember]
	[Description("Head Prongs Profile")]
	HeadBuilderProngProfile = 125,
	[EnumMember]
	[Description("Head Prongs Offset Lookup")]
	HeadBuilderProngOffsetLookup = 126,
	[EnumMember]
	[Description("Head Prongs Offset Force")]
	HeadBuilderProngOffsetForce = 127,
	[EnumMember]
	[Description("Setting Style Prong")]
	ProngPlacerSettingStyleProng = 128,
	[EnumMember]
	[Description("Setting Style Bar")]
	ProngPlacerSettingStyleBar = 129,
	[EnumMember]
	[Description("Setting Style Auto")]
	ProngPlacerSettingStyleAuto = 130,
	[EnumMember]
	[Description("Setting Style Off")]
	ProngPlacerSettingStyleOff = 131,
	[EnumMember]
	[Description("Normal Mode: +X Axis")]
	PlanarCutterNormalModePosX = 132,
	[EnumMember]
	[Description("Normal Mode: -X Axis")]
	PlanarCutterNormalModeNegX = 133,
	[EnumMember]
	[Description("Normal Mode: +Y Axis")]
	PlanarCutterNormalModePosY = 134,
	[EnumMember]
	[Description("Normal Mode: -Y Axis")]
	PlanarCutterNormalModeNegY = 135,
	[EnumMember]
	[Description("Normal Mode: +Z Axis")]
	PlanarCutterNormalModePosZ = 136,
	[EnumMember]
	[Description("Normal Mode: -Z Axis")]
	PlanarCutterNormalModeNegZ = 137,
	[EnumMember]
	[Description("Shank: Angle to Curve")]
	ShankOrientationMethodAxisY = 138,
	[EnumMember]
	[Description("Shank: Closest Point on Curve")]
	ShankOrientationMethodCurve = 139,
	[EnumMember]
	[Description("Orientation method: Closest Point on Curve")]
	OrientationMethodClosestPointOnCurve = 140,
	[EnumMember]
	[Description("Orientation method: Intersect Point on Curve")]
	OrientationMethodIntersectPointOnCurve = 141,
	[EnumMember]
	[Description("Sort On Center Point")]
	SortOnCenterPoint = 142,
	[EnumMember]
	[Description("Sort On Max Point")]
	SortOnMaxPoint = 143,
	[EnumMember]
	[Description("Sort On Min Point")]
	SortOnMinPoint = 144,
	[EnumMember]
	[Description("Start Position Center of Gem")]
	CenterofGem = 145,
	[EnumMember]
	[Description("Start Position Shank Edge")]
	ShankEdge = 146,
	[EnumMember]
	[Description("Start Position Gem Edge")]
	GemEdge = 147,
	[EnumMember]
	[Description("Follow Culet")]
	FollowCulet = 148,
	[EnumMember]
	[Description("Follow Largest Gem")]
	FollowLargestGem = 149,
	[EnumMember]
	[Description("Min Culet Off")]
	NoMinCulet = 150,
	[EnumMember]
	[Description("Min Culet Grow Up")]
	MinCuletGrowUp = 151,
	[EnumMember]
	[Description("Min Culet Grow Smaller")]
	MinCuletGrowSmaller = 152,
	[EnumMember]
	[Description("Straight")]
	Straight = 153,
	[EnumMember]
	[Description("Tangent")]
	Tangent = 154,
	[EnumMember]
	[Description("Gem Cluster - Center Shape Override Mode: Dimension X and Y")]
	GemClusterCenterShapeOverrideModeDimensionXandY = 155,
	[EnumMember]
	[Description("Prong Size Control : Off")]
	NormalProng = 156,
	[EnumMember]
	[Description("Prong Size Control : Largest Prong")]
	LargestProng = 157,
	[EnumMember]
	[Description("Prong Size Control : Smallest Prong")]
	SmallestProng = 158,
	[EnumMember]
	[Description("Prong Size Control : Average Prong")]
	AverageProng = 159,
	[EnumMember]
	[Description("Left and Right")]
	Sweep4MethodLeftAndRight = 160,
	[EnumMember]
	[Description("Top and Bottom")]
	Sweep4MethodTopAndBottom = 161,
	[EnumMember]
	[Description("Unknown")]
	SettingTypeXX = 162,
	[EnumMember]
	[Description("Bright Cut")]
	SettingTypeBC = 163,
	[EnumMember]
	[Description("Faux Bright Cut")]
	SettingTypeFB = 164,
	[EnumMember]
	[Description("Bar")]
	SettingTypeBR = 165,
	[EnumMember]
	[Description("Bezel")]
	SettingTypeBZ = 166,
	[EnumMember]
	[Description("Half Bezel")]
	SettingTypeHB = 167,
	[EnumMember]
	[Description("Prong")]
	SettingTypePR = 168,
	[EnumMember]
	[Description("V-Prong")]
	SettingTypeVP = 169,
	[EnumMember]
	[Description("Channel")]
	SettingTypeCH = 170,
	[EnumMember]
	[Description("Prong Half Channel")]
	SettingTypePC = 171,
	[EnumMember]
	[Description("Cement")]
	SettingTypeCM = 172,
	[EnumMember]
	[Description("Faux Pave")]
	SettingTypeFP = 173,
	[EnumMember]
	[Description("Pave")]
	SettingTypePV = 174,
	[EnumMember]
	[Description("Fish Tail")]
	SettingTypeFT = 175,
	[EnumMember]
	[Description("Gypsy")]
	SettingTypeGP = 176,
	[EnumMember]
	[Description("Mabe")]
	SettingTypeMB = 177,
	[EnumMember]
	[Description("Pin Point")]
	SettingTypePT = 178,
	[EnumMember]
	[Description("Shared Prong")]
	SettingTypeSH = 179,
	[EnumMember]
	[Description("French Set")]
	SettingTypeFS = 180,
	[EnumMember]
	[Description("Claw Prong")]
	SettingTypeCW = 181,
	[EnumMember]
	[Description("Min Culet Shank Grow Up")]
	MinCuletShankGrowUp = 182,
	[EnumMember]
	[Description("Capped")]
	ChannelCutterEndTypeCapped = 183,
	[EnumMember]
	[Description("Round")]
	ChannelCutterEndTypeRound = 184,
	[EnumMember]
	[Description("Flat")]
	ChannelCutterEndTypeFlat = 185,
	[EnumMember]
	[Description("Sharp Corner")]
	ChannelCutterEndTypeSharpCorner = 186,
	[Description("Gem orientation method: Object")]
	GemOrientationMethodObject = 187,
	[EnumMember]
	[Description("Shared Gem")]
	ProngLayout1 = 188,
	[EnumMember]
	[Description("Front and Back")]
	ProngLayout2 = 189,
	[EnumMember]
	[Description("Left and Right")]
	ProngLayout3 = 190,
	[EnumMember]
	[Description("4 Prongs")]
	ProngLayout4 = 191,
	[EnumMember]
	[Description("Left To Right Diagonal")]
	ProngLayout5 = 192,
	[EnumMember]
	[Description("Right To Left Diagonal")]
	ProngLayout6 = 193,
	[EnumMember]
	[Description("6 Prongs")]
	ProngLayout7 = 194,
	[EnumMember]
	[Description("None")]
	OffsetCornerStyleNone = 195,
	[EnumMember]
	[Description("Sharp")]
	OffsetCornerStyleSharp = 196,
	[EnumMember]
	[Description("Round")]
	OffsetCornerStyleRound = 197,
	[EnumMember]
	[Description("Smooth")]
	OffsetCornerStyleSmooth = 198,
	[EnumMember]
	[Description("Chamfer")]
	OffsetCornerStyleChamfer = 199,
	[EnumMember]
	[Description("Left")]
	AlignHorizontalLeft = 200,
	[EnumMember]
	[Description("Middle")]
	AlignHorizontalMiddle = 201,
	[EnumMember]
	[Description("Right")]
	AlignHorizontalRight = 202,
	[EnumMember]
	[Description("Justified")]
	AlignHorizontalJustified = 203,
	[EnumMember]
	[Description("Top")]
	AlignVerticalTop = 204,
	[EnumMember]
	[Description("Middle")]
	AlignVerticalMiddle = 205,
	[EnumMember]
	[Description("Bottom")]
	AlignVerticalBottom = 206,
	[EnumMember]
	[Description("Boolean Union")]
	BooleanUnion = 207,
	[EnumMember]
	[Description("Boolean Intersection")]
	BooleanIntersection = 208,
	[EnumMember]
	[Description("Boolean Difference")]
	BooleanDifference = 209,
	[EnumMember]
	[Description("Start to Start")]
	BlendStartStart = 210,
	[EnumMember]
	[Description("Start to End")]
	BlendStartEnd = 211,
	[EnumMember]
	[Description("End to Start")]
	BlendEndStart = 212,
	[EnumMember]
	[Description("End to End")]
	BlendEndEnd = 213,
	[EnumMember]
	[Description("None")]
	PipeCapNone = 214,
	[EnumMember]
	[Description("Flat")]
	PipeCapFlat = 215,
	[EnumMember]
	[Description("Round")]
	PipeCapRound = 216,
	[EnumMember]
	[Description("Arial")]
	FontFaceArial = 500,
	[EnumMember]
	[Description("Bookman Old Style")]
	FontFaceBookmanOldStyle = 501,
	[EnumMember]
	[Description("Comic Sans MS")]
	FontFaceComicSansMs = 502,
	[EnumMember]
	[Description("Courier New")]
	FontFaceCourierNew = 503,
	[EnumMember]
	[Description("Helvetica")]
	FontFaceHelvetica = 504,
	[EnumMember]
	[Description("Tahoma")]
	FontFaceTahoma = 505,
	[EnumMember]
	[Description("Times New Roman")]
	FontFaceTimesNewRoman = 506,
	[EnumMember]
	[Description("Verdana")]
	FontFaceVerdana = 507
}
