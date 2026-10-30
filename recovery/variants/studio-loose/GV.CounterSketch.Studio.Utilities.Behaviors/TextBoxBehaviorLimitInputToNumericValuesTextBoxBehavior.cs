using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to limit the input of characters in the textbox to numeric only.")]
public class TextBoxBehaviorLimitInputToNumericValuesTextBoxBehavior : Behavior<TextBox>
{
	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.PreviewTextInput += AssociatedObject_PreviewTextInput;
	}

	protected override void OnDetaching()
	{
		((Behavior)this).OnDetaching();
		base.AssociatedObject.PreviewTextInput -= AssociatedObject_PreviewTextInput;
	}

	private static void AssociatedObject_PreviewTextInput(object sender, TextCompositionEventArgs e)
	{
		bool flag = false;
		if (!(sender is TextBox textBox))
		{
			return;
		}
		if (e.Text == ".")
		{
			if (!textBox.Text.Contains("."))
			{
				flag = true;
			}
		}
		else
		{
			flag = CheckIfNumericKey(e.Text);
		}
		if (!flag)
		{
			e.Handled = true;
		}
	}

	private static bool CheckIfNumericKey(string input)
	{
		if (double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out var _))
		{
			return true;
		}
		return false;
	}
}
