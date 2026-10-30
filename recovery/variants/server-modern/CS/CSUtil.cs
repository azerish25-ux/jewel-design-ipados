using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.CSObjects.CSTreeObjects;
using CS.CSValues;
using CS.CSValues.CSValueHandle.Interfaces;
using CS.MaterialGroup;
using CS.Parametric;
using CS.Parametric.Channels;
using CS.Utilities.Helpers;
using CSCompatibility;
using CSDatas;
using CSMaterials;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Shared.Model.Objects;
using RMA.OpenNURBS;
using RMA.Rhino;
using RMA.UI;
using Rhino;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CS;

public static class CSUtil
{
	private static readonly bool _isPrintingAllowed = true;

	private static int _enableDrawingCount;

	private static int _layerIndexCreation;

	private static int _layerIndexCutting;

	private static int _layerIndexFinger;

	private static int _layerIndexGem01;

	private static int _layerIndexGem02;

	private static int _layerIndexGem03;

	private static int _layerIndexMetal01;

	private static int _layerIndexMetal02;

	private static int _layerIndexMetal03;

	private static int _layerIndexMetal04;

	private static int _layerIndexUser01;

	private static Dictionary<LayerNumber, int> _layerMap;

	private static ConcurrentBag<RhinoRenderObject> _rhinoRenderObjects;

	private static Timer _timer;

	public static bool IsRedrawDisabled { get; set; } = false;

	public static bool AddObjectFromDocument(CSBlock obj, string objectName)
	{
		return obj?.AddObjectFromDocument(objectName) ?? false;
	}

	public static bool AddObjectFromFile(CSBlock obj, string fileName)
	{
		return obj?.AddObjectFromFile(Path.Combine(CSApplicationHelper.Application.CSLibraryPath, fileName)) ?? false;
	}

	public static Bitmap BitmapFromOnWindowsBitmap(OnWindowsBitmap owb)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		if (!((OnObject)owb).IsValid())
		{
			return null;
		}
		MRhinoDib val = new MRhinoDib(((OnBitmap)owb).Width(), ((OnBitmap)owb).Height(), 32);
		Bitmap original;
		try
		{
			((MRhinoUiDib)val).SetDib(owb);
			original = ((MRhinoUiDib)val).CopyToBitmap();
			((MRhinoUiDib)val).DestroyDib();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		return new Bitmap(original);
	}

	public static void CallFunctionUsingUndoTransaction(Action targetFunction)
	{
		((DisplayConduit)CSApplicationHelper.Application.Conduit).Enabled = false;
		int transactionNumber;
		using (CSUndoTransaction cSUndoTransaction = new CSUndoTransaction())
		{
			transactionNumber = cSUndoTransaction.TransactionNumber;
			targetFunction();
		}
		CSApplicationHelper.Application.Document.UndoTable.UndoLast(transactionNumber);
		((DisplayConduit)CSApplicationHelper.Application.Conduit).Enabled = true;
		ForceGraphToRedraw();
	}

	public static bool ChangeDisplayOfGroup(CSObject obj, bool state)
	{
		CSObject[] range = GetRange(obj);
		if (range == null)
		{
			return false;
		}
		if (range.Length < 1)
		{
			return false;
		}
		if (range[0] is CSPatchWireStart cSPatchWireStart)
		{
			cSPatchWireStart.DisplayVisible = state;
		}
		return true;
	}

	public static void CheckAndTerminateOrphanedParallelTasks()
	{
		if (Process.GetProcesses().All((Process process) => process.Id != CSApplication.RhinoProcessId))
		{
			CancelPendingParallelCSBlockTasks();
		}
	}

	public static DisplayComponents ComponentsEnumToDisplayComponents(ComponentsEnum e)
	{
		return e switch
		{
			ComponentsEnum.Band1 => DisplayComponents.Band, 
			ComponentsEnum.Band2 => DisplayComponents.Band2, 
			ComponentsEnum.Pendant => DisplayComponents.Pendant, 
			ComponentsEnum.Ring => DisplayComponents.Ring, 
			ComponentsEnum.None => DisplayComponents.None, 
			_ => throw new ArgumentOutOfRangeException("e", e, null), 
		};
	}

	public static bool ContainsPrototype()
	{
		return (from graphObject in CSApplicationHelper.Application.Document.Graph.ActiveObjects()
			select graphObject.GetType()).Any((Type type) => type.GetCustomAttributes(inherit: false).OfType<CSBlockAttribute>().Any((CSBlockAttribute atr) => atr.Prototype));
	}

	public static DisplayComponents DisplayComponentsDisplayed()
	{
		return (from obj in CSApplicationHelper.Application.Document.Graph.DisplayObjects()
			where obj.ParentPatchWire != null
			select obj).Aggregate(DisplayComponents.None, (DisplayComponents current, CSBlock obj) => current | obj.ParentPatchWire.DisplayComponent);
	}

	public static DisplayComponents DisplayComponentsInFile()
	{
		return CSApplicationHelper.Application.Document.Graph.AllObjects().OfType<CSPatchWireStart>().Aggregate(DisplayComponents.None, (DisplayComponents current, CSPatchWireStart start) => current | start.DisplayComponent);
	}

	public static DisplayComponents DisplayComponentsInFileWithObjects()
	{
		DisplayComponents displayComponents = DisplayComponentsInFile();
		if (displayComponents.HasFlag(DisplayComponents.Band2))
		{
			List<CSData> displayObjectsByComponentAndMaterialClassification = GraphHelper.GetDisplayObjectsByComponentAndMaterialClassification(ComponentsEnum.Band2, MaterialClassification.GemCenter, MaterialClassification.GemSide, MaterialClassification.GemAccent, MaterialClassification.MetalPrimary, MaterialClassification.MetalSecondary);
			bool flag = false;
			foreach (CSData item in displayObjectsByComponentAndMaterialClassification)
			{
				if (item is CSOnGeometryData)
				{
					flag = true;
					break;
				}
				if (item is CSGemData)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				displayComponents &= ~DisplayComponents.Band2;
			}
		}
		return displayComponents;
	}

	public static List<ComponentsEnum> DisplayComponentsToComponentsEnum(DisplayComponents layers)
	{
		List<ComponentsEnum> list = new List<ComponentsEnum>();
		if ((DisplayComponents.Ring & layers) != DisplayComponents.None)
		{
			list.Add(ComponentsEnum.Ring);
		}
		if ((DisplayComponents.Pendant & layers) != DisplayComponents.None)
		{
			list.Add(ComponentsEnum.Pendant);
		}
		if ((DisplayComponents.Band & layers) != DisplayComponents.None)
		{
			list.Add(ComponentsEnum.Band1);
		}
		if ((DisplayComponents.Band2 & layers) != DisplayComponents.None)
		{
			list.Add(ComponentsEnum.Band2);
		}
		return list;
	}

	public static void DisposeOfRhinoObject(OnObject onRhinoObject)
	{
		((IDisposable)onRhinoObject)?.Dispose();
	}

	public static void DocumentSetCameraView(string cameraViewName)
	{
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		cameraViewName = cameraViewName.Trim().ToLowerInvariant();
		RhinoViewport mainViewport = RhinoDoc.ActiveDoc.Views.ActiveView.MainViewport;
		RhinoView val = RhinoDoc.ActiveDoc.Views.GetStandardRhinoViews().FirstOrDefault((RhinoView view) => view.MainViewport.Name.ToLower() == "perspective");
		if (val != null)
		{
			mainViewport = val.MainViewport;
		}
		mainViewport.ChangeToPerspectiveProjection(true, 50.0);
		Point3d val2 = default(Point3d);
		Point3d origin;
		Vector3d cameraUp;
		switch (cameraViewName)
		{
		case "top":
			mainViewport.SetConstructionPlane(Plane.WorldXY);
			((Point3d)(ref val2))._002Ector(0.0, 0.0, 80.0);
			origin = Point3d.Origin;
			cameraUp = Vector3d.YAxis;
			CSApplicationHelper.Application.CurrentCameraViewName = CameraView.Top;
			break;
		case "front":
			mainViewport.SetConstructionPlane(Plane.WorldZX);
			((Point3d)(ref val2))._002Ector(0.0, -80.0, 0.0);
			origin = Point3d.Origin;
			cameraUp = Vector3d.ZAxis;
			CSApplicationHelper.Application.CurrentCameraViewName = CameraView.Front;
			break;
		case "side":
			mainViewport.SetConstructionPlane(Plane.WorldYZ);
			((Point3d)(ref val2))._002Ector(80.0, 0.0, 0.0);
			origin = Point3d.Origin;
			cameraUp = Vector3d.ZAxis;
			CSApplicationHelper.Application.CurrentCameraViewName = CameraView.Side;
			break;
		default:
			mainViewport.SetConstructionPlane(Plane.WorldXY);
			((Point3d)(ref val2))._002Ector(45.0, -75.0, 50.0);
			origin = Point3d.Origin;
			cameraUp = Vector3d.ZAxis;
			CSApplicationHelper.Application.CurrentCameraViewName = CameraView.Perspective;
			break;
		}
		mainViewport.SetCameraLocations(origin, val2);
		mainViewport.Camera35mmLensLength = 50.0;
		mainViewport.CameraUp = cameraUp;
		if (cameraViewName != "perspective" && !mainViewport.IsParallelProjection)
		{
			mainViewport.ChangeToParallelProjection(true);
		}
		Redraw();
	}

	public static CSSuperViewGroup DuplicateGroup(CSSuperViewGroup svObj, CSParaGraph temporaryGraph = null)
	{
		return (CSSuperViewGroup)DuplicateGraphSegment(new List<CSObject> { svObj }, null, temporaryGraph)[0];
	}

