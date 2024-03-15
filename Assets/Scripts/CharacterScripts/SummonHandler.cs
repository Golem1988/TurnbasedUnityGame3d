using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SummonHandler : MonoBehaviour
{
    public List<CapturedPets> SummonList = new();
    //public GameObject ActivatedSummon;
    public GameObject SummonPanel;
    public int MaxSummonSlots;
    //private List<GameObject> PanelList = new();

    private void Start()
    {
        //PanelList = SummonPanel.GetComponent<SummonInfoInterface>().SummonList;
        //SummonPanel.GetComponent<SummonInfoInterface>().SummonList = SummonList;
        //SummonPanel.GetComponent<SummonInfoInterface>().MaxSummonSlots = MaxSummonSlots;
    }

    [System.Serializable]
    public class CapturedPetsListWrapper
    {
        public List<CapturedPets> petsList;
    }

    public void SavePetData(string charName)
    {
        SummonList = GetComponent<SummonHandler>().SummonList;
        // Path to the JSON file
        string filePath = Application.persistentDataPath + "/" + charName + "_PetData.json";
        // Create a wrapper object and assign the list
        CapturedPetsListWrapper wrapper = new CapturedPetsListWrapper();
        wrapper.petsList = SummonList;

        // Serialize the wrapper object into JSON
        string json = JsonUtility.ToJson(wrapper);

        // Write the JSON data to a file
        WriteToFile(filePath, json);

        Debug.Log("Data written to JSON file: " + filePath);
    }

    // Method to write data to a file
    void WriteToFile(string path, string data)
    {
        // Write the data to the file
        File.WriteAllText(path, data);
    }

    public void LoadPetData(string charName)
    {
        // Path to the JSON file
        string filePath = Application.persistentDataPath + "/" + charName + "_PetData.json";
        // Read the JSON data from the file
        string json = ReadFromFile(filePath);

        // Deserialize the JSON data into a list of CapturedPets
        CapturedPetsListWrapper wrapper = JsonUtility.FromJson<CapturedPetsListWrapper>(json);

        // Access the list of CapturedPets from the wrapper
        List<CapturedPets> loadedPets = wrapper != null ? wrapper.petsList : new List<CapturedPets>();

        //clear the data in list
        SummonList.Clear();

        // Do something with the loaded data
        foreach (CapturedPets pet in loadedPets)
        {
            SummonList.Add(pet);
        }
        Debug.Log("Data loaded from JSON file: " + filePath);

        //CopyData(charName, loadedPets);

    }

    public void CopyData(string charName, List<CapturedPets> loadedPets)
    {
        var data = HeroDataManager.instance.CharacterInfo;
        CharacterInformation info = new CharacterInformation();
        info.SummonList = loadedPets;
        info.Name = charName;
        info.BaseID = FindID(charName);
        info.isUnlocked = true;
        info.isActive = true;
        info.Level = gameObject.GetComponent<UnitLevel>().level;
        info.Stats = gameObject.GetComponent<UnitAttributes>().Stats;
        var MagicAttacks = gameObject.GetComponent<Abilities>().MagicAttacks;
        foreach (ActiveSkill skill in MagicAttacks)
        {
            info.MagicAttacks.Add(skill.ID);
        }
        //var PassiveSkillss = gameObject.GetComponent<Abilities>().PassiveSkills;
        //foreach (PassiveSkill skill in PassiveSkillss)
        //{
        //    info.PassiveSkills.Add(skill.ID);
        //}
        info.MaxSummonSlots = gameObject.GetComponent<SummonHandler>().MaxSummonSlots;
        data.Add(info);
    }

    public static string FindID(string unitName)
    {
        var summonDB = HeroDataManager.instance.UnitDatabase.HeroList;
        foreach (HeroUnit unit in summonDB)
        {
            // Check if the current unitID matches the desired ID
            if (unit.name == unitName)
            {
                // Return the GameObject if a match is found
                return unit.ID;
            }
        }
        // Return null if no unit with the specified id is found
        return null;
    }

    // Method to read data from a file
    string ReadFromFile(string path)
    {
        // Read the data from the file
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
}
