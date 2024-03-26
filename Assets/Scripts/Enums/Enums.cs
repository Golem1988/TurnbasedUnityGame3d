public enum SkillType
{
    Melee = 0,
    Ranged = 1,
    Magic = 2
}

public enum CostType
{
    None = 0,
    MP = 1,
    RP = 2, //rage
    HP = 3
}

public enum AffectedStat
{
    HP = 0,
    MP = 1,
    RP = 2 //rage
}

public enum SelectionMode
{
    Single = 0,
    All = 1
}

public enum TargetType
{
    Self = 1,
    Foe = 2,
    Ally = 3
}

public enum StatusCalcType
{
    Flat = 1,
    Percentage = 2
}

public enum ScaleStat
{
    Atk = 0,
    Matk = 1,
    FixedDamage = 2
}

public enum VFXtarget
{
    Self = 1,
    Target = 2
}

public enum SortBy
{
    Random = 0,
    HP = 1,
    Speed = 2,
    Attack = 3,
    Defense = 4
}

public enum SortType
{
    None = 0,
    LoHi = 1,
    HiLo = 2
}

public enum EnemyType
{
    NORMAL = 0,
    STRONG = 1,
    ELITE = 2,
    MINIBOSS = 3,
    MVP = 4,
    BABY = 5,
    MUTANT = 6
}

public enum EnemyRarity
{
    COMMON = 0,
    UNCOMMON = 1,
    RARE = 2,
    LEGENDARY = 3
}

public enum SummonType
{
    NORMAL = 1,
    BABY = 2,
    MUTANT = 3,
    MONSTER = 4
}

public enum SummonRarity
{
    COMMON = 1,
    UNCOMMON = 2,
    RARE = 3,
    SUPERRARE = 4
}

public enum SkillElement
{
    NEUTRAL = 1,
    FIRE = 2,
    WIND = 3,
    WATER = 4,
    HOLY = 5,
    DARK = 6
}

public enum UnitStats
{
    NOT_ACTIVE = 0,
    HP = 1,
    MP = 2,
    DEF = 3,
    MDEF = 4,
    Speed = 5,
    Dodge = 6,
    Hit = 7,
    Critrate = 8,
    CritStrength = 9,
    ATK = 10,
    MATK = 11
}

public enum ExpirePoint
{
    TURN_START = 0,
    TURN_END = 1,
    //BATTLE_END = 3
}

public enum StatModType
{
    Flat = 100,
    PercentAdd = 200,
    PercentMult = 300,
}

public enum StatSetType
{
    None,
    MaxValue, // Add more types as needed
}

public enum ActivateOn //event at which the ability is getting activated
{
    TURN_START = 0,
    TURN_END = 1,
    BATTLE_START = 2,
    DEATH = 3
}

public enum Trigger //event at which the ability is getting activated
{
    DEAL_MELEE_DAMAGE = 0, //trigger when we DEAL damage
    TAKE_MELEE_DAMAGE = 1, //trigger when we TAKE damage
    DEATH = 2, //triggers some actions in case of death like ressurrect or undead
    KILL = 3, //trigger some events in case the attack killed an enemy
    CONSTANT = 4, //out of battle, in battle, basically applies to stat modifying passive skills
    DEAL_MAGIC_DAMAGE = 5,
    TAKE_MAGIC_DAMAGE = 6
}

public enum HeroSchool
{
    School0 = 0,
    School1 = 1,
    School2 = 2,
    School3 = 3,
    School4 = 4,
    School5 = 5
}

public enum GameStates
{
    WORLD_STATE,
    TOWN_STATE,
    BATTLE_STATE,
    IDLE,
    DUNGEON_STATE,
}

public enum ChatMessageType
{
    None = 0,
    System = 1,
    Battle = 2,
    Warning = 3,
}