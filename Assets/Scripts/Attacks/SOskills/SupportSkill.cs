using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Support Skill")]

public class SupportSkills : ActiveSkill
{
    [SerializeField]
    protected bool HealHP;
    [SerializeField]
    protected bool HealMP; 
    [SerializeField]
    protected bool Ressurect;
}
