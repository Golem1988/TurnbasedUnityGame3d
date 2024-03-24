using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Kryz.CharacterStats;

public class StatDisplay : MonoBehaviour
{
	private CharacterStat _stat;
	public CharacterStat Stat
	{
		get { return _stat; }
		set
		{
			_stat = value;
			UpdateStatValue();
		}
	}

	private CharacterStat _maxStat;
	public CharacterStat MaxStat
	{
		get { return _maxStat; }
		set
		{
			_maxStat = value;
			UpdateStatValue();
		}
	}

	private string _name;
	public string Name
	{
		get { return _name; }
		set
		{
			_name = value;
			nameText.text = _name;
		}
	}

	[SerializeField] TextMeshProUGUI nameText;
	[SerializeField] TextMeshProUGUI valueText;
	//[SerializeField] TextMeshProUGUI maxValueText;

	//private void OnValidate()
	//{
	//	TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
	//	nameText = texts[0];
	//	valueText = texts[1];
	//}

	public void UpdateStatValue()
	{
		valueText.text = _stat.Value.ToString();// + "/" + _maxStat.Value.ToString();

		//if (maxValueText != null)
  //      {
		//	maxValueText.text = _maxStat.Value.ToString();
		//}
		//if (showingTooltip)
		//{
		//	tooltip.ShowTooltip(Stat, Name);
		//}
	}
}
