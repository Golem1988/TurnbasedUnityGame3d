using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class BaseAttack : ScriptableObject
{
    [SerializeField] string id;
    public string ID { get { return id; } }
    public string SkillName;
    [SerializeField]
    [Range(1, 3)] public int SkillLevel;
    public Sprite SkillIcon;
    [SerializeField]
    [TextArea(5, 7)] public string Description;


#if UNITY_EDITOR
	protected virtual void OnValidate()
	{
		string path = AssetDatabase.GetAssetPath(this);
		id = AssetDatabase.AssetPathToGUID(path);
	}
#endif

}
