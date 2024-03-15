using UnityEngine;
using UnityEngine.UI;
using Kryz.CharacterStats;

public class StatPanel : MonoBehaviour
{
    [SerializeField] StatDisplay[] statDisplays;
    [SerializeField] string[] statNames;
    public CharacterStat[] stats;
	[SerializeField] GameObject characterX;

    private void OnValidate()
    {
        statDisplays = GetComponentsInChildren<StatDisplay>();
        UpdateStatNames();
    }

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

			//if (i < stats.Length)
			//{
			//	statDisplays[i].Stat = stats[i];
			//}
		}
	}

	public void UpdateStatValues()
	{
		for (int i = 0; i < stats.Length; i++)
		{
			statDisplays[i].ValueText.text = stats[i].Value.ToString();
			// = stats[i].Value;
		}
	}

	public void UpdateStatValuesX()
	{
			//characterX.GetComponent<Character>().strengthUpdated.BaseValue = stats[0].Value;
			//characterX.GetComponent<Character>().agilityUpdated.BaseValue = stats[1].Value;
			//characterX.GetComponent<Character>().intellectUpdated.BaseValue = stats[2].Value;
			//characterX.GetComponent<Character>().dexterityUpdated.BaseValue = stats[3].Value;
			//characterX.GetComponent<Character>().staminaUpdated.BaseValue = stats[4].Value;
	}

	public void UpdateStatNames()
	{
		for (int i = 0; i < statNames.Length; i++)
		{
			statDisplays[i].NameText.text = statNames[i];
		}
	}

}
