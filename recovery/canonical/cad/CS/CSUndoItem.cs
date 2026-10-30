using System;
using CS.CSObjects;

namespace CS;

public sealed class CSUndoItem
{
	public DateTime DatetimeObjectCreated { get; private set; }

	public CSObject GraphObject { get; private set; }

	public int UndoNumber { get; private set; }

	public CSObject UndoObject { get; }

	public int RedoNumber { get; private set; }

	public CSUndoItem(int undoNumber, CSObject originalObject, DateTime timeCreated)
	{
		UndoNumber = -1;
		RedoNumber = -1;
		GraphObject = originalObject;
		UndoObject = originalObject.CreateTempObject();
		if (UndoObject != null)
		{
			UndoObject.SetUndo(originalObject);
		}
		UndoNumber = undoNumber;
		RedoNumber = 0;
		DatetimeObjectCreated = timeCreated;
	}
}
