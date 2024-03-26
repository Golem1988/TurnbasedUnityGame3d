using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Basic Attack")]

public class BasicAttack : ActiveSkill
{
    public override void Activate(UnitStateMachine actorSource, UnitStateMachine theTarget)
    {
        //var unit = actorSource.unit;
        //var BSM = actorSource.BSM;
        //List<UnitStateMachine> endTargetList = new();
        //endTargetList.Add(theTarget);
        //calculate the damage
        //float trueDamage = Mathf.Round(unit.Stats.ATK.CurValue * PercentDamageAmount);
        //Actions.OnDoDamage(actorSource, trueDamage, skillType, endTargetList, strikeCount, canCrit, isDodgeable, ignoreDefense, targetStat, !isAttack, applyStatusEffects);
    }
}