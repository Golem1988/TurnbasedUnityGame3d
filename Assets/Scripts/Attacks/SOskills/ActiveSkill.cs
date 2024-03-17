using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Active Skill")]
public class ActiveSkill : BaseAttack
{
    //public string SkillName;
    //[SerializeField]
    //[Range(1, 3)] protected int SkillLevel;
    //public Sprite SkillIcon;
    //[SerializeField]
    //[TextArea(5, 7)] protected string Description;
    [SerializeField]
    protected Transform attackVFX;
    [SerializeField]
    protected bool isMassVFX;
    [SerializeField]
    protected Transform selfVFX;
    [SerializeField]
    protected bool isAttack;
    [SerializeField]
    protected SkillType skillType;
    [SerializeField]
    protected CostType costType;
    [SerializeField]
    protected int costValue;
    [SerializeField]
    protected SelectionMode selectionMode; //this thing is has to be checked by Hero/Summon GUI inputs. We aren't doing anything with this in this script
    [SerializeField]
    protected bool canCrit;
    [SerializeField]
    [Range(1, 10)] [Min(1)] protected int targetCount;
    [SerializeField]
    [Range(1, 10)] [Min(1)] public int strikeCount; //how many times repeatedly will the skill strike
    [SerializeField]
    protected TargetType targetType; //not sure if needed at all?! Self / foe / ally multiple choice
    [SerializeField]
    protected AffectedStat targetStat; //Do we deal damage to HP / MP or rage?
    [SerializeField]
    protected ScaleStat damageAffectedByStat;
    [SerializeField]
    protected int FixedDamageAmount;
    [SerializeField]
    [Range(1, 1000)] protected int PercentDamageAmount = 100; //with what %% of affected stat should we amplify / weaken the damage
    [SerializeField]
    protected SortBy sortBy;
    [SerializeField]
    protected SortType sortType;
    [SerializeField]
    protected List<StatusEffectList> applyStatusEffects = new();
    [SerializeField]
    protected bool onlyOnHit;
    [SerializeField]
    protected List<StatusEffectList> removeStatusEffects = new();
    [SerializeField]
    protected bool isDodgeable;
    [SerializeField]
    protected bool ignoreDefense;

    public ActionSettings Test;

    //private int targets;
    private UnitStateMachine source;
    //private UnitStateMachine target;


    //get sets
    public bool IsAttack { get => isAttack; protected set => isAttack = value; }
    public SkillType SkillType { get => skillType; protected set => skillType = value; }
    public CostType CostType { get => costType; protected set => costType = value; }
    public int CostValue { get => costValue; protected set => costValue = value; }
    public int TargetCount { get => targetCount; private set => targetCount = value; }
    public bool OnlyOnHit { get => onlyOnHit; protected set => onlyOnHit = value; }
    public List<StatusEffectList> ApplyStatusEffects { get => applyStatusEffects; protected set => applyStatusEffects = value; }

    //private void OnValidate()
    //{
    //    Description = (skillType + " attack that strikes " + targetCount + " target(s) " + strikeCount + " times. It hits targets based on their " + sortBy + " and deals damage to their " + targetStat + ". Damage amount is affected by users " + damageAffectedByStat + " and also deals additional " + FixedDamageAmount + " fixed damage.");
    //}

