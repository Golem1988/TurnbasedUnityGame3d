using UnityEngine;

public static class Extensions
{
    //To change layer in all children of the gameObject
    public static void SetLayer(this GameObject obj, int layer)
    {
        obj.layer = layer;
        // Loop through each child of the parent transform
        foreach (Transform child in obj.transform)
        {
            // set layer for each child
            child.gameObject.SetLayer(layer);
        }
    }

    //To destroy all the children in the object
    public static void DestroyAllChildren(this Transform parent)
    {
        // Loop through each child of the parent transform
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            // Destroy the child game object
            Object.Destroy(parent.GetChild(i).gameObject);
        }
    }

    public static Sprite FindSprite(string unitID, bool isHero)
    {
        if (isHero)
        {
            var heroList = HeroDataManager.instance.UnitDatabase.HeroList;
            foreach (HeroUnit unit in heroList)
            {
                // Check if the current unitID matches the desired ID
                if (unit.ID == unitID)
                {
                    // Return the Sprite if a match is found
                    return unit.UnitAvatar;
                }
            }
        }
        else
        {
            var enemyList = HeroDataManager.instance.UnitDatabase.EnemyList;
            foreach (EnemyUnit unit in enemyList)
            {
                // Check if the current unitID matches the desired ID
                if (unit.ID == unitID)
                {
                    // Return the Sprite if a match is found
                    return unit.UnitAvatar;
                }
            }
        }
        // Return null if no unit with the specified id is found
        return null;
    }

    public static Sprite FindActiveSkillSprite(string skillID)
    {
        var skillList = GameManager.instance.SkillDatabase.ActiveSkills;
        foreach (ActiveSkill skill in skillList)
        {
            // Check if the current unitID matches the desired ID
            if (skill.ID == skillID)
            {
                // Return the Sprite if a match is found
                return skill.SkillIcon;
            }
        }
        // Return null if no unit with the specified id is found
        return null;
    }

    public static CharacterInformation FindHeroEntry(string unitName)
    {
        var CharInfo = HeroDataManager.instance.CharacterInfo;
        foreach (CharacterInformation unit in CharInfo)
        {
            // Check if the current unitID matches the desired ID
            if (unit.Name == unitName)
            {
                // Return the Sprite if a match is found
                return unit;
            }
        }
        // Return null if no unit with the specified id is found
        return null;
    }

    //public static int FindHeroIndex(string unitName)
    //{
    //    var CharInfo = HeroData.instance.CharacterInfo;
    //    for (int i = 0; i < CharInfo.Count; i++)
    //    {
    //        if (CharInfo[i].Name == unitName)
    //        {
    //            // Return the Sprite if a match is found
    //            return i;
    //        }
    //    }
    //    return -1;
    //}

    public static CapturedPets FindSummonEntry(string summonID, int heroIndex)
    {
        var SummonList = HeroDataManager.instance.CharacterInfo[heroIndex].SummonList;
        foreach (CapturedPets unit in SummonList)
        {
            // Check if the current unitID matches the desired ID
            if (unit.UniqueID == summonID)
            {
                // Return the Sprite if a match is found
                return unit;
            }
        }
        // Return null if no unit with the specified id is found
        return null;
    }

    public static GameObject FindModelPrefab(string unitID, bool isHero)
    {
        if (!isHero)
        {
            var summonDB = HeroDataManager.instance.UnitDatabase.EnemyList;
            foreach (EnemyUnit unit in summonDB)
            {
                // Check if the current unitID matches the desired ID
                if (unit.ID == unitID)
                {
                    // Return the GameObject if a match is found
                    return unit.ModelPrefab;
                }
            }
        }
        else
        {
            var summonDB = HeroDataManager.instance.UnitDatabase.HeroList;
            foreach (HeroUnit unit in summonDB)
            {
                // Check if the current unitID matches the desired ID
                if (unit.ID == unitID)
                {
                    // Return the GameObject if a match is found
                    return unit.ModelPrefab;
                }
            }
        }
        // Return null if no unit with the specified id is found
        return null;
    }

    public static ActiveSkill FindActiveSkillID(string skillID)
    {
        var skillDB = GameManager.instance.SkillDatabase.ActiveSkills;
        foreach (ActiveSkill skill in skillDB)
        {
            // Check if the current unitID matches the desired ID
            if (skill.ID == skillID)
            {
                // Return the GameObject if a match is found
                return skill;
            }
        }
        // Return null if no unit with the specified id is found
        return null;
    }

    public static PassiveSkill FindPassiveSkillID(string skillID)
    {
        var skillDB = GameManager.instance.SkillDatabase.PassiveSkills;
        foreach (PassiveSkill skill in skillDB)
        {
            // Check if the current unitID matches the desired ID
            if (skill.ID == skillID)
            {
                // Return the GameObject if a match is found
                return skill;
            }
        }
        // Return null if no unit with the specified id is found
        return null;
    }

    public static string FindMainCharacterName()
    {
        var HeroList = HeroDataManager.instance.CharacterInfo;
        foreach (CharacterInformation hero in HeroList)
        {
            if (hero.isMainCharacter == true)
            {
                //Debug.Log("Found main character: " + hero.Name);
                return hero.Name;
            }
        }
        Debug.Log("main character not found");
        return null;
    }

    public static string FindMainCharacterID()
    {
        var HeroList = HeroDataManager.instance.CharacterInfo;
        foreach (CharacterInformation hero in HeroList)
        {
            if (hero.isMainCharacter == true)
            {
                //Debug.Log("Found main character ID: " + hero.Name);
                return hero.BaseID;
            }
        }
        Debug.Log("main character ID not found");
        return null;
    }
}
