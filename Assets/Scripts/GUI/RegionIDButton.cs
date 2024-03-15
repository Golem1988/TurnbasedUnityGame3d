using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RegionIDButton: MonoBehaviour
{
    public int region;
    public TextMeshProUGUI buttonText;
    public ForceBattle forceBattle;

    public void TransmitID()
    {
        GameManager.instance.curRegions = region;
        GameManager.instance.gameState = GameStates.IDLE;
        forceBattle.gameObject.SetActive(false);
        GameManager.instance.StartBattle();
    }
}
