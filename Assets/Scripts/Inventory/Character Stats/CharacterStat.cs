using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Kryz.CharacterStats
{
    [Serializable]
    public class CharacterStat
    {
        public float BaseValue;
        public float MaxValue;
        public float CurValue;

        protected bool isDirty = true;
        protected float lastBaseValue;

        protected float _value;
        public virtual float Value
        {
            get
            {
                if (isDirty || lastBaseValue != BaseValue)
                {
                    lastBaseValue = BaseValue;
                    _value = CalculateFinalValue();
                    isDirty = false;
                }
                return _value;
            }
        }

        protected readonly List<StatModifier> statModifiers;
        public readonly ReadOnlyCollection<StatModifier> StatModifiers;

        public CharacterStat()
        {
            statModifiers = new List<StatModifier>();
            StatModifiers = statModifiers.AsReadOnly();
            CurValue = BaseValue;
        }

        public CharacterStat(float baseValue) : this()
        {
            BaseValue = baseValue;
            MaxValue = baseValue;
            CurValue = baseValue;
        }

        public virtual void AddModifier(StatModifier mod)
        {
            isDirty = true;
            statModifiers.Add(mod);
            statModifiers.Sort(CompareModifierOrder);

            if (mod.StatSetType == StatSetType.MaxValue)
            {
                MaxValue += mod.Value; // Increase MaxValue
                CurValue = MaxValue; // Update CurValue to match MaxValue
            }

        }

        public virtual bool RemoveModifier(StatModifier mod)
        {
            if (statModifiers.Remove(mod))
            {
                isDirty = true;

                // Recalculate MaxValue and CurValue after removing the modifier
                RecalculateValues();

                return true;
            }
            return false;
        }

        public virtual bool RemoveAllModifiersFromSource(object source)
        {
            bool didRemove = false;

            for (int i = statModifiers.Count - 1; i >= 0; i--)
            {
                if (statModifiers[i].Source == source)
                {
                    isDirty = true;
                    didRemove = true;
                    statModifiers.RemoveAt(i);
                }
            }

            // Recalculate MaxValue and CurValue after removing all modifiers from the source
            RecalculateValues();

            return didRemove;
        }

        protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
        {
            if (a.Order < b.Order)
                return -1;
            else if (a.Order > b.Order)
                return 1;
            return 0;
        }

        protected virtual float CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float sumPercentAdd = 0;

            for (int i = 0; i < statModifiers.Count; i++)
            {
                StatModifier mod = statModifiers[i];

                if (mod.Type == StatModType.Flat)
                {
                    finalValue += mod.Value;
                }
                else if (mod.Type == StatModType.PercentAdd)
                {
                    sumPercentAdd += mod.Value;

                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                }
                else if (mod.Type == StatModType.PercentMult)
                {
                    finalValue *= 1 + mod.Value;
                }

            }
            MaxValue = (float)Math.Round(finalValue, 4);

            return (float)Math.Round(finalValue, 4);
        }

        // Method to recalculate MaxValue and CurValue after modifying or removing modifiers
        protected virtual void RecalculateValues()
        {
            MaxValue = BaseValue; // Reset MaxValue to BaseValue

            // Iterate through all modifiers and update MaxValue and CurValue accordingly
            foreach (var modifier in statModifiers)
            {
                    MaxValue += modifier.Value;
            }

            if (CurValue > MaxValue)
                CurValue = MaxValue;
        }

        public static CharacterStat operator +(CharacterStat _actorStat1, CharacterStat _actorStat2)
        {
            return new CharacterStat(_actorStat1.BaseValue + _actorStat2.BaseValue);
        }

        public static CharacterStat operator -(CharacterStat _actorStat1, CharacterStat _actorStat2)
        {
            return new CharacterStat(_actorStat1.BaseValue - _actorStat2.BaseValue);
        }
    }
}
