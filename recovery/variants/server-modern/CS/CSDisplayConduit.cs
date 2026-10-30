using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.CSValues.CSValueHandle.HandleSupportClasses;
using CS.Material;
using CS.MaterialGroup;
using CS.Parametric;
using CS.Parametric.Channels;
using CS.Parametric.Interfaces;
using CSDatas;
using CSDatas.Interfaces;
using CSMaterials;
using GV.CounterSketch.Shared;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace CS;

public sealed class CSDisplayConduit : DisplayConduit
{
	private const uint _glDepthBufferBit = 256u;

	private readonly Color _materialOnColor = Color.White;

	private readonly Color _materialOnSpecular = Color.Black;

	public int _curveLineSize = 1;

	public CSBlock _fingernailObjects;

	public CSBlock _handObjects;

	public double _handRingRailYOffset;

	public bool _isDisplayHandEnabled;

	public bool _isDisplayHighlightEnabled = true;

	public bool _isHandFlipped;

	public bool _isRingFlipped;

	public bool _isSelectionDisplayModeEnabled;

	public bool _isWireframeModeEnabled;

	public Transform _layFlatForm = Transform.Unset;

	public CSBlock _necklaceChainObjects;

	public CSBlock _shirtObjects;

	private bool _isMaterialStoreLoaded;

	public Color? BackgroundColor;

	public static bool IsGemMapEnabled { get; set; }

	public Collection<CSDisplayMaterial> DisplayMaterials { get; set; } = new Collection<CSDisplayMaterial>();

	public bool IsDrawingTargetedBlocksOnly { get; set; }

	public bool IsTurnTableEnabled { get; set; }

	public bool IsViewportHandlesVisible { get; set; }

	public List<Guid> TargetedBlockInstanceIds { get; set; }

	public string OverlayMessage { get; set; }

	public void DrawObjects(DisplayPipeline displayPipeline)
	{
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		if (CSApplicationHelper.Application.Document.DrawingObjectsEnabled)
		{
			CSOptionsDisplay cSOptionsDisplay = new CSOptionsDisplay
			{
				DoShowBackfaces = CSApplicationHelper.Application.Document.DrawingBackfacesEnabled,
				DoShowCurves = CSApplicationHelper.Application.Document.DrawingCurvesEnabled,
				DoShowStarts = CSApplicationHelper.Application.Document.DrawingStartsEnabed,
				DoShowPoints = CSApplicationHelper.Application.Document.DrawingPointsEnabed,
				IsMouseDown = CSApplicationHelper.Application.Document.Graph.MouseDown,
				IsSelectionDisplayModeEnabled = CSApplicationHelper.Application.Conduit._isSelectionDisplayModeEnabled,
				IsWireframeModeEnabled = CSApplicationHelper.Application.Conduit._isWireframeModeEnabled,
				IsGemNumberDisplayEnabled = IsGemMapEnabled,
				DisplayMaterials = DisplayMaterials
			};
			bool isGemNumberDisplayEnabled = cSOptionsDisplay.IsGemNumberDisplayEnabled;
			cSOptionsDisplay.IsGemNumberDisplayEnabled = IsGemMapEnabled;
			Transform designTransform = GetDesignTransform();
			displayPipeline.PushModelTransform(designTransform);
			DrawCSBlockObjects(displayPipeline, cSOptionsDisplay);
			AssignAndDisplayTryOnObjects(displayPipeline, cSOptionsDisplay);
			UpdateChainDisplayMaterials(displayPipeline, designTransform, cSOptionsDisplay);
			UpdateSelectedDisplayMaterials(displayPipeline, isGemNumberDisplayEnabled, cSOptionsDisplay);
			if (_isDisplayHighlightEnabled)
			{
				UpdateHighlightDisplayMaterials(displayPipeline, cSOptionsDisplay);
			}
			UpdateMouseOverDisplayMaterials(displayPipeline, cSOptionsDisplay);
			cSOptionsDisplay.IsGemNumberDisplayEnabled = isGemNumberDisplayEnabled;
			if (IsViewportHandlesVisible)
			{
				CSValueHandleHelper.UpdateViewportControlHandles(displayPipeline, cSOptionsDisplay);
			}
			displayPipeline.PopModelTransform();
		}
	}

