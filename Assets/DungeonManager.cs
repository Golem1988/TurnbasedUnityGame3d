using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;
    [SerializeField]
    private GameObject grid;
    [SerializeField]
    private List<CellData> cellDatas = new();
    [SerializeField]
    private List<CellData> cellDataFiltered = new();
    private string filePath;

    public List<CellData> CellDataFiltered { get => cellDataFiltered; protected set => cellDataFiltered = value; }

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/DungeonData.json";

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
#if UNITY_EDITOR
    private void OnValidate()
    {
        if(cellDatas.Count == 0)
            FillData(grid);
        cellDataFiltered = new(cellDatas);
        if (cellDataFiltered.Count > 0)
            FilterData();
    }

    void FilterData()
    {
        cellDataFiltered.RemoveAll(cell => cell.ScenarioID == "");
    }

    public void FillData(GameObject grid)
    {
        // Loop through each child of the parent transform
        foreach (Transform child in grid.transform)
        {
            // set layer for each child
            var data = child.gameObject.GetComponent<CellDataData>().CellData;
            cellDatas.Add(data);
            continue;
        }
    }
#endif
    void Start()
    {
        //cellDataFiltered = new(cellDatas);
        //if(cellDataFiltered.Count > 0)
        //    FilterData();

        //set the data for each cell in grid
        LoadDungeonData();
        //LoadDataToCells(grid);
    }

    public void UpdateData(CellData cell)
    {
        int index = cellDataFiltered.FindIndex(cellx => cellx.Index == cell.Index);
        cellDataFiltered.RemoveAt(index);
        cellDataFiltered.Add(cell);
        SaveDungeonData();
    }



    //public void LoadDataToCells(GameObject grid)
    //{
    //    // Loop through each child of the parent transform
    //    foreach (Transform child in grid.transform)
    //    {
    //        // set layer for each child
    //        var CellData = child.gameObject.GetComponent<CellDataData>().CellData;
    //        if(CellData == null)
    //            continue;
    //        var index = CellData.Index;
    //        Debug.Log(index.ToString());
    //        var inaaa = cellDataFiltered.FirstOrDefault(cell => cell.Index == index);
    //        if (inaaa != null)
    //        {
    //            Debug.Log(inaaa.ToString());
    //            CellData.isCleared = inaaa.isCleared;
    //            child.gameObject.GetComponent<DungeonCell>().Run();
    //            continue;
    //        }
    //        if (inaaa == null)
    //            continue;
    //    }
    //}

    [System.Serializable]
    public class DungeonDataListWrapper
    {
        public List<CellData> cellDataFiltered;
    }

    public void SaveDungeonData()
    {
        // Create a wrapper object and assign the list
        DungeonDataListWrapper wrapper = new DungeonDataListWrapper();
        wrapper.cellDataFiltered = cellDataFiltered;

        // Serialize the wrapper object into JSON
        string json = JsonUtility.ToJson(wrapper);

        WriteToFile(filePath, json);
        Debug.Log("Data written to JSON file: " + filePath);
        //GameManager.instance.Chat.AddToChatOutput("<#4ffc05>Hero data saved...</color>");

    }

    void WriteToFile(string path, string data)
    {
        File.WriteAllText(path, data);
    }

    public void LoadDungeonData()
    {
        if (File.Exists(filePath))
        {
            string json = ReadFromFile(filePath);

            // Deserialize the JSON data into a list of CapturedPets
            DungeonDataListWrapper wrapper = JsonUtility.FromJson<DungeonDataListWrapper>(json);

            // Access the list of CapturedPets from the wrapper
            List<CellData> loadedInfo = wrapper != null ? wrapper.cellDataFiltered : new List<CellData>();

            cellDataFiltered.Clear();

            foreach (CellData cell in loadedInfo)
            {
                cellDataFiltered.Add(cell);
            }
            Debug.Log("Data loaded from JSON file: " + filePath);
            //GameManager.instance.Chat.AddToChatOutput("<#4ffc05>Hero data loaded...</color>");
        }
        else
            Debug.Log("Dungeon data file doesn't exist!");
    }

    string ReadFromFile(string path)
    {
        if (File.Exists(path))
        {
            return File.ReadAllText(path);
        }
        else
        {
            Debug.LogWarning("File not found: " + path);
            return null;
        }
    }

    public void DeleteDungeonProgressFile()
    {
        // Check if the file exists before attempting to delete it
        if (File.Exists(filePath))
        {
            // Delete the file
            File.Delete(filePath);
            Debug.Log("File deleted: " + filePath);
        }
        else
        {
            Debug.LogWarning("File not found: " + filePath);
        }
    }
}
