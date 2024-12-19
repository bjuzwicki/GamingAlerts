using GamingAlerts.Data.Repositories;
using GamingAlerts.MVVM.Models;
using GamingAlerts.MVVM.ViewModels;
using System.Collections.ObjectModel;

namespace GamingAlerts.Managers
{
	public sealed class AlertManager
	{
		#region singleton
		private static AlertManager? _instance;

		public static AlertManager Instance 
		{
			get
			{
				if(_instance == null)
				{
					_instance = new AlertManager();
				}

				return _instance;
			}
		}
		#endregion

		public ObservableCollection<AlertViewModel> Alerts { get; private set; }
		private IEnumerable<AlertViewModel> _originalAlerts;

		private readonly IAlertRepository _alertRepository;

		public AlertManager()
		{
			_alertRepository = new AlertRepository();
			LoadAlerts();
		}

		public void AddNewAlert(AlertViewModel alertViewModel)
		{
			UpdateLastModified(alertViewModel);
			Alerts.Add(alertViewModel);
		}

		public void UpdateAlert(AlertViewModel alertViewModel)
		{
			UpdateLastModified(alertViewModel);
		}

		public void RemoveAlert(AlertViewModel alertViewModel)
		{
			Alerts.Remove(alertViewModel);
		}

		public async void SaveChanges()
		{
			var newAlerts = Alerts
				.Where(x => x.Alert.Id == 0)
				.Select(x => x.Alert)
				.ToList();

			var addTasks = newAlerts.Select(alert => _alertRepository.Add(alert));
			await Task.WhenAll(addTasks);


			var updateTasks = Alerts
			   .Select(x => x.Alert)
			   .Where(alert => alert.Id != 0)
			   .Where(alert =>
			   {
				   var originalAlert = _originalAlerts.FirstOrDefault(o => o.Id == alert.Id);
				   return originalAlert != null && alert.LastModified != originalAlert.LastModified;
			   })
			   .Select(alert => _alertRepository.Update(alert));
			await Task.WhenAll(updateTasks);

			var alertsToDelete = _originalAlerts
				.Where(original => !Alerts.Any(current => current.Id == original.Id))
				.Select(x => x.Alert)
				.ToList();

			var deleteTasks = alertsToDelete.Select(alert => _alertRepository.Delete(alert));
			await Task.WhenAll(deleteTasks);
		}

		private async void LoadAlerts()
		{
			var alertList = await _alertRepository.GetAll();

			var alertViewModelList = new List<AlertViewModel>();

			alertViewModelList.AddRange(alertList.Select(alert => new AlertViewModel(alert)));

			Alerts = new ObservableCollection<AlertViewModel>(alertViewModelList);
			_originalAlerts = new List<AlertViewModel>(Alerts);
		}

		private void UpdateLastModified(AlertViewModel alert)
		{
			alert.LastModified = DateTime.Now;
		}
	}
}
