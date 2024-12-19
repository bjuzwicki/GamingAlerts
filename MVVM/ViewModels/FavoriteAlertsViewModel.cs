using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace GamingAlerts.MVVM.ViewModels
{
    public class FavoriteAlertsViewModel : BaseAlertsViewModel
    {
		public ObservableCollection<AlertViewModel> FavoriteAlerts;
		public FavoriteAlertsViewModel() : base()
        { 

        }

		public override ICollectionView GetAlertCollectionView()
		{
			FavoriteAlerts = new ObservableCollection<AlertViewModel>(Alerts);
			var AlertCollectionView = CollectionViewSource.GetDefaultView(FavoriteAlerts);

			AddFilter(AlertCollectionView);

			foreach (var ev in FavoriteAlerts)
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
				return AlertViewModel != null && AlertViewModel.IsFavorite;
			};
		}

		private void AlertPropertyChangedHandler(object? sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(AlertViewModel.IsFavorite))
			{
				AlertCollectionView.Refresh();
			}
		}
	}
}
