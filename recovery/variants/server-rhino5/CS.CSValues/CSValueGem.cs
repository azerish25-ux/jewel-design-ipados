using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.CSObjects.CSTreeObjects;
using CS.CSValues.Interfaces;
using CS.JSonDoc;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using CSGemInstance.GemTableData;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Shared.Model.Objects;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CS.CSValues;

[CSBlock("ValueGem", "{29BA96A4-2B1B-4dfa-B0D9-8B635D2EDE90}")]
public sealed class CSValueGem : CSValue, ICSChooser, ICSValueGemSize
{
	public readonly CSInputChannel InGems;

	public readonly CSInputChannel InOnOff;

	public readonly CSOutputChannel OutGem;

	public double Depth;

	public double Height;

	public double Percentage;

	public double Width;

	public List<GemTableSizeExclusion> GemTableSizeExclusions;

	private bool _hasCreatedUndoTransaction;

	private int _currentTableIndex;

	private double _maxX;

	private double _maxY;

	private double _minX;

	private double _minY;

	private List<GemMinMaxInfo> GemMinMaxInfos;

	public double GemCarat
	{
		get
		{
			if (GetChooserSource() == null)
			{
				return 0.0;
			}
			CSGemData currentCSGemData = GetCurrentCSGemData();
			if (currentCSGemData == null)
			{
				return 0.0;
			}
			return Width * Height * Depth * 3.52 * currentCSGemData.FFactor;
		}
	}

	public string GemShape => GetCurrentCSGemData()?.GemShape;

	public string GemType => GetCurrentCSGemData()?.GemType;

	public bool IsAssignedCenter { get; set; }

