using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GV.CounterSketch.Studio.DesignExportServiceReference;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "DesignExportServiceReference.IDesignExportService")]
public interface IDesignExportService
{
	[OperationContract(Action = "http://tempuri.org/IDesignExportService/GetDesignExportCount", ReplyAction = "http://tempuri.org/IDesignExportService/GetDesignExportCountResponse")]
	int GetDesignExportCount(int keyId, int lastNumberOfDays);

	[OperationContract(Action = "http://tempuri.org/IDesignExportService/GetDesignExportCount", ReplyAction = "http://tempuri.org/IDesignExportService/GetDesignExportCountResponse")]
	Task<int> GetDesignExportCountAsync(int keyId, int lastNumberOfDays);

	[OperationContract(Action = "http://tempuri.org/IDesignExportService/GetDesignExportBonus", ReplyAction = "http://tempuri.org/IDesignExportService/GetDesignExportBonusResponse")]
	int GetDesignExportBonus(int keyId);

	[OperationContract(Action = "http://tempuri.org/IDesignExportService/GetDesignExportBonus", ReplyAction = "http://tempuri.org/IDesignExportService/GetDesignExportBonusResponse")]
	Task<int> GetDesignExportBonusAsync(int keyId);

	[OperationContract(Action = "http://tempuri.org/IDesignExportService/SetDesignExportBonus", ReplyAction = "http://tempuri.org/IDesignExportService/SetDesignExportBonusResponse")]
	void SetDesignExportBonus(string secret, int keyIdToChange, int bonusCount);

	[OperationContract(Action = "http://tempuri.org/IDesignExportService/SetDesignExportBonus", ReplyAction = "http://tempuri.org/IDesignExportService/SetDesignExportBonusResponse")]
	Task SetDesignExportBonusAsync(string secret, int keyIdToChange, int bonusCount);

	[OperationContract(Action = "http://tempuri.org/IDesignExportService/AddDesignExportRecord", ReplyAction = "http://tempuri.org/IDesignExportService/AddDesignExportRecordResponse")]
	void AddDesignExportRecord(int keyId, int designId, int numberOfFreehandParts, int currentExportCount, int currentExportLimit);

	[OperationContract(Action = "http://tempuri.org/IDesignExportService/AddDesignExportRecord", ReplyAction = "http://tempuri.org/IDesignExportService/AddDesignExportRecordResponse")]
	Task AddDesignExportRecordAsync(int keyId, int designId, int numberOfFreehandParts, int currentExportCount, int currentExportLimit);
}
