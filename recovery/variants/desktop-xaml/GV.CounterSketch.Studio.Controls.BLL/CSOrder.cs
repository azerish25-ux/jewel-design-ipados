using System;
using System.IO;
using System.Windows.Media;
using System.Xml.Serialization;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using Localization;

namespace GV.CounterSketch.Studio.Controls.BLL;

public class CSOrder : ViewModelBase
{
	private int _camRequestId;

	private string _contactEMail;

	private string _contactPhone;

	private string _customerAddress1;

	private string _customerAddress2;

	private string _customerCity;

	private string _customerFirstName;

	private string _customerLastName;

	private string _customerState;

	private string _customerZip;

	private DateTime _dateSubmitted;

	private ImageSource _designImageSource;

	private string _eMail;

	private string _firstName;

	private bool _isViewingPricingImage;

	private string _lastName;

	private string _notes;

	private string _orderNumber;

	private string _quotedDesignPath;

	private int _quoteId;

	private string _quoteShipDate;

	private string _quoteStatus;

	private string _referenceNumber;

	[XmlIgnore]
	public bool IsZalesUser => UIHelper.KeyProfile.KeyProfileId == 2;

	[XmlIgnore]
	public string PricingImagePath => Path.Combine(UIHelper.CSQuotesPath, "OrderPricing_" + Path.GetFileNameWithoutExtension(QuotedDesignPath) + ".png");

	[XmlIgnore]
	public bool ShowViewPricingImageButton => File.Exists(PricingImagePath);

	public int QuoteId
	{
		get
		{
			return _quoteId;
		}
		set
		{
			_quoteId = value;
			RaisePropertyChanged("QuoteId");
		}
	}

	public string FirstName
	{
		get
		{
			return _firstName;
		}
		set
		{
			_firstName = value;
			RaisePropertyChanged("FirstName");
		}
	}

	public string LastName
	{
		get
		{
			return _lastName;
		}
		set
		{
			_lastName = value;
			RaisePropertyChanged("LastName");
		}
	}

	public string EMail
	{
		get
		{
			return _eMail;
		}
		set
		{
			_eMail = value;
			RaisePropertyChanged("EMail");
		}
	}

	public string CustomerFirstName
	{
		get
		{
			return _customerFirstName;
		}
		set
		{
			_customerFirstName = value;
			RaisePropertyChanged("CustomerFirstName");
		}
	}

	public string CustomerLastName
	{
		get
		{
			return _customerLastName;
		}
		set
		{
			_customerLastName = value;
			RaisePropertyChanged("CustomerLastName");
		}
	}

	public string CustomerAddress1
	{
		get
		{
			return _customerAddress1;
		}
		set
		{
			_customerAddress1 = value;
			RaisePropertyChanged("CustomerAddress1");
		}
	}

	public string CustomerAddress2
	{
		get
		{
			return _customerAddress2;
		}
		set
		{
			_customerAddress2 = value;
			RaisePropertyChanged("CustomerAddress2");
		}
	}

	public string CustomerCity
	{
		get
		{
			return _customerCity;
		}
		set
		{
			_customerCity = value;
			RaisePropertyChanged("CustomerCity");
		}
	}

	public string CustomerState
	{
		get
		{
			return _customerState;
		}
		set
		{
			_customerState = value;
			RaisePropertyChanged("CustomerState");
		}
	}

	public string CustomerZip
	{
		get
		{
			return _customerZip;
		}
		set
		{
			_customerZip = value;
			RaisePropertyChanged("CustomerZip");
		}
	}

	public string ContactEMail
	{
		get
		{
			return _contactEMail;
		}
		set
		{
			_contactEMail = value;
			RaisePropertyChanged("ContactEMail");
		}
	}

	public string ContactPhone
	{
		get
		{
			return _contactPhone;
		}
		set
		{
			_contactPhone = value;
			RaisePropertyChanged("ContactPhone");
		}
	}

	public string Notes
	{
		get
		{
			return _notes;
		}
		set
		{
			_notes = value;
			RaisePropertyChanged("Notes");
		}
	}

	public DateTime DateSubmitted
	{
		get
		{
			return _dateSubmitted;
		}
		set
		{
			_dateSubmitted = value;
			RaisePropertyChanged("DateSubmitted");
		}
	}

	public string QuotedDesignPath
	{
		get
		{
			return _quotedDesignPath;
		}
		set
		{
			_quotedDesignPath = value;
			RaisePropertyChanged("QuotedDesignPath");
			RaisePropertyChanged("PricingImagePath");
		}
	}

	public ImageSource DesignImageSource
	{
		get
		{
			return _designImageSource;
		}
		set
		{
			_designImageSource = value;
			RaisePropertyChanged("DesignImageSource");
		}
	}

	public int CamRequestId
	{
		get
		{
			return _camRequestId;
		}
		set
		{
			_camRequestId = value;
			RaisePropertyChanged("CamRequestId");
		}
	}

	public string ReferenceNumber
	{
		get
		{
			return _referenceNumber;
		}
		set
		{
			_referenceNumber = value;
			RaisePropertyChanged("ReferenceNumber");
		}
	}

	public string OrderNumber
	{
		get
		{
			return _orderNumber;
		}
		set
		{
			_orderNumber = value;
			RaisePropertyChanged("OrderNumber");
		}
	}

	public string QuoteStatus
	{
		get
		{
			return _quoteStatus;
		}
		set
		{
			_quoteStatus = TranslationHelper.GetTranslatedStullerString(value);
			RaisePropertyChanged("QuoteStatus");
		}
	}

	public string QuoteShipDate
	{
		get
		{
			return _quoteShipDate;
		}
		set
		{
			_quoteShipDate = value;
			RaisePropertyChanged("QuoteShipDate");
		}
	}

	public bool IsViewingPricingImage
	{
		get
		{
			return _isViewingPricingImage;
		}
		set
		{
			_isViewingPricingImage = value;
			RaisePropertyChanged("IsViewingPricingImage");
		}
	}
}
