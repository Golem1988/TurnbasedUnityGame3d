using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapTileGridCreator.Core;
using System;

[CreateAssetMenu(menuName = "Dungeon/New battle scenario")]
public class DungeonBattleScenario : DungeonScenario
{
    [SerializeField]
    protected List<EncounterTeam> battleTeam = new();
    [SerializeField]
    protected GameObject enemyModel;
    public List<EncounterTeam> BattleTeam { get => battleTeam; protected set => battleTeam = value; }
    
    
    private void OnValidate()
    {
        if (ID == null || ID == "")
        {
            ID = "scenario" + Guid.NewGuid().ToString();
        }

        if (BattleTeam.Count > 0 && BattleTeam[0].EnemyUnit)
        {
            enemyModel = BattleTeam[0].EnemyUnit.ModelPrefab;
        }

    }

    public override void Activate(Cell cell)
    {
        var spot = cell.transform;
        Spawn(enemyModel, spot);
    }

    public override void Spawn(GameObject obj, Transform spot)
    {
        Vector3 coord = new();
        coord = spot.position + new Vector3(0f, 0.5f, 0f);
        GameObject newObj = Instantiate(obj, coord, Quaternion.Euler(0, 180, 0));
        //newObj.transform.localScale = obj.transform.localScale;
        spot.gameObject.GetComponent<DungeonCell>().Model = newObj;
    }

    public override void StepOn()
    {
        GameManager.instance.Chat.AddToChatOutput("Triggered battle encounter!");
    }
}
