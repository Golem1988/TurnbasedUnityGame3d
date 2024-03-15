using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class SkillPanel : MonoBehaviour
{
    //public CharacterInformation source;
    public Image[] skillIcon;
    public TextMeshProUGUI[] skillLevelText;

    public void ShowHeroSkills(CharacterInformation source)
    {
        foreach (var item in skillIcon)
        {
            item.enabled = false;
        }

        for (int i = 0; i < source.MagicAttacks.Count; i++)
        {
            skillIcon[i].sprite = Extensions.FindActiveSkillSprite(source.MagicAttacks[i]);
            skillIcon[i].enabled = true;
        }
    }

    public void ShowSummonSkills(CapturedPets source)
    {
        foreach (var icon in skillIcon)
        {
            icon.gameObject.SetActive(false);
        }

        if (source != null)
        {
            int j = 0;
            for (int i = 0; i < source.MagicAttacks.Count; i++)
            {
                skillIcon[j].sprite = Extensions.FindActiveSkillSprite(source.MagicAttacks[i]);
                skillIcon[j].gameObject.SetActive(true);
                j++;
            }

            
            for (int i = 0; i < source.PassiveSkills.Count; i++)
            {
                string romanNumber = "";
                //skillIcon[i].sprite = Extensions.FindActiveSkillSprite(source.MagicAttacks[i]);
                skillIcon[j].sprite = GameManager.instance.SkillDatabase.PassiveSkills.FirstOrDefault(skill => skill.ID == source.PassiveSkills[i]).SkillIcon;
                int lvl = GameManager.instance.SkillDatabase.PassiveSkills.FirstOrDefault(skill => skill.ID == source.PassiveSkills[i]).SkillLevel;
                int maxLvl = GameManager.instance.SkillDatabase.PassiveSkills.FirstOrDefault(skill => skill.ID == source.PassiveSkills[i]).MaxLevel;
                if (lvl == 1 && maxLvl > 1)
                    romanNumber = "I";
                if (lvl == 2)
                    romanNumber = "II";
                if (lvl == 3)
                    romanNumber = "III";
                skillLevelText[j].text = romanNumber;
                skillIcon[j].gameObject.SetActive(true);
                j++;
            }
        }





    }

    //private void OnValidate()
    //{
    //    foreach (var item in skillIcon)
    //    {
    //        item.enabled = false;
    //    }
    //}
}
