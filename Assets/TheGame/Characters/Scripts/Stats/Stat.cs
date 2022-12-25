using System.Collections.Generic;
using System;

namespace TheGame
{
    public class Stat
    {
        private float _value;
        private float _originValue;
        private float _maxValue = float.MaxValue;
        private float _minValue = 0f;
        private List<StatModifier> _modifiers = new();

        public float Value => _value;

        public event Action<float> OnValueChange;

        public Stat(float value)
        {
            _originValue = value;
        }

        public Stat(float value, Stat maxStat)
        {
            _originValue = value;
            _maxValue = maxStat.Value;
            maxStat.OnValueChange += ChangeMaxValue;
        }

        public Stat(float value, Stat maxStat, Stat minStat)
        {
            _originValue = value;
            _maxValue = maxStat.Value;
            maxStat.OnValueChange += ChangeMaxValue;

            _minValue = minStat.Value;
            minStat.OnValueChange += ChangeMinValue;
        }

        public void AddModifier(StatModifier modifier)
        {
            _modifiers.Add(modifier);
            UpdateCurrentValue();
            modifier.OnStatModifierChanged += UpdateCurrentValue;
        }

        public void RemoveModifier(StatModifier modifier)
        {
            if (_modifiers.Contains(modifier))
            {
                _modifiers.Remove(modifier);
                UpdateCurrentValue();
                modifier.OnStatModifierChanged -= UpdateCurrentValue;
            }
        }

        private void UpdateCurrentValue()
        {
            var oldValue = _value;
            var newValue = _originValue;
            for (int i = 0, j = _modifiers.Count; i < j; i++)
            {
                newValue += _modifiers[i].ModifierValue;
            }
            _value = Math.Clamp(newValue, _minValue, _maxValue);

            if (newValue != oldValue)
            {
                OnValueChange?.Invoke(_value);
            }
        }

        private void ChangeMaxValue(float maxValue)
        {
            _maxValue = maxValue;
        }

        private void ChangeMinValue(float minValue)
        {
            _minValue = minValue;
        }
    }
}



