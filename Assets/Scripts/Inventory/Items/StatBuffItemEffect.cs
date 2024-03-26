﻿using System.Collections;
using UnityEngine;
using Kryz.CharacterStats;

[CreateAssetMenu(menuName = "Item Effects/Stat Buff")]
public class StatBuffItemEffect : UsableItemEffect
{
	public int AgilityBuff;
	public float Duration;

	public override void ExecuteEffect(UsableItem parentItem, CharacterInformation character)
	{
		StatModifier statModifier = new StatModifier(AgilityBuff, StatModType.Flat, parentItem, StatSetType.MaxValue);
		character.Stats.agility.AddModifier(statModifier);
		//character.UpdateStatValues();
		//character.StartCoroutine(RemoveBuff(character, statModifier, Duration));
	}

	public override string GetDescription()
	{
		return "Grants " + AgilityBuff + " Agility for " + Duration + " seconds.";
	}

	private static IEnumerator RemoveBuff(CharacterInformation character, StatModifier statModifier, float duration)
	{
		yield return new WaitForSeconds(duration);
		character.Stats.agility.RemoveModifier(statModifier);
		//character.UpdateStatValues();
	}
}
