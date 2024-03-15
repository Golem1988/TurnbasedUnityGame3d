using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Call Summon")]

public class CallSummon : BaseAction
{
    public override void Act(UnitStateMachine actor, UnitStateMachine target, int index)
    {
        actor.ui.animator.Play("Attack");
        Actions.OnSummonSpawn(actor, index);
    }
}
