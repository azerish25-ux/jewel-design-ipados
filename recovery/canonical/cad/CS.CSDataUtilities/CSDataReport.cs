using System.Collections.Generic;
using System.IO;
using System.Text;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CS.CSDataUtilities;

public static class CSDataReport
{
	private static readonly StringBuilder _dataReportWriter = new StringBuilder();

	private static bool _doWriteBlockIdentifiers;

	private static bool _doWriteBlockInstanceGuids;

	private static bool _doWriteBlockTypes;

	private static bool _doWriteChannelIdentifiers;

	private static bool _doWritePatchwireIdentifiers;

	private static bool _isBlockLoggingEnabled;

	public static string DataReportWriterFinish(string pathFileOutput = null)
	{
		_isBlockLoggingEnabled = false;
		string text = _dataReportWriter.ToString();
		_dataReportWriter.Clear();
		if (!string.IsNullOrWhiteSpace(pathFileOutput))
		{
			File.WriteAllText(pathFileOutput, text);
		}
		return text;
	}

	public static void DataReportWriterStart(bool doWritePatchwireIdentifiers = false, bool doWriteBlockTypes = false, bool doWriteBlockInstanceGuids = false, bool doWriteBlockIdentifiers = false, bool doWriteChannelIdentifiers = false)
	{
		_isBlockLoggingEnabled = true;
		if (_dataReportWriter.Length > 0)
		{
			_dataReportWriter.Clear();
		}
		_doWritePatchwireIdentifiers = doWritePatchwireIdentifiers;
		_doWriteBlockTypes = doWriteBlockTypes;
		_doWriteBlockInstanceGuids = doWriteBlockInstanceGuids;
		_doWriteBlockIdentifiers = doWriteBlockIdentifiers;
		_doWriteChannelIdentifiers = doWriteChannelIdentifiers;
	}

	public static void PrintBlockInputs(CSBlock block)
	{
		if (!_isBlockLoggingEnabled)
		{
			return;
		}
		_dataReportWriter.AppendLine(string.Empty);
		_dataReportWriter.AppendLine("-------------------------------------------------------------------------------------");
		_dataReportWriter.AppendLine(string.Empty);
		string blockHeader = GetBlockHeader(block);
		_dataReportWriter.AppendLine(blockHeader);
		_dataReportWriter.AppendLine($"OutputDisplayMaterialClassification = {block.OutputDisplayMaterialClassification}");
		_dataReportWriter.AppendLine($"InputChannel.Count = {block.InputChannels.Count}");
		for (int i = 0; i < block.InputChannels.Count; i++)
		{
			CSInputChannel cSInputChannel = block.InputChannels[i];
			if (cSInputChannel == null)
			{
				_dataReportWriter.AppendLine("InputChannel[" + i + "] is NULL");
				continue;
			}
			if (_doWriteChannelIdentifiers)
			{
				_dataReportWriter.AppendLine("InputChannel[" + i + "] : " + cSInputChannel.Identifier);
			}
			else
			{
				_dataReportWriter.AppendLine("InputChannel[" + i + "]");
			}
			Print(cSInputChannel.Data);
		}
	}

	public static void PrintBlockOutputs(CSBlock block)
	{
		if (!_isBlockLoggingEnabled)
		{
			return;
		}
		_dataReportWriter.AppendLine(string.Empty);
		_dataReportWriter.AppendLine("-------------------------------------------------------------------------------------");
		_dataReportWriter.AppendLine(string.Empty);
		string blockHeader = GetBlockHeader(block);
		_dataReportWriter.AppendLine(blockHeader);
		_dataReportWriter.AppendLine($"OutputDisplayMaterialClassification = {block.OutputDisplayMaterialClassification}");
		_dataReportWriter.AppendLine($"OutputChannel.Count = {block.OutputChannels.Count}");
		for (int i = 0; i < block.OutputChannels.Count; i++)
		{
			CSOutputChannel outputChannel = block.GetOutputChannel(i);
			if (outputChannel == null)
			{
				_dataReportWriter.AppendLine("OutputChannel[" + i + "] is NULL");
				continue;
			}
			if (_doWriteChannelIdentifiers)
			{
				_dataReportWriter.AppendLine("OutputChannel[" + i + "] : " + outputChannel.Identifier);
			}
			else
			{
				_dataReportWriter.AppendLine("OutputChannel[" + i + "]");
			}
			Print(block.GetOutputChannelData(i));
		}
	}

