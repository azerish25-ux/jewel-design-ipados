using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GemvisionCommon.UserInterfaceExtensions;

public static class ComboBoxExtensions
{
	public enum DefaultStrategy
	{
		First,
		Last
	}

	public static void Load(this ComboBox cmb, IEnumerable<int> items, DefaultStrategy whichDefaultStrategy = DefaultStrategy.Last, object initialSelectedItem = null)
	{
		cmb.Items.Clear();
		cmb.ResetText();
		if (items == null)
		{
			return;
		}
		foreach (int item in items)
		{
			cmb.Items.Add(item);
		}
		if (!cmb.TrySetSelectedItem(initialSelectedItem))
		{
			cmb.TrySetSelectedItem(whichDefaultStrategy);
		}
	}

	public static bool TrySetSelectedItem(this ComboBox cmb, int restoreItem)
	{
		if (cmb.Items.Count <= 0)
		{
			return false;
		}
		object obj = cmb.Items.Cast<object>().FirstOrDefault((object _) => _.Equals(restoreItem));
		if (obj == null)
		{
			return false;
		}
		cmb.SelectedItem = obj;
		return true;
	}

	public static void Load(this ComboBox cmb, IEnumerable<object> items, DefaultStrategy whichDefaultStrategy = DefaultStrategy.Last, object initialSelectedItem = null)
	{
		cmb.Items.Clear();
		cmb.ResetText();
		if (items != null)
		{
			cmb.Items.AddRange(items.ToArray());
			if (!cmb.TrySetSelectedItem(initialSelectedItem))
			{
				cmb.TrySetSelectedItem(whichDefaultStrategy);
			}
		}
	}

	public static bool TrySetSelectedItem(this ComboBox cmb, object restoreItem)
	{
		if (cmb.Items.Count <= 0)
		{
			return false;
		}
		if (restoreItem == null)
		{
			return false;
		}
		object obj = cmb.Items.Cast<object>().FirstOrDefault((object _) => _.Equals(restoreItem));
		if (obj == null)
		{
			return false;
		}
		cmb.SelectedItem = obj;
		return true;
	}

	public static bool TrySetSelectedItem(this ComboBox cmb, DefaultStrategy whichDefaultStrategy)
	{
		if (cmb.Items.Count <= 0)
		{
			return false;
		}
		switch (whichDefaultStrategy)
		{
		case DefaultStrategy.First:
			cmb.SelectedIndex = 0;
			break;
		case DefaultStrategy.Last:
			cmb.SelectedIndex = cmb.Items.Count - 1;
			break;
		default:
			return false;
		}
		return true;
	}
}
