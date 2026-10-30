using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Media.Imaging;
using CS;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.CSObjects.CSTreeObjects;
using CS.CSValues;
using CS.CSValues.CSValueHandle;
using CS.CSValues.Interfaces;
using CS.CSValues.Obsolete;
using CS.Parametric;
using CSDatas;
using CSGemInstance.GemTableData;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Shared.Model.Objects;
using GV.CounterSketch.Shared.Pricing;
using GV.CounterSketch.Shared.Pricing.Enums;
using GV.CounterSketch.Shared.Utilities.Helpers;

namespace CounterSketchServer.Utilities.Helpers;

public static class UIControlHelper
{
	private static Guid _centerGemControlGuid;

	private static bool _insideAddAGem;

	public static StudioSetType ConvertGemFlagsToStudioSetType(CSGemData gemData)
	{
		return gemData.SettingType switch
		{
			GemSettingType.CH => StudioSetType.Channel, 
			GemSettingType.BZ => StudioSetType.Bezel, 
			_ => StudioSetType.Prong, 
		};
	}

	public static StudioGemShape ConvertGemStringToStudioGemShape(string gemShape)
	{
		string text = ((gemShape.Split('_').Count() > 1) ? gemShape.Split('_')[0] : gemShape);
		switch (text.ToLower())
		{
		case "baguette straight":
			return StudioGemShape.BaguetteStraight;
		case "baguette":
		case "baguettetapered":
		case "baguette tapered":
			return StudioGemShape.BaguetteTapered;
		case "cushion square":
			return StudioGemShape.CushionSquare;
		case "cushion":
			return StudioGemShape.Cushion;
		case "emerald":
			return StudioGemShape.Emerald;
		case "emerald square":
			return StudioGemShape.EmeraldSquare;
		case "half moon":
			return StudioGemShape.HalfMoon;
		case "heart":
			return StudioGemShape.Heart;
		case "marquise":
			return StudioGemShape.Marquise;
		case "oval":
			return StudioGemShape.Oval;
		case "pear":
			return StudioGemShape.Pear;
		case "princess":
			return StudioGemShape.Princess;
		case "radiant":
			return StudioGemShape.Radiant;
		case "round":
			return StudioGemShape.Round;
		case "triangle":
			return StudioGemShape.Triangle;
		case "trillion":
		case "trillion straight":
		case "trillion straight 2":
			return StudioGemShape.Trillion;
		case "trillion round":
		case "trillion curved":
		case "trillion curved 2":
			return StudioGemShape.TrillionRound;
		default:
			return StudioGemShape.Round;
		}
	}

