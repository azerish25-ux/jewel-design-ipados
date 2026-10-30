using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CS.Utilities.Helpers;

public static class NetfabbHelper
{
	public delegate int NetfabbWrappedStatusCallback(IntPtr contextPtr, uint progress, [MarshalAs(UnmanagedType.LPStr)] string message);

	public delegate void StatusCallback(int progress, string message);

	private const string AuthKey = "HadqHfPrKFt9CT5HWVc2uPEEj7dUt4Bm";

	private const string LicenseKey = "7XSjdv2dJaCXAW/2CsH6SbyO7pKb358QO1GWaDAGnbiXQVHivwgoZwFROVvDKgefFkhF/bjFTPgRXFm1VgTtfkQVT9dSJwuVpp2UgjFP3enBvpg5V0klW00OQ4bG3RpZ5EGQqiEXgfnf2pm5c5QxvdUKgY9QKoZZ91xOIXXVv+VBBkysTEExcJyd9wvaWovP+MgUIW2dwLlfZsk2kpxgTskb7ZeESlIQkQEG0+zEBvlYhtyiElG2romesw4RT44MM8BPeOURZ7SeQJzj5UVkIVWBxJrW8BtE10AFzPHdzSQeD1BhmhDpbVxCb59LCp5bbozLHD1T43hK0Vn1JVDr253RBMfdc9KLIEzAIa1RCDnEnEH7jcSl48RSeMqVmJ6WYMQWZBbW8qwUkgi4ZdD/HcxcvpMHvBhQVUEM+i0ay3QerEkojZR7WE1Wm1pCKBfdsgRDWv2OvC1KmJw/RQT1gFCQqxdRZk4c5dlRpOjR1D1XrhAzpFG8ExTGeEaRoocAE5Eadu1Vbb0WUoV/74xiKc0bfo9VdweXv02JlK2WEHDQTBUvjpj+bhDW9Fvb3ccUYYRSJ+BbOGCOdcQvp1hqpUmGgotE69Mbv1Fbgmzbn2CUMs22XVDwYAHZ30IZv4fH8RHJrHEQ+/HbMsDm0Zy8tBlCysqaNJ9bQhUG7mjTPy1V48wqkBx9NMATZtdE4spdDQQPVnnazKWVPZFrpZyn9dGFNA+FOIJEVJVCT+GAqTEdYl0tvxTpaMjLGEtBQE/BoMBFUWUeQbUYIV15tZE6bxEOC8pCUkMFNYB/oJGth6ouSAg128GvXB2ImXvzCN0/4rwT/egjXOBb0tE+vN7PFlFuUHwk7T1Z5a32yuWvzaabOqSF9XE5mzQHrPxkIdT3Szx/b82/pNWNo/18toJM/LxDkqdp5LcUeWmGkKf7VkyXf1OiWgKMl3ELSrt+K1ECwJq6g5Srf+SnFyZ2rpV5ykJBwmbIg+KdfLY//FROQoLh7KVT/Jy5fN4eUO7wxYYaSRFgF2UXjmvn1+ZrLCdtyAGASM9WDHrOShCbzrhUluvqkldEXDngNeHpnwTT0yffApeqx9RaSsbxiChJKbnoKOzCSEiy519bmMxodq5ZG1yRBD/dJbEjdXnUSeck7uIi7Tya9MX8K7op/HTqpdXikAAA";

	private static readonly string LibraryName;

	private static readonly LoadLibraryProxy NetfabbProxy;

	public static NetFabError LastNetFabError;

	private static IntPtr _netfabbContext;

	public static event ProgressChangedEventHandler NetfabbProgressChanged;

	static NetfabbHelper()
	{
		LibraryName = Path.Combine(CSApplicationHelper.Application.CSMeshRepairPath, "gemvision_helper64.dll");
		NetfabbProxy = new LoadLibraryProxy(Path.Combine(CSApplicationHelper.Application.CSMeshRepairPath, "gemvision_netfabbmanaged.dll"));
		int num = Init();
		if (Enum.IsDefined(typeof(NetFabError), num))
		{
			CSUtil.PrintLine((num == 0) ? "Initialization of Mesh Repair Engine Result:  SUCCESSFUL" : $"Initialization of Mesh Repair Engine Result:  ERROR {(NetFabError)num}");
		}
		else
		{
			CSUtil.PrintLine($"Initialization of Mesh Repair Engine Result:  ERROR {num}");
		}
		SetCallbackFunction(CallbackTarget);
	}

