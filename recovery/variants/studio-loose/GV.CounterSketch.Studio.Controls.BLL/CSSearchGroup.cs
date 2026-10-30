using System.Collections.Generic;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GalaSoft.MvvmLight;

namespace GV.CounterSketch.Studio.Controls.BLL;

public class CSSearchGroup : ViewModelBase
{
	private bool _displayCriteria;

	public CSUIAttribute SelectedAttribute
	{
		get
		{
			if (AttributeList == null)
			{
				return null;
			}
			return AttributeList.Find((CSUIAttribute attribute) => attribute.IsSelected);
		}
	}

	public List<CSUIAttribute> AttributeList { get; set; }

	public string Title { get; set; }

	public CSSearchGroupControlType Type { get; set; }

	public CSSearchGroupType TypeID { get; set; }

	public bool DisplayCriteria
	{
		get
		{
			return _displayCriteria;
		}
		set
		{
			_displayCriteria = value;
			((ObservableObject)this).RaisePropertyChanged("DisplayCriteria");
		}
	}

	public CSSearchGroup()
	{
	}

	public CSSearchGroup(CSSearchGroupType typeid, string title, CSSearchGroupControlType type, List<CSUIAttribute> attributeList, bool displayCriteria)
	{
		TypeID = typeid;
		Title = title;
		Type = type;
		AttributeList = attributeList;
		DisplayCriteria = displayCriteria;
	}
}
