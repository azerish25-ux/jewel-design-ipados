using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace Stuller.Web.Services.GV.Pricing;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SettingType", Namespace = "http://schemas.datacontract.org/2004/07/Stuller.Com.Core.Enums.ServiceBureau")]
public enum SettingType
{
	[EnumMember]
	Stuller,
	[EnumMember]
	Customer,
	[EnumMember]
	None
}
