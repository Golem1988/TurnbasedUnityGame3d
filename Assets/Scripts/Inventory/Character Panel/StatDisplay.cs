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
	[SerializeField] TextMeshProUGUI maxValueText;

	//private void OnValidate()
	//{
	//	TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
	//	nameText = texts[0];
	//	valueText = texts[1];
	//}

	public void UpdateStatValue()
	{
		if (maxValueText != null)
		{
			maxValueText.text = _stat.Value.ToString();// + "/" + _maxStat.Value.ToString();
		}
		
		valueText.text = _stat.CurValue.ToString();
		//if (maxValueText != null)
		//      {
		//	maxValueText.text = _stat.MaxValue.ToString();
		//}
		//if (showingTooltip)
		//{
		//	tooltip.ShowTooltip(Stat, Name);
		//}
	}

	public void UpdateStatCurValue()
	{
		//if (valueText != null)
		//{
		//	valueText.text = _stat.CurValue.ToString();
		//}
	}
}
