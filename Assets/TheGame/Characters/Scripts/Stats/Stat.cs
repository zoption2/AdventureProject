using System.Collections.Generic;
using System;

namespace TheGame
{
    public interface IStatGetter
    {
        float Value { get; }
        void AddStatWatcher(IStatWatcher statWatcher);
        void RemoveStatWatcher(IStatWatcher statWatcher);
    }

    public class Stat : IStatGetter
    {
        private float _value;
        private float _originValue;
        private float _maxValue = float.MaxValue;
        private float _minValue = 0f;
        private List<StatModifier> _modifiers = new();
        private List<IStatWatcher> _watchers = new();

        public float Value => _value;

        public event Action<float> OnValueChange;

        public Stat(float value)
        {
            _originValue = value;
            SetClampedValue(value);
        }

        public Stat(float value, Stat maxStat)
        {
            _originValue = value;
            _maxValue = maxStat.Value;
            maxStat.OnValueChange += ChangeMaxValue;
            SetClampedValue(value);
        }

        public Stat(float value, Stat maxStat, Stat minStat)
        {
            _originValue = value;
            _maxValue = maxStat.Value;
            maxStat.OnValueChange += ChangeMaxValue;

            _minValue = minStat.Value;
            minStat.OnValueChange += ChangeMinValue;
            SetClampedValue(value);
        }

        public void AddStatWatcher(IStatWatcher statWatcher)
        {
            if (!_watchers.Contains(statWatcher))
            {
                _watchers.Add(statWatcher);
            }
        }

        public void RemoveStatWatcher(IStatWatcher statWatcher)
        {
            if (_watchers.Contains(statWatcher))
            {
                _watchers.Remove(statWatcher);
            }
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

        private void NotifyWatchers(float value)
        {
            for (int i = 0, j = _watchers.Count; i < j; i++)
            {
                _watchers[i].Notify(value);
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
            SetClampedValue(newValue);

            if (newValue != oldValue)
            {
                OnValueChange?.Invoke(_value);
                NotifyWatchers(_value);
            }
        }

        private void SetClampedValue(float value)
        {
            _value = Math.Clamp(value, _minValue, _maxValue);
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

    public interface IStatWatcher
    {
        void Notify(float value);
    }

    public sealed class StatWatcher : IStatWatcher
    {
        private float _value;
        private Action<float> ON_STAT_CHANGED;
        public float Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    ON_STAT_CHANGED?.Invoke(value);
                }
                _value = value;
            }
        }

        public void Watch(Action<float> watcher)
        {
            ON_STAT_CHANGED += watcher;
        }

        public void StopWatch(Action<float> watcher)
        {
            ON_STAT_CHANGED -= watcher;
        }

        void IStatWatcher.Notify(float value)
        {
            _value = value;
        }
    }
}



