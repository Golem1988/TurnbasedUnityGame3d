using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Status Effect/Heal over Time")]
public class HealOverTime : StatusEffect
{
    public override void Activate(UnitStateMachine target)
    {
        if (isBuff)
        {
            Actions.OnRestore(target.transform, TheValue, AffectedStat.HP);
        }
        else
        {
            Actions.OnBurn(target.transform, TheValue, AffectedStat.HP);
        }                                                                                                                                     
    }
}
