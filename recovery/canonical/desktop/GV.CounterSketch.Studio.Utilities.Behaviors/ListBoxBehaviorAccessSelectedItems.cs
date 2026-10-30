using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to gain access to the selected items collection of the Listbox, which is not bindable otherwise.")]
public class ListBoxBehaviorAccessSelectedItems : Behavior<ListBox>
{
	public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(ObservableCollection<object>), typeof(ListBoxBehaviorAccessSelectedItems), new PropertyMetadata(new ObservableCollection<object>()));

	public ObservableCollection<object> SelectedItems
	{
		get
		{
			return (ObservableCollection<object>)GetValue(SelectedItemsProperty);
		}
		set
		{
			SetValue(SelectedItemsProperty, value);
		}
	}

	protected override void OnAttached()
	{
		base.OnAttached();
		base.AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();
		base.AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
	}

	private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		SelectedItems.Clear();
		foreach (object selectedItem in base.AssociatedObject.SelectedItems)
		{
			SelectedItems.Add(selectedItem);
		}
		BindingOperations.GetBindingExpression(this, SelectedItemsProperty)?.UpdateSource();
	}
}