	public unsafe static bool NetfabbBuildMesh(Mesh mesh, bool doExtendedRepair, out Mesh meshRepaired)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		meshRepaired = null;
		LastNetFabError = NetFabError.NONE;
		if (!mesh.Faces.ConvertQuadsToTriangles())
		{
			CSUtil.PrintLine("Mesh Repair Engine Failed: Unabled to convert quads to triangles");
			return false;
		}
		uint pointCount = Convert.ToUInt32(mesh.Vertices.Count);
		List<float> list = new List<float>();
		foreach (Point3f vertex in mesh.Vertices)
		{
			Point3f current = vertex;
			list.Add(((Point3f)(ref current)).X);
			list.Add(((Point3f)(ref current)).Y);
			list.Add(((Point3f)(ref current)).Z);
		}
		float[] array = list.ToArray();
		uint faceCount = Convert.ToUInt32(mesh.Faces.Count);
		List<int> list2 = new List<int>();
		foreach (MeshFace face in mesh.Faces)
		{
			MeshFace current2 = face;
			if (((MeshFace)(ref current2)).IsQuad)
			{
				list2.Add(((MeshFace)(ref current2)).A);
				list2.Add(((MeshFace)(ref current2)).B);
				list2.Add(((MeshFace)(ref current2)).C);
				list2.Add(((MeshFace)(ref current2)).D);
			}
			else
			{
				list2.Add(((MeshFace)(ref current2)).A);
				list2.Add(((MeshFace)(ref current2)).B);
				list2.Add(((MeshFace)(ref current2)).C);
			}
		}
		int[] array2 = list2.ToArray();
		IntPtr netfabbContext = _netfabbContext;
		List<float> list3;
		List<int> list4;
		fixed (float* positions = array)
		{
			fixed (int* faceIndices = array2)
			{
				uint returnPointCount;
				uint returnFaceCount;
				uint returnPositionsCount;
				uint returnFaceIndicesCount;
				float* returnPositions;
				int* returnFaceIndices;
				int num = netfabbManagedBuildMesh(netfabbContext, pointCount, faceCount, positions, faceIndices, Convert.ToInt32(doExtendedRepair), out returnPointCount, out returnFaceCount, out returnPositionsCount, out returnFaceIndicesCount, out returnPositions, out returnFaceIndices);
				if (num > 0)
				{
					LastNetFabError = (NetFabError)num;
					CSUtil.PrintLine($"Mesh Repair Engine Failed: ERROR {LastNetFabError}");
					return false;
				}
				list3 = ConvertPointerToListFloat(new IntPtr(returnPositions), returnPositionsCount * 3);
				list4 = ConvertPointerToListInt(new IntPtr(returnFaceIndices), returnFaceIndicesCount * 3);
				freeIntBuffer(returnFaceIndices);
				freeFloatBuffer(returnPositions);
			}
		}
		meshRepaired = new Mesh();
		for (int i = 0; i < list3.Count; i += 3)
		{
			float num2 = list3[i];
			float num3 = list3[i + 1];
			float num4 = list3[i + 2];
			meshRepaired.Vertices.Add(num2, num3, num4);
		}
		for (int j = 0; j < list4.Count; j += 3)
		{
			int num5 = list4[j];
			int num6 = list4[j + 1];
			int num7 = list4[j + 2];
			meshRepaired.Faces.AddFace(num5, num6, num7);
		}
		meshRepaired.Compact();
		if (((CommonObject)meshRepaired).IsValid)
		{
			return true;
		}
		CSUtil.PrintLine("Mesh Repair Engine Failed: Repaired mesh is invalid");
		return false;
	}

	private static void CallbackTarget(int progress, string message)
	{
		OnNetfabbProgressChanged(new ProgressChangedEventArgs(progress, message));
	}

	private static List<float> ConvertPointerToListFloat(IntPtr source, uint sourceLength)
	{
		float[] array = new float[sourceLength];
		try
		{
			Marshal.Copy(source, array, 0, Convert.ToInt32(sourceLength));
		}
		catch (Exception ex)
		{
			CSUtil.PrintLine(string.Format(ex.ToString()));
		}
		return array.ToList();
	}

	private static List<int> ConvertPointerToListInt(IntPtr source, uint sourceLength)
	{
		int[] array = new int[sourceLength];
		try
		{
			Marshal.Copy(source, array, 0, Convert.ToInt32(sourceLength));
		}
		catch (Exception ex)
		{
			CSUtil.PrintLine(string.Format(ex.ToString()));
		}
		return array.ToList();
	}

	[DllImport("gemvision_netfabbmanaged.dll", CallingConvention = CallingConvention.Cdecl)]
	private unsafe static extern void freeFloatBuffer(float* floatBuffer);

	[DllImport("gemvision_netfabbmanaged.dll", CallingConvention = CallingConvention.Cdecl)]
	private unsafe static extern void freeIntBuffer(int* intBuffer);

	private unsafe static int Init()
	{
		CSUtil.PrintLine($"Initializing Mesh Repair Engine with Authentication file - {LibraryName}");
		IntPtr zero = IntPtr.Zero;
		byte[] bytes = Encoding.Unicode.GetBytes("HadqHfPrKFt9CT5HWVc2uPEEj7dUt4Bm");
		byte[] array = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, bytes);
		byte[] array2 = Encoding.Convert(bytes: Encoding.Unicode.GetBytes("7XSjdv2dJaCXAW/2CsH6SbyO7pKb358QO1GWaDAGnbiXQVHivwgoZwFROVvDKgefFkhF/bjFTPgRXFm1VgTtfkQVT9dSJwuVpp2UgjFP3enBvpg5V0klW00OQ4bG3RpZ5EGQqiEXgfnf2pm5c5QxvdUKgY9QKoZZ91xOIXXVv+VBBkysTEExcJyd9wvaWovP+MgUIW2dwLlfZsk2kpxgTskb7ZeESlIQkQEG0+zEBvlYhtyiElG2romesw4RT44MM8BPeOURZ7SeQJzj5UVkIVWBxJrW8BtE10AFzPHdzSQeD1BhmhDpbVxCb59LCp5bbozLHD1T43hK0Vn1JVDr253RBMfdc9KLIEzAIa1RCDnEnEH7jcSl48RSeMqVmJ6WYMQWZBbW8qwUkgi4ZdD/HcxcvpMHvBhQVUEM+i0ay3QerEkojZR7WE1Wm1pCKBfdsgRDWv2OvC1KmJw/RQT1gFCQqxdRZk4c5dlRpOjR1D1XrhAzpFG8ExTGeEaRoocAE5Eadu1Vbb0WUoV/74xiKc0bfo9VdweXv02JlK2WEHDQTBUvjpj+bhDW9Fvb3ccUYYRSJ+BbOGCOdcQvp1hqpUmGgotE69Mbv1Fbgmzbn2CUMs22XVDwYAHZ30IZv4fH8RHJrHEQ+/HbMsDm0Zy8tBlCysqaNJ9bQhUG7mjTPy1V48wqkBx9NMATZtdE4spdDQQPVnnazKWVPZFrpZyn9dGFNA+FOIJEVJVCT+GAqTEdYl0tvxTpaMjLGEtBQE/BoMBFUWUeQbUYIV15tZE6bxEOC8pCUkMFNYB/oJGth6ouSAg128GvXB2ImXvzCN0/4rwT/egjXOBb0tE+vN7PFlFuUHwk7T1Z5a32yuWvzaabOqSF9XE5mzQHrPxkIdT3Szx/b82/pNWNo/18toJM/LxDkqdp5LcUeWmGkKf7VkyXf1OiWgKMl3ELSrt+K1ECwJq6g5Srf+SnFyZ2rpV5ykJBwmbIg+KdfLY//FROQoLh7KVT/Jy5fN4eUO7wxYYaSRFgF2UXjmvn1+ZrLCdtyAGASM9WDHrOShCbzrhUluvqkldEXDngNeHpnwTT0yffApeqx9RaSsbxiChJKbnoKOzCSEiy519bmMxodq5ZG1yRBD/dJbEjdXnUSeck7uIi7Tya9MX8K7op/HTqpdXikAAA"), srcEncoding: Encoding.Unicode, dstEncoding: Encoding.UTF8);
		byte[] array3 = Encoding.Convert(bytes: Encoding.Unicode.GetBytes(LibraryName), srcEncoding: Encoding.Unicode, dstEncoding: Encoding.UTF8);
		int result;
		fixed (byte* authkey = array)
		{
			fixed (byte* licensekey = array2)
			{
				fixed (byte* dllname = array3)
				{
					result = netfabbManagedInit(authkey, licensekey, dllname, &zero);
					_netfabbContext = zero;
				}
			}
		}
		return result;
	}

	[DllImport("gemvision_netfabbmanaged.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern int netabbManagedSetStatusCallback(IntPtr contextPtr, NetfabbWrappedStatusCallback functionCallback);

	[DllImport("gemvision_netfabbmanaged.dll", CallingConvention = CallingConvention.Cdecl)]
	private unsafe static extern int netfabbManagedBuildMesh(IntPtr conterPtr, uint pointCount, uint faceCount, float* positions, int* faceIndices, int doExtendedRepair, out uint returnPointCount, out uint returnFaceCount, out uint returnPositionsCount, out uint returnFaceIndicesCount, out float* returnPositions, out int* returnFaceIndices);

	[DllImport("gemvision_netfabbmanaged.dll", CallingConvention = CallingConvention.Cdecl)]
	private unsafe static extern int netfabbManagedInit(byte* authkey, byte* licensekey, byte* dllname, IntPtr* contextPtr);

	private static void OnNetfabbProgressChanged(ProgressChangedEventArgs e)
	{
		NetfabbHelper.NetfabbProgressChanged?.Invoke(null, e);
	}

	private static void SetCallbackFunction(StatusCallback callbackTarget)
	{
	}
}
