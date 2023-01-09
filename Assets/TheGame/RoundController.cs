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
        private List<IPlayerMediator> _players = new();
        private Queue<IPlayerMediator> _roundQueue = new();

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

        public void AddPlayer(IPlayerMediator player)
        {
            if (!_players.Contains(player))
            {
                _players.Add(player);
            }
        }

        public IPlayerMediator GetPlayerFromQueue()
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
}
    


