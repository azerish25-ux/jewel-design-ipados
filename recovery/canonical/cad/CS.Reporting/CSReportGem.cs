using System;
using CSDatas;
using GV.CounterSketch.Shared;
using RMA.OpenNURBS;

namespace CS.Reporting;

public sealed class CSReportGem
{
	public ComponentsEnum WhichComponent;

	public double FFactor;

	public On3dPoint GemCenter;

	public CSGemData GemData;

	public GemPlace GemPlacement;

	public double GemW;

	public double GemWMax;

	public double GemWMin;

	public double GemX;

	public double GemXMax;

	public double GemXMin;

	public double GemY;

	public double GemYMax;

	public double GemYMin;

	public double GemZ;

	public string Identifier;

	public GemSettingType WhichSettingType;

	public string Name;

	public string Shape;

	public string GetGemInfoOrderString()
	{
		string empty = string.Empty;
		empty = empty + Shape + " X = " + Math.Round(GemX, 2) + ", Y = " + Math.Round(GemY, 2) + ", Z = " + Math.Round(GemZ, 2) + ", Ct. = " + Math.Round(GemW, 2);
		return empty + " (" + Enum.GetName(typeof(GemPlace), GemPlacement) + ")";
	}

	public string GetReportString()
	{
		string empty = string.Empty;
		empty = empty + Shape + " Y=" + Math.Round(GemY, 2) + " X=" + Math.Round(GemX, 2) + " Z=" + Math.Round(GemZ, 2) + " Ct.=" + Math.Round(GemW, 2);
		empty = empty + " Setting Type:" + WhichSettingType;
		empty = empty + "    : " + Name;
		return string.Concat(empty, " (", GemPlacement, ")");
	}

	public void SetWeights(double weight = 3.52)
	{
		GemW = GemX * GemY * GemZ * FFactor * weight;
		double num = GemZ * ((GemXMax / GemX + GemYMax / GemY) / 2.0);
		double num2 = GemZ * ((GemXMin / GemX + GemYMin / GemY) / 2.0);
		GemWMax = GemXMax * GemYMax * num * FFactor * weight;
		GemWMin = GemXMin * GemYMin * num2 * FFactor * weight;
	}
}
