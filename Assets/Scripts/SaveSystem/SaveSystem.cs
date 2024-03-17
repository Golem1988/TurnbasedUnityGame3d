//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;
//using UnityEngine;

//public static class SaveSystem
//{
//    public static void SaveCharacter (Character character)
//    {
//        BinaryFormatter formatter = new BinaryFormatter();
//        string path = Application.persistentDataPath + "/" + character.unit.Stats.theName + ".dat";
//        FileStream stream = new FileStream(path, FileMode.Create);

//        CharacterData data = new CharacterData(character);

//        formatter.Serialize(stream, data);
//        stream.Close();
//        Debug.Log("Character data saved");
//    }

//    public static CharacterData LoadCharacter (Character character)
//    {
//        string path = Application.persistentDataPath + "/" + character.unit.Stats.theName + ".dat";
//        if (File.Exists(path))
//        {
//            BinaryFormatter formatter = new BinaryFormatter();
//            FileStream stream = new FileStream(path, FileMode.Open);

//            CharacterData data = formatter.Deserialize(stream) as CharacterData;
//            stream.Close();
//            return data;
//        }
//        else
//        {
//            Debug.LogError("Save file not found in " + path);
//            return null;
//        }
//    }

//}
