using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapTileGridCreator.Core;

[System.Serializable]
public class DungeonCell : MonoBehaviour
{
    //public bool isCleared;
    //public string ScenarioId;
    public DungeonScenario Scenario;
    [SerializeField]
    private Cell cell;
    public GameObject Model;
    //public Vector3Int index;
    public CellDataData Data;
    public bool isBoss = false;

    private void OnValidate()
    {
        Data = GetComponent<CellDataData>();
        if (Scenario)
        {
            Data.CellData.ScenarioID = Scenario.ID;
        }
        if (cell == null)
        {
            cell = GetComponent<Cell>();
            Data.CellData.Index = cell.GetIndex();
        }
        if (cell != null)
        {
            Data.CellData.Index = cell.GetIndex();
        }
    }

    private void Awake()
    {
        //cell = gameObject.GetComponent<Cell>();
    }

    public void Run()
    {
        //Debug.Log("DungeonCell loaded");
        if (!Data.CellData.isCleared && Scenario)
        {
            Scenario.Activate(this);
        }
        //Debug.Log("DungeonCell isCleared = " + Data.CellData.isCleared.ToString());
    }

}
