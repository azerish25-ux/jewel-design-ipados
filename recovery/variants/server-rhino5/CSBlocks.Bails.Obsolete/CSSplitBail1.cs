using System;
using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using CSDatas.Interfaces;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.Bails.Obsolete;

[Obsolete]
[CSBlock("SplitBail_V1", "{C9B6A2F9-4E2C-499a-9E0C-3B987D3FB916}")]
public sealed class CSSplitBail1 : CSBlock
{
	private sealed class AngleMorph : OnSpaceMorph
	{
		private double _distance;

		private double _length;

		public override On3dPoint MorphPoint(IOn3dPoint inputPoint)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			On3dPoint val = new On3dPoint(inputPoint);
			val.x += _distance * (1.0 - Math.Cos(CSBlockUtil.CSUtilMath.ToRadians(val.y / _length * 90.0)));
			return val;
		}

		public void SetUp(double newDistance, double newLength)
		{
			_distance = newDistance;
			_length = newLength;
		}
	}

	public CSInputChannel iBail { get; }

	public CSInputChannel iAngle { get; }

	public CSOutputChannel oBail { get; }

	public CSSplitBail1(string identifier)
		: base(identifier)
	{
		iBail = new CSInputChannel(GetPropertyName(() => iBail), this, ChannelDataType.Brep, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oBail = new CSOutputChannel(GetPropertyName(() => oBail), this, ChannelDataType.Brep, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Expected O, but got Unknown
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_022c: Expected O, but got Unknown
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Expected O, but got Unknown
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Expected O, but got Unknown
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Expected O, but got Unknown
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Expected O, but got Unknown
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Expected O, but got Unknown
		CSData data = iBail.Data;
		if (data == null)
		{
			return UpdateMissing("Bail Input Missing");
		}
		OnBrep val = data.GetBrep().Convert();
		if (val == null)
		{
			return UpdateError("Bail Input Invalid Brep");
		}
		double value = 0.0;
		iAngle.GetDouble(ref value);
		value = CSBlockUtil.CSUtilMath.ToRadians(value);
		Transform value2;
		if (data is ICSTransform iCSTransform)
		{
			if (!iCSTransform.GetTransform(out value2))
			{
				value2 = Transform.Identity;
			}
		}
		else
		{
			value2 = Transform.Identity;
		}
		if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms((IOnXform)(object)value2.Convert(), out var xUp, out var xDown))
		{
			return UpdateError("Get Unscaled Transform Failed");
		}
		((OnGeometry)val).Transform((IOnXform)(object)xDown);
		OnBoundingBox val2 = new OnBoundingBox();
		if (!((OnGeometry)val).GetTightBoundingBox(ref val2))
		{
			return UpdateError("Get BoundingBox Failed");
		}
		double num = Math.Abs(val2.Max().y - val2.Min().y);
		double newDistance = -1.0 * Math.Tan(value) * num;
		AngleMorph angleMorph = new AngleMorph();
		angleMorph.SetUp(newDistance, num);
		((OnSpaceMorph)angleMorph).SetTolerance(0.05);
		OnBrep val3 = val.Duplicate();
		if (!((OnGeometry)val3).Morph((IOnSpaceMorph)(object)angleMorph))
		{
			return UpdateError("bailLhs Morph Failed");
		}
		On3dPoint val4 = new On3dPoint(0.0, val2.Min().y - 50.0, val2.Min().z - 50.0);
		On3dPoint val5 = new On3dPoint(0.0, val2.Max().y + 50.0, val2.Min().z - 50.0);
		On3dPoint val6 = new On3dPoint(0.0, val2.Max().y + 50.0, val2.Max().z + 50.0);
		On3dPoint val7 = new On3dPoint(0.0, val2.Min().y - 50.0, val2.Max().z + 50.0);
		OnBrep val8 = ((OnGeometry)RhUtil.RhinoCreateSurfaceFromCorners((IOn3dPoint)val4, (IOn3dPoint)(object)val5, ref val6, ref val7)).BrepForm();
		if (val8 == null)
		{
			return UpdateError("Intersection Failed");
		}
		OnXform val9 = new OnXform();
		val9.Mirror(OnUtil.On_origin, OnUtil.On_xaxis);
		List<IOnBrep> list = new List<IOnBrep>();
		RhUtil.RhinoRepairBrep(val3, CSApplicationHelper.Application.Tolerance);
		((OnGeometry)val3).MakeDeformable();
		OnBrep[] array = default(OnBrep[]);
		if (!RhUtil.RhinoBrepSplit((IOnBrep)(object)val3, (IOnBrep)(object)val8, CSApplicationHelper.Application.Tolerance, ref array))
		{
			return UpdateError("Brep Split Failed");
		}
		OnBrep[] array2 = array;
		IOnMesh[] array3 = default(IOnMesh[]);
		foreach (OnBrep val10 in array2)
		{
			OnMeshParameters val11 = new OnMeshParameters();
			val11.JaggedAndFasterMeshParameters();
			MRhinoBrepObject val12 = new MRhinoBrepObject();
			val12.SetBrep(val10);
			((MRhinoObject)val12).CreateMeshes((mesh_type)3, (IOnMeshParameters)(object)val11);
			((MRhinoObject)val12).GetMeshes((mesh_type)3, ref array3);
			if (array3.Length >= 1)
			{
				OnMesh val13 = new OnMesh();
				for (int j = 0; j < array3.Length; j++)
				{
					val13.Append(array3[j]);
				}
				OnBoundingBox val14 = new OnBoundingBox();
				if (((OnGeometry)val13).GetBoundingBox(ref val14) && val14.Center().x < 0.0)
				{
					list.Add((IOnBrep)(object)val10);
					OnBrep val15 = val10.Duplicate();
					((OnGeometry)val15).Transform((IOnXform)(object)val9);
					list.Add((IOnBrep)(object)val15);
				}
			}
		}
		OnBrep val16 = new OnBrep();
		if (!RhUtil.RhinoMergeBreps(list.ToArray(), CSApplicationHelper.Application.Tolerance, ref val16))
		{
			return UpdateError("Brep Merge Failed");
		}
		((OnGeometry)val16).Transform((IOnXform)(object)xUp);
		oBail.SetData(new CSOnGeometryData(val16.Convert(), xUp.Convert(), applyTransform: false));
		return base.Update();
	}
}
