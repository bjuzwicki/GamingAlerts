using GamingAlerts.Helpers;
using GamingAlerts.MVVM.Models;
using System.Windows.Input;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GamingAlerts.MVVM.ViewModels
{
    public class MainViewModel : ObservableObject
    {
		private object _currentViewModel;
		public object CurrentViewModel
		{
			get => _currentViewModel;
			set
			{
				_currentViewModel = value;
				OnPropertyChanged(nameof(CurrentViewModel));
			}
		}
		public ICommand ShowAllAlertsCommand { get; private set;}
		public ICommand ShowActiveAlertsCommand { get; private set; }
		public ICommand ShowFavoriteAlertsCommand { get; private set; }
		public ICommand AddNewAlertCommand { get; private set; }
		public ICommand EditAlertCommand { get; private set; }

		public ICommand HideWindowCommand { get; private set; }
		public ICommand ExitWindowCommand { get; private set; }

		public NotifyIconViewModel NotifyIconViewModel { get;set;}

		public MainViewModel()
        {
			InitializeViewModels();
			InitializeCommands();
			RegisterMessageHandlers();

			ShowAllAlertsCommand?.Execute(null);
		}

		private void InitializeCommands()
		{
			ShowAllAlertsCommand = new RelayCommand(() => CurrentViewModel = new AllAlertsViewModel());
			ShowActiveAlertsCommand = new RelayCommand(() => CurrentViewModel = new ActiveAlertsViewModel());
			ShowFavoriteAlertsCommand = new RelayCommand(() => CurrentViewModel = new FavoriteAlertsViewModel());
			AddNewAlertCommand = new RelayCommand(() => CurrentViewModel = new AddNewAlertViewModel());
			EditAlertCommand = new RelayCommand<AlertViewModel>((editAlert) => CurrentViewModel = new EditAlertViewModel(editAlert));

			HideWindowCommand = new RelayCommand(HideWindow);
			ExitWindowCommand = new RelayCommand(ExitWindow);
		}

		private void InitializeViewModels()
		{
			NotifyIconViewModel = new NotifyIconViewModel();
		}

		private void RegisterMessageHandlers()
		{
			Mediator.Register("ShowBalloon", async args =>
			{
				if (args is BalloonArgs balloonArgs)
				{
					await ShowBalloon(balloonArgs.Title, balloonArgs.Message);
				}
			});

			Mediator.Register("ShowAllAlerts", args =>
			{
				ShowAllAlertsCommand.Execute(args);
			});

			Mediator.Register("EditAlert", args =>
			{
				EditAlertCommand.Execute(args);
			});
		}

		private async Task ShowBalloon(string title, string message)
		{
			await NotifyIconViewModel.ShowBalloon(title, message);
		}

		private void HideWindow()
		{
			Application.Current.MainWindow.Close();
		}

		private void ExitWindow()
		{
			Application.Current.Shutdown();
		}
	}
}
