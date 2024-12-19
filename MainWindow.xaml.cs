using GamingAlerts.MVVM.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace GamingAlerts
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			if(DataContext is MainViewModel viewModel) 
			{ 
				viewModel.PropertyChanged += ViewModelPropertyChangedHandler;
			}
		}

		private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		private void ViewModelPropertyChangedHandler(object? sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(MainViewModel.CurrentViewModel) && sender is MainViewModel mainViewModel)
			{
				if(mainViewModel.CurrentViewModel is AllAlertsViewModel allAlertsViewModel)
				{
					AllAlertsButton.IsChecked = true;
				}				
			}
		}
	}
}
