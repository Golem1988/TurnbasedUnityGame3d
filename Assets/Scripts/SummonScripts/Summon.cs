using System.Linq;
using UnityEngine;

[System.Serializable]
public class Summon : MonoBehaviour
{
    private GameManager gameManager;
    public SummonInfoInterface statPanel;
    public UnitAttributes unit;
    public UnitLevel UnitLevel;
    public int curLvl;

    public string OwnerName;
    public string UniqueID;
    public string BaseID;

    [Header("Summon Only Things")]
    public EnemyType summonType;
    public EnemyRarity summonRarity;
    private string typeColorCode;

    public int SummonLoyalty; //implement loyalty mechanics later

    private void Awake()
    {
        gameManager = GameManager.instance;
    }

    void Start()
    {
        statPanel = gameManager.summonInfoInterface;
        int ownerid = HeroDataManager.instance.CharacterInfo.FindIndex(hero => hero.Name == OwnerName);
        CapturedPets thisSummon = Extensions.FindSummonEntry(unit.Stats.theName, ownerid);
        var unitLevel = thisSummon.Level;

        UnitLevel.level = unitLevel;

        curLvl = UnitLevel.level.currentlevel;

        UnitLevel.level = new Level(curLvl);

        UnitLevel.level.experience = unitLevel.totalExperience;
        UnitLevel.level.totalExperience = unitLevel.totalExperience;
        UnitLevel.level.CUR_EXP = unitLevel.CUR_EXP;
        UnitLevel.level.NEXT_EXP = unitLevel.NEXT_EXP;

        if (summonType == EnemyType.NORMAL)
        {
            typeColorCode = "#FFFFFF";
        }

        if (summonType == EnemyType.BABY)
        {
            typeColorCode = "#ff03ff";
        }

        if (summonType == EnemyType.MUTANT)
        {
            typeColorCode = "#9B009B";
        }
    }

    //public void OnLevelUp()
    //{
    //    gameManager.Chat.AddToChatOutput("Summon " + unit.Stats.displayName + " leveled up from level " + curLvl + " to level " + UnitLevel.level.currentlevel + "!");
    //    int levelsUpped = UnitLevel.level.currentlevel - curLvl;
    //    int ownerid = HeroDataManager.instance.CharacterInfo.FindIndex(hero => hero.Name == OwnerName);
    //    CapturedPets thisSummon = Extensions.FindSummonEntry(unit.Stats.theName, ownerid);
    //    for (int i = 0; i < levelsUpped; i++)
    //    {
    //        unit.Stats.unspentStatPoints += HeroDataManager.instance.UnitDatabase.statpointsPerLevel;
    //        statPanel.CalculateBonus(this);
    //        //implement skill learning mechanics here
    //        if (summonType == EnemyType.BABY || summonType == EnemyType.MUTANT)
    //            LearnSkills(thisSummon);
    //    }
    //    curLvl = UnitLevel.level.currentlevel;

    //    //here we will write the data about pet to the HeroData manager so all the changes will be saved and data will be persistent

    //    thisSummon.Stats = unit.Stats;
    //    thisSummon.Level = UnitLevel.level;
    //    //if (HeroDataManager.instance.CharacterInfo[ownerid].isMainCharacter)
    //    //{
    //    //    Actions.OnMainHeroSummonChange(HeroDataManager.instance.CharacterInfo[ownerid], summon);
    //    //}
    //}

