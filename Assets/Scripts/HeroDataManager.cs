using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class HeroDataManager : MonoBehaviour
{
    public static HeroDataManager instance;
    public List<CharacterInformation> CharacterInfo = new List<CharacterInformation>();
    //public string MainCharacter;
    public UnitDatabase UnitDatabase;
    public string saveID;
    private string filePath;
    public TextAsset BlankData;
    private Scene currentScene;
    [Space(5f)]
    public bool newGame = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        filePath = Application.persistentDataPath + "/CharData.json";
    }

    private void OnEnable()
    {
        Debug.Log("HeroData is online");
        //currentScene = SceneManager.GetActiveScene();
        SceneManager.sceneLoaded += OnSceneLoaded;
        //MainCharacter = CharacterInfo[0].Name;
        filePath = Application.persistentDataPath + "/CharData.json";

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SaveCharData();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = SceneManager.GetActiveScene();
    }

    private void Start()
    {
        filePath = Application.persistentDataPath + "/CharData.json";
        if (currentScene.name != "CharacterCreation" && currentScene.name != "MainScreen")
        {
            LoadCharData();
        }
    }

    //private void OnDestroy()
    //{
    //    SaveCharData();
    //}

    //private void OnValidate()
    //{
    //    filePath = Application.persistentDataPath + "/CharData.json";
    //    //    LoadCharData();
    //    // or
    //    SaveCharData();
    //}

    private void Update()
    {
        //load manually
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadCharData();
        }

        //save manually
        if (Input.GetKeyDown(KeyCode.T))
        {
            SaveCharData();
        }
    }

    [System.Serializable]
    public class CharacterInfoListWrapper
    {
        public List<CharacterInformation> charList;
    }

    public void SaveCharData()
    {
        filePath = Application.persistentDataPath + "/CharData.json";

        if (currentScene.name != "CharacterCreation" && currentScene.name != "MainScreen")
        {
            // Create a wrapper object and assign the list
            CharacterInfoListWrapper wrapper = new CharacterInfoListWrapper();
            wrapper.charList = CharacterInfo;

            // Serialize the wrapper object into JSON
            string json = JsonUtility.ToJson(wrapper);

            WriteToFile(filePath, json);
            Debug.Log("Data written to JSON file: " + filePath);
            //GameManager.instance.Chat.AddToChatOutput("<#4ffc05>Hero data saved...</color>");
        }
    }

    void WriteToFile(string path, string data)
    {
        File.WriteAllText(path, data);
    }

    public void LoadCharData()
    {
        string json = ReadFromFile(filePath);

        // Deserialize the JSON data into a list of CapturedPets
        CharacterInfoListWrapper wrapper = JsonUtility.FromJson<CharacterInfoListWrapper>(json);

        // Access the list of CapturedPets from the wrapper
        List<CharacterInformation> loadedInfo = wrapper != null ? wrapper.charList : new List<CharacterInformation>();

        CharacterInfo.Clear();

        foreach (CharacterInformation character in loadedInfo)
        {
            CharacterInfo.Add(character);
        }
        Debug.Log("Data loaded from JSON file: " + filePath);
        //GameManager.instance.Chat.AddToChatOutput("<#4ffc05>Hero data loaded...</color>");

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

    //void FillX()
    //{
    //    for (int i = 5; i < 6; i++)
    //    {
    //        CharacterInformation NEW = new CharacterInformation();
    //        NEW.Level = CharacterInfo[1].Level;
    //        NEW.Stats = CharacterInfo[1].Stats;
    //        NEW.Stats.displayName = UnitDatabase.HeroList[i].DisplayName;
    //        NEW.Stats.theName = NEW.Stats.displayName;
    //        NEW.Name = NEW.Stats.displayName;
    //        NEW.BaseID = UnitDatabase.HeroList[i].ID;
    //        NEW.isUnlocked = true;
    //        NEW.active = true;
    //        NEW.PassiveSkills = CharacterInfo[0].PassiveSkills;
    //        NEW.MagicAttacks = CharacterInfo[0].MagicAttacks;
    //        NEW.MaxSummonSlots = 20;
    //        //NEW.HeroUnit = UnitDatabase.HeroList[i];
    //        CharacterInfo.Add(NEW);
    //    }
    //}

    //void GoBlank()
    //{
    //    for (int i = 1; i < CharacterInfo.Count; i++)
    //    {
    //        //CharacterInformation NEW = new CharacterInformation();
    //        CharacterInfo[i].Level = CharacterInfo[0].Level;
    //        CharacterInfo[i].Stats = CharacterInfo[0].Stats;
    //        CharacterInfo[i].Stats.theName = UnitDatabase.HeroList[i].DisplayName;
    //        CharacterInfo[i].Stats.displayName = UnitDatabase.HeroList[i].DisplayName;
    //        CharacterInfo[i].SummonList.Clear();
    //    }
    //}

    //write new save file to persistent path directory

    public void CreateSaveFile()
    {
        string jsonString = BlankData.text;
        WriteToFile(filePath, jsonString);
        Debug.Log("New save file created!");
    }
}
