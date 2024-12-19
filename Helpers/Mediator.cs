namespace GamingAlerts.Helpers
{
	public class Mediator
	{
		private static readonly Dictionary<string, List<Action<object>>> _actions = new Dictionary<string, List<Action<object>>>();

		public static void Register(string token, Action<object> callback)
		{
			if (!_actions.ContainsKey(token))
			{
				_actions[token] = new List<Action<object>>();
			}
			_actions[token].Add(callback);
		}

		public static void Notify(string token, object args = null)
		{
			if (_actions.ContainsKey(token))
			{
				foreach (var callback in _actions[token])
				{
					callback(args);
				}
			}
		}
	}
}