	public static List<CSPatchWireStart> DuplicateImportGroup(ImportGroup importGroupToDuplicate, CSParaGraph temporaryGraph = null, Guid partGroupIdentifier = default(Guid))
	{
		ImportGroup duplicateImportGroup = null;
		CSParaGraph graph = temporaryGraph ?? CSApplicationHelper.Application.Document.Graph;
		if (graph.ImportedCSGParts == null)
		{
			graph.ImportedCSGParts = new List<ImportGroup>();
		}
		string empty = string.Empty;
		duplicateImportGroup = new ImportGroup
		{
			Identifier = Guid.NewGuid(),
			Name = importGroupToDuplicate.Name + empty,
			ImportGroupBitmapImage = null,
			IsViewportVisible = true,
			BlockIdentifiers = new List<Guid>(),
			IsSelectedImportGroup = false,
			IsParentPart = importGroupToDuplicate.IsParentPart,
			IsSecondarySelectedImportGroup = false,
			IsPrimaryMetal = importGroupToDuplicate.IsPrimaryMetal,
			PartGroupId = partGroupIdentifier,
			PartGroupName = ((!string.IsNullOrWhiteSpace(importGroupToDuplicate.PartGroupName)) ? (importGroupToDuplicate.PartGroupName ?? "") : string.Empty)
		};
		CSApplicationHelper.Application.Document.Graph.CategoryTable.Categories.Where((CSUICategory category) => category.ImportGroupIdentifier == importGroupToDuplicate.Identifier).ToList().ForEach(delegate(CSUICategory category)
		{
			graph.CategoryTable.Categories.Add(new CSUICategory(category.CategoryName, duplicateImportGroup.Identifier));
		});
		graph.ImportedCSGParts.Add(duplicateImportGroup);
		Guid startImportObjectIdentifier = importGroupToDuplicate.FirstGraphIdentifier;
		Guid endImportObjectIdentifier = importGroupToDuplicate.LastGraphIdentifier;
		if (startImportObjectIdentifier == Guid.Empty && endImportObjectIdentifier == Guid.Empty)
		{
			startImportObjectIdentifier = importGroupToDuplicate.BlockIdentifiers.FirstOrDefault();
			endImportObjectIdentifier = importGroupToDuplicate.BlockIdentifiers.LastOrDefault();
		}
		CSObject cSObject = CSApplicationHelper.Application.Document.Graph.AllObjects().FirstOrDefault((CSObject csObject) => csObject.InstanceGuid == startImportObjectIdentifier);
		CSObject cSObject2 = CSApplicationHelper.Application.Document.Graph.AllObjects().FirstOrDefault((CSObject csObject) => csObject.InstanceGuid == endImportObjectIdentifier);
		if (cSObject != null && cSObject2 != null)
		{
			int index = GetIndex(cSObject);
			int index2 = GetIndex(cSObject2);
			List<CSTreeObject> source = DuplicateGraphSegment(CSApplicationHelper.Application.Document.Graph.GraphList.Skip(index).Take(index2 - index + 1).ToList(), duplicateImportGroup, temporaryGraph);
			ForceGraphToRedraw(source.SelectMany((CSTreeObject _) => GetRange(_, temporaryGraph)).OfType<CSBlock>().ToList());
			return source.OfType<CSPatchWireStart>().ToList();
		}
		return null;
	}

	public static CSPatchWireStart DuplicatePatchWire(CSObject obj, CSParaGraph temporaryGraph = null)
	{
		int index = GetIndex(obj);
		int index2 = GetIndex(GetRange(obj).LastOrDefault());
		ImportGroup importGroup = null;
		if (CSApplicationHelper.Application.Document.Graph.ImportedCSGParts != null)
		{
			importGroup = CSApplicationHelper.Application.Document.Graph.ImportedCSGParts.FirstOrDefault((ImportGroup _) => _.BlockIdentifiers.Contains(obj.InstanceGuid));
		}
		return (CSPatchWireStart)DuplicateGraphSegment(CSApplicationHelper.Application.Document.Graph.GraphList.Skip(index).Take(index2 - index + 1).ToList(), importGroup, temporaryGraph).FirstOrDefault();
	}

	public static BitmapSource GetBitmapSource(Bitmap image)
	{
		if (image == null)
		{
			return null;
		}
		IntPtr hbitmap = image.GetHbitmap();
		BitmapSource result = Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
		DeleteObject(hbitmap);
		return result;
	}

	public static Bitmap GetBlankImage(int sizeX, int sizeY, Color foreColor)
	{
		Bitmap bitmap = new Bitmap(sizeX, sizeY, PixelFormat.Format32bppArgb);
		Graphics.FromImage(bitmap).DrawString(format: new StringFormat
		{
			Alignment = StringAlignment.Center,
			LineAlignment = StringAlignment.Center
		}, s: "NO\nIMAGE", font: new System.Drawing.Font("Verdana", 7f), brush: new SolidBrush(foreColor), layoutRectangle: new RectangleF(0f, 0f, sizeX, sizeY));
		return bitmap;
	}

	public static int GetCategoryIndex(CSValueBase csValueBase)
	{
		if (csValueBase == null || !csValueBase.Categories.Any())
		{
			return -1;
		}
		return CSApplicationHelper.Application.Document.Graph.CategoryTable.GetCategoryIndex(csValueBase.Categories.FirstOrDefault().CategoryName);
	}

