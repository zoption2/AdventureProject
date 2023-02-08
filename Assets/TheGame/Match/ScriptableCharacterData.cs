using UnityEngine;
using System;
using TheGame.Utils;

namespace TheGame.Data
{
    public interface ICharacterBaseData
    {
        Character Character { get; }
        IBaseStat[] Stats { get; }
        float GetValue(StatType stat);
    }

    public interface IBaseStat
    {
        StatType StatType { get; }
        float BaseValue { get; }
    }

    [CreateAssetMenu(fileName = "NewCharacterData", menuName = "ScriptableObjects/NewCharacterData")]
    public class ScriptableCharacterData : ScriptableObject, ICharacterBaseData
    {
        [field: SerializeField] public Character Character { get; private set; }
        [SerializeField] private Stat[] _stats;

        public IBaseStat[] Stats => _stats;

        public float GetValue(StatType stat)
        {
            for (int i = 0, j = _stats.Length; i < j; i++)
            {
                if (_stats[i].StatType == stat)
                {
                    return _stats[i].BaseValue;
                }
            }
            throw new System.ArgumentException(
                string.Format("Stat of type {0} for {1} character not found", stat, Character)
                );
        }

        [ContextMenu("Prepare base")]
        private void PrepareBase()
        {
            if (Stats.Length == 0)
            {
                var enums = SupportUtility.GetEnumValues<StatType>();
                var newStats = new Stat[enums.Length];

                for (int i = 0, j = enums.Length;  i < j; i++)
                {
                    newStats[i] = new Stat(enums[i], 0);
                }
                _stats = newStats;
            }
        }

        [Serializable]
        public class Stat : IBaseStat
        {
            [field: SerializeField] public StatType StatType { get; private set; }
            [field: SerializeField] public float BaseValue { get; private set; }

            public Stat(StatType stat, float value)
            {
                StatType = stat;
                BaseValue = value;
            }
        }
    }
}
    


