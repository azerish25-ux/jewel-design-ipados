using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using Rhino;

namespace CS;

public static class CSApplicationHelper
{
	public enum CounterSketchApplication
	{
		Unknown,
		Designer,
		Studio,
		Verifier
	}

	private static ICSApplication _csApplication;

	public static ICSApplication Application
	{
		get
		{
			if (_csApplication == null)
			{
				if ((bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue)
				{
					_csApplication = new CSApplicationMock();
				}
				else
				{
					_csApplication = new CSApplication();
				}
				CSApplication.RhinoProcessId = Process.GetCurrentProcess().Id;
			}
			return _csApplication;
		}
	}

	public static int MaxNumberOfBlockUpdateAvailableCores
	{
		get
		{
			if (IsParallelProcessingDisabled)
			{
				return 1;
			}
			if (Environment.ProcessorCount <= 1)
			{
				return 1;
			}
			if (Environment.ProcessorCount / 2 - 1 <= 1)
			{
				return 1;
			}
			return Environment.ProcessorCount / 2 - 1;
		}
	}

	public static int MaxNumberOfAvailableCores
	{
		get
		{
			if (Environment.ProcessorCount <= 1)
			{
				return 1;
			}
			return Environment.ProcessorCount - 1;
		}
	}

	public static bool IsCounterSketchDesigner => WhichCounterSketchApplication == CounterSketchApplication.Designer;

	public static bool IsCounterSketchStudio => WhichCounterSketchApplication == CounterSketchApplication.Studio;

	public static bool IsCounterSketchVerifier => WhichCounterSketchApplication == CounterSketchApplication.Verifier;

	public static bool IsParallelProcessingDisabled { get; set; }

	public static CounterSketchApplication WhichCounterSketchApplication { get; set; }

	public static bool CheckSavedVersionAndPrompt(bool isOverrideAllowed = true)
	{
		Version documentVersion = GetDocumentVersion();
		if (documentVersion != null)
		{
			Version applicationVersion = GetApplicationVersion();
			if (applicationVersion < documentVersion && TopMostMessageBox.Show(string.Format("The file you are attempting to load was last edited in version {0}, you must upgrade your application from version {1} to that version to open this design." + (isOverrideAllowed ? "\n\nWould you like to attempt to open the file anyways?" : string.Empty), documentVersion, applicationVersion), "Design File Too New", isOverrideAllowed ? MessageBoxButtons.YesNo : MessageBoxButtons.OK, MessageBoxIcon.Exclamation) != DialogResult.Yes)
			{
				return false;
			}
		}
		return true;
	}

	public static Version GetApplicationVersion()
	{
		return Assembly.GetAssembly(typeof(CSApplicationHelper)).GetName().Version;
	}

	public static string GetApplicationVersionString()
	{
		Version applicationVersion = GetApplicationVersion();
		return string.Format("{0}.{1}.{2}.{3}", applicationVersion.Major, applicationVersion.Minor, applicationVersion.Build, applicationVersion.Revision.ToString("0###"));
	}

	public static Version GetDocumentVersion()
	{
		Version result = null;
		string text = Application.Document.DocumentUserData.FindValue("Version", "Core");
		string text2 = Application.Document.DocumentUserData.FindValue("Version", "Designer");
		List<string> list = new List<string>();
		if (!string.IsNullOrWhiteSpace(text))
		{
			list.Add(text);
		}
		if (!string.IsNullOrWhiteSpace(text2))
		{
			list.Add(text2);
		}
		if (list.Any())
		{
			result = (from versionString in list
				select new Version(versionString) into coreVersion
				orderby coreVersion descending
				select coreVersion).FirstOrDefault();
		}
		return result;
	}

	public static bool GetIsCounterSketchStudio()
	{
		return IsCounterSketchStudio;
	}

	public static bool GetIsCounterSketchVerifier()
	{
		return IsCounterSketchVerifier;
	}

	public static void WriteVersionInformation()
	{
		Version version = new Version(RhinoApp.ExeVersion, RhinoApp.ExeServiceRelease);
		Application.Document.DocumentUserData.AddKey("Version", "Rhino", version.ToString());
		Version documentVersion = GetDocumentVersion();
		Version applicationVersion = GetApplicationVersion();
		if (applicationVersion.Major == 9999)
		{
			return;
		}
		if (documentVersion != null)
		{
			if (GetApplicationVersion() > documentVersion)
			{
				Application.Document.DocumentUserData.AddKey("Version", "Core", applicationVersion.ToString());
			}
		}
		else
		{
			Application.Document.DocumentUserData.AddKey("Version", "Core", applicationVersion.ToString());
		}
	}
}
