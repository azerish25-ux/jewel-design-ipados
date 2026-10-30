using System;

namespace GV.CounterSketch.Common.Annotations;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
public sealed class CannotApplyEqualityOperatorAttribute : Attribute
{
}
