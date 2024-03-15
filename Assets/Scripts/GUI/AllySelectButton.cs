using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllySelectButton : MonoBehaviour
{
    public GameObject AllyPrefab;
    public Sprite emptySlot;
    public Image allyIconObject;
    public Sprite allyIcon;
    public bool isAssigned = false;

    private void OnValidate()
    {
        if (!isAssigned)
        {
            GetComponent<Image>().sprite = emptySlot; 
            if (allyIconObject != null)
                allyIconObject.gameObject.SetActive(false);
        }
        else
        {
            allyIconObject.gameObject.SetActive(true);
            if (allyIcon != null)
                allyIcon = AllyPrefab.GetComponent<UnitUI>().heroAvatar;
        }
    }
    public void SelectAlly()
    {
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().Input6(AllyPrefab); //save input enemy prefab
    }

    public void HideSelector()
    {
        if (AllyPrefab)
            AllyPrefab.GetComponent<UnitUI>().Selector.SetActive(false);
    }
    public void ShowSelector()
    {
        if (AllyPrefab)
            AllyPrefab.GetComponent<UnitUI>().Selector.SetActive(true);
    }
}
