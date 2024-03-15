using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Capture")]

public class Capture : ActiveSkill
{
    public override ActionSettings CalculateSkill(UnitStateMachine actorSource, UnitStateMachine theTarget)
    {
        //Actions.OnSummonCapture(actorSource, theTarget);
        List<UnitStateMachine> endTargetList = new();
        endTargetList.Add(theTarget);
        var Calculation = new ActionSettings { TrueDamage = 0f, skillType = skillType, endTargetList = endTargetList, strikeCount = strikeCount, canCrit = canCrit, isDodgeable = isDodgeable, ignoreDefense = ignoreDefense, targetStat = targetStat, isHeal = !isAttack, applyStatusEffects = applyStatusEffects };
        return Calculation;
    }

    public override void PlaySkillVFX(UnitStateMachine source, List<UnitStateMachine> endTargetList)
    {
        if (attackVFX != null)
        {
            var go = Instantiate(attackVFX, endTargetList[0].transform.position, Quaternion.identity, endTargetList[0].transform);
            Destroy(go.gameObject, 3f);
        }
        Actions.OnSummonCapture(source, endTargetList[0]);
    }


}