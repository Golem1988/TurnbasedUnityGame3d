using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Status Effect")]
public class StatusEffect : BaseAttack
{
    public Transform durableEffectVfx;
    public bool isStackable;
    public bool isRefillable;
    public bool isBuff;
    [SerializeField]
    protected UnitStats stat;
    [SerializeField]
    protected StatModType modType;
    [SerializeField]
    protected float theValue;
    public ExpirePoint expirePoint; //turn start / turn end / battle end
    //[SerializeField]
    public bool triggerEachTurn;
    
    public float TheValue { get => theValue; protected set => theValue = value; }

    public virtual void Activate(UnitStateMachine target)
    {
        //Actions.OnRestore(target.transform, value, AffectedStat.HP);
        //we possibly want to have some code sheet with all the possible status effects somewhere and based on the SO settings we want to trigger different effects.

        //at this point I still have no real idea how to implement that, something to think about

        //Heal each turn
        //Debug.Log("virtual void triggered");
        //Poison / burn each turn hp / mp

        //Apply stat debuffs like decrease speed, decrease attack / magic attack, defense

        //Apply seals / forbid actions. That should be status effects, since they have duration

        //apply skills that have duration like make all incoming damage 0 for few turns

    }

    public virtual void DeActivate(UnitStateMachine target)
    {

    }
}
