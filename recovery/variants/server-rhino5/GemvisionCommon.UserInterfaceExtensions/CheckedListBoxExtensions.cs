using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GemvisionCommon.UserInterfaceExtensions;

public static class CheckedListBoxExtensions
{
	public enum DefaultStrategy
	{
		CheckAll,
		UncheckAll
	}

	public static void CheckAll(this CheckedListBox chkLst)
	{
		for (int i = 0; i < chkLst.Items.Count; i++)
		{
			if (!chkLst.GetItemChecked(i))
			{
				chkLst.SetItemChecked(i, value: true);
			}
		}
	}

	public static void UncheckAll(this CheckedListBox chkLst)
	{
		for (int i = 0; i < chkLst.Items.Count; i++)
		{
			if (chkLst.GetItemChecked(i))
			{
				chkLst.SetItemChecked(i, value: false);
			}
		}
	}

	public static void Load(this CheckedListBox chkLst, IEnumerable<object> items, DefaultStrategy whichDefaultStrategy = DefaultStrategy.UncheckAll, IEnumerable<object> restoreItems = null)
	{
		chkLst.Items.Clear();
		chkLst.ResetText();
		if (items != null)
		{
			chkLst.Items.AddRange(items.ToArray());
			if (!chkLst.TrySetCheckedItems(restoreItems))
			{
				chkLst.TrySetCheckedItems(whichDefaultStrategy);
			}
		}
	}

	public static bool TrySetCheckedItems(this CheckedListBox chkLst, IEnumerable<object> restoreItems)
	{
		if (chkLst.Items.Count <= 0)
		{
			return false;
		}
		if (restoreItems == null)
		{
			return false;
		}
		bool flag = true;
		foreach (object restoreItem in restoreItems)
		{
			int num = chkLst.Items.IndexOf(restoreItem);
			flag = flag && num >= 0;
			if (num >= 0)
			{
				chkLst.SetItemChecked(num, value: true);
			}
		}
		return flag;
	}

	public static bool TrySetCheckedItems(this CheckedListBox chkLst, DefaultStrategy whichDefaultStrategy)
	{
		if (chkLst.Items.Count <= 0)
		{
			return false;
		}
		switch (whichDefaultStrategy)
		{
		case DefaultStrategy.CheckAll:
			chkLst.CheckAll();
			break;
		case DefaultStrategy.UncheckAll:
			chkLst.UncheckAll();
			break;
		default:
			return false;
		}
		return false;
	}
}
