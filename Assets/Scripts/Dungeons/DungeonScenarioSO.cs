using System;
using System.Collections.Generic;
using UnityEngine;
using MapTileGridCreator.Core;

[CreateAssetMenu(menuName = "Dungeon/New scenario")]
public class DungeonScenario : ScriptableObject
{
    [SerializeField]
    protected string id;
    [SerializeField]
    protected bool isFight;
    [SerializeField]
    protected bool isChest;
    [SerializeField]
    protected bool isHeal;
    [SerializeField]
    protected GameObject enemyModel;
    [SerializeField]
    protected GameObject chestModel;
    [SerializeField]
    protected GameObject healModel;
    [SerializeField]
    protected List<EncounterTeam> battleTeam = new();
    [SerializeField]
    protected List<DungeonLootChest> loot = new();

    public string ID { get => id; protected set => id = value; }
    public bool IsFight { get => isFight; protected set => isFight = value; }
    public bool IsChest { get => isChest; protected set => isChest = value; }
    public bool IsHeal { get => isHeal; protected set => isHeal = value; }
    public List<EncounterTeam> BattleTeam { get => battleTeam; protected set => battleTeam = value; }
    public List<DungeonLootChest> Loot { get => loot; protected set => loot = value; }


    private void OnValidate()
    {
        if (id == null || id == "")
        {
            id = "scenario" + Guid.NewGuid().ToString();
        }
        if (BattleTeam.Count > 0)
        {
            enemyModel = BattleTeam[0].EnemyUnit.ModelPrefab;
        }
    }

    public void Activate(Cell cell)
    {
        var spot = cell.transform;
        if (IsFight)
        {
            //Vector3 coord = spot.position + new Vector3(0f, 0.5f, 0f);
            Spawn(enemyModel, spot);
        }
        else if (IsChest)
        {
            Spawn(chestModel, spot);
        }
        else if (IsHeal)
        {
            Spawn(healModel, spot);
        }
    }

    private void Spawn(GameObject obj, Transform spot)
    {
        Vector3 coord = new();
        if (IsFight || IsChest)
            coord = spot.position + new Vector3(0f, 0.5f, 0f);
        else
            coord = spot.position + new Vector3(0f, 1.5f, 0f);
        GameObject newObj = Instantiate(obj, coord, Quaternion.Euler(0, 180, 0));
        //newObj.transform.localScale = obj.transform.localScale;
        spot.gameObject.GetComponent<DungeonCell>().Model = newObj;
    }
}
