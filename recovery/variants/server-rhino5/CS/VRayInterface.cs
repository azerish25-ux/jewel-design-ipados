using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CS;

public sealed class VRayInterface
{
	public enum AnimationType
	{
		NTSC,
		PAL,
		Film,
		Custom
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void AddResourceSearchPathDelegate([MarshalAs(UnmanagedType.LPWStr)] string filename);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int ApplyVRayMaterialToObjectsDelegate([MarshalAs(UnmanagedType.LPWStr)] string material, int num_objects, IntPtr arrayRhinoObjectPointers);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int ApplyWrapperMaterialToObjectsDelegate([MarshalAs(UnmanagedType.LPWStr)] string filename, int count, IntPtr arrayRhinoObjectPointers);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int ApplyWrapperMaterialToSelectionDelegate([MarshalAs(UnmanagedType.LPWStr)] string filename);

	private delegate int ChangeMaterialSourceForObjectsDelegate(int source, int numObjects, IntPtr objects);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int DeleteMaterialDelegate([MarshalAs(UnmanagedType.LPWStr)] string material);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int DoneWithStringArrayDelegate(IntPtr arr);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.U1)]
	private delegate bool DrawTextureToContextDelegate([MarshalAs(UnmanagedType.LPWStr)] string file, IntPtr context, IntPtr imgWidth, IntPtr imgHeight);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate float GetAlphaContributionForMaterialDelegate([MarshalAs(UnmanagedType.LPWStr)] string material);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int GetDependentMapsForVismatDelegate([MarshalAs(UnmanagedType.LPWStr)] string path, out object maps);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int GetDependentMapsForVisoptDelegate([MarshalAs(UnmanagedType.LPWStr)] string path, out object maps);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate float GetDepthOfFieldApertureDelegate();

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate float GetDepthOfFieldFocalDistDelegate();

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int GetDepthOfFieldFocalDistOnDelegate();

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int GetDepthOfFieldSidesDelegate();

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int GetDepthOfFieldSidesOnDelegate();

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int GetDepthOfFieldSubdivisionsDelegate();

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int GetMaterialNameFromVismatDelegate([MarshalAs(UnmanagedType.LPWStr)] string filename, int show_error_msgs, out object name);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int GetObjectExclusionForMaterialDelegate([MarshalAs(UnmanagedType.LPWStr)] string material);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int GetObjectExclusionShortCircuitForMaterialDelegate([MarshalAs(UnmanagedType.LPWStr)] string material);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.U1)]
	private delegate bool GetOptionParameterValueBoolDelegate([MarshalAs(UnmanagedType.LPWStr)] string plugin_url, [MarshalAs(UnmanagedType.LPWStr)] string param_name, IntPtr doubleValue);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.U1)]
	private delegate bool GetOptionParameterValueDoubleDelegate([MarshalAs(UnmanagedType.LPWStr)] string plugin_url, [MarshalAs(UnmanagedType.LPWStr)] string param_name, IntPtr doubleValue);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.U1)]
	private delegate bool GetOptionParameterValueFloatDelegate([MarshalAs(UnmanagedType.LPWStr)] string plugin_url, [MarshalAs(UnmanagedType.LPWStr)] string param_name, IntPtr floatValue);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.U1)]
	private delegate bool GetOptionParameterValueIntegerDelegate([MarshalAs(UnmanagedType.LPWStr)] string plugin_url, [MarshalAs(UnmanagedType.LPWStr)] string param_name, IntPtr doubleValue);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.U1)]
	private delegate bool GetOptionParameterValueStringDelegate([MarshalAs(UnmanagedType.LPWStr)] string plugin_url, [MarshalAs(UnmanagedType.LPWStr)] string param_name, IntPtr value, int max_value_length);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int GetSceneMaterialNamesDelegate(out object names);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.U1)]
	private delegate bool GetWrapperMaterialFromObjectDelegate(IntPtr rhinoObject, out object path);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.U1)]
	private delegate bool HasRenderFinishedDelegate();

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void LoadDirectoryOfVismatsDelegate([MarshalAs(UnmanagedType.LPWStr)] string dir);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void LoadVismatDelegate([MarshalAs(UnmanagedType.LPWStr)] string file);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void LoadVisoptDelegate([MarshalAs(UnmanagedType.LPWStr)] string filename);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void PurgeUnusedMaterialsDelegate();

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void RemoveVFBChannelByNameDelegate([MarshalAs(UnmanagedType.LPWStr)] string channel);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int Render3dmToContextDelegate(IntPtr model, IntPtr context, string visOptFile);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int Render3dmWidePathToContextDelegate([MarshalAs(UnmanagedType.LPWStr)] string filename, IntPtr context, IntPtr read3dmErrorLog, [MarshalAs(UnmanagedType.LPWStr)] string visOptFile);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int RenderActiveDocToContextDelegate(IntPtr context, IntPtr view, int width, int height);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int RenderBinary3dmToContextDelegate(IntPtr model_acrhive, IntPtr context, IntPtr read3dmErrorLog, [MarshalAs(UnmanagedType.LPWStr)] string visOptFile);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void SaveMaterialDelegate([MarshalAs(UnmanagedType.LPWStr)] string material);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void SaveMaterialToFileDelegate([MarshalAs(UnmanagedType.LPWStr)] string material, [MarshalAs(UnmanagedType.LPWStr)] string filename);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void SaveVisoptDelegate([MarshalAs(UnmanagedType.LPWStr)] string filename);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int SetAlphaContributionForMaterialDelegate([MarshalAs(UnmanagedType.LPWStr)] string material, float alpha_contrib_multiplier);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void SetAnimationOnDelegate(int enabled);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void SetAnimationTypeDelegate(int type, int fps);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void SetBatchRenderOnDelegate(int enabled);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int SetDepthOfFieldApertureDelegate(float aperture);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int SetDepthOfFieldFocalDistOnDelegate(int override_focal_dist, float focal_dist);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int SetDepthOfFieldOnDelegate(int dof_on);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int SetDepthOfFieldSidesOnDelegate(int sides_on, int sides_num);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int SetDepthOfFieldSubdivisionDelegate(int subdivs);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void SetFrameStampDelegate([MarshalAs(UnmanagedType.LPWStr)] string material, int enabled, int nothing, int nothing2);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void SetObjectExclusionForMaterialDelegate([MarshalAs(UnmanagedType.LPWStr)] string material, int enabled);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void SetObjectExclusionShortCircuitForMaterialDelegate([MarshalAs(UnmanagedType.LPWStr)] string material, int enabled);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void SetOffscreenRenderModalityDelegate(int modal);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.U1)]
	private delegate bool SetOptionParameterValueBoolDelegate([MarshalAs(UnmanagedType.LPWStr)] string plugin_url, [MarshalAs(UnmanagedType.LPWStr)] string param_name, int value);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.U1)]
	private delegate bool SetOptionParameterValueDoubleDelegate([MarshalAs(UnmanagedType.LPWStr)] string plugin_url, [MarshalAs(UnmanagedType.LPWStr)] string param_name, double value);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.U1)]
	private delegate bool SetOptionParameterValueFloatDelegate([MarshalAs(UnmanagedType.LPWStr)] string plugin_url, [MarshalAs(UnmanagedType.LPWStr)] string param_name, float value);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.U1)]
	private delegate bool SetOptionParameterValueIntegerDelegate([MarshalAs(UnmanagedType.LPWStr)] string plugin_url, [MarshalAs(UnmanagedType.LPWStr)] string param_name, int value);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.U1)]
	private delegate bool SetOptionParameterValueStringDelegate([MarshalAs(UnmanagedType.LPWStr)] string plugin_url, [MarshalAs(UnmanagedType.LPWStr)] string param_name, [MarshalAs(UnmanagedType.LPWStr)] string value);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void SetProgressWindowOnDelegate(int enabled);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void SetRenderOutputEnableDelegate(int renderOutput);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void SetRenderOutputSaveFileDelegate([MarshalAs(UnmanagedType.LPWStr)] string filePath);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void SetRenderOutputSizeDelegate(int width, int height);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void SetThreadPriorityOnDelegate(int enabled);

	private static RenderBinary3dmToContextDelegate vrRenderBinary3dmToContext;

	private static Render3dmWidePathToContextDelegate vrRender3dmWidePathToContext;

	private static Render3dmToContextDelegate vrRender3dmToContext;

	private static RenderActiveDocToContextDelegate vrRenderActiveDocToContext;

	private static ApplyWrapperMaterialToSelectionDelegate vrApplyWrapperMaterialToSelection;

	private static ApplyWrapperMaterialToObjectsDelegate vrApplyWrapperMaterialToObjects;

	private static GetMaterialNameFromVismatDelegate vrGetMaterialNameFromVismat;

	private static LoadVisoptDelegate vrLoadVisopt;

	private static SaveVisoptDelegate vrSaveVisopt;

	private static AddResourceSearchPathDelegate vrAddResourceSearchPath;

	private static SetObjectExclusionForMaterialDelegate vrSetObjectExclusionForMaterial;

	private static GetObjectExclusionForMaterialDelegate vrGetObjectExclusionForMaterial;

	private static SetObjectExclusionShortCircuitForMaterialDelegate vrSetObjectExclusionShortCircuitForMaterial;

	private static GetObjectExclusionShortCircuitForMaterialDelegate vrGetObjectExclusionShortCircuitForMaterial;

	private static GetSceneMaterialNamesDelegate vrGetSceneMaterialNames;

	private static GetDependentMapsForVisoptDelegate vrGetDependentMapsForVisopt;

	private static GetDependentMapsForVismatDelegate vrGetDependentMapsForVismat;

	private static ApplyVRayMaterialToObjectsDelegate vrApplyVRayMaterialToObjects;

	private static DoneWithStringArrayDelegate vrDoneWithStringArray;

	private static SetRenderOutputSizeDelegate vrSetRenderOutputSize;

	private static SetRenderOutputSaveFileDelegate vrSetRenderOutputSaveFile;

	private static SetRenderOutputEnableDelegate vrSetRenderOutputEnable;

	private static DeleteMaterialDelegate vrDeleteMaterial;

	private static SaveMaterialDelegate vrSaveMaterial;

	private static SaveMaterialToFileDelegate vrSaveMaterialToFile;

	private static LoadVismatDelegate vrLoadVismat;

	private static LoadDirectoryOfVismatsDelegate vrLoadDirectoryOfVismats;

	private static SetAlphaContributionForMaterialDelegate vrSetAlphaContributionForMaterial;

	private static GetAlphaContributionForMaterialDelegate vrGetAlphaContributionForMaterial;

	private static SetFrameStampDelegate vrSetFrameStamp;

	private static SetProgressWindowOnDelegate vrSetProgressWindowOn;

	private static SetBatchRenderOnDelegate vrSetBatchRenderOn;

	private static SetThreadPriorityOnDelegate vrSetThreadPriorityOn;

	private static SetAnimationOnDelegate vrSetAnimationOn;

	private static SetAnimationTypeDelegate vrSetAnimationType;

	private static RemoveVFBChannelByNameDelegate vrRemoveVFBChannelByName;

	private static PurgeUnusedMaterialsDelegate vrPurgeUnusedMaterials;

	private static GetDepthOfFieldApertureDelegate vrGetDepthOfFieldAperture;

	private static GetDepthOfFieldFocalDistDelegate vrGetDepthOfFieldFocalDist;

	private static GetDepthOfFieldFocalDistOnDelegate vrGetDepthOfFieldFocalDistOn;

	private static GetDepthOfFieldSidesDelegate vrGetDepthOfFieldSides;

	private static GetDepthOfFieldSidesOnDelegate vrGetDepthOfFieldSidesOn;

	private static GetDepthOfFieldSubdivisionsDelegate vrGetDepthOfFieldSubdivisions;

	private static SetDepthOfFieldApertureDelegate vrSetDepthOfFieldAperture;

	private static SetDepthOfFieldFocalDistOnDelegate vrSetDepthOfFieldFocalDistOn;

	private static SetDepthOfFieldOnDelegate vrSetDepthOfFieldOn;

	private static SetDepthOfFieldSidesOnDelegate vrSetDepthOfFieldSidesOn;

	private static SetDepthOfFieldSubdivisionDelegate vrSetDepthOfFieldSubdivision;

	private static GetOptionParameterValueStringDelegate vrGetOptionParameterValueString;

	private static GetOptionParameterValueFloatDelegate vrGetOptionParameterValueFloat;

	private static GetOptionParameterValueDoubleDelegate vrGetOptionParameterValueDouble;

	private static GetOptionParameterValueIntegerDelegate vrGetOptionParameterValueInteger;

	private static GetOptionParameterValueBoolDelegate vrGetOptionParameterValueBool;

	private static SetOptionParameterValueStringDelegate vrSetOptionParameterValueString;

	private static SetOptionParameterValueFloatDelegate vrSetOptionParameterValueFloat;

	private static SetOptionParameterValueDoubleDelegate vrSetOptionParameterValueDouble;

	private static SetOptionParameterValueIntegerDelegate vrSetOptionParameterValueInteger;

	private static SetOptionParameterValueBoolDelegate vrSetOptionParameterValueBool;

	private static DrawTextureToContextDelegate vrDrawTextureToContext;

	private static GetWrapperMaterialFromObjectDelegate vrGetWrapperMaterialFromObject;

	private static SetOffscreenRenderModalityDelegate vrSetOffscreenRenderModality;

	private static HasRenderFinishedDelegate vrHasRenderFinished;

	private static ChangeMaterialSourceForObjectsDelegate vrChangeMaterialSourceForObjects;

	private static bool registered;

	public static string GetMaterialName(IRhinoObject obj)
	{
		if (obj == null)
		{
			return "";
		}
		Guid defaultRenderApp = RhUtil.RhinoApp().GetDefaultRenderApp();
		IRhinoMaterial val = default(IRhinoMaterial);
		IRhinoMaterial val2 = default(IRhinoMaterial);
		RhUtil.RhinoApp().ActiveDoc().m_material_table.GetMaterials(defaultRenderApp, (IOn3dmObjectAttributes)(object)obj.Attributes(), ref val, ref val2);
		if (val == null)
		{
			return "";
		}
		return ((IOnMaterial)val).m_material_name;
	}

	public static bool Render3dmWidePathToContext(string filename, IntPtr hdc, string visOptFile)
	{
		RegisterFunctions();
		if (vrRender3dmWidePathToContext == null)
		{
			return false;
		}
		return vrRender3dmWidePathToContext(filename, hdc, IntPtr.Zero, visOptFile) != 0;
	}

	public static bool Render3dmWidePathToContext(string filename, IntPtr hdc)
	{
		RegisterFunctions();
		if (vrRender3dmWidePathToContext == null)
		{
			return false;
		}
		return vrRender3dmWidePathToContext(filename, hdc, IntPtr.Zero, null) != 0;
	}

	public static bool Render3dmToContext(OnBinaryArchive archive, IntPtr hdc, string visOptFile)
	{
		RegisterFunctions();
		if (vrRender3dmToContext == null)
		{
			return false;
		}
		return vrRender3dmToContext(archive.InternalPointer, hdc, visOptFile) != 0;
	}

	public static bool RenderActiveDocToContext(IntPtr hdc, MRhinoView view, int width, int height)
	{
		RegisterFunctions();
		if (vrRenderActiveDocToContext == null)
		{
			return false;
		}
		return vrRenderActiveDocToContext(hdc, view.InternalPointer, width, height) != 0;
	}

	public static bool ApplyWrapperMaterialToSelection(string filename)
	{
		RegisterFunctions();
		if (vrApplyWrapperMaterialToSelection == null)
		{
			return false;
		}
		return vrApplyWrapperMaterialToSelection(filename) != 0;
	}

	public static bool ApplyWrapperMaterialToObjects(string filename, List<MRhinoObject> objects)
	{
		RegisterFunctions();
		if (vrApplyWrapperMaterialToObjects == null)
		{
			return false;
		}
		IntPtr[] array = new IntPtr[objects.Count];
		IntPtr intPtr = Marshal.AllocHGlobal(IntPtr.Size * objects.Count);
		for (int i = 0; i < objects.Count; i++)
		{
			array[i] = ((OnObject)objects[i]).InternalPointer;
		}
		Marshal.Copy(array, 0, intPtr, objects.Count);
		int num = vrApplyWrapperMaterialToObjects(filename, objects.Count, intPtr);
		Marshal.FreeHGlobal(intPtr);
		return num != 0;
	}

	public static string GetMaterialNameFromVismat(string filename, bool showError)
	{
		RegisterFunctions();
		if (vrGetMaterialNameFromVismat == null)
		{
			return "";
		}
		object name = null;
		vrGetMaterialNameFromVismat(filename, showError ? 1 : 0, out name);
		if (name == null)
		{
			return "";
		}
		return name.ToString();
	}

	public static bool LoadVisopt(string filename)
	{
		RegisterFunctions();
		if (vrLoadVisopt == null)
		{
			return false;
		}
		vrLoadVisopt(filename);
		return true;
	}

	public static bool SaveVisopt(string filename)
	{
		RegisterFunctions();
		if (vrSaveVisopt == null)
		{
			return false;
		}
		vrSaveVisopt(filename);
		return true;
	}

	public static bool AddResourceSearchPath(string filename)
	{
		RegisterFunctions();
		if (vrAddResourceSearchPath == null)
		{
			return false;
		}
		vrAddResourceSearchPath(filename);
		return true;
	}

	public static bool SetObjectExclusionForMaterial(string material, int enabled)
	{
		RegisterFunctions();
		if (vrSetObjectExclusionForMaterial == null)
		{
			return false;
		}
		vrSetObjectExclusionForMaterial(material, enabled);
		return true;
	}

	public static bool GetObjectExclusionForMaterial(string material)
	{
		RegisterFunctions();
		if (vrGetObjectExclusionForMaterial == null)
		{
			return false;
		}
		return vrGetObjectExclusionForMaterial(material) != 0;
	}

	public static bool SetObjectExclusionShortCircuitForMaterial(string material, int enabled)
	{
		RegisterFunctions();
		if (vrSetObjectExclusionShortCircuitForMaterial == null)
		{
			return false;
		}
		vrSetObjectExclusionShortCircuitForMaterial(material, enabled);
		return true;
	}

	public static bool GetObjectExclusionShortCircuitForMaterial(string material)
	{
		RegisterFunctions();
		if (vrGetObjectExclusionShortCircuitForMaterial == null)
		{
			return false;
		}
		return vrGetObjectExclusionShortCircuitForMaterial(material) != 0;
	}

	public static List<string> GetSceneMaterialNames()
	{
		RegisterFunctions();
		List<string> list = new List<string>();
		if (vrGetSceneMaterialNames == null)
		{
			return list;
		}
		object names = null;
		vrGetSceneMaterialNames(out names);
		if (names != null)
		{
			if (names is object[] array)
			{
				object[] array2 = array;
				foreach (object obj in array2)
				{
					list.Add(obj.ToString());
				}
			}
			else
			{
				list.Add(names.ToString());
			}
		}
		return list;
	}

	public static List<string> GetDependentMapsForVisopt(string path)
	{
		RegisterFunctions();
		List<string> list = new List<string>();
		if (vrGetDependentMapsForVisopt == null)
		{
			return list;
		}
		object maps = null;
		vrGetDependentMapsForVisopt(path, out maps);
		if (maps != null)
		{
			if (maps is object[] array)
			{
				object[] array2 = array;
				foreach (object obj in array2)
				{
					list.Add(obj.ToString());
				}
			}
			else
			{
				list.Add(maps.ToString());
			}
		}
		return list;
	}

	public static List<string> GetDependentMapsForVismat(string path)
	{
		RegisterFunctions();
		List<string> list = new List<string>();
		if (vrGetDependentMapsForVismat == null)
		{
			return list;
		}
		object maps = null;
		vrGetDependentMapsForVismat(path, out maps);
		if (maps != null)
		{
			if (maps is object[] array)
			{
				object[] array2 = array;
				foreach (object obj in array2)
				{
					list.Add(obj.ToString());
				}
			}
			else
			{
				list.Add(maps.ToString());
			}
		}
		return list;
	}

	public static bool ApplyVRayMaterialToObjects(string filename, List<MRhinoObject> objects)
	{
		RegisterFunctions();
		if (vrApplyWrapperMaterialToObjects == null)
		{
			return false;
		}
		IntPtr[] array = new IntPtr[objects.Count];
		IntPtr intPtr = Marshal.AllocHGlobal(IntPtr.Size * objects.Count);
		for (int i = 0; i < objects.Count; i++)
		{
			array[i] = ((OnObject)objects[i]).InternalPointer;
		}
		Marshal.Copy(array, 0, intPtr, objects.Count);
		int num = vrApplyVRayMaterialToObjects(filename, objects.Count, intPtr);
		Marshal.FreeHGlobal(intPtr);
		return num != 0;
	}

	public static bool SetRenderOutputSize(int width, int height)
	{
		RegisterFunctions();
		if (vrSetRenderOutputSize != null)
		{
			vrSetRenderOutputSize(width, height);
			return true;
		}
		return false;
	}

	public static bool SetRenderOutputSaveFile(string filename)
	{
		RegisterFunctions();
		if (filename == null)
		{
			return false;
		}
		if (vrSetRenderOutputSaveFile != null)
		{
			vrSetRenderOutputSaveFile(filename);
			return true;
		}
		return false;
	}

	public static bool SetRenderOutputEnable(bool enable)
	{
		RegisterFunctions();
		if (vrSetRenderOutputEnable != null)
		{
			vrSetRenderOutputEnable(enable ? 1 : 0);
			return true;
		}
		return false;
	}

	public static bool DeleteMaterial(string material)
	{
		RegisterFunctions();
		if (vrDeleteMaterial != null)
		{
			return vrDeleteMaterial(material) != 0;
		}
		return false;
	}

	public static bool SaveMaterial(string material)
	{
		RegisterFunctions();
		if (vrSaveMaterial != null)
		{
			vrSaveMaterial(material);
			return true;
		}
		return false;
	}

	public static bool SaveMaterialToFile(string material, string filename)
	{
		RegisterFunctions();
		if (vrSaveMaterialToFile != null)
		{
			vrSaveMaterialToFile(material, filename);
			return true;
		}
		return false;
	}

	public static bool LoadVismat(string material)
	{
		RegisterFunctions();
		if (vrLoadVismat != null)
		{
			vrLoadVismat(material);
			return true;
		}
		return false;
	}

	public static bool LoadDirectoryofVismats(string dir)
	{
		RegisterFunctions();
		if (vrLoadDirectoryOfVismats != null)
		{
			vrLoadDirectoryOfVismats(dir);
			return true;
		}
		return false;
	}

	public static bool SetAlphaContributionForMaterial(string material, float alpha_contrib_multiplier)
	{
		RegisterFunctions();
		if (vrSetAlphaContributionForMaterial != null)
		{
			return vrSetAlphaContributionForMaterial(material, alpha_contrib_multiplier) != 0;
		}
		return false;
	}

	public static float GetAlphaContributionForMaterial(string material)
	{
		RegisterFunctions();
		if (vrGetAlphaContributionForMaterial != null)
		{
			return vrGetAlphaContributionForMaterial(material);
		}
		return 0f;
	}

	public static bool SetFrameStep(string material, bool enabled)
	{
		RegisterFunctions();
		if (vrSetFrameStamp != null)
		{
			vrSetFrameStamp(material, enabled ? 1 : 0, 0, 0);
			return true;
		}
		return false;
	}

	public static bool SetProgressWindowOn(bool enabled)
	{
		RegisterFunctions();
		if (vrSetProgressWindowOn != null)
		{
			vrSetProgressWindowOn(enabled ? 1 : 0);
			return true;
		}
		return false;
	}

	public static bool SetBatchRender(bool enabled)
	{
		RegisterFunctions();
		if (vrSetBatchRenderOn != null)
		{
			vrSetBatchRenderOn(enabled ? 1 : 0);
			return true;
		}
		return false;
	}

	public static bool SetThreadPriorityOn(bool enabled)
	{
		RegisterFunctions();
		if (vrSetThreadPriorityOn != null)
		{
			vrSetThreadPriorityOn(enabled ? 1 : 0);
			return true;
		}
		return false;
	}

	public static bool SetAnimationOn(bool enabled)
	{
		RegisterFunctions();
		if (vrSetAnimationOn != null)
		{
			vrSetAnimationOn(enabled ? 1 : 0);
			return true;
		}
		return false;
	}

	public static bool SetAnimationType(AnimationType type, int fps)
	{
		RegisterFunctions();
		if (vrSetAnimationType != null)
		{
			vrSetAnimationType((int)type, fps);
			return true;
		}
		return false;
	}

	public static bool RemoveVFBChannelByName(string channel)
	{
		RegisterFunctions();
		if (vrRemoveVFBChannelByName != null)
		{
			vrRemoveVFBChannelByName(channel);
			return true;
		}
		return false;
	}

	public static bool PurgeUnusedMaterials()
	{
		RegisterFunctions();
		if (vrPurgeUnusedMaterials != null)
		{
			vrPurgeUnusedMaterials();
			return true;
		}
		return false;
	}

	public static float GetDepthOfFieldAperture()
	{
		RegisterFunctions();
		if (vrGetDepthOfFieldAperture != null)
		{
			return vrGetDepthOfFieldAperture();
		}
		return -1f;
	}

	public static float GetDepthOfFieldFocalDist()
	{
		RegisterFunctions();
		if (vrGetDepthOfFieldFocalDist != null)
		{
			return vrGetDepthOfFieldFocalDist();
		}
		return -1f;
	}

	public static int GetDepthOfFieldFocalDistOn()
	{
		RegisterFunctions();
		if (vrGetDepthOfFieldFocalDistOn != null)
		{
			return vrGetDepthOfFieldFocalDistOn();
		}
		return -1;
	}

	public static int GetDepthOfFieldSides()
	{
		RegisterFunctions();
		if (vrGetDepthOfFieldSides != null)
		{
			return vrGetDepthOfFieldSides();
		}
		return -1;
	}

	public static int GetDepthOfFieldSidesOn()
	{
		RegisterFunctions();
		if (vrGetDepthOfFieldSidesOn != null)
		{
			return vrGetDepthOfFieldSidesOn();
		}
		return -1;
	}

	public static int GetDepthOfFieldSubdivisions()
	{
		RegisterFunctions();
		if (vrGetDepthOfFieldSubdivisions != null)
		{
			return vrGetDepthOfFieldSubdivisions();
		}
		return -1;
	}

	public static int SetDepthOfFieldAperture(float aperture)
	{
		RegisterFunctions();
		if (vrSetDepthOfFieldAperture != null)
		{
			return vrSetDepthOfFieldAperture(aperture);
		}
		return -1;
	}

	public static int SetDepthOfFieldFocalDistOn(int override_focal_dist, float focal_dist)
	{
		RegisterFunctions();
		if (vrSetDepthOfFieldFocalDistOn != null)
		{
			return vrSetDepthOfFieldFocalDistOn(override_focal_dist, focal_dist);
		}
		return -1;
	}

	public static int SetDepthOfFieldOn(int dof_on)
	{
		RegisterFunctions();
		if (vrSetDepthOfFieldOn != null)
		{
			return vrSetDepthOfFieldOn(dof_on);
		}
		return -1;
	}

	public static int SetDepthOfFieldSidesOn(int sides_on, int sides_num)
	{
		RegisterFunctions();
		if (vrSetDepthOfFieldSidesOn != null)
		{
			return vrSetDepthOfFieldSidesOn(sides_on, sides_num);
		}
		return -1;
	}

	public static int SetDepthOfFieldSubdivision(int subdivs)
	{
		RegisterFunctions();
		if (vrSetDepthOfFieldSubdivision != null)
		{
			return vrSetDepthOfFieldSubdivision(subdivs);
		}
		return -1;
	}

	public static bool GetOptionParameterValueString(string plugin_url, string param_name, ref string value)
	{
		RegisterFunctions();
		if (string.IsNullOrEmpty(plugin_url))
		{
			return false;
		}
		if (string.IsNullOrEmpty(param_name))
		{
			return false;
		}
		if (vrGetOptionParameterValueString != null)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(512);
			bool flag = false;
			if (intPtr != IntPtr.Zero)
			{
				flag = vrGetOptionParameterValueString(plugin_url, param_name, intPtr, 255);
				if (flag)
				{
					value = Marshal.PtrToStringUni(intPtr);
				}
				Marshal.FreeHGlobal(intPtr);
			}
			return flag;
		}
		return false;
	}

	public static bool GetOptionParameterValueFloat(string plugin_url, string param_name, ref float value)
	{
		RegisterFunctions();
		if (string.IsNullOrEmpty(plugin_url))
		{
			return false;
		}
		if (string.IsNullOrEmpty(param_name))
		{
			return false;
		}
		if (vrGetOptionParameterValueFloat != null)
		{
			bool flag = false;
			IntPtr intPtr = Marshal.AllocHGlobal(4);
			if (intPtr != IntPtr.Zero)
			{
				flag = vrGetOptionParameterValueFloat(plugin_url, param_name, intPtr);
				if (flag)
				{
					float[] array = new float[1];
					Marshal.Copy(intPtr, array, 0, 1);
					value = array[0];
				}
				Marshal.FreeHGlobal(intPtr);
			}
			return flag;
		}
		return false;
	}

	public static bool GetOptionParameterValueDouble(string plugin_url, string param_name, ref double value)
	{
		RegisterFunctions();
		if (string.IsNullOrEmpty(plugin_url))
		{
			return false;
		}
		if (string.IsNullOrEmpty(param_name))
		{
			return false;
		}
		if (vrGetOptionParameterValueDouble != null)
		{
			bool flag = false;
			IntPtr intPtr = Marshal.AllocHGlobal(8);
			if (intPtr != IntPtr.Zero)
			{
				flag = vrGetOptionParameterValueDouble(plugin_url, param_name, intPtr);
				if (flag)
				{
					double[] array = new double[1];
					Marshal.Copy(intPtr, array, 0, 1);
					value = array[0];
				}
				Marshal.FreeHGlobal(intPtr);
			}
			return flag;
		}
		return false;
	}

	public static bool GetOptionParameterValueInteger(string plugin_url, string param_name, ref int value)
	{
		RegisterFunctions();
		if (string.IsNullOrEmpty(plugin_url))
		{
			return false;
		}
		if (string.IsNullOrEmpty(param_name))
		{
			return false;
		}
		if (vrGetOptionParameterValueInteger != null)
		{
			bool flag = false;
			IntPtr intPtr = Marshal.AllocHGlobal(4);
			if (intPtr != IntPtr.Zero)
			{
				flag = vrGetOptionParameterValueInteger(plugin_url, param_name, intPtr);
				if (flag)
				{
					value = Marshal.ReadInt32(intPtr);
				}
				Marshal.FreeHGlobal(intPtr);
			}
			return flag;
		}
		return false;
	}

	public static bool GetOptionParameterValueBool(string plugin_url, string param_name, ref int value)
	{
		RegisterFunctions();
		if (string.IsNullOrEmpty(plugin_url))
		{
			return false;
		}
		if (string.IsNullOrEmpty(param_name))
		{
			return false;
		}
		if (vrGetOptionParameterValueInteger != null)
		{
			bool flag = false;
			IntPtr intPtr = Marshal.AllocHGlobal(4);
			if (intPtr != IntPtr.Zero)
			{
				flag = vrGetOptionParameterValueBool(plugin_url, param_name, intPtr);
				if (flag)
				{
					value = Marshal.ReadInt32(intPtr);
				}
				Marshal.FreeHGlobal(intPtr);
			}
			return flag;
		}
		return false;
	}

	public static bool SetOptionParameterValueString(string plugin_url, string param_name, string value)
	{
		RegisterFunctions();
		if (string.IsNullOrEmpty(plugin_url))
		{
			return false;
		}
		if (string.IsNullOrEmpty(param_name))
		{
			return false;
		}
		if (string.IsNullOrEmpty(value))
		{
			return false;
		}
		if (vrSetOptionParameterValueString != null)
		{
			return vrSetOptionParameterValueString(plugin_url, param_name, value);
		}
		return false;
	}

	public static bool SetOptionParameterValueFloat(string plugin_url, string param_name, float value)
	{
		RegisterFunctions();
		if (string.IsNullOrEmpty(plugin_url))
		{
			return false;
		}
		if (string.IsNullOrEmpty(param_name))
		{
			return false;
		}
		if (vrSetOptionParameterValueFloat != null)
		{
			return vrSetOptionParameterValueFloat(plugin_url, param_name, value);
		}
		return false;
	}

	public static bool SetOptionParameterValueDouble(string plugin_url, string param_name, double value)
	{
		RegisterFunctions();
		if (string.IsNullOrEmpty(plugin_url))
		{
			return false;
		}
		if (string.IsNullOrEmpty(param_name))
		{
			return false;
		}
		if (vrSetOptionParameterValueDouble != null)
		{
			return vrSetOptionParameterValueDouble(plugin_url, param_name, value);
		}
		return false;
	}

	public static bool SetOptionParameterValueInteger(string plugin_url, string param_name, int value)
	{
		RegisterFunctions();
		if (string.IsNullOrEmpty(plugin_url))
		{
			return false;
		}
		if (string.IsNullOrEmpty(param_name))
		{
			return false;
		}
		if (vrSetOptionParameterValueInteger != null)
		{
			return vrSetOptionParameterValueInteger(plugin_url, param_name, value);
		}
		return false;
	}

	public static bool SetOptionParameterValueBool(string plugin_url, string param_name, int value)
	{
		RegisterFunctions();
		if (string.IsNullOrEmpty(plugin_url))
		{
			return false;
		}
		if (string.IsNullOrEmpty(param_name))
		{
			return false;
		}
		if (vrSetOptionParameterValueBool != null)
		{
			return vrSetOptionParameterValueBool(plugin_url, param_name, value);
		}
		return false;
	}

	public static Bitmap DrawTextureToBitmap(string file)
	{
		RegisterFunctions();
		if (string.IsNullOrEmpty(file))
		{
			return null;
		}
		if (vrDrawTextureToContext == null)
		{
			return null;
		}
		IntPtr intPtr = Marshal.AllocHGlobal(4);
		IntPtr intPtr2 = Marshal.AllocHGlobal(4);
		if (!vrDrawTextureToContext(file, IntPtr.Zero, intPtr, intPtr2))
		{
			Marshal.FreeHGlobal(intPtr);
			Marshal.FreeHGlobal(intPtr2);
			return null;
		}
		int width = Marshal.ReadInt32(intPtr);
		int height = Marshal.ReadInt32(intPtr2);
		Bitmap bitmap = new Bitmap(width, height);
		using (Graphics graphics = Graphics.FromImage(bitmap))
		{
			IntPtr hdc = graphics.GetHdc();
			vrDrawTextureToContext(file, hdc, intPtr, intPtr2);
			graphics.ReleaseHdc(hdc);
		}
		Marshal.FreeHGlobal(intPtr);
		Marshal.FreeHGlobal(intPtr2);
		return bitmap;
	}

	public static bool GetWrapperMaterialFromObject(MRhinoObject obj, ref string path)
	{
		RegisterFunctions();
		if (vrGetWrapperMaterialFromObject == null)
		{
			return false;
		}
		if (obj == null)
		{
			return false;
		}
		object path2 = null;
		vrGetWrapperMaterialFromObject(((OnObject)obj).InternalPointer, out path2);
		if (path2 == null)
		{
			return false;
		}
		path = path2.ToString();
		return true;
	}

	public static void SetOffscreenRenderModality(bool modal)
	{
		RegisterFunctions();
		if (vrSetOffscreenRenderModality != null)
		{
			vrSetOffscreenRenderModality(modal ? 1 : 0);
		}
	}

	public static bool HasRenderFinished()
	{
		RegisterFunctions();
		if (vrHasRenderFinished == null)
		{
			return true;
		}
		return vrHasRenderFinished();
	}

	public static bool ChangeMateralSourceForObjects(object_material_source materialSource, List<MRhinoObject> objects)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected I4, but got Unknown
		if (vrChangeMaterialSourceForObjects == null)
		{
			return false;
		}
		IntPtr[] array = new IntPtr[objects.Count];
		IntPtr intPtr = Marshal.AllocHGlobal(IntPtr.Size * objects.Count);
		for (int i = 0; i < objects.Count; i++)
		{
			if (objects[i] == null)
			{
				array[i] = IntPtr.Zero;
			}
			else
			{
				array[i] = ((OnObject)objects[i]).InternalPointer;
			}
		}
		Marshal.Copy(array, 0, intPtr, objects.Count);
		int num = vrChangeMaterialSourceForObjects((int)materialSource, objects.Count, intPtr);
		Marshal.FreeHGlobal(intPtr);
		return num == objects.Count;
	}

	public static bool ChangeMateralSourceForObjects(object_material_source materialSource, List<IRhinoObject> objects)
	{
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected I4, but got Unknown
		if (vrChangeMaterialSourceForObjects == null)
		{
			return false;
		}
		IntPtr[] array = new IntPtr[objects.Count];
		IntPtr intPtr = Marshal.AllocHGlobal(IntPtr.Size * objects.Count);
		for (int i = 0; i < objects.Count; i++)
		{
			if (objects[i] == null)
			{
				array[i] = IntPtr.Zero;
				continue;
			}
			MRhinoObject val = RhUtil.RhinoApp().ActiveDoc().LookupObject(((IOn3dmObjectAttributes)objects[i].Attributes()).m_uuid);
			if (val == null)
			{
				array[i] = IntPtr.Zero;
			}
			else
			{
				array[i] = ((OnObject)val).InternalPointer;
			}
		}
		Marshal.Copy(array, 0, intPtr, objects.Count);
		int num = vrChangeMaterialSourceForObjects((int)materialSource, objects.Count, intPtr);
		Marshal.FreeHGlobal(intPtr);
		return num == objects.Count;
	}

	public static bool ChangeMateralSourceForObject(object_material_source materialSource, IRhinoObject obj)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		List<IRhinoObject> list = new List<IRhinoObject>();
		list.Add(obj);
		return ChangeMateralSourceForObjects(materialSource, list);
	}

	[DllImport("kernel32")]
	public static extern IntPtr GetModuleHandle(string filename);

	[DllImport("kernel32")]
	public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

	[DllImport("gdi32.dll", SetLastError = true)]
	private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

	[DllImport("gdi32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool DeleteDC(IntPtr hdc);

	public static bool RegisterFunctions()
	{
		if (registered)
		{
			return true;
		}
		string vRayPluginFromRhinoKey = GetVRayPluginFromRhinoKey();
		LoadPlugin(vRayPluginFromRhinoKey);
		IntPtr moduleHandle = GetModuleHandle(vRayPluginFromRhinoKey);
		if (IntPtr.Zero == moduleHandle)
		{
			return false;
		}
		IntPtr procAddress = GetProcAddress(moduleHandle, "Render3dmWidePathToContext");
		if (procAddress != IntPtr.Zero)
		{
			vrRender3dmWidePathToContext = (Render3dmWidePathToContextDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(Render3dmWidePathToContextDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "RenderBinary3dmToContext");
		if (procAddress != IntPtr.Zero)
		{
			vrRenderBinary3dmToContext = (RenderBinary3dmToContextDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(RenderBinary3dmToContextDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "Render3dmToContext");
		if (procAddress != IntPtr.Zero)
		{
			vrRender3dmToContext = (Render3dmToContextDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(Render3dmToContextDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "RenderActiveDocToContext");
		if (procAddress != IntPtr.Zero)
		{
			vrRenderActiveDocToContext = (RenderActiveDocToContextDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(RenderActiveDocToContextDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "ApplyWrapperMaterialToSelection");
		if (procAddress != IntPtr.Zero)
		{
			vrApplyWrapperMaterialToSelection = (ApplyWrapperMaterialToSelectionDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(ApplyWrapperMaterialToSelectionDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "ApplyWrapperMaterialToObjects");
		if (procAddress != IntPtr.Zero)
		{
			vrApplyWrapperMaterialToObjects = (ApplyWrapperMaterialToObjectsDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(ApplyWrapperMaterialToObjectsDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "GetMaterialNameFromVismat");
		if (procAddress != IntPtr.Zero)
		{
			vrGetMaterialNameFromVismat = (GetMaterialNameFromVismatDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(GetMaterialNameFromVismatDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "LoadVisopt");
		if (procAddress != IntPtr.Zero)
		{
			vrLoadVisopt = (LoadVisoptDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(LoadVisoptDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SaveVisopt");
		if (procAddress != IntPtr.Zero)
		{
			vrSaveVisopt = (SaveVisoptDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SaveVisoptDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "AddResourceSearchPath");
		if (procAddress != IntPtr.Zero)
		{
			vrAddResourceSearchPath = (AddResourceSearchPathDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(AddResourceSearchPathDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SetObjectExclusionForMaterial");
		if (procAddress != IntPtr.Zero)
		{
			vrSetObjectExclusionForMaterial = (SetObjectExclusionForMaterialDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SetObjectExclusionForMaterialDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "GetObjectExclusionForMaterial");
		if (procAddress != IntPtr.Zero)
		{
			vrGetObjectExclusionForMaterial = (GetObjectExclusionForMaterialDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(GetObjectExclusionForMaterialDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SetObjectExclusionShortCircuitForMaterial");
		if (procAddress != IntPtr.Zero)
		{
			vrSetObjectExclusionShortCircuitForMaterial = (SetObjectExclusionShortCircuitForMaterialDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SetObjectExclusionShortCircuitForMaterialDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "GetObjectExclusionShortCircuitForMaterial");
		if (procAddress != IntPtr.Zero)
		{
			vrGetObjectExclusionShortCircuitForMaterial = (GetObjectExclusionShortCircuitForMaterialDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(GetObjectExclusionShortCircuitForMaterialDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "GetSceneMaterialNames");
		if (procAddress != IntPtr.Zero)
		{
			vrGetSceneMaterialNames = (GetSceneMaterialNamesDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(GetSceneMaterialNamesDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "GetDependentMapsForVisopt");
		if (procAddress != IntPtr.Zero)
		{
			vrGetDependentMapsForVisopt = (GetDependentMapsForVisoptDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(GetDependentMapsForVisoptDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "GetDependentMapsForVismat");
		if (procAddress != IntPtr.Zero)
		{
			vrGetDependentMapsForVismat = (GetDependentMapsForVismatDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(GetDependentMapsForVismatDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "ApplyVRayMaterialToObjects");
		if (procAddress != IntPtr.Zero)
		{
			vrApplyVRayMaterialToObjects = (ApplyVRayMaterialToObjectsDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(ApplyVRayMaterialToObjectsDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "DoneWithStringArray");
		if (procAddress != IntPtr.Zero)
		{
			vrDoneWithStringArray = (DoneWithStringArrayDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(DoneWithStringArrayDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SetRenderOutputSize");
		if (procAddress != IntPtr.Zero)
		{
			vrSetRenderOutputSize = (SetRenderOutputSizeDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SetRenderOutputSizeDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SetRenderOutputSaveFile");
		if (procAddress != IntPtr.Zero)
		{
			vrSetRenderOutputSaveFile = (SetRenderOutputSaveFileDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SetRenderOutputSaveFileDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SetRenderOutputEnable");
		if (procAddress != IntPtr.Zero)
		{
			vrSetRenderOutputEnable = (SetRenderOutputEnableDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SetRenderOutputEnableDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "DeleteMaterial");
		if (procAddress != IntPtr.Zero)
		{
			vrDeleteMaterial = (DeleteMaterialDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(DeleteMaterialDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SaveMaterial");
		if (procAddress != IntPtr.Zero)
		{
			vrSaveMaterial = (SaveMaterialDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SaveMaterialDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SaveMaterialToFile");
		if (procAddress != IntPtr.Zero)
		{
			vrSaveMaterialToFile = (SaveMaterialToFileDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SaveMaterialToFileDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "LoadVismat");
		if (procAddress != IntPtr.Zero)
		{
			vrLoadVismat = (LoadVismatDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(LoadVismatDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "LoadDirectoryOfVismats");
		if (procAddress != IntPtr.Zero)
		{
			vrLoadDirectoryOfVismats = (LoadDirectoryOfVismatsDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(LoadDirectoryOfVismatsDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SetAlphaContributionForMaterial");
		if (procAddress != IntPtr.Zero)
		{
			vrSetAlphaContributionForMaterial = (SetAlphaContributionForMaterialDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SetAlphaContributionForMaterialDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "GetAlphaContributionForMaterial");
		if (procAddress != IntPtr.Zero)
		{
			vrGetAlphaContributionForMaterial = (GetAlphaContributionForMaterialDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(GetAlphaContributionForMaterialDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SetFrameStamp");
		if (procAddress != IntPtr.Zero)
		{
			vrSetFrameStamp = (SetFrameStampDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SetFrameStampDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SetProgressWindowOn");
		if (procAddress != IntPtr.Zero)
		{
			vrSetProgressWindowOn = (SetProgressWindowOnDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SetProgressWindowOnDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SetBatchRenderOn");
		if (procAddress != IntPtr.Zero)
		{
			vrSetBatchRenderOn = (SetBatchRenderOnDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SetBatchRenderOnDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SetThreadPriorityOn");
		if (procAddress != IntPtr.Zero)
		{
			vrSetThreadPriorityOn = (SetThreadPriorityOnDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SetThreadPriorityOnDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SetAnimationOn");
		if (procAddress != IntPtr.Zero)
		{
			vrSetAnimationOn = (SetAnimationOnDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SetAnimationOnDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SetAnimationType");
		if (procAddress != IntPtr.Zero)
		{
			vrSetAnimationType = (SetAnimationTypeDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SetAnimationTypeDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "RemoveVFBChannelByName");
		if (procAddress != IntPtr.Zero)
		{
			vrRemoveVFBChannelByName = (RemoveVFBChannelByNameDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(RemoveVFBChannelByNameDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "PurgeUnusedMaterials");
		if (procAddress != IntPtr.Zero)
		{
			vrPurgeUnusedMaterials = (PurgeUnusedMaterialsDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(PurgeUnusedMaterialsDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "GetDepthOfFieldAperture");
		if (procAddress != IntPtr.Zero)
		{
			vrGetDepthOfFieldAperture = (GetDepthOfFieldApertureDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(GetDepthOfFieldApertureDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "GetDepthOfFieldFocalDist");
		if (procAddress != IntPtr.Zero)
		{
			vrGetDepthOfFieldFocalDist = (GetDepthOfFieldFocalDistDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(GetDepthOfFieldFocalDistDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "GetDepthOfFieldFocalDistOn");
		if (procAddress != IntPtr.Zero)
		{
			vrGetDepthOfFieldFocalDistOn = (GetDepthOfFieldFocalDistOnDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(GetDepthOfFieldFocalDistOnDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "GetDepthOfFieldSides");
		if (procAddress != IntPtr.Zero)
		{
			vrGetDepthOfFieldSides = (GetDepthOfFieldSidesDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(GetDepthOfFieldSidesDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "GetDepthOfFieldSidesOn");
		if (procAddress != IntPtr.Zero)
		{
			vrGetDepthOfFieldSidesOn = (GetDepthOfFieldSidesOnDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(GetDepthOfFieldSidesOnDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "GetDepthOfFieldSubdivisions");
		if (procAddress != IntPtr.Zero)
		{
			vrGetDepthOfFieldSubdivisions = (GetDepthOfFieldSubdivisionsDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(GetDepthOfFieldSubdivisionsDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SetDepthOfFieldAperture");
		if (procAddress != IntPtr.Zero)
		{
			vrSetDepthOfFieldAperture = (SetDepthOfFieldApertureDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SetDepthOfFieldApertureDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SetDepthOfFieldFocalDistOn");
		if (procAddress != IntPtr.Zero)
		{
			vrSetDepthOfFieldFocalDistOn = (SetDepthOfFieldFocalDistOnDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SetDepthOfFieldFocalDistOnDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SetDepthOfFieldOn");
		if (procAddress != IntPtr.Zero)
		{
			vrSetDepthOfFieldOn = (SetDepthOfFieldOnDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SetDepthOfFieldOnDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SetDepthOfFieldSidesOn");
		if (procAddress != IntPtr.Zero)
		{
			vrSetDepthOfFieldSidesOn = (SetDepthOfFieldSidesOnDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SetDepthOfFieldSidesOnDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SetDepthOfFieldSubdivision");
		if (procAddress != IntPtr.Zero)
		{
			vrSetDepthOfFieldSubdivision = (SetDepthOfFieldSubdivisionDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SetDepthOfFieldSubdivisionDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "GetOptionParameterValueString");
		if (procAddress != IntPtr.Zero)
		{
			vrGetOptionParameterValueString = (GetOptionParameterValueStringDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(GetOptionParameterValueStringDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "GetOptionParameterValueFloat");
		if (procAddress != IntPtr.Zero)
		{
			vrGetOptionParameterValueFloat = (GetOptionParameterValueFloatDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(GetOptionParameterValueFloatDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "GetOptionParameterValueDouble");
		if (procAddress != IntPtr.Zero)
		{
			vrGetOptionParameterValueDouble = (GetOptionParameterValueDoubleDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(GetOptionParameterValueDoubleDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "GetOptionParameterValueInteger");
		if (procAddress != IntPtr.Zero)
		{
			vrGetOptionParameterValueInteger = (GetOptionParameterValueIntegerDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(GetOptionParameterValueIntegerDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "GetOptionParameterValueBool");
		if (procAddress != IntPtr.Zero)
		{
			vrGetOptionParameterValueBool = (GetOptionParameterValueBoolDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(GetOptionParameterValueBoolDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SetOptionParameterValueString");
		if (procAddress != IntPtr.Zero)
		{
			vrSetOptionParameterValueString = (SetOptionParameterValueStringDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SetOptionParameterValueStringDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SetOptionParameterValueFloat");
		if (procAddress != IntPtr.Zero)
		{
			vrSetOptionParameterValueFloat = (SetOptionParameterValueFloatDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SetOptionParameterValueFloatDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SetOptionParameterValueDouble");
		if (procAddress != IntPtr.Zero)
		{
			vrSetOptionParameterValueDouble = (SetOptionParameterValueDoubleDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SetOptionParameterValueDoubleDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SetOptionParameterValueInteger");
		if (procAddress != IntPtr.Zero)
		{
			vrSetOptionParameterValueInteger = (SetOptionParameterValueIntegerDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SetOptionParameterValueIntegerDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SetOptionParameterValueBool");
		if (procAddress != IntPtr.Zero)
		{
			vrSetOptionParameterValueBool = (SetOptionParameterValueBoolDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SetOptionParameterValueBoolDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "DrawTextureToContext");
		if (procAddress != IntPtr.Zero)
		{
			vrDrawTextureToContext = (DrawTextureToContextDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(DrawTextureToContextDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "GetWrapperMaterialFromObject");
		if (procAddress != IntPtr.Zero)
		{
			vrGetWrapperMaterialFromObject = (GetWrapperMaterialFromObjectDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(GetWrapperMaterialFromObjectDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "SetOffscreenRenderModality");
		if (procAddress != IntPtr.Zero)
		{
			vrSetOffscreenRenderModality = (SetOffscreenRenderModalityDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(SetOffscreenRenderModalityDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "HasRenderFinished");
		if (procAddress != IntPtr.Zero)
		{
			vrHasRenderFinished = (HasRenderFinishedDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(HasRenderFinishedDelegate));
		}
		procAddress = GetProcAddress(moduleHandle, "ChangeMaterialSourceForObjects");
		if (procAddress != IntPtr.Zero)
		{
			vrChangeMaterialSourceForObjects = (ChangeMaterialSourceForObjectsDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(ChangeMaterialSourceForObjectsDelegate));
		}
		registered = IsValid();
		return registered;
	}

	public static bool IsValid()
	{
		if (vrRender3dmWidePathToContext == null)
		{
			return false;
		}
		if (vrRenderBinary3dmToContext == null)
		{
			return false;
		}
		if (vrRender3dmToContext == null)
		{
			return false;
		}
		if (vrRenderActiveDocToContext == null)
		{
			return false;
		}
		if (vrApplyWrapperMaterialToSelection == null)
		{
			return false;
		}
		if (vrApplyWrapperMaterialToObjects == null)
		{
			return false;
		}
		if (vrGetMaterialNameFromVismat == null)
		{
			return false;
		}
		if (vrLoadVisopt == null)
		{
			return false;
		}
		if (vrAddResourceSearchPath == null)
		{
			return false;
		}
		if (vrSetObjectExclusionForMaterial == null)
		{
			return false;
		}
		if (vrSetObjectExclusionShortCircuitForMaterial == null)
		{
			return false;
		}
		if (vrGetObjectExclusionForMaterial == null)
		{
			return false;
		}
		if (vrGetObjectExclusionShortCircuitForMaterial == null)
		{
			return false;
		}
		if (vrGetSceneMaterialNames == null)
		{
			return false;
		}
		if (vrGetDependentMapsForVisopt == null)
		{
			return false;
		}
		if (vrGetDependentMapsForVismat == null)
		{
			return false;
		}
		if (vrApplyVRayMaterialToObjects == null)
		{
			return false;
		}
		if (vrDoneWithStringArray == null)
		{
			return false;
		}
		if (vrSetRenderOutputSize == null)
		{
			return false;
		}
		if (vrSetRenderOutputSaveFile == null)
		{
			return false;
		}
		if (vrSetRenderOutputEnable == null)
		{
			return false;
		}
		if (vrSaveVisopt == null)
		{
			return false;
		}
		if (vrDeleteMaterial == null)
		{
			return false;
		}
		if (vrSaveMaterial == null)
		{
			return false;
		}
		if (vrSaveMaterialToFile == null)
		{
			return false;
		}
		if (vrLoadVismat == null)
		{
			return false;
		}
		if (vrLoadDirectoryOfVismats == null)
		{
			return false;
		}
		if (vrSetAlphaContributionForMaterial == null)
		{
			return false;
		}
		if (vrGetAlphaContributionForMaterial == null)
		{
			return false;
		}
		if (vrSetFrameStamp == null)
		{
			return false;
		}
		if (vrSetProgressWindowOn == null)
		{
			return false;
		}
		if (vrSetBatchRenderOn == null)
		{
			return false;
		}
		if (vrSetThreadPriorityOn == null)
		{
			return false;
		}
		if (vrSetAnimationOn == null)
		{
			return false;
		}
		if (vrSetAnimationType == null)
		{
			return false;
		}
		if (vrRemoveVFBChannelByName == null)
		{
			return false;
		}
		if (vrPurgeUnusedMaterials == null)
		{
			return false;
		}
		if (vrGetDepthOfFieldAperture == null)
		{
			return false;
		}
		if (vrGetDepthOfFieldFocalDist == null)
		{
			return false;
		}
		if (vrGetDepthOfFieldFocalDistOn == null)
		{
			return false;
		}
		if (vrGetDepthOfFieldSides == null)
		{
			return false;
		}
		if (vrGetDepthOfFieldSidesOn == null)
		{
			return false;
		}
		if (vrGetDepthOfFieldSubdivisions == null)
		{
			return false;
		}
		if (vrSetDepthOfFieldAperture == null)
		{
			return false;
		}
		if (vrSetDepthOfFieldFocalDistOn == null)
		{
			return false;
		}
		if (vrSetDepthOfFieldAperture == null)
		{
			return false;
		}
		if (vrSetDepthOfFieldFocalDistOn == null)
		{
			return false;
		}
		if (vrSetDepthOfFieldOn == null)
		{
			return false;
		}
		if (vrSetDepthOfFieldSidesOn == null)
		{
			return false;
		}
		if (vrSetDepthOfFieldSubdivision == null)
		{
			return false;
		}
		if (vrGetOptionParameterValueString == null)
		{
			return false;
		}
		if (vrGetOptionParameterValueFloat == null)
		{
			return false;
		}
		if (vrGetOptionParameterValueDouble == null)
		{
			return false;
		}
		if (vrGetOptionParameterValueInteger == null)
		{
			return false;
		}
		if (vrGetOptionParameterValueBool == null)
		{
			return false;
		}
		if (vrSetOptionParameterValueString == null)
		{
			return false;
		}
		if (vrSetOptionParameterValueFloat == null)
		{
			return false;
		}
		if (vrSetOptionParameterValueDouble == null)
		{
			return false;
		}
		if (vrSetOptionParameterValueInteger == null)
		{
			return false;
		}
		if (vrSetOptionParameterValueBool == null)
		{
			return false;
		}
		if (vrSetOffscreenRenderModality == null)
		{
			return false;
		}
		if (vrHasRenderFinished == null)
		{
			return false;
		}
		if (vrChangeMaterialSourceForObjects == null)
		{
			return false;
		}
		return true;
	}

	public static string GetVRayPluginFromRhinoKey()
	{
		string text = "C:\\ProgramData\\ASGVIS\\VRayForMatrix.rhp";
		List<string> list = new List<string>();
		string text2 = "";
		RhUtil.RhinoApp().GetRhinoRegistryKey(ref text2);
		text2 += "\\Plug-ins\\e8cfe179-b60c-411a-8416-62a893334519\\PlugIn";
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(text2);
		if (registryKey != null)
		{
			string text3 = (string)registryKey.GetValue("FileName");
			if (text3 != null)
			{
				list.Add(text3);
				registryKey.Close();
			}
		}
		text2 = "";
		RhUtil.RhinoApp().GetRhinoRegistryKey(ref text2);
		text2 += "\\Plug-ins\\e8cfe179-b60c-411a-8416-62a893334519\\PlugIn";
		registryKey = Registry.LocalMachine.OpenSubKey(text2);
		if (registryKey != null)
		{
			string text4 = (string)registryKey.GetValue("FileName");
			if (text4 != null)
			{
				list.Add(text4);
				registryKey.Close();
			}
		}
		list.Add(text);
		foreach (string item in list)
		{
			if (File.Exists(item))
			{
				return item;
			}
		}
		return text;
	}

	public static bool LoadPlugin(string path)
	{
		if (path == null)
		{
			return false;
		}
		RhUtil.GetPlugInInstance(path);
		return true;
	}
}
