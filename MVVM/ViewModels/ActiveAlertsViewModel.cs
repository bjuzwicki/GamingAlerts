using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace GamingAlerts.MVVM.ViewModels
{
    public class ActiveAlertsViewModel : BaseAlertsViewModel
    {
		public ObservableCollection<AlertViewModel> ActiveAlerts;

        public ActiveAlertsViewModel() : base()
        {
			
		}

		public override ICollectionView GetAlertCollectionView()
		{
			ActiveAlerts = new ObservableCollection<AlertViewModel>(Alerts);
			var AlertCollectionView = CollectionViewSource.GetDefaultView(ActiveAlerts);

			AddFilter(AlertCollectionView);

			foreach (var ev in ActiveAlerts)
			{
				ev.PropertyChanged += AlertPropertyChangedHandler;
			}

			return AlertCollectionView;
		}

		public override void AddFilter(ICollectionView AlertCollectionView)
		{
			base.AddFilter(AlertCollectionView);

			AlertCollectionView.Filter = x =>
			{
				var AlertViewModel = x as AlertViewModel;
				return AlertViewModel != null && AlertViewModel.IsActive;
			};
		}

		private void AlertPropertyChangedHandler(object? sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(AlertViewModel.IsActive))
			{
				AlertCollectionView.Refresh();
			}
		}
	}
}