	public static UIControl CreateUIControlFromGraphObject(CSObject graphObject)
	{
		CSPatchWireStart patchwireStartUsingCSBlock = CSUtil.GetPatchwireStartUsingCSBlock(graphObject as CSBlock);
		UIControl uiControl = new UIControl
		{
			CurrentUIControlStatus = GetUIControlStatusFromGraphObject(graphObject),
			ControlName = graphObject.Identifier,
			EnglishControlName = graphObject.Identifier,
			CategoryName = (((CSValueBase)graphObject).Categories.Any() ? ((CSValueBase)graphObject).Categories[0].CategoryName : string.Empty),
			EnglishCategoryName = (((CSValueBase)graphObject).Categories.Any() ? ((CSValueBase)graphObject).Categories[0].CategoryName : string.Empty),
			CategoryDeletable = ((_insideAddAGem || ((CSValueBase)graphObject).Categories.Any()) && ((CSValueBase)graphObject).Categories[0].Deletable),
			BlockInstanceGuid = graphObject.InstanceGuid,
			AssignedDisplayComponents = ((patchwireStartUsingCSBlock != null) ? CSUtil.DisplayComponentsToComponentsEnum(patchwireStartUsingCSBlock.DisplayComponent) : new List<ComponentsEnum>()),
			CurrentUIControlStackLocation = GetUIControlLocationFromGraphObject(graphObject),
			IsUIDisabled = CheckIfUIControlIsUIDisabled(graphObject)
		};
		string value = graphObject.UserData.FindValue("ExpandedSlider", "IsOn");
		bool result = false;
		bool.TryParse(value, out result);
		uiControl.HasMissingObject = result;
		if (_insideAddAGem)
		{
			uiControl.AssignedDisplayComponents = new List<ComponentsEnum>
			{
				ComponentsEnum.Ring,
				ComponentsEnum.Pendant,
				ComponentsEnum.Band1,
				ComponentsEnum.Band2
			};
		}
		CSBlock csBlock = graphObject as CSBlock;
		if (csBlock != null && CSApplicationHelper.Application.Document.Graph.ImportedCSGParts != null)
		{
			ImportGroup importGroup = CSApplicationHelper.Application.Document.Graph.ImportedCSGParts.FirstOrDefault((ImportGroup group) => group.BlockIdentifiers.Contains(csBlock.InstanceGuid));
			if (importGroup != null)
			{
				uiControl.ImportGroupIdentifier = importGroup.Identifier;
				uiControl.ImportGroupName = importGroup.Name;
				uiControl.ImportGroupPartGroupId = importGroup.PartGroupId;
			}
		}
		int.TryParse(graphObject.UserData.FindValue("UI", "SortIndex"), out var result2);
		uiControl.SortIndex = result2;
		IEnumerable<UserDataElement> enumerable = graphObject.UserData.ElementsThatMatchType("Spec");
		IEnumerable<UserDataElement> source = (enumerable as IList<UserDataElement>) ?? enumerable.ToList();
		if (source.Any())
		{
			uiControl.ControlSpecifier = source.First().Key;
			uiControl.ControlSpecifierValue = source.First().Value;
		}
		if (string.IsNullOrEmpty(uiControl.ControlSpecifier))
		{
			string[] array = uiControl.ControlName.Split(':');
			if (array.Length > 1)
			{
				uiControl.ControlSpecifier = array[0].Trim();
				uiControl.ControlSpecifierValue = array[1].Trim();
			}
		}
		if (graphObject is CSValueRange)
		{
			CSValueRange cSValueRange = graphObject as CSValueRange;
			uiControl.CurrentUIControlType = UIControlType.CSValueRange;
			uiControl.Minimum1 = cSValueRange.MinValue;
			uiControl.Value1 = cSValueRange.Value;
			uiControl.Maximum1 = cSValueRange.MaxValue;
			uiControl.Step1 = cSValueRange.Step;
		}
		if (graphObject is CSValueChooser)
		{
			CSValueChooser cSValueChooser = graphObject as CSValueChooser;
			uiControl.CurrentUIControlType = UIControlType.CSValueChooser;
			uiControl.DataItems = GetDataItemsFromCSValue(cSValueChooser);
			uiControl.Value1 = cSValueChooser.Value;
		}
		if (graphObject is CSValueRingSize)
		{
			CSValueRingSize cSValueRingSize = graphObject as CSValueRingSize;
			uiControl.CurrentUIControlType = UIControlType.CSValueRingSize;
			uiControl.Minimum1 = cSValueRingSize.MinValueIndex;
			uiControl.Value1 = cSValueRingSize.ValueIndex;
			uiControl.Maximum1 = cSValueRingSize.MaxValueIndex;
			uiControl.Step1 = 1.0;
		}
		if (graphObject is CSValueGem)
		{
			CSValueGem cSValueGem = graphObject as CSValueGem;
			string text = CSApplicationHelper.Application.Document.DocumentUserData.FindValue("Design Info", "Center Gem Stone Control Guid");
			if (!string.IsNullOrWhiteSpace(text))
			{
				Guid.TryParse(text, out _centerGemControlGuid);
			}
			if (_centerGemControlGuid != default(Guid))
			{
				uiControl.IsLargeGemEnabled = CSUtil.GetRange(GetPatchwireForUIControl(cSValueGem.InstanceGuid)).OfType<CSBlock>().Any((CSBlock _) => _.InstanceGuid == _centerGemControlGuid);
			}
			cSValueGem.IsAssignedCenter = uiControl.IsLargeGemEnabled;
			uiControl.CurrentUIControlType = UIControlType.CSValueGem;
			uiControl.DataItems = GetDataItemsFromCSValue(cSValueGem);
			uiControl.GemTableIndex = cSValueGem.GetTableIndex();
			uiControl.GemTableItemCount = cSValueGem.GetSizeTableCount() - 1;
			uiControl.Value1 = cSValueGem.Value;
			uiControl.GemSize = cSValueGem.Height;
			uiControl.GemHeight = cSValueGem.Height;
			uiControl.GemWidth = cSValueGem.Width;
			uiControl.GemDepth = cSValueGem.Depth;
			uiControl.GemPercentage = cSValueGem.Percentage;
			uiControl.GemTableSizeInfoList = cSValueGem.GetGemTableSizeInfoList();
			uiControl.GemCarat = cSValueGem.GemCarat;
			uiControl.IsLargeGemEnabled = cSValueGem.IsAssignedCenter;
			uiControl.IsCustomGemSize = cSValueGem.IsCustomGemSize;
		}
		if (graphObject is CSValueGemShape)
		{
			CSValueGemShape cSValueGemShape = graphObject as CSValueGemShape;
			uiControl.CurrentUIControlType = UIControlType.CSValueGemShape;
			uiControl.DataItems = GetDataItemsFromCSValue(cSValueGemShape);
			uiControl.GemTableItemCount = cSValueGemShape.GetSizeTableCount() - 1;
			uiControl.Value1 = cSValueGemShape.Value;
		}
		if (graphObject is CSValueGemSize)
		{
			CSValueGemSize cSValueGemSize = graphObject as CSValueGemSize;
			string text2 = CSApplicationHelper.Application.Document.DocumentUserData.FindValue("Design Info", "Center Gem Stone Control Guid");
			if (!string.IsNullOrWhiteSpace(text2))
			{
				Guid.TryParse(text2, out _centerGemControlGuid);
			}
			if (_centerGemControlGuid != default(Guid))
			{
				uiControl.IsLargeGemEnabled = CSUtil.GetRange(GetPatchwireForUIControl(cSValueGemSize.InstanceGuid)).OfType<CSBlock>().Any((CSBlock _) => _.InstanceGuid == _centerGemControlGuid);
			}
			cSValueGemSize.IsAssignedCenter = uiControl.IsLargeGemEnabled;
			uiControl.CurrentUIControlType = UIControlType.CSValueGemSize;
			uiControl.DataItems = GetDataItemsFromCSValue(cSValueGemSize);
			uiControl.GemTableIndex = cSValueGemSize.GetTableIndex();
			uiControl.GemTableItemCount = cSValueGemSize.GetSizeTableCount() - 1;
			uiControl.Value1 = cSValueGemSize.Value;
			uiControl.GemSize = cSValueGemSize.Height;
			uiControl.GemHeight = cSValueGemSize.Height;
			uiControl.GemWidth = cSValueGemSize.Width;
			uiControl.GemDepth = cSValueGemSize.Depth;
			uiControl.GemPercentage = cSValueGemSize.Percentage;
			uiControl.GemTableSizeInfoList = cSValueGemSize.GetGemTableSizeInfoList();
			uiControl.GemCarat = cSValueGemSize.GemCarat;
			uiControl.IsLargeGemEnabled = cSValueGemSize.IsAssignedCenter;
			uiControl.IsCustomGemSize = cSValueGemSize.IsCustomGemSize;
		}
		if (graphObject is CSValueDualRange)
		{
			CSValueDualRange cSValueDualRange = graphObject as CSValueDualRange;
			uiControl.CurrentUIControlType = UIControlType.CSValueDualRange;
			uiControl.Minimum1 = cSValueDualRange.MinValue1;
			uiControl.Value1 = cSValueDualRange.Value;
			uiControl.Maximum1 = cSValueDualRange.MaxValue1;
			uiControl.Minimum2 = cSValueDualRange.MinValue2;
			uiControl.Value2 = cSValueDualRange.Value2;
			uiControl.Maximum2 = cSValueDualRange.MaxValue2;
			uiControl.Step1 = cSValueDualRange.Step;
		}
		if (graphObject is CSValueOnOff1)
		{
			CSValueOnOff1 cSValueOnOff = graphObject as CSValueOnOff1;
			uiControl.CurrentUIControlType = UIControlType.CSValueOnOff;
			uiControl.Value1 = (cSValueOnOff.Value ? 1.0 : 0.0);
		}
		if (graphObject is CSValueOnOff)
		{
			CSValueOnOff cSValueOnOff2 = graphObject as CSValueOnOff;
			uiControl.CurrentUIControlType = UIControlType.CSValueOnOff;
			uiControl.Value1 = cSValueOnOff2.Value;
		}
		if (graphObject is CSValueList)
		{
			CSValueList cSValueList = graphObject as CSValueList;
			uiControl.CurrentUIControlType = UIControlType.CSValueList;
			uiControl.ListCount = cSValueList.GetListLength() - 1;
			uiControl.ListItems = cSValueList.List;
			uiControl.ListIndex = cSValueList.ListIndex;
			uiControl.ListAsText = cSValueList.IsTextOnly;
			string valueListImageModifier = null;
			CSDataArray cSDataArray = cSValueList.InGem?.GetFlattenArray();
			if (cSDataArray != null && cSDataArray.GetArrayCount() > 0 && cSDataArray.GetArrayItem(0) is CSGemData cSGemData)
			{
				GemTableShape valueFromDescription = GemTableHelper.GetValueFromDescription(cSGemData.GemShape, GemTableShape.Unknown);
				if (valueFromDescription != GemTableShape.Unknown)
				{
					valueListImageModifier = Enum.GetName(typeof(GemTableShape), valueFromDescription);
				}
			}
			uiControl.ValueListImageModifier = valueListImageModifier;
		}
		if (graphObject is CSValueFontList)
		{
			CSValueFontList cSValueFontList = graphObject as CSValueFontList;
			IEnumerable<string> source2 = cSValueFontList.List.Select((GV.CounterSketch.Shared.Font _) => EnumHelper.GetEnumDescriptionFromValue(_));
			List<DataItem> fontDataItems = new List<DataItem>();
			source2.ToList().ForEach(delegate(string _)
			{
				fontDataItems.Add(new DataItem
				{
					Identifier = _
				});
			});
			uiControl.CurrentUIControlType = UIControlType.CSValueFontList;
			uiControl.ListCount = cSValueFontList.GetListLength() - 1;
			uiControl.DataItems = fontDataItems;
			uiControl.ListIndex = cSValueFontList.ListIndex;
		}
		if (graphObject is CSValueHeadStyleList)
		{
			CSValueHeadStyleList cSValueHeadStyleList = graphObject as CSValueHeadStyleList;
			uiControl.CurrentUIControlType = UIControlType.CSValueHeadStyleList;
			uiControl.ListCount = cSValueHeadStyleList.GetListLength() - 1;
			uiControl.HeadStyleListItems = cSValueHeadStyleList.List;
			uiControl.ListIndex = cSValueHeadStyleList.SelectedIndex;
			uiControl.ListAsText = false;
			string valueListImageModifier2 = null;
			CSDataArray cSDataArray2 = cSValueHeadStyleList.InGem?.GetFlattenArray();
			if (cSDataArray2 != null && cSDataArray2.GetArrayCount() > 0 && cSDataArray2.GetArrayItem(0) is CSGemData cSGemData2)
			{
				GemTableShape valueFromDescription2 = GemTableHelper.GetValueFromDescription(cSGemData2.GemShape, GemTableShape.Unknown);
				if (valueFromDescription2 != GemTableShape.Unknown)
				{
					valueListImageModifier2 = Enum.GetName(typeof(GemTableShape), valueFromDescription2);
				}
			}
			uiControl.ValueListImageModifier = valueListImageModifier2;
		}
		if (graphObject is CSValueInput1)
		{
			CSValueInput1 cSValueInput = graphObject as CSValueInput1;
			uiControl.CurrentUIControlType = UIControlType.CSValueInput;
			uiControl.Value1 = cSValueInput.Value;
		}
		if (graphObject is CSValueInput)
		{
			CSValueInput cSValueInput2 = graphObject as CSValueInput;
			uiControl.CurrentUIControlType = UIControlType.CSValueInput;
			uiControl.ValueGeneric = cSValueInput2.Value;
		}
		if (graphObject is CSValueOutput)
		{
			CSValueOutput cSValueOutput = graphObject as CSValueOutput;
			uiControl.CurrentUIControlType = UIControlType.CSValueOutput;
			uiControl.DisplayText = cSValueOutput.Value;
		}
		if (graphObject is CSValueGemOutput)
		{
			CSValueGemOutput obj = graphObject as CSValueGemOutput;
			uiControl.CurrentUIControlType = UIControlType.CSValueGemOutput;
			uiControl.GemList = new List<StudioGem>();
			int gemIndex = 0;
			obj.Gems.ForEach(delegate(CSGemData gemData)
			{
				uiControl.GemList.Add(new StudioGem
				{
					Quantity = 1,
					Length = Convert.ToDecimal(Math.Round(gemData.GemYSize, 3)),
					Width = Convert.ToDecimal(Math.Round(gemData.GemXSize, 3)),
					Height = Convert.ToDecimal(Math.Round(gemData.GemZSize, 3)),
					ItemNumber = null,
					GemIndex = gemIndex,
					GemNumber = gemData.GemNumber,
					GemTypeOption = StudioGemType.Genuine,
					GemShapeOption = ConvertGemStringToStudioGemShape(gemData.GemShape),
					SetTypeOption = ConvertGemFlagsToStudioSetType(gemData),
					Weight = Math.Round(Convert.ToDecimal(gemData.FFactor * 3.52 * gemData.GemXSize * gemData.GemYSize * gemData.GemZSize), 4, MidpointRounding.ToEven),
					GemMaterialOption = GemMaterial.Diamond
				});
				gemIndex++;
			});
		}
		if (graphObject is CSValueXFormObject)
		{
			CSValueXFormObject cSValueXFormObject = graphObject as CSValueXFormObject;
			uiControl.CurrentUIControlType = UIControlType.CSValueXFormObject;
			uiControl.Value1 = cSValueXFormObject.FlipMode;
			uiControl.Value2 = cSValueXFormObject.MirrorMode;
		}
		graphObject.FinishedUI();
		return uiControl;
	}

