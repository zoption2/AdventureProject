namespace TheGame
{
	public interface ICachedPlayer
	{
		void SavePlayer(IPlayerMediator player);
		IPlayerMediator Get();
	}

	public class CachedPlayer : ICachedPlayer
    {
		private IPlayerMediator _player;

		public void SavePlayer(IPlayerMediator player)
        {
			_player = player;
        }

		public IPlayerMediator Get()
        {
			if (_player != null)
            {
				return _player;
            }
			else
            {
				var player = new PlayerMediator();
				player.Init();
				return player;
			}
        }
    }
}


