using System;
using System.Reflection;
using CS.CSObjects;

namespace CS.Parametric;

public sealed class CSBlockDescription
{
	protected string BlockClassName;

	public Guid Guid { get; }

	public Type BlockClass { get; protected set; }

	public string BlockName { get; protected set; }

	public CSBlockDescription(string blockName, Type blockClass, Guid guid)
	{
		BlockName = blockName;
		BlockClass = blockClass;
		BlockClassName = blockClass.FullName;
		Guid = guid;
	}

	public CSObject GetNew(string identifier)
	{
		Type[] types = new Type[1] { typeof(string) };
		ConstructorInfo constructor = BlockClass.GetConstructor(types);
		if (constructor == null)
		{
			throw new Exception($"Failed to create new object!  Missing constructor {BlockName}(string identifier)\n");
		}
		return (CSObject)constructor.Invoke(new object[1] { identifier });
	}

	public CSObject GetNewTemp(string identifier)
	{
		int count = CSApplicationHelper.Application.Document.Graph.GraphList.Count;
		Type[] types = new Type[1] { typeof(string) };
		ConstructorInfo constructor = BlockClass.GetConstructor(types);
		if (constructor == null)
		{
			throw new Exception($"Failed to create new temporary object!  Missing constructor {BlockName}(string identifier)\n");
		}
		CSObject obj = (CSObject)constructor.Invoke(new object[1] { identifier });
		CSApplicationHelper.Application.Document.Graph.GraphList.RemoveAt(count);
		obj.ObjectLastChanged = 0;
		obj.UpdateMarker = 0;
		return obj;
	}

	public bool IsBlockClass(Guid blockValue)
	{
		return blockValue.CompareTo(Guid) == 0;
	}

	public bool IsBlockClass(string fullName)
	{
		return string.Compare(BlockClass.FullName, fullName, StringComparison.OrdinalIgnoreCase) == 0;
	}

	public bool IsBlockInstance(CSObject obj)
	{
		return BlockClass == obj.GetType();
	}
}
