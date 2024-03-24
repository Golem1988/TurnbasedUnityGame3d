using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

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
    private Scene currentScene;
    private void Start()
    {
        Debug.Log("Started looking for info");
        string mainHeroId = HeroDataManager.instance.CharacterInfo.FirstOrDefault(name => name.isMainCharacter)?.BaseID;
        int mainHeroIndex = HeroDataManager.instance.CharacterInfo.FindIndex(hero => hero.isMainCharacter);
        //Debug.Log("Hero index = " + mainHeroIndex.ToString());
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
        ChangeHeroBars(hero);
    }


    private void OnEnable()
    {
        Actions.OnMainHeroSummonChange += ChangeSummonAvatar;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        Actions.OnMainHeroSummonChange -= ChangeSummonAvatar;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void ChangeHeroBars(CharacterInformation hero)
    {
        HeroHpText.text = hero.Stats.curHP.BaseValue.ToString() + "/" + hero.Stats.baseHP.BaseValue.ToString();
        HeroMpText.text = hero.Stats.curMP.BaseValue.ToString() + "/" + hero.Stats.baseMP.BaseValue.ToString();

        HeroExpText.text = hero.Level.CUR_EXP.ToString() + "/" + hero.Level.NEXT_EXP.ToString();
        HeroLevelText.text = hero.Level.currentlevel.ToString();

        heroHealthBar.SetSize(hero.Stats.curHP.BaseValue / hero.Stats.baseHP.BaseValue);
        heroManaBar.SetSize(hero.Stats.curMP.BaseValue / hero.Stats.baseMP.BaseValue);
        float curExp = hero.Level.CUR_EXP;
        if (curExp == 0)
            curExp = 0.01f;
        float reqExp = hero.Level.NEXT_EXP;
        heroExpBar.SetSize(curExp / reqExp);
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
                SummonHpText.text = summon.Stats.curHP.BaseValue.ToString() + "/" + summon.Stats.baseHP.BaseValue.ToString();
                SummonMpText.text = summon.Stats.curMP.BaseValue.ToString() + "/" + summon.Stats.baseMP.BaseValue.ToString();
                //SummonExpText.text = summon.Level.experience.ToString() + "/" + summon.Level.requiredExp.ToString();
                SummonLevelText.text = summon.Level.currentlevel.ToString();
                summonHealthBar.SetSize(summon.Stats.curHP.BaseValue / summon.Stats.baseHP.BaseValue);
                summonManaBar.SetSize(summon.Stats.curMP.BaseValue / summon.Stats.baseMP.BaseValue);
            }
            else
            {
                SummonAvatarObj.SetActive(false);
                SummonBarObj.SetActive(false);
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = SceneManager.GetActiveScene();
        int mainHeroIndex = HeroDataManager.instance.CharacterInfo.FindIndex(hero => hero.isMainCharacter);
        var hero = HeroDataManager.instance.CharacterInfo[mainHeroIndex];
        var summon = hero.SummonList.FirstOrDefault(summon => summon.active);
        if (currentScene.name == "Village")
        {
            ChangeSummonAvatar(hero, summon);
            ChangeHeroBars(hero);
        }
    }

}
