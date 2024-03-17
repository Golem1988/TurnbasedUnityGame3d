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
    public float baseHP;
    public float curHP;

    public float baseMP;
    public float curMP;

    public float baseATK;
    public float curATK;

    public float baseMATK;
    public float curMATK;

    public float maxATK;
    public float minATK;

    public float baseDEF;
    public float curDEF;

    public float baseCRIT = 0.25f;
    public float curCRIT = 0.25f;

    public float critDamage = 1.5f;

    public float baseSpeed;
    public float curSpeed;

    public float curDodge;
    public float baseDodge;

    public float baseHit;
    public float curHit;

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
