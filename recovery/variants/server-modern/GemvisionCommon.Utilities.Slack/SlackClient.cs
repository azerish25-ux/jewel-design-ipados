using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using GemvisionCommon.Helpers;
using Newtonsoft.Json;

namespace GemvisionCommon.Utilities.Slack;

public sealed class SlackClient
{
	private const string UrlWithAccessToken = "REDACTED_EMBEDDED_CREDENTIAL";

	private readonly Uri _uri = new Uri("REDACTED_EMBEDDED_CREDENTIAL");

	private readonly Encoding _encoding = new UTF8Encoding();

	public void PostMessage(string text, string username = null, string channel = null)
	{
		Payload payload = new Payload
		{
			Channel = channel,
			Username = username,
			Text = text
		};
		RetryHelper.Do(() => PostMessage(payload));
	}

	private bool PostMessage(Payload payload)
	{
		string value = JsonConvert.SerializeObject(payload);
		using WebClient webClient = new WebClient();
		NameValueCollection data = new NameValueCollection { ["payload"] = value };
		byte[] bytes = webClient.UploadValues(_uri, "POST", data);
		string text = _encoding.GetString(bytes);
		return text.Trim().ToLowerInvariant().Equals("ok");
	}
}
