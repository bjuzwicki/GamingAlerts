using GamingAlerts.MVVM.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GamingAlerts.Data
{
	public class DatabaseContext : DbContext
	{
		public DbSet<Alert> Alerts { get; set; }
		public string DbName { get; }

		public DatabaseContext() 
		{
			DbName = "gaming_alerts_database.db";
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite($"DataSource={DbName}", option =>
			{
				option.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
			});

			base.OnConfiguring(optionsBuilder);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Alert>().ToTable("Alerts","main");
			modelBuilder.Entity<Alert>(entity =>
			{
				entity.HasKey(k => k.Id);
				entity.HasIndex(i => i.Name).IsUnique();
			});

			base.OnModelCreating(modelBuilder);
		}
	}
}