	public static OnWindowsBitmap GetCurrentViewportImage()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		int num = 0;
		int width = 1326;
		int height = 760;
		int num2 = 0;
		RhUtil.RhinoApp().ActiveView().MainViewport()
			.VP()
			.GetScreenPort(ref num, ref width, ref height, ref num2);
		using (Bitmap bitmap = CSDisplayCaptureHelper.CaptureViewportToBitmap(new System.Drawing.Size(width, height)))
		{
			if (bitmap != null)
			{
				return new OnWindowsBitmap(bitmap);
			}
		}
		return null;
	}

	public static Bitmap GetCurrentViewportImageBitmap()
	{
		int num = 0;
		int width = 1326;
		int height = 760;
		int num2 = 0;
		RhUtil.RhinoApp().ActiveView().MainViewport()
			.VP()
			.GetScreenPort(ref num, ref width, ref height, ref num2);
		return CSDisplayCaptureHelper.CaptureViewportToBitmap(new System.Drawing.Size(width, height));
	}

	public static Bitmap GetCurrentViewportImageBitmap(double maxBitmapSize, uint backgroundColor)
	{
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		MRhinoView obj = RhUtil.RhinoApp().ActiveView();
		MRhinoDisplayPipeline val = obj.DisplayPipeline();
		int num = 0;
		int num2 = 1326;
		int num3 = 760;
		int num4 = 0;
		obj.MainViewport().VP().GetScreenPort(ref num, ref num2, ref num3, ref num4);
		int num5 = num2 - num;
		int num6 = num3 - num4;
		if (num5 > num6)
		{
			if ((double)num5 > maxBitmapSize)
			{
				num6 = (int)((double)num6 * (maxBitmapSize / (double)num5));
				num5 = (int)maxBitmapSize;
			}
		}
		else if ((double)num6 > maxBitmapSize)
		{
			num5 = (int)((double)num5 * (maxBitmapSize / (double)num6));
			num6 = (int)maxBitmapSize;
		}
		Bitmap bitmap = new Bitmap(num5, num6, PixelFormat.Format24bppRgb);
		bool flag;
		using (Graphics graphics = Graphics.FromImage(bitmap))
		{
			MDisplayPipelineAttributes val2 = new MDisplayPipelineAttributes(val.DisplayAttrs())
			{
				m_SolidColor = backgroundColor,
				m_eFillMode = (FrameBufferFillMode)2
			};
			flag = val.DrawToDC((IDeviceContext)graphics, num5, num6, val2);
		}
		if (!flag)
		{
			return null;
		}
		return bitmap;
	}

	public static Bitmap GetCurrentViewportImageBitmap(double maxBitmapSize)
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		MRhinoView obj = RhUtil.RhinoApp().ActiveView();
		MRhinoDisplayPipeline val = obj.DisplayPipeline();
		int num = 0;
		int num2 = 1326;
		int num3 = 760;
		int num4 = 0;
		obj.MainViewport().VP().GetScreenPort(ref num, ref num2, ref num3, ref num4);
		int num5 = num2 - num;
		int num6 = num3 - num4;
		if (num5 > num6)
		{
			if ((double)num5 > maxBitmapSize)
			{
				num6 = (int)((double)num6 * (maxBitmapSize / (double)num5));
				num5 = (int)maxBitmapSize;
			}
		}
		else if ((double)num6 > maxBitmapSize)
		{
			num5 = (int)((double)num5 * (maxBitmapSize / (double)num6));
			num6 = (int)maxBitmapSize;
		}
		Bitmap bitmap = new Bitmap(Math.Abs(num5), Math.Abs(num6), PixelFormat.Format24bppRgb);
		bool flag;
		using (Graphics graphics = Graphics.FromImage(bitmap))
		{
			MDisplayPipelineAttributes val2 = new MDisplayPipelineAttributes(val.DisplayAttrs());
			flag = val.DrawToDC((IDeviceContext)graphics, num5, num6, val2);
		}
		if (!flag)
		{
			return null;
		}
		return bitmap;
	}

	public static List<OnGeometry> GetDuplicateObjectsInDisplay(bool checkSolids, bool checkCurves)
	{
		List<OnGeometry> allDisplayObjects = GetAllDisplayObjects(checkSolids, checkCurves);
		List<OnGeometry> list = new List<OnGeometry>();
		for (int i = 0; i < allDisplayObjects.Count; i++)
		{
			OnGeometry geo = allDisplayObjects[i];
			for (int j = i + 1; j < allDisplayObjects.Count; j++)
			{
				OnGeometry val = allDisplayObjects[j];
				if (OnObjectEquals(geo, val))
				{
					list.Add(val);
				}
			}
		}
		return list;
	}

	public static int GetDuplicateObjectsInDisplayCount(bool checkSolids, bool checkCurves)
	{
		return GetDuplicateObjectsInDisplay(checkSolids, checkCurves).Count;
	}

	public static List<CSBlock> GetErrorBlocks()
	{
		List<CSBlock> list = new List<CSBlock>();
		foreach (CSBlock item in CSApplicationHelper.Application.Document.Graph.CSBlocks.Where((CSBlock block) => block != null))
		{
			if (item.Error || item.ExceptionThrown)
			{
				list.Add(item);
			}
		}
		return list;
	}

	public static List<CSBlock> GetErrorBlocksPatchwire(CSObject obj)
	{
		List<CSBlock> list = new List<CSBlock>();
		int start = GetStart(GetIndex(obj));
		int end = GetEnd(GetIndex(obj));
		List<CSObject> graphList = CSApplicationHelper.Application.Document.Graph.GraphList;
		if (start < 0)
		{
			return list;
		}
		for (int i = start; i <= end && i < graphList.Count; i++)
		{
			if (graphList[i] is CSBlock cSBlock && (cSBlock.HasThisObjectChangedSinceLastUpdate() || cSBlock.Error || cSBlock.ExceptionThrown))
			{
				list.Add(cSBlock);
			}
		}
		return list;
	}

	public static List<CSBlock> GetErrorMissingBlocksPatchwire(CSObject obj)
	{
		List<CSBlock> list = new List<CSBlock>();
		int start = GetStart(GetIndex(obj));
		int end = GetEnd(GetIndex(obj));
		List<CSObject> graphList = CSApplicationHelper.Application.Document.Graph.GraphList;
		if (start < 0)
		{
			return list;
		}
		for (int i = start; i <= end && i < graphList.Count; i++)
		{
			if (graphList[i] is CSBlock cSBlock)
			{
				try
				{
					cSBlock.Generate(printTiming: false);
					cSBlock.Update();
				}
				catch (Exception ex)
				{
					cSBlock.ExceptionThrown = true;
					cSBlock.ExceptionMessage = ex.ToString();
				}
				if (cSBlock.HasThisObjectChangedSinceLastUpdate())
				{
					list.Add(cSBlock);
				}
				else if (cSBlock.Error)
				{
					list.Add(cSBlock);
				}
				else if (cSBlock.MissingInput)
				{
					list.Add(cSBlock);
				}
				else if (cSBlock.ExceptionThrown)
				{
					list.Add(cSBlock);
				}
			}
		}
		return list;
	}

	[Obsolete]
	public static CSOutputChannel GetNextOutput(CSOutputChannel source, int offset = 1)
	{
		CSObject parent = source.Parent;
		if (parent == null)
		{
			return null;
		}
		int num = -1;
		for (int i = 0; i < parent.OutputChannelCount; i++)
		{
			if (((CSBlock)parent).OutputChannels[i] == source)
			{
				num = i;
				break;
			}
		}
		if (num < 0)
		{
			return null;
		}
		int num2 = num + offset;
		if (num2 >= parent.OutputChannelCount)
		{
			return null;
		}
		return ((CSBlock)parent).OutputChannels[num2];
	}

	public static CSPatchWireStart GetPatchwireStartUsingCSBlock(CSBlock block)
	{
		int blockIndex = CSApplicationHelper.Application.Document.Graph.AllObjects().ToList().IndexOf(block);
		return CSApplicationHelper.Application.Document.Graph.AllObjects().OfType<CSPatchWireStart>().LastOrDefault((CSPatchWireStart patchwireStart) => CSApplicationHelper.Application.Document.Graph.AllObjects().ToList().IndexOf(patchwireStart) <= blockIndex);
	}

	public static int GetSortIndex(CSValueBase csValueBase)
	{
		if (csValueBase == null)
		{
			return -1;
		}
		int.TryParse(csValueBase.UserData.FindValue("UI", "SortIndex"), out var result);
		return result;
	}

	public static bool GetThumbnailFromFile(string filename, out Bitmap previewImage, out string notes, int height, int width)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		previewImage = null;
		notes = null;
		bool result = false;
		OnFileHandle val = OnUtil.OpenFile(filename, "rb");
		try
		{
			if (val == null)
			{
				return false;
			}
			OnBinaryFile val2 = new OnBinaryFile((archive_mode)5, val);
			try
			{
				int num = 0;
				string empty = string.Empty;
				((OnBinaryArchive)val2).Read3dmStartSection(ref num, ref empty);
				On3dmProperties val3 = new On3dmProperties();
				if (((OnBinaryArchive)val2).Read3dmProperties(ref val3))
				{
					if (val3.m_PreviewImage != null && ((OnBitmap)val3.m_PreviewImage).Width() > 0 && ((OnBitmap)val3.m_PreviewImage).Height() > 0)
					{
						MRhinoDib val4 = new MRhinoDib();
						try
						{
							((MRhinoUiDib)val4).SetDib(val3.m_PreviewImage);
							previewImage = ResizeBitmap(((MRhinoUiDib)val4).CopyToBitmap(), new System.Drawing.Size(width, height));
							result = true;
						}
						finally
						{
							((IDisposable)val4)?.Dispose();
						}
					}
					notes = val3.m_Notes.m_notes;
					val3.Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			OnUtil.CloseFile(val);
			return result;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static bool GetThumbnailFromFile(string filename, out Bitmap previewImage, out string notes)
	{
		return GetThumbnailFromFile(filename, out previewImage, out notes, 70, 90);
	}

	public static string GetVariableName<T>(Expression<Func<T>> expr)
	{
		return ((MemberExpression)expr.Body).Member.Name;
	}

	public static void HighlightGraphItem(Guid blockInstanceGuid, bool showHighlight)
	{
		CSPatchWireStart patchwireForUIControl = GetPatchwireForUIControl(blockInstanceGuid);
		if (patchwireForUIControl != null)
		{
			patchwireForUIControl.MouseOverStart = showHighlight;
		}
	}

	public static ImportGroup IsInImportGroup(CSObject objectItem)
	{
		if (CSApplicationHelper.Application.Document.Graph.ImportedCSGParts != null)
		{
			return CSApplicationHelper.Application.Document.Graph.ImportedCSGParts.FirstOrDefault((ImportGroup importGroup) => importGroup.BlockIdentifiers.Any((Guid _) => _ == objectItem.InstanceGuid));
		}
		return null;
	}

	public static bool IsLayFlatOn()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		Transform layFlatForm = CSApplicationHelper.Application.Conduit._layFlatForm;
		return ((Transform)(ref layFlatForm)).IsValid;
	}

	public static bool IsPrintingAllowed()
	{
		return _isPrintingAllowed;
	}

	public static bool IsValid(List<Guid> invalidGuids)
	{
		if (invalidGuids == null)
		{
			return true;
		}
		if (invalidGuids.Count < 1)
		{
			return true;
		}
		return (from graphObject in CSApplicationHelper.Application.Document.Graph.ActiveObjects()
			select graphObject.GetType()).All((Type type) => !type.GetCustomAttributes(inherit: false).OfType<CSBlockAttribute>().Any((CSBlockAttribute atr) => invalidGuids.Contains(atr.Guid)));
	}

	public static bool IsValidType(List<Type> invalidTypes)
	{
		if (invalidTypes == null)
		{
			return true;
		}
		if (invalidTypes.Count < 1)
		{
			return true;
		}
		foreach (CSObject item in CSApplicationHelper.Application.Document.Graph.ActiveObjects())
		{
			Type type = item.GetType();
			if (invalidTypes.Contains(type))
			{
				return false;
			}
		}
		return true;
	}

	public static bool IsViewCentered()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		if (RhUtil.RhinoApp().ActiveView() == null)
		{
			return false;
		}
		if (RhUtil.RhinoApp().ActiveView().ActiveViewport() == null)
		{
			return false;
		}
		On3dPoint val = new On3dPoint(RhUtil.RhinoApp().ActiveView().ActiveViewport()
			.Target());
		if (Math.Abs(val.x) < 0.001 && Math.Abs(val.x) < 0.001)
		{
			return Math.Abs(val.x) < 0.001;
		}
		return false;
	}

	public static void LayFlat()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Expected O, but got Unknown
		//IL_0240: Expected O, but got Unknown
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		if ((DisplayComponentsInFile() & DisplayComponents.Pendant) == DisplayComponents.Pendant)
		{
			Transform layFlatForm = Transform.Rotation(90.0 * OnUtil.On_DEGREES_TO_RADIANS, Vector3d.XAxis, Point3d.Origin);
			CSApplicationHelper.Application.Conduit._layFlatForm = layFlatForm;
			Redraw();
			return;
		}
		OnMesh val = new OnMesh();
		foreach (CSBlock item in CSApplicationHelper.Application.Document.Graph.DisplayObjects())
		{
			if (item == null)
			{
				continue;
			}
			for (int i = 0; i < item.OutputChannelCount; i++)
			{
				Mesh val2 = item.GetOutputChannelData(i)?.MeshObject(MeshingParameters.Coarse);
				if (val2 != null)
				{
					val.Append((IOnMesh)(object)val2.Convert());
				}
			}
		}
		Transform val3 = Transform.Identity;
		if (CSApplicationHelper.Application.Conduit._isRingFlipped)
		{
			val3 = Transform.Rotation(180.0 * OnUtil.On_DEGREES_TO_RADIANS, Vector3d.ZAxis, Point3d.Origin);
		}
		Transform val4 = Transform.Rotation(90.0 * OnUtil.On_DEGREES_TO_RADIANS, Vector3d.XAxis, Point3d.Origin);
		((OnGeometry)val).Transform((IOnXform)(OnXform)Interop.ToOnXform(val4 * val3));
		On3fPoint val5 = new On3fPoint(0f, 0f, 1000f);
		for (int j = 0; j < ((ArrayOn3fPoint)val.m_V).Count(); j++)
		{
			if (((ArrayOn3fPoint)val.m_V)[j].z < val5.z)
			{
				val5 = ((ArrayOn3fPoint)val.m_V)[j];
			}
		}
		double num = 0f - val5.z;
		((OnGeometry)val).Translate((IOn3dVector)new On3dVector(0.0, 0.0, num));
		val5.z = 0f;
		double num2 = 0.0;
		double num3 = 11.0;
		double num4 = 22.0;
		for (int k = 0; k < 5; k++)
		{
			OnMesh val6 = new OnMesh((IOnMesh)(object)val);
			((OnGeometry)val6).Rotate(CSCoreMath.ToRadians(0.0 - num3), (IOn3dVector)new On3dVector(1.0, 0.0, 0.0), (IOn3dPoint)new On3dPoint((double)val5.x, (double)val5.y, (double)val5.z));
			bool flag = false;
			for (int l = 0; l < ((ArrayOn3fPoint)val6.m_V).Count(); l++)
			{
				if (((ArrayOn3fPoint)val6.m_V)[l].DistanceTo((IOn3fPoint)(object)val5) > ((OnGeometry)val).BoundingBox().Diagonal().y / 2.0 && (double)((ArrayOn3fPoint)val6.m_V)[l].z < -0.1)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				num4 = num3;
				num3 = (num2 + num3) / 2.0;
			}
			else
			{
				num2 = num3;
				num3 = (num4 + num3) / 2.0;
			}
			((OnObject)val6).Dispose();
		}
		Transform val7 = Transform.Translation(new Vector3d(0.0, 0.0, num));
		Transform val8 = Transform.Rotation((0.0 - num3) * OnUtil.On_DEGREES_TO_RADIANS, Vector3d.XAxis, new Point3d((double)val5.x, (double)val5.y, (double)val5.z));
		CSApplicationHelper.Application.Conduit._layFlatForm = val8 * val7 * val4;
		Redraw();
	}

	public static void LayFlatOff()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		CSApplicationHelper.Application.Conduit._layFlatForm = Transform.Unset;
		Redraw();
	}

	public static List<string> ListOfExpandedSliders()
	{
		return (from obj in CSApplicationHelper.Application.Document.Graph.AllObjects()
			let isOn = obj.UserData.FindValue("ExpandedSlider", "IsOn")
			where isOn != null
			select obj.Identifier).ToList();
	}

	public static bool LoadGemFromLibrary(string type, string shape)
	{
		return CSApplicationHelper.Application.Document.GemTable.LoadGemInTable(type, shape);
	}

	public static void Print(object toStringValue)
	{
		if (_isPrintingAllowed)
		{
			RhinoApp.Write(toStringValue?.ToString() ?? "NULL Object");
		}
	}

	public static void PrintLine()
	{
		if (_isPrintingAllowed)
		{
			RhinoApp.WriteLine();
		}
	}

	public static void PrintLine(object toStringValue)
	{
		if (_isPrintingAllowed)
		{
			RhinoApp.WriteLine(toStringValue?.ToString() ?? "NULL Object");
		}
	}

	public static void RedrawAllUI()
	{
		CSApplicationHelper.Application.RaiseRedrawAllUINeededEvent();
	}

	public static void RedrawUI()
	{
		CSApplicationHelper.Application.RaiseRedrawUINeededEvent();
	}

	public static void RedrawUIForced()
	{
		CSApplicationHelper.Application.RaiseRedrawForceUINeededEvent();
	}

	public static bool ResetPatchWireErrors(CSObject obj, bool resetMissing, bool resetError, bool resetException)
	{
		CSPatchWireStart startObject = GetStartObject(obj);
		if (startObject == null)
		{
			return false;
		}
		startObject.MissingInputInPatchwire = false;
		startObject.ErrorInPatchwire = false;
		startObject.ExceptionInPatchwire = false;
		return true;
	}

	public static Bitmap ResizeBitmap(Bitmap source, System.Drawing.Size newSize)
	{
		Bitmap bitmap = new Bitmap(newSize.Width, newSize.Height);
		using (source)
		{
			double num = ((!((double)source.Width / Convert.ToDouble(newSize.Width) > (double)source.Height / Convert.ToDouble(newSize.Height))) ? (Convert.ToDouble(source.Height) / Convert.ToDouble(newSize.Height)) : (Convert.ToDouble(source.Width) / Convert.ToDouble(newSize.Width)));
			double num2 = Math.Ceiling((double)source.Height / num);
			double num3 = Math.Ceiling((double)source.Width / num);
			System.Drawing.Size size = new System.Drawing.Size((int)num3, (int)num2);
			int x = (newSize.Width - size.Width) / 2;
			int y = newSize.Height - size.Height;
			using Graphics graphics = Graphics.FromImage(bitmap);
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
			Rectangle destRect = new Rectangle(x, y, size.Width, size.Height);
			graphics.DrawImage(source, destRect, 0, 0, source.Width, source.Height, GraphicsUnit.Pixel);
			graphics.Flush();
			return bitmap;
		}
	}

	public static void RewireToSourceOutputChannel(CSInputChannel sourceInputChannel, CSInputChannel destinationInputChannel)
	{
		if (sourceInputChannel.OutputChannelBlock != null)
		{
			CSBlock outputChannelBlock = sourceInputChannel.OutputChannelBlock;
			if (outputChannelBlock.IsOutputBlockProvider && !(sourceInputChannel.Parent is ICSVHandle))
			{
				destinationInputChannel.SetLink(sourceInputChannel.OutputChannelBlock, isMaintainingUndo: false);
			}
			else
			{
				destinationInputChannel.SetLink(outputChannelBlock, sourceInputChannel.OutputChannelIndex, isMaintainingUndo: false);
			}
		}
	}

	public static void SetAllUnsetComponents(DisplayComponents cd)
	{
		foreach (CSObject item in CSApplicationHelper.Application.Document.Graph.AllObjects())
		{
			if (item is CSPatchWireStart { DisplayComponent: DisplayComponents.None } cSPatchWireStart)
			{
				cSPatchWireStart.DisplayComponent = cd;
			}
		}
	}

	public static void SetUIModified(bool modifed)
	{
		CSApplicationHelper.Application.IsUIModified = modifed;
	}

	public static void SuperviewClosing()
	{
		CSApplicationHelper.Application.RaiseUnloadSuperviewEvent();
	}

	public static void ToggleWireframeMode()
	{
		CSApplicationHelper.Application.Conduit._isWireframeModeEnabled = !CSApplicationHelper.Application.Conduit._isWireframeModeEnabled;
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		PrintLine(CSApplicationHelper.Application.Conduit._isWireframeModeEnabled ? "Wireframe Mode On" : "Wireframe Mode Off");
		foreach (CSBlock item in CSApplicationHelper.Application.Document.Graph.ActiveOutputObjects())
		{
			item.Change();
		}
		Redraw();
	}

	private static void CancelPendingParallelCSBlockTasks()
	{
		CSApplicationHelper.Application.Document.Graph.AllObjects().OfType<CSBlock>().ToList()
			.ForEach(delegate(CSBlock csBlock)
			{
				csBlock.RequestCancellationOfParallelTasks();
			});
	}

	private static bool CompareBrep(OnBrep brep1, OnBrep brep2)
	{
		if (brep1.m_C3.Count() != brep2.m_C3.Count())
		{
			return false;
		}
		if (brep1.m_F.Count() != brep2.m_F.Count())
		{
			return false;
		}
		for (int i = 0; i < brep1.m_C3.Count(); i++)
		{
			OnCurve obj = brep1.m_C3[i];
			OnCurve val = brep2.m_C3[i];
			if (RhUtil.Rhino_dup_cmp_curve((IOnCurve)(object)obj, (IOnCurve)(object)val) != 0)
			{
				return false;
			}
		}
		for (int j = 0; j < brep1.m_F.Count(); j++)
		{
			OnBrepFace obj2 = brep1.m_F[j];
			OnSurface val2 = (OnSurface)(object)brep2.m_F[j];
			if (RhUtil.Rhino_dup_cmp_surface((IOnSurface)(object)obj2, (IOnSurface)(object)val2) != 0)
			{
				return false;
			}
		}
		return true;
	}

	private static bool CompareMesh(OnMesh mesh1, OnMesh mesh2)
	{
		if (((ArrayOn3fPoint)mesh1.m_V).Count() != ((ArrayOn3fPoint)mesh2.m_V).Count())
		{
			return false;
		}
		for (int i = 0; i < ((ArrayOn3fPoint)mesh1.m_V).Count(); i++)
		{
			if (((ArrayOn3fPoint)mesh1.m_V)[i].DistanceTo((IOn3fPoint)(object)((ArrayOn3fPoint)mesh2.m_V)[i]) > 0.01)
			{
				return false;
			}
		}
		return true;
	}

	[DllImport("gdi32.dll")]
	private static extern bool DeleteObject(IntPtr hObject);

	private static List<CSTreeObject> DuplicateGraphSegment(List<CSObject> source, ImportGroup importGroup = null, CSParaGraph temporaryGraph = null)
	{
		List<CSTreeObject> duplicatedPatchWires = new List<CSTreeObject>();
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("DuplicateGraphSegment");
		CSBlockList currentBlocks = CSApplicationHelper.Application.BlockList;
		Dictionary<CSBlock, CSBlock> duplicationTransaction = new Dictionary<CSBlock, CSBlock>();
		try
		{
			source.ForEach(delegate(CSObject sourceGraphObject)
			{
				CSBlockDescription cSBlockDescription = currentBlocks.FindBlockFrom(sourceGraphObject);
				if (cSBlockDescription != null)
				{
					CSObject cSObject = ((temporaryGraph == null) ? cSBlockDescription.GetNew(sourceGraphObject.Identifier) : cSBlockDescription.GetNewTemp(sourceGraphObject.Identifier));
					if (importGroup != null)
					{
						if (importGroup.FirstGraphIdentifier == Guid.Empty && source.ToList().FirstOrDefault() == sourceGraphObject)
						{
							importGroup.FirstGraphIdentifier = cSObject.InstanceGuid;
						}
						if (importGroup.LastGraphIdentifier == Guid.Empty && source.ToList().LastOrDefault() == sourceGraphObject)
						{
							importGroup.LastGraphIdentifier = cSObject.InstanceGuid;
						}
					}
					temporaryGraph?.Add(cSObject);
					if (cSObject is CSSuperViewGroup cSSuperViewGroup)
					{
						duplicatedPatchWires.Add(cSSuperViewGroup);
						cSSuperViewGroup.IsFiltered = sourceGraphObject.IsFiltered;
					}
					if (cSObject is CSPatchWireStart cSPatchWireStart)
					{
						duplicatedPatchWires.Add(cSPatchWireStart);
						if (sourceGraphObject is CSPatchWireStart cSPatchWireStart2)
						{
							cSPatchWireStart.PatchwireIdentifier = cSPatchWireStart2.PatchwireIdentifier;
							cSPatchWireStart.PatchwireVersionNumber = cSPatchWireStart2.PatchwireVersionNumber;
							cSPatchWireStart.DisplayComponent = cSPatchWireStart2.DisplayComponent;
							cSPatchWireStart.DisplayVisible = cSPatchWireStart2.DisplayVisible;
							cSPatchWireStart.InDisplay = cSPatchWireStart2.InDisplay;
							cSPatchWireStart.StudioVisible = cSPatchWireStart2.StudioVisible;
							cSPatchWireStart.StudioVisibleAdvanced = cSPatchWireStart2.StudioVisibleAdvanced;
							cSPatchWireStart.StudioVisibleDesignOption = cSPatchWireStart2.StudioVisibleDesignOption;
							cSPatchWireStart.UIVisible = cSPatchWireStart2.UIVisible;
							cSPatchWireStart.ForceVisible = cSPatchWireStart2.ForceVisible;
							cSPatchWireStart.ForceHide = cSPatchWireStart2.ForceHide;
							importGroup?.BlockIdentifiers.Add(cSPatchWireStart.InstanceGuid);
						}
					}
					(cSObject as CSValueList)?.CloneList(sourceGraphObject as CSValueList);
					if (cSObject != null)
					{
						cSObject.SetUndo(sourceGraphObject);
						cSObject.Parent = null;
						if (cSObject is CSPatchWireStart)
						{
							int xPosLocal = (cSObject.YPosLocal = 0);
							cSObject.XPosLocal = xPosLocal;
						}
					}
					(cSObject as CSValueFontList)?.CloneList(sourceGraphObject as CSValueFontList);
					if (cSObject != null)
					{
						cSObject.SetUndo(sourceGraphObject);
						cSObject.Parent = null;
						if (cSObject is CSPatchWireStart)
						{
							int xPosLocal = (cSObject.YPosLocal = 0);
							cSObject.XPosLocal = xPosLocal;
						}
					}
					(cSObject as CSValueHeadStyleList)?.CloneDictionary(sourceGraphObject as CSValueHeadStyleList);
					if (cSObject != null)
					{
						cSObject.SetUndo(sourceGraphObject);
						cSObject.Parent = null;
						if (cSObject is CSPatchWireStart)
						{
							int xPosLocal = (cSObject.YPosLocal = 0);
							cSObject.XPosLocal = xPosLocal;
						}
					}
					CSBlock cSBlock = sourceGraphObject as CSBlock;
					CSBlock duplicateBlock = cSObject as CSBlock;
					if (cSBlock != null && duplicateBlock != null)
					{
						duplicationTransaction.Add(cSBlock, duplicateBlock);
						duplicateBlock.StudioVisible = cSBlock.StudioVisible;
						duplicateBlock.StudioVisibleAdvanced = cSBlock.StudioVisibleAdvanced;
						duplicateBlock.StudioVisibleDesignOption = cSBlock.StudioVisibleDesignOption;
						duplicateBlock.UIVisible = cSBlock.UIVisible;
						duplicateBlock.DisplayVisible = cSBlock.DisplayVisible;
						duplicateBlock.InDisplay = cSBlock.InDisplay;
						duplicateBlock.ForceVisible = cSBlock.ForceVisible;
						duplicateBlock.ForceHide = cSBlock.ForceHide;
						if (!(duplicateBlock is CSBlockExpandableInput))
						{
							duplicateBlock.InputChannels.ForEach(delegate(CSInputChannel inputObject)
							{
								duplicateBlock.ResetInputChannel(duplicateBlock.InputChannels.IndexOf(inputObject));
							});
						}
						CS.MaterialGroup.MaterialGroupHelper.DuplicateMaterialGroupAssignments(cSBlock, duplicateBlock);
						if (importGroup != null)
						{
							importGroup.BlockIdentifiers.Add(duplicateBlock.InstanceGuid);
							CSValueBase cSValueBase = duplicateBlock as CSValueBase;
							CSValueBase sourceValueBase = cSBlock as CSValueBase;
							if (cSValueBase != null && sourceValueBase != null && sourceValueBase.Categories.Any())
							{
								CSUICategory cSUICategory = (temporaryGraph ?? CSApplicationHelper.Application.Document.Graph).CategoryTable.Categories.FirstOrDefault((CSUICategory category) => category.ImportGroupIdentifier == importGroup.Identifier && sourceValueBase.Categories.FirstOrDefault().CategoryName == category.CategoryName);
								cSValueBase.Categories.Add(cSUICategory ?? sourceValueBase.Categories.FirstOrDefault());
							}
						}
					}
				}
			});
			duplicationTransaction.Keys.ToList().ForEach(delegate(CSBlock _)
			{
				_.Generate();
			});
			duplicationTransaction.Keys.ToList().ForEach(delegate(CSBlock sourceBlock)
			{
				(from inputChannel in sourceBlock.InputChannels.ToList()
					where inputChannel.OutputChannelBlock != null
					select inputChannel).ToList().ForEach(delegate(CSInputChannel sourceBlockInputChannel)
				{
					duplicationTransaction.TryGetValue(sourceBlock, out var value);
					if (value != null)
					{
						CSInputChannel inputChannel = value.GetInputChannel(sourceBlock.InputChannels.IndexOf(sourceBlockInputChannel));
						RewireToSourceOutputChannel(sourceBlockInputChannel, inputChannel);
						if (inputChannel.OutputChannelBlock != null && duplicationTransaction.Keys.Contains(inputChannel.OutputChannelBlock))
						{
							CSBlock value2 = null;
							duplicationTransaction.TryGetValue(inputChannel.OutputChannelBlock, out value2);
							if (value2 != null)
							{
								if (value2.IsOutputBlockProvider && !(sourceBlockInputChannel.Parent is ICSVHandle))
								{
									inputChannel.SetLink(value2, isMaintainingUndo: false);
								}
								else
								{
									inputChannel.SetLink(value2, inputChannel.OutputChannelIndex, isMaintainingUndo: false);
								}
							}
						}
					}
				});
			});
		}
		finally
		{
			CSApplicationHelper.Application.Document.Graph.ClearIndexesOfGraphItems();
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("DuplicateGraphSegment");
			CSApplicationHelper.Application.Document.Graph.Generate();
			Redraw();
		}
		return duplicatedPatchWires;
	}

	private static List<OnGeometry> GetAllDisplayObjects(bool getSolids, bool getCurves)
	{
		List<OnGeometry> list = new List<OnGeometry>();
		foreach (CSBlock item in CSApplicationHelper.Application.Document.Graph.DisplayObjects())
		{
			if (item == null || item.Deleted)
			{
				continue;
			}
			int outputChannelCount = item.OutputChannelCount;
			for (int i = 0; i < outputChannelCount; i++)
			{
				CSData outputChannelData = item.GetOutputChannelData(i);
				if (outputChannelData == null)
				{
					continue;
				}
				bool flag = false;
				if (!getSolids)
				{
					continue;
				}
				Brep brep = outputChannelData.GetBrep();
				if (brep != null)
				{
					list.Add((OnGeometry)(object)brep.Convert());
					flag = true;
				}
				if (!flag)
				{
					Mesh mesh = outputChannelData.GetMesh();
					if (mesh != null)
					{
						list.Add((OnGeometry)(object)mesh.Convert());
						flag = true;
					}
				}
				if (getCurves && !flag)
				{
					NurbsCurve nurbsCurve = outputChannelData.GetNurbsCurve();
					if (nurbsCurve != null)
					{
						list.Add((OnGeometry)(object)nurbsCurve.Convert());
					}
				}
			}
		}
		return list;
	}

	private static CSObject GetCSObjectByInstanceGuid(Guid blockInstanceGuid)
	{
		return CSApplicationHelper.Application.Document.Graph.AllObjects().FirstOrDefault((CSObject graphObject) => graphObject.InstanceGuid == blockInstanceGuid);
	}

	private static CSPatchWireStart GetPatchwireForUIControl(Guid blockInstanceGuid)
	{
		if (GetCSObjectByInstanceGuid(blockInstanceGuid) is CSBlock block)
		{
			return GetPatchwireStartUsingCSBlock(block);
		}
		return null;
	}

	private static bool OnObjectEquals(OnGeometry geo, OnGeometry geo2)
	{
		OnCurve val = OnCurve.Cast((OnObject)(object)geo);
		OnCurve val2 = OnCurve.Cast((OnObject)(object)geo2);
		if (val != null && val2 != null)
		{
			return RhUtil.Rhino_dup_cmp_curve((IOnCurve)(object)val, (IOnCurve)(object)val2) == 0;
		}
		OnBrep val3 = OnBrep.Cast((OnObject)(object)geo);
		OnBrep val4 = OnBrep.Cast((OnObject)(object)geo2);
		if (val3 != null && val4 != null)
		{
			return CompareBrep(val3, val4);
		}
		OnMesh val5 = OnMesh.Cast((OnObject)(object)geo);
		OnMesh val6 = OnMesh.Cast((OnObject)(object)geo2);
		if (val5 != null && val6 != null)
		{
			return CompareMesh(val5, val6);
		}
		return false;
	}

	public static bool FindLastBlockOutputValue(string identifier, out CSOutputChannel outValue)
	{
		outValue = null;
		if (!FindLastBlockByIdentifier(identifier, out var result))
		{
			return false;
		}
		if (!(result is CSValue cSValue))
		{
			return false;
		}
		outValue = cSValue.OutValue;
		return true;
	}

	public static bool FindLastOutputByIdentifier(string identifier, out CSOutputChannel result)
	{
		result = null;
		if (string.IsNullOrEmpty(identifier))
		{
			return false;
		}
		for (int num = CSApplicationHelper.Application.Document.Graph.GraphList.Count - 1; num >= 0; num--)
		{
			if (!CSApplicationHelper.Application.Document.Graph.GraphList[num].Deleted && CSApplicationHelper.Application.Document.Graph.GraphList[num] is CSBlock cSBlock)
			{
				for (int i = 0; i < cSBlock.OutputChannelCount; i++)
				{
					CSOutputChannel outputChannel = cSBlock.GetOutputChannel(i);
					if (identifier.Equals(outputChannel.Identifier, StringComparison.OrdinalIgnoreCase))
					{
						result = outputChannel;
						return true;
					}
				}
			}
		}
		return false;
	}

	public static bool FindLastOutputRingRailSize(ref CSOutputChannel outValue)
	{
		CSOutputChannel cSOutputChannel = FindAllValidOutputRingRailSizes().LastOrDefault();
		if (cSOutputChannel == null)
		{
			return false;
		}
		outValue = cSOutputChannel;
		return true;
	}

	public static List<CSOutputChannel> FindAllValidOutputRingRailSizes()
	{
		List<CSOutputChannel> outputValues = new List<CSOutputChannel>();
		List<CSValueRingSize> list = CSApplicationHelper.Application.Document.Graph.CSBlocks.Where((CSBlock graphItem) => !graphItem.Deleted).OfType<CSValueRingSize>().ToList();
		if (list.Any())
		{
			list.ForEach(delegate(CSValueRingSize _)
			{
				outputValues.Add(_.GetOutputChannel(_.OutputChannels.IndexOf(_.OutValue)));
			});
		}
		return outputValues;
	}

	public static int CountValueRingSizeBlocks()
	{
		return CSApplicationHelper.Application.Document.Graph.GraphList.Where((CSObject graphItem) => !graphItem.Deleted).OfType<CSValueRingSize>().Count();
	}

	public static bool FindLastBlockByIdentifier(string identifier, out CSBlock result)
	{
		result = null;
		if (string.IsNullOrEmpty(identifier))
		{
			return false;
		}
		for (int num = CSApplicationHelper.Application.Document.Graph.GraphList.Count - 1; num >= 0; num--)
		{
			if (!CSApplicationHelper.Application.Document.Graph.GraphList[num].Deleted && CSApplicationHelper.Application.Document.Graph.GraphList[num] is CSBlock cSBlock && identifier.Equals(cSBlock.IdentifierOriginal, StringComparison.OrdinalIgnoreCase))
			{
				result = cSBlock;
				return true;
			}
		}
		return false;
	}

	public static int GetEnd(int location, CSParaGraph temporaryGraph = null)
	{
		CSParaGraph cSParaGraph = temporaryGraph ?? CSApplicationHelper.Application.Document.Graph;
		if (cSParaGraph.GraphList.Count == 0)
		{
			return -1;
		}
		if (location < 0)
		{
			location = 0;
		}
		for (int i = location + 1; i < cSParaGraph.GraphList.Count; i++)
		{
			if (cSParaGraph.GraphList[i] is CSPatchWireStart || cSParaGraph.GraphList[i] is CSSuperViewGroup)
			{
				return i - 1;
			}
		}
		return cSParaGraph.GraphList.Count - 1;
	}

	public static int GetIndex(CSObject obj, CSParaGraph temporaryGraph = null)
	{
		CSParaGraph cSParaGraph = temporaryGraph ?? CSApplicationHelper.Application.Document.Graph;
		if (obj.TableIndex >= 0)
		{
			return obj.TableIndex;
		}
		return cSParaGraph.GraphList.IndexOf(obj);
	}

	public static CSObject[] GetRange(CSObject obj, CSParaGraph temporaryGraph = null, List<Type> ignoreBlockTypesList = null)
	{
		CSParaGraph cSParaGraph = temporaryGraph ?? CSApplicationHelper.Application.Document.Graph;
		List<CSObject> list = new List<CSObject>();
		int index = GetIndex(obj, cSParaGraph);
		if (index < 0)
		{
			return null;
		}
		int start = GetStart(index, cSParaGraph);
		int end = GetEnd(index, cSParaGraph);
		if (start < 0 || end < 0)
		{
			return null;
		}
		for (int i = start; i <= end; i++)
		{
			if (ignoreBlockTypesList == null)
			{
				list.Add(cSParaGraph.GraphList[i]);
			}
			else if (!ignoreBlockTypesList.Contains(cSParaGraph.GraphList[i].GetType()))
			{
				list.Add(cSParaGraph.GraphList[i]);
			}
		}
		return list.ToArray();
	}

	public static int GetStart(int location, CSParaGraph temporaryGraph = null)
	{
		CSParaGraph cSParaGraph = temporaryGraph ?? CSApplicationHelper.Application.Document.Graph;
		if (cSParaGraph.GraphList.Count == 0)
		{
			return -1;
		}
		if (location < 0)
		{
			location = 0;
		}
		if (location >= cSParaGraph.GraphList.Count)
		{
			location = cSParaGraph.GraphList.Count - 1;
		}
		for (int num = location; num >= 0; num--)
		{
			if (cSParaGraph.GraphList[num] is CSPatchWireStart)
			{
				return num;
			}
		}
		return -1;
	}

	public static CSPatchWireStart GetStartObject(CSObject obj)
	{
		int start = GetStart(GetIndex(obj));
		if (start >= 0 && start < CSApplicationHelper.Application.Document.Graph.GraphList.Count)
		{
			return CSApplicationHelper.Application.Document.Graph.GraphList[start] as CSPatchWireStart;
		}
		return null;
	}

	public static void RemoveMouseDown()
	{
		CSApplicationHelper.Application.Document.Graph.MouseDown = false;
		for (int i = 0; i < CSApplicationHelper.Application.Document.Graph.GraphList.Count; i++)
		{
			CSApplicationHelper.Application.Document.Graph.GraphList[i].MouseDownObject = false;
		}
	}

	public static void ToggleMouseDown(CSObject mouseDownObject)
	{
		CSApplicationHelper.Application.Document.Graph.MouseDown = true;
		int index = GetIndex(mouseDownObject);
		if (index < 0)
		{
			return;
		}
		int num = 0;
		if (mouseDownObject is CSPatchWireStart)
		{
			mouseDownObject.MouseDownObject = true;
			return;
		}
		for (int num2 = index; num2 >= 0; num2--)
		{
			CSApplicationHelper.Application.Document.Graph.GraphList[num2].MouseDownObject = true;
			num++;
			if (CSApplicationHelper.Application.Document.Graph.GraphList[num2] is CSPatchWireStart)
			{
				break;
			}
		}
		for (int i = index + 1; i < CSApplicationHelper.Application.Document.Graph.GraphList.Count && !(CSApplicationHelper.Application.Document.Graph.GraphList[i] is CSPatchWireStart); i++)
		{
			num++;
			CSApplicationHelper.Application.Document.Graph.GraphList[i].MouseDownObject = true;
		}
	}

	public static void AddToDisplayAndGenerate(CSBlock obj, MaterialClassification whichMaterialClassification = MaterialClassification.UseOriginal)
	{
		if (obj != null)
		{
			obj.DisplayVisible = true;
			obj.InDisplay = true;
			obj.UIGenerate = true;
			obj.UIVisible = true;
			if (obj.OutputDisplayMaterialClassification != whichMaterialClassification)
			{
				CS.MaterialGroup.MaterialGroupHelper.RemoveItemFromMaterialGroupByLayer(obj);
				obj.OutputDisplayMaterialClassification = whichMaterialClassification;
				CS.MaterialGroup.MaterialGroupHelper.AddBlockToDefaultMaterialGroups(obj, redraw: false);
			}
		}
	}

	public static void CenterDesign()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		if (RhUtil.RhinoApp().ActiveView() != null)
		{
			RhUtil.RhinoApp().ActiveView().ActiveViewport()
				.SetTargetAndCameraLocation(OnUtil.On_origin, (IOn3dPoint)new On3dPoint(45.0, -45.0, 45.0));
			Redraw();
		}
	}

	public static CSBlockGroupEnd CreateBlockGroupEnd()
	{
		return new CSBlockGroupEnd();
	}

	public static CSBlockGroupStart CreateBlockGroupStart(string identifier, Guid groupIdentifier)
	{
		return new CSBlockGroupStart(identifier)
		{
			BlockGroupIdentifier = groupIdentifier
		};
	}

	public static CSBlockGroupStart CreateBlockGroupStart(string identifier, bool isCollapsed, Guid groupIdentifier)
	{
		return new CSBlockGroupStart(identifier)
		{
			BlockGroupIdentifier = groupIdentifier,
			UICollapsedGroup = isCollapsed
		};
	}

	public static CSPatchWireStart CreatePatchWireStart(string identifier, Guid patchwireIdentifier, int patchwireVersionNumber, DisplayComponents currentComponent, bool isDisplayVisible = true)
	{
		return new CSPatchWireStart(identifier)
		{
			DisplayComponent = currentComponent,
			PatchwireIdentifier = patchwireIdentifier,
			PatchwireVersionNumber = patchwireVersionNumber,
			DisplayVisible = isDisplayVisible
		};
	}

	public static CSPatchWireStart CreatePatchWireStart(string identifier, Guid patchwireIdentifier, int patchwireVersionNumber, bool isDisplayVisible = true)
	{
		return CreatePatchWireStart(identifier, patchwireIdentifier, patchwireVersionNumber, CSApplicationHelper.Application.PatchWireComponentDisplay, isDisplayVisible);
	}

	public static bool RhinoDocHasImportableObjects()
	{
		return ((IEnumerable<RhinoObject>)RhinoDoc.ActiveDoc.Objects).Any();
	}

	public static CSPatchWireStart CreateSecondStartBlock(string identifier, Guid patchwireIdentifier, int patchwireVersionNumber)
	{
		CSPatchWireStart lastPatchWireStart = GetLastPatchWireStart();
		CSPatchWireStart cSPatchWireStart = CreatePatchWireStart(identifier, patchwireIdentifier, patchwireVersionNumber);
		if (lastPatchWireStart != null)
		{
			string text = lastPatchWireStart.UserData.FindValue("UI", "IconImagePath");
			if (text != null)
			{
				cSPatchWireStart.UserData.AddKey("UI", "IconImagePath", text);
			}
			string text2 = lastPatchWireStart.UserData.FindValue("UI", "Description");
			if (text2 != null)
			{
				text2 += " B";
				cSPatchWireStart.UserData.AddKey("UI", "Description", text2);
			}
			string text3 = lastPatchWireStart.UserData.FindValue("UI", "ControlColorBrush");
			if (text3 != null)
			{
				cSPatchWireStart.UserData.AddKey("UI", "ControlColorBrush", text3);
			}
		}
		return cSPatchWireStart;
	}

	public static CSPatchWireStart CreateSecondStartBlock(string identifier, Guid patchwireIdentifier, int patchwireVersionNumber, string pathToImage, string styleSheetFolder)
	{
		CSPatchWireStart lastPatchWireStart = GetLastPatchWireStart();
		CSPatchWireStart cSPatchWireStart = CreatePatchWireStart(identifier, patchwireIdentifier, patchwireVersionNumber);
		if (lastPatchWireStart != null)
		{
			string text = lastPatchWireStart.UserData.FindValue("UI", "Description");
			if (text != null)
			{
				text += " B";
				cSPatchWireStart.UserData.AddKey("UI", "Description", text);
			}
			string text2 = lastPatchWireStart.UserData.FindValue("UI", "ControlColorBrush");
			if (text2 != null)
			{
				cSPatchWireStart.UserData.AddKey("UI", "ControlColorBrush", text2);
			}
		}
		if (pathToImage != null)
		{
			cSPatchWireStart.UserData.AddKey("UI", "IconImagePath", pathToImage);
		}
		if (styleSheetFolder != null)
		{
			cSPatchWireStart.UserData.AddKey("StyleSheet", "Folder", styleSheetFolder);
		}
		return cSPatchWireStart;
	}

	public static void EnableDrawing(bool which)
	{
		if (which)
		{
			if (_enableDrawingCount > 0)
			{
				_enableDrawingCount--;
			}
			if (_enableDrawingCount == 0)
			{
				MRhinoView.EnableDrawing(true);
			}
		}
		else
		{
			MRhinoView.EnableDrawing(false);
			_enableDrawingCount++;
		}
	}

	public static void ForceGraphToRedraw(List<CSBlock> targetCSBlocks = null, CSParaGraph temporaryGraph = null)
	{
		CSParaGraph cSParaGraph = temporaryGraph ?? CSApplicationHelper.Application.Document.Graph;
		cSParaGraph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.Pause(isPausing: true);
		if (targetCSBlocks == null)
		{
			targetCSBlocks = (from graphObject in cSParaGraph.AllObjects().OfType<CSBlock>()
				where !graphObject.Deleted
				select graphObject).ToList();
		}
		targetCSBlocks.AsParallel().AsOrdered().WithDegreeOfParallelism(CSApplicationHelper.MaxNumberOfBlockUpdateAvailableCores)
			.ForAll(delegate(CSBlock csObject)
			{
				csObject.Change();
			});
		cSParaGraph.Generate();
		CSApplicationHelper.Application.Document.UndoTable.Pause(isPausing: false);
		Redraw();
	}

	public static void ForceGraphToRedrawCAM()
	{
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.Pause(isPausing: true);
		((IEnumerable<CSBlock>)(from graphObject in CSApplicationHelper.Application.Document.Graph.AllObjects().OfType<CSValue>()
			where !graphObject.Deleted && graphObject.HasCAM()
			select graphObject)).AsParallel().AsOrdered().WithDegreeOfParallelism(CSApplicationHelper.MaxNumberOfBlockUpdateAvailableCores)
			.ForAll(delegate(CSBlock csObject)
			{
				csObject.Change();
			});
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSApplicationHelper.Application.Document.UndoTable.Pause(isPausing: false);
		Redraw();
	}

	public static void Generate(CSBlock obj)
	{
		Generate(obj, printTiming: true);
	}

	public static void Generate(CSBlock obj, bool printTiming)
	{
		try
		{
			obj.Generate(printTiming);
		}
		catch (Exception ex)
		{
			Print(ex.Message);
			Print(ex.StackTrace);
		}
	}

	public static CSPatchWireStart GetLastPatchWireStart()
	{
		for (int num = CSApplicationHelper.Application.Document.Graph.GraphList.Count - 1; num >= 0; num--)
		{
			if (CSApplicationHelper.Application.Document.Graph.GraphList[num] is CSPatchWireStart { Deleted: false } cSPatchWireStart)
			{
				return cSPatchWireStart;
			}
		}
		return null;
	}

	public static bool InvokePatchWire(MemberInfo methodToCall, bool loadDefault = true, object creationObject = null)
	{
		CSPatchWireStart cSPatchWireStart = null;
		bool flag = false;
		try
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("InvokePatchWire");
			MethodInfo methodInfo = methodToCall as MethodInfo;
			if (methodInfo != null)
			{
				object[] customAttributes = methodToCall.GetCustomAttributes(typeof(CSPatchWireAttribute), inherit: false);
				int count = CSApplicationHelper.Application.Document.Graph.GraphList.Count;
				if (customAttributes.Length == 1 && customAttributes[0] is CSPatchWireAttribute { DoNoAdd: false } cSPatchWireAttribute)
				{
					cSPatchWireStart = CreatePatchWireStart(cSPatchWireAttribute.Name, cSPatchWireAttribute.Identifier, cSPatchWireAttribute.VersionNumber, cSPatchWireAttribute.IsDisplayVisible);
					cSPatchWireStart.UserData.AddKey("UI", "IconImagePath", cSPatchWireAttribute.PathToImage);
					cSPatchWireStart.UserData.AddKey("UI", "Description", cSPatchWireAttribute.Description);
					cSPatchWireStart.UserData.AddKey("UI", "ControlColorBrush", cSPatchWireAttribute.Grouping);
					if (cSPatchWireAttribute.StyleSheetFolder != null)
					{
						cSPatchWireStart.UserData.AddKey("StyleSheet", "Folder", cSPatchWireAttribute.StyleSheetFolder);
					}
				}
				object obj = methodInfo.Invoke(creationObject, null);
				if (obj.GetType() == typeof(CSPatchWireFunctionResult))
				{
					if ((CSPatchWireFunctionResult)obj != CSPatchWireFunctionResult.Success)
					{
						while (count < CSApplicationHelper.Application.Document.Graph.GraphList.Count)
						{
							CSApplicationHelper.Application.Document.Graph.GraphList.Remove(CSApplicationHelper.Application.Document.Graph.GraphList[count]);
						}
					}
					else
					{
						flag = true;
						if (loadDefault && cSPatchWireStart != null && !cSPatchWireStart.ReadDefaultStyleSheet())
						{
							PrintLine($"Unable to apply default StyleSheet for {cSPatchWireStart.Identifier}:  does not match current PatchWire version.n");
						}
						cSPatchWireStart?.LoadSettings();
					}
				}
			}
		}
		catch (Exception ex)
		{
			RhUtil.RhinoApp().Print(ex.Message + "\n");
			RhUtil.RhinoApp().Print(ex.StackTrace + "\n");
			if (ex.InnerException != null)
			{
				RhUtil.RhinoApp().Print(ex.InnerException.Message + "\n");
				RhUtil.RhinoApp().Print(ex.InnerException.StackTrace + "\n");
			}
		}
		finally
		{
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("InvokePatchWire");
		}
		if (flag)
		{
			CSApplicationHelper.Application.Document.Graph.Generate();
			Redraw();
		}
		return flag;
	}

	public static void Redraw()
	{
		if (!IsRedrawDisabled && RhUtil.RhinoApp().ActiveDoc() != null)
		{
			RhUtil.RhinoApp().ActiveDoc().Regen();
		}
	}

	public static void Turntable(bool isTurnTableEnabled, int speed = 1)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		if (!isTurnTableEnabled)
		{
			CSApplicationHelper.Application.CurrentCameraViewName = CameraView.Perspective;
			Point3d cameraLocation = new Point3d(0.0, 0.0, 0.0);
			Point3d targetLocation = Point3d.Origin;
			Vector3d cameraUp = Vector3d.ZAxis;
			RhinoViewport viewport = RhinoDoc.ActiveDoc.Views.ActiveView.MainViewport;
			RhinoView val = RhinoDoc.ActiveDoc.Views.GetStandardRhinoViews().FirstOrDefault((RhinoView view) => view.MainViewport.Name.ToLower() == "perspective");
			if (val != null)
			{
				viewport = val.MainViewport;
			}
			viewport.ChangeToPerspectiveProjection(true, 50.0);
			viewport.SetConstructionPlane(Plane.WorldXY);
			_timer = new Timer
			{
				Interval = 60
			};
			Point3d cameraLocation2 = viewport.CameraLocation;
			double currentX = ((Point3d)(ref cameraLocation2)).X;
			cameraLocation2 = viewport.CameraLocation;
			double currentY = ((Point3d)(ref cameraLocation2)).Y;
			double theta = Math.Atan2(currentY, currentX);
			_timer.Tick += delegate
			{
				//IL_0095: Unknown result type (might be due to invalid IL or missing references)
				//IL_009a: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00de: Unknown result type (might be due to invalid IL or missing references)
				//IL_0103: Unknown result type (might be due to invalid IL or missing references)
				if (CSMouseCallback.IsRightMouseButtonDown && CSMouseCallback.IsLeftMouseButtonDown)
				{
					_timer.Stop();
					_timer.Dispose();
					CSApplicationHelper.Application.Conduit.IsTurnTableEnabled = false;
				}
				theta -= 0.005;
				double num = Math.Sqrt(currentX * currentX + currentY * currentY);
				currentX = num * Math.Sin(theta);
				currentY = num * Math.Cos(theta);
				Point3d cameraLocation3 = viewport.CameraLocation;
				double z = ((Point3d)(ref cameraLocation3)).Z;
				cameraLocation = new Point3d(currentX, currentY, z);
				targetLocation = Point3d.Origin;
				cameraUp = Vector3d.ZAxis;
				viewport.SetCameraLocations(targetLocation, cameraLocation);
				viewport.Camera35mmLensLength = 50.0;
				viewport.CameraUp = cameraUp;
				Redraw();
			};
			_timer.Start();
		}
		else
		{
			_timer.Stop();
		}
	}

	public static ConcurrentBag<RhinoRenderObject> AddExportObjectsToRhino()
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		ConcurrentBag<RhinoRenderObject> exportObjectsToRhino = new ConcurrentBag<RhinoRenderObject>();
		CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Where((CSMaterials.MaterialGroup materialGroup) => materialGroup.IsGemMaterialGroup).ToList().ForEach(delegate(CSMaterials.MaterialGroup designerMaterialGroup)
		{
			RhinoDoc.ActiveDoc.Groups.Add(designerMaterialGroup.Name);
		});
		Transform ringXform = CSApplicationHelper.Application.Conduit.GetDesignTransform();
		CSApplicationHelper.Application.Document.Graph.DisplayObjects().ToList().ForEach(delegate(CSBlock csBlock)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			csBlock.AddToDocumentExport(ref exportObjectsToRhino, ringXform);
		});
		Transform handTransform = CSDisplayConduit.GetHandTransform();
		if (CSApplicationHelper.Application.Conduit._handObjects != null)
		{
			CSApplicationHelper.Application.Conduit._handObjects.AddToDocumentExport(ref exportObjectsToRhino, handTransform);
			if (CSApplicationHelper.Application.Conduit._fingernailObjects != null)
			{
				CSApplicationHelper.Application.Conduit._fingernailObjects.AddToDocumentExport(ref exportObjectsToRhino, handTransform);
			}
			if (CSApplicationHelper.Application.Conduit._shirtObjects != null)
			{
				CSApplicationHelper.Application.Conduit._shirtObjects.AddToDocumentRender(ref exportObjectsToRhino, handTransform);
			}
		}
		if (CSApplicationHelper.Application.Conduit._necklaceChainObjects != null)
		{
			Transform chainPointTransform = CSDisplayConduit.GetChainPointTransform();
			CSApplicationHelper.Application.Conduit._necklaceChainObjects.AddToDocumentExport(ref exportObjectsToRhino, ringXform * chainPointTransform);
		}
		return exportObjectsToRhino;
	}

	public static ConcurrentBag<RhinoRenderObject> AddExportObjectsToRhino(Transform transform)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		ConcurrentBag<RhinoRenderObject> exportObjectsToRhino = new ConcurrentBag<RhinoRenderObject>();
		CSApplicationHelper.Application.Document.Graph.DisplayObjects().ToList().ForEach(delegate(CSBlock csBlock)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			csBlock.AddToDocumentExport(ref exportObjectsToRhino, transform);
		});
		return exportObjectsToRhino;
	}

	public static void AddModelToRhino(bool isRenderModelExport = false)
	{
		SetupExportLayers();
		AddGeometryToRhino(AddExportObjectsToRhino(), isRenderModelExport);
	}

	public static ConcurrentBag<RhinoRenderObject> AddRenderObjectsToRhino()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		ConcurrentBag<RhinoRenderObject> objects = new ConcurrentBag<RhinoRenderObject>();
		Transform ringXform = CSApplicationHelper.Application.Conduit.GetDesignTransform();
		CSApplicationHelper.Application.Document.Graph.DisplayObjects().ToList().ForEach(delegate(CSBlock csBlock)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			csBlock.AddToDocumentRender(ref objects, ringXform);
		});
		Transform handTransform = CSDisplayConduit.GetHandTransform();
		if (CSApplicationHelper.Application.Conduit._handObjects != null)
		{
			CSApplicationHelper.Application.Conduit._handObjects.AddToDocumentRender(ref objects, handTransform);
			if (CSApplicationHelper.Application.Conduit._fingernailObjects != null)
			{
				CSApplicationHelper.Application.Conduit._fingernailObjects.AddToDocumentRender(ref objects, handTransform);
			}
			if (CSApplicationHelper.Application.Conduit._shirtObjects != null)
			{
				CSApplicationHelper.Application.Conduit._shirtObjects.AddToDocumentRender(ref objects, handTransform);
			}
		}
		if (CSApplicationHelper.Application.Conduit._necklaceChainObjects != null)
		{
			Transform chainPointTransform = CSDisplayConduit.GetChainPointTransform();
			CSApplicationHelper.Application.Conduit._necklaceChainObjects.AddToDocumentRender(ref objects, ringXform * chainPointTransform);
		}
		return objects;
	}

	public static ConcurrentBag<RhinoRenderObject> AddRenderObjectsToRhino(Transform transform)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		ConcurrentBag<RhinoRenderObject> objects = new ConcurrentBag<RhinoRenderObject>();
		CSApplicationHelper.Application.Document.Graph.DisplayObjects().AsParallel().AsOrdered()
			.WithDegreeOfParallelism((Environment.ProcessorCount <= 1) ? 1 : (Environment.ProcessorCount - 1))
			.ForAll(delegate(CSBlock csBlock)
			{
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				csBlock.AddToDocumentRender(ref objects, transform);
			});
		return objects;
	}

	public static void AddSelectedBlocksToRhino(IEnumerable<CSBlock> currentCSBlocks)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		SetupExportLayers();
		ConcurrentBag<RhinoRenderObject> rhinoRenderObjects = new ConcurrentBag<RhinoRenderObject>();
		Transform designTransform = CSApplicationHelper.Application.Conduit.GetDesignTransform();
		foreach (CSBlock item in currentCSBlocks.ToList())
		{
			item.AddToDocumentExport(ref rhinoRenderObjects, designTransform);
		}
		AddGeometryToRhino(rhinoRenderObjects);
	}

	public static void DeleteModelFromRhino()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		if (_rhinoRenderObjects == null || _rhinoRenderObjects.IsEmpty)
		{
			return;
		}
		foreach (RhinoRenderObject rhinoRenderObject in _rhinoRenderObjects)
		{
			if (rhinoRenderObject.RhinoObject != null && !rhinoRenderObject.RhinoObject.IsInstanceDefinitionGeometry)
			{
				RhinoDoc.ActiveDoc.Objects.Delete(rhinoRenderObject.RhinoObject, true);
			}
		}
		MRhinoObjectIterator val = new MRhinoObjectIterator((object_state)(-1));
		val.IncludeLights(true);
		val.IncludeGrips(true);
		for (MRhinoObject val2 = val.First(); val2 != null; val2 = val.Next())
		{
			if (!val2.IsInstanceDefinitionGeometry())
			{
				RhUtil.RhinoApp().ActiveDoc().DeleteObject((IRhinoObjRef)new MRhinoObjRef((IRhinoObject)(object)val2), true, true);
			}
		}
		EnableDrawing(which: true);
	}

	public static ConcurrentBag<RhinoRenderObject> GetModelObjectsFromRhino()
	{
		return _rhinoRenderObjects ?? new ConcurrentBag<RhinoRenderObject>();
	}

	private static void AddGeometryToRhino(ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, bool isRenderModelExport = false)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		_rhinoRenderObjects = rhinoRenderObjects;
		RhUtil.RhinoApp().ActiveDoc().UnselectAll();
		foreach (RhinoRenderObject rhinoRenderObject in _rhinoRenderObjects)
		{
			if (rhinoRenderObject.RhinoObject == null)
			{
				continue;
			}
			BoundingBox boundingBox = rhinoRenderObject.RhinoObject.Geometry.GetBoundingBox(true);
			if (((BoundingBox)(ref boundingBox)).IsValid)
			{
				Point3d val = ((BoundingBox)(ref boundingBox)).Max;
				double x = ((Point3d)(ref val)).X;
				val = ((BoundingBox)(ref boundingBox)).Min;
				double num = x - ((Point3d)(ref val)).X;
				val = ((BoundingBox)(ref boundingBox)).Max;
				double y = ((Point3d)(ref val)).Y;
				val = ((BoundingBox)(ref boundingBox)).Min;
				double num2 = y - ((Point3d)(ref val)).Y;
				val = ((BoundingBox)(ref boundingBox)).Max;
				double z = ((Point3d)(ref val)).Z;
				val = ((BoundingBox)(ref boundingBox)).Min;
				double num3 = z - ((Point3d)(ref val)).Z;
				if (num < 3.0 && num2 < 3.0 && num3 < 3.0)
				{
					_layerMap[LayerNumber.Metal01] = _layerIndexMetal03;
					_layerMap[LayerNumber.Metal02] = _layerIndexMetal04;
				}
				else
				{
					_layerMap[LayerNumber.Metal01] = _layerIndexMetal01;
					_layerMap[LayerNumber.Metal02] = _layerIndexMetal02;
				}
				ObjectAttributes val2 = rhinoRenderObject.RhinoObject.Attributes.Duplicate();
				LayerNumber key = CS.MaterialGroup.MaterialGroupHelper.LookupLayerForMaterialClassification(rhinoRenderObject.AssignedMaterialGroup.Classification);
				val2.LayerIndex = (_layerMap.ContainsKey(key) ? _layerMap[key] : _layerMap[LayerNumber.User01]);
				if (isRenderModelExport && (val2.LayerIndex == _layerIndexCutting || val2.LayerIndex == _layerIndexFinger || val2.LayerIndex == _layerIndexCreation))
				{
					RhinoDoc.ActiveDoc.Objects.Delete(rhinoRenderObject.RhinoObject, true);
				}
				else
				{
					RhinoDoc.ActiveDoc.Objects.ModifyAttributes(rhinoRenderObject.RhinoObject, val2, true);
				}
			}
		}
		Redraw();
	}

	private static void SetupExportLayers()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Expected O, but got Unknown
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Expected O, but got Unknown
		Layer val = new Layer
		{
			Color = Color.FromArgb(0, 130, 85),
			Name = "Metal 01"
		};
		Layer layer = new Layer
		{
			Color = Color.FromArgb(68, 190, 100),
			Name = "Metal 02"
		};
		Layer layer2 = new Layer
		{
			Color = Color.FromArgb(132, 250, 114),
			Name = "Metal 03"
		};
		Layer layer3 = new Layer
		{
			Color = Color.FromArgb(197, 253, 189),
			Name = "Metal 04"
		};
		Layer layer4 = new Layer
		{
			Color = Color.FromArgb(0, 60, 160),
			Name = "Gem 01"
		};
		Layer layer5 = new Layer
		{
			Color = Color.FromArgb(65, 120, 205),
			Name = "Gem 02"
		};
		Layer layer6 = new Layer
		{
			Color = Color.FromArgb(130, 180, 250),
			Name = "Gem 03"
		};
		Layer layer7 = new Layer
		{
			Color = Color.FromArgb(173, 63, 63),
			Name = "Finger Sizes"
		};
		Layer layer8 = new Layer
		{
			Color = Color.FromArgb(255, 118, 9),
			Name = "Cutting Objects"
		};
		Layer layer9 = new Layer
		{
			Color = Color.FromArgb(255, 187, 25),
			Name = "Creation Curves"
		};
		Layer layer10 = new Layer
		{
			Color = Color.FromArgb(255, 0, 0),
			Name = "User Layer 01"
		};
		for (int i = 0; i < RhinoDoc.ActiveDoc.Layers.Count; i++)
		{
			RhinoDoc.ActiveDoc.Layers.Delete(i, true);
		}
		int currentLayerIndex = RhinoDoc.ActiveDoc.Layers.CurrentLayerIndex;
		RhinoDoc.ActiveDoc.Layers.Modify(val, currentLayerIndex, true);
		_layerIndexMetal01 = currentLayerIndex;
		_layerIndexMetal02 = AddLayer(layer);
		_layerIndexMetal03 = AddLayer(layer2);
		_layerIndexMetal04 = AddLayer(layer3);
		_layerIndexGem01 = AddLayer(layer4);
		_layerIndexGem02 = AddLayer(layer5);
		_layerIndexGem03 = AddLayer(layer6);
		_layerIndexFinger = AddLayer(layer7);
		_layerIndexCutting = AddLayer(layer8);
		_layerIndexCreation = AddLayer(layer9);
		_layerIndexUser01 = AddLayer(layer10);
		_layerMap = new Dictionary<LayerNumber, int>
		{
			{
				LayerNumber.Metal01,
				_layerIndexMetal01
			},
			{
				LayerNumber.Metal02,
				_layerIndexMetal02
			},
			{
				LayerNumber.Gem01,
				_layerIndexGem01
			},
			{
				LayerNumber.Gem02,
				_layerIndexGem02
			},
			{
				LayerNumber.Gem03,
				_layerIndexGem03
			},
			{
				LayerNumber.User01,
				_layerIndexUser01
			},
			{
				LayerNumber.User02,
				_layerIndexUser01
			},
			{
				LayerNumber.User03,
				_layerIndexUser01
			},
			{
				LayerNumber.User04,
				_layerIndexUser01
			},
			{
				LayerNumber.Finger,
				_layerIndexFinger
			},
			{
				LayerNumber.Cutting,
				_layerIndexCutting
			},
			{
				LayerNumber.Creation,
				_layerIndexCreation
			}
		};
	}

	private static int AddLayer(Layer layer)
	{
		int num = RhinoDoc.ActiveDoc.Layers.Find(layer.Name, true);
		if (num < 0)
		{
			num = RhinoDoc.ActiveDoc.Layers.Add(layer);
		}
		else
		{
			RhinoDoc.ActiveDoc.Layers.Modify(layer, num, true);
		}
		return num;
	}
}
