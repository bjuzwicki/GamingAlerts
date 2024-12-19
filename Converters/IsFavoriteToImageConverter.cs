using System.Globalization;
using System.Windows.Data;

namespace GamingAlerts.Converters
{
	public class IsFavoriteToImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool isFavorite)
			{
				return isFavorite ? @"..\..\Resources\star2.png" : @"..\..\Resources\star1.png";
			}
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
