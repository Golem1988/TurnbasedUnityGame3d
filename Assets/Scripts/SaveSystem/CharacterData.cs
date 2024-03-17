//using Kryz.CharacterStats;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[System.Serializable]
//public class CharacterData
//{
//    public string characterName;
//    public int level;
//    public int currentExp;
//    public int requiredExp;
//    public int statPoints;
//    public int strength;
//    public int intellect;
//    public int dexterity;
//    public int agility;
//    public int stamina;

//    public float[] position;


//    public CharacterData (Character character)
//    {
//        characterName = character.unit.Stats.theName;
//        level = character.UnitLevel.Level.currentlevel;
//        currentExp = character.UnitLevel.level.experience;
//        requiredExp = character.UnitLevel.level.requiredExp;
//        statPoints = character.unit.Stats.unspentStatPoints;

//        strength = character.unit.Stats.strength.BaseValue;
//        intellect = character.unit.Stats.intellect.BaseValue;
//        dexterity = character.unit.Stats.dexterity.BaseValue;
//        agility = character.unit.Stats.agility.BaseValue;
//        stamina = character.unit.Stats.stamina.BaseValue;

//        position = new float[3];
//        position[0] = character.transform.position.x;
//        position[1] = character.transform.position.y;
//        position[2] = character.transform.position.z;
//    }

//}
