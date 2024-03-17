using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLevel : MonoBehaviour
{
    public Level level;
    //public int startingExperience;
    //public int experience;
    //public int currentlevel;
    //public int requiredExperience;

    //public int MAX_EXP;
    //public int NEXT_EXP;
    //public int MAX_LEVEL = 99;

    //private int curLvl;
    //private HeroInfoInterface statPanel;


    //private void Start()
    //{
    //    statPanel = GameManager.instance.heroInfoInterface;
    //    int level = currentlevel;
    //    startingExperience = GetXPForLevel(level);
    //    requiredExperience = CalculateRequiredExp(level);
    //}

    //public void AddExp(int amount, UnitAttributes unit)
    //{
    //    if (amount + experience < 0 || experience > MAX_EXP)
    //    {
    //        if (experience > MAX_EXP)
    //            experience = MAX_EXP;
    //    }
    //    int oldLevel = GetLevelForXP(experience);
    //    int nextLevel = oldLevel + 1;
    //    experience += amount;
    //    if (oldLevel < GetLevelForXP(experience))
    //    {
    //        if (currentlevel < GetLevelForXP(experience))
    //        {
    //            currentlevel = GetLevelForXP(experience);
    //            requiredExperience = CalculateRequiredExp(currentlevel + 1);
    //            //fire level increase
    //            OnLevelUp(unit);
    //        }
    //    }
    //}

    //public void OnLevelUp(UnitAttributes unit)
    //{
    //    if (unit.gameObject.CompareTag("Hero"))
    //    {
    //        GameManager.instance.Chat.AddToChatOutput("Hero" + unit.Stats.displayName + "leveled up from level " + curLvl + " to level " + currentlevel + "!");
    //        int levelsUpped = currentlevel - curLvl;
    //        for (int i = 0; i < levelsUpped; i++)
    //        {
    //            unit.Stats.unspentStatPoints += HeroDataManager.instance.UnitDatabase.statpointsPerLevel;
    //            statPanel.CalculateBonus(unit);
    //        }
    //        //writing data to HeroData
    //        //int index = Extensions.FindHeroIndex(unit.Stats.theName);
    //        int index = HeroDataManager.instance.CharacterInfo.FindIndex(hero => hero.Name == unit.Stats.displayName);
    //        HeroDataManager.instance.CharacterInfo[index].Stats = unit.Stats;
    //        HeroDataManager.instance.CharacterInfo[index].Level = this;
    //    }


    //    curLvl = currentlevel;
    //}

    //public int GetLevelForXP(int exp)
    //{
    //    if (exp > MAX_EXP)
    //        return MAX_EXP;

    //    int firstPass = 0;
    //    int secondPass = 0;
    //    for (int levelCycle = 1; levelCycle <= MAX_LEVEL; levelCycle++)
    //    {
    //        firstPass += (int)Math.Floor(levelCycle + (300.0f * Math.Pow(2.0f, levelCycle / 7.0f)));
    //        secondPass = firstPass / 4;
    //        if (secondPass > exp)
    //            return levelCycle;
    //    }

    //    if (exp > secondPass)
    //        return MAX_LEVEL;

    //    return 0;
    //}

    //public int GetXPForLevel(int level)
    //{
    //    if (level > MAX_LEVEL) //throw an exception dependant on game design
    //        return 0;

    //    int firstPass = 0;
    //    int secondPass = 0;
    //    for (int levelCycle = 1; levelCycle < level; levelCycle++)
    //    {
    //        firstPass += (int)Math.Floor(levelCycle + (300.0f * Math.Pow(2.0f, levelCycle / 7.0f)));
    //        secondPass = firstPass / 4;
    //    }
    //    if (secondPass > MAX_EXP && MAX_EXP != 0)
    //        return MAX_EXP;

    //    if (secondPass < 0)
    //        return MAX_EXP;

    //    return secondPass;
    //}

    //public int CalculateRequiredExp(int level)
    //{
    //    int solveForRequiredExp = 0;
    //    for (int levelCycle = 1; levelCycle <= level; levelCycle++)
    //    {
    //        solveForRequiredExp += (int)Math.Floor(levelCycle + (300.0f * Math.Pow(2.0f, levelCycle / 7.0f)));
    //    }
    //    return solveForRequiredExp / 4;
    //}

}
