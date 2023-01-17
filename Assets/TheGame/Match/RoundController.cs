using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Mirror;


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
                totalPower += _players[i].Model.StatsGetter.TotalPower;
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
            var sortedBySpeed = _players.OrderByDescending(x => x.Model.StatsGetter.SpeedValue).ToArray();

            for (int i = 0, j = sortedBySpeed.Length; i < j; i++)
            {
                _roundQueue.Enqueue(sortedBySpeed[i]);
            }
        }

    }

    public enum Team
    {
        Player,
        Enemy
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
        private const float kLowLevelRange = 0.5f;

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
        public IPlayerController GetPlayer(IPlayerInfo playerInfo)
        {
            throw new System.NotImplementedException();
        }
    }
}
    


