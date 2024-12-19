using CommunityToolkit.Mvvm.ComponentModel;
using GamingAlerts.Managers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace GamingAlerts.MVVM.ViewModels
{
    public abstract class BaseAlertsViewModel : ObservableObject
    {
		public ObservableCollection<AlertViewModel> Alerts { get; private set; }

		public ICollectionView AlertCollectionView { get; }

		public BaseAlertsViewModel()
		{
			Alerts = AlertManager.Instance.Alerts;

			AlertCollectionView = GetAlertCollectionView();
		}
		
		public virtual ICollectionView GetAlertCollectionView()
		{
			var AlertCollectionView = CollectionViewSource.GetDefaultView(Alerts);

			AddFilter(AlertCollectionView);

			return AlertCollectionView;
		}

		public virtual void AddFilter(ICollectionView AlertCollectionView)
		{ 
			AlertCollectionView.Filter = null;
		}
	}
}
