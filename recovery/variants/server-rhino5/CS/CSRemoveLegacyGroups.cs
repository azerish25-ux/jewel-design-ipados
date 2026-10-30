using System.Collections.Generic;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.CSObjects.CSTreeObjects;
using CS.CSObjects.ObsoleteGrouping;
using CS.Parametric.Channels;

namespace CS;

public static class CSRemoveLegacyGroups
{
	public static void Delete()
	{
		RelinkBlocks();
		RePositionAllGroupBlocks();
		DeleteGroupBlocks();
	}

	public static void Remove()
	{
		RelinkBlocks();
		RePositionAllGroupBlocks();
		CleanUpGroupBlocks();
	}

	private static void CleanUpGroupBlocks()
	{
		List<CSObject> graphList = CSApplicationHelper.Application.Document.Graph.GraphList;
		int count = graphList.Count;
		for (int i = 0; i < count; i++)
		{
			CSObject cSObject = graphList[i];
			if (cSObject is CSGroupStartObsolete cSGroupStartObsolete)
			{
				cSGroupStartObsolete.InputChannels.Clear();
				cSGroupStartObsolete.OutputChannels.Clear();
			}
			if (cSObject is CSGroupEndObsolete cSGroupEndObsolete)
			{
				cSGroupEndObsolete.InputChannels.Clear();
				cSGroupEndObsolete.OutputChannels.Clear();
			}
		}
	}

	private static void DeleteGroupBlocks()
	{
		List<CSObject> graphList = CSApplicationHelper.Application.Document.Graph.GraphList;
		int num = graphList.Count;
		for (int i = 0; i < num; i++)
		{
			CSObject cSObject = graphList[i];
			if (cSObject is CSGroupStartObsolete cSGroupStartObsolete)
			{
				cSGroupStartObsolete.InputChannels.Clear();
				cSGroupStartObsolete.OutputChannels.Clear();
				graphList.RemoveAt(i);
				i--;
				num--;
			}
			if (cSObject is CSGroupEndObsolete cSGroupEndObsolete)
			{
				cSGroupEndObsolete.InputChannels.Clear();
				cSGroupEndObsolete.OutputChannels.Clear();
				graphList.RemoveAt(i);
				i--;
				num--;
			}
		}
	}

	private static void RelinkBlocks()
	{
		bool flag = true;
		while (flag)
		{
			flag = false;
			foreach (CSBlock cSBlock in CSApplicationHelper.Application.Document.Graph.CSBlocks)
			{
				int count = cSBlock.InputChannels.Count;
				for (int i = 0; i < count; i++)
				{
					CSInputChannel cSInputChannel = cSBlock.InputChannels[i];
					if (cSInputChannel.OutputChannelBlock is CSGroupStartObsolete cSGroupStartObsolete)
					{
						flag = true;
						CSInputChannel cSInputChannel2 = cSGroupStartObsolete.InputChannels[cSInputChannel.OutputChannelIndex];
						cSInputChannel.OutputChannelBlock = cSInputChannel2.OutputChannelBlock;
						cSInputChannel.OutputChannelIndex = cSInputChannel2.OutputChannelIndex;
					}
					if (cSInputChannel.OutputChannelBlock is CSGroupEndObsolete cSGroupEndObsolete)
					{
						flag = true;
						CSInputChannel cSInputChannel3 = cSGroupEndObsolete.InputChannels[cSInputChannel.OutputChannelIndex];
						cSInputChannel.OutputChannelBlock = cSInputChannel3.OutputChannelBlock;
						cSInputChannel.OutputChannelIndex = cSInputChannel3.OutputChannelIndex;
					}
				}
			}
		}
	}

	private static void RePositionAllGroupBlocks()
	{
		foreach (CSObject graph in CSApplicationHelper.Application.Document.Graph.GraphList)
		{
			if (graph is CSSuperViewGroup cSSuperViewGroup)
			{
				for (int i = 0; i < cSSuperViewGroup.ChildCount(); i++)
				{
					CSObject cSObject = cSSuperViewGroup.Child(i);
					cSObject.Parent = cSSuperViewGroup.Parent;
					cSObject.XPosLocal += cSSuperViewGroup.XPosLocal;
					cSObject.YPosLocal += cSSuperViewGroup.YPosLocal;
				}
				cSSuperViewGroup.ChildClear();
			}
		}
	}
}
