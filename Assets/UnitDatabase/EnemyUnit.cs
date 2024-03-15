using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit/EnemyData")]
public class EnemyUnit : UnitDatabaseSO
{
    public List<ActiveSkillSet> ActiveSkills;
    public List<PassiveSkillSet> PassiveSkills;
    public bool canBeBaby;
    public bool canBeMutant;
}
