using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    public BattleStateMachine BSM;
    public List<StatusList> StatusList = new List<StatusList>();
    public Image BuffIcon;

    private void OnEnable()
    {
        Actions.OnRequestStatusApply += CalculateChances;
        Actions.OnUnitDeath += RemoveStatusOnDeath;
        //Actions.OnBattleEnd += ClearStatusList;
        Actions.OnTurnEnd += TurnEnd;
        Actions.OnTurnStart += TurnStart;
    }

    private void OnDisable()
    {
        Actions.OnRequestStatusApply -= CalculateChances;
        Actions.OnUnitDeath -= RemoveStatusOnDeath;
        //Actions.OnBattleEnd -= ClearStatusList;
        Actions.OnTurnEnd -= TurnEnd;
        Actions.OnTurnStart -= TurnStart;
    }

    //public void ExecuteBuffManager()
    //{
        //if (StatusList.Count > 0)
        //{
        //    if (BSM.battlePhases == BattleStateMachine.BattlePhases.PREFIGHT)
        //        TurnStart();
        //    if (BSM.battlePhases == BattleStateMachine.BattlePhases.POSTFIGHT)
        //        TurnEnd();
        //}   
    //}

    public void TurnStart(BattleStateMachine BSM)
    {
        for (int i = 0; i < StatusList.Count; i++)
        {          
             if (StatusList[i].effect.expirePoint == ExpirePoint.TURN_START)
                 ExecuteEffect(i);
        }
        StatusList.RemoveAll(status => status.duration == 0);
    }

    public void TurnEnd(BattleStateMachine BSM)
    {
        for (int i = 0; i < StatusList.Count; i++)
        {
            if (StatusList[i].effect.expirePoint == ExpirePoint.TURN_END)
                ExecuteEffect(i);
        }
        StatusList.RemoveAll(status => status.duration == 0);
    }

    private void ExecuteEffect(int i)
    {
        var checkTarget = StatusList[i].target;
        //Trigger the effect
        if (StatusList[i].effect.triggerEachTurn)
            StatusList[i].effect.Activate(checkTarget);

        if (checkTarget.currentState != UnitStateMachine.TurnState.DEAD)
        {
            //reduce it's duration
            StatusList[i].duration--;
            //reduce the duration display on the icon
            StatusList[i].icon.GetComponentInChildren<TextMeshProUGUI>().text = StatusList[i].duration.ToString();
            //remove from list if duration is expired
            if (StatusList[i].duration == 0)
            {
                ExpiredEffect(i);
                StatusList[i].effect.DeActivate(StatusList[i].target);
            }
        }    
  
    }

    public void ExpiredEffect(int i)
    {
        //remove instantiated icon
        Destroy(StatusList[i].icon);
        //remove instantiated skill vfx
        Destroy(StatusList[i].vfx);
    }

    private void CalculateChances(UnitStateMachine target, List<StatusEffectList> applyStatusEffects)
    {
        for (int i = 0; i < applyStatusEffects.Count; i++)
        {
            if (Random.Range(0, 100) <= applyStatusEffects[i].applicationChance)
            {
                ApplyStatus(target, applyStatusEffects[i].effect, applyStatusEffects[i].duration);
            }
        }
    }

    public void ApplyStatus(UnitStateMachine target, StatusEffect effect, int duration)
    {
        //if (StatusList already has target with this skill, determine what to do)
        bool alreadyHasStatus = StatusList.Any(status => status.target == target && status.effect == effect);
        if (!alreadyHasStatus)
        {
            var eff = Instantiate(effect.durableEffectVfx, target.transform.position, Quaternion.identity, target.transform);
            var go = Instantiate(BuffIcon, target.ui.BuffPanel.transform);
            go.GetComponentInChildren<TextMeshProUGUI>().text = duration.ToString();
            go.sprite = effect.SkillIcon;

            StatusList Data = new();
            Data.target = target;
            Data.icon = go.gameObject;
            Data.vfx = eff.gameObject;
            Data.effect = effect;
            Data.duration = duration;
            StatusList.Add(Data);
            if (!effect.triggerEachTurn) //if we only have to activate the effect once, then do it right away oppose to waiting for it's trigger
                effect.Activate(target);
        }
        if (alreadyHasStatus && effect.isRefillable == true)
        {
            if (effect.isStackable)
            {
                int index = StatusList.FindIndex(status => status.target == target && status.effect == effect);
                StatusList[index].duration = StatusList[index].duration + duration;
                StatusList[index].icon.GetComponentInChildren<TextMeshProUGUI>().text = StatusList[index].duration.ToString();
            }
            else 
            {
                //this possibly ignores future mechanic I want to implement about passive skill that increases the duration of applied buffs so check this when there will be that mechanic
                int index = StatusList.FindIndex(status => status.target == target && status.effect == effect);
                StatusList[index].duration = duration;
                StatusList[index].icon.GetComponentInChildren<TextMeshProUGUI>().text = StatusList[index].duration.ToString();
            }
        }
        
    }

    void RemoveStatusOnDeath(UnitStateMachine target)
    {
        var StatusList = BSM.BuffManager.StatusList;
        if (StatusList.Count > 0)
        {
            for (int i = 0; i < StatusList.Count; i++)
            {
                if (StatusList[i].target == target)
                {
                    StatusList[i].effect.DeActivate(target);
                    ExpiredEffect(i);
                }
            }
            StatusList.RemoveAll(status => status.target == target);
        }
    }

}
