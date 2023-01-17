using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheGame
{


    public interface IPlayerController : IMediator<IPlayerView, IPlayerModel>
	{
		void Init();
    }

	public class PlayerController : IPlayerController
	{
		private PlayerModel _model;
		private PlayerView _view;
		private CharacterProvider _characterProvider;

		public IPlayerModel Model => _model;
		public IPlayerView View => _view;

		public PlayerController(CharacterProvider characterProvider)
		{
			_characterProvider = characterProvider;
		}

		public async void Init()
		{
			
		}

		public void SetModel(PlayerModel model)
		{
			_model = model;
			InitView();
		}

		private async void InitView()
		{
			var characterTask = _characterProvider.GetCharacterAsync(_model.Character);
			await characterTask;
			GameObject characterGO = characterTask.GetAwaiter().GetResult();
			_view = characterGO.GetComponent<PlayerView>();			
		}
	}

	public interface IPlayerModel : IModel
	{
        IStatsGetter StatsGetter { get; }
        Character Character { get; }
    }

	public class PlayerModel : IPlayerModel
	{
		private readonly Character _character;
		private Stats _stats;
		//provide access to player exectly and all it's characters achieved progress
		//private IPlayerDataService
		public IStatsGetter StatsGetter => _stats;
		public Character Character { get; }

		public PlayerModel(Character character)
		{
			_character = character;
		}
	}
}


