using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Passive Skill")]
public class PassiveSkill : BaseAttack
{
    [SerializeField]
    protected int maxLevel;
    [SerializeField]
    protected UnitStats increaseStat;
    [SerializeField]
    protected UnitStats decreaseStat;
    //[SerializeField]
    //protected bool applyStatusOnSelf;
    //[SerializeField]
    //protected bool removeStatusFromSelf;
    //[SerializeField]
    //protected ExpirePoint removeWhen;
    [SerializeField]
    protected List<StatusEffectList> applyStatusEffects = new();
    [SerializeField]
    protected bool onlyOnHit;
    [SerializeField]
    protected List<StatusEffectList> removeStatusEffects = new();
    //[SerializeField]
    public Trigger trigger;
    [SerializeField]
    protected PassiveSkill nextLevelSkill;
    //When does the skill apply
    //OnDamage?
    //OnBattleStart?
    //OnBeingHit?
    //AfterDamageDone?
    //Constantly?
    public bool OnlyOnHit { get => onlyOnHit; protected set => onlyOnHit = value; }
    public int MaxLevel { get => maxLevel; protected set => maxLevel = value; }
    public List<StatusEffectList> ApplyStatusEffects { get => applyStatusEffects; protected set => applyStatusEffects = value; }
    public List<StatusEffectList> RemoveStatusEffects { get => removeStatusEffects; protected set => removeStatusEffects = value; }
    public PassiveSkill NextLevelSkill { get => nextLevelSkill; protected set => nextLevelSkill = value; }
    //we shall implement all those options in corresponding files
    //and call to passive skill list and find out if any of them shall be triggered

    //for example we add the line of code something like this after dealing damage:
    // for (int i = 0; i<PassiveSkills.Count; i++)
    //{
    //             if (PassiveSkills[i].trigger == Trigger.After_Damage)
    //             {
    //                 PassiveSkills[i].Activate(this);
    //             }
    //}

    public virtual void Activate(UnitStateMachine actor, ActionSettings actionSettings)
    {
        //exectute what the skill should do
    }

    public virtual List<StatusEffectList> ApplyEffect(List<StatusEffectList> effects)
    {
        List<StatusEffectList> NewEffects = new(effects);
        foreach (StatusEffectList status in ApplyStatusEffects)
        {
            NewEffects.Add(status);
        }

        //Debug.Log("ActionSettings updated with new information");
        return NewEffects;
    }

}
