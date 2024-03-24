using UnityEngine;
using UnityEngine.UI;
using Kryz.CharacterStats;

public class StatPanel : MonoBehaviour
{
    [SerializeField] StatDisplay[] statDisplays;
    [SerializeField] string[] statNames;
    public CharacterStat[] stats;
	//public CharacterInformation characterX;

    //private void OnValidate()
    //{
    //    statDisplays = GetComponentsInChildren<StatDisplay>();
    //    UpdateStatNames(); 
    //}

	public void SetStats(params CharacterStat[] charStats)
	{
		stats = charStats;

		if (stats.Length > statDisplays.Length)
		{
			Debug.LogError("Not Enough Stat Displays!");
			return;
		}

		for (int i = 0; i < statDisplays.Length; i++)
		{
			statDisplays[i].gameObject.SetActive(i < stats.Length);

			if (i < stats.Length)
			{
				statDisplays[i].Stat = stats[i];
			}
		}
	}

	public void UpdateStatValues()
	{
		for (int i = 0; i < stats.Length; i++)
		{
			statDisplays[i].UpdateStatValue();
		}
	}

	public void UpdateStatValuesX(CharacterInformation characterX)
	{
		characterX.Stats.strengthUpdated.BaseValue = stats[0].Value;
		characterX.Stats.agilityUpdated.BaseValue = stats[1].Value;
		characterX.Stats.intellectUpdated.BaseValue = stats[2].Value;
		characterX.Stats.dexterityUpdated.BaseValue = stats[3].Value;
		characterX.Stats.staminaUpdated.BaseValue = stats[4].Value;
	}

	public void UpdateStatNames()
	{
		for (int i = 0; i < statNames.Length; i++)
		{
			statNames[i] = statDisplays[i].Name;
		}
	}

}