	public static CSObject GetCSObjectByInstanceGuid(Guid blockInstanceGuid)
	{
		return CSApplicationHelper.Application.Document.Graph.AllObjects().FirstOrDefault((CSObject graphObject) => graphObject.InstanceGuid == blockInstanceGuid);
	}

	public static CSPatchWireStart GetPatchwireForUIControl(Guid blockInstanceGuid)
	{
		if (GetCSObjectByInstanceGuid(blockInstanceGuid) is CSBlock block)
		{
			return CSUtil.GetPatchwireStartUsingCSBlock(block);
		}
		return null;
	}

	public static List<UIControl> GetUIControlsFromGraphObjects()
	{
		_insideAddAGem = false;
		List<UIControl> uiControls = new List<UIControl>();
		CSApplicationHelper.Application.Document.Graph.AllObjectsList.ForEach(delegate(CSObject graphObject)
		{
			if (CSApplicationHelper.Application.Document.Graph.ImportedCSGParts == null || !CSApplicationHelper.Application.Document.Graph.ImportedCSGParts.Any((ImportGroup group) => group.BlockIdentifiers.Contains(graphObject.InstanceGuid)))
			{
				if (graphObject is CSPatchWireStart)
				{
					switch (graphObject.Identifier)
					{
					case "Studio - Gem":
					case "Studio - Gem w/ Bezel":
					case "Studio - Gem/Head":
					case "Studio - Gem w/ Custom Head":
					case "Add-A-Part":
						_insideAddAGem = true;
						break;
					default:
						_insideAddAGem = false;
						break;
					}
				}
				else
				{
					if (_insideAddAGem)
					{
						if (!graphObject.UIVisible)
						{
							return;
						}
					}
					else if (!graphObject.StudioVisible && !graphObject.StudioVisibleAdvanced && !graphObject.StudioVisibleDesignOption)
					{
						return;
					}
					if (graphObject is CSValueBase cSValueBase && cSValueBase.Categories.Any() && !cSValueBase.Deleted)
					{
						UIControl item = CreateUIControlFromGraphObject(graphObject);
						uiControls.Add(item);
					}
				}
			}
		});
		List<UIControl> list = new List<UIControl>();
		foreach (CSUICategory item2 in CSApplicationHelper.Application.Document.Graph.CategoryTable.Categories.OrderBy((CSUICategory category) => category.Index))
		{
			CSUICategory categoryLocal = item2;
			list.AddRange(uiControls.Where((UIControl controls) => controls.CategoryName == categoryLocal.CategoryName && categoryLocal.ImportGroupIdentifier == Guid.Empty));
		}
		return list;
	}

