using System;

namespace CS;

public sealed class CSUndoTransaction : IDisposable
{
	private readonly bool _requiresGraphUpdate;

	private bool _disposed;

	public int TransactionNumber { get; }

	public CSUndoTransaction(bool requiresGraphUpdate = true)
	{
		_requiresGraphUpdate = requiresGraphUpdate;
		if (_requiresGraphUpdate)
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		}
		TransactionNumber = CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord(".ctor");
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected void Dispose(bool disposing)
	{
		if (_disposed)
		{
			return;
		}
		if (disposing)
		{
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("Dispose");
			if (_requiresGraphUpdate)
			{
				CSApplicationHelper.Application.Document.Graph.Generate();
				CSUtil.Redraw();
			}
		}
		_disposed = true;
	}

	~CSUndoTransaction()
	{
		Dispose(disposing: false);
	}
}
