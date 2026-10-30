using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

internal class SocketIOHelper
{
	private static class ErrorTypes
	{
		public const string NOTFOUND = "NOTFOUND";

		public const string INVALID = "INVALID";

		public const string PERMISSION = "PERMISSION";

		public const string OPERATIONFAILED = "OPERATIONFAILED";
	}

	private static class ResponseTypes
	{
		public const string SUCESSFUL_CONNECTION = "sucessfulConnection";

		public const string USERCRED = "userCred";
	}

	private static Socket socket = null;

	private static readonly string GEMSBASEPATH = "https://gems.gemvision.services/v3";

	private static readonly string DECRYPTENDPONT = "/decrypt";

	private static readonly string AUTHTOKENENDPOINT = "/get/paramStore";

	private static string FINGERPRINT = "";

	private static string USER = "";

	private static string SESSIONID = "";

	private static string TOKEN = "";

	private static string USERTOKEN = "";

	public static string UnencryptedPassword = "";

	public static string Error = "";

	public static bool IsAuthenticating = false;

	internal static void GetStullerCredentialsFromAmazon(string userName, string sessionId, string fingerPrint)
	{
		IsAuthenticating = true;
		FINGERPRINT = fingerPrint;
		USER = userName;
		SESSIONID = sessionId;
		GlobalErrorHandler.WriteInfoToEventLog("Starting Connection Attempt for User: " + USER);
		IO.Options opts = new IO.Options
		{
			AutoConnect = false
		};
		socket = IO.Socket("http://message.gemvision.services/", opts);
		if (socket == null)
		{
			return;
		}
		socket.On(Socket.EVENT_CONNECT, (Action)async delegate
		{
			GlobalErrorHandler.WriteInfoToEventLog("Connected To Server...");
			GlobalErrorHandler.WriteInfoToEventLog(socket.Io().EngineSocket.Id);
			GlobalErrorHandler.WriteInfoToEventLog("Lets Link To The Current Session...");
			await Task.Run(() => LinkToRoom());
		});
		socket.On("sucessfulConnection", async delegate(object obj)
		{
			JObject jObject = JObject.Parse(obj.ToString());
			GlobalErrorHandler.WriteInfoToEventLog("Successful Connection: " + jObject.GetValue("message").ToString());
			await Task.Run(delegate
			{
				RequestUserCred();
			});
		});
		socket.On("userCred", async delegate(object obj)
		{
			USERTOKEN = (string)(JToken)JObject.Parse(obj.ToString()).Property("message");
			GlobalErrorHandler.WriteInfoToEventLog($"User Creds Fetched... {string.Compare(TOKEN, USERTOKEN)}");
			await Task.Run(() => DecryptUserToken("\"" + USERTOKEN + "\""));
			IsAuthenticating = false;
		});
		socket.On(Socket.EVENT_CONNECT_ERROR, delegate(object m)
		{
			GlobalErrorHandler.WriteInfoToEventLog($"Connect Error : \r{JObject.Parse(m.ToString())}");
			Abort();
		});
		socket.On("INVALID", delegate(object m)
		{
			GlobalErrorHandler.WriteInfoToEventLog($"Invalid Error: \r{JObject.Parse(m.ToString())}");
			Abort();
		});
		socket.On("NOTFOUND", delegate(object m)
		{
			GlobalErrorHandler.WriteInfoToEventLog($"Not Found Error: \r{JObject.Parse(m.ToString())}");
			Abort();
		});
		socket.On("OPERATIONFAILED", delegate(object m)
		{
			GlobalErrorHandler.WriteInfoToEventLog($"Operation Failed Error: \r{JObject.Parse(m.ToString())}");
			Abort();
		});
		socket.On("PERMISSION", delegate(object m)
		{
			GlobalErrorHandler.WriteInfoToEventLog($"Permission Error: \r{JObject.Parse(m.ToString())}");
			Abort();
		});
		socket.Connect();
	}

	public static void Abort()
	{
		IsAuthenticating = false;
		GlobalErrorHandler.WriteInfoToEventLog("Aborting Socket IO Logic. Disconnect()");
		socket.Disconnect();
	}

	private static async Task RequestAuthToken()
	{
		try
		{
			GlobalErrorHandler.WriteInfoToEventLog("Requesting Auth Token....");
			string stringContent = "{\"path\":\"/gvSocketIO/authKey\"}";
			TOKEN = await RequestGEMSResponse(GEMSBASEPATH + AUTHTOKENENDPOINT, stringContent, USER, FINGERPRINT);
			GlobalErrorHandler.WriteInfoToEventLog("Auth Token Fetched...");
		}
		catch (Exception ex)
		{
			GlobalErrorHandler.WriteErrorToEventLog(ex.ToString());
		}
	}

	private static async Task LinkToRoom()
	{
		try
		{
			await RequestAuthToken();
			GlobalErrorHandler.WriteInfoToEventLog("Sending Link Event To Join Session");
			string text = "{\"sessionId\":\"" + SESSIONID + "\",\"authKey\":" + TOKEN + ",\"userId\":\"1\"}";
			socket.Emit("link", text);
		}
		catch (Exception ex)
		{
			GlobalErrorHandler.WriteErrorToEventLog(ex.ToString());
		}
	}

	private static void RequestUserCred()
	{
		try
		{
			GlobalErrorHandler.WriteInfoToEventLog("Sending Request Cred Event");
			string text = "{\"sessionId\":\"" + SESSIONID + "\",\"authKey\":" + TOKEN + "}";
			socket.Emit("requestCred", text);
		}
		catch (Exception ex)
		{
			GlobalErrorHandler.WriteErrorToEventLog(ex.ToString());
		}
	}

	private static async Task DecryptUserToken(string userToken)
	{
		try
		{
			GlobalErrorHandler.WriteInfoToEventLog("DECRYPTING USER Token....");
			string stringContent = "{\"token\":" + userToken + "}";
			UnencryptedPassword = JObject.Parse(await RequestGEMSResponse(GEMSBASEPATH + DECRYPTENDPONT, stringContent, USER, FINGERPRINT)).Property("Payload").Value.ToString();
			GlobalErrorHandler.WriteInfoToEventLog("USER Token DECRYPTED...");
			GlobalErrorHandler.WriteInfoToEventLog(UnencryptedPassword ?? "");
		}
		catch (Exception ex)
		{
			GlobalErrorHandler.WriteErrorToEventLog(ex.ToString());
		}
	}

	private static async Task<string> RequestGEMSResponse(string endpoint, string stringContent, string userName, string fingerPrint)
	{
		try
		{
			HttpClient httpClient = new HttpClient();
			httpClient.BaseAddress = new Uri(endpoint);
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userName + " 0 " + fingerPrint);
			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, endpoint)
			{
				Content = new StringContent(stringContent, Encoding.UTF8, "application/json")
			};
			return await httpClient.SendAsync(request).ContinueWith((Task<HttpResponseMessage> responseTask) => responseTask.Result.Content.ReadAsStringAsync()).Result;
		}
		catch (Exception)
		{
		}
		return string.Empty;
	}
}
