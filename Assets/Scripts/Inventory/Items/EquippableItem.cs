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

    public void Equip(CharacterInformation c)
    {
        if (StrengthBonus != 0)
            c.Stats.strengthUpdated.AddModifier(new StatModifier(StrengthBonus, StatModType.Flat, this));
        if (IntellectBonus != 0)
            c.Stats.intellectUpdated.AddModifier(new StatModifier(IntellectBonus, StatModType.Flat, this));
        if (AgilityBonus != 0)
            c.Stats.agilityUpdated.AddModifier(new StatModifier(AgilityBonus, StatModType.Flat, this));
        if (DexterityBonus != 0)
            c.Stats.dexterityUpdated.AddModifier(new StatModifier(DexterityBonus, StatModType.Flat, this));
        if (StaminaBonus != 0)
            c.Stats.staminaUpdated.AddModifier(new StatModifier(StaminaBonus, StatModType.Flat, this));

        if (StrengthPercentBonus != 0)
            c.Stats.strengthUpdated.AddModifier(new StatModifier(StrengthPercentBonus, StatModType.PercentMult, this));
        if (IntellectPercentBonus != 0)
            c.Stats.intellectUpdated.AddModifier(new StatModifier(IntellectPercentBonus, StatModType.PercentMult, this));
        if (AgilityPercentBonus != 0)
            c.Stats.agilityUpdated.AddModifier(new StatModifier(AgilityPercentBonus, StatModType.PercentMult, this));
        if (DexterityPercentBonus != 0)
            c.Stats.dexterityUpdated.AddModifier(new StatModifier(DexterityPercentBonus, StatModType.PercentMult, this));
        if (StaminaPercentBonus != 0)
            c.Stats.staminaUpdated.AddModifier(new StatModifier(StaminaPercentBonus, StatModType.PercentMult, this));

        if (HPBonus != 0)
            c.Stats.maxedHP.AddModifier(new StatModifier(HPBonus, StatModType.Flat, this));
        if (MPBonus != 0)
            c.Stats.maxedMP.AddModifier(new StatModifier(MPBonus, StatModType.Flat, this));
        if (SpeedBonus != 0)
            c.Stats.maxedSpeed.AddModifier(new StatModifier(SpeedBonus, StatModType.Flat, this));
        if (ATKBonus != 0)
            c.Stats.maxedATK.AddModifier(new StatModifier(ATKBonus, StatModType.Flat, this));
        if (MATKBonus != 0)
            c.Stats.maxedMATK.AddModifier(new StatModifier(MATKBonus, StatModType.Flat, this));
        if (DEFBonus != 0)
            c.Stats.maxedDEF.AddModifier(new StatModifier(DEFBonus, StatModType.Flat, this));
        if (HitBonus != 0)
            c.Stats.maxedHit.AddModifier(new StatModifier(HitBonus, StatModType.Flat, this));
        if (DodgeBonus != 0)
            c.Stats.maxedDodge.AddModifier(new StatModifier(DodgeBonus, StatModType.Flat, this));

        if (HPPercentBonus != 0)
            c.Stats.maxedHP.AddModifier(new StatModifier(HPPercentBonus, StatModType.PercentMult, this));
        if (MPPercentBonus != 0)
            c.Stats.maxedMP.AddModifier(new StatModifier(HPPercentBonus, StatModType.PercentMult, this));
        if (SpeedPercentBonus != 0)
            c.Stats.maxedSpeed.AddModifier(new StatModifier(HPPercentBonus, StatModType.PercentMult, this));
        if (ATKPercentBonus != 0)
            c.Stats.maxedATK.AddModifier(new StatModifier(HPPercentBonus, StatModType.PercentMult, this));
        if (MATKPercentBonus != 0)
            c.Stats.maxedMATK.AddModifier(new StatModifier(HPPercentBonus, StatModType.PercentMult, this));
        if (DEFPercentBonus != 0)
            c.Stats.maxedDEF.AddModifier(new StatModifier(HPPercentBonus, StatModType.PercentMult, this));
        if (HitPercentBonus != 0)
            c.Stats.maxedHit.AddModifier(new StatModifier(HPPercentBonus, StatModType.PercentMult, this));
        if (DodgePercentBonus != 0)
            c.Stats.maxedDodge.AddModifier(new StatModifier(HPPercentBonus, StatModType.PercentMult, this));

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

        c.Stats.maxedHP.RemoveAllModifiersFromSource(this);
        c.Stats.maxedMP.RemoveAllModifiersFromSource(this);
        c.Stats.maxedSpeed.RemoveAllModifiersFromSource(this);
        c.Stats.maxedATK.RemoveAllModifiersFromSource(this);
        c.Stats.maxedMATK.RemoveAllModifiersFromSource(this);
        c.Stats.maxedDEF.RemoveAllModifiersFromSource(this);
        c.Stats.maxedHit.RemoveAllModifiersFromSource(this);
        c.Stats.maxedDodge.RemoveAllModifiersFromSource(this);

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
