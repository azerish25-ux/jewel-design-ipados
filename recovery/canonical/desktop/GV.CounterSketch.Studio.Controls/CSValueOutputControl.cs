using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using GV.CounterSketch.Shared.Model;

namespace GV.CounterSketch.Studio.Controls;

public sealed class CSValueOutputControl : CSUIControl, IComponentConnector
{
	internal CSValueOutputControl TheValueOutputControl;

	internal Grid GridDisplayImage;

	internal Rectangle RectImage;

	internal Border BorderSubMenu;

	internal TextBlock TxtTitle;

	private bool _contentLoaded;

	public CSValueOutputControl(UIControl uiControl)
	{
		InitializeComponent();
		base.LocalUIControl = uiControl;
		SetControl();
	}

	public override void SetControl()
	{
		base.IsInitializing = true;
		base.Title = base.LocalUIControl.ControlName;
		base.DisplayText = base.LocalUIControl.DisplayText;
		base.SortIndex = base.LocalUIControl.SortIndex;
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

	public override void UpdateUIControlSource(UIControl uiControl)
	{
		base.UpdateUIControlSource(uiControl);
		base.DisplayText = uiControl.DisplayText;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/csvalueoutputcontrol.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		switch (connectionId)
		{
		case 1:
			TheValueOutputControl = (CSValueOutputControl)target;
			break;
		case 2:
			GridDisplayImage = (Grid)target;
			break;
		case 3:
			RectImage = (Rectangle)target;
			break;
		case 4:
			BorderSubMenu = (Border)target;
			break;
		case 5:
			TxtTitle = (TextBlock)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
