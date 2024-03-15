using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class AttackButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ActiveSkill magicAttackToPerform;
    public Sprite emptySlot;
    public Image skillIconObject;
    public Sprite skillIcon;
    public bool isAssigned = false;
    [SerializeField]
    private ActiveSkillTooltip tooltip;


    public void CastMagicAttack()
    {
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().Input4(magicAttackToPerform);
    }

    private void OnValidate()
    {
        if (!isAssigned)
        {
            GetComponent<Image>().sprite = emptySlot;
            if (skillIconObject != null)
            skillIconObject.gameObject.SetActive(false);
        }
        else
        {
            skillIconObject.gameObject.SetActive(true);
            if (skillIcon != null)
                skillIcon = magicAttackToPerform.SkillIcon;
        }
    }

    public void ShowTooltip()
    {
        tooltip.ShowTooltip(this);
    }

    public void HideTooltip()
    {
        tooltip.HideTooltip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isAssigned)
            tooltip.ShowTooltip(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isAssigned)
            tooltip.HideTooltip();
    }

    private void OnDisable()
    {
        if (isAssigned)
            tooltip.HideTooltip();
    }
}
