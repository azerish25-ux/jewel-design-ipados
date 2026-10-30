using CS.CSValues.CSValueHandle.HandleSupportClasses;
using CSDatas;
using Rhino.Display;
using Rhino.Geometry;
using Rhino.UI;

namespace CS.CSValues.CSValueHandle.Interfaces;

public interface ICSVHandle
{
	bool MouseOver { get; set; }

	bool MouseDown { get; set; }

	void DrawBackGround(DisplayPipeline displayPipeline);

	void DrawHandle(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions);

	void DrawOverLay(DisplayPipeline displayPipeline);

	bool MouseMove(MouseCallbackEventArgs e);

	bool MouseMoveHitTest(MouseCallbackEventArgs e);

	void MouseUp(MouseCallbackEventArgs e);

	bool SelectionChanged();

	void SetLinked(CSValueHandleHelper.WSelected type, double value, Transform xTrans);
}
