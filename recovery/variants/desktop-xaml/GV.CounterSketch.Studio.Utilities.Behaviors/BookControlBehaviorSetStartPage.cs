using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using WPFMitsuControls;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to set the initial page of a WPFMitsuControls.Book to 1 when the ItemSource is updated.")]
public class BookControlBehaviorSetStartPage : Behavior<Book>
{
	protected override void OnAttached()
	{
		base.OnAttached();
		base.AssociatedObject.ItemContainerGenerator.ItemsChanged += ItemContainerGenerator_ItemsChanged;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();
		base.AssociatedObject.ItemContainerGenerator.ItemsChanged -= ItemContainerGenerator_ItemsChanged;
	}

	private void ItemContainerGenerator_ItemsChanged(object sender, ItemsChangedEventArgs e)
	{
		if (base.AssociatedObject.CurrentSheetIndex == 0 && base.AssociatedObject.Items.Count > 1)
		{
			base.AssociatedObject.CurrentSheetIndex = 1;
		}
	}
}