	public static void RefreshUIControlList()
	{
		if (CSApplicationHelper.Application.Document != null)
		{
			GetUIControlsFromGraphObjects();
		}
	}

	public static void UpdateCenterGemControlIdentifier(Guid centerGemControlGuid, int? centerGemGraphIndex)
	{
		if (CSApplicationHelper.Application.Document.Graph.AllObjectsList.All((CSObject _) => _.InstanceGuid != centerGemControlGuid) && centerGemGraphIndex.HasValue)
		{
			centerGemControlGuid = CSApplicationHelper.Application.Document.Graph.AllObjectsList.ElementAt(centerGemGraphIndex.GetValueOrDefault()).InstanceGuid;
		}
		_centerGemControlGuid = centerGemControlGuid;
		if (_centerGemControlGuid != default(Guid))
		{
			CSApplicationHelper.Application.Document.DocumentUserData.AddKey("Design Info", "Center Gem Stone Control Guid", _centerGemControlGuid.ToString());
		}
	}

	public static void UpdateCenterGemControlIdentifierForImportGroup(ImportGroup importGroup, int? centerGemGraphIndex)
	{
		if (centerGemGraphIndex.HasValue)
		{
			Guid guid = importGroup.BlockIdentifiers.ElementAt(centerGemGraphIndex.GetValueOrDefault());
			if (guid != default(Guid))
			{
				importGroup.CenterGemControlGuid = guid;
			}
		}
	}

