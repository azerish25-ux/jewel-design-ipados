// Adapted scheduling logic from canonical CSUndoTable; native snapshot creation is a typed target boundary.
// UI-thread ownership and complete, isolated snapshots are responsibilities of the integrating host.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace JewelDesign.Graph;

public sealed class UndoJournal<TTarget, TState> where TTarget : class, IUndoTarget<TState>
{
	private readonly List<UndoEntry> _redoList = new List<UndoEntry>();

	private readonly List<UndoEntry> _undoList = new List<UndoEntry>();

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
		else if (!_undoList.Any((UndoEntry _) => _.UndoNumber == _undoNumber))
		{
			_undoNumber = _undoList.Max((UndoEntry _) => _.UndoNumber);
		}
	}



	public void Pause(bool isPausing)
	{
		if (!_isDisabled)
		{
			_isPaused = isPausing;
		}
	}

	public bool RecordObjectStateInUndoList(TTarget csObject, string callingMember = "")
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
			_undoList.Add(new UndoEntry(_undoNumber, csObject, DateTime.Now, callingMember.Trim().ToLowerInvariant()));
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
				_undoList.Add(new UndoEntry(_undoNumber, _redoList[num].GraphObject, _redoList[num].DatetimeObjectCreated, _redoList[num].Source));
				_redoList[num].GraphObject.RestoreUndoState(_redoList[num].UndoObject);
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
				_redoList.Add(new UndoEntry(_undoNumber, _undoList[num].GraphObject, _undoList[num].DatetimeObjectCreated, _undoList[num].Source));
				_undoList[num].GraphObject.RestoreUndoState(_undoList[num].UndoObject);
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
		UndoEntry? obj = _undoList?.LastOrDefault();
		if (obj != null && obj.UndoNumber == transactionNumber)
		{
			result = true;
		}
		return result;
	}

    private sealed class UndoEntry
    {
        public string Source { get; }
        public DateTime DatetimeObjectCreated { get; }
        public TTarget GraphObject { get; }
        public int UndoNumber { get; }
        public TState UndoObject { get; }

        public UndoEntry(int undoNumber, TTarget target, DateTime created, string source)
        {
            Source = source;
            GraphObject = target;
            UndoObject = target.CaptureUndoState();
            UndoNumber = undoNumber;
            DatetimeObjectCreated = created;
        }
    }
}
