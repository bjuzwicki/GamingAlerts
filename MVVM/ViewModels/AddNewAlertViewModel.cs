using GamingAlerts.Managers;
using GamingAlerts.MVVM.Models;
using System.Windows.Input;
using GamingAlerts.Helpers;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GamingAlerts.MVVM.ViewModels
{
    public class AddNewAlertViewModel : ObservableObject
    {
        public Alert Alert { get; set; }
        public bool RunAfterSave { get; set; }
        public ICommand SaveAlertCommand { get; }

        public AddNewAlertViewModel()
        {
            Alert = new Alert();
			Alert.Interval = new DateTime(DateOnly.MinValue, new TimeOnly(0, 0, 5, 0, 0));

			SaveAlertCommand = new RelayCommand(SaveNewAlert);

        }

        private void SaveNewAlert()
        {
            var newAlertViewModel = new AlertViewModel(Alert);

            AlertManager.Instance.AddNewAlert(newAlertViewModel);

            if(RunAfterSave)
            {
                RunAlertAfterSave(newAlertViewModel);
            }

            ShowAlertsListView();
        }

        private void RunAlertAfterSave(AlertViewModel AlertViewModel)
        {
            AlertViewModel.ChangeAlertStatusCommand.Execute(null);
        }

        private void ShowAlertsListView()
        {
            Mediator.Notify("ShowAllAlerts");
        }
    }
}
