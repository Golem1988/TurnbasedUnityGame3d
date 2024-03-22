using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/New Database")]
public class ScenarioDatabaseSO : ScriptableObject
{
	public DungeonScenario[] Scenarios;

#if UNITY_EDITOR
	private void OnValidate()
	{
		LoadScenarios(); 
	}

	private void OnEnable()
	{
		EditorApplication.projectChanged -= LoadScenarios;
		EditorApplication.projectChanged += LoadScenarios;
	}

	private void OnDisable()
	{
		EditorApplication.projectChanged -= LoadScenarios;
	}

	private void LoadScenarios()
	{
		Scenarios = FindAssetsByType<DungeonScenario>("Assets/Scripts/Dungeons/Scenarios");
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

