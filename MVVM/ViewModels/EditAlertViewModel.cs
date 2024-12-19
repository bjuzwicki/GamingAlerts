using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamingAlerts.Helpers;
using GamingAlerts.Managers;
using GamingAlerts.MVVM.Models;
using System.Windows.Input;

namespace GamingAlerts.MVVM.ViewModels
{
	public class EditAlertViewModel : ObservableObject
	{
		public AlertViewModel AlertViewModel { get; set; }
		public Alert Alert { get; set; }
		public bool RunAfterSave { get; set; }
		public ICommand SaveAlertCommand { get; }

		public EditAlertViewModel(AlertViewModel editAlertViewModel)
		{
			AlertViewModel = editAlertViewModel;

			Alert = editAlertViewModel.Alert;

			SaveAlertCommand = new RelayCommand(SaveAlert);
		}

		private void SaveAlert()
		{
			AlertManager.Instance.UpdateAlert(AlertViewModel);

			if (RunAfterSave)
			{
				RunAlertAfterSave(AlertViewModel);
			}

			ShowAlertsListView();
		}

		private void RunAlertAfterSave(AlertViewModel AlertViewModel)
		{
			AlertViewModel.ForceStartAlertCommand.Execute(null);
		}

		private void ShowAlertsListView()
		{
			Mediator.Notify("ShowAllAlerts");
		}
	}
}
