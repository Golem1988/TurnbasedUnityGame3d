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
		//UpdateStatMaxValues();
	}

	public void UpdateStatMaxValues()
	{
		for (int i = 0; i < stats.Length; i++)
		{
			statDisplays[i].UpdateStatCurValue();
		}
	}

	public void UpdateStatValuesX(CharacterInformation characterX)
	{
		characterX.Stats.HP = stats[0];
		characterX.Stats.MP = stats[1];
		characterX.Stats.strength = stats[2];
		characterX.Stats.intellect = stats[3];
		characterX.Stats.dexterity = stats[4];
		characterX.Stats.agility = stats[5];
        characterX.Stats.stamina = stats[6];
		characterX.Stats.ATK = stats[7];
		characterX.Stats.MATK = stats[8];
		characterX.Stats.DEF = stats[9];
		characterX.Stats.Dodge = stats[10];
		characterX.Stats.Hit = stats[11];
		characterX.Stats.Speed = stats[12];
	}

    public void UpdateStatNames()
	{
		for (int i = 0; i < statNames.Length; i++)
		{
			statNames[i] = statDisplays[i].Name;
		}
	}

}
