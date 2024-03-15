using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UnitedInfoPanel : MonoBehaviour
{
    [SerializeField] HeroInfoInterface heroStats;
    public SummonInfoInterface summonInterface;
    [SerializeField] EquipmentPanel heroEquip;
    [SerializeField] Inventory inventory;
    [SerializeField] SkillPanel heroSkillPanel;
    [SerializeField] GameObject[] avatarButtons;
    [SerializeField] Transform modelPlaceholder;
    public GameObject ShowSummonButton;
    public GameObject HideSummonButton;
    private GameObject myAv;
    //private List<CharacterInformation> HeroList = new ();
    private void Start()
    {
        
        heroStats.HeroList = HeroDataManager.instance.CharacterInfo;
        string mainHeroName = heroStats.HeroList.FirstOrDefault(name => name.isMainCharacter)?.Name;
        //stats
        heroStats.HeroPrefab = Extensions.FindHeroEntry(mainHeroName);
        heroStats.Clean();
        //summons
        //summonInterface.Start();
        summonInterface.Owner = Extensions.FindHeroEntry(mainHeroName);
        if (heroStats.HeroPrefab.SummonList.Count > 0)
        {
            summonInterface.EditableSummon = heroStats.HeroPrefab.SummonList[0];
            ShowSummonButton.GetComponent<Button>().interactable = true;
        }
        if (heroStats.HeroPrefab.SummonList.Count == 0)
        {
            summonInterface.EditableSummon = null;
            ShowSummonButton.GetComponent<Button>().interactable = false;
        }
        summonInterface.Refresh();

        //heroSkillPanel.ShowHeroSkills(heroStats.HeroList[0]);
        int index = heroStats.HeroList.FindIndex(hero => hero.isMainCharacter);
        heroSkillPanel.ShowHeroSkills(heroStats.HeroList[index]);
        UpdateModel(index);
        ButtonActions();
        avatarButtons[index].GetComponent<Button>().interactable = false;

    }

    private void OnValidate()
    {
        foreach (var btn in avatarButtons)
        {
            btn.GetComponent<HeroAvatarButton>().infoPanel = this;
        }
    }

    public void ButtonActions()
    {
        for (int i = 0; i < avatarButtons.Length; i++)
        {
            if (heroStats.HeroList[i].isUnlocked)
            {
                avatarButtons[i].GetComponent<Button>().interactable = true;
                avatarButtons[i].GetComponent<HeroAvatarButton>().avatarImage.color = new Color32(255, 255, 255, 255);
            }
            else
            {
                avatarButtons[i].GetComponent<Button>().interactable = false;
                avatarButtons[i].GetComponent<HeroAvatarButton>().avatarImage.color = new Color32(255, 255, 255, 50);
            }    
        }

    }

    public void HealEveryone()
    {
        foreach (CharacterInformation hero in heroStats.HeroList)
        {
            hero.Stats.curHP = hero.Stats.baseHP;
            hero.Stats.curMP = hero.Stats.baseMP;

            if(hero.SummonList.Count > 0)
            {
                foreach (CapturedPets summon in hero.SummonList)
                {
                    summon.Stats.curHP = summon.Stats.baseHP;
                    summon.Stats.curMP = summon.Stats.baseMP;
                }
            }

        }
        heroStats.UpdateStats();
        summonInterface.UpdateStats();
    }

    public void EnableHeroEditing(int a)
    {
        heroStats.HeroPrefab = heroStats.HeroList[a];
        heroStats.Clean();
        //summons
        summonInterface.Owner = heroStats.HeroPrefab;
        if (heroStats.HeroPrefab.SummonList.Count > 0)
        {
            summonInterface.EditableSummon = heroStats.HeroPrefab.SummonList[0];
            ShowSummonButton.GetComponent<Button>().interactable = true;
        }
        if (heroStats.HeroPrefab.SummonList.Count == 0)
        {
            summonInterface.EditableSummon = null;
            ShowSummonButton.GetComponent<Button>().interactable = false;
        }
        summonInterface.Refresh();
        //skills
        heroSkillPanel.ShowHeroSkills(heroStats.HeroPrefab);
        //equip
        UpdateModel(a);
    }

    

    void UpdateModel(int index)
    {
        if (myAv != null)
        {
            Destroy(myAv);
        }
        GameObject summonModel = Extensions.FindModelPrefab(heroStats.HeroList[index].BaseID, true);
        myAv = Instantiate(summonModel, modelPlaceholder.position, Quaternion.Euler(15, 180, 0), modelPlaceholder);
        //myAv.transform.rotation =  Quaternion.Euler(15, 0, 0);
        Extensions.SetLayer(myAv, 5);
    }

    //void DestroyAvatar()
    //{
    //    if (myAv != null)
    //        Destroy(myAv);
    //}

    //private void OnValidate()
    //{
    //    HeroList = HeroData.instance.CharacterInfo;
    //    CreateCharButtons();
    //}

}

