using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.UDT;

[CSBlock("FlowAlongSurface", "{31B25588-EB2B-473d-A655-F61C82201D79}")]
public sealed class CSFlowAlongSurface : CSBlock
{
	private sealed class CSFlowAlongSurfaceMorph : CSSpaceMorph
	{
		private readonly OnNurbsSurface _baseSrf;

		private readonly OnNurbsSurface _destSrf;

		public bool FlipZ;

		public OnPlane Plane;

		public OnLine PlaneLineX;

		public OnLine PlaneLineY;

		public CSFlowAlongSurfaceMorph(OnNurbsSurface b, OnNurbsSurface d)
		{
			_baseSrf = b;
			_destSrf = d;
		}

		public override bool IsValid()
		{
			if (_baseSrf == null)
			{
				return false;
			}
			if (_destSrf == null)
			{
				return false;
			}
			return true;
		}

		public override Transform MorphFixed(Transform form)
		{
			On3dPoint on3dPoint = new On3dPoint(OnUtil.On_origin);
			on3dPoint.Transform(form.Convert());
			new OnXform().Identity();
			double s = 0.0;
			double t = 0.0;
			_baseSrf.GetClosestPoint(on3dPoint, ref s, ref t);
			On3dPoint point = new On3dPoint();
			On3dVector on3dVector = _baseSrf.NormalAt(s, t);
			On3dVector du = new On3dVector();
			On3dVector dv = new On3dVector();
			_baseSrf.Ev1Der(s, t, ref point, ref du, ref dv);
			du.Unitize();
			dv.Unitize();
			du = OnUtil.ON_CrossProduct(dv, on3dVector);
			du.Unitize();
			double num = point.DistanceTo(on3dPoint);
			if (on3dVector * (on3dPoint - point) < 0.0)
			{
				num = 0.0 - num;
			}
			s = _baseSrf.Domain(0).NormalizedParameterAt(s);
			t = _baseSrf.Domain(1).NormalizedParameterAt(t);
			s = _destSrf.Domain(0).ParameterAt(s);
			t = _destSrf.Domain(1).ParameterAt(t);
			On3dPoint point2 = new On3dPoint();
			On3dVector on3dVector2 = _destSrf.NormalAt(s, t);
			On3dVector du2 = new On3dVector();
			On3dVector dv2 = new On3dVector();
			_destSrf.Ev1Der(s, t, ref point2, ref du2, ref dv2);
			du2.Unitize();
			dv2.Unitize();
			du2 = OnUtil.ON_CrossProduct(dv2, on3dVector2);
			du2.Unitize();
			On3dPoint p = _destSrf.PointAt(s, t);
			p += (IOn3dVector)(on3dVector2 * num);
			OnXform onXform = new OnXform();
			OnXform onXform2 = new OnXform();
			onXform.ChangeBasis(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_yaxis, OnUtil.On_zaxis, on3dPoint, du, dv, on3dVector);
			onXform2.ChangeBasis(p, du2, dv2, on3dVector2, OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_yaxis, OnUtil.On_zaxis);
			return (onXform2 * onXform).Convert();
		}

		public override Point3d MorphPoint(Point3d point)
		{
			double s = 0.0;
			double t = 0.0;
			On3dPoint on3dPoint;
			On3dVector on3dVector;
			if (Plane != null)
			{
				PlaneLineX.ClosestPointTo(point.Convert(), ref s);
				PlaneLineY.ClosestPointTo(point.Convert(), ref t);
				double u = 0.0;
				double v = 0.0;
				Plane.ClosestPointTo(point.Convert(), ref u, ref v);
				on3dPoint = Plane.PointAt(u, v);
				on3dVector = new On3dVector(Plane.Normal());
				int num = _destSrf.IsAtSeam(_destSrf.Domain(0).Min(), _destSrf.Domain(1).Mid());
				int num2 = _destSrf.IsAtSeam(_destSrf.Domain(0).Mid(), _destSrf.Domain(1).Min());
				if (num != 0)
				{
					s += 2.0;
					s %= 1.0;
				}
				if (num2 != 0)
				{
					t += 2.0;
					t %= 1.0;
				}
			}
			else
			{
				_baseSrf.GetClosestPoint(point.Convert(), ref s, ref t);
				on3dPoint = _baseSrf.PointAt(s, t);
				on3dVector = _baseSrf.NormalAt(s, t);
				s = _baseSrf.Domain(0).NormalizedParameterAt(s);
				t = _baseSrf.Domain(1).NormalizedParameterAt(t);
			}
			double num3 = on3dPoint.DistanceTo(point.Convert());
			if (on3dVector * (new On3dPoint(point.Convert()) - on3dPoint) < 0.0)
			{
				num3 = 0.0 - num3;
			}
			s = _destSrf.Domain(0).ParameterAt(s);
			t = _destSrf.Domain(1).ParameterAt(t);
			On3dPoint on3dPoint2 = _destSrf.PointAt(s, t);
			On3dVector on3dVector2 = _destSrf.NormalAt(s, t);
			if (FlipZ)
			{
				num3 *= -1.0;
			}
			return (on3dPoint2 + on3dVector2 * num3).Convert();
		}
	}

	public CSInputChannel iBase { get; }

