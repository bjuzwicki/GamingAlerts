using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;
using System.Windows.Input;

namespace GamingAlerts.MVVM.ViewModels
{
    public class NotifyIconViewModel
    {
		private TaskbarIcon _notifyIcon;

		private bool _isBalloonOpen = false;
		private readonly SemaphoreSlim _balloonSemaphore = new SemaphoreSlim(1, 1);

		public NotifyIconViewModel()
        {
			_notifyIcon = (TaskbarIcon)Application.Current.Resources["NotifyIcon"];
		}

        public ICommand ShowWindowCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CanExecuteFunc = () => Application.Current.MainWindow == null || (Application.Current.MainWindow.Visibility == Visibility.Hidden
										|| Application.Current.MainWindow.Visibility == Visibility.Collapsed),

					CommandAction = () =>
                    {
                        Application.Current.MainWindow = new MainWindow();
                        Application.Current.MainWindow.Show();
                    }
                };
            }
        }

        public ICommand HideWindowCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CanExecuteFunc = () => Application.Current.MainWindow != null && Application.Current.MainWindow.Visibility == Visibility.Visible,
                    CommandAction = () => Application.Current.MainWindow.Close(),           
                };
            }
        }

        public ICommand ExitApplicationCommand
        {
            get
            {
                return new DelegateCommand {CommandAction = () => Application.Current.Shutdown()};
            }
        }

        public async Task ShowBalloon(string title, string text)
        {
			await _balloonSemaphore.WaitAsync();

			try
			{
				_notifyIcon.ShowBalloonTip(title, text, BalloonIcon.Info);

				await Task.Delay(3000);
			}
			finally
			{
				_balloonSemaphore.Release();
			}
		}
    }

    public class DelegateCommand : ICommand
    {
        public Action CommandAction { get; set; }
        public Func<bool> CanExecuteFunc { get; set; }

        public void Execute(object parameter)
        {
            CommandAction();
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc == null  || CanExecuteFunc();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