	public static List<UIControl> UpdateUIControl(UIControl uiControl)
	{
		UpdateGraphObjectFromUIControl(uiControl, quickUpdateOnly: false);
		if (uiControl.ImportGroupIdentifier != Guid.Empty)
		{
			if (GetCSObjectByInstanceGuid(uiControl.BlockInstanceGuid) is CSBlock block)
			{
				CSPatchWireStart patchwireStartUsingCSBlock = CSUtil.GetPatchwireStartUsingCSBlock(block);
				if (patchwireStartUsingCSBlock != null)
				{
					ImportGroupHelper.UpdateFreehandStackControlsFromPatchwireStart(patchwireStartUsingCSBlock);
				}
			}
			return null;
		}
		return GetUIControlsFromGraphObjects();
	}

	public static List<UIControl> UpdateUIControlQuick(UIControl uiControl)
	{
		UpdateGraphObjectFromUIControl(uiControl, quickUpdateOnly: true);
		return null;
	}

	public static void UpdateValueChooserFromUI(Guid blockInstanceGuid)
	{
		if (GetCSObjectByInstanceGuid(blockInstanceGuid) is CSValueChooser cSValueChooser)
		{
			cSValueChooser.SetValueFromUI(cSValueChooser.Value);
		}
	}

	private static bool CheckIfUIControlIsUIDisabled(CSObject graphObject)
	{
		bool result = false;
		if (graphObject is CSValueBase cSValueBase)
		{
			result = cSValueBase.IsUIDisabled;
		}
		return result;
	}

