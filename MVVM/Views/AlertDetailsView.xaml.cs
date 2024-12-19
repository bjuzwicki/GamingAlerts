using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Controls;

namespace GamingAlerts.MVVM.Views
{
    public partial class AlertDetailsView : UserControl
    {
		public AlertDetailsView()
        {
            InitializeComponent();
        }

		private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}
	}
}
