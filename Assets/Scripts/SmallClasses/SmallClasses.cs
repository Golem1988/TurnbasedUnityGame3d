using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattlePosition
{
    public GameObject UnitOnSpot;
    public int Spot;
}

[System.Serializable]
public class HandleTurn
{
    public string AttackersName; //name of attacker
    public float attackersSpeed; //speed of the attacker
    public string Type; // 
    public GameObject Attacker; //who attacks
    public GameObject AttackersTarget; // who is main target
    //which type of attack is performed
    public ActiveSkill choosenAttack;
    public BaseAction choosenAction;
    //multipurpouse ID
    public int index;
}

[System.Serializable]
public class StatusList
{
    public StatusEffect effect;
    public UnitStateMachine target;
    public int duration;
    public GameObject icon;
    public GameObject vfx;
}

[System.Serializable]
public class StatusEffectList
{
    public StatusEffect effect;
    [Range(0, 100)] public float applicationChance;
    [Range(1, 10)] public int duration = 1;
}

[System.Serializable]
public class ActiveSkillSet
{
    public ActiveSkill possibleSkill;
    [Range(0, 100)] public int skillSpawnChance = 25;
}

[System.Serializable]
public class PassiveSkillSet
{
    public PassiveSkill posPassive;
    [Range(0, 100)] public int skillSpawnChance = 25;
}

[System.Serializable]
public class SkillSet
{
    public List<PassiveSkill> PassiveSkills;
    public List<ActiveSkill> ActiveSkills;
}

[System.Serializable]
public class CapturedPets
{
    [Header("IDs")]
    public string BaseID;
    public string UniqueID;
    [Header("Battle Bools")]
    public bool isDeployable;
    public bool active;
    [Header("Stat Info")]
    public BaseClass Stats;
    public Level Level;
    [Header("Types")]
    public EnemyType Type;
    public EnemyRarity Rarity;
    [Header("Skills")]
    public List<string> MagicAttacks = new();
    public List<string> PassiveSkills = new();
}

[System.Serializable]
public class CharacterInformation
{
    [Header("IDs")]
    public string Name;
    public string BaseID;
    [Header("General Bools")]
    public bool isUnlocked;
    public bool isActive;
    public bool isMainCharacter;
    [Header("Stat Info")]
    public BaseClass Stats;
    public Level Level;
    [Header("Skills")]
    public List<string> MagicAttacks = new();
    public List<string> PassiveSkills = new();
    [Header("SummonList")]
    public List<CapturedPets> SummonList = new();
    public int MaxSummonSlots;
    [Header("Equipment")]
    public List<string> Equipment = new();
}

//[System.Serializable]
//public class EquippedItems
//{
//    [Header("ItemSlots")]
//    public string HelmetID;
//    public string ArmorID;
//    public string WeaponID;
//    public string BootsID;
//    public string RingID;
//    public string NecklaceID;
//    public string BeltID;
//    public string WingsID;
//}

[System.Serializable]
public class HeroPool
{
    public GameObject thePrefab;
    public bool unlocked;
    public BaseClass Stats;
    public Level Level;
}

[System.Serializable]
public class SkillTree
{
    //implement the hero skill tree later
}

[System.Serializable]
public class Scenario
{
    public int TurnNumber;
    public BaseAttack Attack;
}

[System.Serializable]
public class RegionData
{
    public string regionName;
    public int maxAmountEnemys = 8;
    public string battleScene;
    public List<EnemyUnit> possibleEnemys = new();
    public int EnemyLevel;
    public List<ItemDrops> itemDrops = new();
}

[System.Serializable]
public class EncounterTeam
{
    public EnemyUnit EnemyUnit;
    public int EnemyLevel;
}

[System.Serializable]
public class DungeonLootChest
{
    public Item Item;
    public int DropChance;
}

[System.Serializable]
public class ItemDrops
{
    public Item item;
    public float dropChance;
}

[System.Serializable]
public class UnitDatabase
{
    public GameObject HeroPrefab;
    public GameObject EnemyPrefab;
    public GameObject SummonPrefab;

    public List<EnemyUnit> EnemyList;
    public List<HeroUnit> HeroList;

    [Header("Statpoint growth")]
    public int statpointsPerLevel = 5;
    public int statIncreasePerLevel = 1;

    [Header("Attribute multipliers")]
    public float hpPerStr = 10;
    public float atkPerStr = 10;
    public float mpPerInt = 10;
    public float atkPerInt = 3;
    public float spdPerAgi = 2;
    public float dodgePerAgi = 3;
    public float hitPerDex = 2;
    public float atkPerDex = 2;
    public float hpPerSta = 25;
    public float defPerSta = 5;
    public float matkPerInt = 10;
    public float matkPerStr = 3;

    [Header("Summon / enemy further multipliers")]
    public float NormalSummonStatMultiplier = 1f;
    public float BabySummonStatMultiplier = 1f;
    public float MutantSummonStatMultiplier = 1.5f;
    public float StrongEnemyStatMultiplier = 1.5f;
    public float EliteEnemyStatMultiplier = 2f;

    [Header("Summon skill learn")]
    public float SkillLearnChance = 50f;

}

[System.Serializable]
public struct ActionSettings
{
    public float TrueDamage;
    public SkillType skillType;
    public List<UnitStateMachine> endTargetList;
    public int strikeCount;
    public bool canCrit;
    public bool isDodgeable;
    public bool ignoreDefense;
    public AffectedStat targetStat;
    public bool isHeal;
    public List<StatusEffectList> applyStatusEffects;
}

[System.Serializable]
public class AllowedActions
{
    public bool canUseMelee = true;
    public bool canUseMagic = true;
    public bool canAct = true;
    public bool canFlee = true;
    //public bool canCounterattack = true;
    public bool canUseItems = true;
}