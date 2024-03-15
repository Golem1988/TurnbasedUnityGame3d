using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PartyManager : MonoBehaviour
{
    [SerializeField] Transform inBattlePanel;
    [SerializeField] Transform notInBattlePanel;
    [SerializeField] Transform heroAvatarPanel;
    [SerializeField] GameObject HeroAvatarHolder;
    [SerializeField] GameObject panelAvatarHolder;
    //public Sprite emptySlot;
    private HeroDataManager heroManager;
    private int employedHeroes;
    private List<CharacterInformation> heroes = new List<CharacterInformation>();
    private List<AvatarHolder> avatarHolders = new List<AvatarHolder>();
    
    //private void OnValidate()
    //{

    //}

    // Start is called before the first frame update
    void Start()
    {
        heroManager = HeroDataManager.instance;
        heroes = heroManager.CharacterInfo;
        employedHeroes = 0;
        DistributeAvatars();
    }

    void DistributeAvatars()
    {
        heroAvatarPanel.gameObject.SetActive(false);
        foreach (CharacterInformation hero in heroes)
        {
            if (hero.isUnlocked)
            {
                Transform destination;
                if (hero.isActive && employedHeroes < 5)
                    destination = inBattlePanel;
                else
                    destination = notInBattlePanel;

                var go = Instantiate(HeroAvatarHolder, destination.position, Quaternion.identity, destination);
                var holder = go.GetComponent<PartyAvatarHolder>();
                string mainHeroName = HeroDataManager.instance.CharacterInfo.FirstOrDefault(name => name.isMainCharacter)?.Name;
                if (hero.Name == mainHeroName)
                {
                    holder.mainMark.gameObject.SetActive(true);
                    //holder.removeButton.GetComponent<Button>().enabled = false;
                    holder.removeButton.GetComponent<Button>().interactable = false;
                }
                    
                holder.heroName = hero.Name;
                holder.partyManager = this;
                //set sprite for hero avatar
                holder.heroAvatar.sprite = Extensions.FindSprite(hero.BaseID, true);
                //set sprite for summon
                var summons = hero.SummonList;
                int index = summons.FindIndex(summon => summon.active);
                if (index != -1)
                {
                    holder.summonAvatar.enabled = true;
                    holder.summonAvatar.sprite = Extensions.FindSprite(summons[index].BaseID, false);
                }
                //set buttons accordingly
                if (employedHeroes < 5)
                {
                    if (hero.isActive)
                    {
                        holder.removeButton.SetActive(true);
                        employedHeroes++;
                        AddAvatarHolder(hero.BaseID);
                    }
                    else
                        holder.addButton.SetActive(true);
                }
                
            }
        }
        if (employedHeroes > 1)
            heroAvatarPanel.gameObject.SetActive(true);
    }

    public void AddHero(PartyAvatarHolder holder, string heroName)
    {
        if (employedHeroes < 5)
        {
            employedHeroes++;
            holder.transform.SetParent(inBattlePanel);
            var CharInfo = HeroDataManager.instance.CharacterInfo;
            for (int i = 0; i < CharInfo.Count; i++)
            {
                if (CharInfo[i].Name == heroName)
                {
                    CharInfo[i].isActive = true;
                    AddAvatarHolder(CharInfo[i].BaseID);
                    heroAvatarPanel.gameObject.SetActive(true);
                    break;
                }
            }
            holder.addButton.SetActive(false);
            holder.removeButton.SetActive(true);
        }
        else
            GameManager.instance.Chat.AddToChatOutput("<#de0404>Team is already full!</color>");
    }

    public void RemoveHero(PartyAvatarHolder holder, string heroName)
    {
        string mainHeroName = HeroDataManager.instance.CharacterInfo.FirstOrDefault(name => name.isMainCharacter)?.Name;
        if (heroName != mainHeroName)
        {
            if (employedHeroes > 1)
            {
                employedHeroes--;
                if (employedHeroes == 1)
                    heroAvatarPanel.gameObject.SetActive(false);
                holder.transform.SetParent(notInBattlePanel);
                var CharInfo = HeroDataManager.instance.CharacterInfo;
                for (int i = 0; i < CharInfo.Count; i++)
                {
                    if (CharInfo[i].Name == heroName)
                    {
                        CharInfo[i].isActive = false;
                        DestroyAvatar(CharInfo[i].BaseID);
                        break;
                    }
                }
                holder.addButton.SetActive(true);
                holder.removeButton.SetActive(false);
            }
            else
                GameManager.instance.Chat.AddToChatOutput("<#de0404>Team has to have at least 1 hero!</color>");
        }
        else
            GameManager.instance.Chat.AddToChatOutput("<#de0404>Can't remove main character from the team!</color>");
    }

    void AddAvatarHolder(string BaseID)
    {
        var avatar = Extensions.FindSprite(BaseID, true);
        var go = Instantiate(panelAvatarHolder, heroAvatarPanel.position, Quaternion.identity, heroAvatarPanel);
        var holder = go.GetComponent<AvatarHolder>();
        holder.avatar.sprite = avatar;
        holder.baseID = BaseID;
        avatarHolders.Add(holder);
    }

    void DestroyAvatar(string BaseID)
    {
        for (int i = 0; i < avatarHolders.Count; i++)
        {
            if (avatarHolders[i].baseID == BaseID)
            {
                Destroy(avatarHolders[i].gameObject);
                avatarHolders.Remove(avatarHolders[i]);
                break;
            }
        }
    }
}
