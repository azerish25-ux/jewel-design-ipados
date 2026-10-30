using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using CS.CSObjects;

namespace CS;

public sealed class CSUndoTable
{
	private readonly List<CUndoItem> _redoList = new List<CUndoItem>();

	private readonly List<CUndoItem> _undoList = new List<CUndoItem>();

	private bool _isDisabled;

	private bool _isPaused;

	private string _passphrase = string.Empty;

	private int _transactionLevel;

	private int _undoNumber;

	public DateTime DateTimeOfFirstRedo
	{
		get
		{
			if (_redoList.Count == 0 && _undoList.Count == 0)
			{
				return DateTime.Now;
			}
			if (_redoList.Count == 0)
			{
				return _undoList[_undoList.Count - 1].DatetimeObjectCreated;
			}
			return _redoList[0].DatetimeObjectCreated;
		}
	}

	public bool IsUndoBufferEmpty => !_undoList.Any();

	public bool IsRedoBufferEmpty => !_redoList.Any();

	public bool IsUIUpdateRequiredForLastAction { get; set; }

	public int BeginUndoRecord([CallerMemberName] string callingMember = "")
	{
		int result = -1;
		if (!_isPaused)
		{
			if (_transactionLevel == 0)
			{
				_undoNumber++;
			}
			_transactionLevel++;
			while (_undoList.Count > 1000)
			{
				_undoList.Remove(_undoList[0]);
			}
			_redoList.Clear();
			result = _undoNumber;
		}
		return result;
	}

	public bool DisableAndLock(bool disable, string passphrase)
	{
		bool result = false;
		if (!disable)
		{
			if (_passphrase.Trim().ToLower() == passphrase.Trim().ToLower())
			{
				_isDisabled = false;
				_isPaused = false;
				_passphrase = string.Empty;
				result = true;
			}
		}
		else if (!_isDisabled)
		{
			_isDisabled = true;
			_isPaused = true;
			_passphrase = passphrase;
			result = true;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public void EndUndoRecord([CallerMemberName] string callingMember = "")
	{
		if (_isPaused || _transactionLevel <= 0)
		{
			return;
		}
		_transactionLevel--;
		if (_transactionLevel != 0)
		{
			return;
		}
		if (_undoList.Count == 0)
		{
			_undoNumber = 0;
		}
		else if (!_undoList.Any((CUndoItem _) => _.UndoNumber == _undoNumber))
		{
			_undoNumber = _undoList.Max((CUndoItem _) => _.UndoNumber);
		}
	}

	[Conditional("DEBUG")]
	public void GetRedoListState()
	{
		_redoList.GroupBy((CUndoItem _) => _.RedoNumber, (int redoNumber, IEnumerable<CUndoItem> transactions) => new
		{
			RedoNumber = redoNumber,
			Transactions = transactions
		}).ToList().ForEach(_ =>
		{
		});
	}

	[Conditional("DEBUG")]
	public void GetUndoListState()
	{
		_undoList.GroupBy((CUndoItem _) => _.UndoNumber, (int undoNumber, IEnumerable<CUndoItem> transactions) => new
		{
			UndoNumber = undoNumber,
			Transactions = transactions
		}).ToList().ForEach(_ =>
		{
		});
	}

	public void Pause(bool isPausing)
	{
		if (!_isDisabled)
		{
			_isPaused = isPausing;
		}
	}

	public bool RecordObjectStateInUndoList(CSObject csObject, string callingMember = "")
	{
		if (_transactionLevel > 0 && !_isPaused)
		{
			int num = _undoList.Count - 1;
			while (num >= 0 && _undoList[num].UndoNumber == _undoNumber)
			{
				if (_undoList[num].GraphObject == csObject)
				{
					return true;
				}
				num--;
			}
			_undoList.Add(new CUndoItem(_undoNumber, csObject, DateTime.Now, callingMember.Trim().ToLowerInvariant()));
			return true;
		}
		return false;
	}

	public bool RedoLast()
	{
		if (!_isDisabled)
		{
			_transactionLevel = 0;
			if (_redoList.Count == 0)
			{
				return false;
			}
			_undoNumber++;
			IsUIUpdateRequiredForLastAction = false;
			int num = _redoList.Count - 1;
			while (num >= 0 && _redoList[num].UndoNumber < _undoNumber)
			{
				if (!_redoList[num].Source.Contains("setvalue"))
				{
					IsUIUpdateRequiredForLastAction = true;
				}
				_undoList.Add(new CUndoItem(_undoNumber, _redoList[num].GraphObject, _redoList[num].DatetimeObjectCreated, _redoList[num].Source));
				_redoList[num].GraphObject.SetUndo(_redoList[num].UndoObject);
				_redoList.Remove(_redoList[num]);
				num--;
			}
			return true;
		}
		return false;
	}

	public void Reset()
	{
		_undoNumber = 0;
		_transactionLevel = 0;
		_isDisabled = false;
		_passphrase = string.Empty;
		_undoList.Clear();
		_redoList.Clear();
	}

	public bool UndoLast(int transactionNumber = -1)
	{
		if (!_isDisabled)
		{
			if (transactionNumber >= 0 && !CheckForMatchingUndoTransactions(transactionNumber))
			{
				return false;
			}
			_transactionLevel = 0;
			if (_undoList.Count == 0)
			{
				return false;
			}
			_undoNumber--;
			IsUIUpdateRequiredForLastAction = false;
			int num = _undoList.Count - 1;
			while (num >= 0 && _undoList[num].UndoNumber > _undoNumber)
			{
				if (!_undoList[num].Source.Contains("setvalue"))
				{
					IsUIUpdateRequiredForLastAction = true;
				}
				_redoList.Add(new CUndoItem(_undoNumber, _undoList[num].GraphObject, _undoList[num].DatetimeObjectCreated, _undoList[num].Source));
				_undoList[num].GraphObject.SetUndo(_undoList[num].UndoObject);
				_undoList.Remove(_undoList[num]);
				num--;
			}
			if (_undoList.Count == 0)
			{
				_undoNumber = 0;
			}
			return true;
		}
		return false;
	}

	private bool CheckForMatchingUndoTransactions(int transactionNumber)
	{
		bool result = false;
		CUndoItem obj = _undoList?.LastOrDefault();
		if (obj != null && obj.UndoNumber == transactionNumber)
		{
			result = true;
		}
		return result;
	}
}