	public static string PrintCurve(Curve c)
	{
		return PrintGeometry((OnCurve)Interop.ToOnCurve(c));
	}

	public static string PrintGeometry(IOnGeometry geo)
	{
		if (geo == null)
		{
			return "NULL";
		}
		using StringWriter stringWriter = new StringWriter();
		OnTextLog arg = new OnTextLog(stringWriter);
		geo.Dump(ref arg);
		string result = stringWriter.ToString();
		stringWriter.Flush();
		return result;
	}

	public static string PrintGeometry(GeometryBase geo)
	{
		if (geo == null)
		{
			return "NULL";
		}
		OnObject onObject = CSConvert.ToOnObject(geo);
		if (onObject == null)
		{
			return "Failed convert";
		}
		using StringWriter stringWriter = new StringWriter();
		OnTextLog arg = new OnTextLog(stringWriter);
		onObject.Dump(ref arg);
		string result = stringWriter.ToString();
		stringWriter.Flush();
		return result;
	}

	public static string PrintPoint(IOn3dPoint pt)
	{
		return $"{pt.x}, {pt.y}, {pt.z}";
	}

	public static string PrintPoint(Point3d pt)
	{
		return $"{pt.X}, {pt.Y}, {pt.Z}";
	}

	public static string PrintTransform(Transform t)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				stringBuilder.Append(t[i, j] + " ");
			}
		}
		return stringBuilder.ToString();
	}

	public static string PrintTransform(OnXform t)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				stringBuilder.Append(t[i, j] + " ");
			}
		}
		return stringBuilder.ToString();
	}

	public static string PrintVector(IOn3dVector v)
	{
		return $"{v.x}, {v.y}, {v.z}";
	}

	public static string PrintVector(Vector3d v)
	{
		return $"{v.X}, {v.Y}, {v.Z}";
	}

	private static string GetBlockHeader(CSObject block)
	{
		List<string> list = new List<string>();
		if (_doWriteBlockIdentifiers)
		{
			list.Add(block.Identifier);
		}
		if (_doWriteBlockInstanceGuids)
		{
			list.Add(block.InstanceGuid.ToString());
		}
		if (_doWriteBlockTypes)
		{
			list.Add(block.GetType().ToString());
		}
		if (_doWritePatchwireIdentifiers)
		{
			string item = ((block.ParentPatchWire != null) ? block.ParentPatchWire.Identifier : "NULL");
			list.Add(item);
		}
		string result = string.Empty;
		if (list.Count > 0)
		{
			result = string.Join(" : ", list);
		}
		return result;
	}

	private static void Print(CSData data, int depth = 0)
	{
		if (data == null)
		{
			_dataReportWriter.AppendLine($"{depth} : Data is NULL");
			return;
		}
		_dataReportWriter.AppendLine($"{depth} : Data is {data.GetType()}");
		_dataReportWriter.AppendLine($"{depth} : CurrentMaterialGroup = {data.CurrentMaterialGroup}");
		_dataReportWriter.AppendLine($"{depth} : DefaultDisplayMaterialClassification = {data.DefaultDisplayMaterialClassification}");
		if (data is CSDataArray csda)
		{
			Print(csda, depth);
		}
		else if (data is CSGemData csgd)
		{
			Print(csgd, depth);
		}
		else if (data is CSNumberData csnd)
		{
			Print(csnd, depth);
		}
		else if (data is CSOnGeometryData csogd)
		{
			Print(csogd, depth);
		}
		else if (data is CSOutsideRingRailData csosrrd)
		{
			Print(csosrrd, depth);
		}
		else if (data is CSPointData cspd)
		{
			Print(cspd, depth);
		}
		else if (data is CSProfileData csprd)
		{
			Print(csprd, depth);
		}
		else if (data is CSRingRailData csrrd)
		{
			Print(csrrd, depth);
		}
		else if (data is CSStringData cssd)
		{
			Print(cssd, depth);
		}
		else if (data is CSVectorData csvd)
		{
			Print(csvd, depth);
		}
		else if (data is CSXformData csxfd)
		{
			Print(csxfd, depth);
		}
	}

	private static void Print(CSDataArray csda, int depth)
	{
		int arrayCount = csda.GetArrayCount();
		_dataReportWriter.AppendLine($"{depth} : _array.Count = {arrayCount}");
		for (int i = 0; i < arrayCount; i++)
		{
			Print(csda.GetArrayItem(i), depth + 1);
		}
	}

	private static void Print(CSGemData csgd, int depth)
	{
		_dataReportWriter.AppendLine($"{depth} : _type = {csgd.GemType}");
		_dataReportWriter.AppendLine($"{depth} : _shape = {csgd.GemShape}");
		Transform value;
		string arg = (csgd.GetTransform(out value) ? PrintTransform(value) : "NULL");
		_dataReportWriter.AppendLine($"{depth} : _xform = {arg}");
	}

	private static void Print(CSNumberData csnd, int depth)
	{
		double value = 0.0;
		csnd.GetDouble(ref value);
		_dataReportWriter.AppendLine($"{depth} : _numberData = {value}");
	}

	private static void Print(CSOnGeometryData csogd, int depth)
	{
		_dataReportWriter.AppendLine($"{depth} : _geometry = {PrintGeometry(csogd.GetGeometry())}");
		csogd.GetTransform(out var value);
		_dataReportWriter.AppendLine($"{depth} : _xform = {PrintTransform(value)}");
	}

	private static void Print(CSOutsideRingRailData csosrrd, int depth)
	{
		_dataReportWriter.AppendLine($"{depth} : _scale = {csosrrd.Scale}");
		Print((CSRailData)csosrrd, depth);
	}

	private static void Print(CSPointData cspd, int depth)
	{
		Point3d point;
		string arg = (cspd.GetPoint(out point) ? (point.X + " : " + point.Y + " : " + point.Z) : "NULL");
		_dataReportWriter.AppendLine($"{depth} : _pointStore = {arg}");
	}

	private static void Print(CSProfileData csprd, int depth)
	{
		csprd.GetTransform(out var value);
		string arg = (value.IsValid ? PrintTransform(value) : "Unset");
		_dataReportWriter.AppendLine($"{depth} : _crv = {PrintGeometry(csprd.GetGeometry())} : _xfrom = {arg}");
	}

	private static void Print(CSRailData csrd, int depth)
	{
		_dataReportWriter.AppendLine($"{depth} : _crv = {PrintGeometry(csrd.GetGeometry())}");
		Print((CSXformData)csrd, depth);
	}

	private static void Print(CSRingRailData csrrd, int depth)
	{
		_dataReportWriter.AppendLine($"{depth} : _radius = {csrrd.Radius}");
		Print((CSRailData)csrrd, depth);
	}

	private static void Print(CSStringData cssd, int depth)
	{
		string output = null;
		cssd.GetString(ref output);
		_dataReportWriter.AppendLine($"{depth} : _data = {output}");
	}

	private static void Print(CSVectorData csvd, int depth)
	{
		Vector3d value;
		string arg = (csvd.GetVector(out value) ? (value.X + " : " + value.Y + " : " + value.Z) : "NULL");
		_dataReportWriter.AppendLine($"{depth} : _vectorStore = {arg}");
	}

	private static void Print(CSXformData csxfd, int depth)
	{
		Transform value;
		string arg = (csxfd.GetTransform(out value) ? PrintTransform(value) : "NULL");
		_dataReportWriter.AppendLine($"{depth} : _xform = {arg}");
	}
}
