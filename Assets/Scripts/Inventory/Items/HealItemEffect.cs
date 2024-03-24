﻿using UnityEngine;

[CreateAssetMenu(menuName = "Item Effects/Heal")]
public class HealItemEffect : UsableItemEffect
{
	public int HealAmount;

	public override void ExecuteEffect(UsableItem usableItem, CharacterInformation character)
	{
		character.Stats.curHP.BaseValue += HealAmount;
	}

	public override string GetDescription()
	{
		return "Heals for " + HealAmount + " health.";
	}
}
