using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class MainAvatarUI : MonoBehaviour
{
    public Image HeroAvatar;
    public Image SummonAvatar;
    public TMP_Text HeroLevelText;
    public TMP_Text SummonLevelText;
    public Transform BuffHolder;
    public GameObject SummonAvatarObj;
    public GameObject SummonBarObj;
    public GameObject BuffIcon;
    public TMP_Text HeroHpText;
    public TMP_Text HeroMpText;
    public TMP_Text HeroExpText;
    public TMP_Text SummonHpText;
    public TMP_Text SummonMpText;
    public HealthBar heroHealthBar;
    public HealthBar heroManaBar;
    public HealthBar heroExpBar;
    public HealthBar summonHealthBar;
    public HealthBar summonManaBar;

    private void Start()
    {
        Debug.Log("Started looking for info");
        //string mainHeroId = Extensions.FindMainCharacterID();
        string mainHeroId = HeroDataManager.instance.CharacterInfo.FirstOrDefault(name => name.isMainCharacter)?.BaseID;
        //string mainHeroName = Extensions.FindMainCharacterName();
        //string mainHeroName = HeroData.instance.CharacterInfo.FirstOrDefault(name => name.isMainCharacter)?.Name;
        int mainHeroIndex = HeroDataManager.instance.CharacterInfo.FindIndex(hero => hero.isMainCharacter);
        //int mainHeroIndex = Extensions.FindHeroIndex(mainHeroName);
        Debug.Log("Hero index = " + mainHeroIndex.ToString());
        var SummonList = HeroDataManager.instance.CharacterInfo[mainHeroIndex].SummonList;
        var hero = HeroDataManager.instance.CharacterInfo[mainHeroIndex];
        HeroAvatar.sprite = Extensions.FindSprite(mainHeroId, true);
        if (SummonList.Count > 0)
        {
            foreach (CapturedPets summon in SummonList)
            {
                if (summon.active)
                {
                    ChangeSummonAvatar(hero, summon);
                    break;
                }
            }
        }
        else
        {
            SummonAvatarObj.SetActive(false);
            SummonBarObj.SetActive(false);
        }
        HeroHpText.text = hero.Stats.curHP.ToString() + "/" + hero.Stats.baseHP.ToString();
        HeroMpText.text = hero.Stats.curMP.ToString() + "/" + hero.Stats.baseMP.ToString();

        HeroExpText.text = hero.Level.experience.ToString() + "/" + hero.Level.requiredExp.ToString();
        HeroLevelText.text = hero.Level.currentlevel.ToString();

        heroHealthBar.SetSize(hero.Stats.curHP / hero.Stats.baseHP);
        heroManaBar.SetSize(hero.Stats.curMP / hero.Stats.baseMP);
        float curExp = hero.Level.experience;
        float reqExp = hero.Level.requiredExp;
        heroExpBar.SetSize(curExp / reqExp);
    }


    private void OnEnable()
    {
        Actions.OnMainHeroSummonChange += ChangeSummonAvatar;
    }

    private void OnDisable()
    {
        Actions.OnMainHeroSummonChange -= ChangeSummonAvatar;
    }

    private void ChangeSummonAvatar(CharacterInformation hero, CapturedPets summon)
    {
        if (hero.isMainCharacter)
        {
            if (hero.SummonList.Count > 0 && summon.active)
            {
                if (!SummonAvatarObj.activeSelf)
                    SummonAvatarObj.SetActive(true);
                if (!SummonBarObj.activeSelf)
                    SummonBarObj.SetActive(true);
                SummonAvatar.sprite = Extensions.FindSprite(summon.BaseID, false);
                SummonHpText.text = summon.Stats.curHP.ToString() + "/" + summon.Stats.baseHP.ToString();
                SummonMpText.text = summon.Stats.curMP.ToString() + "/" + summon.Stats.baseMP.ToString();
                //SummonExpText.text = summon.Level.experience.ToString() + "/" + summon.Level.requiredExp.ToString();
                SummonLevelText.text = summon.Level.currentlevel.ToString();
                summonHealthBar.SetSize(summon.Stats.curHP / summon.Stats.baseHP);
                summonManaBar.SetSize(summon.Stats.curMP / summon.Stats.baseMP);
            }
            else
            {
                SummonAvatarObj.SetActive(false);
                SummonBarObj.SetActive(false);
            }
        }
    }

}
