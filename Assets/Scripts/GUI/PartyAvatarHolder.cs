using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PartyAvatarHolder : MonoBehaviour
{
    public Image heroAvatar;
    public Image summonAvatar;
    public GameObject addButton;
    public GameObject removeButton;
    public string heroName;
    public PartyManager partyManager;
    public Image mainMark; //is hero main hero?

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
        if (hero.Name == heroName)
        {
            if (hero.SummonList.Any(n => n.active))
            {
                summonAvatar.enabled = true;
                summonAvatar.sprite = Extensions.FindSprite(summon.BaseID, false);
            }
            else
                summonAvatar.enabled = false;
        }

    }

    public void AddHero()
    {
        partyManager.AddHero(this, heroName);
    }

    public void RemoveHero()
    {
        partyManager.RemoveHero(this, heroName);
    }
}
