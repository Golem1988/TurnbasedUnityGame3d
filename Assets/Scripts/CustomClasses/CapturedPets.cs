using Kryz.CharacterStats;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CapturedPetsOld
{
    public string theName;
    //public string prefabName;
    public GameObject thePrefab;

    //[Header("Stats")]
    //public CharacterStat strength;
    //public CharacterStat intellect;
    //public CharacterStat dexterity;
    //public CharacterStat agility;
    //public CharacterStat stamina;

    [Header("Abilities")]
    public List<ActiveSkill> attacks = new();
    public List<ActiveSkill> MagicAttacks = new();
    //public List<BaseAttack> attacks = new List<BaseAttack>();
    //public List<BaseAttack> MagicAttacks = new List<BaseAttack>();
    //public List<string> Attacks = new();
    //public List<string> MagicAttacks = new();
}
