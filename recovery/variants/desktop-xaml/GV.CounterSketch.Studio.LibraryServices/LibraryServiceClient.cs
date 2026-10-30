using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace GV.CounterSketch.Studio.LibraryServices;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class LibraryServiceClient : ClientBase<ILibraryService>, ILibraryService
{
	public LibraryServiceClient()
	{
	}

	public LibraryServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public LibraryServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public LibraryServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public LibraryServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public int GetLatestLibraryVersionByKeyId(int keyId, string counterSketchVersion)
	{
		return base.Channel.GetLatestLibraryVersionByKeyId(keyId, counterSketchVersion);
	}

	public Task<int> GetLatestLibraryVersionByKeyIdAsync(int keyId, string counterSketchVersion)
	{
		return base.Channel.GetLatestLibraryVersionByKeyIdAsync(keyId, counterSketchVersion);
	}

	public int GetIterationIndexForIterationComponentIdAndVersionNumber(int iterationComponentId, int versionNumber)
	{
		return base.Channel.GetIterationIndexForIterationComponentIdAndVersionNumber(iterationComponentId, versionNumber);
	}

	public Task<int> GetIterationIndexForIterationComponentIdAndVersionNumberAsync(int iterationComponentId, int versionNumber)
	{
		return base.Channel.GetIterationIndexForIterationComponentIdAndVersionNumberAsync(iterationComponentId, versionNumber);
	}

	public DateTime GetDesignLibraryReleaseDate(int libraryId)
	{
		return base.Channel.GetDesignLibraryReleaseDate(libraryId);
	}

	public Task<DateTime> GetDesignLibraryReleaseDateAsync(int libraryId)
	{
		return base.Channel.GetDesignLibraryReleaseDateAsync(libraryId);
	}

	public DateTime GetPartLibraryReleaseDate(int libraryId)
	{
		return base.Channel.GetPartLibraryReleaseDate(libraryId);
	}

	public Task<DateTime> GetPartLibraryReleaseDateAsync(int libraryId)
	{
		return base.Channel.GetPartLibraryReleaseDateAsync(libraryId);
	}
}
