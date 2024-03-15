using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActiveSkillTooltip : MonoBehaviour
{
    [SerializeField] TMP_Text SkillNameText;
    //[SerializeField] TMP_Text SkillTypeText;
    [SerializeField] TMP_Text SkillLevelText;
    [SerializeField] TMP_Text SkillCostText;
    //[SerializeField] TMP_Text SkillCostTypeText;
    [SerializeField] TMP_Text SkillDescriptionText;
    [SerializeField] Image SkillIcon;
    //private ActiveSkill skill;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    //private void Start()
    //{
    //    skill = GetComponentInParent<AttackButton>().magicAttackToPerform;
    //}
    public void ShowTooltip(AttackButton button)
    {
        var skill = button.magicAttackToPerform;
        SkillNameText.text = skill.SkillName;
        SkillLevelText.text = skill.SkillLevel.ToString();
        SkillCostText.text = skill.CostValue.ToString() + skill.CostType.ToString();
        SkillDescriptionText.text = skill.Description;
        SkillIcon.sprite = skill.SkillIcon;
        //ItemDescriptionText.text = item.GetDescription();
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
