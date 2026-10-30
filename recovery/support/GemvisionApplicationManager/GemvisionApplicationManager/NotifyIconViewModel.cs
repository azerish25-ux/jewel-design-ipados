using System.Windows;
using System.Windows.Input;

namespace GemvisionApplicationManager;

public class NotifyIconViewModel
{
	public ICommand ShowWindowCommand => new DelegateCommand
	{
		CanExecuteFunc = () => Application.Current.MainWindow == null,
		CommandAction = delegate
		{
			Application.Current.MainWindow = new MainWindow();
			Application.Current.MainWindow.Show();
		}
	};

	public ICommand HideWindowCommand => new DelegateCommand
	{
		CommandAction = delegate
		{
			Application.Current.MainWindow.Close();
		},
		CanExecuteFunc = () => Application.Current.MainWindow != null
	};

	public ICommand ExitApplicationCommand => new DelegateCommand
	{
		CommandAction = delegate
		{
			Application.Current.Shutdown();
		}
	};
}
