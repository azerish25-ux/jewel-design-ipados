using System;
using System.IO;
using System.Net;

namespace GV.CounterSketch.Shared;

internal sealed class StullerFileService
{
	private readonly string devKey = "REDACTED_EMBEDDED_CREDENTIAL";

	private string fileURL;

	public string TemporaryPath { get; set; }

	public string LastResult { get; set; }

	public string DownloadFile()
	{
		string text = "www.stuller.com";
		string text2 = "developer/services/stullerfileservice.svc/Download";
		string text3 = "GemVision.zip";
		string text4 = $"DeveloperToken={devKey}";
		if (InternetUtility.IsConnectedToInternet())
		{
			switch (InternetUtility.TestProtocolToSite(text))
			{
			case InternetUtility.NetProtocol.HTTPS:
				fileURL = $"https://{text}/{text2}/{text3}?{text4}";
				break;
			case InternetUtility.NetProtocol.HTTP:
				fileURL = $"http://{text}/{text2}/{text3}?{text4}";
				break;
			case InternetUtility.NetProtocol.BLOCKED:
				throw new Exception($"Connection blocked by Firewall!  Please open outbound HTTP (port 80) to enable updates to: {text}");
			default:
				throw new Exception("Unknown connection issue, we tried to update via HTTP (port 80), and HTTPS (port 443) but could not update.");
			}
			if (TemporaryPath == null)
			{
				throw new Exception("HTTP ERROR: You must specify the temporary path to save the file.");
			}
			if (!Directory.Exists(TemporaryPath))
			{
				LastResult += $"Temporary Folder: {TemporaryPath} does not exist.";
				return null;
			}
			string text5 = Path.Combine(TemporaryPath, $"{default(Guid)}StullerDBUpdate.zip");
			using (WebClient webClient = new WebClient())
			{
				webClient.DownloadFile(fileURL, text5);
			}
			LastResult += $"{GetTimeStamp()} Successfully downloaded the update.\n\r";
			return text5;
		}
		throw new Exception("You must be connected to the Internet to download an update.  Please connect and try again");
	}

	private string GetTimeStamp()
	{
		return $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} : ";
	}
}
