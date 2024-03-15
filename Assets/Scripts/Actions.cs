using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Actions
{
    public static Action<Transform, bool, float, bool, AffectedStat> OnDamageReceived; //Target, isCritical?, damage amount, isHeal?
    public static Action<Transform, float, AffectedStat> OnRestore; //Target, heal amount, stat to heal
    public static Action<Transform, float, AffectedStat> OnBurn; //Target, burn amount, stat to burn
    public static Action<Transform> OnDodge; //Target
    //public static Action<GameObject> OnEnemySpawn; //when we spawn an enemy into battle
    public static Action<Transform, float> OnRessurect; //when we spawn an enemy into battle
    public static Action<UnitStateMachine> OnZeroHealth; //when target health becomes 0
    public static Action<UnitStateMachine> OnUnitDeath; //when target dies
    public static Action<UnitStateMachine, float, SkillType, List<UnitStateMachine>, int, bool, bool, bool, AffectedStat, bool, List<StatusEffectList>> OnDoDamage;
    public static Action<UnitStateMachine, AffectedStat> OnBarChange; //rage bar mana bar hp bar
    public static Action<UnitStateMachine, UnitStateMachine> OnSummonCapture;
    public static Action<UnitStateMachine, int> OnSummonSpawn; //we call summon to battle
    public static Action<UnitStateMachine, List<StatusEffectList>> OnRequestStatusApply; //if skill applies or removes status effects List of targets, list of status effects to apply
    public static Action<UnitStateMachine, List<StatusEffectList>> OnRequestStatusRemove; //if skill applies or removes status effects List of targets, list of status effects to remove
    public static Action<BattleStateMachine> OnBattleStart; //do all the actions on the battle start
    public static Action<BattleStateMachine> OnBattleEnd; //do all the actions on the battle end
    public static Action<BattleStateMachine> OnTurnEnd; //do all the actions on the battle end
    public static Action<BattleStateMachine> OnTurnStart; //do all the actions on the battle end
    public static Action<int> OnHeroEdit; //switch hero interface UI
    public static Action<CharacterInformation, CapturedPets> OnMainHeroSummonChange;
}
