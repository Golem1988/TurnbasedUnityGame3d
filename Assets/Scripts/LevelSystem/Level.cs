using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    public int experience;
    public int totalExperience;
    public int currentlevel;
    //public Action OnLevelUp;
    public int requiredExp;

    public int MAX_EXP;
    public int NEXT_EXP;
    public int MAX_LEVEL = 99;
    public Level(int level)
    {
        MAX_EXP = GetXPForLevel(MAX_LEVEL);
        currentlevel = level;
        experience = GetXPForLevel(level);
        if (experience < totalExperience)
            experience = totalExperience;
        //OnLevelUp = OnLevUp;
        requiredExp = CalculateRequiredExp(level);
    }

    public int GetXPForLevel(int level)
    {
        if (level > MAX_LEVEL) //throw an exception dependant on game design
            return 0;

        int firstPass = 0;
        int secondPass = 0;
        for (int levelCycle = 1; levelCycle < level; levelCycle++)
        {
            firstPass += (int)Math.Floor(levelCycle + (300.0f * Math.Pow(2.0f, levelCycle / 7.0f)));
            secondPass = firstPass / 4;
        }
        if (secondPass > MAX_EXP && MAX_EXP != 0)
            return MAX_EXP;

        if (secondPass < 0)
            return MAX_EXP;

        return secondPass;
    }

    public int CalculateRequiredExp(int level)
    {
        int solveForRequiredExp = 0;
        for (int levelCycle = 1; levelCycle <= level; levelCycle++)
        {
            solveForRequiredExp += (int)Math.Floor(levelCycle + (300.0f * Math.Pow(2.0f, levelCycle / 7.0f)));
        }
        return solveForRequiredExp / 4;
    }

    public int CalculateNext(int level)
    {
        int solveForRequiredExp = 0;
        for (int levelCycle = 1; levelCycle <= level; levelCycle++)
        {
            solveForRequiredExp += (int)Math.Floor(levelCycle + (300.0f * Math.Pow(2.0f, levelCycle / 7.0f)));
        }
        return solveForRequiredExp / 4;
    }

    public int GetLevelForXP(int exp)
    {
        if (exp > MAX_EXP)
            return MAX_EXP;

        int firstPass = 0;
        int secondPass = 0;
        for (int levelCycle = 1; levelCycle <= MAX_LEVEL; levelCycle++)
        {
            firstPass += (int)Math.Floor(levelCycle + (300.0f * Math.Pow(2.0f, levelCycle / 7.0f)));
            secondPass = firstPass / 4;
            if (secondPass > exp)
                return levelCycle;
        }

        if (exp > secondPass)
            return MAX_LEVEL;

        return 0;
    }

    //public bool AddExp(int amount)
    //{
    //    if (amount + experience < 0 || experience > MAX_EXP)
    //    {
    //        if (experience > MAX_EXP)
    //            experience = MAX_EXP;
    //        return false;
    //    }
    //    int oldLevel = GetLevelForXP(experience);
    //    int nextLevel = oldLevel + 1;
    //    experience += amount;
    //    if(oldLevel < GetLevelForXP(experience))
    //    {
    //        if(currentlevel < GetLevelForXP(experience))
    //        {
    //            currentlevel = GetLevelForXP(experience);
    //            if (OnLevelUp != null)
    //                OnLevelUp.Invoke();
    //                requiredExp = CalculateRequiredExp(currentlevel+1);
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    public void AddExp(int amount, UnitAttributes unit)
    {
        if (amount + experience < 0 || experience > MAX_EXP)
        {
            if (experience > MAX_EXP)
                experience = MAX_EXP;
        }
        int oldLevel = GetLevelForXP(experience);
        int nextLevel = oldLevel + 1;
        experience += amount;
        totalExperience += amount;
        if (oldLevel < GetLevelForXP(experience))
        {
            if (currentlevel < GetLevelForXP(experience))
            {
                currentlevel = GetLevelForXP(experience);
                requiredExp = CalculateRequiredExp(currentlevel);
            }
        }
        IncreaseStats(unit);
    }

    public void IncreaseStats(UnitAttributes unit)
    {
        if (unit.gameObject.CompareTag("Hero"))
        {
            int curLvl = unit.gameObject.GetComponent<Character>().curLvl;
            int levelsUpped = currentlevel - curLvl;

            int index = HeroDataManager.instance.CharacterInfo.FindIndex(hero => hero.Name == unit.Stats.displayName);

            if (levelsUpped > 0)
            {
                GameManager.instance.Chat.AddToChatOutput("Hero " + unit.Stats.displayName + "leveled up from level " + curLvl + " to level " + currentlevel + "!");

                var statPanel = GameManager.instance.heroInfoInterface;
                for (int i = 0; i < levelsUpped; i++)
                {
                    unit.Stats.unspentStatPoints += HeroDataManager.instance.UnitDatabase.statpointsPerLevel;
                    statPanel.CalculateBonus(unit);
                }
            }
            //writing data to HeroData
            //int index = Extensions.FindHeroIndex(unit.Stats.theName);
            
            HeroDataManager.instance.CharacterInfo[index].Stats = unit.Stats;
            HeroDataManager.instance.CharacterInfo[index].Level = this;

            unit.gameObject.GetComponent<Character>().curLvl = currentlevel;
        }
        else
        {
            int curLvl = unit.gameObject.GetComponent<Summon>().curLvl;
            int levelsUpped = currentlevel - curLvl;

            var Summon = unit.gameObject.GetComponent<Summon>();
            int ownerid = HeroDataManager.instance.CharacterInfo.FindIndex(hero => hero.Name == Summon.OwnerName);
            CapturedPets thisSummon = Extensions.FindSummonEntry(unit.Stats.theName, ownerid);

            if (levelsUpped > 0)
            {
                GameManager.instance.Chat.AddToChatOutput("Summon " + unit.Stats.displayName + " leveled up from level " + curLvl + " to level " + currentlevel + "!");

                var statPanel = GameManager.instance.summonInfoInterface;
                for (int i = 0; i < levelsUpped; i++)
                {
                    unit.Stats.unspentStatPoints += HeroDataManager.instance.UnitDatabase.statpointsPerLevel;
                    statPanel.CalculateBonus(Summon);
                    //implement skill learning mechanics here
                    if (Summon.summonType == EnemyType.BABY || Summon.summonType == EnemyType.MUTANT)
                        Summon.LearnSkills(thisSummon);
                }
            }
            //here we will write the data about pet to the HeroData manager so all the changes will be saved and data will be persistent

            thisSummon.Stats = unit.Stats;
            thisSummon.Level = this;
            unit.gameObject.GetComponent<Summon>().curLvl = currentlevel;
        }
    }
}
