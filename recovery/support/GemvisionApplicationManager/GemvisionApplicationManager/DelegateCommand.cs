using System;
using System.Windows.Input;

namespace GemvisionApplicationManager;

public class DelegateCommand : ICommand
{
	public Action CommandAction { get; set; }

	public Func<bool> CanExecuteFunc { get; set; }

	public event EventHandler CanExecuteChanged
	{
		add
		{
			CommandManager.RequerySuggested += value;
		}
		remove
		{
			CommandManager.RequerySuggested -= value;
		}
	}

	public void Execute(object parameter)
	{
		CommandAction();
	}

	public bool CanExecute(object parameter)
	{
		if (CanExecuteFunc != null)
		{
			return CanExecuteFunc();
		}
		return true;
	}
}
