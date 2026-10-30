using System;
using System.IO;
using System.Net;
using GV.CounterSketch.Shared;

namespace GV.CounterSketch.Studio.Web_Services;

internal class StullerFileService
{
	private const string DevKey = "REDACTED_EMBEDDED_CREDENTIAL";

	private string _fileUrl;

	public string TemporaryPath { get; set; }

	public string LastResult { get; set; }

	public string DownloadFile()
	{
		GlobalErrorHandler.WriteMethodInfo("DownloadFile");
		string text = string.Format("DeveloperToken={0}", "aa515afe-6077-4f02-9e78-f7fab2a4aff3");
		if (InternetUtility.IsConnectedToInternet())
		{
			switch (InternetUtility.TestProtocolToSite("www.stuller.com"))
			{
			case InternetUtility.NetProtocol.HTTPS:
				_fileUrl = string.Format("https://{0}/{1}/{2}?{3}", "www.stuller.com", "developer/services/stullerfileservice.svc/Download", "GemVision.zip", text);
				break;
			case InternetUtility.NetProtocol.HTTP:
				_fileUrl = string.Format("http://{0}/{1}/{2}?{3}", "www.stuller.com", "developer/services/stullerfileservice.svc/Download", "GemVision.zip", text);
				break;
			case InternetUtility.NetProtocol.BLOCKED:
				throw new Exception(string.Format("Connection blocked by Firewall!  Please open outbound HTTP (port 80) to enable updates to: {0}", "www.stuller.com"));
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
			string text2 = Path.Combine(TemporaryPath, $"{default(Guid)}StullerDBUpdate.zip");
			using (WebClient webClient = new WebClient())
			{
				webClient.DownloadFile(_fileUrl, text2);
			}
			LastResult += $"{GetTimeStamp()} Successfully downloaded the update.\n\r";
			return text2;
		}
		throw new Exception("You must be connected to the Internet to download an update.  Please connect and try again");
	}

	private string GetTimeStamp()
	{
		return $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} : ";
	}
}
