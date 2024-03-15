using UnityEngine;
using Kryz.CharacterStats;

public enum EquipmentType
{
    Helmet,
    Armor,
    Belt,
    Weapon,
    Ring,
    Pendant,
    Boots,
    Wings
}

[CreateAssetMenu(menuName = "Items/Equippable Item")]
public class EquippableItem : Item
{
    public int RequiredLevel;
    [Space]
    public int StrengthBonus;
    public int IntellectBonus;
    public int AgilityBonus;
    public int DexterityBonus;
    public int StaminaBonus;
    [Space]
    public float StrengthPercentBonus;
    public float IntellectPercentBonus;
    public float AgilityPercentBonus;
    public float DexterityPercentBonus;
    public float StaminaPercentBonus;
    [Space]
    public int HPBonus;
    public int MPBonus;
    public int SpeedBonus;
    public int ATKBonus;
    public int MATKBonus;
    public int DEFBonus;
    public int HitBonus;
    public int DodgeBonus;

    [Space]
    public float HPPercentBonus;
    public float MPPercentBonus;
    public float SpeedPercentBonus;
    public float ATKPercentBonus;
    public float MATKPercentBonus;
    public float DEFPercentBonus;
    public float HitPercentBonus;
    public float DodgePercentBonus;
    [Space]
    public EquipmentType EquipmentType;

    public override Item GetCopy()
    {
        return Instantiate(this);
    }

    public override void Destroy()
    {
        Destroy(this);
    }

    public void Equip(Character c)
    {
        if (StrengthBonus != 0)
            c.unit.Stats.strength.AddModifier(new StatModifier(StrengthBonus, StatModType.Flat, this));
        if (IntellectBonus != 0)
            c.unit.Stats.intellect.AddModifier(new StatModifier(IntellectBonus, StatModType.Flat, this));
        if (AgilityBonus != 0)
            c.unit.Stats.agility.AddModifier(new StatModifier(AgilityBonus, StatModType.Flat, this));
        if (DexterityBonus != 0)
            c.unit.Stats.dexterity.AddModifier(new StatModifier(DexterityBonus, StatModType.Flat, this));
        if (StaminaBonus != 0)
            c.unit.Stats.stamina.AddModifier(new StatModifier(StaminaBonus, StatModType.Flat, this));

        if (StrengthPercentBonus != 0)
            c.unit.Stats.strength.AddModifier(new StatModifier(StrengthPercentBonus, StatModType.PercentMult, this));
        if (IntellectPercentBonus != 0)
            c.unit.Stats.intellect.AddModifier(new StatModifier(IntellectPercentBonus, StatModType.PercentMult, this));
        if (AgilityPercentBonus != 0)
            c.unit.Stats.agility.AddModifier(new StatModifier(AgilityPercentBonus, StatModType.PercentMult, this));
        if (DexterityPercentBonus != 0)
            c.unit.Stats.dexterity.AddModifier(new StatModifier(DexterityPercentBonus, StatModType.PercentMult, this));
        if (StaminaPercentBonus != 0)
            c.unit.Stats.stamina.AddModifier(new StatModifier(StaminaPercentBonus, StatModType.PercentMult, this));
    }

    public void Unequip(Character c)
    {
        c.unit.Stats.strength.RemoveAllModifiersFromSource(this);
        c.unit.Stats.agility.RemoveAllModifiersFromSource(this);
        c.unit.Stats.intellect.RemoveAllModifiersFromSource(this);
        c.unit.Stats.dexterity.RemoveAllModifiersFromSource(this);
        c.unit.Stats.stamina.RemoveAllModifiersFromSource(this);
    }

    public override string GetItemType()
    {
        return EquipmentType.ToString();
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        AddStat(StrengthBonus, "Strength");
        AddStat(AgilityBonus, "Agility");
        AddStat(IntellectBonus, "Intellect");
        AddStat(DexterityBonus, "Dexterity");
        AddStat(StaminaBonus, "Stamina");

        AddStat(StrengthPercentBonus, "Strength", isPercent: true);
        AddStat(AgilityPercentBonus, "Agility", isPercent: true);
        AddStat(IntellectPercentBonus, "Intellect", isPercent: true);
        AddStat(DexterityPercentBonus, "Dexterity", isPercent: true);
        AddStat(StaminaPercentBonus, "Stamina", isPercent: true);

        return sb.ToString();
    }

    private void AddStat(float value, string statName, bool isPercent = false)
    {
        if (value != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();

            if (value > 0)
                sb.Append("+");

            if (isPercent)
            {
                sb.Append(value * 100);
                sb.Append("% ");
            }
            else
            {
                sb.Append(value);
                sb.Append(" ");
            }
            sb.Append(statName);
        }
    }

}
