using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;

namespace GV.CounterSketch.Shared.Model;

[DataContract]
public sealed class DataItem : INotifyPropertyChanged
{
	private BitmapSourceByteArrayInfo _bitmapSourceByteArrayInfo;

	private string _identifier;

	private bool _isItemDisabled;

	private IEnumerable<Point> _polyPointCollection;

	private string _vectorResourceName;

	private bool _hasImage;

	public string DisplayStyle => UpdateDisplayStyle();

	public Visibility ItemVisibility
	{
		get
		{
			if (!IsItemDisabled)
			{
				return Visibility.Visible;
			}
			return Visibility.Collapsed;
		}
	}

	[DataMember]
	public BitmapSourceByteArrayInfo BitmapSourceByteArrayInfo
	{
		get
		{
			return _bitmapSourceByteArrayInfo;
		}
		set
		{
			_bitmapSourceByteArrayInfo = value;
			RaisePropertyChanged("BitmapSourceByteArrayInfo");
		}
	}

	[DataMember]
	public bool HasImage
	{
		get
		{
			return _hasImage;
		}
		set
		{
			_hasImage = value;
			RaisePropertyChanged("HasImage");
		}
	}

	[DataMember]
	public string Identifier
	{
		get
		{
			return _identifier;
		}
		set
		{
			_identifier = value;
			RaisePropertyChanged("Identifier");
		}
	}

	[DataMember]
	public bool IsItemDisabled
	{
		get
		{
			return _isItemDisabled;
		}
		set
		{
			_isItemDisabled = value;
			RaisePropertyChanged("IsItemDisabled");
		}
	}

	[DataMember]
	public IEnumerable<Point> PolyPointCollection
	{
		get
		{
			return _polyPointCollection;
		}
		set
		{
			_polyPointCollection = value;
			RaisePropertyChanged("PolyPointCollection");
		}
	}

	[DataMember]
	public string VectorResourceName
	{
		get
		{
			return _vectorResourceName;
		}
		set
		{
			_vectorResourceName = value;
			RaisePropertyChanged("VectorResourceName");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public string UpdateDisplayStyle()
	{
		string text = string.Empty;
		if (HasImage)
		{
			text = "BitmapImage";
		}
		else if (!string.IsNullOrWhiteSpace(VectorResourceName))
		{
			text = "VectorImage";
		}
		else if (PolyPointCollection != null && PolyPointCollection.First() == PolyPointCollection.Last())
		{
			text = "Polygon";
		}
		else if (PolyPointCollection != null)
		{
			Point point = PolyPointCollection.First();
			point.X = Math.Round(point.X, 3);
			point.Y = Math.Round(point.Y, 3);
			Point point2 = PolyPointCollection.Last();
			point2.X = Math.Round(point2.X, 3);
			point2.Y = Math.Round(point2.Y, 3);
			text = ((!(point == point2)) ? "Polyline" : "Polygon");
		}
		if (string.IsNullOrWhiteSpace(text))
		{
			return "Default";
		}
		return text;
	}

	private void RaisePropertyChanged(string info)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(info));
		}
	}
}