    public virtual ActionSettings CalculateSkill(UnitStateMachine actorSource, UnitStateMachine theTarget)
    {
        source = actorSource;
        //target = theTarget;
        var gameManager = GameManager.instance;
        var BSM = source.BSM;
        var StatusList = source.BSM.BuffManager.StatusList;
        var unit = source.unit;
        var heroUnit = source.GetComponent<Character>();
        var targets = TargetCount;

        if (skillType == SkillType.Magic)
            isDodgeable = false;
        bool isEnemy = false;
        float trueDamage = 0;
        List<UnitStateMachine> endTargetList = new();
        List<UnitStateMachine> potentialTargets = new();

        //determine if the actor is enemy or player/summon
        if (source.CompareTag("Enemy"))
        {
            isEnemy = true;
        }

        //calculate the damage
        //we need to get unit stat that scales / affects the damage
        if (damageAffectedByStat == ScaleStat.Atk)
        {
            trueDamage = Mathf.Round((unit.Stats.curATK * PercentDamageAmount) / 100) + FixedDamageAmount;
        }

        if (damageAffectedByStat == ScaleStat.Matk)
        {
            trueDamage = Mathf.Round((unit.Stats.curMATK * PercentDamageAmount) / 100) + FixedDamageAmount;
        }

        if (damageAffectedByStat == ScaleStat.FixedDamage)
        {
            trueDamage = FixedDamageAmount;
        }

        //sortby
        if (TargetCount > 1)
        {
            //we want to determine what list do we take the targets from (From EnemiesInBattle or from Heroesinbattle)
            //and create a list of all potential targets based on Enemy / Hero tags
            if (isEnemy || targetType == TargetType.Ally)
            {
                for (int i = 0; i < BSM.HeroesInBattle.Count; i++)
                {
                    potentialTargets.Add(BSM.HeroesInBattle[i].GetComponent<UnitStateMachine>());
                    Debug.Log("potentialTargets Added allies: " + BSM.HeroesInBattle[i].GetComponent<UnitStateMachine>());
                }
                Debug.Log("potentialTargets Added allies: " + potentialTargets.Count);
            }
            else
            {
                for (int i = 0; i < BSM.EnemiesInBattle.Count; i++)
                {
                    potentialTargets.Add(BSM.EnemiesInBattle[i].GetComponent<UnitStateMachine>());
                    Debug.Log("potentialTargets Added enemies: " + BSM.EnemiesInBattle[i].GetComponent<UnitStateMachine>());
                }
                Debug.Log("potentialTargets Added enemies: " + potentialTargets.Count);
            }

            //if target count is let's say 5 and we only have 1-2-3-4 potentialTargets, then make them equal
            //targets = targetCount;
            Debug.Log("targets = " + targets);
            if (targets > potentialTargets.Count)
            {
                targets = potentialTargets.Count;
                Debug.Log("targets changed = " + targets);
            }

            //remove enemy that were chosen to be the target
            potentialTargets.Remove(theTarget);
            Debug.Log("potentialTargets removed: " + theTarget);
            Debug.Log("potentialTargets after removing initial target: " + potentialTargets.Count);
            for (int i = 0; i < potentialTargets.Count; i++)
            {
                Debug.Log("Potential target [" + i + "] = " + potentialTargets[i]);
            }
            if (sortBy == SortBy.Random && potentialTargets.Count > 1)
            {
                sortType = SortType.None; //we won't use any ascension / descension order since it's not relevant and will simply add random targets from the list
                for (int i = 0; i < targets - 1; i++)
                {
                    int index = Random.Range(0, potentialTargets.Count - 1);
                    endTargetList.Add(potentialTargets[index]);
                    potentialTargets.Remove(potentialTargets[index]);
                    //if (endTargetList.Count == targets - 1)
                    //    break;
                    //if (potentialTargets.Count == 1)
                    //{
                        //endTargetList.Add(potentialTargets[0]);
                        //break;
                    //}
                }
                Debug.Log("endTargetList has targets: " + endTargetList.Count);
            }
            else if (sortBy != SortBy.Random && potentialTargets.Count > 1)
            {
                List<UnitStateMachine> sortList = new();
                //add all enemies to the list 
                foreach (UnitStateMachine en in potentialTargets)
                {
                    sortList.Add(en);
                }

                //sort enemies in the list by the speed, then reverse, so we attack enemies with the highest speed
                if (sortBy == SortBy.Speed)
                {
                    sortList = sortList.OrderBy(x => x.unit.Stats.curSpeed).ToList();
                }
                else if (sortBy == SortBy.HP)
                {
                    sortList = sortList.OrderBy(x => x.unit.Stats.curHP).ToList();
                }
                else if (sortBy == SortBy.Attack)
                {
                    sortList = sortList.OrderBy(x => x.unit.Stats.curATK).ToList();
                }
                else if (sortBy == SortBy.Defense)
                {
                    sortList = sortList.OrderBy(x => x.unit.Stats.curDEF).ToList();
                }
                //if we want to target the highest, we do list reversal
                if (sortType == SortType.HiLo)
                {
                    sortList.Reverse();
                }

                if (targets > 1)
                {
                    for (int i = 0; i < targets - 1; i++)
                    {
                        endTargetList.Add(sortList[i]);
                        Debug.Log("end target list Added " + sortList[i]);
                    }
                }
            }
            else if (potentialTargets.Count == 1)
            {
                endTargetList.Add(potentialTargets[0]);
                Debug.Log("Added " + potentialTargets[0]);
            }
            endTargetList.Add(theTarget);
            Debug.Log("endTargetList has targets: " + endTargetList.Count);
            for (int i = 0; i < endTargetList.Count; i++)
            {
                Debug.Log("endTarget [" + i + "] = " + endTargetList[i]);
            }

        }
        else if (TargetCount == 1) //if there's only 1 target, just add it to the list and that's it, no sorting or anything
        {
            endTargetList.Add(theTarget);
        }

        Test = new ActionSettings { TrueDamage = trueDamage, skillType = SkillType, endTargetList = endTargetList, strikeCount = strikeCount, canCrit = canCrit, isDodgeable = isDodgeable, ignoreDefense = ignoreDefense, targetStat = targetStat, isHeal = !isAttack, applyStatusEffects = ApplyStatusEffects };
        return Test;
    }

