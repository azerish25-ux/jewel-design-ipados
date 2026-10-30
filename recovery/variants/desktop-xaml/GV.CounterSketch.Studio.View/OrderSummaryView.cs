using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.ViewModel;
using GalaSoft.MvvmLight.Messaging;

namespace GV.CounterSketch.Studio.View;

public partial class OrderSummaryView : UserControl, IComponentConnector
{
	public OrderSummaryView()
	{
		InitializeComponent();
		Messenger.Default.Register<string>(this, ProcessGenericMessage);
	}

	private void CaptureStoneReportImage()
	{
		byte[] stoneReportImageBytes = WpfHelper.SaveElementToPngByteArray(controlDesignReport);
		ViewModelLocator.Current.OrderSummaryVM.StoneReportImageBytes = stoneReportImageBytes;
	}

	private void ProcessGenericMessage(string message)
	{
		if (message != null)
		{
			string text = message.ToLower();
			if (text != null && text == "capturestonereportimage")
			{
				CaptureStoneReportImage();
			}
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
