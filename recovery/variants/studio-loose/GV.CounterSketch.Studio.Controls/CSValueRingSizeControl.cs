using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using Localization;

namespace GV.CounterSketch.Studio.Controls;

public sealed class CSValueRingSizeControl : CSUIControl, IComponentConnector
{
	public static readonly DependencyProperty RingSizeDisplayLookupProperty = DependencyProperty.Register("RingSizeDisplayLookup", typeof(ObservableCollection<KeyValuePair<double, string>>), typeof(CSValueRingSizeControl));

	public static readonly DependencyProperty SelectedRingSizeIndexProperty = DependencyProperty.Register("SelectedRingSizeIndex", typeof(double), typeof(CSValueRingSizeControl));

	internal CSValueRingSizeControl CSValueRingSizeUserControl;

	internal TextBlock txtTitle;

	private bool _contentLoaded;

	public ObservableCollection<KeyValuePair<double, string>> RingSizeDisplayLookup
	{
		get
		{
			return (ObservableCollection<KeyValuePair<double, string>>)GetValue(RingSizeDisplayLookupProperty);
		}
		set
		{
			SetValue(RingSizeDisplayLookupProperty, value);
		}
	}

	public double SelectedRingSizeIndex
	{
		get
		{
			return (double)GetValue(SelectedRingSizeIndexProperty);
		}
		set
		{
			SetValue(SelectedRingSizeIndexProperty, value);
		}
	}

	public CSValueRingSizeControl(UIControl uiControl)
	{
		InitializeComponent();
		base.LocalUIControl = uiControl;
		if (UIHelper.DefaultRingSizeIndex >= 0)
		{
			base.DefaultControlValue = UIHelper.DefaultRingSizeIndex;
		}
		else
		{
			base.DefaultControlValue = base.LocalUIControl.Value1;
		}
		SetControl();
		Messenger.Default.Register<PropertyChangedMessage<int>>((object)this, (Action<PropertyChangedMessage<int>>)ProcessPropertyChangedMessage, false);
	}

	public override void SetControl()
	{
		base.IsInitializing = true;
		base.Title = base.LocalUIControl.ControlName;
		base.Value = base.LocalUIControl.Value1;
		base.MinValue = base.LocalUIControl.Minimum1;
		base.MaxValue = base.LocalUIControl.Maximum1;
		base.StepValue = base.LocalUIControl.Step1;
		base.OriginalStep = base.StepValue;
		base.SortIndex = base.LocalUIControl.SortIndex;
		UpdateRingSizeDisplayLookup(base.LocalUIControl.Minimum1, base.LocalUIControl.Maximum1, base.LocalUIControl.Step1, base.LocalUIControl.Value1);
		SelectedRingSizeIndex = base.LocalUIControl.Value1;
		if (Math.Abs(base.DefaultControlValue - base.LocalUIControl.Value1) > 0.0)
		{
			base.LocalUIControl.Value1 = base.DefaultControlValue;
			SetGraphValue(isQuickSet: false);
		}
		GetIconForUIControl();
		base.SetControl();
		base.IsInitializing = false;
	}

	public void UnregisterFromMessenger()
	{
		Messenger.Default.Unregister<PropertyChangedMessage<int>>((object)this);
	}

	public override void UpdateUIControlSource(UIControl uiControl)
	{
		base.IsUpdatingControlSource = true;
		base.UpdateUIControlSource(uiControl);
		base.Title = base.LocalUIControl.ControlName;
		base.Value = base.LocalUIControl.Value1;
		base.MinValue = base.LocalUIControl.Minimum1;
		base.MaxValue = base.LocalUIControl.Maximum1;
		base.StepValue = base.LocalUIControl.Step1;
		base.OriginalStep = base.StepValue;
		base.SortIndex = base.LocalUIControl.SortIndex;
		SelectedRingSizeIndex = base.LocalUIControl.Value1;
		if (Math.Abs(SelectedRingSizeIndex - base.LocalUIControl.Value1) > 0.001)
		{
			ViewModelLocator.Current.OrderSummaryVM.PopulateRingSize();
		}
		base.IsUpdatingControlSource = false;
	}

	private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (base.LocalUIControl != null && !base.IsInitializing && !base.IsUpdatingControlSource && e.AddedItems != null && e.AddedItems.Count > 0)
		{
			double key = ((KeyValuePair<double, string>)e.AddedItems[0]).Key;
			base.LocalUIControl.Value1 = key;
			SetGraphValue(isQuickSet: false);
		}
	}

	private void ProcessPropertyChangedMessage(PropertyChangedMessage<int> propertyChangedMessage)
	{
		string propertyName = ((PropertyChangedMessageBase)propertyChangedMessage).PropertyName;
		if (propertyName != null && propertyName == "DefaultRingSizeIndex" && !UIHelper.IsResettingApplicationState)
		{
			base.LocalUIControl.Value1 = propertyChangedMessage.NewValue;
			SetGraphValue(isQuickSet: false);
		}
	}

	private void UpdateRingSizeDisplayLookup(double minValue, double maxValue, double stepValue, double value)
	{
		if (RingSizeDisplayLookup != null)
		{
			return;
		}
		RingSizeDisplayLookup = new ObservableCollection<KeyValuePair<double, string>>();
		for (double num = minValue; num <= maxValue; num += stepValue)
		{
			string text = UIHelper.CounterSketchOptions.RingSizeSetting;
			if (text == "UnitedStates")
			{
				text = "US";
			}
			RingSizeDisplayLookup.Add(new KeyValuePair<double, string>(num, text + " " + Strings.Size + " " + UIHelper.CoreDataService.DocumentGetRingSizeTextFromSizeIndex(num)));
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/csvalueringsizecontrol.xaml", UriKind.Relative);
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
			CSValueRingSizeUserControl = (CSValueRingSizeControl)target;
			break;
		case 2:
			txtTitle = (TextBlock)target;
			break;
		case 3:
			((ComboBox)target).SelectionChanged += ComboBoxSelectionChanged;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
