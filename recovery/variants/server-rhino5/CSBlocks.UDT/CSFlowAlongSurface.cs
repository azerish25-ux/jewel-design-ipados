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
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected O, but got Unknown
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Expected O, but got Unknown
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Expected O, but got Unknown
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Expected O, but got Unknown
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Expected O, but got Unknown
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Expected O, but got Unknown
			On3dPoint val = new On3dPoint(OnUtil.On_origin);
			val.Transform((IOnXform)(object)form.Convert());
			new OnXform().Identity();
			double num = 0.0;
			double num2 = 0.0;
			((OnSurface)_baseSrf).GetClosestPoint((IOn3dPoint)(object)val, ref num, ref num2);
			On3dPoint val2 = new On3dPoint();
			On3dVector val3 = ((OnSurface)_baseSrf).NormalAt(num, num2);
			On3dVector val4 = new On3dVector();
			On3dVector val5 = new On3dVector();
			((OnSurface)_baseSrf).Ev1Der(num, num2, ref val2, ref val4, ref val5);
			val4.Unitize();
			val5.Unitize();
			val4 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val5, (IOn3dVector)(object)val3);
			val4.Unitize();
			double num3 = val2.DistanceTo((IOn3dPoint)(object)val);
			if (val3 * (IOn3dVector)(object)(val - (IOn3dPoint)(object)val2) < 0.0)
			{
				num3 = 0.0 - num3;
			}
			num = ((OnSurface)_baseSrf).Domain(0).NormalizedParameterAt(num);
			num2 = ((OnSurface)_baseSrf).Domain(1).NormalizedParameterAt(num2);
			num = ((OnSurface)_destSrf).Domain(0).ParameterAt(num);
			num2 = ((OnSurface)_destSrf).Domain(1).ParameterAt(num2);
			On3dPoint val6 = new On3dPoint();
			On3dVector val7 = ((OnSurface)_destSrf).NormalAt(num, num2);
			On3dVector val8 = new On3dVector();
			On3dVector val9 = new On3dVector();
			((OnSurface)_destSrf).Ev1Der(num, num2, ref val6, ref val8, ref val9);
			val8.Unitize();
			val9.Unitize();
			val8 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val9, (IOn3dVector)(object)val7);
			val8.Unitize();
			On3dPoint val10 = ((OnSurface)_destSrf).PointAt(num, num2);
			val10 += (IOn3dVector)(object)(val7 * num3);
			OnXform val11 = new OnXform();
			OnXform val12 = new OnXform();
			val11.ChangeBasis(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_yaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)val, (IOn3dVector)(object)val4, (IOn3dVector)(object)val5, (IOn3dVector)(object)val3);
			val12.ChangeBasis((IOn3dPoint)(object)val10, (IOn3dVector)(object)val8, (IOn3dVector)(object)val9, (IOn3dVector)(object)val7, OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_yaxis, OnUtil.On_zaxis);
			return (val12 * (IOnXform)(object)val11).Convert();
		}

		public override Point3d MorphPoint(Point3d point)
		{
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Expected O, but got Unknown
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Expected O, but got Unknown
			double num = 0.0;
			double num2 = 0.0;
			On3dPoint val;
			On3dVector val2;
			if (Plane != (IOnPlane)null)
			{
				PlaneLineX.ClosestPointTo((IOn3dPoint)(object)point.Convert(), ref num);
				PlaneLineY.ClosestPointTo((IOn3dPoint)(object)point.Convert(), ref num2);
				double num3 = 0.0;
				double num4 = 0.0;
				Plane.ClosestPointTo((IOn3dPoint)(object)point.Convert(), ref num3, ref num4);
				val = Plane.PointAt(num3, num4);
				val2 = new On3dVector(Plane.Normal());
				int num5 = ((OnSurface)_destSrf).IsAtSeam(((OnSurface)_destSrf).Domain(0).Min(), ((OnSurface)_destSrf).Domain(1).Mid());
				int num6 = ((OnSurface)_destSrf).IsAtSeam(((OnSurface)_destSrf).Domain(0).Mid(), ((OnSurface)_destSrf).Domain(1).Min());
				if (num5 != 0)
				{
					num += 2.0;
					num %= 1.0;
				}
				if (num6 != 0)
				{
					num2 += 2.0;
					num2 %= 1.0;
				}
			}
			else
			{
				((OnSurface)_baseSrf).GetClosestPoint((IOn3dPoint)(object)point.Convert(), ref num, ref num2);
				val = ((OnSurface)_baseSrf).PointAt(num, num2);
				val2 = ((OnSurface)_baseSrf).NormalAt(num, num2);
				num = ((OnSurface)_baseSrf).Domain(0).NormalizedParameterAt(num);
				num2 = ((OnSurface)_baseSrf).Domain(1).NormalizedParameterAt(num2);
			}
			double num7 = val.DistanceTo((IOn3dPoint)(object)point.Convert());
			if (val2 * (IOn3dVector)(object)(new On3dPoint((IOn3dPoint)(object)point.Convert()) - (IOn3dPoint)(object)val) < 0.0)
			{
				num7 = 0.0 - num7;
			}
			num = ((OnSurface)_destSrf).Domain(0).ParameterAt(num);
			num2 = ((OnSurface)_destSrf).Domain(1).ParameterAt(num2);
			On3dPoint obj = ((OnSurface)_destSrf).PointAt(num, num2);
			On3dVector val3 = ((OnSurface)_destSrf).NormalAt(num, num2);
			if (FlipZ)
			{
				num7 *= -1.0;
			}
			return (obj + (IOn3dVector)(object)(val3 * num7)).Convert();
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
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected O, but got Unknown
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Expected O, but got Unknown
		OnNurbsSurface val = iBase.GetNurbsSurface();
		if (val == null)
		{
			OnBrep brep = iBase.GetBrep();
			if (brep != null && brep.m_F.Count() > 0)
			{
				val = ((OnSurface)brep.m_F[0]).NurbsSurface();
			}
		}
		if (val == null)
		{
			return UpdateMissing("Missing Src");
		}
		OnNurbsSurface val2 = iDestination.GetNurbsSurface();
		if (val2 == null)
		{
			CSDataArray flattenArray = iDestination.GetFlattenArray();
			if (flattenArray.GetArrayCount() > 0)
			{
				OnBrep val3 = flattenArray.GetArrayItem(0).GetBrep().Convert();
				if (val3 != null && val3.m_F.Count() > 0)
				{
					val2 = ((OnSurface)val3.m_F[0]).NurbsSurface();
				}
			}
		}
		if (val2 == null)
		{
			return UpdateMissing("Missing Dest");
		}
		bool value = false;
		iSwapUV.GetBoolean(ref value);
		if (value)
		{
			((OnSurface)val).Transpose();
		}
		bool value2 = false;
		iFlipU.GetBoolean(ref value2);
		if (value2)
		{
			((OnSurface)val).Reverse(0);
		}
		bool value3 = false;
		iFlipV.GetBoolean(ref value3);
		if (value3)
		{
			((OnSurface)val).Reverse(1);
		}
		CSFlowAlongSurfaceMorph cSFlowAlongSurfaceMorph = new CSFlowAlongSurfaceMorph(val, val2);
		OnPlane val4 = new OnPlane();
		if (((OnSurface)val).IsPlanar(val4))
		{
			val4 = new OnPlane((IOn3dPoint)(object)((OnSurface)val).PointAt(((OnSurface)val).Domain(0).Min(), ((OnSurface)val).Domain(1).Min()), (IOn3dPoint)(object)((OnSurface)val).PointAt(((OnSurface)val).Domain(0).Max(), ((OnSurface)val).Domain(1).Min()), (IOn3dPoint)(object)((OnSurface)val).PointAt(((OnSurface)val).Domain(0).Min(), ((OnSurface)val).Domain(1).Max()));
			cSFlowAlongSurfaceMorph.PlaneLineX = new OnLine((IOn3dPoint)(object)((OnSurface)val).PointAt(((OnSurface)val).Domain(0).Max(), ((OnSurface)val).Domain(1).Min()), (IOn3dPoint)(object)((OnSurface)val).PointAt(((OnSurface)val).Domain(0).Min(), ((OnSurface)val).Domain(1).Min()));
			cSFlowAlongSurfaceMorph.PlaneLineY = new OnLine((IOn3dPoint)(object)((OnSurface)val).PointAt(((OnSurface)val).Domain(0).Min(), ((OnSurface)val).Domain(1).Max()), (IOn3dPoint)(object)((OnSurface)val).PointAt(((OnSurface)val).Domain(0).Min(), ((OnSurface)val).Domain(1).Min()));
			cSFlowAlongSurfaceMorph.Plane = val4;
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
