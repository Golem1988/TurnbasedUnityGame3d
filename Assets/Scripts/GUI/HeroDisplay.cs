using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDisplay : MonoBehaviour
{
    public GameObject ObjectParent;


    void Awake()
    {
        ObjectParent = GameObject.FindWithTag("Hero");
    }

}
