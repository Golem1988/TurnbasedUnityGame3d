using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SummonSelectButton : MonoBehaviour
{
    public Image summonIcon;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI hpText;
    //public HealthBar healthBar;
    //public ManaBar manaBar;
    public int index;
    public Button Button;

    public void SelectSummon()
    {
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().Input10(index); //save input enemy prefab
    }

    public void Start()
    {
        if (!Button.interactable)
        {
            summonIcon.GetComponent<Image>().color = new Color32(128, 128, 128, 128);
        }

        if (Button.interactable)
        {
            summonIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }

    public void OnValidate()
    {
        if (!Button.interactable)
        {
            summonIcon.GetComponent<Image>().color = new Color32(128, 128, 128, 128); 
        }

        if (Button.interactable)
        {
            summonIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }
}

