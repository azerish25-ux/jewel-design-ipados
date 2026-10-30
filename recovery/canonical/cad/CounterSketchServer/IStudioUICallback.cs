using System;
using System.Collections.Generic;
using System.Drawing;
using System.ServiceModel;
using System.Windows.Input;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Shared.Model.Objects;

namespace CounterSketchServer;

public interface IStudioUICallback
{
	[OperationContract(IsOneWay = true)]
	void FreehandControlUpdateCallback(IEnumerable<UIControl> uiControls);

	[OperationContract(IsOneWay = true)]
	void FreehandImportGroupBitmapUpdateCallback(KeyValuePair<Guid, Bitmap> importGroupBitmap);

	[OperationContract(IsOneWay = true)]
	void FreehandImportGroupUpdateCallback(IEnumerable<ImportGroup> importGroups);

	[OperationContract(IsOneWay = true)]
	void ResetApplicationState();

	[OperationContract(IsOneWay = true)]
	void RhinoWindowClickedCallback();

	[OperationContract(IsOneWay = true)]
	void RhinoWindowKeyPressedCallback(Key keyPressed);
}
