using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapTileGridCreator.Core;
using System;

[CreateAssetMenu(menuName = "Dungeon/New heal scenario")]
public class DungeonHealScenario : DungeonScenario
{
    [SerializeField]
    protected GameObject healModel;

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
        Spawn(healModel, spot);
    }

    public override void Spawn(GameObject obj, Transform spot)
    {
        Vector3 coord = new();
        coord = spot.position + new Vector3(0f, 1.5f, 0f);
        GameObject newObj = Instantiate(obj, coord, Quaternion.Euler(0, 180, 0));
        //newObj.transform.localScale = obj.transform.localScale;
        spot.gameObject.GetComponent<DungeonCell>().Model = newObj;
    }

    public override void StepOn()
    {
        var heroList = HeroDataManager.instance.CharacterInfo;
        foreach (var hero in heroList)
        {
            if (hero.isActive)
            {
                hero.Stats.HP.CurValue = hero.Stats.HP.MaxValue;
                hero.Stats.MP.CurValue = hero.Stats.MP.MaxValue;
                foreach (var summon in hero.SummonList)
                {
                    summon.Stats.HP.CurValue = summon.Stats.HP.MaxValue;
                    summon.Stats.MP.CurValue = summon.Stats.MP.MaxValue;
                }
            }
        }
        GameManager.instance.Chat.AddToChatOutput("Triggered heal encounter!");
    }
}
