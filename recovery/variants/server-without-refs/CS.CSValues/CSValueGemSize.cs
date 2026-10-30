using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CS.CSObjects;
using CS.CSValues.Interfaces;
using CS.JSonDoc;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using CSGemInstance.GemTableData;
using GV.CounterSketch.Shared.Model.Objects;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CS.CSValues;

[CSBlock("ValueGemSize", "{7E045F64-4288-4502-B6BF-39838467A69E}")]
public sealed class CSValueGemSize : CSValue, ICSValueGemSize, ICSLoopUpdate
{
	public double Depth;

	public double Height;

	public double MaxX;

	public double MaxY;

	public double MinX;

	public double MinY;

	public double Percentage;

	public double Width;

	private bool _hasCreatedUndoTransaction;

	private int _currentTableIndex;

	private bool _isPerformingInitialDefaultConversion = true;

	public CSInputChannel InGemShape { get; }

	public CSInputChannel InOnOff { get; }

	public CSOutputChannel OutGem { get; }

	public List<GemTableSizeExclusion> GemTableSizeExclusions { get; set; }

	public bool IsAssignedCenter { get; set; }

	private List<GemMinMaxInfo> GemMinMaxInfos { get; set; }

	private CSGemData PreviousCSGemData { get; set; }

	public CSInputChannel InDisplayUpdate { get; set; }

	public bool IsCustomGemSize => CheckGemTableForCustomSize();

