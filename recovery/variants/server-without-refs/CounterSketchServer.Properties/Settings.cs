using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CounterSketchServer.Properties;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
internal sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

	public static Settings Default => defaultInstance;

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[SpecialSetting(SpecialSetting.WebServiceUrl)]
	[DefaultSettingValue("https://countersketch.biz/CounterSketchService.asmx")]
	public string CounterSketchServer_CounterSketchServices_CounterSketchService => (string)this["CounterSketchServer_CounterSketchServices_CounterSketchService"];
}
