using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Status Effect/Basic Seal Effect")]
public class SealEffect : StatusEffect
{
    [SerializeField]
    protected bool BanMelee = false;
    [SerializeField]
    protected bool BanMagic = false;
    [SerializeField]
    protected bool BanItemUse = false;
    [SerializeField]
    protected bool BanFlee = false;
    //[SerializeField]
    //protected bool BanCounterattack = false;
    [SerializeField]
    protected bool BanEverything = false;
    [SerializeField]
    protected bool isRemovable = true; //can we remove this seal by remove seals / remove debuffs skills?
    [SerializeField]
    protected bool allowNewSealsIfApplied = true; //can we apply another seal if this one is already active on the target?

    public override void Activate(UnitStateMachine target)
    {
        AllowedActions allowedActions = new();
        allowedActions.canUseMelee = !BanMelee;
        allowedActions.canUseMagic = !BanMagic;
        allowedActions.canAct = !BanEverything;
        allowedActions.canFlee = !BanFlee;
        //allowedActions.canCounterattack = !BanCounterattack;
        allowedActions.canUseItems = !BanItemUse;

        target.AllowedActions = allowedActions;
    }

    public override void DeActivate(UnitStateMachine target)
    {
        //we remove the seal that this SO is applying. If this seal is not applying THIS effect, we go to else, where we just don't touch it.
        //logic: if we ban melee, then it's true in this class and well, if it's true, we want to remove it now so we go like: canUseMelee = true; 
        //if this seal doesn't apply some status but target has it, it will be intact
        //WARNING: This approach may still not be the greatest, because if we will have seals that can be applied on top of eachother and will bear same effect partly, we will be screwed.
        //I might add some additional check or code to prevent this later when / if I will encounter this problem.

        AllowedActions allowedActions = new();
        if (BanMelee && target.AllowedActions.canUseMelee == false)         
            allowedActions.canUseMelee = BanMelee;                          
        else
            allowedActions.canUseMelee = target.AllowedActions.canUseMelee; 
                                                                            
        if (BanMagic && target.AllowedActions.canUseMagic == false)         
            allowedActions.canUseMagic = BanMagic;
        else
            allowedActions.canUseMagic = target.AllowedActions.canUseMagic;

        if (BanEverything && target.AllowedActions.canAct == false)
            allowedActions.canAct = BanEverything;
        else
            allowedActions.canAct = target.AllowedActions.canAct;

        if (BanFlee && target.AllowedActions.canFlee == false)
            allowedActions.canFlee = BanFlee;
        else
            allowedActions.canFlee = target.AllowedActions.canFlee;

        //if (BanCounterattack && target.AllowedActions.canCounterattack == false)
        //  allowedActions.canCounterattack = BanCounterattack;
        //else
        //  allowedActions.canCounterattack = target.AllowedActions.canCounterattack;

        if (BanItemUse && target.AllowedActions.canUseItems == false)
            allowedActions.canUseItems = BanItemUse;
        else
            allowedActions.canUseItems = target.AllowedActions.canUseItems;

        target.AllowedActions = allowedActions;
    }

}
