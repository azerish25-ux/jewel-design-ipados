using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to adjust various appearance settings for items in the item container.")]
public class ItemsControlBehaviorAdjustItemAppearance : Behavior<ItemsControl>
{
	public static readonly DependencyProperty EndingOpacityFactorProperty = DependencyProperty.Register("EndingOpacityFactor", typeof(double), typeof(ItemsControlBehaviorAdjustItemAppearance));

	public static readonly DependencyProperty EndingScaleFactorProperty = DependencyProperty.Register("EndingScaleFactor", typeof(double), typeof(ItemsControlBehaviorAdjustItemAppearance));

	public static readonly DependencyProperty NumberOfItemsToAffectProperty = DependencyProperty.Register("NumberOfItemsToAffect", typeof(int), typeof(ItemsControlBehaviorAdjustItemAppearance));

	public static readonly DependencyProperty ReverseOrderProperty = DependencyProperty.Register("ReverseOrder", typeof(bool), typeof(ItemsControlBehaviorAdjustItemAppearance));

	public static readonly DependencyProperty ShowUnaffectedItemsProperty = DependencyProperty.Register("ShowUnaffectedItems", typeof(bool?), typeof(ItemsControlBehaviorAdjustItemAppearance), new UIPropertyMetadata(false));

	public static readonly DependencyProperty StartingOpacityFactorProperty = DependencyProperty.Register("StartingOpacityFactor", typeof(double), typeof(ItemsControlBehaviorAdjustItemAppearance));

	public static readonly DependencyProperty StartingScaleFactorProperty = DependencyProperty.Register("StartingScaleFactor", typeof(double), typeof(ItemsControlBehaviorAdjustItemAppearance));

	public double EndingOpacityFactor
	{
		get
		{
			return (double)GetValue(EndingOpacityFactorProperty);
		}
		set
		{
			SetValue(EndingOpacityFactorProperty, value);
		}
	}

	public double EndingScaleFactor
	{
		get
		{
			return (double)GetValue(EndingScaleFactorProperty);
		}
		set
		{
			SetValue(EndingScaleFactorProperty, value);
		}
	}

	public int NumberOfItemsToAffect
	{
		get
		{
			return (int)GetValue(NumberOfItemsToAffectProperty);
		}
		set
		{
			SetValue(NumberOfItemsToAffectProperty, value);
		}
	}

	public bool ReverseOrder
	{
		get
		{
			return (bool)GetValue(ReverseOrderProperty);
		}
		set
		{
			SetValue(ReverseOrderProperty, value);
		}
	}

	public bool? ShowUnaffectedItems
	{
		get
		{
			return (bool?)GetValue(ShowUnaffectedItemsProperty);
		}
		set
		{
			SetValue(ShowUnaffectedItemsProperty, value);
		}
	}

	public double StartingOpacityFactor
	{
		get
		{
			return (double)GetValue(StartingOpacityFactorProperty);
		}
		set
		{
			SetValue(StartingOpacityFactorProperty, value);
		}
	}

	public double StartingScaleFactor
	{
		get
		{
			return (double)GetValue(StartingScaleFactorProperty);
		}
		set
		{
			SetValue(StartingScaleFactorProperty, value);
		}
	}

	protected override void OnAttached()
	{
		base.OnAttached();
		base.AssociatedObject.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();
		base.AssociatedObject.ItemContainerGenerator.StatusChanged -= ItemContainerGenerator_StatusChanged;
	}

	private void AffectItem(int i, ref double currentScale, ref double currentOpacity, bool ascendingOpacity, bool ascendingScale)
	{
		if (base.AssociatedObject.ItemContainerGenerator.ContainerFromIndex(i) is FrameworkElement frameworkElement)
		{
			frameworkElement.Visibility = Visibility.Visible;
			ApplyTransform(frameworkElement, currentScale, currentOpacity);
			if (ascendingOpacity)
			{
				currentOpacity += GetOpacityChangeFactor();
			}
			else
			{
				currentOpacity -= GetOpacityChangeFactor();
			}
			currentOpacity = ((currentOpacity < 0.0) ? 0.0 : currentOpacity);
			if (ascendingScale)
			{
				currentScale += GetScaleChangeFactor();
			}
			else
			{
				currentScale -= GetScaleChangeFactor();
			}
			currentScale = ((currentScale < 0.0) ? 0.0 : currentScale);
		}
	}

	private void ApplySettingsToItems()
	{
		double currentScale = StartingScaleFactor;
		double currentOpacity = StartingOpacityFactor;
		bool ascendingOpacity = false;
		bool ascendingScale = false;
		int num = NumberOfItemsToAffect;
		if (num > base.AssociatedObject.Items.Count)
		{
			num = base.AssociatedObject.Items.Count;
		}
		if (StartingOpacityFactor < EndingOpacityFactor)
		{
			ascendingOpacity = true;
		}
		if (StartingScaleFactor < EndingScaleFactor)
		{
			ascendingScale = true;
		}
		int num2;
		int num3;
		if (ReverseOrder)
		{
			num2 = num;
			num3 = 0;
		}
		else
		{
			num2 = 0;
			num3 = num;
		}
		if (ReverseOrder)
		{
			for (int num4 = num2; num4 < base.AssociatedObject.Items.Count; num4--)
			{
				if (num4 >= num3 && ShowUnaffectedItems != true)
				{
					HideItem(num4);
				}
				else
				{
					AffectItem(num4, ref currentScale, ref currentOpacity, ascendingOpacity, ascendingScale);
				}
			}
			return;
		}
		for (int i = num2; i < base.AssociatedObject.Items.Count; i++)
		{
			if (i >= num3 && ShowUnaffectedItems != true)
			{
				HideItem(i);
			}
			else
			{
				AffectItem(i, ref currentScale, ref currentOpacity, ascendingOpacity, ascendingScale);
			}
		}
	}

	private void ApplyTransform(FrameworkElement item, double scale, double opacity)
	{
		ScaleTransform layoutTransform = new ScaleTransform(scale, scale);
		item.LayoutTransform = layoutTransform;
		item.RenderTransformOrigin = new Point(0.5, 0.5);
		item.Opacity = opacity;
	}

	private double GetOpacityChangeFactor()
	{
		return (StartingOpacityFactor - EndingOpacityFactor) / (double)(NumberOfItemsToAffect - 1);
	}

	private double GetScaleChangeFactor()
	{
		return (StartingScaleFactor - EndingScaleFactor) / (double)(NumberOfItemsToAffect - 1);
	}

	private void HideItem(int i)
	{
		if (base.AssociatedObject.ItemContainerGenerator.ContainerFromIndex(i) is FrameworkElement frameworkElement)
		{
			frameworkElement.Visibility = Visibility.Collapsed;
		}
	}

	private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
	{
		if (base.AssociatedObject.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
		{
			ApplySettingsToItems();
		}
	}
}
