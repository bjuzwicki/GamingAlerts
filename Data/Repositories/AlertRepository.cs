using GamingAlerts.MVVM.Models;
using Microsoft.EntityFrameworkCore;

namespace GamingAlerts.Data.Repositories
{
	public class AlertRepository : IAlertRepository
	{
		public async Task<Alert> Get(int id)
		{
			await using var dbContext = new DatabaseContext();

			var alert = await dbContext.Alerts.FirstOrDefaultAsync(x => x.Id == id);

			if(alert == null)
			{
				//throw new NotFoundException
			}
			return alert;
		}

		public async Task<ICollection<Alert>> GetAll()
		{
			await using var dbContext = new DatabaseContext();
			return await dbContext.Alerts.ToListAsync();
		}

		public async Task Add(Alert alert)
		{
			await using var dbContext = new DatabaseContext();
			
			dbContext.Alerts.Add(alert);

			await dbContext.SaveChangesAsync();
		}
		public async Task Update(Alert alert)
		{
			await using var dbContext = new DatabaseContext();
			dbContext.Alerts.Update(alert);

			await dbContext.SaveChangesAsync();
		}

		public async Task Delete(Alert alert)
		{
			await using var dbContext = new DatabaseContext();
			dbContext.Alerts.Remove(alert);

			await dbContext.SaveChangesAsync();
		}
	}
}
