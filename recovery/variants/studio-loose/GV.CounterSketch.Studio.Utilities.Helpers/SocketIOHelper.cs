using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Quobject.EngineIoClientDotNet.ComponentEmitter;
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
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		IsAuthenticating = true;
		FINGERPRINT = fingerPrint;
		USER = userName;
		SESSIONID = sessionId;
		GlobalErrorHandler.WriteInfoToEventLog("Starting Connection Attempt for User: " + USER);
		Options val = new Options
		{
			AutoConnect = false
		};
		socket = IO.Socket("http://message.gemvision.services/", val);
		if (socket == null)
		{
			return;
		}
		((Emitter)socket).On(Socket.EVENT_CONNECT, (Action)async delegate
		{
			GlobalErrorHandler.WriteInfoToEventLog("Connected To Server...");
			GlobalErrorHandler.WriteInfoToEventLog(socket.Io().EngineSocket.Id);
			GlobalErrorHandler.WriteInfoToEventLog("Lets Link To The Current Session...");
			await Task.Run(() => LinkToRoom());
		});
		((Emitter)socket).On("sucessfulConnection", (Action<object>)async delegate(object obj)
		{
			JObject val2 = JObject.Parse(obj.ToString());
			GlobalErrorHandler.WriteInfoToEventLog("Successful Connection: " + ((object)val2.GetValue("message")).ToString());
			await Task.Run(delegate
			{
				RequestUserCred();
			});
		});
		((Emitter)socket).On("userCred", (Action<object>)async delegate(object obj)
		{
			USERTOKEN = (string)(JToken)(object)JObject.Parse(obj.ToString()).Property("message");
			GlobalErrorHandler.WriteInfoToEventLog($"User Creds Fetched... {string.Compare(TOKEN, USERTOKEN)}");
			await Task.Run(() => DecryptUserToken("\"" + USERTOKEN + "\""));
			IsAuthenticating = false;
		});
		((Emitter)socket).On(Socket.EVENT_CONNECT_ERROR, (Action<object>)delegate(object m)
		{
			GlobalErrorHandler.WriteInfoToEventLog($"Connect Error : \r{JObject.Parse(m.ToString())}");
			Abort();
		});
		((Emitter)socket).On("INVALID", (Action<object>)delegate(object m)
		{
			GlobalErrorHandler.WriteInfoToEventLog($"Invalid Error: \r{JObject.Parse(m.ToString())}");
			Abort();
		});
		((Emitter)socket).On("NOTFOUND", (Action<object>)delegate(object m)
		{
			GlobalErrorHandler.WriteInfoToEventLog($"Not Found Error: \r{JObject.Parse(m.ToString())}");
			Abort();
		});
		((Emitter)socket).On("OPERATIONFAILED", (Action<object>)delegate(object m)
		{
			GlobalErrorHandler.WriteInfoToEventLog($"Operation Failed Error: \r{JObject.Parse(m.ToString())}");
			Abort();
		});
		((Emitter)socket).On("PERMISSION", (Action<object>)delegate(object m)
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
			((Emitter)socket).Emit("link", new object[1] { text });
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
			((Emitter)socket).Emit("requestCred", new object[1] { text });
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
			UnencryptedPassword = ((object)JObject.Parse(await RequestGEMSResponse(GEMSBASEPATH + DECRYPTENDPONT, stringContent, USER, FINGERPRINT)).Property("Payload").Value).ToString();
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
			HttpClient val = new HttpClient
			{
				BaseAddress = new Uri(endpoint)
			};
			val.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			val.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userName + " 0 " + fingerPrint);
			HttpRequestMessage val2 = new HttpRequestMessage(HttpMethod.Post, endpoint)
			{
				Content = (HttpContent)new StringContent(stringContent, Encoding.UTF8, "application/json")
			};
			return await val.SendAsync(val2).ContinueWith((Task<HttpResponseMessage> responseTask) => responseTask.Result.Content.ReadAsStringAsync()).Result;
		}
		catch (Exception)
		{
		}
		return string.Empty;
	}
}
