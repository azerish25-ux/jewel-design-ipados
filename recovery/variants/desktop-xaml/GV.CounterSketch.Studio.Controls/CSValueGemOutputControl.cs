using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Markup;
using System.Windows.Media;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Shared.Pricing;

namespace GV.CounterSketch.Studio.Controls;

public sealed partial class CSValueGemOutputControl : CSUIControl, IComponentConnector
{
	public CSValueGemOutputControl(UIControl uiControl)
	{
		InitializeComponent();
		base.LocalUIControl = uiControl;
		SetControl();
	}

	public override void SetControl()
	{
		base.IsInitializing = true;
		base.Title = base.LocalUIControl.ControlName;
		base.SortIndex = base.LocalUIControl.SortIndex;
		SetGemList(base.LocalUIControl.GemList);
		if (!string.IsNullOrEmpty(base.PathToIconImage))
		{
			DrawingBrush drawingBrush = (DrawingBrush)RectImage.TryFindResource(base.PathToIconImage);
			if (drawingBrush != null)
			{
				RectImage.Fill = drawingBrush;
			}
		}
		base.SetControl();
		base.IsInitializing = false;
	}

	public void SetGemList(List<StudioGem> gems)
	{
		if (gems.Count > 0)
		{
			IEnumerable<StudioGem> itemsSource = gems.GroupBy((StudioGem gemKey) => new
			{
				GemShapeOption = gemKey.GemShapeOption,
				Height = Math.Round(gemKey.Length, 2),
				Width = Math.Round(gemKey.Width, 2),
				Length = Math.Round(gemKey.Height.GetValueOrDefault(), 2)
			}, (key, IEnumerable<StudioGem> grouping) =>
			{
				StudioGem[] source = (grouping as StudioGem[]) ?? grouping.ToArray();
				return new StudioGem
				{
					GemShapeOption = key.GemShapeOption,
					Height = key.Height,
					Width = key.Width,
					Length = key.Length,
					Quantity = source.Count()
				};
			});
			LstGemItems.ItemsSource = itemsSource;
		}
		else
		{
			LstGemItems.ItemsSource = null;
		}
	}

	public override void UpdateUIControlSource(UIControl uiControl)
	{
		base.UpdateUIControlSource(uiControl);
		SetGemList(uiControl.GemList);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
