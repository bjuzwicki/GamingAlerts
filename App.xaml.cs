using GamingAlerts.Data;
using GamingAlerts.Managers;
using Hardcodet.Wpf.TaskbarNotification;
using System.IO;
using System.Windows;

namespace GamingAlerts
{
	public partial class App : Application
	{
		private TaskbarIcon _notifyIcon;

		protected override async void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			_notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

			var dbName = "gaming_alerts_database.db";

			if (!File.Exists(dbName))
			{
				await using var dbContext = new DatabaseContext();
				await dbContext.Database.EnsureCreatedAsync();
				await dbContext.SaveChangesAsync();
			}
		}

		protected override void OnExit(ExitEventArgs e)
		{
			AlertManager.Instance.SaveChanges();
			_notifyIcon.Dispose();

			base.OnExit(e);
		}
	}
}
