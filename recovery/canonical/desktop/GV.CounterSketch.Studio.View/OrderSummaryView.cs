using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Controls;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.ViewModel;
using GalaSoft.MvvmLight.Messaging;

namespace GV.CounterSketch.Studio.View;

public class OrderSummaryView : UserControl, IComponentConnector
{
	internal Grid grid;

	internal DockPanel dockTitle;

	internal Grid OrderOptionsGrid;

	internal DockPanel CustomerInfo;

	internal TextBox tbCustomerTitleName;

	internal TextBox tbCustomerFirstName;

	internal TextBox tbCustomerMiddleInitial;

	internal TextBox tbCustomerLastName;

	internal TextBox tbCustomerNameSuffix;

	internal TextBox tbCustomerAddress1;

	internal TextBox tbCustomerAddress2;

	internal TextBox tbCustomerCity;

	internal TextBox tbCustomerStateRegion;

	internal TextBox tbCustomerPostalCode;

	internal TextBox tbCustomerCountry;

	internal TextBox tbCustomerPhone1;

	internal TextBox tbCustomerPhone2;

	internal TextBox tbCustomerEmail;

	internal DockPanel StoneReport;

	internal DesignReport controlDesignReport;

	internal DockPanel BandDuplicate;

	internal DockPanel Hallmark;

	internal DockPanel Engraving;

	internal TextBox txtEngravingText;

	internal ComboBox cboEngravingFont;

	internal ComboBox cboEngravingFontSize;

	internal ComboBox cboEngravingFillColor;

	internal DockPanel EngagementRing;

	internal CheckBox cbSendingRing;

	internal DockPanel MoreThanARing;

	internal DockPanel WaxMemo;

	internal DockPanel SupportingDocs;

	internal StackPanel stackPanel;

	internal Button btnSaveQuote;

	internal Button btnPrintQuote;

	internal Viewbox CSButtonPrint_Default;

	internal Button btnExportSTL3DM;

	internal Button btnGetCompleteQuote;

	internal StackPanel txtOrderedAlreadyText;

	internal StackPanel txtOrderedErrorsOnOrder;

	internal VisualStateGroup OrderStateGroup;

	internal VisualState Default;

	internal VisualState Ordered;

	internal VisualState Errored;

	private bool _contentLoaded;

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
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/ordersummaryview.xaml", UriKind.Relative);
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
			grid = (Grid)target;
			break;
		case 2:
			dockTitle = (DockPanel)target;
			break;
		case 3:
			OrderOptionsGrid = (Grid)target;
			break;
		case 4:
			CustomerInfo = (DockPanel)target;
			break;
		case 5:
			tbCustomerTitleName = (TextBox)target;
			break;
		case 6:
			tbCustomerFirstName = (TextBox)target;
			break;
		case 7:
			tbCustomerMiddleInitial = (TextBox)target;
			break;
		case 8:
			tbCustomerLastName = (TextBox)target;
			break;
		case 9:
			tbCustomerNameSuffix = (TextBox)target;
			break;
		case 10:
			tbCustomerAddress1 = (TextBox)target;
			break;
		case 11:
			tbCustomerAddress2 = (TextBox)target;
			break;
		case 12:
			tbCustomerCity = (TextBox)target;
			break;
		case 13:
			tbCustomerStateRegion = (TextBox)target;
			break;
		case 14:
			tbCustomerPostalCode = (TextBox)target;
			break;
		case 15:
			tbCustomerCountry = (TextBox)target;
			break;
		case 16:
			tbCustomerPhone1 = (TextBox)target;
			break;
		case 17:
			tbCustomerPhone2 = (TextBox)target;
			break;
		case 18:
			tbCustomerEmail = (TextBox)target;
			break;
		case 19:
			StoneReport = (DockPanel)target;
			break;
		case 20:
			controlDesignReport = (DesignReport)target;
			break;
		case 21:
			BandDuplicate = (DockPanel)target;
			break;
		case 22:
			Hallmark = (DockPanel)target;
			break;
		case 23:
			Engraving = (DockPanel)target;
			break;
		case 24:
			txtEngravingText = (TextBox)target;
			break;
		case 25:
			cboEngravingFont = (ComboBox)target;
			break;
		case 26:
			cboEngravingFontSize = (ComboBox)target;
			break;
		case 27:
			cboEngravingFillColor = (ComboBox)target;
			break;
		case 28:
			EngagementRing = (DockPanel)target;
			break;
		case 29:
			cbSendingRing = (CheckBox)target;
			break;
		case 30:
			MoreThanARing = (DockPanel)target;
			break;
		case 31:
			WaxMemo = (DockPanel)target;
			break;
		case 32:
			SupportingDocs = (DockPanel)target;
			break;
		case 33:
			stackPanel = (StackPanel)target;
			break;
		case 34:
			btnSaveQuote = (Button)target;
			break;
		case 35:
			btnPrintQuote = (Button)target;
			break;
		case 36:
			CSButtonPrint_Default = (Viewbox)target;
			break;
		case 37:
			btnExportSTL3DM = (Button)target;
			break;
		case 38:
			btnGetCompleteQuote = (Button)target;
			break;
		case 39:
			txtOrderedAlreadyText = (StackPanel)target;
			break;
		case 40:
			txtOrderedErrorsOnOrder = (StackPanel)target;
			break;
		case 41:
			OrderStateGroup = (VisualStateGroup)target;
			break;
		case 42:
			Default = (VisualState)target;
			break;
		case 43:
			Ordered = (VisualState)target;
			break;
		case 44:
			Errored = (VisualState)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