	public double GemCarat
	{
		get
		{
			if (InGemShape == null)
			{
				return 0.0;
			}
			if (InGemShape.OutputChannelBlock == null)
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

	public string GemShape
	{
		get
		{
			CSGemData currentCSGemData = GetCurrentCSGemData();
			if (currentCSGemData == null || currentCSGemData.Def == null)
			{
				return string.Empty;
			}
			return currentCSGemData.GemShape;
		}
	}

	public string GemType
	{
		get
		{
			CSGemData currentCSGemData = GetCurrentCSGemData();
			if (currentCSGemData == null || currentCSGemData.Def == null)
			{
				return string.Empty;
			}
			return currentCSGemData.GemType;
		}
	}

	public bool IsShapeSpecificGemMinMax => GetShapeSpecificGemMinMaxInfo() != null;

	public double ValueMaxX => GetShapeSpecificGemMinMaxInfo()?.MaxX ?? GlobalValueMaxX;

	public double ValueMaxY => GetShapeSpecificGemMinMaxInfo()?.MaxY ?? GlobalValueMaxY;

	public double ValueMinX => GetShapeSpecificGemMinMaxInfo()?.MinX ?? GlobalValueMinX;

	public double ValueMinY => GetShapeSpecificGemMinMaxInfo()?.MinY ?? GlobalValueMinY;

	public double GlobalValueMaxX
	{
		get
		{
			CSGemData currentCSGemData = GetCurrentCSGemData();
			if (currentCSGemData == null)
			{
				return 0.0;
			}
			if (currentCSGemData.Def == null)
			{
				return 0.0;
			}
			GemSize gemSize = currentCSGemData.Sizes.OrderByDescending((GemSize gemSize2) => gemSize2.X).FirstOrDefault();
			if (gemSize != null)
			{
				if (!(Math.Abs(MaxX) < 0.001))
				{
					return MaxX;
				}
				return gemSize.X;
			}
			return MaxX;
		}
	}

	public double GlobalValueMaxY
	{
		get
		{
			CSGemData currentCSGemData = GetCurrentCSGemData();
			if (currentCSGemData == null)
			{
				return 0.0;
			}
			if (currentCSGemData.Def == null)
			{
				return 0.0;
			}
			GemSize gemSize = currentCSGemData.Sizes.OrderByDescending((GemSize gemSize2) => gemSize2.Y).FirstOrDefault();
			if (gemSize != null)
			{
				if (!(Math.Abs(MaxY) < 0.001))
				{
					return MaxY;
				}
				return gemSize.Y;
			}
			return MaxY;
		}
	}

	public double GlobalValueMinX
	{
		get
		{
			CSGemData currentCSGemData = GetCurrentCSGemData();
			if (currentCSGemData == null)
			{
				return 0.0;
			}
			if (currentCSGemData.Def == null)
			{
				return 0.0;
			}
			GemSize gemSize = currentCSGemData.Sizes.OrderBy((GemSize gemSize2) => gemSize2.X).FirstOrDefault();
			if (gemSize != null)
			{
				if (!(Math.Abs(MinX) < 0.001))
				{
					return MinX;
				}
				return gemSize.X;
			}
			return MinX;
		}
	}

	public double GlobalValueMinY
	{
		get
		{
			CSGemData currentCSGemData = GetCurrentCSGemData();
			if (currentCSGemData == null)
			{
				return 0.0;
			}
			if (currentCSGemData.Def == null)
			{
				return 0.0;
			}
			GemSize gemSize = currentCSGemData.Sizes.OrderBy((GemSize gemSize2) => gemSize2.Y).FirstOrDefault();
			if (gemSize != null)
			{
				if (!(Math.Abs(MinY) < 0.001))
				{
					return MinY;
				}
				return gemSize.Y;
			}
			return MinY;
		}
	}

	public CSValueGemSize(string identifier)
		: base(identifier)
	{
		GemMinMaxInfos = new List<GemMinMaxInfo>();
		GemTableSizeExclusions = new List<GemTableSizeExclusion>();
		InGemShape = new CSInputChannel(GetPropertyName(() => InGemShape), this, ChannelDataType.Gem, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		InOnOff = new CSInputChannel(GetPropertyName(() => InOnOff), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		InDisplayUpdate = new CSInputChannel(GetPropertyName(() => InDisplayUpdate), this, ChannelDataType.Double, ChannelArrayType.Individual, ChannelFlag.NotLoopChecked);
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutGem = new CSOutputChannel(GetPropertyName(() => OutGem), this, ChannelDataType.Gem, ChannelArrayType.Individual);
	}

	public void LoopUpdate()
	{
		if (InDisplayUpdate == null)
		{
			return;
		}
		double value = 0.0;
		if (InDisplayUpdate.GetDouble(ref value))
		{
			int valueFromXSize = GetValueFromXSize(value);
			if ((double)valueFromXSize != base.RealValue)
			{
				_previousValue = base.RealValue;
				_doubleValue = valueFromXSize;
				SetSizeValuesFromSliderIndex(valueFromXSize, doPreChangeChange: false);
				UIChanged = true;
			}
		}
	}

	public List<GemTableSizeInfo> GetGemTableSizeInfoList()
	{
		List<GemTableSizeInfo> gemTableSizeInfoList = new List<GemTableSizeInfo>();
		if (InGemShape == null)
		{
			return new List<GemTableSizeInfo>();
		}
		if (InGemShape.OutputChannelBlock == null)
		{
			return new List<GemTableSizeInfo>();
		}
		CSGemData gemData = GetCurrentCSGemData();
		if (gemData == null)
		{
			return new List<GemTableSizeInfo>();
		}
		if (gemData.Def == null)
		{
			return new List<GemTableSizeInfo>();
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

	public bool IsCSGemDataShapeChanged()
	{
		CSGemData currentCSGemData = GetCurrentCSGemData();
		if (currentCSGemData == null)
		{
			return false;
		}
		if (PreviousCSGemData == null)
		{
			PreviousCSGemData = currentCSGemData.Duplicate() as CSGemData;
			return false;
		}
		return PreviousCSGemData.GemShape != currentCSGemData.GemShape;
	}

	public void SetCombinedGemSizeFromUI(double height, double width, double depth)
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

	public override bool Update()
	{
		if (IsCSGemDataShapeChanged())
		{
			SetSizeValuesFromNewGemShape(doPreChangeChange: false);
		}
		return base.Update();
	}

	private bool CheckGemTableForCustomSize()
	{
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
			if (!(size.X > ValueMaxX) && !(size.X < ValueMinX) && !(size.Y > ValueMaxY) && !(size.Y < ValueMinY) && (shapeSpecificGemTableSizeExclusions == null || !shapeSpecificGemTableSizeExclusions.ExcludedGemTableSizeIndexes.Contains(currentCSGemData.Sizes.ToList().IndexOf(size))) && Math.Abs(Width - size.X) < 0.01 && Math.Abs(Height - size.Y) < 0.01 && Math.Abs(Depth - size.Z) < 0.1)
			{
				_currentTableIndex = currentCSGemData.Sizes.IndexOf(size);
				return false;
			}
		}
		return true;
	}

	private CSGemData GetCurrentCSGemData()
	{
		CSGemData cSGemData = InGemShape.Data as CSGemData;
		if (cSGemData?.Def == null)
		{
			return null;
		}
		if (!cSGemData.Def.IsGeneratedGem)
		{
			return cSGemData;
		}
		CSGemData obj = new CSGemData(cSGemData.Def.GenerateCSGemData(_currentTableIndex))
		{
			CurrentSizeTableIndex = _currentTableIndex
		};
		CSApplicationHelper.Application.Document.ReplaceGeneratedGem(cSGemData.Def);
		obj.SettingType = cSGemData.SettingType;
		return obj;
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

	private int GetValueFromXSize(double sizeX)
	{
		if (!(InGemShape.Data is CSGemData cSGemData))
		{
			return 0;
		}
		if (cSGemData.Def == null)
		{
			return 0;
		}
		int result = 0;
		double num = 999.0;
		for (int i = 0; i < cSGemData.Sizes.Count; i++)
		{
			double num2 = Math.Abs(cSGemData.Sizes[i].X - sizeX);
			if (num2 < num)
			{
				num = num2;
				result = i;
			}
		}
		return result;
	}

	public override CSOutputChannel GetOutputChannel(int outputChannelIndex)
	{
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		if (outputChannelIndex >= 0 && outputChannelIndex < base.OutputChannels.Count && base.OutputChannels[outputChannelIndex] == OutGem)
		{
			if (InGemShape == null)
			{
				return null;
			}
			if (InGemShape.OutputChannelBlock == null)
			{
				return null;
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
				return null;
			}
			if (cSGemData.Def != null && cSGemData.Def.IsGeneratedGem)
			{
				CheckGemTableForCustomSize();
				cSGemData = new CSGemData(cSGemData.Def.GenerateCSGemData(_currentTableIndex))
				{
					CurrentSizeTableIndex = _currentTableIndex,
					SettingType = cSGemData.SettingType
				};
				CSApplicationHelper.Application.Document.ReplaceGeneratedGem(cSGemData.Def);
			}
			CSGemData cSGemData2 = new CSGemData(cSGemData);
			if (base.IsOutputDisplayMaterialClassificationAssigned)
			{
				cSGemData2.DefaultDisplayMaterialClassification = base.OutputDisplayMaterialClassification;
			}
			cSGemData2.TableMinX = ValueMinX;
			cSGemData2.TableMaxX = ValueMaxX;
			cSGemData2.TableMinY = ValueMinY;
			cSGemData2.TableMaxY = ValueMaxY;
			cSGemData2.CurrentSizeTableIndex = _currentTableIndex;
			BoundingBox boundingBox = BoundingBox.Unset;
			cSGemData2.GetTightBoundingBox(ref boundingBox);
			Point3d val = ((BoundingBox)(ref boundingBox)).Max;
			double y = ((Point3d)(ref val)).Y;
			val = ((BoundingBox)(ref boundingBox)).Min;
			double num = y - ((Point3d)(ref val)).Y;
			val = ((BoundingBox)(ref boundingBox)).Max;
			double x = ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref boundingBox)).Min;
			double num2 = x - ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref boundingBox)).Max;
			double z = ((Point3d)(ref val)).Z;
			val = ((BoundingBox)(ref boundingBox)).Min;
			double num3 = z - ((Point3d)(ref val)).Z;
			if (!(num > 0.0) || !(num2 > 0.0) || !(num3 > 0.0))
			{
				OutGem.SetData(cSGemData);
				return OutGem;
			}
			OnXform val2 = new OnXform();
			val2.Scale(Width / num2, Height / num, Depth / num3);
			cSGemData2.ApplyTransform(val2.Convert());
			OutGem.SetData(cSGemData2);
			return OutGem;
		}
		return base.GetOutputChannel(outputChannelIndex);
	}

	public override CSData GetOutputChannelData(int outputChannelIndex)
	{
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Expected O, but got Unknown
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		if (outputChannelIndex >= 0 && outputChannelIndex < base.OutputChannels.Count && base.OutputChannels[outputChannelIndex] == OutGem)
		{
			if (InGemShape.OutputChannelBlock == null)
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
				return null;
			}
			if (cSGemData.Def != null && cSGemData.Def.IsGeneratedGem)
			{
				CheckGemTableForCustomSize();
				cSGemData = new CSGemData(cSGemData.Def.GenerateCSGemData(_currentTableIndex))
				{
					CurrentSizeTableIndex = _currentTableIndex,
					SettingType = cSGemData.SettingType
				};
				CSApplicationHelper.Application.Document.ReplaceGeneratedGem(cSGemData.Def);
			}
			CSGemData cSGemData2 = new CSGemData(cSGemData);
			if (base.IsOutputDisplayMaterialClassificationAssigned)
			{
				cSGemData2.DefaultDisplayMaterialClassification = base.OutputDisplayMaterialClassification;
			}
			cSGemData2.TableMinX = ValueMinX;
			cSGemData2.TableMaxX = ValueMaxX;
			cSGemData2.TableMinY = ValueMinY;
			cSGemData2.TableMaxY = ValueMaxY;
			cSGemData2.CurrentSizeTableIndex = _currentTableIndex;
			BoundingBox boundingBox = BoundingBox.Unset;
			cSGemData2.GetTightBoundingBox(ref boundingBox);
			Point3d val = ((BoundingBox)(ref boundingBox)).Max;
			double y = ((Point3d)(ref val)).Y;
			val = ((BoundingBox)(ref boundingBox)).Min;
			double num = y - ((Point3d)(ref val)).Y;
			val = ((BoundingBox)(ref boundingBox)).Max;
			double x = ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref boundingBox)).Min;
			double num2 = x - ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref boundingBox)).Max;
			double z = ((Point3d)(ref val)).Z;
			val = ((BoundingBox)(ref boundingBox)).Min;
			double num3 = z - ((Point3d)(ref val)).Z;
			if (!(num > 0.0) || !(num2 > 0.0) || !(num3 > 0.0))
			{
				return cSGemData;
			}
			OnXform val2 = new OnXform();
			val2.Scale(Width / num2, Height / num, Depth / num3);
			cSGemData2.ApplyTransform(val2.Convert());
			return cSGemData2;
		}
		return base.GetOutputChannelData(outputChannelIndex);
	}

	public override bool ReadJson(JObject archive, CSParaGraph graph, int major, int minor)
	{
		List<GemTableSizeExclusion> value;
		List<GemMinMaxInfo> value2;
		bool result = base.ReadJson(archive, graph, major, minor) & archive.ReadJsonValue<double>("Height", out Height) & archive.ReadJsonValue<double>("Width", out Width) & archive.ReadJsonValue<double>("Depth", out Depth) & archive.ReadJsonValue<double>("Percentage", out Percentage) & archive.ReadJsonValue<double>("MinX", out MinX) & archive.ReadJsonValue<double>("MaxX", out MaxX) & archive.ReadJsonValue<double>("MinY", out MinY) & archive.ReadJsonValue<double>("MaxY", out MaxY) & archive.ReadJsonObject<List<GemTableSizeExclusion>>("GemTableExclusions", out value) & archive.ReadJsonObject<List<GemMinMaxInfo>>("GemMinMaxInformation", out value2);
		GemTableSizeExclusions = value;
		GemMinMaxInfos = value2;
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
		flag &= archive.ReadDouble(ref MinX);
		flag &= archive.ReadDouble(ref MaxX);
		flag &= archive.ReadDouble(ref MinY);
		flag &= archive.ReadDouble(ref MaxY);
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
		string text = base.UserData.FindValue("GemTableSize", "Exclusions");
		if (!string.IsNullOrWhiteSpace(text))
		{
			try
			{
				XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(List<GemTableSizeExclusion>));
				StringReader textReader2 = new StringReader(text);
				GemTableSizeExclusions = xmlSerializer2.Deserialize(textReader2) as List<GemTableSizeExclusion>;
			}
			catch (Exception)
			{
			}
		}
		return flag;
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		archive.Add("Height", Height);
		archive.Add("Width", Width);
		archive.Add("Depth", Depth);
		archive.Add("Percentage", Percentage);
		archive.Add("MinX", MinX);
		archive.Add("MaxX", MaxX);
		archive.Add("MinY", MinY);
		archive.Add("MaxY", MaxY);
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
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<GemTableSizeExclusion>));
			StringWriter stringWriter = new StringWriter();
			xmlSerializer.Serialize(stringWriter, GemTableSizeExclusions);
			base.UserData.AddKey("GemTableSize", "Exclusions", stringWriter.ToString());
		}
		catch (Exception)
		{
		}
		bool flag = base.Write(ref archive);
		flag &= archive.WriteDouble(Height);
		flag &= archive.WriteDouble(Width);
		flag &= archive.WriteDouble(Depth);
		flag &= archive.WriteDouble(Percentage);
		flag &= archive.WriteDouble(MinX);
		flag &= archive.WriteDouble(MaxX);
		flag &= archive.WriteDouble(MinY);
		flag &= archive.WriteDouble(MaxY);
		try
		{
			XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(List<GemMinMaxInfo>));
			StringWriter stringWriter2 = new StringWriter();
			xmlSerializer2.Serialize(stringWriter2, GemMinMaxInfos);
			flag &= archive.WriteString(stringWriter2.ToString());
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

	public GemMinMaxInfo GetShapeSpecificGemMinMaxInfo()
	{
		CSGemData csGemData = GetCurrentCSGemData();
		GemMinMaxInfo result = null;
		if (csGemData != null && csGemData.Def != null)
		{
			result = GemMinMaxInfos.FirstOrDefault((GemMinMaxInfo gemInfo) => gemInfo.GemShape.Replace(" ", "") == csGemData.Def.Shape.Replace(" ", "") && gemInfo.GemType == csGemData.Def.Type);
		}
		return result;
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
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetDepthFromUI");
		if (Math.Abs(Depth - value) > 0.001 && value > 0.0)
		{
			PreChange("SetDepthFromUI");
			Depth = value;
			if (Width > 0.0)
			{
				Percentage = Depth / Width * 100.0;
			}
			Change();
		}
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetDepthFromUI");
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
		CSUtil.RedrawUI();
	}

	public void SetGlobalMinMaxFromUI(double minX, double maxX, double minY, double maxY)
	{
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetGlobalMinMaxFromUI");
		PreChange("SetGlobalMinMaxFromUI");
		MinX = minX;
		MaxX = maxX;
		MinY = minY;
		MaxY = maxY;
		Change();
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetGlobalMinMaxFromUI");
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
		CSUtil.RedrawUI();
	}

	public void SetHeightFromUI(double value)
	{
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetHeightFromUI");
		if (Math.Abs(Height - value) > 0.001 && value > 0.0)
		{
			PreChange("SetHeightFromUI");
			Height = value;
			Change();
		}
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetHeightFromUI");
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
		CSUtil.RedrawUI();
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
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetPercentageFromUI");
		if (Math.Abs(Percentage - value) > 0.001 && value > 0.0)
		{
			PreChange("SetPercentageFromUI");
			Percentage = value;
			Depth = Percentage / 100.0 * Width;
			Change();
		}
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetPercentageFromUI");
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
		CSUtil.RedrawUI();
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

	public void SetWidthFromUI(double value)
	{
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetWidthFromUI");
		if (Math.Abs(Width - value) > 0.001 && value > 0.0)
		{
			PreChange("SetWidthFromUI");
			Width = value;
			Percentage = Depth / Width * 100.0;
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
		if (!(setObject is CSValueGemSize cSValueGemSize))
		{
			return true;
		}
		Height = cSValueGemSize.Height;
		Width = cSValueGemSize.Width;
		Depth = cSValueGemSize.Depth;
		Percentage = cSValueGemSize.Percentage;
		return true;
	}

	public override bool SetStyle(CSObject setObject, bool showRewirePrompt = true)
	{
		base.SetStyle(setObject, showRewirePrompt);
		if (base.NotCreatedYet)
		{
			return true;
		}
		if (!(setObject is CSValueGemSize cSValueGemSize))
		{
			return true;
		}
		Height = cSValueGemSize.Height;
		Width = cSValueGemSize.Width;
		Depth = cSValueGemSize.Depth;
		Percentage = cSValueGemSize.Percentage;
		MinX = cSValueGemSize.MinX;
		MaxX = cSValueGemSize.MaxX;
		MinY = cSValueGemSize.MinY;
		MaxY = cSValueGemSize.MaxY;
		GemMinMaxInfos = cSValueGemSize.GemMinMaxInfos;
		return true;
	}

	public override bool SetUndo(CSObject setObject)
	{
		base.SetUndo(setObject);
		if (!base.NotCreatedYet && setObject is CSValueGemSize cSValueGemSize)
		{
			Height = cSValueGemSize.Height;
			Width = cSValueGemSize.Width;
			Depth = cSValueGemSize.Depth;
			Percentage = cSValueGemSize.Percentage;
			MinX = cSValueGemSize.MinX;
			MaxX = cSValueGemSize.MaxX;
			MinY = cSValueGemSize.MinY;
			MaxY = cSValueGemSize.MaxY;
			GemMinMaxInfos = cSValueGemSize.GemMinMaxInfos;
		}
		return true;
	}

	public int GetSizeTableCount()
	{
		if (InGemShape == null)
		{
			return 0;
		}
		if (InGemShape.OutputChannelBlock == null)
		{
			return 0;
		}
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
		if (currentCSGemData == null || currentCSGemData.Def == null)
		{
			return;
		}
		double num = 999.0;
		int sliderValueIndex = -1;
		List<GemSize> list = ValidSizes(currentCSGemData.Sizes);
		for (int i = 0; i < list.Count; i++)
		{
			GemSize gemSize = list[i];
			double num2 = gemSize.X * gemSize.Y * gemSize.Z * currentCSGemData.FFactor * 3.52;
			if (Math.Abs(num2 - carat) <= num)
			{
				num = Math.Abs(num2 - carat);
				sliderValueIndex = i;
			}
		}
		SetSizeValuesFromSliderIndex(sliderValueIndex);
	}

	public void SetSizeValuesFromNewGemShape(bool doPreChangeChange = true)
	{
		if (doPreChangeChange)
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetSizeValuesFromNewGemShape");
		}
		CSGemData currentCSGemData = GetCurrentCSGemData();
		if (currentCSGemData == null || currentCSGemData.Def == null)
		{
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetSizeValuesFromNewGemShape");
			return;
		}
		CSGemData previousCSGemData = PreviousCSGemData;
		if (previousCSGemData == null)
		{
			PreviousCSGemData = currentCSGemData.Duplicate() as CSGemData;
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetSizeValuesFromNewGemShape");
			return;
		}
		int sliderValueIndex = -1;
		double num = Width * Height * Depth * previousCSGemData.FFactor;
		double num2 = 999.0;
		List<GemSize> list = ValidSizes(currentCSGemData.Sizes);
		for (int i = 0; i < list.Count; i++)
		{
			GemSize gemSize = list[i];
			double num3 = gemSize.X * gemSize.Y * gemSize.Z * currentCSGemData.FFactor;
			if (Math.Abs(num3 - num) <= num2)
			{
				num2 = Math.Abs(num3 - num);
				sliderValueIndex = i;
			}
		}
		SetSizeValuesFromSliderIndex(sliderValueIndex);
		PreviousCSGemData = currentCSGemData.Duplicate() as CSGemData;
		if (doPreChangeChange)
		{
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetSizeValuesFromNewGemShape");
			CSApplicationHelper.Application.Document.Graph.Generate();
			CSUtil.Redraw();
		}
	}

	private void SetSizeValuesFromSliderIndex(int sliderValueIndex, bool doPreChangeChange = true)
	{
		CSGemData cSGemData = GetCurrentCSGemData();
		if (cSGemData == null || cSGemData.Def == null)
		{
			return;
		}
		if (sliderValueIndex < 0)
		{
			sliderValueIndex = 0;
		}
		if (doPreChangeChange)
		{
			PreChange("SetSizeValuesFromSliderIndex");
		}
		GemTableSizeExclusion shapeSpecificGemTableSizeExclusions = GetShapeSpecificGemTableSizeExclusions();
		int num = -1;
		int num2 = 0;
		List<GemSize> list = ValidSizes(cSGemData.Sizes);
		for (int i = 0; i < list.Count; i++)
		{
			if ((shapeSpecificGemTableSizeExclusions == null || !shapeSpecificGemTableSizeExclusions.ExcludedGemTableSizeIndexes.Contains(i)) && !(ValueMinX > list[i].X) && !(ValueMaxX < list[i].X) && !(ValueMinY > list[i].Y) && !(ValueMaxY < list[i].Y))
			{
				num++;
				if (num > sliderValueIndex)
				{
					break;
				}
				num2 = i;
			}
		}
		_currentTableIndex = num2;
		if (cSGemData.Def.IsGeneratedGem)
		{
			cSGemData = new CSGemData(cSGemData.Def.GenerateCSGemData(num2))
			{
				CurrentSizeTableIndex = num2
			};
			CSApplicationHelper.Application.Document.ReplaceGeneratedGem(cSGemData.Def);
		}
		list = ValidSizes(cSGemData.Sizes);
		Height = list.ElementAt(num2).Height;
		Width = list.ElementAt(num2).Width;
		Depth = list.ElementAt(num2).Depth;
		Percentage = list.ElementAt(num2).DepthPercentage;
		if (doPreChangeChange)
		{
			Change();
		}
	}

	private List<GemSize> ValidSizes(IEnumerable<GemSize> sizes, bool isCSDValidSize = false)
	{
		new List<GemSize>();
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
			list = ((!CSApplicationHelper.IsCounterSketchStudio || isCSDValidSize) ? source.Select((KeyValuePair<double, GemSize> _) => _.Value).ToList() : (from _ in source
				where _.Key <= CSApplicationHelper.Application.LargeStoneCaratMax
				select _.Value).ToList());
			if (!list.Any())
			{
				list = source.Select((KeyValuePair<double, GemSize> _) => _.Value).ToList();
			}
		}
		List<GemSize> list2 = FilterGemSizes(array, shapeSpecificGemTableSizeExclusions, list);
		if (list2 != null && !list2.Any())
		{
			IsAssignedCenter = false;
			list = source.Select((KeyValuePair<double, GemSize> _) => _.Value).ToList();
			list2 = FilterGemSizes(array, shapeSpecificGemTableSizeExclusions, list);
		}
		return list2;
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