    public void LearnSkills(CapturedPets thisSummon)
    {
        if (Random.Range(0, 100) <= HeroDataManager.instance.UnitDatabase.SkillLearnChance)
        {
            var summonDB = HeroDataManager.instance.UnitDatabase.EnemyList.FirstOrDefault(summon => summon.ID == BaseID);
            int skillTotalCount = summonDB.ActiveSkills.Count + summonDB.PassiveSkills.Count;
            Debug.Log("skilltotalCount = " + skillTotalCount.ToString());
            int randomNumber = Random.Range(0, skillTotalCount);
            Debug.Log("randomNumber = " + randomNumber.ToString());
            if (summonDB.ActiveSkills.Count > 0 && randomNumber < summonDB.ActiveSkills.Count)
            {
                var ActiveSkills = GetComponent<Abilities>().MagicAttacks;
                //we learn active skill
                if (thisSummon.MagicAttacks.Any(skillId => skillId == summonDB.ActiveSkills[randomNumber].possibleSkill.ID))
                {
                    //as active skill can't grow level so we do something different
                    //like, well, best luck next time
                    Debug.Log("Summon already has this skill, could not learn");
                }
                else
                {
                    //we actually learn that skill by doing next:
                    thisSummon.MagicAttacks.Add(summonDB.ActiveSkills[randomNumber].possibleSkill.ID);
                    GameManager.instance.Chat.AddToChatOutput("<" + typeColorCode + ">" + unit.Stats.displayName + "</color> learned a new skill <#00FF00>[" + summonDB.ActiveSkills[randomNumber].possibleSkill.SkillName + "]</color>!");
                }
            }
            else
            {
                var PassiveSkills = GetComponent<Abilities>().PassiveSkills;
                int newRandomNumber = randomNumber - summonDB.ActiveSkills.Count;
                Debug.Log("new randomNumber = " + newRandomNumber.ToString());
                //we check if unit already has the skill we are about to learn
                if (thisSummon.PassiveSkills.Any(skillId => skillId == summonDB.PassiveSkills[newRandomNumber].posPassive.ID)/* && summonDB.PassiveSkills[randomNumber].posPassive.SkillLevel != 3 */)
                {
                    //and if he has, we check if that skill level is lower than maximal possible
                    if (summonDB.PassiveSkills[newRandomNumber].posPassive.SkillLevel == 3)
                    {
                        //if yes, better luck next time
                    }
                    else
                    {
                        //and if not, we increase the level
                        //basically delete old skill of lower level
                            thisSummon.PassiveSkills.Remove(summonDB.PassiveSkills[newRandomNumber].posPassive.ID);
                        // and add new one of 1 level higher
                            thisSummon.PassiveSkills.Add(summonDB.PassiveSkills[newRandomNumber].posPassive.NextLevelSkill.ID);
                        GameManager.instance.Chat.AddToChatOutput("<" + typeColorCode + ">" + unit.Stats.displayName + "</color> upgraded the skill <#00FF00>[" + summonDB.PassiveSkills[newRandomNumber].posPassive.SkillName + "]</color> to level <#00FF00>" + summonDB.PassiveSkills[newRandomNumber].posPassive.NextLevelSkill.SkillLevel.ToString() + "</color>!");
                    }

                }
                else //if we don't have that skill yet
                {
                    //we actually learn that skill by doing next:
                    thisSummon.PassiveSkills.Add(summonDB.PassiveSkills[newRandomNumber].posPassive.ID);
                    GameManager.instance.Chat.AddToChatOutput("<" + typeColorCode + ">" + unit.Stats.displayName + "</color> learned a new skill <#00FF00>[" + summonDB.PassiveSkills[newRandomNumber].posPassive.SkillName + "]</color>!");
                }
            }
        }
    }

    private void OnDisable()
    {
        //if (HeroDataManager.instance)
        //{
        //    Debug.Log("OnDisable fired");
        //    int ownerid = HeroDataManager.instance.CharacterInfo.FindIndex(hero => hero.Name == OwnerName);
        //    CapturedPets thisSummon = Extensions.FindSummonEntry(unit.Stats.theName, ownerid);
        //    //UnitLevel.Start();
        //    thisSummon.Stats = unit.Stats;
        //    thisSummon.Level = UnitLevel.level;
        //}
    }

    private void OnDestroy()
    {
        //if (HeroDataManager.instance)
        //{
        //    Debug.Log("OnDestroy fired");
        //    int ownerid = HeroDataManager.instance.CharacterInfo.FindIndex(hero => hero.Name == OwnerName);
        //    CapturedPets thisSummon = Extensions.FindSummonEntry(unit.Stats.theName, ownerid);
        //    UnitLevel.UpdateExp();
        //    thisSummon.Stats = unit.Stats;
        //    thisSummon.Level = UnitLevel.level;
        //}
    }

}
