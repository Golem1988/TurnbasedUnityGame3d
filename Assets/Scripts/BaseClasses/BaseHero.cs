using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BaseHero : BaseClass
{
    //[Header("Statpoints")]
    //public int unspentStatPoints;

    [Header("Secondary Attributes")]
    public float maxRage = 150f;
    public float curRage = 0f;

    //[Header("Statpoint growth")]
    //public int statpointsPerLevel = 5;
    //public int statIncreasePerLevel = 1;
    
}
