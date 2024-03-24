using System;
using System.Collections.Generic;
using UnityEngine;
using MapTileGridCreator.Core;

[CreateAssetMenu(menuName = "Dungeon/New scenario")]
public class DungeonScenario : ScriptableObject
{
    [SerializeField]
    protected string id;

    public string ID { get => id; protected set => id = value; }


    //private void OnValidate()
    //{
    //    if (ID == null || ID == "")
    //    {
    //        ID = "scenario" + Guid.NewGuid().ToString();
    //    }
    //}

    public virtual void Activate(DungeonCell cell)
    {
        //
    }

    public virtual void Spawn(GameObject obj, Transform spot)
    {
        //
    }

    public virtual void StepOn()
    {
        //fire the events from here.
    }
}
