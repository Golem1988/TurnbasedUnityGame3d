using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class CellDataData : MonoBehaviour
{
    public CellData CellData;

    private void Start()
    {
        if(CellData.ScenarioID != "")
        {
            var dataList = DungeonManager.instance.CellDataFiltered;
            var index = CellData.Index;
            var data = dataList.FirstOrDefault(cell => cell.Index == index);
            CellData.isCleared = data.isCleared;
        }
        GetComponent<DungeonCell>().Run();
    }
}
