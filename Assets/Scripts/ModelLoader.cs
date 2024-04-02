using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelLoader : MonoBehaviour
{
    public GameObject Model;
    public Transform ModelHolder;
    public Transform ObjectParent;

    public GameObject WeaponModel;
    public GameObject WingsModel;

    private Transform weaponSpot;
    private Transform wingSpot;

    //private void Awake()
    //{
    //    if (gameObject.CompareTag("Hero"))
    //    {
    //        weaponSpot = Model.GetComponent<GameObjectContainer>().objectArray[3].gameObject.transform;
    //        wingSpot = Model.GetComponent<GameObjectContainer>().objectArray[5].gameObject.transform;
    //    }
    //}

    public void ShowWeapon(string heroID, int weaponIndex)
    {
        var weaponList = Extensions.FindHeroWeapons(heroID);
        WeaponModel = Instantiate(weaponList[weaponIndex], weaponSpot.position, Quaternion.identity, weaponSpot);
    }

    public void ShowWings(GameObject wingModel)
    {
        WingsModel = Instantiate(wingModel, wingSpot.position, Quaternion.identity, wingSpot);
    }
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
