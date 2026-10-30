using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace GV.CounterSketchServices.WebServices;

[GeneratedCode("wsdl", "4.0.30319.18020")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[WebServiceBinding(Name = "CounterSketchSecurityServiceSoap", Namespace = "http://CounterSketch.biz/")]
public class CounterSketchSecurityService : SoapHttpClientProtocol
{
	private SendOrPostCallback getKeyIdFromCSCodeOperationCompleted;

	private SendOrPostCallback sendbytesOperationCompleted;

	private SendOrPostCallback sendbytes1OperationCompleted;

	private SendOrPostCallback sendbytes3OperationCompleted;

	private SendOrPostCallback sendbytes4OperationCompleted;

	public event getKeyIdFromCSCodeCompletedEventHandler getKeyIdFromCSCodeCompleted;

	public event sendbytesCompletedEventHandler sendbytesCompleted;

	public event sendbytes1CompletedEventHandler sendbytes1Completed;

	public event sendbytes3CompletedEventHandler sendbytes3Completed;

	public event sendbytes4CompletedEventHandler sendbytes4Completed;

	public CounterSketchSecurityService()
	{
		base.Url = "https://countersketch.biz/CounterSketchSecurityService.asmx";
	}

	[SoapDocumentMethod("http://CounterSketch.biz/getKeyIdFromCSCode", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int getKeyIdFromCSCode([XmlElement(DataType = "base64Binary")] byte[] code)
	{
		return (int)Invoke("getKeyIdFromCSCode", new object[1] { code })[0];
	}

	public IAsyncResult BegingetKeyIdFromCSCode(byte[] code, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("getKeyIdFromCSCode", new object[1] { code }, callback, asyncState);
	}

	public int EndgetKeyIdFromCSCode(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void getKeyIdFromCSCodeAsync(byte[] code)
	{
		getKeyIdFromCSCodeAsync(code, null);
	}

	public void getKeyIdFromCSCodeAsync(byte[] code, object userState)
	{
		if (getKeyIdFromCSCodeOperationCompleted == null)
		{
			getKeyIdFromCSCodeOperationCompleted = OngetKeyIdFromCSCodeOperationCompleted;
		}
		InvokeAsync("getKeyIdFromCSCode", new object[1] { code }, getKeyIdFromCSCodeOperationCompleted, userState);
	}

	private void OngetKeyIdFromCSCodeOperationCompleted(object arg)
	{
		if (this.getKeyIdFromCSCodeCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.getKeyIdFromCSCodeCompleted(this, new getKeyIdFromCSCodeCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/sendbytes", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement(DataType = "base64Binary")]
	public byte[] sendbytes([XmlElement(DataType = "base64Binary")] byte[] sendB, string Name, string CompanyName)
	{
		return (byte[])Invoke("sendbytes", new object[3] { sendB, Name, CompanyName })[0];
	}

	public IAsyncResult Beginsendbytes(byte[] sendB, string Name, string CompanyName, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("sendbytes", new object[3] { sendB, Name, CompanyName }, callback, asyncState);
	}

	public byte[] Endsendbytes(IAsyncResult asyncResult)
	{
		return (byte[])EndInvoke(asyncResult)[0];
	}

	public void sendbytesAsync(byte[] sendB, string Name, string CompanyName)
	{
		sendbytesAsync(sendB, Name, CompanyName, null);
	}

	public void sendbytesAsync(byte[] sendB, string Name, string CompanyName, object userState)
	{
		if (sendbytesOperationCompleted == null)
		{
			sendbytesOperationCompleted = OnsendbytesOperationCompleted;
		}
		InvokeAsync("sendbytes", new object[3] { sendB, Name, CompanyName }, sendbytesOperationCompleted, userState);
	}

	private void OnsendbytesOperationCompleted(object arg)
	{
		if (this.sendbytesCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.sendbytesCompleted(this, new sendbytesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/sendbytes1", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement(DataType = "base64Binary")]
	public byte[] sendbytes1([XmlElement(DataType = "base64Binary")] byte[] sendB, string Name, string CompanyName)
	{
		return (byte[])Invoke("sendbytes1", new object[3] { sendB, Name, CompanyName })[0];
	}

	public IAsyncResult Beginsendbytes1(byte[] sendB, string Name, string CompanyName, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("sendbytes1", new object[3] { sendB, Name, CompanyName }, callback, asyncState);
	}

	public byte[] Endsendbytes1(IAsyncResult asyncResult)
	{
		return (byte[])EndInvoke(asyncResult)[0];
	}

	public void sendbytes1Async(byte[] sendB, string Name, string CompanyName)
	{
		sendbytes1Async(sendB, Name, CompanyName, null);
	}

	public void sendbytes1Async(byte[] sendB, string Name, string CompanyName, object userState)
	{
		if (sendbytes1OperationCompleted == null)
		{
			sendbytes1OperationCompleted = Onsendbytes1OperationCompleted;
		}
		InvokeAsync("sendbytes1", new object[3] { sendB, Name, CompanyName }, sendbytes1OperationCompleted, userState);
	}

	private void Onsendbytes1OperationCompleted(object arg)
	{
		if (this.sendbytes1Completed != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.sendbytes1Completed(this, new sendbytes1CompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/sendbytes3", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement(DataType = "base64Binary")]
	public byte[] sendbytes3([XmlElement(DataType = "base64Binary")] byte[] sendB, string Name, string CompanyName, string CSVersion, string RhinoVersion, string LibraryVersion, string ModelCount)
	{
		return (byte[])Invoke("sendbytes3", new object[7] { sendB, Name, CompanyName, CSVersion, RhinoVersion, LibraryVersion, ModelCount })[0];
	}

	public IAsyncResult Beginsendbytes3(byte[] sendB, string Name, string CompanyName, string CSVersion, string RhinoVersion, string LibraryVersion, string ModelCount, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("sendbytes3", new object[7] { sendB, Name, CompanyName, CSVersion, RhinoVersion, LibraryVersion, ModelCount }, callback, asyncState);
	}

	public byte[] Endsendbytes3(IAsyncResult asyncResult)
	{
		return (byte[])EndInvoke(asyncResult)[0];
	}

	public void sendbytes3Async(byte[] sendB, string Name, string CompanyName, string CSVersion, string RhinoVersion, string LibraryVersion, string ModelCount)
	{
		sendbytes3Async(sendB, Name, CompanyName, CSVersion, RhinoVersion, LibraryVersion, ModelCount, null);
	}

	public void sendbytes3Async(byte[] sendB, string Name, string CompanyName, string CSVersion, string RhinoVersion, string LibraryVersion, string ModelCount, object userState)
	{
		if (sendbytes3OperationCompleted == null)
		{
			sendbytes3OperationCompleted = Onsendbytes3OperationCompleted;
		}
		InvokeAsync("sendbytes3", new object[7] { sendB, Name, CompanyName, CSVersion, RhinoVersion, LibraryVersion, ModelCount }, sendbytes3OperationCompleted, userState);
	}

	private void Onsendbytes3OperationCompleted(object arg)
	{
		if (this.sendbytes3Completed != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.sendbytes3Completed(this, new sendbytes3CompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/sendbytes4", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement(DataType = "base64Binary")]
	public byte[] sendbytes4([XmlElement(DataType = "base64Binary")] byte[] sendB, string Name, string CompanyName)
	{
		return (byte[])Invoke("sendbytes4", new object[3] { sendB, Name, CompanyName })[0];
	}

	public IAsyncResult Beginsendbytes4(byte[] sendB, string Name, string CompanyName, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("sendbytes4", new object[3] { sendB, Name, CompanyName }, callback, asyncState);
	}

	public byte[] Endsendbytes4(IAsyncResult asyncResult)
	{
		return (byte[])EndInvoke(asyncResult)[0];
	}

	public void sendbytes4Async(byte[] sendB, string Name, string CompanyName)
	{
		sendbytes4Async(sendB, Name, CompanyName, null);
	}

	public void sendbytes4Async(byte[] sendB, string Name, string CompanyName, object userState)
	{
		if (sendbytes4OperationCompleted == null)
		{
			sendbytes4OperationCompleted = Onsendbytes4OperationCompleted;
		}
		InvokeAsync("sendbytes4", new object[3] { sendB, Name, CompanyName }, sendbytes4OperationCompleted, userState);
	}

	private void Onsendbytes4OperationCompleted(object arg)
	{
		if (this.sendbytes4Completed != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.sendbytes4Completed(this, new sendbytes4CompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public new void CancelAsync(object userState)
	{
		base.CancelAsync(userState);
	}
}
