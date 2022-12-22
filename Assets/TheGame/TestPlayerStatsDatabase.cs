using UnityEngine;

namespace TheGame.Data
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]
    public class TestPlayerStatsDatabase : ScriptableObject
    {
        [field: SerializeField] public float Power { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
    }
}