	private static List<DataItem> GetDataItemsFromCSValue<T>(T csValue)
	{
		int num = 0;
		if (csValue is ICSChooser)
		{
			num = (csValue as ICSChooser).ChooserCount();
		}
		List<DataItem> list = new List<DataItem>();
		for (int i = 0; i < num; i++)
		{
			DataItem dataItem = new DataItem();
			CSData cSData = new CSData();
			if (csValue is CSValueChooser)
			{
				CSValueChooser obj = csValue as CSValueChooser;
				cSData = obj.GetChooserObject(i);
				dataItem.PolyPointCollection = cSData.GetPolyline(1.0);
				if (obj.IsInputChannelImageOverridden(i) || dataItem.PolyPointCollection == null)
				{
					dataItem.HasImage = true;
				}
			}
			else if (csValue is CSValueGem)
			{
				CSValueGem cSValueGem = csValue as CSValueGem;
				cSData = cSValueGem.GetChooserObject(i);
				dataItem.Identifier = cSValueGem.GetChooserObjectIdentifier(i);
				if (cSValueGem.GetChooserObject(i) is CSGemData cSGemData)
				{
					dataItem.VectorResourceName = cSGemData.GemShape;
				}
			}
			else if (csValue is CSValueGemShape)
			{
				CSValueGemShape cSValueGemShape = csValue as CSValueGemShape;
				cSData = cSValueGemShape.GetChooserObject(i);
				dataItem.Identifier = cSValueGemShape.GetChooserObjectIdentifier(i);
				if (cSValueGemShape.GetChooserObject(i) is CSGemData cSGemData2)
				{
					dataItem.VectorResourceName = cSGemData2.GemShape;
				}
			}
			if (dataItem.PolyPointCollection == null)
			{
				dataItem.PolyPointCollection = cSData.GetPolyline(1.0);
			}
			if (dataItem.HasImage || (dataItem.PolyPointCollection == null && dataItem.VectorResourceName == null))
			{
				BitmapSource bitmapSource = CSUtil.GetBitmapSource(cSData.GetImage(50, 50, Color.FromArgb(Color.Silver.A, Color.Silver.R, Color.Silver.G, Color.Silver.B)) as Bitmap);
				dataItem.BitmapSourceByteArrayInfo = new BitmapSourceByteArrayInfo
				{
					ItemImageHeight = bitmapSource.PixelHeight,
					ItemImageWidth = bitmapSource.PixelWidth,
					ItemImageStride = bitmapSource.PixelWidth * ((bitmapSource.Format.BitsPerPixel + 7) / 8)
				};
				dataItem.BitmapSourceByteArrayInfo.ItemImageSourceByteArray = new byte[dataItem.BitmapSourceByteArrayInfo.ItemImageHeight * dataItem.BitmapSourceByteArrayInfo.ItemImageStride];
				bitmapSource.CopyPixels(dataItem.BitmapSourceByteArrayInfo.ItemImageSourceByteArray, dataItem.BitmapSourceByteArrayInfo.ItemImageStride, 0);
				dataItem.IsItemDisabled = cSData.Disabled;
			}
			list.Add(dataItem);
		}
		return list;
	}

