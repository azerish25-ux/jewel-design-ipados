using System;
using CS;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.JSonDoc;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CSBlocks.Core;

[CSBlock("Point", "{3755895F-8907-4f49-8C19-CE370219AB28}")]
public sealed class CSPoint : CSBlock, IDisposable
{
	private On3dPoint _pointValue;

	private On3dVector _add = new On3dVector();

	private On3dVector _mul = new On3dVector();

	public On3dPoint PointValue => _pointValue;

	public CSInputChannel iX { get; }

	public CSInputChannel iY { get; }

	public CSInputChannel iZ { get; }

	public CSInputChannel iPoint { get; }

	public CSInputChannel iAddX { get; }

	public CSInputChannel iAddY { get; }

	public CSInputChannel iAddZ { get; }

	public CSInputChannel iSubX { get; }

	public CSInputChannel iSubY { get; }

	public CSInputChannel iSubZ { get; }

	public CSInputChannel iMulX { get; }

	public CSInputChannel iMulY { get; }

	public CSInputChannel iMulZ { get; }

	public CSOutputChannel oPoint { get; }

	public CSOutputChannel oX { get; }

	public CSOutputChannel oY { get; }

	public CSOutputChannel oZ { get; }

	public CSPoint(string identifier)
		: base(identifier)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		iX = new CSInputChannel(GetPropertyName(() => iX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iY = new CSInputChannel(GetPropertyName(() => iY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZ = new CSInputChannel(GetPropertyName(() => iZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPoint = new CSInputChannel(GetPropertyName(() => iPoint), this, ChannelDataType.Point, ChannelArrayType.Individual);
		iAddX = new CSInputChannel(GetPropertyName(() => iAddX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAddY = new CSInputChannel(GetPropertyName(() => iAddY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAddZ = new CSInputChannel(GetPropertyName(() => iAddZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSubX = new CSInputChannel(GetPropertyName(() => iSubX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSubY = new CSInputChannel(GetPropertyName(() => iSubY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSubZ = new CSInputChannel(GetPropertyName(() => iSubZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMulX = new CSInputChannel(GetPropertyName(() => iMulX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMulY = new CSInputChannel(GetPropertyName(() => iMulY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMulZ = new CSInputChannel(GetPropertyName(() => iMulZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oPoint = new CSOutputChannel(GetPropertyName(() => oPoint), this, ChannelDataType.Point, ChannelArrayType.Individual);
		oX = new CSOutputChannel(GetPropertyName(() => oX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oY = new CSOutputChannel(GetPropertyName(() => oY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oZ = new CSOutputChannel(GetPropertyName(() => oZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public override bool ReadJson(JObject jBlock, CSParaGraph graph, int major, int minor)
	{
		bool num = base.ReadJson(jBlock, graph, major, minor);
		_pointValue = jBlock.GetOn3dPoint("Value");
		if (num)
		{
			return _pointValue != (IOn3dPoint)null;
		}
		return false;
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		base.Read(ref archive, graph, major, minor);
		if (_pointValue == (IOn3dPoint)null)
		{
			_pointValue = new On3dPoint(0.0, 0.0, 0.0);
		}
		archive.ReadPoint(ref _pointValue);
		return true;
	}

	public override bool SetUndo(CSObject setObject)
	{
		if (!(setObject is CSPoint cSPoint))
		{
			return base.SetUndo(setObject);
		}
		_pointValue = cSPoint.PointValue;
		return base.SetUndo(setObject);
	}

	public bool SetValue(On3dPoint newValue)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		if (newValue != (IOn3dPoint)(object)_pointValue)
		{
			Change();
			_pointValue = new On3dPoint((IOn3dPoint)(object)newValue);
		}
		return true;
	}

	public override bool Update()
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Expected O, but got Unknown
		double value = 0.0;
		double value2 = 0.0;
		double value3 = 0.0;
		if (_pointValue != (IOn3dPoint)null)
		{
			value = _pointValue.x;
			value2 = _pointValue.y;
			value3 = _pointValue.z;
		}
		else
		{
			_pointValue = new On3dPoint();
		}
		On3dPoint onPoint = new On3dPoint();
		if (iPoint != null && iPoint.GetPoint(ref onPoint))
		{
			_pointValue = new On3dPoint(onPoint.x, onPoint.y, onPoint.z);
			value = _pointValue.x;
			value2 = _pointValue.y;
			value3 = _pointValue.z;
		}
		if (iX != null)
		{
			iX.GetDouble(ref value);
		}
		else
			_ = 0;
		if (iY != null)
		{
			iY.GetDouble(ref value2);
		}
		else
			_ = 0;
		if (iZ != null)
		{
			iZ.GetDouble(ref value3);
		}
		else
			_ = 0;
		double value4 = 0.0;
		double value5 = 0.0;
		double value6 = 0.0;
		if (iAddX != null)
		{
			iAddX.GetDouble(ref value4);
		}
		if (iAddY != null)
		{
			iAddY.GetDouble(ref value5);
		}
		if (iAddZ != null)
		{
			iAddZ.GetDouble(ref value6);
		}
		double value7 = 0.0;
		double value8 = 0.0;
		double value9 = 0.0;
		if (iSubX != null)
		{
			iSubX.GetDouble(ref value7);
		}
		if (iSubY != null)
		{
			iSubY.GetDouble(ref value8);
		}
		if (iSubZ != null)
		{
			iSubZ.GetDouble(ref value9);
		}
		_add.x = value4 - value7;
		_add.y = value5 - value8;
		_add.z = value6 - value9;
		double value10 = 1.0;
		double value11 = 1.0;
		double value12 = 1.0;
		if (iMulX != null)
		{
			iMulX.GetDouble(ref value10);
		}
		if (iMulY != null)
		{
			iMulY.GetDouble(ref value11);
		}
		if (iMulZ != null)
		{
			iMulZ.GetDouble(ref value12);
		}
		_mul.x = value10;
		_mul.y = value11;
		_mul.z = value12;
		_pointValue.x = value;
		_pointValue.y = value2;
		_pointValue.z = value3;
		On3dPoint val = new On3dPoint((IOn3dPoint)(object)(_pointValue + (IOn3dVector)(object)_add));
		val.x *= _mul.x;
		val.y *= _mul.y;
		val.z *= _mul.z;
		oPoint.SetData(new CSPointData(val.Convert()));
		On3dPoint val2 = new On3dPoint((IOn3dPoint)(object)(_pointValue + (IOn3dVector)(object)_add));
		oX.SetData(new CSNumberData(val2.x));
		oY.SetData(new CSNumberData(val2.y));
		oZ.SetData(new CSNumberData(val2.z));
		return base.Update();
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		archive.Add("Value", _pointValue);
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		bool num = base.Write(ref archive);
		if (_pointValue == (IOn3dPoint)null)
		{
			_pointValue = new On3dPoint(0.0, 0.0, 0.0);
		}
		return num & archive.WritePoint((IOn3dPoint)(object)_pointValue);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (_pointValue != (IOn3dPoint)null)
			{
				_pointValue.Dispose();
			}
			_pointValue = null;
			if (_add != (IOn3dVector)null)
			{
				_add.Dispose();
			}
			_add = null;
			if (_mul != (IOn3dVector)null)
			{
				_mul.Dispose();
			}
			_mul = null;
		}
	}
}
