using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheGame
{
	public interface IPlayerMediator
	{
		PlayerModel Model { get; }
		void Init();
		void SetView(IPlayerView view);
    }

	public class PlayerMediator : IPlayerMediator
	{
		private PlayerModel _model;
		private IPlayerView _view;

		public PlayerModel Model => _model;

		public void Init()
		{
			_model = new PlayerModel();
		}

		public void SetView(IPlayerView view)
		{
			_view = view;
		}
	}

	public class PlayerModel
	{
		public IStatsGetter StatsGetter { get; }
	}

	public interface IPlayerView
	{

	}
}

