using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Kryz.CharacterStats;

[System.Serializable]
public class BaseClass
{
    [Header("Main Info")]
    public string theName;
    public string displayName;

    [Header("Main Attributes")]
    public CharacterStat HP;
    //public CharacterStat HP.CurValue;
    //public CharacterStat maxedHP;
    public CharacterStat MP;
    //public CharacterStat MP.CurValue;
    //public CharacterStat maxedMP;
    public CharacterStat ATK;
    //public CharacterStat curATK;
    //public CharacterStat maxedATK;
    public CharacterStat MATK;
    //public CharacterStat curMATK;
    //public CharacterStat maxedMATK;

    //public float maxATK;
    //public float minATK;

    public CharacterStat DEF;
    //public CharacterStat curDEF;
    //public CharacterStat maxedDEF;

    public CharacterStat Speed;
    //public CharacterStat curSpeed;
    //public CharacterStat maxedSpeed;
    //public CharacterStat curDodge;
    public CharacterStat Dodge;
    //public CharacterStat maxedDodge;

    public CharacterStat Hit;
    //public CharacterStat curHit;
    //public CharacterStat maxedHit;

    [Header("CritThings")]
    public float baseCRIT = 25f;
    public float curCRIT = 25f;
    public float critDamage = 1.5f;

    [Header("Stats")]
    public CharacterStat strength;
    public CharacterStat intellect;
    public CharacterStat dexterity;
    public CharacterStat agility;
    public CharacterStat stamina;

    //[Header("Statpoint growth")]
    //public int statpointsPerLevel = 5;
    //public int statIncreasePerLevel = 1;

    //[Header("Attribute multipliers")]
    //public float hpPerStr = 10;
    //public float atkPerStr = 10;
    //public float mpPerInt = 10;
    //public float atkPerInt = 3;
    //public float spdPerAgi = 2;
    //public float dodgePerAgi = 3;
    //public float hitPerDex = 2;
    //public float atkPerDex = 2;
    //public float hpPerSta = 25;
    //public float defPerSta = 5;
    //public float matkPerInt = 10;
    //public float matkPerStr = 3;

    [Header("Statpoints")]
    public int unspentStatPoints;

    [Header("Secondary Attributes")]
    public float maxRage = 150f;
    public float curRage = 0f;
}
