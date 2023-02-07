using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Mirror;
using TheGame.Data;
using TheGame.Utils;


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
        public CharacterInstanceData CharacterData;
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

    public interface ICharacterInstanceData
    {
        string Name { get; }
        string ID { get; }
    }

    [Serializable]
    public class CharacterInstanceData : ICharacterInstanceData
    {
        private readonly string _id;
        private readonly ICharacterBaseData _base;
        private string _name = "Noname";
        private Dictionary<StatType, DataStat> _statsData = new();

        public string ID => _id;
        public string Name => _name;
        public ICharacterBaseData Base => _base;

        public int TotalSkillPoints;
        public int AvailableSkillPoints;

        public CharacterInstanceData(string id, ICharacterBaseData baseData)
        {
            _id = id;
            _base = baseData;
        }

        public float GetStatValue(StatType stat)
        {
            if (!_statsData.ContainsKey(stat))
            {
                var baseData = _base.GetValue(stat);
                var statData = new DataStat(stat, baseData);
            }
            return _statsData[stat].TotalValue;
        }

        public void AddStatModifier(DataStatModifier modifier)
        {
            var stat = modifier.Stat;
            if (_statsData.ContainsKey(stat))
            {
                _statsData[stat].AddModifier(modifier);
            }
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

        private void AddBaseStatData(StatType stat)
        {

        }

        [Serializable]
        private class DataStat
        {
            private StatType _stat;
            private float _baseValue;
            private float _totalValue;
            private Dictionary<TripleKey, DataStatModifier> _modifiers = new();

            public StatType Stat => _stat;
            public float TotalValue => _totalValue;

            public DataStat(StatType stat, float baseValue)
            {
                _stat = stat;
                _baseValue = baseValue;
            }

            public void AddModifier(DataStatModifier modifier)
            {
                var key = modifier.Key;
                if (!_modifiers.ContainsKey(key))
                {
                    _modifiers.Add(key, modifier);
                }
                _modifiers[key].UpdateValue(modifier.Value);
                UpdateTotalValue();
            }

            public void RemoveModifier(TripleKey key)
            {
                if (_modifiers.ContainsKey(key))
                {
                    _modifiers.Remove(key);
                }
                UpdateTotalValue();
            }

            public void UpdateBaseValue(float newValue)
            {
                _baseValue = newValue;
                UpdateTotalValue();
            }

            private void UpdateTotalValue()
            {
                var absolutValue = _baseValue;
                var relativeValue = 0f;
                foreach (var modifier in _modifiers.Values)
                {
                    switch (modifier.ChangeType)
                    {
                        case DataStatModifier.Type.Absolute:
                            absolutValue += modifier.Value;
                            break;
                        case DataStatModifier.Type.Relative:
                            relativeValue += modifier.Value;
                            break;
                        default:
                            break;
                    }
                }
                _totalValue = absolutValue + absolutValue * relativeValue;
            }
        }
    }

    public class DataStatModifier
    {
        private StatType _stat;
        private TripleKey _changerKey;
        private float _value;
        private Type _changeType;

        public StatType Stat => _stat;
        public TripleKey Key => _changerKey;
        public float Value => _value;
        public Type ChangeType => _changeType;

        public DataStatModifier(StatType stat, float value, Type type, TripleKey key)
        {
            _stat = stat;
            _value = value;
            _changeType = type;
            _changerKey = key;
        }

        public void UpdateValue(float newValue)
        {
            _value = newValue;
        }

        public enum Type
        {
            Absolute,
            Relative
        }
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

    public class Player
    {
        private List<ICharacterInstanceData> _characters;
    }
}
    


