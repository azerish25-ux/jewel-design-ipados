using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CSDatas;
using CSDatas.Interfaces;

namespace CS.Reporting;

public sealed class CSReporting
{
	public static string GetBlockIdentifier(CSBlock iObj)
	{
		if (iObj == null)
		{
			return "Empty";
		}
		string identifier = iObj.Identifier;
		if (identifier != null)
		{
			return identifier;
		}
		return iObj.GetType().ToString();
	}

	public static string GetDataArrayCountString(CSData data)
	{
		double num = 0.0;
		if (data != null)
		{
			num = CSDataHelper.GetArrayCount(data);
		}
		if (num > 0.0)
		{
			return "[" + num + "]" + GetDataArrayCountString(CSDataHelper.GetArrayItem(data, 0));
		}
		return string.Empty;
	}

	public static IOReportingTypes GetDataType(CSData data)
	{
		if (data == null)
		{
			return IOReportingTypes.Nothing;
		}
		if (!(data is ICSArray iCSArray))
		{
			if (data is CSGemData)
			{
				return IOReportingTypes.Gems;
			}
			double value = 0.0;
			if (data.GetDouble(ref value))
			{
				return IOReportingTypes.Values;
			}
			if (data.HasNurbsCurve())
			{
				return IOReportingTypes.Curves;
			}
			if (data.HasNurbsSurface() || data.HasBrep())
			{
				return IOReportingTypes.Surfaces;
			}
			if (data.HasMesh())
			{
				return IOReportingTypes.Surfaces;
			}
		}
		else
		{
			if (iCSArray.GetArrayCount() <= 0)
			{
				return IOReportingTypes.Nothing;
			}
			IOReportingTypes dataType = GetDataType(iCSArray.GetArrayItem(0));
			for (int i = 1; i < iCSArray.GetArrayCount(); i++)
			{
				IOReportingTypes dataType2 = GetDataType(iCSArray.GetArrayItem(i));
				if (dataType != dataType2)
				{
					return IOReportingTypes.MiscArray;
				}
			}
			switch (dataType)
			{
			case IOReportingTypes.Values:
			case IOReportingTypes.ValuesArray:
				return IOReportingTypes.ValuesArray;
			case IOReportingTypes.Gems:
			case IOReportingTypes.GemsArray:
				return IOReportingTypes.GemsArray;
			case IOReportingTypes.Curves:
			case IOReportingTypes.CurvesArray:
				return IOReportingTypes.CurvesArray;
			case IOReportingTypes.Surfaces:
			case IOReportingTypes.SurfacesArray:
				return IOReportingTypes.SurfacesArray;
			}
		}
		return IOReportingTypes.Everything;
	}

	public static string GetInputName(CSBlock iObj, int iIndex)
	{
		if (iObj == null)
		{
			return "Empty";
		}
		string text = GetBlockIdentifier(iObj);
		string inputChannelIdentifier = iObj.GetInputChannelIdentifier(iIndex);
		if (inputChannelIdentifier != null)
		{
			text = text + " : " + inputChannelIdentifier;
		}
		return text;
	}

	public static string GetOutputIdentifier(CSBlock iObj, int iIndex)
	{
		if (iObj == null)
		{
			return "Empty";
		}
		string text = GetBlockIdentifier(iObj);
		string outputChannelIdentifier = iObj.GetOutputChannelIdentifier(iIndex);
		if (outputChannelIdentifier != null)
		{
			text = text + " : " + outputChannelIdentifier;
		}
		return text + " : " + GetOutputTypeString(iObj.GetOutputChannelData(iIndex));
	}

	public static string GetOutputTypeString(CSData data)
	{
		return GetOutputTypeString(GetDataType(data)) + GetDataArrayCountString(data);
	}

	public static string GetOutputTypeString(IOReportingTypes filterType)
	{
		string result = "Unknown";
		switch (filterType)
		{
		case IOReportingTypes.Nothing:
			result = "Nothing";
			break;
		case IOReportingTypes.Everything:
			result = "Everything";
			break;
		case IOReportingTypes.Values:
			result = "Value";
			break;
		case IOReportingTypes.Gems:
			result = "Gem";
			break;
		case IOReportingTypes.Curves:
			result = "Curve";
			break;
		case IOReportingTypes.Surfaces:
			result = "Surface";
			break;
		case IOReportingTypes.ValuesArray:
			result = "Array of Values";
			break;
		case IOReportingTypes.GemsArray:
			result = "Array of Gems";
			break;
		case IOReportingTypes.CurvesArray:
			result = "Array of Curves";
			break;
		case IOReportingTypes.SurfacesArray:
			result = "Array of Surfaces";
			break;
		case IOReportingTypes.MiscArray:
			result = "Array of Miscellaneous";
			break;
		}
		return result;
	}
}
