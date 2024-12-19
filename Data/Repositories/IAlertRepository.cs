using GamingAlerts.MVVM.Models;

namespace GamingAlerts.Data.Repositories
{
	public interface IAlertRepository
	{
		public Task<Alert> Get(int id);
		public Task<ICollection<Alert>> GetAll();
		public Task Add(Alert alert);
		public Task Update(Alert alert);
		public Task Delete(Alert alert);
	}
}
