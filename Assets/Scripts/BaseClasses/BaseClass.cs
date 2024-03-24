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
    public CharacterStat baseHP;
    public CharacterStat curHP;
    public CharacterStat maxedHP;
    public CharacterStat baseMP;
    public CharacterStat curMP;
    public CharacterStat maxedMP;
    public CharacterStat baseATK;
    public CharacterStat curATK;
    public CharacterStat maxedATK;
    public CharacterStat baseMATK;
    public CharacterStat curMATK;
    public CharacterStat maxedMATK;

    public float maxATK;
    public float minATK;

    public CharacterStat baseDEF;
    public CharacterStat curDEF;
    public CharacterStat maxedDEF;

    public float baseCRIT = 25f;
    public float curCRIT = 25f;

    public float critDamage = 1.5f;

    public CharacterStat baseSpeed;
    public CharacterStat curSpeed;
    public CharacterStat maxedSpeed;
    public CharacterStat curDodge;
    public CharacterStat baseDodge;
    public CharacterStat maxedDodge;
    public CharacterStat baseHit;
    public CharacterStat curHit;
    public CharacterStat maxedHit;

    [Header("Stats")]
    public CharacterStat strength;
    public CharacterStat intellect;
    public CharacterStat dexterity;
    public CharacterStat agility;
    public CharacterStat stamina;

    public CharacterStat strengthUpdated;
    public CharacterStat intellectUpdated;
    public CharacterStat dexterityUpdated;
    public CharacterStat agilityUpdated;
    public CharacterStat staminaUpdated;

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
