using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheGame.Gameplay
{
    public abstract class Scenario
    {
        public abstract Actions Actions { get; }
    }

    public enum Actions
    {
        Movement,
        Attack
    }
}

