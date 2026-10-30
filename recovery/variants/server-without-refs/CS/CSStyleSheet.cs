using System.Linq;
using CS.CSObjects;
using CS.CSObjects.CSTreeObjects;

namespace CS;

public sealed class CSStyleSheet : CSParaGraph
{
	public bool CreateStyleSheetFromMain(CSObject obj)
	{
		int num = -1;
		for (int i = 0; i < CSApplicationHelper.Application.Document.Graph.GraphList.Count; i++)
		{
			if (CSApplicationHelper.Application.Document.Graph.GraphList[i] == obj)
			{
				num = i;
				break;
			}
		}
		if (num < 0)
		{
			return false;
		}
		int num2 = 0;
		for (int i = num; i >= 0; i--)
		{
			CSApplicationHelper.Application.Document.Graph.GraphList[i].MouseDownObject = true;
			if (CSApplicationHelper.Application.Document.Graph.GraphList[i] is CSPatchWireStart)
			{
				num2 = i;
				break;
			}
		}
		int num3 = CSApplicationHelper.Application.Document.Graph.GraphList.Count - 1;
		for (int i = num + 1; i < CSApplicationHelper.Application.Document.Graph.GraphList.Count; i++)
		{
			if (CSApplicationHelper.Application.Document.Graph.GraphList[i] is CSPatchWireStart)
			{
				num3 = i - 1;
				break;
			}
		}
		for (int i = num2; i <= num3; i++)
		{
			CSObject cSObject = CSApplicationHelper.Application.Document.Graph.GraphList[i].CreateTempObject();
			cSObject.SetStyle(CSApplicationHelper.Application.Document.Graph.GraphList[i], showRewirePrompt: false);
			base.GraphList.Add(cSObject);
		}
		return true;
	}

	public CSPatchWireStart GetPatchwireStart()
	{
		if (base.GraphList != null && base.GraphList.Any())
		{
			return base.GraphList[0] as CSPatchWireStart;
		}
		return null;
	}

	public bool SetStyleSheetToMain(CSObject obj, bool showRewirePrompt)
	{
		int num = -1;
		for (int i = 0; i < CSApplicationHelper.Application.Document.Graph.GraphList.Count; i++)
		{
			if (CSApplicationHelper.Application.Document.Graph.GraphList[i] == obj)
			{
				num = i;
				break;
			}
		}
		if (num < 0)
		{
			return false;
		}
		int num2 = 0;
		for (int i = num; i >= 0; i--)
		{
			CSApplicationHelper.Application.Document.Graph.GraphList[i].MouseDownObject = true;
			if (CSApplicationHelper.Application.Document.Graph.GraphList[i] is CSPatchWireStart)
			{
				num2 = i;
				break;
			}
		}
		int num3 = CSApplicationHelper.Application.Document.Graph.GraphList.Count - 1;
		for (int i = num + 1; i < CSApplicationHelper.Application.Document.Graph.GraphList.Count; i++)
		{
			if (CSApplicationHelper.Application.Document.Graph.GraphList[i] is CSPatchWireStart)
			{
				num3 = i - 1;
				break;
			}
		}
		int num4 = 0;
		for (int i = num2; i <= num3; i++)
		{
			if (num4 >= base.GraphList.Count)
			{
				break;
			}
			CSApplicationHelper.Application.Document.Graph.GraphList[i].PreChange("SetStyleSheetToMain");
			CSApplicationHelper.Application.Document.Graph.GraphList[i].SetStyle(base.GraphList[num4], showRewirePrompt);
			CSApplicationHelper.Application.Document.Graph.GraphList[i].PreChange("SetStyleSheetToMain");
			num4++;
		}
		return true;
	}
}
