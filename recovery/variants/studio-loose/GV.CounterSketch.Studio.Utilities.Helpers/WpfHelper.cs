using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class WpfHelper
{
	public static void AddChild(this DependencyObject parent, UIElement child)
	{
		if (parent is Panel panel)
		{
			panel.Children.Add(child);
		}
		else if (parent is Decorator decorator)
		{
			if (decorator.Child == null)
			{
				decorator.Child = child;
			}
		}
		else if (parent is ContentPresenter contentPresenter)
		{
			if (contentPresenter.Content == null)
			{
				contentPresenter.Content = child;
			}
		}
		else if (parent is ContentControl { Content: null } contentControl)
		{
			contentControl.Content = child;
		}
	}

	public static IEnumerable<DependencyObject> GetChildObjects(this DependencyObject parent)
	{
		if (parent == null)
		{
			yield break;
		}
		if (parent is ContentElement current)
		{
			foreach (object child in LogicalTreeHelper.GetChildren(current))
			{
				if (child is DependencyObject)
				{
					yield return (DependencyObject)child;
				}
			}
		}
		else
		{
			int count = VisualTreeHelper.GetChildrenCount(parent);
			for (int i = 0; i < count; i++)
			{
				yield return VisualTreeHelper.GetChild(parent, i);
			}
		}
	}

	public static T FindAncestorOrSelf<T>(DependencyObject obj) where T : DependencyObject
	{
		while (obj != null)
		{
			if (obj is T result)
			{
				return result;
			}
			obj = GetParent(obj);
		}
		return null;
	}

	public static IEnumerable<T> FindChildren<T>(this DependencyObject source) where T : DependencyObject
	{
		if (source == null)
		{
			yield break;
		}
		IEnumerable<DependencyObject> childObjects = GetChildObjects(source);
		foreach (DependencyObject child in childObjects)
		{
			if (child is T)
			{
				yield return (T)child;
			}
			foreach (T item in child.FindChildren<T>())
			{
				yield return item;
			}
		}
	}

	public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
	{
		if (depObj == null)
		{
			yield break;
		}
		for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
		{
			DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
			if (child is T)
			{
				yield return (T)child;
			}
			foreach (T item in FindVisualChildren<T>(child))
			{
				yield return item;
			}
		}
	}

	private static DependencyObject GetParent(DependencyObject obj)
	{
		if (obj == null)
		{
			return null;
		}
		if (obj is ContentElement contentElement)
		{
			DependencyObject parent = ContentOperations.GetParent(contentElement);
			if (parent != null)
			{
				return parent;
			}
			if (!(contentElement is FrameworkContentElement frameworkContentElement))
			{
				return null;
			}
			return frameworkContentElement.Parent;
		}
		if (obj is FrameworkElement { Parent: { } parent2 })
		{
			return parent2;
		}
		return VisualTreeHelper.GetParent(obj);
	}

	public static DependencyObject GetParentObject(this DependencyObject child)
	{
		if (child == null)
		{
			return null;
		}
		if (child is ContentElement contentElement)
		{
			DependencyObject parent = ContentOperations.GetParent(contentElement);
			if (parent != null)
			{
				return parent;
			}
			if (!(contentElement is FrameworkContentElement frameworkContentElement))
			{
				return null;
			}
			return frameworkContentElement.Parent;
		}
		return VisualTreeHelper.GetParent(child);
	}

	public static void RemoveChild(this DependencyObject parent, UIElement child)
	{
		if (parent is Panel panel)
		{
			panel.Children.Remove(child);
		}
		else if (parent is Decorator decorator)
		{
			if (decorator.Child == child)
			{
				decorator.Child = null;
			}
		}
		else if (parent is ContentPresenter contentPresenter)
		{
			if (contentPresenter.Content == child)
			{
				contentPresenter.Content = null;
			}
		}
		else if (parent is ContentControl contentControl && contentControl.Content == child)
		{
			contentControl.Content = null;
		}
	}

	public static T TryFindParent<T>(this DependencyObject child) where T : DependencyObject
	{
		DependencyObject parentObject = GetParentObject(child);
		if (parentObject == null)
		{
			return null;
		}
		if (parentObject is T result)
		{
			return result;
		}
		return parentObject.TryFindParent<T>();
	}

	public static void AwesomeMethodinator()
	{
	}

	public static byte[] BitmapToPngByte(Bitmap bitmap)
	{
		using MemoryStream memoryStream = new MemoryStream();
		bitmap.Save(memoryStream, ImageFormat.Png);
		return memoryStream.ToArray();
	}

	public static bool CheckDesignTime()
	{
		if ((bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue)
		{
			return true;
		}
		return false;
	}

	public static UIElement Clone(this UIElement uiElement)
	{
		return (UIElement)XamlReader.Load(XmlReader.Create(new StringReader(XamlWriter.Save(uiElement))));
	}

	public static byte[] ImageToByte(System.Windows.Controls.Image img)
	{
		return (byte[])new ImageConverter().ConvertTo(img, typeof(byte[]));
	}

	public static void Refresh(this UIElement uiElement)
	{
		if (uiElement != null)
		{
			uiElement.InvalidateMeasure();
			uiElement.InvalidateArrange();
			uiElement.UpdateLayout();
		}
	}

	public static void ResetToAutoHeight(this ScrollViewer scrollViewer)
	{
		if (scrollViewer != null)
		{
			scrollViewer.Height = double.NaN;
			scrollViewer.InvalidateMeasure();
			scrollViewer.InvalidateArrange();
			scrollViewer.UpdateLayout();
		}
	}

	public static void SaveElementToBmp(FrameworkElement visual, string fileName)
	{
		BmpBitmapEncoder encoder = new BmpBitmapEncoder();
		SaveUsingEncoder(visual, fileName, encoder);
	}

	public static byte[] SaveElementToBmpByteArray(FrameworkElement visual)
	{
		BmpBitmapEncoder encoder = new BmpBitmapEncoder();
		return GetBytesUsingEncoder(visual, encoder);
	}

	public static void SaveElementToPng(FrameworkElement visual, string fileName)
	{
		PngBitmapEncoder encoder = new PngBitmapEncoder();
		SaveUsingEncoder(visual, fileName, encoder);
	}

	public static byte[] SaveElementToPngByteArray(FrameworkElement visual)
	{
		PngBitmapEncoder encoder = new PngBitmapEncoder();
		return GetBytesUsingEncoder(visual, encoder);
	}

	public static T TryFindFromPoint<T>(UIElement reference, System.Windows.Point point) where T : DependencyObject
	{
		if (!(reference.InputHitTest(point) is DependencyObject dependencyObject))
		{
			return null;
		}
		if (dependencyObject is T)
		{
			return (T)dependencyObject;
		}
		return dependencyObject.TryFindParent<T>();
	}

	private static byte[] GetBytesUsingEncoder(FrameworkElement visual, BitmapEncoder encoder)
	{
		RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);
		renderTargetBitmap.Render(visual);
		BitmapFrame item = BitmapFrame.Create(renderTargetBitmap);
		encoder.Frames.Add(item);
		using MemoryStream memoryStream = new MemoryStream();
		encoder.Save(memoryStream);
		return memoryStream.GetBuffer();
	}

	private static void SaveUsingEncoder(FrameworkElement visual, string fileName, BitmapEncoder encoder)
	{
		RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);
		renderTargetBitmap.Render(visual);
		BitmapFrame item = BitmapFrame.Create(renderTargetBitmap);
		encoder.Frames.Add(item);
		using FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate);
		encoder.Save(stream);
	}
}
