using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace DesignLibrary;

public class SlackClient
{
	private readonly Uri _uri;

	private readonly Encoding _encoding = new UTF8Encoding();

	public SlackClient(string urlWithAccessToken)
	{
		_uri = new Uri(urlWithAccessToken);
	}

	public void PostMessage(string text, string username = null, string channel = null)
	{
		Payload payload = new Payload
		{
			Channel = channel,
			Username = username,
			Text = text
		};
		PostMessage(payload);
	}

	public void PostMessage(Payload payload)
	{
		string value = JsonConvert.SerializeObject((object)payload);
		using WebClient webClient = new WebClient();
		NameValueCollection nameValueCollection = new NameValueCollection();
		nameValueCollection["payload"] = value;
		byte[] bytes = webClient.UploadValues(_uri, "POST", nameValueCollection);
		_encoding.GetString(bytes);
	}
}
