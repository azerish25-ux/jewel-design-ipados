using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.ServiceModel;
using System.Windows.Input;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Shared.Model.Objects;

namespace GV.CounterSketch.Studio.RhinoServicesReference;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public interface IRhinoServicesCallback
{
	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IRhinoServices/FreehandControlUpdateCallback")]
	void FreehandControlUpdateCallback(UIControl[] uiControls);

	[OperationContract(IsOneWay = true, AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/FreehandControlUpdateCallback")]
	IAsyncResult BeginFreehandControlUpdateCallback(UIControl[] uiControls, AsyncCallback callback, object asyncState);

	void EndFreehandControlUpdateCallback(IAsyncResult result);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IRhinoServices/FreehandImportGroupBitmapUpdateCallback")]
	void FreehandImportGroupBitmapUpdateCallback(KeyValuePair<Guid, Bitmap> importGroupBitmap);

	[OperationContract(IsOneWay = true, AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/FreehandImportGroupBitmapUpdateCallback")]
	IAsyncResult BeginFreehandImportGroupBitmapUpdateCallback(KeyValuePair<Guid, Bitmap> importGroupBitmap, AsyncCallback callback, object asyncState);

	void EndFreehandImportGroupBitmapUpdateCallback(IAsyncResult result);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IRhinoServices/FreehandImportGroupUpdateCallback")]
	void FreehandImportGroupUpdateCallback(ImportGroup[] importGroups);

	[OperationContract(IsOneWay = true, AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/FreehandImportGroupUpdateCallback")]
	IAsyncResult BeginFreehandImportGroupUpdateCallback(ImportGroup[] importGroups, AsyncCallback callback, object asyncState);

	void EndFreehandImportGroupUpdateCallback(IAsyncResult result);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IRhinoServices/ResetApplicationState")]
	void ResetApplicationState();

	[OperationContract(IsOneWay = true, AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ResetApplicationState")]
	IAsyncResult BeginResetApplicationState(AsyncCallback callback, object asyncState);

	void EndResetApplicationState(IAsyncResult result);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IRhinoServices/RhinoWindowClickedCallback")]
	void RhinoWindowClickedCallback();

	[OperationContract(IsOneWay = true, AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RhinoWindowClickedCallback")]
	IAsyncResult BeginRhinoWindowClickedCallback(AsyncCallback callback, object asyncState);

	void EndRhinoWindowClickedCallback(IAsyncResult result);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IRhinoServices/RhinoWindowKeyPressedCallback")]
	void RhinoWindowKeyPressedCallback(Key keyPressed);

	[OperationContract(IsOneWay = true, AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RhinoWindowKeyPressedCallback")]
	IAsyncResult BeginRhinoWindowKeyPressedCallback(Key keyPressed, AsyncCallback callback, object asyncState);

	void EndRhinoWindowKeyPressedCallback(IAsyncResult result);
}
