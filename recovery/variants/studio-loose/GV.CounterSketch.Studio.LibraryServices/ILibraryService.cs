using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GV.CounterSketch.Studio.LibraryServices;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "LibraryServices.ILibraryService")]
public interface ILibraryService
{
	[OperationContract(Action = "http://tempuri.org/ILibraryService/GetLatestLibraryVersionByKeyId", ReplyAction = "http://tempuri.org/ILibraryService/GetLatestLibraryVersionByKeyIdResponse")]
	int GetLatestLibraryVersionByKeyId(int keyId, string counterSketchVersion);

	[OperationContract(Action = "http://tempuri.org/ILibraryService/GetLatestLibraryVersionByKeyId", ReplyAction = "http://tempuri.org/ILibraryService/GetLatestLibraryVersionByKeyIdResponse")]
	Task<int> GetLatestLibraryVersionByKeyIdAsync(int keyId, string counterSketchVersion);

	[OperationContract(Action = "http://tempuri.org/ILibraryService/GetIterationIndexForIterationComponentIdAndVersionNumber", ReplyAction = "http://tempuri.org/ILibraryService/GetIterationIndexForIterationComponentIdAndVersionNumberResponse")]
	int GetIterationIndexForIterationComponentIdAndVersionNumber(int iterationComponentId, int versionNumber);

	[OperationContract(Action = "http://tempuri.org/ILibraryService/GetIterationIndexForIterationComponentIdAndVersionNumber", ReplyAction = "http://tempuri.org/ILibraryService/GetIterationIndexForIterationComponentIdAndVersionNumberResponse")]
	Task<int> GetIterationIndexForIterationComponentIdAndVersionNumberAsync(int iterationComponentId, int versionNumber);

	[OperationContract(Action = "http://tempuri.org/ILibraryService/GetDesignLibraryReleaseDate", ReplyAction = "http://tempuri.org/ILibraryService/GetDesignLibraryReleaseDateResponse")]
	DateTime GetDesignLibraryReleaseDate(int libraryId);

	[OperationContract(Action = "http://tempuri.org/ILibraryService/GetDesignLibraryReleaseDate", ReplyAction = "http://tempuri.org/ILibraryService/GetDesignLibraryReleaseDateResponse")]
	Task<DateTime> GetDesignLibraryReleaseDateAsync(int libraryId);

	[OperationContract(Action = "http://tempuri.org/ILibraryService/GetPartLibraryReleaseDate", ReplyAction = "http://tempuri.org/ILibraryService/GetPartLibraryReleaseDateResponse")]
	DateTime GetPartLibraryReleaseDate(int libraryId);

	[OperationContract(Action = "http://tempuri.org/ILibraryService/GetPartLibraryReleaseDate", ReplyAction = "http://tempuri.org/ILibraryService/GetPartLibraryReleaseDateResponse")]
	Task<DateTime> GetPartLibraryReleaseDateAsync(int libraryId);
}
