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
			On3dPoint on3dPoint = new On3dPoint(inputPoint);
			on3dPoint.x += _distance * (1.0 - Math.Cos(CSBlockUtil.CSUtilMath.ToRadians(on3dPoint.y / _length * 90.0)));
			return on3dPoint;
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
		CSData data = iBail.Data;
		if (data == null)
		{
			return UpdateMissing("Bail Input Missing");
		}
		OnBrep onBrep = data.GetBrep().Convert();
		if (onBrep == null)
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
		if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(value2.Convert(), out var xUp, out var xDown))
		{
			return UpdateError("Get Unscaled Transform Failed");
		}
		onBrep.Transform(xDown);
		OnBoundingBox tight_bbox = new OnBoundingBox();
		if (!onBrep.GetTightBoundingBox(ref tight_bbox))
		{
			return UpdateError("Get BoundingBox Failed");
		}
		double num = Math.Abs(tight_bbox.Max().y - tight_bbox.Min().y);
		double newDistance = -1.0 * Math.Tan(value) * num;
		AngleMorph angleMorph = new AngleMorph();
		angleMorph.SetUp(newDistance, num);
		angleMorph.SetTolerance(0.05);
		OnBrep onBrep2 = onBrep.Duplicate();
		if (!onBrep2.Morph(angleMorph))
		{
			return UpdateError("bailLhs Morph Failed");
		}
		On3dPoint a = new On3dPoint(0.0, tight_bbox.Min().y - 50.0, tight_bbox.Min().z - 50.0);
		On3dPoint b = new On3dPoint(0.0, tight_bbox.Max().y + 50.0, tight_bbox.Min().z - 50.0);
		On3dPoint c = new On3dPoint(0.0, tight_bbox.Max().y + 50.0, tight_bbox.Max().z + 50.0);
		On3dPoint d = new On3dPoint(0.0, tight_bbox.Min().y - 50.0, tight_bbox.Max().z + 50.0);
		OnBrep onBrep3 = RhUtil.RhinoCreateSurfaceFromCorners(a, b, ref c, ref d).BrepForm();
		if (onBrep3 == null)
		{
			return UpdateError("Intersection Failed");
		}
		OnXform onXform = new OnXform();
		onXform.Mirror(OnUtil.On_origin, OnUtil.On_xaxis);
		List<IOnBrep> list = new List<IOnBrep>();
		RhUtil.RhinoRepairBrep(onBrep2, CSApplicationHelper.Application.Tolerance);
		onBrep2.MakeDeformable();
		if (!RhUtil.RhinoBrepSplit(onBrep2, onBrep3, CSApplicationHelper.Application.Tolerance, out var pieces))
		{
			return UpdateError("Brep Split Failed");
		}
		OnBrep[] array = pieces;
		foreach (OnBrep onBrep4 in array)
		{
			OnMeshParameters onMeshParameters = new OnMeshParameters();
			onMeshParameters.JaggedAndFasterMeshParameters();
			MRhinoBrepObject mRhinoBrepObject = new MRhinoBrepObject();
			mRhinoBrepObject.SetBrep(onBrep4);
			mRhinoBrepObject.CreateMeshes(IOn.mesh_type.preview_mesh, onMeshParameters);
			mRhinoBrepObject.GetMeshes(IOn.mesh_type.preview_mesh, out var meshes);
			if (meshes.Length >= 1)
			{
				OnMesh onMesh = new OnMesh();
				for (int j = 0; j < meshes.Length; j++)
				{
					onMesh.Append(meshes[j]);
				}
				OnBoundingBox bbox = new OnBoundingBox();
				if (onMesh.GetBoundingBox(ref bbox) && bbox.Center().x < 0.0)
				{
					list.Add(onBrep4);
					OnBrep onBrep5 = onBrep4.Duplicate();
					onBrep5.Transform(onXform);
					list.Add(onBrep5);
				}
			}
		}
		OnBrep MergedBrep = new OnBrep();
		if (!RhUtil.RhinoMergeBreps(list.ToArray(), CSApplicationHelper.Application.Tolerance, ref MergedBrep))
		{
			return UpdateError("Brep Merge Failed");
		}
		MergedBrep.Transform(xUp);
		oBail.SetData(new CSOnGeometryData(MergedBrep.Convert(), xUp.Convert(), applyTransform: false));
		return base.Update();
	}
}
