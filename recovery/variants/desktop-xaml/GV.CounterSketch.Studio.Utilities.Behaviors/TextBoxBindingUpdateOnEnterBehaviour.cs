using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

public class TextBoxBindingUpdateOnEnterBehaviour : Behavior<TextBox>
{
	protected override void OnAttached()
	{
		base.AssociatedObject.KeyDown += OnTextBoxKeyDown;
	}

	protected override void OnDetaching()
	{
		base.AssociatedObject.KeyDown -= OnTextBoxKeyDown;
	}

	private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Return)
		{
			(sender as TextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
		}
	}
}
