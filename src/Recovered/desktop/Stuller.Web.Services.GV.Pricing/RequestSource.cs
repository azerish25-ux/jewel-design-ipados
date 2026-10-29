using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace Stuller.Web.Services.GV.Pricing;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "RequestSource", Namespace = "http://schemas.datacontract.org/2004/07/Stuller.Com.Core.Enums.ServiceBureau")]
public enum RequestSource
{
	[EnumMember]
	Standard,
	[EnumMember]
	CounterSketch,
	[EnumMember]
	SpecialProjects,
	[EnumMember]
	ProgramSales,
	[EnumMember]
	Repair,
	[EnumMember]
	Flex,
	[EnumMember]
	Test,
	[EnumMember]
	_3C,
	[EnumMember]
	CCC,
	[EnumMember]
	CounterSketchOrder,
	[EnumMember]
	CounterSketchNonOrder,
	[EnumMember]
	CastingService
}