	public static Transform GetChainPointTransform()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		Point3d val = Point3d.Origin;
		foreach (CSObject item in CSApplicationHelper.Application.Document.Graph.ActiveObjects())
		{
			if (item is IChainPoint chainPoint)
			{
				val = chainPoint.GetChainPoint();
			}
		}
		return Transform.Translation(((Point3d)(ref val)).X, ((Point3d)(ref val)).Y, ((Point3d)(ref val)).Z);
	}

	public CSDisplayMaterial GetCSDisplayMaterialFromName(string materialName)
	{
		CSDisplayMaterial cSDisplayMaterial = DisplayMaterials.FirstOrDefault((CSDisplayMaterial material) => material.Name == materialName);
		if (cSDisplayMaterial != null)
		{
			if (!CSApplicationHelper.Application.Document.DrawingBackfacesEnabled || cSDisplayMaterial.Mat == null)
			{
				return cSDisplayMaterial;
			}
			CSDisplayMaterial cSDisplayMaterial2 = cSDisplayMaterial.Duplicate();
			cSDisplayMaterial2.Mat.BackDiffuse = Color.MediumVioletRed;
			cSDisplayMaterial2.Mat.IsTwoSided = true;
			return cSDisplayMaterial2;
		}
		return DisplayMaterials.FirstOrDefault((CSDisplayMaterial material) => material.Name == "Undefined");
	}

	public Transform GetDesignTransform()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		Transform result = Transform.Identity;
		if ((CSUtil.DisplayComponentsInFile() & DisplayComponents.Pendant) == DisplayComponents.Pendant)
		{
			return ((Transform)(ref _layFlatForm)).IsValid ? Transform.Identity : Transform.Rotation(CSCoreMath.ToRadians(90.0), Vector3d.XAxis, Point3d.Origin);
		}
		if (((Transform)(ref _layFlatForm)).IsValid)
		{
			result = _layFlatForm;
		}
		if (CSApplicationHelper.Application.Conduit._isRingFlipped)
		{
			Transform val = Transform.Rotation(CSCoreMath.ToRadians(180.0), Vector3d.ZAxis, Point3d.Origin);
			result = ((!((Transform)(ref _layFlatForm)).IsValid) ? val : (_layFlatForm * val));
		}
		return result;
	}

	public static Transform GetHandTransform()
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		double value = 8.66;
		CSOutputChannel outValue = null;
		if (CSUtil.FindLastOutputRingRailSize(ref outValue))
		{
			outValue.Data?.GetDouble(ref value);
		}
		Transform val = (CSApplicationHelper.Application.Conduit._isHandFlipped ? Transform.Scale(Plane.WorldXY, (0.0 - value) / 8.66, value / 8.66, value / 8.66) : Transform.Scale(Plane.WorldXY, value / 8.66, value / 8.66, value / 8.66));
		return Transform.Translation(0.0, CSApplicationHelper.Application.Conduit._handRingRailYOffset, 0.0) * val;
	}

	public void LoadMaterials()
	{
		if (_isMaterialStoreLoaded)
		{
			return;
		}
		foreach (object value in Enum.GetValues(typeof(GemMaterial)))
		{
			AddGem((GemMaterial)value);
		}
		foreach (object value2 in Enum.GetValues(typeof(MetalMaterial)))
		{
			AddMetal((MetalMaterial)value2);
		}
		foreach (object value3 in Enum.GetValues(typeof(HandMaterial)))
		{
			AddHandMaterial((HandMaterial)value3);
		}
		foreach (object value4 in Enum.GetValues(typeof(PolishMaterial)))
		{
			AddFingerPolishMaterial((PolishMaterial)value4);
		}
		foreach (object value5 in Enum.GetValues(typeof(ShirtMaterial)))
		{
			AddShirtMaterial((ShirtMaterial)value5);
		}
		AddUtilityMaterials();
		_isMaterialStoreLoaded = true;
	}

	public void ResetTryOnMaterials()
	{
		_isDisplayHandEnabled = true;
		_handObjects = null;
		_fingernailObjects = null;
		_shirtObjects = null;
		_necklaceChainObjects = null;
		CSUtil.Redraw();
	}

	public void SetMouseOverColor(Color color)
	{
		GetLoadedMaterialByName("MouseOver")?.UpdateMaterial(color);
	}

	public void SetPreviewColor(Color color)
	{
		GetLoadedMaterialByName("Highlighted")?.UpdateMaterial(color);
	}

	public void SetSelectionColor(Color color)
	{
		GetLoadedMaterialByName("Selected")?.UpdateMaterial(color);
	}

	protected override void CalculateBoundingBox(CalculateBoundingBoxEventArgs e)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		((DisplayConduit)this).CalculateBoundingBox(e);
		e.IncludeBoundingBox(new BoundingBox(-500.0, -500.0, -500.0, 500.0, 500.0, 500.0));
	}

	protected override void DrawOverlay(DrawEventArgs e)
	{
		((DisplayConduit)this).DrawOverlay(e);
		DrawOverlayObjects(e.Display);
	}

	[DllImport("opengl32.dll")]
	protected static extern void glClear(uint mask);

	protected override void PreDrawObjects(DrawEventArgs e)
	{
		((DisplayConduit)this).PreDrawObjects(e);
		if (BackgroundColor.HasValue)
		{
			e.Display.Draw2dRectangle(new Rectangle(0, 0, e.Display.FrameSize.Width, e.Display.FrameSize.Height), BackgroundColor.Value, 1, BackgroundColor.Value);
		}
		DrawBackObjects(e.Display);
		DrawObjects(e.Display);
	}

	private void AddFingerPolishMaterial(PolishMaterial polishMaterial)
	{
		Dictionary<string, Color> dictionary = new Dictionary<string, Color>
		{
			{
				"None",
				Color.Transparent
			},
			{
				"PolishWhite",
				Color.FromArgb(247, 245, 246)
			},
			{
				"PolishBlack",
				Color.FromArgb(5, 5, 5)
			},
			{
				"PolishBlueSlate",
				Color.FromArgb(74, 74, 112)
			},
			{
				"PolishCinnamon",
				Color.FromArgb(78, 1, 7)
			},
			{
				"PolishGreen",
				Color.FromArgb(174, 200, 129)
			},
			{
				"PolishLavender",
				Color.FromArgb(161, 127, 166)
			},
			{
				"PolishPeach",
				Color.FromArgb(233, 106, 87)
			},
			{
				"PolishPinkAgain",
				Color.FromArgb(209, 93, 106)
			},
			{
				"PolishPinkDark",
				Color.FromArgb(177, 81, 88)
			},
			{
				"PolishPinkUltra",
				Color.FromArgb(241, 217, 217)
			},
			{
				"PolishPurple",
				Color.FromArgb(123, 34, 102)
			},
			{
				"PolishPurpleDust",
				Color.FromArgb(105, 56, 85)
			},
			{
				"PolishRed",
				Color.FromArgb(152, 0, 15)
			},
			{
				"PolishRedChalk",
				Color.FromArgb(182, 32, 67)
			},
			{
				"PolishRose",
				Color.FromArgb(131, 61, 61)
			},
			{
				"PolishRoseLight",
				Color.FromArgb(212, 140, 144)
			},
			{
				"PolishSalmon",
				Color.FromArgb(255, 130, 114)
			},
			{
				"PolishSand",
				Color.FromArgb(240, 193, 139)
			},
			{
				"PolishStone",
				Color.FromArgb(156, 110, 95)
			}
		};
		string name = Enum.GetName(typeof(PolishMaterial), polishMaterial);
		if (name != null)
		{
			CSDisplayMaterial cSDisplayMaterial = new CSDisplayMaterial(dictionary[name], name);
			cSDisplayMaterial.Mat.Shine = 0.5 * Material.MaxShine;
			cSDisplayMaterial.Mat.Specular = Color.White;
			DisplayMaterials.Add(cSDisplayMaterial);
		}
	}

	private void AddGem(GemMaterial gemMaterial)
	{
		string name = Enum.GetName(typeof(GemMaterial), gemMaterial);
		DisplayMaterials.Add(new CSDisplayMaterial(CSApplicationHelper.Application.CSSystemPath + "\\Environment Maps\\Gems\\" + name + ".jpg", 0.99, CSApplicationHelper.Application.CSSystemPath + "\\Environment Maps\\Gems\\GemTrans1.jpg", 0.99, _materialOnColor, _materialOnSpecular, 0.0, 0.0, name, 0.1, 1.0));
	}

	private void AddHandMaterial(HandMaterial handMaterial)
	{
		string name = Enum.GetName(typeof(HandMaterial), handMaterial);
		if (name != null && name.Contains('_'))
		{
			string text = name.Substring(name.IndexOf('_') + 1);
			string text2 = name.Substring(0, name.IndexOf('_'));
			CSDisplayMaterial item = new CSDisplayMaterial(CSApplicationHelper.Application.CSLibraryPath + "\\Hands\\" + text2 + "\\" + text + ".jpg", name);
			DisplayMaterials.Add(item);
		}
	}

	private void AddMetal(MetalMaterial metalMaterial)
	{
		string name = Enum.GetName(typeof(MetalMaterial), metalMaterial);
		DisplayMaterials.Add(new CSDisplayMaterial(CSApplicationHelper.Application.CSSystemPath + "\\Environment Maps\\Metals\\" + name + ".jpg", 0.9, Color.Black, 0.99, 0.0, name));
	}

	private void AddShirtMaterial(ShirtMaterial shirtMaterial)
	{
		string name = Enum.GetName(typeof(ShirtMaterial), shirtMaterial);
		CSDisplayMaterial cSDisplayMaterial = new CSDisplayMaterial(CSApplicationHelper.Application.CSLibraryPath + "\\Hands\\" + name + ".jpg", name);
		cSDisplayMaterial.Mat.Shine = 0.0;
		cSDisplayMaterial.Mat.Specular = Color.White;
		DisplayMaterials.Add(cSDisplayMaterial);
	}

	private void AddUtilityMaterials()
	{
		DisplayMaterials.Add(new CSDisplayMaterial(Color.GhostWhite, "Undefined"));
		DisplayMaterials.Add(new CSDisplayMaterial(Color.FromArgb(255, 26, 100), "Highlighted"));
		DisplayMaterials.Add(new CSDisplayMaterial(Color.Firebrick, "Selected"));
		DisplayMaterials.Add(new CSDisplayMaterial(Color.Tomato, "MouseOver"));
		DisplayMaterials.Add(new CSDisplayMaterial(Color.DarkRed, "Finger"));
		DisplayMaterials.Add(new CSDisplayMaterial(Color.Orange, "Cutting"));
		DisplayMaterials.Add(new CSDisplayMaterial(Color.Goldenrod, "Creation"));
		DisplayMaterials.Add(new CSDisplayMaterial(Color.FromArgb(0, 130, 85), "Verification_MetalPrimary"));
		DisplayMaterials.Add(new CSDisplayMaterial(Color.FromArgb(123, 28, 168), "Verification_MetalSecondary"));
		DisplayMaterials.Add(new CSDisplayMaterial(Color.FromArgb(0, 60, 160), "Verification_GemCenter"));
		DisplayMaterials.Add(new CSDisplayMaterial(Color.FromArgb(65, 120, 205), "Verification_GemSide"));
		DisplayMaterials.Add(new CSDisplayMaterial(Color.FromArgb(130, 180, 250), "Verification_GemAccent"));
	}

	private void AssignAndDisplayTryOnObjects(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		if (!_isDisplayHandEnabled || _handObjects == null)
		{
			return;
		}
		Transform handTransform = GetHandTransform();
		displayPipeline.PushModelTransform(handTransform);
		for (int i = 0; i < _handObjects.OutputChannelCount; i++)
		{
			if (_handObjects.GetOutputChannelData(i) != null && _handObjects.GetOutputChannelData(i) is ICSDisplayObject iCSDisplayObject)
			{
				iCSDisplayObject.AssignCurrentMaterialGroup(CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.TryOnHand).FirstOrDefault());
				iCSDisplayObject.Display(displayPipeline, displayOptions);
			}
		}
		if (_fingernailObjects != null)
		{
			for (int j = 0; j < _fingernailObjects.OutputChannelCount; j++)
			{
				if (_fingernailObjects.GetOutputChannelData(j) != null && _fingernailObjects.GetOutputChannelData(j) is ICSDisplayObject iCSDisplayObject2)
				{
					iCSDisplayObject2.AssignCurrentMaterialGroup(CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.TryOnFingernails).FirstOrDefault());
					iCSDisplayObject2.Display(displayPipeline, displayOptions);
				}
			}
		}
		if (_shirtObjects != null)
		{
			for (int k = 0; k < _shirtObjects.OutputChannelCount; k++)
			{
				if (_shirtObjects.GetOutputChannelData(k) != null && _shirtObjects.GetOutputChannelData(k) is ICSDisplayObject iCSDisplayObject3)
				{
					iCSDisplayObject3.AssignCurrentMaterialGroup(CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.TryOnShirt).FirstOrDefault());
					iCSDisplayObject3.Display(displayPipeline, displayOptions);
				}
			}
		}
		displayPipeline.PopModelTransform();
	}

	private static void DrawBackObjects(DisplayPipeline displayPipeline)
	{
		if (CSApplicationHelper.Application.Document.DrawingObjectsEnabled)
		{
			CSValueHandleHelper.UpdateViewportControlHandlesBackGround(displayPipeline);
		}
	}

	private void DrawCSBlockObjects(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions)
	{
		IEnumerable<CSBlock> enumerable = ((!IsDrawingTargetedBlocksOnly) ? CSApplicationHelper.Application.Document.Graph.DisplayObjects() : (from csBlock in (from objectItem in CSApplicationHelper.Application.Document.Graph.AllObjects()
				where objectItem.InDisplay
				select objectItem).OfType<CSBlock>()
			where TargetedBlockInstanceIds.Contains(csBlock.InstanceGuid)
			select csBlock));
		foreach (CSBlock item in enumerable)
		{
			for (int num = 0; num < item.OutputChannelCount; num++)
			{
				(item.GetOutputChannelData(num) as ICSDisplayObject)?.Display(displayPipeline, displayOptions);
			}
		}
	}

	private void DrawOverlayObjects(DisplayPipeline pipeline)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		if (!CSApplicationHelper.Application.Document.DrawingObjectsEnabled)
		{
			return;
		}
		if (!string.IsNullOrWhiteSpace(OverlayMessage))
		{
			int num = default(int);
			int num2 = default(int);
			int num3 = default(int);
			int num4 = default(int);
			int num5 = default(int);
			int num6 = default(int);
			pipeline.Viewport.GetScreenPort(ref num, ref num2, ref num3, ref num4, ref num5, ref num6);
			Rectangle rectangle = pipeline.Measure2dText(OverlayMessage, Point2d.Origin, false, 0.0, 36, "Consolas");
			int num7 = (num2 - num) / 2 - rectangle.Width / 2;
			int num8 = (num3 - num4) / 2;
			pipeline.Draw2dText(OverlayMessage, Color.Black, new Point2d((double)num7, (double)num8), false, 36, "Consolas");
		}
		if (IsGemMapEnabled)
		{
			int yOffset = 25;
			int keyLeft = default(int);
			int num9 = default(int);
			int num10 = default(int);
			int keyTop = default(int);
			int num11 = default(int);
			int num12 = default(int);
			pipeline.Viewport.GetScreenPort(ref keyLeft, ref num9, ref num10, ref keyTop, ref num11, ref num12);
			DisplayPipeline displayPipeline = pipeline;
			List<CSMaterials.MaterialGroup> list = (from materialGroup in CS.MaterialGroup.MaterialGroupHelper.GetMaterialGroups()
				where materialGroup.IsGemMaterialGroup && materialGroup.MaterialGroupItems.Any()
				select materialGroup).ToList();
			int maxKeyItemWidth = 0;
			list.ForEach(delegate(CSMaterials.MaterialGroup materialGroup)
			{
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_008b: Unknown result type (might be due to invalid IL or missing references)
				string text = "Group " + CS.MaterialGroup.MaterialGroupHelper.GetAlphaIdentifierForMaterialGroup(materialGroup) + " : " + materialGroup.Name;
				Rectangle rectangle3 = displayPipeline.Measure2dText(text, Point2d.Origin, false, 0.0, 15, "Consolas");
				if (rectangle3.Width > maxKeyItemWidth)
				{
					maxKeyItemWidth = rectangle3.Width;
				}
				displayPipeline.Draw2dText(text, Color.Black, new Point2d((double)(keyLeft + 25), (double)(keyTop + (yOffset += 20))), false, 15, "Consolas");
			});
			Rectangle rectangle2 = new Rectangle(keyLeft + 15, keyTop + 25, maxKeyItemWidth + 25, yOffset - 15);
			pipeline.Draw2dRectangle(rectangle2, Color.Black, 1, Color.Transparent);
		}
		CSValueHandleHelper.UpdateViewportControlHandlesOverLayObjects(pipeline);
	}

	private CSDisplayMaterial GetLoadedMaterialByName(string name)
	{
		return DisplayMaterials.FirstOrDefault((CSDisplayMaterial material) => material.Name == name);
	}

	private void UpdateChainDisplayMaterials(DisplayPipeline displayPipeline, Transform flipRingTransform, CSOptionsDisplay displayOptions)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		if (_necklaceChainObjects == null)
		{
			return;
		}
		Transform chainPointTransform = GetChainPointTransform();
		displayPipeline.PushModelTransform(flipRingTransform * chainPointTransform);
		for (int i = 0; i < _necklaceChainObjects.OutputChannelCount; i++)
		{
			if (_necklaceChainObjects.GetOutputChannelData(i) != null && _necklaceChainObjects.GetOutputChannelData(i) is ICSDisplayObject iCSDisplayObject)
			{
				iCSDisplayObject.AssignCurrentMaterialGroup(CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.TryOnHand).FirstOrDefault());
				iCSDisplayObject.Display(displayPipeline, displayOptions);
			}
		}
		displayPipeline.PopModelTransform();
	}

	private void UpdateDisplayMaterialsUsingMaterialClassification(ref DisplayPipeline displayPipeline, MaterialClassification materialClassification, CSOptionsDisplay displayOptions, bool isAllObjects = false)
	{
		CSMaterials.MaterialGroup materialGroup = CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(materialClassification).FirstOrDefault();
		if (materialGroup == null)
		{
			return;
		}
		CSDisplayMaterial cSDisplayMaterialFromName = GetCSDisplayMaterialFromName(materialGroup.MaterialName);
		IEnumerable<CSBlock> enumerable;
		if (isAllObjects)
		{
			enumerable = (from objectItem in CSApplicationHelper.Application.Document.Graph.AllObjects()
				where objectItem.InDisplay
				select objectItem).OfType<CSBlock>();
			if (!CSApplicationHelper.Application.IsForceHideDisabled)
			{
				enumerable = enumerable.Where((CSBlock objectItem) => !objectItem.ForceHide);
			}
			if (CSApplicationHelper.IsCounterSketchStudio)
			{
				enumerable = enumerable.Where((CSBlock objectItem) => !objectItem.ForceHide);
			}
		}
		else
		{
			enumerable = CSApplicationHelper.Application.Document.Graph.DisplayObjects();
		}
		if (IsDrawingTargetedBlocksOnly)
		{
			enumerable = enumerable.Where((CSBlock csBlock) => TargetedBlockInstanceIds.Contains(csBlock.InstanceGuid));
		}
		foreach (CSBlock item in enumerable)
		{
			bool flag = CS.MaterialGroup.MaterialGroupHelper.CheckMaterialGroupForItemByClassification(materialClassification, $"{item.InstanceGuid}");
			for (int num = 0; num < item.OutputChannelCount; num++)
			{
				CSData outputChannelData = item.GetOutputChannelData(num);
				if (outputChannelData == null)
				{
					continue;
				}
				if (CS.MaterialGroup.MaterialGroupHelper.CheckMaterialGroupForItemByClassification(materialClassification, $"{item.InstanceGuid}:{num}") || flag)
				{
					(outputChannelData as ICSDisplayObject)?.Display(displayPipeline, displayOptions, cSDisplayMaterialFromName);
				}
				if (outputChannelData is CSDataArray cSDataArray)
				{
					CSDataArray flattenArray = cSDataArray.GetFlattenArray();
					for (int num2 = 0; num2 < flattenArray.GetArrayCount(); num2++)
					{
						CSData cSData = flattenArray.Array[num2];
						if (CS.MaterialGroup.MaterialGroupHelper.CheckMaterialGroupForItemByClassification(materialClassification, $"{item.InstanceGuid}:{num}:{num2}"))
						{
							(cSData as ICSDisplayObject)?.Display(displayPipeline, displayOptions, cSDisplayMaterialFromName);
						}
					}
				}
				else if (CS.MaterialGroup.MaterialGroupHelper.CheckMaterialGroupForItemByClassification(materialClassification, $"{item.InstanceGuid}:{num}:{0}"))
				{
					(outputChannelData as ICSDisplayObject)?.Display(displayPipeline, displayOptions, cSDisplayMaterialFromName);
				}
			}
		}
	}

	private void UpdateHighlightDisplayMaterials(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions)
	{
		if (displayPipeline.IsOpenGL)
		{
			glClear(256u);
		}
		CSApplicationHelper.Application.Conduit._curveLineSize = 3;
		CSApplicationHelper.Application.Conduit._isSelectionDisplayModeEnabled = true;
		UpdateDisplayMaterialsUsingMaterialClassification(ref displayPipeline, MaterialClassification.Highlighted, displayOptions, isAllObjects: true);
		CSApplicationHelper.Application.Conduit._isSelectionDisplayModeEnabled = false;
		CSApplicationHelper.Application.Conduit._curveLineSize = 1;
	}

	private void UpdateMouseOverDisplayMaterials(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions)
	{
		UpdateDisplayMaterialsUsingMaterialClassification(ref displayPipeline, MaterialClassification.MouseOver, displayOptions);
	}

	private void UpdateSelectedDisplayMaterials(DisplayPipeline displayPipeline, bool displayGemNumbers, CSOptionsDisplay displayOptions)
	{
		displayOptions.IsGemNumberDisplayEnabled = displayGemNumbers;
		UpdateDisplayMaterialsUsingMaterialClassification(ref displayPipeline, MaterialClassification.Selected, displayOptions);
		displayOptions.IsGemNumberDisplayEnabled = false;
	}

	~CSDisplayConduit()
	{
		try
		{
			DisplayMaterials.ToList().ForEach(delegate(CSDisplayMaterial csDisplayMaterial)
			{
				csDisplayMaterial.Dispose();
			});
		}
		finally
		{
			((object)this).Finalize();
		}
	}
}
