using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapTileGridCreator.Core;
using System;

[CreateAssetMenu(menuName = "Dungeon/New chest scenario")]
public class DungeonChestScenario : DungeonScenario
{
    [SerializeField]
    protected ItemDrops[] loot;
    [SerializeField]
    protected GameObject chestModel;
    public ItemDrops[] Loot { get => loot; protected set => loot = value; }

    private void OnValidate()
    {
        if (ID == null || ID == "")
        {
            ID = "scenario" + Guid.NewGuid().ToString();
        }
    }

    public override void Activate(DungeonCell cell)
    {
        var spot = cell.transform;
        Spawn(chestModel, spot);
    }

    public override void Spawn(GameObject obj, Transform spot)
    {
        Vector3 coord = new();
        coord = spot.position + new Vector3(0f, 0.5f, 0f);
        GameObject newObj = Instantiate(obj, coord, Quaternion.Euler(0, 180, 0));
        newObj.transform.localScale = new Vector3(2f, 2f, 2f);
        //newObj.transform.localScale = obj.transform.localScale;
        spot.gameObject.GetComponent<DungeonCell>().Model = newObj;
    }

    public override void StepOn()
    {
        GameManager.instance.Chat.AddToChatOutput("Triggered chest encounter!");
        if (Actions.OnDungeonChestTrigger != null)
        {
            Actions.OnDungeonChestTrigger.Invoke();
        }
        
    }
}
