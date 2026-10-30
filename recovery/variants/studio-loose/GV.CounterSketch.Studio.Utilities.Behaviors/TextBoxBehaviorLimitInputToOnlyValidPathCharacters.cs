using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to limit the input of characters in the textbox to valid path characters only.")]
public class TextBoxBehaviorLimitInputToOnlyValidPathCharacters : Behavior<TextBox>
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
		char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
		e.Handled = e.Text.IndexOfAny(invalidFileNameChars) >= 0;
	}
}
