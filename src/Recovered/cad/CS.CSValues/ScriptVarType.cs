using System.ComponentModel;

namespace CS.CSValues;

public enum ScriptVarType
{
	[Description("Double")]
	vDouble,
	[Description("Integer")]
	vInt,
	[Description("Bool")]
	vBool,
	[Description("String")]
	vString,
	[Description("Object")]
	vObject,
	[Description("List<Double>")]
	vListDouble,
	[Description("List<Int>")]
	vListInt,
	[Description("List<Bool>")]
	vListBool,
	[Description("List<String>")]
	vListString,
	[Description("List<Object>")]
	vListObject
}