	public CSInputChannel iDestination { get; }

	public CSInputChannel iObjects { get; }

	public CSInputChannel iFlipZ { get; }

	public CSInputChannel iSwapUV { get; }

	public CSInputChannel iFlipU { get; }

	public CSInputChannel iFlipV { get; }

	public CSOutputChannel oObjects { get; }

	public CSFlowAlongSurface(string identifier)
		: base(identifier)
	{
		iBase = new CSInputChannel(GetPropertyName(() => iBase), this, ChannelDataType.Any, ChannelArrayType.Individual);
		iDestination = new CSInputChannel(GetPropertyName(() => iDestination), this, ChannelDataType.Surface, ChannelArrayType.Array | ChannelArrayType.Individual);
		iObjects = new CSInputChannel(GetPropertyName(() => iObjects), this, ChannelDataType.Any, ChannelArrayType.Any);
		iFlipZ = new CSInputChannel(GetPropertyName(() => iFlipZ), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iSwapUV = new CSInputChannel(GetPropertyName(() => iSwapUV), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iFlipU = new CSInputChannel(GetPropertyName(() => iFlipU), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iFlipV = new CSInputChannel(GetPropertyName(() => iFlipV), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oObjects = new CSOutputChannel(GetPropertyName(() => oObjects), this, ChannelDataType.Any, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		OnNurbsSurface onNurbsSurface = iBase.GetNurbsSurface();
		if (onNurbsSurface == null)
		{
			OnBrep brep = iBase.GetBrep();
			if (brep != null && brep.m_F.Count() > 0)
			{
				onNurbsSurface = brep.m_F[0].NurbsSurface();
			}
		}
		if (onNurbsSurface == null)
		{
			return UpdateMissing("Missing Src");
		}
		OnNurbsSurface onNurbsSurface2 = iDestination.GetNurbsSurface();
		if (onNurbsSurface2 == null)
		{
			CSDataArray flattenArray = iDestination.GetFlattenArray();
			if (flattenArray.GetArrayCount() > 0)
			{
				OnBrep onBrep = flattenArray.GetArrayItem(0).GetBrep().Convert();
				if (onBrep != null && onBrep.m_F.Count() > 0)
				{
					onNurbsSurface2 = onBrep.m_F[0].NurbsSurface();
				}
			}
		}
		if (onNurbsSurface2 == null)
		{
			return UpdateMissing("Missing Dest");
		}
		bool value = false;
		iSwapUV.GetBoolean(ref value);
		if (value)
		{
			onNurbsSurface.Transpose();
		}
		bool value2 = false;
		iFlipU.GetBoolean(ref value2);
		if (value2)
		{
			onNurbsSurface.Reverse(0);
		}
		bool value3 = false;
		iFlipV.GetBoolean(ref value3);
		if (value3)
		{
			onNurbsSurface.Reverse(1);
		}
		CSFlowAlongSurfaceMorph cSFlowAlongSurfaceMorph = new CSFlowAlongSurfaceMorph(onNurbsSurface, onNurbsSurface2);
		OnPlane plane = new OnPlane();
		if (onNurbsSurface.IsPlanar(plane))
		{
			plane = new OnPlane(onNurbsSurface.PointAt(onNurbsSurface.Domain(0).Min(), onNurbsSurface.Domain(1).Min()), onNurbsSurface.PointAt(onNurbsSurface.Domain(0).Max(), onNurbsSurface.Domain(1).Min()), onNurbsSurface.PointAt(onNurbsSurface.Domain(0).Min(), onNurbsSurface.Domain(1).Max()));
			cSFlowAlongSurfaceMorph.PlaneLineX = new OnLine(onNurbsSurface.PointAt(onNurbsSurface.Domain(0).Max(), onNurbsSurface.Domain(1).Min()), onNurbsSurface.PointAt(onNurbsSurface.Domain(0).Min(), onNurbsSurface.Domain(1).Min()));
			cSFlowAlongSurfaceMorph.PlaneLineY = new OnLine(onNurbsSurface.PointAt(onNurbsSurface.Domain(0).Min(), onNurbsSurface.Domain(1).Max()), onNurbsSurface.PointAt(onNurbsSurface.Domain(0).Min(), onNurbsSurface.Domain(1).Min()));
			cSFlowAlongSurfaceMorph.Plane = plane;
		}
		cSFlowAlongSurfaceMorph.FlipZ = false;
		iFlipZ.GetBoolean(ref cSFlowAlongSurfaceMorph.FlipZ);
		if (!cSFlowAlongSurfaceMorph.IsValid())
		{
			return UpdateError("Invalid Morph");
		}
		CSData data = iObjects.Data;
		if (data == null)
		{
			return UpdateError("Data invalid");
		}
		oObjects.SetData(data.Duplicate());
		if (oObjects.Data == null)
		{
			return UpdateError("Data Duplicate invalid");
		}
		cSFlowAlongSurfaceMorph.Tolerance = 0.05;
		oObjects.Data.Morph(cSFlowAlongSurfaceMorph);
		if (cSFlowAlongSurfaceMorph.FlipZ)
		{
			CSDataHelper.Flip(oObjects.Data);
		}
		return base.Update();
	}
}
