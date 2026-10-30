using System;
using System.Collections.Generic;
using System.Drawing;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Shared.Model.Objects;
using GV.CounterSketch.Studio.RhinoServicesReference;
using GV.CounterSketch.Studio.ViewModel;

namespace GV.CounterSketch.Studio.Model;

[CallbackBehavior(UseSynchronizationContext = false)]
public class StudioUIServiceCallback : IRhinoServicesCallback
{
	public IAsyncResult BeginFreehandControlPopulateCallback(UIControl[] uiControls, AsyncCallback callback, object asyncState)
	{
		throw new NotImplementedException();
	}

	public IAsyncResult BeginFreehandControlUpdateCallback(UIControl[] uiControls, AsyncCallback callback, object asyncState)
	{
		throw new NotImplementedException();
	}

	public IAsyncResult BeginFreehandImportGroupBitmapUpdateCallback(KeyValuePair<Guid, Bitmap> importGroupBitmap, AsyncCallback callback, object asyncState)
	{
		throw new NotImplementedException();
	}

	public IAsyncResult BeginFreehandImportGroupUpdateCallback(ImportGroup[] importGroups, AsyncCallback callback, object asyncState)
	{
		throw new NotImplementedException();
	}

	public IAsyncResult BeginResetApplicationState(AsyncCallback callback, object asyncState)
	{
		throw new NotImplementedException();
	}

	public IAsyncResult BeginRhinoWindowClickedCallback(AsyncCallback callback, object asyncState)
	{
		throw new NotImplementedException();
	}

	public IAsyncResult BeginRhinoWindowKeyPressedCallback(Key keyPressed, AsyncCallback callback, object asyncState)
	{
		throw new NotImplementedException();
	}

	public void EndFreehandControlPopulateCallback(IAsyncResult result)
	{
		throw new NotImplementedException();
	}

	public void EndFreehandControlUpdateCallback(IAsyncResult result)
	{
		throw new NotImplementedException();
	}

	public void EndFreehandImportGroupBitmapUpdateCallback(IAsyncResult result)
	{
		throw new NotImplementedException();
	}

	public void EndFreehandImportGroupUpdateCallback(IAsyncResult result)
	{
		throw new NotImplementedException();
	}

	public void EndResetApplicationState(IAsyncResult result)
	{
		throw new NotImplementedException();
	}

	public void EndRhinoWindowClickedCallback(IAsyncResult result)
	{
		throw new NotImplementedException();
	}

	public void EndRhinoWindowKeyPressedCallback(IAsyncResult result)
	{
		throw new NotImplementedException();
	}

	public void FreehandControlUpdateCallback(UIControl[] uiControls)
	{
		Application.Current.Dispatcher.InvokeAsync(delegate
		{
			ViewModelLocator.Current.FreehandStackVM.UpdateFreehandStackControls(uiControls, null);
		});
	}

	public void FreehandImportGroupBitmapUpdateCallback(KeyValuePair<Guid, Bitmap> importGroupBitmap)
	{
		Application.Current.Dispatcher.InvokeAsync(delegate
		{
			ViewModelLocator.Current.CustomizePartsListVM.UpdateImportGroupImageCaptures(importGroupBitmap.Key, importGroupBitmap.Value);
		}, DispatcherPriority.Background);
	}

	public void FreehandImportGroupUpdateCallback(ImportGroup[] importGroups)
	{
		Application.Current.Dispatcher.InvokeAsync(delegate
		{
			ViewModelLocator.Current.CustomizePartsListVM.UpdatePartsCollection(importGroups);
		});
	}

	public void ResetApplicationState()
	{
		ViewModelLocator.Current.MainVM.ResetApplication();
	}

	public void RhinoWindowClickedCallback()
	{
	}

	public void RhinoWindowKeyPressedCallback(Key keyPressed)
	{
		Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			Window mainWindow = Application.Current.MainWindow;
			RoutedEvent keyDownEvent = Keyboard.KeyDownEvent;
			mainWindow.RaiseEvent(new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(mainWindow), 0, keyPressed)
			{
				RoutedEvent = keyDownEvent
			});
		});
	}
}
