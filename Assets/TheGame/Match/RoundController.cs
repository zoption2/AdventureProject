using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Mirror;
using TheGame.Data;


namespace TheGame
{
    public enum RoundState
    {
        Begin,
        Turns,
        End
    }

    public enum PlayerStates
    {
        Wait,
        Defend,
        Gain,
        Turn,
        Dead
    }
   

    [Serializable]
    public class Match
    {
        private int _maxPlayers;
        private string _id = "";
        private List<IPlayerController> _players = new();
        private GameQueue<IPlayerController> _roundQueue = new();

        public string ID => _id;
        public int MaxPlayers => _maxPlayers;
        public int PlayersCount => _players.Count;
        public bool IsRoundHasPlayers => _roundQueue.Count > 0;

        public Match()
        {

        }

        public Match (string id)
        {
            _id = id;
        }

        public float GetMiddlePower()
        {
            if (PlayersCount == 0)
            {
                return -1;
            }

            float totalPower= 0;
            for (int i = 0, j = _players.Count; i < j; i++)
            {
                totalPower += _players[i].Model.StatsGetter.Power.Value;
            }
            var result = totalPower / PlayersCount;

            return result;
        }

        public void AddPlayer(IPlayerController player)
        {
            if (!_players.Contains(player))
            {
                _players.Add(player);
            }
        }

        public IPlayerController GetPlayerFromQueue()
        {
            return _roundQueue.Dequeue();
        }

        public void PrepareQueue()
        {
            var playersCount = _players.Count;
            var sortedBySpeed = _players.OrderByDescending(x => x.Model.StatsGetter.Speed.Value).ToArray();

            for (int i = 0, j = sortedBySpeed.Length; i < j; i++)
            {
                _roundQueue.Enqueue(sortedBySpeed[i]);
            }
        }

    }

    public enum Team
    {
        First,
        Second
    }

    public enum PlayerType
    {
        Human,
        AI
    }

    public struct PlayerInfo
    {
        public PlayerType PlayerType;
        //public List<Booster> Boosters;
        public CharacterData CharacterData;
    }

    [Serializable]
    public class Battle
    {
        private List<Fighter> _fighters;
        public IReadOnlyList<Fighter> Fighters => _fighters;

        public void AddFighter(Character character, Team team)
        {
            var fighter = new Fighter(character, team);
            _fighters.Add(fighter);
        }

        public struct Fighter : IPlayerInfo
        {
            private Character _character;
            private Team _team;

            public Character Character => _character;
            public Team Team => _team;

            public Fighter(Character character, Team team)
            {
                _character = character;
                _team = team;
            }
        }
    }

    public interface IMatchService
    {
        void StartMatch(Battle battle);
    }

    public class MatchService : IMatchService
    {
        private const int kIdLength = 7;

        private Match _match;
        private IPlayerFactory _playerFactory;

        public void StartMatch(Battle battle)
        {
            var match = CreateMatch();
            for (int i = 0, j = battle.Fighters.Count; i < j; i++)
            {
                IPlayerInfo info = battle.Fighters[i];
                IPlayerController player = _playerFactory.GetPlayer(info);
                match.AddPlayer(player);
            }

            _match = match;
            //load match scene additive
            //show loading screen
            //setup match to scene MatchController
        }

        private Match CreateMatch()
        {
            var matchId = GetRandomMatchID();
            var match = new Match(matchId);
            return match;
        }

        private string GetRandomMatchID()
        {
            return DataUtils.GetUniqueKey(kIdLength);
        }
    }

    public interface IPlayerInfo
    {
        Character Character { get; }
        Team Team { get; }
    }

    public interface IPlayerFactory
    {
        IPlayerController GetPlayer(IPlayerInfo playerInfo);
    }

    public class PlayerFactory : IPlayerFactory
    {
        private CharacterProvider _characterProvider;
        private IPlayerService _playerService;

        public PlayerFactory(CharacterProvider characterProvider
            , IPlayerService playerService)
        {
            _characterProvider = characterProvider;
            _playerService = playerService;
        }

        public IPlayerController GetPlayer(IPlayerInfo playerInfo)
        {
            var playerController = new PlayerController(_characterProvider);
            var playerModel = new PlayerModel()
        }
    }

    public interface IPlayerService
    {
        IPlayerModel GetModel(int id);
        //IPlayerInfo CreateNewCharacter();
    }

    public class PlayerService: IPlayerService
    {
        private IDataService _dataService;

        public IPlayerModel GetModel(int id)
        {
            var model = new PlayerModel();
        }
    }

    public class CharacterInstanceDataProvider
    {
        private Dictionary<string, CharacterData> _charactersData = new();

        public void AddCharacterStatsData(CharacterData data)
        {
            if (!_charactersData.ContainsKey(data.ID))
            {
                _charactersData.Add(data.ID, data);
            }
        }

        public ICharacterData Get(string id)
        {
            if (_charactersData.ContainsKey(id))
            {
                return _charactersData[id];
            }
            else throw new System.ArgumentNullException(
                string.Format("There is no instance finded with ID {0}", id));
        }
    }

    public class CharacterDataProvider
    {
        public CharacterInstanceDataProvider InstanceData { get; } = new();
        public CharacterBaseDataProvider BaseData { get; } = new();
    }

    public class CharacterBaseDataProvider
    {
        private Dictionary<Character, CharacterBaseData> _charactersData = new();

        public void AddStatsData(CharacterBaseData data)
        {
            if (!_charactersData.ContainsKey(data.Character))
            {
                _charactersData.Add(data.Character, data);
            }
        }

        public CharacterBaseData Get(Character character)
        {
            if (_charactersData.ContainsKey(character))
            {
                return _charactersData[character];
            }
            else throw new System.ArgumentNullException(
                string.Format("There is no base data exist for character {0}", character));
        }
    }

    public interface ICharacterData
    {
        string Name { get; }
        bool CompareID(string id);
        string GetID(object requester);
    }

    [Serializable]
    public class CharacterData : ICharacterData
    {
        private readonly string _id;
        private readonly CharacterBaseData _base;
        private string _name;

        public string ID => _id;
        public string Name => _name;
        public CharacterBaseData Base => _base;


        public int TotalSkillPoints;
        public int AvailableSkillPoints;

        public CharacterData(string id, CharacterBaseData baseData)
        {
            _id = id;
        }

        public bool CompareID(string id)
        {
            return _id == id;
        }

        public string GetID(object requester)
        {
            //observe requester
            return _id;
        }

        [Serializable]
        private class StatStorage
        {
            public StatType Stat;
            public float TotalValue;
        }
    }

    public struct StatChanger
    {
        public TripleKey ChangerKey;
        public float Value;
        public Type ChangeType;

        public enum Type
        {
            Absolute,
            Relative
        }
    }

    


    [Serializable]
    public struct CharacterBaseData
    {
        public Character Character;
        public float Power;
        public float Attack;
        public float Defence;
        public float Health;
        public float Mana;
        public float Speed;
        public float CritRate;
        public float CritDamage;
    }

    public enum StatType
    {
        Attack,
        Defence,
        Health,
        Mana,
        Speed,
        CritRate,
        CritDamage
    }

}
    


