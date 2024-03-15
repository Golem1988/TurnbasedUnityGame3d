using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Skill Database")]
public class SkillDatabase : ScriptableObject
{
    public ActiveSkill[] ActiveSkills;
	public PassiveSkill[] PassiveSkills;

#if UNITY_EDITOR
	private void OnValidate()
	{
		LoadSkills(); 
	}

		private void OnEnable()
	{
		EditorApplication.projectChanged -= LoadSkills;
		EditorApplication.projectChanged += LoadSkills;
	}

	private void OnDisable()
	{
		EditorApplication.projectChanged -= LoadSkills;
	}

	private void LoadSkills()
	{
		ActiveSkills = FindAssetsByType<ActiveSkill>("Assets/Scripts/Attacks/SOskills");
	}
	public static T[] FindAssetsByType<T>(params string[] folders) where T : Object
	{
		string type = typeof(T).Name;

		string[] guids;
		if (folders == null || folders.Length == 0)
		{
			guids = AssetDatabase.FindAssets("t:" + type);
		}
		else
		{
			guids = AssetDatabase.FindAssets("t:" + type, folders);
		}

		T[] assets = new T[guids.Length];

		for (int i = 0; i < guids.Length; i++)
		{
			string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
			assets[i] = AssetDatabase.LoadAssetAtPath<T>(assetPath);
		}
		return assets;
	}
#endif
}
