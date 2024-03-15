using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    [Header("Abilities")]
    public List<ActiveSkill> BasicActions = new();
    public List<BaseAction> BasicActs = new();
    public List<ActiveSkill> MagicAttacks = new();
    public List<PassiveSkill> PassiveSkills = new();
}
