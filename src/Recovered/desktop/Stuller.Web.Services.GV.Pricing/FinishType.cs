using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace Stuller.Web.Services.GV.Pricing;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "FinishType", Namespace = "http://schemas.datacontract.org/2004/07/Stuller.Com.Core.Enums.ServiceBureau")]
public enum FinishType
{
	[EnumMember]
	Clip,
	[EnumMember]
	Semi,
	[EnumMember]
	Full,
	[EnumMember]
	Special,
	[EnumMember]
	Na,
	[EnumMember]
	Tumble
}
