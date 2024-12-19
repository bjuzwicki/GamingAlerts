namespace GamingAlerts.MVVM.Models
{
	public class BalloonArgs
	{
		public string Title { get; set; }
		public string Message { get; set; }

		public BalloonArgs(string title, string message)
		{
			Title = title;
			Message = message;
		}
	}
}