	public GemSettingType MountingType
	{
		get
		{
			string value = base.UserData.FindValue("CSValueGem", "Mounting");
			if (string.IsNullOrWhiteSpace(value))
			{
				return GemSettingType.XX;
			}
			if (!Enum.TryParse<GemSettingType>(value, out var result))
			{
				return GemSettingType.XX;
			}
			return result;
		}
		set
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			Change();
			base.UserData.AddKey("CSValueGem", "Mounting", value.ToString());
			CSApplicationHelper.Application.Document.Graph.Generate();
		}
	}

	public bool IsCustomGemSize => CheckGemTableForCustomSize();

	public double ValueMaxX => GetShapeSpecificGemMinMaxInfo()?.MaxX ?? GlobalValueMaxX;

	public double ValueMaxY => GetShapeSpecificGemMinMaxInfo()?.MaxY ?? GlobalValueMaxY;

	public double ValueMinX => GetShapeSpecificGemMinMaxInfo()?.MinX ?? GlobalValueMinX;

	public double ValueMinY => GetShapeSpecificGemMinMaxInfo()?.MinY ?? GlobalValueMinY;

	public double GlobalValueMaxX
	{
		get
		{
			GemSize gemSize = GetCurrentCSGemData()?.Sizes.OrderByDescending((GemSize gemSize2) => gemSize2.X).FirstOrDefault();
			if (gemSize != null)
			{
				if (!(Math.Abs(_maxX) < 0.001))
				{
					return _maxX;
				}
				return gemSize.X;
			}
			return _maxX;
		}
	}

	public double GlobalValueMaxY
	{
		get
		{
			GemSize gemSize = GetCurrentCSGemData()?.Sizes.OrderByDescending((GemSize gemSize2) => gemSize2.Y).FirstOrDefault();
			if (gemSize != null)
			{
				if (!(Math.Abs(_maxY) < 0.001))
				{
					return _maxY;
				}
				return gemSize.Y;
			}
			return _maxY;
		}
	}

	public double GlobalValueMinX
	{
		get
		{
			GemSize gemSize = GetCurrentCSGemData()?.Sizes.OrderBy((GemSize gemSize2) => gemSize2.X).FirstOrDefault();
			if (gemSize != null)
			{
				if (!(Math.Abs(_minX) < 0.001))
				{
					return _minX;
				}
				return gemSize.X;
			}
			return _minX;
		}
	}

	public double GlobalValueMinY
	{
		get
		{
			GemSize gemSize = GetCurrentCSGemData()?.Sizes.OrderBy((GemSize gemSize2) => gemSize2.Y).FirstOrDefault();
			if (gemSize != null)
			{
				if (!(Math.Abs(_minY) < 0.001))
				{
					return _minY;
				}
				return gemSize.Y;
			}
			return _minY;
		}
	}

	public CSValueGem(string identifier)
		: base(identifier)
	{
		GemMinMaxInfos = new List<GemMinMaxInfo>();
		GemTableSizeExclusions = new List<GemTableSizeExclusion>();
		InGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Any, ChannelFlag.NotRequired);
		InOnOff = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		base.InIsUIDisabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public CSGemData GetCurrentCSGemData()
	{
		CSBlock chooserSource = GetChooserSource();
		if (chooserSource == null)
		{
			return null;
		}
		CSGemData cSGemData = chooserSource.GetOutputChannelData((int)base.Value) as CSGemData;
		if (cSGemData == null)
		{
			return null;
		}
		if (cSGemData.Def == null)
		{
			return cSGemData;
		}
		if (cSGemData.Def.IsGeneratedGem)
		{
			cSGemData = new CSGemData(cSGemData.Def.GenerateCSGemData(_currentTableIndex))
			{
				CurrentSizeTableIndex = _currentTableIndex
			};
		}
		return cSGemData;
	}

	public int GetTableIndex()
	{
		CSGemData currentCSGemData = GetCurrentCSGemData();
		if (currentCSGemData == null)
		{
			return 0;
		}
		if (currentCSGemData.Def == null)
		{
			return 0;
		}
		int result = 0;
		double num = double.MaxValue;
		List<GemSize> list = ValidSizes(currentCSGemData.Sizes);
		for (int i = 0; i < list.Count; i++)
		{
			GemSize gemSize = list[i];
			double num2 = Math.Abs(Width - gemSize.X) + Math.Abs(Height - gemSize.Y);
			if (num2 < num)
			{
				num = num2;
				result = i;
			}
		}
		return result;
	}

	private bool CheckGemTableForCustomSize()
	{
		if (GetChooserSource() == null)
		{
			return true;
		}
		CSGemData currentCSGemData = GetCurrentCSGemData();
		if (currentCSGemData == null)
		{
			return true;
		}
		if (currentCSGemData.Def == null)
		{
			return true;
		}
		GemTableSizeExclusion shapeSpecificGemTableSizeExclusions = GetShapeSpecificGemTableSizeExclusions();
		foreach (GemSize size in currentCSGemData.Sizes)
		{
			if (size.X <= ValueMaxX && size.X >= ValueMinX && size.Y <= ValueMaxY && size.Y >= ValueMinY && (shapeSpecificGemTableSizeExclusions == null || !shapeSpecificGemTableSizeExclusions.ExcludedGemTableSizeIndexes.Contains(currentCSGemData.Sizes.ToList().IndexOf(size))) && Math.Abs(Width - size.X) < 0.01 && Math.Abs(Height - size.Y) < 0.01 && Math.Abs(Depth - size.Z) < 0.1)
			{
				_currentTableIndex = currentCSGemData.Sizes.IndexOf(size);
				return false;
			}
		}
		return true;
	}

	private GemTableSizeExclusion GetShapeSpecificGemTableSizeExclusions()
	{
		CSGemData csGemData = GetCurrentCSGemData();
		GemTableSizeExclusion result = null;
		if (csGemData != null && csGemData.Def != null && GemTableSizeExclusions != null)
		{
			result = GemTableSizeExclusions.FirstOrDefault((GemTableSizeExclusion gemExclusion) => gemExclusion.GemShape.Replace(" ", "") == csGemData.Def.Shape.Replace(" ", "") && gemExclusion.GemType == csGemData.Def.Type);
		}
		return result;
	}

	public int ChooserCount()
	{
		return GetChooserSource()?.OutputChannelCount ?? 0;
	}

	public bool DeleteUnused()
	{
		CSBlock chooserSource = GetChooserSource();
		if (chooserSource == null)
		{
			return true;
		}
		int num = ChooserCount();
		int num2 = (int)base.Value;
		if (num > 1 && !base.StudioVisible && !base.StudioVisibleAdvanced && !base.StudioVisibleDesignOption && base.ParentPatchWire != null)
		{
			int num3 = CSApplicationHelper.Application.Document.Graph.GraphList.FindIndex((CSObject check) => base.ParentPatchWire.Equals(check));
			if (num3 < 0)
			{
				return false;
			}
			if (num3 == int.MaxValue)
			{
				return false;
			}
			int num4 = CSApplicationHelper.Application.Document.Graph.GraphList.Count - 1;
			if (num3 >= CSApplicationHelper.Application.Document.Graph.GraphList.Count)
			{
				return false;
			}
			for (int num5 = num3 + 1; num5 < CSApplicationHelper.Application.Document.Graph.GraphList.Count; num5++)
			{
				if (CSApplicationHelper.Application.Document.Graph.GraphList[num5] is CSPatchWireStart)
				{
					num4 = num5 - 1;
					break;
				}
			}
			CSObject outputChannelBlock = InGems.OutputChannelBlock;
			List<CSValueGem> list = new List<CSValueGem>();
			if (base.InOverride.OutputChannelBlock != null)
			{
				return false;
			}
			if (outputChannelBlock == null)
			{
				return false;
			}
			if (outputChannelBlock.ParentPatchWire != base.ParentPatchWire)
			{
				return false;
			}
			for (int num6 = num3 + 1; num6 < num4; num6++)
			{
				if (this == CSApplicationHelper.Application.Document.Graph.GraphList[num6] || !(CSApplicationHelper.Application.Document.Graph.GraphList[num6] is CSValueGem))
				{
					continue;
				}
				CSValueGem cSValueGem = CSApplicationHelper.Application.Document.Graph.GraphList[num6] as CSValueGem;
				if (cSValueGem.InGems.OutputChannelBlock == outputChannelBlock)
				{
					if (cSValueGem.StudioVisible || cSValueGem.StudioVisibleAdvanced || cSValueGem.StudioVisibleDesignOption)
					{
						return false;
					}
					list.Add(cSValueGem);
				}
			}
			if (list.Count > 0)
			{
				return false;
			}
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("DeleteUnused");
			if (num2 != 0)
			{
				SetValue(0.0);
			}
			for (int num7 = ChooserCount() - 1; num7 >= 0; num7--)
			{
				if (num7 != num2)
				{
					chooserSource.DeleteObject(num7);
				}
			}
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("DeleteUnused");
			return true;
		}
		return false;
	}

	public CSBlock GetChooserSource()
	{
		if (InGems != null)
		{
			return InGems.OutputChannelBlock;
		}
		return null;
	}

	public void SetChooserSource(CSBlock csBlock, bool isMaintainingUndo = true)
	{
		if (isMaintainingUndo)
		{
			PreChange("SetChooserSource");
		}
		InGems.OutputChannelBlock = csBlock;
		InGems.OutputChannelIndex = -1;
		if (isMaintainingUndo)
		{
			Change();
		}
	}

	public CSData GetChooserObject(int index)
	{
		return GetChooserSource()?.GetOutputChannelData(index);
	}

	public string GetChooserObjectIdentifier(int index)
	{
		return GetChooserSource()?.GetOutputChannelIdentifier(index);
	}

	public override CSOutputChannel GetOutputChannel(int outputChannelIndex)
	{
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		if (outputChannelIndex >= 0 && outputChannelIndex < base.OutputChannels.Count && base.OutputChannels[outputChannelIndex] == OutGem)
		{
			if (GetChooserSource() == null)
			{
				OutGem.SetData(new CSData());
				return OutGem;
			}
			bool value = true;
			InOnOff.GetBoolean(ref value);
			if (!value)
			{
				OutGem.SetData(new CSData());
				return OutGem;
			}
			CSGemData cSGemData = GetCurrentCSGemData();
			if (cSGemData == null)
			{
				OutGem.SetData(new CSData());
				return OutGem;
			}
			if (cSGemData.Def != null && cSGemData.Def.IsGeneratedGem)
			{
				CheckGemTableForCustomSize();
				cSGemData = new CSGemData(cSGemData.Def.GenerateCSGemData(_currentTableIndex))
				{
					CurrentSizeTableIndex = _currentTableIndex
				};
				CSApplicationHelper.Application.Document.ReplaceGeneratedGem(cSGemData.Def);
			}
			cSGemData.SettingType = MountingType;
			CSGemData cSGemData2 = new CSGemData(cSGemData);
			if (base.IsOutputDisplayMaterialClassificationAssigned)
			{
				cSGemData2.DefaultDisplayMaterialClassification = base.OutputDisplayMaterialClassification;
			}
			GetSizeTableMinMax(out var minx, out var maxx, out var miny, out var maxy);
			cSGemData.TableMinX = minx;
			cSGemData.TableMaxX = maxx;
			cSGemData.TableMinY = miny;
			cSGemData.TableMaxY = maxy;
			cSGemData.CurrentSizeTableIndex = _currentTableIndex;
			BoundingBox boundingBox = BoundingBox.Unset;
			cSGemData2.GetTightBoundingBox(ref boundingBox);
			double num = boundingBox.Max.Y - boundingBox.Min.Y;
			double num2 = boundingBox.Max.X - boundingBox.Min.X;
			double num3 = boundingBox.Max.Z - boundingBox.Min.Z;
			if (num > 0.0 && num2 > 0.0 && num3 > 0.0)
			{
				OnXform val = new OnXform();
				val.Scale(Width / num2, Height / num, Depth / num3);
				cSGemData2.ApplyTransform(val.Convert());
				OutGem.SetData(cSGemData2);
				return OutGem;
			}
			OutGem.SetData(cSGemData);
			return OutGem;
		}
		return base.GetOutputChannel(outputChannelIndex);
	}

	public override CSData GetOutputChannelData(int outputChannelIndex)
	{
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Expected O, but got Unknown
		if (outputChannelIndex >= 0 && outputChannelIndex < base.OutputChannels.Count && base.OutputChannels[outputChannelIndex] == OutGem)
		{
			if (GetChooserSource() == null)
			{
				return null;
			}
			bool value = true;
			InOnOff.GetBoolean(ref value);
			if (!value)
			{
				return new CSData();
			}
			CSGemData cSGemData = GetCurrentCSGemData();
			if (cSGemData == null)
			{
				return new CSData();
			}
			if (cSGemData.Def != null && cSGemData.Def.IsGeneratedGem)
			{
				CheckGemTableForCustomSize();
				cSGemData = new CSGemData(cSGemData.Def.GenerateCSGemData(_currentTableIndex))
				{
					CurrentSizeTableIndex = _currentTableIndex
				};
				CSApplicationHelper.Application.Document.ReplaceGeneratedGem(cSGemData.Def);
			}
			cSGemData.SettingType = MountingType;
			CSGemData cSGemData2 = new CSGemData(cSGemData);
			if (base.IsOutputDisplayMaterialClassificationAssigned)
			{
				cSGemData2.DefaultDisplayMaterialClassification = base.OutputDisplayMaterialClassification;
			}
			GetSizeTableMinMax(out var minx, out var maxx, out var miny, out var maxy);
			cSGemData.TableMinX = minx;
			cSGemData.TableMaxX = maxx;
			cSGemData.TableMinY = miny;
			cSGemData.TableMaxY = maxy;
			cSGemData.CurrentSizeTableIndex = _currentTableIndex;
			BoundingBox boundingBox = BoundingBox.Unset;
			cSGemData2.GetTightBoundingBox(ref boundingBox);
			double num = boundingBox.Max.Y - boundingBox.Min.Y;
			double num2 = boundingBox.Max.X - boundingBox.Min.X;
			double num3 = boundingBox.Max.Z - boundingBox.Min.Z;
			if (num > 0.0 && num2 > 0.0 && num3 > 0.0)
			{
				OnXform val = new OnXform();
				val.Scale(Width / num2, Height / num, Depth / num3);
				cSGemData2.ApplyTransform(val.Convert());
				return cSGemData2;
			}
			return cSGemData;
		}
		return base.GetOutputChannelData(outputChannelIndex);
	}

	public override bool ReadJson(JObject archive, CSParaGraph graph, int major, int minor)
	{
		bool result = base.ReadJson(archive, graph, major, minor) & archive.ReadJsonValue<double>("Height", out Height) & archive.ReadJsonValue<double>("Width", out Width) & archive.ReadJsonValue<double>("Depth", out Depth) & archive.ReadJsonValue<double>("Percentage", out Percentage) & archive.ReadJsonValue<double>("MinX", out _minX) & archive.ReadJsonValue<double>("MaxX", out _maxX) & archive.ReadJsonValue<double>("MinY", out _minY) & archive.ReadJsonValue<double>("MaxY", out _maxY) & archive.ReadJsonObject<List<GemMinMaxInfo>>("GemMinMaxInformation", out GemMinMaxInfos) & archive.ReadJsonObject<List<GemTableSizeExclusion>>("GemTableExclusions", out GemTableSizeExclusions);
		CheckGemTableForCustomSize();
		return result;
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		bool flag = base.Read(ref archive, graph, major, minor);
		string empty = string.Empty;
		flag &= archive.ReadDouble(ref Height);
		flag &= archive.ReadDouble(ref Width);
		flag &= archive.ReadDouble(ref Depth);
		flag &= archive.ReadDouble(ref Percentage);
		flag &= archive.ReadDouble(ref _minX);
		flag &= archive.ReadDouble(ref _maxX);
		flag &= archive.ReadDouble(ref _minY);
		flag &= archive.ReadDouble(ref _maxY);
		if (major >= 3 || (major == 2 && minor >= 1))
		{
			flag &= archive.ReadString(ref empty);
			if (flag)
			{
				try
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<GemMinMaxInfo>));
					StringReader textReader = new StringReader(empty);
					GemMinMaxInfos = xmlSerializer.Deserialize(textReader) as List<GemMinMaxInfo>;
				}
				catch (Exception)
				{
				}
			}
		}
		GemTableSizeExclusions = base.UserData.FindSerializedObject<List<GemTableSizeExclusion>>("GemTableSize", "Exclusions");
		CheckGemTableForCustomSize();
		return flag;
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		archive.Add("Height", Height);
		archive.Add("Width", Width);
		archive.Add("Depth", Depth);
		archive.Add("Percentage", Percentage);
		archive.Add("MinX", _minX);
		archive.Add("MaxX", _maxX);
		archive.Add("MinY", _minY);
		archive.Add("MaxY", _maxY);
		if (GemMinMaxInfos != null)
		{
			archive.Add("GemMinMaxInformation", JArray.FromObject(GemMinMaxInfos));
		}
		if (GemTableSizeExclusions != null)
		{
			archive.Add("GemTableExclusions", JArray.FromObject(GemTableSizeExclusions));
		}
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.UserData.AddSerializedObject("GemTableSize", "Exclusions", GemTableSizeExclusions);
		bool flag = base.Write(ref archive);
		flag &= archive.WriteDouble(Height);
		flag &= archive.WriteDouble(Width);
		flag &= archive.WriteDouble(Depth);
		flag &= archive.WriteDouble(Percentage);
		flag &= archive.WriteDouble(_minX);
		flag &= archive.WriteDouble(_maxX);
		flag &= archive.WriteDouble(_minY);
		flag &= archive.WriteDouble(_maxY);
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<GemMinMaxInfo>));
			StringWriter stringWriter = new StringWriter();
			xmlSerializer.Serialize(stringWriter, GemMinMaxInfos);
			flag &= archive.WriteString(stringWriter.ToString());
		}
		catch (Exception)
		{
		}
		return flag;
	}

	public void CheckAndRemoveShapeSpecificMinMax()
	{
		CSGemData csGemData = GetCurrentCSGemData();
		if (csGemData != null && csGemData.Def != null)
		{
			GemMinMaxInfo gemMinMaxInfo = GemMinMaxInfos.ToList().FirstOrDefault((GemMinMaxInfo gemInfo) => gemInfo.GemShape.Replace(" ", "") == csGemData.Def.Shape.Replace(" ", "") && gemInfo.GemType == csGemData.Def.Type);
			if (gemMinMaxInfo != null)
			{
				GemMinMaxInfos.Remove(gemMinMaxInfo);
			}
		}
	}

	private GemMinMaxInfo GetShapeSpecificGemMinMaxInfo()
	{
		CSGemData csGemData = GetCurrentCSGemData();
		GemMinMaxInfo result = null;
		if (csGemData != null && csGemData.Def != null)
		{
			result = GemMinMaxInfos.FirstOrDefault((GemMinMaxInfo gemInfo) => gemInfo.GemShape.Replace(" ", "") == csGemData.Def.Shape.Replace(" ", "") && gemInfo.GemType == csGemData.Def.Type);
		}
		return result;
	}

	public bool HasShapeSpecificGemMinMax()
	{
		return GetShapeSpecificGemMinMaxInfo() != null;
	}

	private void GetSizeTableMinMax(out double minx, out double maxx, out double miny, out double maxy)
	{
		minx = double.MaxValue;
		miny = double.MaxValue;
		maxx = 0.0;
		maxy = 0.0;
		if (GetChooserSource() == null)
		{
			return;
		}
		CSGemData currentCSGemData = GetCurrentCSGemData();
		if (currentCSGemData == null || currentCSGemData.Def == null)
		{
			return;
		}
		if (ValueMinX > 0.0 && ValueMaxX > 0.0 && ValueMinY > 0.0 && ValueMaxY > 0.0)
		{
			foreach (GemSize size in currentCSGemData.Sizes)
			{
				if (size.X >= ValueMinX && size.X <= ValueMaxX && size.Y >= ValueMinY && size.Y <= ValueMaxY)
				{
					minx = Math.Min(size.X, minx);
					miny = Math.Min(size.Y, miny);
					maxx = Math.Max(size.X, maxx);
					maxy = Math.Max(size.Y, maxy);
				}
			}
			return;
		}
		foreach (GemSize size2 in currentCSGemData.Sizes)
		{
			minx = Math.Min(size2.X, minx);
			miny = Math.Min(size2.Y, miny);
			maxx = Math.Max(size2.X, maxx);
			maxy = Math.Max(size2.Y, maxy);
		}
	}

	private void SetGemMinMaxInfoFromCurrentCSGemData(double minX, double maxX, double minY, double maxY)
	{
		CSGemData csGemData = GetCurrentCSGemData();
		if (csGemData != null && csGemData.Def != null)
		{
			GemMinMaxInfo gemMinMaxInfo = GemMinMaxInfos.FirstOrDefault((GemMinMaxInfo gemInfo) => gemInfo.GemShape.Replace(" ", "") == csGemData.Def.Shape.Replace(" ", "") && gemInfo.GemType == csGemData.Def.Type);
			if (gemMinMaxInfo == null)
			{
				gemMinMaxInfo = new GemMinMaxInfo
				{
					GemShape = csGemData.Def.Shape.Replace(" ", ""),
					GemType = csGemData.Def.Type
				};
				GemMinMaxInfos.Add(gemMinMaxInfo);
			}
			gemMinMaxInfo.MinX = minX;
			gemMinMaxInfo.MaxX = maxX;
			gemMinMaxInfo.MinY = minY;
			gemMinMaxInfo.MaxY = maxY;
		}
	}

	public override bool AddObjectFromCurrentSelection()
	{
		CSBlock chooserSource = GetChooserSource();
		if (chooserSource == null)
		{
			return false;
		}
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("AddObjectFromCurrentSelection");
		bool num = chooserSource.AddObjectFromCurrentSelection();
		if (num)
		{
			double num2 = chooserSource.OutputChannelCount - 1;
			if (num2 < 0.0)
			{
				num2 = 0.0;
			}
			SetValue(num2);
		}
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("AddObjectFromCurrentSelection");
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
		CSUtil.RedrawUI();
		return num;
	}

	public override bool AddObjectFromFile(string fileName)
	{
		CSBlock chooserSource = GetChooserSource();
		if (chooserSource == null)
		{
			return false;
		}
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("AddObjectFromFile");
		bool num = chooserSource.AddObjectFromFile(fileName);
		if (num)
		{
			double num2 = chooserSource.OutputChannelCount - 1;
			if (num2 < 0.0)
			{
				num2 = 0.0;
			}
			SetValue(num2);
		}
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("AddObjectFromFile");
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
		CSUtil.RedrawUI();
		return num;
	}

	public override bool DeleteObject(int index)
	{
		CSBlock chooserSource = GetChooserSource();
		if (chooserSource == null)
		{
			return false;
		}
		if (ChooserCount() == 1)
		{
			return false;
		}
		if (!chooserSource.DeleteObject(index))
		{
			return false;
		}
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("DeleteObject");
		double num = base.Value - 1.0;
		if (num < 0.0)
		{
			num = 0.0;
		}
		SetValue(num);
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("DeleteObject");
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
		CSUtil.RedrawUI();
		return true;
	}

	public void DeleteObjects(int[] indexes)
	{
		CSBlock chooserSource = GetChooserSource();
		if (chooserSource == null)
		{
			return;
		}
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("DeleteObjects");
		Array.Sort(indexes);
		int num = indexes.Length - 1;
		while (num >= 0 && ChooserCount() != 1)
		{
			if (chooserSource.DeleteObject(indexes[num]) && !(base.Value < (double)indexes[num]))
			{
				double num2 = base.Value - 1.0;
				if (num2 < 0.0)
				{
					num2 = 0.0;
				}
				SetValue(num2);
			}
			num--;
		}
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("DeleteObjects");
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
		CSUtil.RedrawUI();
	}

	public void SetCombinedGemSizeFromUI(double height, double width, double depth)
	{
		if (Math.Abs(Height - height) > 0.005 || Math.Abs(Width - width) > 0.005 || Math.Abs(depth - Depth) > 0.005)
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetCombinedGemSizeFromUI");
			PreChange("SetCombinedGemSizeFromUI");
			Height = height;
			Width = width;
			Depth = depth;
			if (Width > 0.0)
			{
				Percentage = Depth / Width * 100.0;
			}
			Change();
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetCombinedGemSizeFromUI");
			CSApplicationHelper.Application.Document.Graph.Generate();
			CSUtil.Redraw();
			CSUtil.RedrawUI();
		}
	}

	public void SetCaratFromUI(double value)
	{
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetCaratFromUI");
		SetSizeValuesFromCarat(value);
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetCaratFromUI");
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
		CSUtil.RedrawUI();
	}

	public void SetDepthFromUI(double value)
	{
		if (Math.Abs(Depth - value) > 0.005 && value > 0.0)
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetDepthFromUI");
			PreChange("SetDepthFromUI");
			Depth = value;
			if (Width > 0.0)
			{
				Percentage = Depth / Width * 100.0;
			}
			Change();
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetDepthFromUI");
			CSApplicationHelper.Application.Document.Graph.Generate();
			CSUtil.Redraw();
			CSUtil.RedrawUI();
		}
	}

	public void SetGlobalMinMaxFromUI(double minX, double maxX, double minY, double maxY)
	{
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetGlobalMinMaxFromUI");
		PreChange("SetGlobalMinMaxFromUI");
		_minX = minX;
		_maxX = maxX;
		_minY = minY;
		_maxY = maxY;
		Change();
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetGlobalMinMaxFromUI");
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
		CSUtil.RedrawUI();
	}

	public void SetHeightFromUI(double value)
	{
		if (Math.Abs(Height - value) > 0.005 && value > 0.0)
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetHeightFromUI");
			PreChange("SetHeightFromUI");
			Height = value;
			Change();
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetHeightFromUI");
			CSApplicationHelper.Application.Document.Graph.Generate();
			CSUtil.Redraw();
			CSUtil.RedrawUI();
		}
	}

	public void SetInfoFromUI(string title, bool studioVisible, bool studioVisibleAdvanced, bool studioVisibleDesignOption)
	{
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetInfoFromUI");
		base.Identifier = title;
		base.StudioVisible = studioVisible;
		base.StudioVisibleAdvanced = studioVisibleAdvanced;
		base.StudioVisibleDesignOption = studioVisibleDesignOption;
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetInfoFromUI");
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
		CSUtil.RedrawUI();
	}

	public void SetPercentageFromUI(double value)
	{
		if (Math.Abs(Percentage - value) > 0.005 && value > 0.0)
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetPercentageFromUI");
			PreChange("SetPercentageFromUI");
			Percentage = value;
			Depth = Percentage / 100.0 * Width;
			Change();
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetPercentageFromUI");
			CSApplicationHelper.Application.Document.Graph.Generate();
			CSUtil.Redraw();
			CSUtil.RedrawUI();
		}
	}

	public void SetShapeSpecificMinMaxFromUI(double minXSpecific, double maxXSpecific, double minYSpecific, double maxYSpecific)
	{
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetShapeSpecificMinMaxFromUI");
		PreChange("SetShapeSpecificMinMaxFromUI");
		SetGemMinMaxInfoFromCurrentCSGemData(minXSpecific, maxXSpecific, minYSpecific, maxYSpecific);
		Change();
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetShapeSpecificMinMaxFromUI");
	}

	public void SetSizeFromTableUI(int index, bool mouseDown)
	{
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		if (!_hasCreatedUndoTransaction)
		{
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetSizeFromTableUI");
			PreChange("SetSizeFromTableUI");
			_hasCreatedUndoTransaction = true;
		}
		if (mouseDown)
		{
			CSUtil.ToggleMouseDown(this);
			SetSizeValuesFromSliderIndex(index);
		}
		else
		{
			CSUtil.RemoveMouseDown();
			SetSizeValuesFromSliderIndex(index);
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetSizeFromTableUI");
			_hasCreatedUndoTransaction = false;
		}
		CSApplicationHelper.Application.Document.Graph.Generate(mouseDown);
		CSUtil.Redraw();
		if (!mouseDown)
		{
			CSUtil.RedrawUI();
		}
	}

	public bool SetValue(double newValue, bool doOverridePrevious = false)
	{
		if (!(Math.Abs(newValue - _doubleValue) > CSApplicationHelper.Application.Tolerance))
		{
			return true;
		}
		_previousValue = (doOverridePrevious ? newValue : _doubleValue);
		PreChange("SetValue");
		_doubleValue = newValue;
		Change();
		return true;
	}

	public void SetValueFromUI(double value)
	{
		try
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetValueFromUI");
			if (Math.Abs(value - base.RealValue) > 0.001)
			{
				PreChange("SetValueFromUI");
				SetSizeValuesFromNewGemShape(value);
				Change();
			}
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetValueFromUI");
			CSApplicationHelper.Application.Document.Graph.Generate();
			CSUtil.Redraw();
			CSUtil.RedrawUI();
		}
		catch (Exception ex)
		{
			CSUtil.Print(ex.Message);
			CSUtil.Print(ex.StackTrace);
		}
	}

	public void SetWidthFromUI(double value)
	{
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetWidthFromUI");
		if (Math.Abs(Width - value) > 0.001 && value > 0.0)
		{
			PreChange("SetWidthFromUI");
			Width = value;
			Depth = Percentage * 0.01 * Width;
			Change();
		}
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetWidthFromUI");
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
		CSUtil.RedrawUI();
	}

	public override bool SetFileValue(CSObject setObject)
	{
		base.SetFileValue(setObject);
		if (base.NotCreatedYet)
		{
			return true;
		}
		if (!(setObject is CSValueGem cSValueGem))
		{
			return true;
		}
		Height = cSValueGem.Height;
		Width = cSValueGem.Width;
		Depth = cSValueGem.Depth;
		Percentage = cSValueGem.Percentage;
		return true;
	}

	public override bool SetStyle(CSObject setObject, bool showRewirePrompt = true)
	{
		base.SetStyle(setObject, showRewirePrompt);
		if (base.NotCreatedYet)
		{
			return true;
		}
		if (!(setObject is CSValueGem cSValueGem))
		{
			return true;
		}
		Height = cSValueGem.Height;
		Width = cSValueGem.Width;
		Depth = cSValueGem.Depth;
		Percentage = cSValueGem.Percentage;
		_minX = cSValueGem._minX;
		_maxX = cSValueGem._maxX;
		_minY = cSValueGem._minY;
		_maxY = cSValueGem._maxY;
		GemMinMaxInfos = cSValueGem.GemMinMaxInfos;
		return true;
	}

	public override bool SetUndo(CSObject setObject)
	{
		base.SetUndo(setObject);
		if (!base.NotCreatedYet && setObject is CSValueGem cSValueGem)
		{
			Height = cSValueGem.Height;
			Width = cSValueGem.Width;
			Depth = cSValueGem.Depth;
			Percentage = cSValueGem.Percentage;
			_minX = cSValueGem._minX;
			_maxX = cSValueGem._maxX;
			_minY = cSValueGem._minY;
			_maxY = cSValueGem._maxY;
			GemMinMaxInfos = cSValueGem.GemMinMaxInfos;
		}
		return true;
	}

	public List<GemTableSizeInfo> GetGemTableSizeInfoList()
	{
		List<GemTableSizeInfo> gemTableSizeInfoList = new List<GemTableSizeInfo>();
		CSGemData gemData = GetCurrentCSGemData();
		if (gemData == null)
		{
			return gemTableSizeInfoList;
		}
		ValidSizes(gemData.Sizes).ForEach(delegate(GemSize gemItem)
		{
			gemTableSizeInfoList.Add(new GemTableSizeInfo
			{
				Carat = gemItem.Width * gemItem.Height * gemItem.Depth * 3.52 * gemData.FFactor,
				Height = gemItem.Height,
				Width = gemItem.Width,
				Depth = gemItem.Depth
			});
		});
		return gemTableSizeInfoList;
	}

	public int GetSizeTableCount()
	{
		CSGemData currentCSGemData = GetCurrentCSGemData();
		if (currentCSGemData == null)
		{
			return 0;
		}
		if (currentCSGemData.Def == null)
		{
			return 0;
		}
		return ValidSizes(currentCSGemData.Sizes).Count();
	}

	public void SetSizeValuesFromCarat(double carat)
	{
		CSGemData currentCSGemData = GetCurrentCSGemData();
		if (currentCSGemData == null)
		{
			return;
		}
		double num = 999.0;
		int sizeValuesFromSliderIndex = -1;
		List<GemSize> list = ValidSizes(currentCSGemData.Sizes);
		for (int i = 0; i < list.Count; i++)
		{
			GemSize gemSize = list[i];
			double num2 = gemSize.X * gemSize.Y * gemSize.Z * currentCSGemData.FFactor * 3.52;
			if (Math.Abs(num2 - carat) <= num)
			{
				num = Math.Abs(num2 - carat);
				sizeValuesFromSliderIndex = i;
			}
		}
		SetSizeValuesFromSliderIndex(sizeValuesFromSliderIndex);
	}

	public void SetSizeValuesFromNewGemShape(double value)
	{
		CSBlock chooserSource = GetChooserSource();
		if (chooserSource == null)
		{
			return;
		}
		SetValue(value);
		if (Math.Abs(_previousValue - value) < CSApplicationHelper.Application.Tolerance)
		{
			return;
		}
		CSData outputChannelData = chooserSource.GetOutputChannelData((int)_previousValue);
		_previousValue = value;
		if (!(outputChannelData is CSGemData cSGemData))
		{
			return;
		}
		CSGemData currentCSGemData = GetCurrentCSGemData();
		if (currentCSGemData == null)
		{
			return;
		}
		double num = Width * Height * Depth * cSGemData.FFactor;
		double num2 = 999.0;
		int sizeValuesFromSliderIndex = -1;
		List<GemSize> list = ValidSizes(currentCSGemData.Sizes);
		for (int i = 0; i < list.Count; i++)
		{
			GemSize gemSize = list[i];
			double num3 = gemSize.X * gemSize.Y * gemSize.Z * currentCSGemData.FFactor;
			if (Math.Abs(num3 - num) <= num2)
			{
				num2 = Math.Abs(num3 - num);
				sizeValuesFromSliderIndex = i;
			}
		}
		SetSizeValuesFromSliderIndex(sizeValuesFromSliderIndex);
	}

	private void SetSizeValuesFromSliderIndex(int sliderValueIndex)
	{
		CSGemData cSGemData = GetCurrentCSGemData();
		if (cSGemData != null && cSGemData.Def != null)
		{
			if (sliderValueIndex < 0)
			{
				sliderValueIndex = 0;
			}
			PreChange("SetSizeValuesFromSliderIndex");
			if (cSGemData.Def.IsGeneratedGem)
			{
				cSGemData = new CSGemData(cSGemData.Def.GenerateCSGemData(sliderValueIndex))
				{
					CurrentSizeTableIndex = sliderValueIndex
				};
				CSApplicationHelper.Application.Document.ReplaceGeneratedGem(cSGemData.Def);
			}
			List<GemSize> list = ValidSizes(cSGemData.Sizes);
			if (sliderValueIndex > list.Count - 1)
			{
				string message = string.Format("Error Setting CSValueGem to Selected UI Size Index.\nDesign Id: {0}\nIndex: {1}\nValidSizeCount: {2}\n", CSApplicationHelper.Application.Document.DocumentUserData.FindValue("DesignInfo", "FileNumber"), sliderValueIndex, list.Count);
				LogHelper.Log.WriteLog(message);
				sliderValueIndex = list.Count - 1;
			}
			Height = list.ElementAt(sliderValueIndex).Height;
			Width = list.ElementAt(sliderValueIndex).Width;
			Depth = list.ElementAt(sliderValueIndex).Depth;
			Percentage = list.ElementAt(sliderValueIndex).DepthPercentage;
			Change();
		}
	}

	public List<GemSize> GetCurrentValidSizes(bool isCSDValidSize = false)
	{
		return ValidSizes(GetCurrentCSGemData().Sizes, isCSDValidSize);
	}

	private List<GemSize> ValidSizes(IEnumerable<GemSize> sizes, bool isCSDValidSize = false)
	{
		GemSize[] array = (sizes as GemSize[]) ?? sizes.ToArray();
		CSGemData csGemData = GetCurrentCSGemData();
		GemTableSizeExclusion shapeSpecificGemTableSizeExclusions = GetShapeSpecificGemTableSizeExclusions();
		Dictionary<double, GemSize> source = array.GroupBy((GemSize _) => _.Width * _.Height * _.Depth * 3.52 * csGemData.FFactor, (double key, IEnumerable<GemSize> grouping) => grouping.FirstOrDefault()).ToDictionary((GemSize _) => _.Width * _.Height * _.Depth * 3.52 * csGemData.FFactor);
		List<GemSize> list;
		if (IsAssignedCenter && CSApplicationHelper.Application.IsQuarterCaratLargeStoneRoundingEnabled && !isCSDValidSize)
		{
			list = (from _ in source.Where((KeyValuePair<double, GemSize> _) => _.Key >= 0.3 && _.Key <= CSApplicationHelper.Application.LargeStoneCaratMax).GroupBy((KeyValuePair<double, GemSize> _) => Math.Round(_.Key * 4.0, MidpointRounding.ToEven) / 4.0, (double key, IEnumerable<KeyValuePair<double, GemSize>> grouping) => new
				{
					Key = key,
					ClosestQuarter = grouping.OrderBy((KeyValuePair<double, GemSize> carats) => Math.Abs(carats.Key % 0.25)).FirstOrDefault()
				})
				select _.ClosestQuarter.Value).ToList();
			if (!list.Any())
			{
				list = (from _ in source.Where((KeyValuePair<double, GemSize> _) => _.Key <= CSApplicationHelper.Application.LargeStoneCaratMax).GroupBy((KeyValuePair<double, GemSize> _) => Math.Round(_.Key * 4.0, MidpointRounding.ToEven) / 4.0, (double key, IEnumerable<KeyValuePair<double, GemSize>> grouping) => new
					{
						Key = key,
						ClosestQuarter = grouping.OrderBy((KeyValuePair<double, GemSize> carats) => Math.Abs(carats.Key % 0.25)).FirstOrDefault()
					})
					select _.ClosestQuarter.Value).ToList();
			}
		}
		else
		{
			list = ((!CSApplicationHelper.IsCounterSketchStudio || !IsAssignedCenter || isCSDValidSize) ? source.Select((KeyValuePair<double, GemSize> _) => _.Value).ToList() : (from _ in source
				where _.Key <= CSApplicationHelper.Application.LargeStoneCaratMax
				select _.Value).ToList());
			if (!list.Any())
			{
				list = source.Select((KeyValuePair<double, GemSize> _) => _.Value).ToList();
			}
		}
		List<GemSize> list2 = FilterGemSizes(array, shapeSpecificGemTableSizeExclusions, list);
		if (list2 == null || list2.Any())
		{
			return list2;
		}
		IsAssignedCenter = false;
		list = source.Select((KeyValuePair<double, GemSize> _) => _.Value).ToList();
		return FilterGemSizes(array, shapeSpecificGemTableSizeExclusions, list);
	}

	private List<GemSize> FilterGemSizes(GemSize[] gemSizes, GemTableSizeExclusion excludedSizes, List<GemSize> filteredSizes)
	{
		List<GemSize> list = new List<GemSize>();
		foreach (GemSize gemSize in gemSizes)
		{
			if (gemSize.X <= ValueMaxX && gemSize.X >= ValueMinX && gemSize.Y <= ValueMaxY && gemSize.Y >= ValueMinY && (excludedSizes == null || !excludedSizes.ExcludedGemTableSizeIndexes.Contains(gemSizes.ToList().IndexOf(gemSize))) && filteredSizes.Contains(gemSize))
			{
				list.Add(gemSize);
			}
		}
		return list;
	}
}
