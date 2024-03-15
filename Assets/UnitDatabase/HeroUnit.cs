using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit/HeroData")]
public class HeroUnit : UnitDatabaseSO
{
    public HeroSchool HeroSchool;
    public List<GameObject> HeroWeapons;

    //private void OnValidate()
    //{
    //    HeroWeapons = new();
    //    LoadWeapons();
    //}

    //void LoadWeapons()
    //{
    //    string nameZero = ModelId.ToString();
        
    //    for (int i = 1; i < 10; i++)
    //    {
    //        string name = nameZero + "_" + i.ToString();
    //        string name2 = "weapon" + name;
    //        var WeaponPrefab = Resources.Load<GameObject>("GameRes/Model/Weapon/" + name + "/Prefabs/" + name2);
    //        if (WeaponPrefab == null)
    //        {
    //            Debug.Log("tried to find weapon with ID " + name + ". but it does not exist");
    //            return;
    //        }
    //        HeroWeapons.Add(WeaponPrefab);
    //    }
        


    //    //weapon1320_7

    //}
}
