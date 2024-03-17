using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelectButton : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public Sprite emptySlot;
    public Image enemyIconObject;
    public Sprite enemyIcon;
    public bool isAssigned = false;

    private void OnValidate()
    {
        if (!isAssigned)
        {
            GetComponent<Image>().sprite = emptySlot;
            if (enemyIconObject != null)
                enemyIconObject.gameObject.SetActive(false);
        }
        else
        {
            enemyIconObject.gameObject.SetActive(true);
            if (enemyIcon != null)
                enemyIcon = EnemyPrefab.GetComponent<UnitUI>().Avatar;
        }
    }

    //if enemy behind button is not capturable:
    //set the button to be not interactable in case of capture action
    private void Start()
    {
        var BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        if (BSM.HeroChoice.choosenAttack == BSM.HeroesToManage[0].GetComponent<Abilities>().BasicActions[1] && EnemyPrefab != null && EnemyPrefab.GetComponent<Enemy>().enemyType == EnemyType.ELITE)
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
    }
    public void SelectEnemy()
    {
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().Input2(EnemyPrefab); //save input enemy prefab
    }

    public void HideSelector()
    {
        if (EnemyPrefab)
            EnemyPrefab.GetComponent<UnitUI>().Selector.SetActive(false);      
    }
    public void ShowSelector()
    {
        if (EnemyPrefab)
            EnemyPrefab.GetComponent<UnitUI>().Selector.SetActive(true);
    }

}
