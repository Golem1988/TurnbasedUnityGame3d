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
    [Space]
    public GameObject Model;


    public override Item GetCopy()
    {
        return Instantiate(this);
    }

    public override void Destroy()
    {
        Destroy(this);
    }

    public void Equip(CharacterInformation c)
    {
        if (StrengthBonus != 0)
            c.Stats.strength.AddModifier(new StatModifier(StrengthBonus, StatModType.Flat, this, StatSetType.MaxValue));
        if (IntellectBonus != 0)
            c.Stats.intellect.AddModifier(new StatModifier(IntellectBonus, StatModType.Flat, this, StatSetType.MaxValue));
        if (AgilityBonus != 0)
            c.Stats.agility.AddModifier(new StatModifier(AgilityBonus, StatModType.Flat, this, StatSetType.MaxValue));
        if (DexterityBonus != 0)
            c.Stats.dexterity.AddModifier(new StatModifier(DexterityBonus, StatModType.Flat, this, StatSetType.MaxValue));
        if (StaminaBonus != 0)
            c.Stats.stamina.AddModifier(new StatModifier(StaminaBonus, StatModType.Flat, this, StatSetType.MaxValue));

        if (StrengthPercentBonus != 0)
            c.Stats.strength.AddModifier(new StatModifier(StrengthPercentBonus, StatModType.PercentMult, this, StatSetType.MaxValue));
        if (IntellectPercentBonus != 0)
            c.Stats.intellect.AddModifier(new StatModifier(IntellectPercentBonus, StatModType.PercentMult, this, StatSetType.MaxValue));
        if (AgilityPercentBonus != 0)
            c.Stats.agility.AddModifier(new StatModifier(AgilityPercentBonus, StatModType.PercentMult, this, StatSetType.MaxValue));
        if (DexterityPercentBonus != 0)
            c.Stats.dexterity.AddModifier(new StatModifier(DexterityPercentBonus, StatModType.PercentMult, this, StatSetType.MaxValue));
        if (StaminaPercentBonus != 0)
            c.Stats.stamina.AddModifier(new StatModifier(StaminaPercentBonus, StatModType.PercentMult, this, StatSetType.MaxValue));

        if (HPBonus != 0)
            c.Stats.HP.AddModifier(new StatModifier(HPBonus, StatModType.Flat, this, StatSetType.None));
        if (MPBonus != 0)
            c.Stats.MP.AddModifier(new StatModifier(MPBonus, StatModType.Flat, this, StatSetType.None));
        if (SpeedBonus != 0)
            c.Stats.Speed.AddModifier(new StatModifier(SpeedBonus, StatModType.Flat, this, StatSetType.MaxValue));
        if (ATKBonus != 0)
            c.Stats.ATK.AddModifier(new StatModifier(ATKBonus, StatModType.Flat, this, StatSetType.MaxValue));
        if (MATKBonus != 0)
            c.Stats.MATK.AddModifier(new StatModifier(MATKBonus, StatModType.Flat, this, StatSetType.MaxValue));
        if (DEFBonus != 0)
            c.Stats.DEF.AddModifier(new StatModifier(DEFBonus, StatModType.Flat, this, StatSetType.MaxValue));
        if (HitBonus != 0)
            c.Stats.Hit.AddModifier(new StatModifier(HitBonus, StatModType.Flat, this, StatSetType.MaxValue));
        if (DodgeBonus != 0)
            c.Stats.Dodge.AddModifier(new StatModifier(DodgeBonus, StatModType.Flat, this, StatSetType.MaxValue));

        if (HPPercentBonus != 0)
            c.Stats.HP.AddModifier(new StatModifier(HPPercentBonus, StatModType.PercentMult, this, StatSetType.None));
        if (MPPercentBonus != 0)
            c.Stats.MP.AddModifier(new StatModifier(HPPercentBonus, StatModType.PercentMult, this, StatSetType.None));
        if (SpeedPercentBonus != 0)
            c.Stats.Speed.AddModifier(new StatModifier(HPPercentBonus, StatModType.PercentMult, this, StatSetType.MaxValue));
        if (ATKPercentBonus != 0)
            c.Stats.ATK.AddModifier(new StatModifier(HPPercentBonus, StatModType.PercentMult, this, StatSetType.MaxValue));
        if (MATKPercentBonus != 0)
            c.Stats.MATK.AddModifier(new StatModifier(HPPercentBonus, StatModType.PercentMult, this, StatSetType.MaxValue));
        if (DEFPercentBonus != 0)
            c.Stats.DEF.AddModifier(new StatModifier(HPPercentBonus, StatModType.PercentMult, this, StatSetType.MaxValue));
        if (HitPercentBonus != 0)
            c.Stats.Hit.AddModifier(new StatModifier(HPPercentBonus, StatModType.PercentMult, this, StatSetType.MaxValue));
        if (DodgePercentBonus != 0)
            c.Stats.Dodge.AddModifier(new StatModifier(HPPercentBonus, StatModType.PercentMult, this, StatSetType.MaxValue));

        if (EquipmentType == EquipmentType.Helmet)
            c.Equipment.HelmetID = ID;
        if (EquipmentType == EquipmentType.Weapon)
            c.Equipment.WeaponID = ID;
        if (EquipmentType == EquipmentType.Wings)
            c.Equipment.WingsID = ID;
        if (EquipmentType == EquipmentType.Ring)
            c.Equipment.RingID = ID;
        if (EquipmentType == EquipmentType.Armor)
            c.Equipment.ArmorID = ID;
        if (EquipmentType == EquipmentType.Belt)
            c.Equipment.BeltID = ID;
        if (EquipmentType == EquipmentType.Boots)
            c.Equipment.BootsID = ID;
        if (EquipmentType == EquipmentType.Pendant)
            c.Equipment.PendantID = ID;
    }

    public void Unequip(CharacterInformation c)
    {
        c.Stats.strength.RemoveAllModifiersFromSource(this);
        c.Stats.agility.RemoveAllModifiersFromSource(this);
        c.Stats.intellect.RemoveAllModifiersFromSource(this);
        c.Stats.dexterity.RemoveAllModifiersFromSource(this);
        c.Stats.stamina.RemoveAllModifiersFromSource(this);

        c.Stats.HP.RemoveAllModifiersFromSource(this);
        c.Stats.MP.RemoveAllModifiersFromSource(this);
        c.Stats.Speed.RemoveAllModifiersFromSource(this);
        c.Stats.ATK.RemoveAllModifiersFromSource(this);
        c.Stats.MATK.RemoveAllModifiersFromSource(this);
        c.Stats.DEF.RemoveAllModifiersFromSource(this);
        c.Stats.Hit.RemoveAllModifiersFromSource(this);
        c.Stats.Dodge.RemoveAllModifiersFromSource(this);

        if (EquipmentType == EquipmentType.Helmet)
            c.Equipment.HelmetID = "";
        if (EquipmentType == EquipmentType.Weapon)
            c.Equipment.WeaponID = "";
        if (EquipmentType == EquipmentType.Wings)
            c.Equipment.WingsID = "";
        if (EquipmentType == EquipmentType.Ring)
            c.Equipment.RingID = "";
        if (EquipmentType == EquipmentType.Armor)
            c.Equipment.ArmorID = "";
        if (EquipmentType == EquipmentType.Belt)
            c.Equipment.BeltID = "";
        if (EquipmentType == EquipmentType.Boots)
            c.Equipment.BootsID = "";
        if (EquipmentType == EquipmentType.Pendant)
            c.Equipment.PendantID = "";
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
