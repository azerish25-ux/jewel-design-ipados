using System;
using System.Linq;
using System.Reflection;

namespace CS;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class CSPatchWireAttribute : Attribute
{
	private readonly Guid _identifier;

	public Guid Identifier => _identifier;

	public string Description { get; set; }

	public bool DoNoAdd { get; set; }

	public string Grouping { get; set; }

	public bool IsArchive { get; set; }

	public bool IsBeta { get; set; }

	public bool IsDisplayVisible { get; set; }

	public string Name { get; set; }

	public string PathToImage { get; set; }

	public string StyleSheetFolder { get; set; }

	public int VersionNumber { get; set; }

	public CSPatchWireAttribute(string identifier, int versionNumber, string grouping, string name, string description, string pathToImage)
	{
		Guid.TryParse(identifier, out _identifier);
		VersionNumber = versionNumber;
		Grouping = grouping;
		Name = name;
		Description = description;
		PathToImage = pathToImage;
		IsBeta = false;
		IsArchive = false;
		IsDisplayVisible = true;
	}

	public CSPatchWireAttribute(string identifier, int versionNumber, string grouping, string name, string description, string pathToImage, string styleSheetFolder)
	{
		Guid.TryParse(identifier, out _identifier);
		VersionNumber = versionNumber;
		Grouping = grouping;
		Name = name;
		Description = description;
		PathToImage = pathToImage;
		StyleSheetFolder = styleSheetFolder;
		IsBeta = false;
		IsArchive = false;
		IsDisplayVisible = true;
	}

	public CSPatchWireAttribute(string identifier, int versionNumber, string grouping, string name, string description, string pathToImage, string styleSheetFolder, bool isBeta)
	{
		Guid.TryParse(identifier, out _identifier);
		VersionNumber = versionNumber;
		Grouping = grouping;
		Name = name;
		Description = description;
		PathToImage = pathToImage;
		StyleSheetFolder = styleSheetFolder;
		IsBeta = isBeta;
		IsArchive = false;
		IsDisplayVisible = true;
	}

	public CSPatchWireAttribute(string identifier, int versionNumber, string grouping, string name, string description, string pathToImage, string styleSheetFolder, bool isBeta, bool isArchive)
	{
		Guid.TryParse(identifier, out _identifier);
		VersionNumber = versionNumber;
		Grouping = grouping;
		Name = name;
		Description = description;
		PathToImage = pathToImage;
		StyleSheetFolder = styleSheetFolder;
		IsBeta = isBeta;
		IsArchive = isArchive;
		IsDisplayVisible = true;
	}

	public static CSPatchWireAttribute GetAttributeFromDecoratedClass(Type type, string methodName)
	{
		CSPatchWireAttribute result = null;
		MemberInfo memberInfo = type.GetMember(methodName).FirstOrDefault();
		if (memberInfo != null)
		{
			result = memberInfo.GetCustomAttributes(typeof(CSPatchWireAttribute), inherit: false).OfType<CSPatchWireAttribute>().FirstOrDefault();
		}
		return result;
	}
}
