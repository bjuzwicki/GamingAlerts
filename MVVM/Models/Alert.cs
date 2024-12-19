using System.ComponentModel.DataAnnotations;

namespace GamingAlerts.MVVM.Models
{
    public class Alert
    {
		[Key] public int Id { get; set; }
		public string Name { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime Interval { get; set; }
		public bool IsRepeatable { get; set; }
		public bool IsFavorite { get; set; }

		public DateTime LastModified { get; set; }

		public Alert()
		{
		}
	}
}
