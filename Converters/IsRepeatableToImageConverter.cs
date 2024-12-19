using System.Globalization;
using System.Windows.Data;

namespace GamingAlerts.Converters
{
	public class IsRepeatableToImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool isRepeatable)
			{
				return isRepeatable ? @"..\..\Resources\repeat2.png" : @"..\..\Resources\repeat1.png";
			}
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
