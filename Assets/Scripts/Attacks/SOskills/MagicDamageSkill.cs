using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Skills/Magic Damage Skill")]

public class MagicDamageSkill : ActiveSkill
{
    //[SerializeField]
    //protected bool ApplyStatusEffects;
    //[SerializeField]
    //protected List<StatusEffects> applyStatusEffects = new();
    //[SerializeField]
    //protected bool RemoveStatusEffects;
    //[SerializeField]
    //protected List<StatusEffects> removeStatusEffects = new();
    //[SerializeField]
    //protected bool isDodgeable;
    //[SerializeField]
    //protected bool ignoreDefense;

    //private void OnValidate()
    //{
    //    Description = (skillType + " attack that strikes " + targetCount + " target(s) " + strikeCount + " times. It hits " + sortBy + " targets and deals damage to their " + targetStat + ". Damage amount is affected by users " + damageAffectedByStat + " and also deals additional " + FixedDamageAmount + " fixed damage.");
    //}

    //public void Activate(UnitStateMachine actorSource, UnitStateMachine theTarget)
    //{
    //    skillType = SkillType.Magic;
    //    isDodgeable = false;
    //    var gameManager = GameManager.instance;
    //    var BSM = actorSource.BSM;
    //    var unit = actorSource.unit;
    //    var heroUnit = actorSource.GetComponent<Character>().hero;

    //    //so when we chose the ability, we might have had enough HP / MP / Rage to actually choose it
    //    //but at the time we actually cast the ability, we might have lost some HP / Rage / MP so we can't cast the ability anymore
    //    //this is why we will use canActivate check here:
    //    bool canActivate = false;
    //    bool isEnemy = false;
    //    float trueDamage = 0;
    //    List<UnitStateMachine> endTargetList = new();

    //    if (costType == CostType.MP)
    //    {
    //        if (unit.MP.CurValue >= costValue)
    //        {
    //            canActivate = true;
    //            unit.MP.CurValue -= costValue;
    //            Actions.OnBarChange(actorSource, AffectedStat.MP);
    //        }
    //    }
    //    if (costType == CostType.HP)
    //    {
    //        if (unit.HP.CurValue >= costValue)
    //        {
    //            canActivate = true;
    //            unit.HP.CurValue -= costValue;
    //            Actions.OnBarChange(actorSource, AffectedStat.HP);
    //        }
    //    }
    //    if (costType == CostType.RP)
    //    {
    //        if (heroUnit.curRage >= costValue)
    //        {
    //            canActivate = true;
    //            heroUnit.curRage -= costValue;
    //            Actions.OnBarChange(actorSource, AffectedStat.RP);
    //        }
    //    }

    //    //so if we can, let's actually do all the following actions:

    //    if (canActivate)
    //    {
    //        //determine if the actor is enemy or player/summon
    //        if (actorSource.CompareTag("Enemy"))
    //        {
    //            isEnemy = true;
    //        }
    //        //Play attack animations
    //        if (selfVFX != null)
    //        {
    //            Instantiate(selfVFX, actorSource.gameObject.transform);
    //        }

    //        if (attackVFX != null)
    //        {
    //            if (isMassVFX == false)
    //            {
    //                for (int i = 0; i < targetCount; i++)
    //                {
    //                    Instantiate(attackVFX, theTarget.gameObject.transform);
    //                }
    //            }
    //            else
    //            {
    //                //instantiate mass attack VFX on the desired position on the map.
    //                //something to implement later
    //                //Instantiate(attackVFX, transform);
    //            }
    //        }

    //        //calculate the damage
    //        //we need to get unit stat that scales / affects the damage
    //        if (damageAffectedByStat == ScaleStat.Atk)
    //        {
    //            trueDamage = unit.curATK + FixedDamageAmount;
    //        }

    //        if (damageAffectedByStat == ScaleStat.Matk)
    //        {
    //            trueDamage = unit.curMATK + FixedDamageAmount;
    //        }

    //        if (damageAffectedByStat == ScaleStat.FixedDamage)
    //        {
    //            trueDamage = FixedDamageAmount;
    //        }

    //        //sortby
    //        if (targetCount > 1)
    //        {
    //            List<UnitStateMachine> potentialTargets = new();
    //            //we want to determine what list do we take the targets from (From EnemiesInBattle or from Heroesinbattle)
    //            //and create a list of all potential targets based on Enemy / Hero tags
    //            if (isEnemy)
    //            {
    //                for (int i = 0; i < BSM.HeroesInBattle.Count; i++)
    //                {
    //                    potentialTargets.Add(BSM.HeroesInBattle[i].GetComponent<UnitStateMachine>());
    //                }
    //            }
    //            else
    //            {
    //                for (int i = 0; i < BSM.EnemiesInBattle.Count; i++)
    //                {
    //                    potentialTargets.Add(BSM.EnemiesInBattle[i].GetComponent<UnitStateMachine>());
    //                }
    //            }

    //            //if target count is let's say 5 and we only have 1-2-3-4 potentialTargets, then make them equal
    //            if (targetCount > potentialTargets.Count)
    //            {
    //                targetCount = potentialTargets.Count;
    //            }

    //            if (sortBy == SortBy.Random)
    //            {
    //                sortType = SortType.None; //we won't use any ascension / descension order since it's not relevant and will simply add random targets from the list
    //                for (int i = 0; i < targetCount; i++)
    //                {
    //                    int target = Random.Range(0, potentialTargets.Count);
    //                    potentialTargets.Remove(potentialTargets[target]);
    //                    endTargetList.Add(potentialTargets[target]);
    //                }
    //            }
    //            else if (sortBy != SortBy.Random)
    //            {
    //                List<UnitStateMachine> sortList = new();
    //                //add all enemies to the list 
    //                foreach (UnitStateMachine en in potentialTargets)
    //                {
    //                    sortList.Add(en);
    //                }
    //                //remove enemy that were chosen to be the target
    //                sortList.Remove(theTarget);
    //                //sort enemies in the list by the speed, then reverse, so we attack enemies with the highest speed
    //                if (sortBy == SortBy.Speed)
    //                {
    //                    sortList = sortList.OrderBy(x => x.unit.curSpeed).ToList();
    //                }
    //                else if (sortBy == SortBy.HP)
    //                {
    //                    sortList = sortList.OrderBy(x => x.unit.HP.CurValue).ToList();
    //                }
    //                else if (sortBy == SortBy.Attack)
    //                {
    //                    sortList = sortList.OrderBy(x => x.unit.curATK).ToList();
    //                }
    //                else if (sortBy == SortBy.Defense)
    //                {
    //                    sortList = sortList.OrderBy(x => x.unit.curDEF).ToList();
    //                }
    //                //if we want to target the highest, we do list reversal
    //                if (sortType == SortType.HiLo)
    //                {
    //                    sortList.Reverse();
    //                }

    //                for (int i = 0; i < targetCount - 1; i++)
    //                {
    //                    endTargetList.Add(sortList[i]);
    //                }
    //                endTargetList.Add(theTarget);
    //            }


    //        }
    //        else if (targetCount == 1) //if there's only 1 target, just add it to the list and that's it, no sorting or anything
    //        {
    //            endTargetList.Add(theTarget);
    //        }

    //        Actions.OnDoDamage(actorSource, trueDamage, endTargetList, strikeCount, canCrit, isDodgeable, ignoreDefense, targetStat);
    //    }

    //    else
    //    {
    //        gameManager.Chat.AddToChatOutput("Character " + actorSource + " could not use the skill " + SkillName + " and skipped their action.");
    //    }

    //}
}
