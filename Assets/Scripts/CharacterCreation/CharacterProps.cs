using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Creation/New Character Prop Placeholder")]
public class CharacterProps : ScriptableObject
{
    public string characterName;
    public string characterID;
    public string ID;
    public GameObject schoolButton;
    [TextArea (5, 7)] public string characterDescription;
    [Range (1,5)]public List<int> strengths = new();
    public List<Sprite> skillIcons = new();
    public GameObject characterModel;
    public GameObject characterWeapon;
    public RuntimeAnimatorController weaponAnimator;
    public GameObject characterWings;
    public GameObject characterVFX1;
    public GameObject characterVFX3;
    public RuntimeAnimatorController characterAnimator;
    public Sprite characterAvatar;
}
