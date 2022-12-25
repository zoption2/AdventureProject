using System;

namespace TheGame
{
    public class StatModifier
    {
        private float _modifierValue;
        public float ModifierValue => _modifierValue;
        private Stat _stat;

        public event Action OnStatModifierChanged;

        public StatModifier(float initedValue)
        {
            _modifierValue = initedValue;
        }

        public void SetValue(float value)
        {
            _modifierValue = value;
            DoOnStatModifierChanged();
        }

        public void AddValue(float value)
        {
            _modifierValue += value;
            DoOnStatModifierChanged();
        }

        private void DoOnStatModifierChanged()
        {
            OnStatModifierChanged?.Invoke();
        }
    }
}