    public virtual void PlaySkillVFX(UnitStateMachine source, List<UnitStateMachine> endTargetList)
    {
        var BSM = source.BSM;
        //Play attack animations
        if (selfVFX != null)
        {
            var go = Instantiate(selfVFX, source.gameObject.transform);
            Destroy(go.gameObject, 3f);
        }

        if (attackVFX != null)
        {
            if (isMassVFX == false)
            {
                for (int i = 0; i < endTargetList.Count; i++)
                {
                    var go = Instantiate(attackVFX, endTargetList[i].transform.position, Quaternion.identity, endTargetList[i].transform);
                    Destroy(go.gameObject, 3f);
                }
            }
            else
            {
                Transform spawnPoint;
                if (source.gameObject.CompareTag("Enemy"))
                {
                    spawnPoint = BSM.MassVFXplayer;
                }
                else
                {
                    spawnPoint = BSM.MassVFXenemy;
                }
                var go2 = Instantiate(attackVFX, spawnPoint.transform.position, Quaternion.identity, spawnPoint.transform);
                Destroy(go2.gameObject, 3f);
            }
        }
    }

    public virtual void PayTheCost(UnitStateMachine actorSource)
    {
        var unit = actorSource.GetComponent<UnitAttributes>();
        //if (actorSource.gameObject.CompareTag("Hero"))
        if (CostType == CostType.MP && unit.Stats.curMP >= costValue)
        {
            unit.Stats.curMP -= costValue;
            Actions.OnBarChange(source, AffectedStat.MP);
        }
        if (CostType == CostType.HP && unit.Stats.curHP >= costValue)
        {
            unit.Stats.curHP -= costValue;
            Actions.OnBarChange(source, AffectedStat.HP);
        }
        if (CostType == CostType.RP && unit.Stats.curRage >= costValue)
        {
            unit.Stats.curRage -= costValue;
            Actions.OnBarChange(source, AffectedStat.RP);
        }
    }

    public virtual bool CheckCost(UnitStateMachine actorSource)
    {
        var unit = actorSource.GetComponent<UnitAttributes>();
        if (CostType == CostType.None)
            return true;
        if (CostType == CostType.MP && unit.Stats.curMP >= costValue)
            return true;
        if (CostType == CostType.HP && unit.Stats.curHP >= costValue)
            return true;
        if (CostType == CostType.RP && unit.Stats.curRage >= costValue)
            return true;
        else
        {
            GameManager.instance.Chat.AddToChatOutput(unit.Stats.displayName + " could not use their skill.");
            return false;
        }
    }

