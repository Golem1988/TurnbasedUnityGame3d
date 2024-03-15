using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UnitDatabaseSO : ScriptableObject
{
    //public int ID;
    [SerializeField] string id;
    public string ID { get { return id; } }
    public int ModelId;
    public string DisplayName;
    public GameObject ModelPrefab;
    public Sprite UnitAvatar;

    //private void OnValidate()
    //{

    //}
#if UNITY_EDITOR
	private void OnValidate()
	{
		string path = AssetDatabase.GetAssetPath(this);
		id = "unit_" + AssetDatabase.AssetPathToGUID(path);
        //if (ModelId != 0)
            //LoadDetails();
	}
#endif

    public virtual void LoadDetails()
    {
        string name = ModelId.ToString();
        string name2 = "model" + ModelId.ToString();
        ModelPrefab = Resources.Load<GameObject>("GameRes/Model/Character/" + name + "/Prefabs/" + name2);
        if (ModelPrefab == null)
        {
            Debug.Log("tried to instantate summon with ID " + name + ". but it does not exist");
            return;
        }

        if (ModelPrefab)
        {
            UnitAvatar = Resources.Load<Sprite>("GameRes/Avatars/" + name);
            if (UnitAvatar == null)
            {
                Debug.Log("tried to find sprite with ID " + name + ". but it does not exist");
                return;
            }
        }
    }
}