	private static UIControlStackLocation GetUIControlLocationFromGraphObject(CSObject graphObject)
	{
		if (graphObject.StudioVisibleAdvanced)
		{
			return UIControlStackLocation.Advanced;
		}
		if (graphObject.StudioVisibleDesignOption)
		{
			return UIControlStackLocation.DesignOption;
		}
		return UIControlStackLocation.Normal;
	}

	private static UIControlStatus GetUIControlStatusFromGraphObject(CSObject csObject)
	{
		if (csObject.Deleted)
		{
			return UIControlStatus.Deleted;
		}
		if (csObject.NotCreatedYet)
		{
			return UIControlStatus.New;
		}
		if (csObject.UIChanged)
		{
			return UIControlStatus.Changed;
		}
		return UIControlStatus.Normal;
	}

	private static void UpdateGraphObjectFromUIControl(UIControl uiControl, bool quickUpdateOnly)
	{
		CSObject cSObjectByInstanceGuid = GetCSObjectByInstanceGuid(uiControl.BlockInstanceGuid);
		switch (uiControl.CurrentUIControlType)
		{
		case UIControlType.CSValueRange:
			if (cSObjectByInstanceGuid is CSValueRange cSValueRange)
			{
				cSValueRange.SetMinFromUI(uiControl.Minimum1);
				cSValueRange.SetMaxFromUI(uiControl.Maximum1);
				cSValueRange.SetValueFromUI(uiControl.Value1, quickUpdateOnly);
			}
			break;
		case UIControlType.CSValueRingSize:
			(cSObjectByInstanceGuid as CSValueRingSize)?.SetValueFromUI(uiControl.Value1, quickUpdateOnly);
			break;
		case UIControlType.CSValueGem:
			if (cSObjectByInstanceGuid is CSValueGem cSValueGem)
			{
				if (uiControl.Value1 != cSValueGem.Value)
				{
					cSValueGem.SetValueFromUI(uiControl.Value1);
				}
				else if (!uiControl.IsCustomGemSize)
				{
					cSValueGem.SetSizeFromTableUI(uiControl.GemTableIndex, quickUpdateOnly);
				}
				else if (uiControl.IsAssignedFromStone)
				{
					cSValueGem.SetCombinedGemSizeFromUI(uiControl.GemHeight, uiControl.GemWidth, uiControl.GemDepth);
				}
				else if (uiControl.GemDepth != cSValueGem.Depth)
				{
					cSValueGem.SetDepthFromUI(uiControl.GemDepth);
				}
				else if (uiControl.GemHeight != cSValueGem.Height)
				{
					cSValueGem.SetHeightFromUI(uiControl.GemHeight);
				}
				else if (uiControl.GemPercentage != cSValueGem.Percentage)
				{
					cSValueGem.SetPercentageFromUI(uiControl.GemPercentage);
				}
				else if (uiControl.GemWidth != cSValueGem.Width)
				{
					cSValueGem.SetWidthFromUI(uiControl.GemWidth);
				}
			}
			break;
		case UIControlType.CSValueGemShape:
			if (cSObjectByInstanceGuid is CSValueGemShape cSValueGemShape && uiControl.Value1 != cSValueGemShape.Value)
			{
				cSValueGemShape.SetValueFromUI(uiControl.Value1);
			}
			break;
		case UIControlType.CSValueGemSize:
			if (cSObjectByInstanceGuid is CSValueGemSize cSValueGemSize)
			{
				if (!uiControl.IsCustomGemSize || cSValueGemSize.IsCSGemDataShapeChanged())
				{
					cSValueGemSize.SetSizeFromTableUI(uiControl.GemTableIndex, quickUpdateOnly);
				}
				else if (uiControl.IsAssignedFromStone)
				{
					cSValueGemSize.SetCombinedGemSizeFromUI(uiControl.GemHeight, uiControl.GemWidth, uiControl.GemDepth);
				}
				else if (uiControl.GemDepth != cSValueGemSize.Depth)
				{
					cSValueGemSize.SetDepthFromUI(uiControl.GemDepth);
				}
				else if (uiControl.GemHeight != cSValueGemSize.Height)
				{
					cSValueGemSize.SetHeightFromUI(uiControl.GemHeight);
				}
				else if (uiControl.GemPercentage != cSValueGemSize.Percentage)
				{
					cSValueGemSize.SetPercentageFromUI(uiControl.GemPercentage);
				}
				else if (uiControl.GemWidth != cSValueGemSize.Width)
				{
					cSValueGemSize.SetWidthFromUI(uiControl.GemWidth);
				}
			}
			break;
		case UIControlType.CSValueChooser:
			(cSObjectByInstanceGuid as CSValueChooser)?.SetValueFromUI(uiControl.Value1);
			break;
		case UIControlType.CSValueOnOff:
			if (cSObjectByInstanceGuid is CSValueOnOff1)
			{
				(cSObjectByInstanceGuid as CSValueOnOff1).SetValueFromUI(uiControl.Value1 > 0.5, quickUpdateOnly);
			}
			else if (cSObjectByInstanceGuid is CSValueOnOff)
			{
				(cSObjectByInstanceGuid as CSValueOnOff).SetValueFromUI(uiControl.Value1, quickUpdateOnly);
			}
			break;
		case UIControlType.CSValueDualRange:
			(cSObjectByInstanceGuid as CSValueDualRange)?.SetValueFromUI(uiControl.Value1, uiControl.Value2, quickUpdateOnly);
			break;
		case UIControlType.CSValueList:
			(cSObjectByInstanceGuid as CSValueList)?.SetListIndexFromUI(uiControl.ListIndex, quickUpdateOnly);
			break;
		case UIControlType.CSValueHeadStyleList:
			(cSObjectByInstanceGuid as CSValueHeadStyleList)?.SetListIndexFromUI(uiControl.ListIndex, quickUpdateOnly);
			break;
		case UIControlType.CSValueInput:
			if (cSObjectByInstanceGuid is CSValueInput1)
			{
				(cSObjectByInstanceGuid as CSValueInput1).SetValueFromUI(uiControl.Value1, quickUpdateOnly);
			}
			else if (cSObjectByInstanceGuid is CSValueInput)
			{
				(cSObjectByInstanceGuid as CSValueInput).SetValueFromUI(uiControl.ValueGeneric.ToString(), quickUpdateOnly);
			}
			break;
		case UIControlType.CSValueXFormObject:
			(cSObjectByInstanceGuid as CSValueXFormObject)?.SetModesFromUI((int)uiControl.Value1, (int)uiControl.Value2, mouseDown: false);
			break;
		case UIControlType.CSValueFontList:
			(cSObjectByInstanceGuid as CSValueFontList)?.SetListIndexFromUI(uiControl.ListIndex, quickUpdateOnly);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case UIControlType.CSStringBlock:
			break;
		}
	}
}
