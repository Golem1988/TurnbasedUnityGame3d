using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttributes : MonoBehaviour
{
    public BaseClass Stats;

    private void OnValidate()
    {
        Stats.theName = gameObject.name;
        Stats.displayName = Stats.theName;
    }
    private void Start()
    {
        //Stats.theName = gameObject.name;
        //Stats.displayName = Stats.theName;
    }
}
