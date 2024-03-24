using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    //public BaseEnemy enemy;
    public UnitAttributes unit;
    public UnitLevel UnitLevel;
    private int enemyLevel;
    public EnemyType enemyType;
    public EnemyRarity enemyRarity;
    public float expAmount;
    public string BaseID;
    //public Abilities abilities;

    //public List<SkillSet> PossibleSkills = new();
    //public List<PassiveSkillSet> PossiblePassives = new();

    private void OnValidate()
    {
        unit = GetComponent<UnitStateMachine>().unit;
        UnitLevel = GetComponent<UnitLevel>();
    }

    private void Awake()
    {
        //GetComponent<UnitUI>().LoadUI();
        //SetEnemyLevel();
    }

    private void Start()
    {
        //if (UnitLevel.level.currentlevel == 0)
        //    SetEnemyLevel();
        //UnitLevel = GetComponent<UnitLevel>();
    }

    void SetEnemyLevel()
    {
        GameManager.instance.Chat.AddToChatOutput("<#de0404>You can't decrease your stat any lower!</color>");
        int curRegions = GameManager.instance.curRegions;
        enemyLevel = GameManager.instance.Regions[curRegions].EnemyLevel;
        UnitLevel.level = new Level(enemyLevel);
        SetParams();
        Debug.Log("SetEnemyLevel triggered");
    }

    public void SetEnemyLevelX(int newEnemyLevel)
    {
        enemyLevel = newEnemyLevel;
        UnitLevel.level = new Level(newEnemyLevel);
        SetParams();
        gameObject.GetComponent<UnitUI>().levelText.GetComponent<TextMeshProUGUI>().text = enemyLevel.ToString();
        //Debug.Log("SetEnemyLevelX triggered");
    }
    private void SetParams()
    {
        //level 1 stats = 5x each. Then for each level we get 5 statpoints + 1 for each stat
        //for randomness
        //Debug.Log("SetParams triggered");
        //var UnitDatabase = HeroData.instance.UnitDatabase;
        var levelBasedStat = 5 + (1 * enemyLevel);

        unit.Stats.strength.BaseValue = Random.Range(levelBasedStat, levelBasedStat + enemyLevel + Random.Range(0, 3));
        unit.Stats.intellect.BaseValue = Random.Range(levelBasedStat, levelBasedStat + enemyLevel + Random.Range(0, 3));
        unit.Stats.dexterity.BaseValue = Random.Range(levelBasedStat, levelBasedStat + enemyLevel + Random.Range(0, 3));
        unit.Stats.agility.BaseValue = Random.Range(levelBasedStat, levelBasedStat + enemyLevel + Random.Range(0, 3));
        unit.Stats.stamina.BaseValue = Random.Range(levelBasedStat, levelBasedStat + enemyLevel + Random.Range(0, 3));

        //Calculate HP based on Stats
        unit.Stats.baseHP.BaseValue = Mathf.Round(unit.Stats.strength.BaseValue * HeroDataManager.instance.UnitDatabase.hpPerStr) + (unit.Stats.stamina.BaseValue * HeroDataManager.instance.UnitDatabase.hpPerSta);
        unit.Stats.curHP.BaseValue = unit.Stats.baseHP.BaseValue;

        //Calculate MP based on stats
        unit.Stats.baseMP.BaseValue = Mathf.Round(unit.Stats.intellect.BaseValue * HeroDataManager.instance.UnitDatabase.mpPerInt);
        unit.Stats.curMP.BaseValue = unit.Stats.baseMP.BaseValue;

        //Calculate Attack based on stats
        unit.Stats.baseATK.BaseValue = Mathf.Round((unit.Stats.strength.BaseValue * HeroDataManager.instance.UnitDatabase.atkPerStr) + (unit.Stats.intellect.BaseValue * HeroDataManager.instance.UnitDatabase.atkPerInt));
        unit.Stats.curATK.BaseValue = unit.Stats.baseATK.BaseValue;

        //Calculate magic Attack based on stats
        //unit.Stats.baseMATK = Mathf.Round((unit.Stats.strength.BaseValue * HeroDataManager.instance.UnitDatabase.atkPerStr) + (unit.Stats.intellect.BaseValue * HeroDataManager.instance.UnitDatabase.atkPerInt));
        //unit.Stats.curMATK = unit.Stats.baseMATK;

        unit.Stats.maxATK = unit.Stats.baseATK.BaseValue + Random.Range(10, 50);
        unit.Stats.minATK = unit.Stats.baseATK.BaseValue;

        //Calculate HIT based on stats
        unit.Stats.baseHit.BaseValue = Mathf.Round(unit.Stats.dexterity.BaseValue * HeroDataManager.instance.UnitDatabase.hitPerDex);
        unit.Stats.curHit.BaseValue = unit.Stats.baseHit.BaseValue;

        //Calculate dodge based on stats
        unit.Stats.baseDodge.BaseValue = Mathf.Round(unit.Stats.agility.BaseValue * HeroDataManager.instance.UnitDatabase.dodgePerAgi);
        unit.Stats.curDodge.BaseValue = unit.Stats.baseDodge.BaseValue;

        //calculate def based on stats
        unit.Stats.baseDEF.BaseValue = Mathf.Round(unit.Stats.stamina.BaseValue * HeroDataManager.instance.UnitDatabase.defPerSta);
        unit.Stats.curDEF.BaseValue = unit.Stats.baseDEF.BaseValue;

        //calculate critrate based on stats
        unit.Stats.baseCRIT = 25f;
        unit.Stats.curCRIT = unit.Stats.baseCRIT;

        //calculate speed based on stats
        unit.Stats.baseSpeed.BaseValue = Mathf.Round(unit.Stats.agility.BaseValue * HeroDataManager.instance.UnitDatabase.spdPerAgi);
        unit.Stats.curSpeed.BaseValue = unit.Stats.baseSpeed.BaseValue;

        expAmount = unit.Stats.strength.BaseValue + unit.Stats.intellect.BaseValue + unit.Stats.dexterity.BaseValue + unit.Stats.agility.BaseValue + unit.Stats.stamina.BaseValue;
    }



}
