namespace Kryz.CharacterStats
{
	public class StatModifier
	{
		public readonly float Value;
		public readonly StatModType Type;
		public readonly int Order;
		public readonly object Source;
		public readonly StatSetType StatSetType;

		public StatModifier(float value, StatModType type, int order, object source, StatSetType statSetType)
		{
			Value = value;
			Type = type;
			Order = order;
			Source = source;
			StatSetType = statSetType;
		}

		public StatModifier(float value, StatModType type, StatSetType statSetType) : this(value, type, (int)type, null, statSetType) { }

		public StatModifier(float value, StatModType type, int order, StatSetType statSetType) : this(value, type, order, null, statSetType) { }

		public StatModifier(float value, StatModType type, object source, StatSetType statSetType) : this(value, type, (int)type, source, statSetType) { }
	}
}
