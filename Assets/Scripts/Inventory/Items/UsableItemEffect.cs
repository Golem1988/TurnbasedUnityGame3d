using UnityEngine;

public abstract class UsableItemEffect : ScriptableObject
{
	public abstract void ExecuteEffect(UsableItem parentItem, CharacterInformation character);

	public abstract string GetDescription();
}
