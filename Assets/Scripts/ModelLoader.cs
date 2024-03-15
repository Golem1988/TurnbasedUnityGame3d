using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelLoader : MonoBehaviour
{
    public GameObject Model;
    public Transform ModelHolder;
    public Transform ObjectParent;

    //private void OnValidate()
    //{
    //    if (gameObject.CompareTag("Hero") || gameObject.CompareTag("Summon"))
    //    {
    //        ModelHolder = transform.Find("ModelHolder");
    //        if (transform.Find("ModelHolder").transform.GetChild(0).gameObject != null)
    //        {
    //            Model = transform.Find("ModelHolder").transform.GetChild(0).gameObject;
    //        }

    //        else
    //            Debug.Log("Unit model is not assigned under Template / ModelHolder!");
    //    }
    //}

    //void Awake()
    //{
    //    if (gameObject.CompareTag("Hero"))
    //    {
    //        var op = Instantiate(Model, ObjectParent.position + new Vector3(0f, 0f, -0.81f), Quaternion.Euler(0f, 180f, 0f), ObjectParent);
    //        //custom extension to set all the GameObject and it's children layers to desired layer id 
    //        Extensions.SetLayer(op, 5);
    //    }
    //}

}