    public virtual void Activate(UnitStateMachine actorSource, UnitStateMachine theTarget)
    {
        //source = actorSource;
        //target = theTarget;
        //var gameManager = GameManager.instance;
        //var BSM = source.BSM;
        //var StatusList = source.BSM.BuffManager.StatusList;
        //var unit = source.unit;
        //var heroUnit = source.GetComponent<Character>();
        //targets = TargetCount;

        //if (skillType == SkillType.Magic)
        //    isDodgeable = false;
        ////so when we chose the ability, we might have had enough HP / MP / Rage to actually choose it
        ////but at the time we actually cast the ability, we might have lost some HP / Rage / MP so we can't cast the ability anymore
        ////this is why we will use canActivate check here:
        //bool canActivate = false;
        //bool isEnemy = false;
        //float trueDamage = 0;
        //List<UnitStateMachine> endTargetList = new();

        //if (canActivate)
        //{
        //    //determine if the actor is enemy or player/summon
        //    if (source.CompareTag("Enemy"))
        //    {
        //        isEnemy = true;
        //    }

        //    //calculate the damage
        //    //we need to get unit stat that scales / affects the damage
        //    if (damageAffectedByStat == ScaleStat.Atk)
        //    {
        //        trueDamage = Mathf.Round((unit.Stats.curATK * PercentDamageAmount) / 100) + FixedDamageAmount;
        //    }

        //    if (damageAffectedByStat == ScaleStat.Matk)
        //    {
        //        trueDamage = Mathf.Round((unit.Stats.curMATK * PercentDamageAmount) / 100) + FixedDamageAmount;
        //    }

        //    if (damageAffectedByStat == ScaleStat.FixedDamage)
        //    {
        //        trueDamage = FixedDamageAmount;
        //    }

        //    //sortby
        //    if (targetCount > 1)
        //    {
                
        //        List<UnitStateMachine> potentialTargets = new();
        //        //we want to determine what list do we take the targets from (From EnemiesInBattle or from Heroesinbattle)
        //        //and create a list of all potential targets based on Enemy / Hero tags
        //        if (isEnemy || targetType == TargetType.Ally)
        //        {
        //            for (int i = 0; i < BSM.HeroesInBattle.Count; i++)
        //            {
        //                potentialTargets.Add(BSM.HeroesInBattle[i].GetComponent<UnitStateMachine>());
        //                //Debug.Log("potentialTargets Added allies: " + BSM.HeroesInBattle[i].GetComponent<UnitStateMachine>());
        //            }
        //            //Debug.Log("potentialTargets Added allies: " + potentialTargets.Count);
        //        }
        //        else
        //        {
        //            for (int i = 0; i < BSM.EnemiesInBattle.Count; i++)
        //            {
        //                potentialTargets.Add(BSM.EnemiesInBattle[i].GetComponent<UnitStateMachine>());
        //                //Debug.Log("potentialTargets Added enemies: " + BSM.EnemiesInBattle[i].GetComponent<UnitStateMachine>());
        //            }
        //            //Debug.Log("potentialTargets Added enemies: " + potentialTargets.Count);
        //        }

        //        //if target count is let's say 5 and we only have 1-2-3-4 potentialTargets, then make them equal
        //        targets = targetCount;
        //        //Debug.Log("targets = " + targets);
        //        if (targetCount > potentialTargets.Count)
        //        {
        //            targets = potentialTargets.Count;
        //            //Debug.Log("targets = " + targets);
        //        }

        //        //remove enemy that were chosen to be the target
                
        //        potentialTargets.Remove(target);
        //        //Debug.Log("potentialTargets removed: " + target);
        //        //Debug.Log("potentialTargets after removing initial target: " + potentialTargets.Count);
        //        //for (int i = 0; i < potentialTargets.Count; i++)
        //        //{
        //        //    Debug.Log("Potential target [" + i + "] = " + potentialTargets[i]);
        //        //}
        //        if (sortBy == SortBy.Random && potentialTargets.Count > 1)
        //        {
        //            sortType = SortType.None; //we won't use any ascension / descension order since it's not relevant and will simply add random targets from the list
        //            for (int i = 0; i < targets - 1; i++)
        //            {
        //                int target = Random.Range(0, potentialTargets.Count - 1);
        //                endTargetList.Add(potentialTargets[target]);
        //                potentialTargets.Remove(potentialTargets[target]);
        //                if (potentialTargets.Count == 1) 
        //                {
        //                    endTargetList.Add(potentialTargets[0]);
        //                    break;
        //                }
        //            }
        //            //Debug.Log("TargetList has targets: " + endTargetList.Count);
        //        }
        //        else if (sortBy != SortBy.Random && potentialTargets.Count > 1)
        //        {
        //            List<UnitStateMachine> sortList = new();
        //            //add all enemies to the list 
        //            foreach (UnitStateMachine en in potentialTargets)
        //            {
        //                sortList.Add(en);
        //            }
                    
        //            //sort enemies in the list by the speed, then reverse, so we attack enemies with the highest speed
        //            if (sortBy == SortBy.Speed)
        //            {
        //                sortList = sortList.OrderBy(x => x.unit.Stats.curSpeed).ToList();
        //            }
        //            else if (sortBy == SortBy.HP)
        //            {
        //                sortList = sortList.OrderBy(x => x.unit.Stats.curHP).ToList();
        //            }
        //            else if (sortBy == SortBy.Attack)
        //            {
        //                sortList = sortList.OrderBy(x => x.unit.Stats.curATK).ToList();
        //            }
        //            else if (sortBy == SortBy.Defense)
        //            {
        //                sortList = sortList.OrderBy(x => x.unit.Stats.curDEF).ToList();
        //            }
        //            //if we want to target the highest, we do list reversal
        //            if (sortType == SortType.HiLo)
        //            {
        //                sortList.Reverse();
        //            }

        //            if (targets > 1)
        //            {
        //                for (int i = 0; i < targets-1; i++)
        //                {
        //                    endTargetList.Add(sortList[i]);
        //                    //Debug.Log("end target list Added " + sortList[i]);
        //                }
        //            }
        //        }
        //        else if (potentialTargets.Count == 1)
        //        {
        //            endTargetList.Add(potentialTargets[0]);
        //            //Debug.Log("Added " + potentialTargets[0]);
        //        }
        //        endTargetList.Add(target);
        //        //Debug.Log("endTargetList has targets: " + endTargetList.Count);
        //        for (int i = 0; i < endTargetList.Count; i++)
        //        {
        //            //Debug.Log("endTarget [" + i + "] = " + endTargetList[i]);
        //        }

        //    }
        //    else if (targetCount == 1) //if there's only 1 target, just add it to the list and that's it, no sorting or anything
        //    {
        //        endTargetList.Add(target);
        //    }

        //    //Play attack animations
        //    if (selfVFX != null)
        //    {
        //        var go = Instantiate(selfVFX, source.gameObject.transform);
        //        Destroy(go.gameObject, 3f);
        //    }

        //    if (attackVFX != null)
        //    {
        //        if (isMassVFX == false)
        //        {
        //            for (int i = 0; i < endTargetList.Count; i++)
        //            {
        //                var go = Instantiate(attackVFX, endTargetList[i].transform.position, Quaternion.identity, endTargetList[i].transform);
        //                Destroy(go.gameObject, 3f);
        //            }
        //        }
        //        else
        //        {
        //            Transform spawnPoint;
        //            if (isEnemy)
        //            {
        //                spawnPoint = BSM.MassVFXplayer;
        //            }
        //            else
        //            {
        //                spawnPoint = BSM.MassVFXenemy;
        //            }
        //            var go2 = Instantiate(attackVFX, spawnPoint.transform.position, Quaternion.identity, spawnPoint.transform);
        //            Destroy(go2.gameObject, 3f);
        //        }
        //    }

        //    if (applyStatusEffects.Count > 0) //work on it later to actually implement OnlyApplyOnHit mechanic. Basically add list of effects to the OnDoDamage and make sure to code that
        //    {
        //        for (int i = 0; i < endTargetList.Count; i++)
        //        {
        //            Actions.OnRequestStatusApply(endTargetList[i], applyStatusEffects);
        //        }
        //    }

        //    if (removeStatusEffects.Count > 0) //work on it later to actually implement OnlyApplyOnHit mechanic. Basically add list of effects to the OnDoDamage and make sure to code that
        //    {
        //        for (int i = 0; i < endTargetList.Count; i++)
        //        {
        //            Actions.OnRequestStatusApply(endTargetList[i], removeStatusEffects);
        //        }
        //    }
        //    //Actions.OnDoDamage(source, trueDamage, skillType, endTargetList, strikeCount, canCrit, isDodgeable, ignoreDefense, targetStat, !isAttack, applyStatusEffects);

        //    ActionSettings Test = new ActionSettings { TrueDamage = trueDamage, skillType = skillType, endTargetList = endTargetList, strikeCount = strikeCount, canCrit = canCrit, isDodgeable = isDodgeable, ignoreDefense = ignoreDefense, targetStat = targetStat, isHeal = !isAttack, applyStatusEffects = applyStatusEffects };
        //}

        //else
        //{
        //    gameManager.Chat.AddToChatOutput(source + " could not use the skill " + SkillName + " and skipped their action.");
        //}

    }

}