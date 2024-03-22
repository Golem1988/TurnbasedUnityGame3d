using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapTileGridCreator.Core;

[System.Serializable]
public class DungeonCell : MonoBehaviour
{
    public bool isCleared;
    public string ScenarioId;
    public DungeonScenario Scenario;
    private Cell cell;
    public GameObject Model;


    private void OnValidate()
    {
        if (Scenario)
        {
            ScenarioId = Scenario.ID;
        }
    }

    private void Awake()
    {
        cell = gameObject.GetComponent<Cell>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!isCleared && Scenario)
        {

            Scenario.Activate(cell);
        }
    }

}
