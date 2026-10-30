using System;
using System.IO;
using System.Xml.Serialization;

namespace CS.Parametric;

public sealed class PatchWireOptions
{
	private readonly PatchWireSettings m_UserPatchWireSettings;

	private readonly PatchWireSettings m_DefaultPatchWireSettings;

	private string m_strUserSetting = "";

	private string m_strDefaultSetting = "";

	public PatchWireOptions()
	{
		ValidateSettingsPath();
		try
		{
			if (File.Exists(m_strUserSetting))
			{
				using FileStream stream = File.OpenRead(m_strUserSetting);
				PatchWireSettings userPatchWireSettings = new XmlSerializer(typeof(PatchWireSettings)).Deserialize(stream) as PatchWireSettings;
				m_UserPatchWireSettings = userPatchWireSettings;
			}
		}
		catch (Exception)
		{
		}
		if (m_UserPatchWireSettings == null)
		{
			m_UserPatchWireSettings = new PatchWireSettings();
		}
		try
		{
			if (File.Exists(m_strDefaultSetting))
			{
				using FileStream stream2 = File.OpenRead(m_strDefaultSetting);
				PatchWireSettings defaultPatchWireSettings = new XmlSerializer(typeof(PatchWireSettings)).Deserialize(stream2) as PatchWireSettings;
				m_DefaultPatchWireSettings = defaultPatchWireSettings;
			}
		}
		catch (Exception)
		{
		}
		if (m_DefaultPatchWireSettings == null)
		{
			m_DefaultPatchWireSettings = new PatchWireSettings();
		}
	}

	public bool GetSetting(Guid _ParentID, string _strParent, Guid _ChildID, string _strChild, string _strValue, ref int _nValue, int _nDefault)
	{
		bool result = false;
		if (_ParentID != Guid.Empty && !(result = ReadSetting(_bDefault: false, _ParentID, _strParent, _strChild, _strValue, ref _nValue, _nDefault)) && !(result = ReadSetting(_bDefault: true, _ParentID, _strParent, _strChild, _strValue, ref _nValue, _nDefault)))
		{
			_nValue = _nDefault;
		}
		return result;
	}

	private bool ReadSetting(bool _bDefault, Guid _ParentID, string _strParent, string _strChild, string _strValue, ref int _nValue, int _nDefault)
	{
		bool flag = false;
		PatchWireSettings patchWireSettings = m_UserPatchWireSettings;
		if (_bDefault)
		{
			patchWireSettings = m_DefaultPatchWireSettings;
		}
		_nValue = 0;
		PatchWirePos patchWirePos = patchWireSettings.PatchWires.Find(delegate(PatchWirePos pos)
		{
			if (pos.Guid != Guid.Empty)
			{
				return pos.Guid == _ParentID;
			}
			return pos.Guid == _ParentID && pos.Name == _strParent;
		});
		if (patchWirePos != null)
		{
			PatchWirePosSetting patchWirePosSetting = null;
			if (_strChild != null)
			{
				patchWirePosSetting = patchWirePos.Children.Find((PatchWirePosSetting setting) => setting.Name == _strChild);
				if (patchWirePosSetting != null)
				{
					if (string.Compare(_strValue, "posx", ignoreCase: true) == 0)
					{
						_nValue = patchWirePosSetting.PosX;
						flag = _nValue > 0;
					}
					else if (string.Compare(_strValue, "posy", ignoreCase: true) == 0)
					{
						_nValue = patchWirePosSetting.PosY;
						flag = _nValue > 0;
					}
					else if (string.Compare(_strValue, "width", ignoreCase: true) == 0)
					{
						_nValue = patchWirePosSetting.Width;
						flag = _nValue > 0;
					}
					else if (string.Compare(_strValue, "height", ignoreCase: true) == 0)
					{
						_nValue = patchWirePosSetting.Height;
						flag = _nValue > 0;
					}
					else if (string.Compare(_strValue, "expandwidth", ignoreCase: true) == 0)
					{
						_nValue = patchWirePosSetting.ExpandWidth;
						flag = _nValue > 0;
					}
					else
					{
						if (string.Compare(_strValue, "expandheight", ignoreCase: true) != 0)
						{
							return flag;
						}
						_nValue = patchWirePosSetting.ExpandHeight;
						flag = _nValue > 0;
					}
				}
			}
			else if (string.Compare(_strValue, "width", ignoreCase: true) == 0)
			{
				_nValue = patchWirePos.Width;
				flag = _nValue > 0;
			}
			else if (string.Compare(_strValue, "height", ignoreCase: true) == 0)
			{
				_nValue = patchWirePos.Height;
				flag = _nValue > 0;
			}
			else if (string.Compare(_strValue, "expandwidth", ignoreCase: true) == 0)
			{
				_nValue = patchWirePos.ExpandWidth;
				flag = _nValue > 0;
			}
			else
			{
				if (string.Compare(_strValue, "expandheight", ignoreCase: true) != 0)
				{
					_nValue = _nDefault;
					return flag;
				}
				_nValue = patchWirePos.ExpandHeight;
				flag = _nValue > 0;
			}
		}
		if (!flag)
		{
			_nValue = _nDefault;
		}
		return flag;
	}

	private void ValidateSettingsPath()
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Gemvision\\Designer");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		m_strUserSetting = Path.Combine(text, "SuperviewUserSettings.xml");
		m_strDefaultSetting = Path.Combine(text, "SuperviewDefaultSettings.xml");
	}
}
