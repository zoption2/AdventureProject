using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


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

    public class Matchmaker
    {
        private IRoundService _roundService;
        public void StartMatch()
        {
            //_roundService.CreateRound();
        }
    }

   
    public interface IRoundService
    {
        Round CreateRound();
    }

    public class Round
    {
        private List<IPlayerMediator> _players = new();
        private Queue<IPlayerMediator> _roundQueue = new();

        private void PrepareQueue()
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

