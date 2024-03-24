using UnityEngine;

[CreateAssetMenu(menuName = "Battle Scenario/New battle scenario")]
public class BattleSetupSO : ScriptableObject
{
    [SerializeField]
    protected EncounterTeam[] enemyUnits;
    [SerializeField]
    protected ItemDrops[] possibleLoot;
    [SerializeField]
    protected int experience;


    public EncounterTeam[] EnemyUnits { get => enemyUnits; protected set => enemyUnits = value; }
    public ItemDrops[] PossibleLoot { get => possibleLoot; protected set => possibleLoot = value; }
    public int Experience { get => experience; protected set => experience = value; }

}
