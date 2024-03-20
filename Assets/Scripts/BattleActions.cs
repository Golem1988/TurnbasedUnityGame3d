using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class BattleActions : MonoBehaviour
{
    [SerializeField] Transform FloatingText;
    //[SerializeField] Transform HealVFX;
    [SerializeField] BattleStateMachine BSM;
    //[SerializeField] GameManager gameManager;
    //private bool actionStarted = false;

    private void OnValidate()
    {
        BSM = GetComponent<BattleStateMachine>();
    }

    private void Start()
    {
        BSM = GetComponent<BattleStateMachine>();

    }

    private void OnEnable()
    {
        Actions.OnDamageReceived += DamagePopup;
        Actions.OnRestore += RestoreHPMPRP;
        Actions.OnBurn += BurnHPMP;
        Actions.OnDodge += DodgePopup;
        //Actions.OnEnemySpawn += PopulateSkilllist;
        Actions.OnRessurect += ResPopup;
        //Actions.OnDoDamage += DoDamage;
        Actions.OnBarChange += SetBarSize;
        Actions.OnSummonCapture += CaptureNew;
        Actions.OnSummonSpawn += SpawnSummon;
        Actions.OnZeroHealth += ZeroHP;
        Actions.OnUnitDeath += UnitDeath;
        //Actions.OnBattleEnd += BattleEnd;
    }

    private void OnDisable()
    {
        Actions.OnDamageReceived -= DamagePopup;
        Actions.OnRestore -= RestoreHPMPRP;
        Actions.OnBurn -= BurnHPMP;
        Actions.OnDodge -= DodgePopup;
        //Actions.OnEnemySpawn -= PopulateSkilllist;
        Actions.OnRessurect -= ResPopup;
        //Actions.OnDoDamage -= DoDamage;
        Actions.OnBarChange -= SetBarSize;
        Actions.OnSummonCapture -= CaptureNew;
        Actions.OnSummonSpawn -= SpawnSummon;
        Actions.OnZeroHealth -= ZeroHP;
        Actions.OnUnitDeath -= UnitDeath;
        //Actions.OnBattleEnd -= BattleEnd;
    }

    public void DamagePopup(Transform target, bool isCritical, float DamageAmount, bool isHeal, AffectedStat affectedStat)
    {
        Vector3 vector3 = new Vector3(0f, 1f, 0f);
        var go = Instantiate(FloatingText, target.position + vector3, Quaternion.identity, target);
        if (isCritical == true)
        {
            go.GetComponentInChildren<SpriteRenderer>().enabled = true;
            go.GetComponentInChildren<TextMeshPro>().fontSize = 6;
            if(!isHeal)
                go.GetComponentInChildren<TextMeshPro>().color = Color.red;
        }

        if (isHeal)
        {
            if (!isCritical)
                go.GetComponentInChildren<SpriteRenderer>().enabled = false;
            if (affectedStat == AffectedStat.HP)
                go.GetComponentInChildren<TextMeshPro>().color = Color.green;
            if (affectedStat == AffectedStat.MP)
                go.GetComponentInChildren<TextMeshPro>().color = Color.blue;
            go.GetComponentInChildren<TextMeshPro>().text = DamageAmount.ToString();
            //Instantiate(HealVFX, target.position, Quaternion.identity, target);
        }

        else
        {
            if (!isCritical)
                go.GetComponentInChildren<SpriteRenderer>().enabled = false;
            if (affectedStat == AffectedStat.HP)
            {
                go.GetComponentInChildren<TextMeshPro>().color = new Color32(197, 164, 0, 255);
                go.GetComponentInChildren<TextMeshPro>().text = DamageAmount.ToString();
            }

            if (affectedStat == AffectedStat.MP)
            {
                go.GetComponentInChildren<TextMeshPro>().color = Color.blue;
                go.GetComponentInChildren<TextMeshPro>().text = "-" + DamageAmount.ToString();
            }

        }

        //SetBarSize(target.GetComponent<UnitStateMachine>(), AffectedStat.HP);
        //var unit = target.GetComponent<UnitStateMachine>().unit;
        //target.GetComponent<UnitUI>().healthBar.SetSize(((unit.curHP * 100) / unit.baseHP) / 100);
    }

    public void RestoreHPMPRP(Transform target, float amount, AffectedStat affectedStat)
    {   
        DamagePopup(target, false, amount, true, affectedStat);
        var unit = target.GetComponent<UnitAttributes>().Stats;
        if (affectedStat == AffectedStat.HP)
        {
            unit.curHP += amount;
            if (unit.curHP > unit.baseHP)
            {
                unit.curHP = unit.baseHP;
            }
            target.GetComponent<UnitUI>().healthBar.SetSize(unit.curHP / unit.baseHP);
        }
        if (affectedStat == AffectedStat.MP)
        {
            unit.curMP += amount;
            if (unit.curMP > unit.baseMP)
            {
                unit.curMP = unit.baseMP;
            }
            target.GetComponent<UnitUI>().manaBar.SetSize(unit.curMP / unit.baseMP);
        }
    }

    public void BurnHPMP(Transform target, float amount, AffectedStat affectedStat)
    {
        DamagePopup(target, false, amount, false, affectedStat);
        var unit = target.GetComponent<UnitAttributes>().Stats;
        if (affectedStat == AffectedStat.HP)
        {
            unit.curHP -= amount;
            if (unit.curHP < unit.baseHP)
            {
                unit.curHP = 0;
                Actions.OnZeroHealth(target.GetComponent<UnitStateMachine>());
            }
            target.GetComponent<UnitUI>().healthBar.SetSize(unit.curHP / unit.baseHP);
        }
        if (affectedStat == AffectedStat.MP)
        {
            unit.curMP -= amount;
            if (unit.curMP < unit.baseMP)
            {
                unit.curMP = 0;
            }
            target.GetComponent<UnitUI>().manaBar.SetSize(unit.curMP / unit.baseMP);
        }
    }

    public void DodgePopup(Transform target)
    {
        var ui = target.GetComponent<UnitUI>();
        ui.animator.Play("Hurt");
        Vector3 vector3 = new Vector3(0f, 1f, 0f);
        var go = Instantiate(FloatingText, target.transform.position + vector3, Quaternion.identity, target); //tell that we dodged and no damage is dealt
        go.GetComponentInChildren<SpriteRenderer>().enabled = false;
        go.GetComponentInChildren<TextMeshPro>().fontSize = 3;
        go.GetComponentInChildren<TextMeshPro>().color = Color.white;
        go.GetComponentInChildren<TextMeshPro>().text = "DODGE";
    }

    //public void PopulateSkilllist(GameObject enemyObj) //Take skills from the list of possible skills, calculate chances and spawn in according lists
    //{
    //    var PossibleSkills = enemyObj.GetComponent<Enemy>().PossibleSkills;
    //    BaseEnemy enemyClass = enemyObj.GetComponent<Enemy>().enemy;
    //    for (int i = 0; i < PossibleSkills.Count; i++)
    //    {
    //        if (Random.Range(0, 100) < PossibleSkills[i].skillSpawnChance)
    //        {
    //            BaseAttack NewSkill = PossibleSkills[i].possibleSkill;
    //            if (NewSkill.attackType == "Spell")
    //            {
    //                enemyClass.MagicAttacks.Add(NewSkill);
    //            }
    //            else
    //            {
    //                enemyClass.attacks.Add(NewSkill);
    //            }
    //        }
    //    }
    //}

    public void ResPopup(Transform target, float ResHP)
    {
        Vector3 vector3 = new Vector3(0f, 1f, 0f);
        var go2 = Instantiate(FloatingText, target.transform.position + vector3, Quaternion.identity);
        go2.GetComponentInChildren<SpriteRenderer>().enabled = false;
        go2.GetComponentInChildren<TextMeshPro>().color = Color.green;
        go2.GetComponentInChildren<TextMeshPro>().text = ResHP.ToString();
    }

    //public void DoDamage(UnitStateMachine actor, float trueDamage, SkillType skillType, List<UnitStateMachine> targetList, int strikeCount, bool canCrit, bool isDodgeable, bool ignoresDef, AffectedStat affectedStat, bool isHeal, List<StatusEffectList> statusEffectLists) //we get the true damage calculated from SO attack script, same for targetCount
    //{
    //    bool isCritical = false;
    //    for (int i = 0; i < targetList.Count; i++)
    //    {
    //        for (int j = 0; j < strikeCount; j++)
    //        {
    //            if (canCrit && Random.Range(0, 100) <= actor.unit.Stats.curCRIT)
    //            {
    //                isCritical = true;
    //                trueDamage = Mathf.Round(trueDamage * actor.unit.Stats.critDamage);
    //            }
    //            stop striking the target if it's already dead
    //            if (targetList[i].currentState != UnitStateMachine.TurnState.DEAD)
    //                targetList[i].TakeDamage(actor, trueDamage, isDodgeable, isCritical, ignoresDef, affectedStat, isHeal, statusEffectLists);

    //            if (targetList[0].dodgedAtt == false)
    //            {
    //                float HealAmount = trueDamage * 30 / 100; //testing vampirism and restore HP. How much we should heal and how much %% from this.
    //                Actions.OnRestoreHP(actor.transform, HealAmount);
    //            }
    //        }
    //    }
    //    StartCoroutine(DoDamageCo(actor, trueDamage, targetList, strikeCount, canCrit, isDodgeable, ignoresDef, affectedStat, isHeal));
    //    if (targetList.Count > 1)
    //    {
    //        CalcDamageForEachTarget(actor, trueDamage, targetList, strikeCount, canCrit, isDodgeable, ignoresDef, affectedStat, isHeal);
    //    }

    //    else //if we only have 1 target
    //    {
    //        if (canCrit && Random.Range(0, 100) <= actor.unit.Stats.curCRIT)
    //        {
    //            isCritical = true;
    //            trueDamage = Mathf.Round(trueDamage * actor.unit.Stats.critDamage);
    //        }
    //        for (int i = 0; i < strikeCount; i++)
    //        {
    //            targetList[0].TakeDamage(actor, trueDamage, isDodgeable, isCritical, ignoresDef, affectedStat, isHeal);
    //        }


    //    }

    //    ui.manaBar.SetSize(((unit.curMP * 100) / unit.baseMP) / 100);

    //}

    //public IEnumerator DoDamageCo(UnitStateMachine actor, float trueDamage, SkillType skillType, List<UnitStateMachine> targetList, int strikeCount, bool canCrit, bool isDodgeable, bool ignoresDef, AffectedStat affectedStat, bool isHeal, List<StatusEffectList> statusEffectLists) //we get the true damage calculated from SO attack script, same for targetCount
    //{
    //    Debug.Log("Coroutine running");
    //    bool isCritical = false;
    //    var ui = actor.GetComponent<UnitUI>();
    //    //actor.isAttacking = true;
    //    for (int i = 0; i < targetList.Count; i++)
    //    {
    //        for (int j = 0; j < strikeCount; j++)
    //        {
    //            if (canCrit && Random.Range(0, 100) <= actor.unit.Stats.curCRIT)
    //            {
    //                isCritical = true;
    //                trueDamage = Mathf.Round(trueDamage * actor.unit.Stats.critDamage);
    //            }
    //            //stop striking the target if it's already dead
    //            if (targetList[i].currentState == UnitStateMachine.TurnState.DEAD)
    //                break;
    //            ui.animator.Play("Attack");
    //            yield return new WaitForSeconds(0.5f);
    //            targetList[i].TakeDamage(actor, trueDamage, isDodgeable, isCritical, ignoresDef, affectedStat, isHeal, statusEffectLists);
    //            yield return new WaitForSeconds(0.5f);
    //        }
    //    }
    //    bool isCritical = false;
    //    var ui = actor.GetComponent<UnitUI>();
    //    for (int i = 0; i < targetList.Count; i++)
    //    {
    //        for (int j = 0; j < strikeCount; j++)
    //        {
    //            ui.animator.Play("Attack");
    //            while (ui.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
    //            {
    //                yield return null;
    //            }
    //            if (canCrit && Random.Range(0, 100) <= actor.unit.Stats.curCRIT)
    //            {
    //                isCritical = true;
    //                trueDamage = Mathf.Round(trueDamage * actor.unit.Stats.critDamage);
    //            }
    //            stop striking the target if it's already dead
    //            if (targetList[i].currentState != UnitStateMachine.TurnState.DEAD)
    //                targetList[i].TakeDamage(actor, trueDamage, isDodgeable, isCritical, ignoresDef, affectedStat, isHeal, statusEffectLists);
    //            else
    //                break;
    //            if (targetList[0].dodgedAtt == false)
    //            {
    //                float HealAmount = trueDamage * 30 / 100; //testing vampirism and restore HP. How much we should heal and how much %% from this.
    //                Actions.OnRestoreHP(actor.transform, HealAmount);
    //            }
    //        }
    //    }
    //    actor.isAttacking = false;
    //}

    public void ZeroHP(UnitStateMachine target)
    {
        //if (Random.Range(0, 100) <= 25) //25 = res chance
        //{
        //    target.unit.Stats.curHP = Mathf.Round((target.unit.Stats.baseHP / 100) * 50); //50 = %% HP when ressurected
        //    Actions.OnDamageReceived(transform, false, target.unit.Stats.curHP, true);
        //    //Instantiate(ui.RessurectVFX, transform.position, Quaternion.identity, transform);
        //}
        //else
        //{
            Actions.OnUnitDeath(target);
        //} 

    }

    public void UnitDeath(UnitStateMachine target)
    {
        target.ui.animator.Play("Die");
        target.currentState = UnitStateMachine.TurnState.DEAD;
    }

    public void SetBarSize(UnitStateMachine target, AffectedStat theBar)
    {
        if (theBar == AffectedStat.HP)
            target.GetComponent<UnitUI>().healthBar.SetSize(target.unit.Stats.curHP / target.unit.Stats.baseHP);
        if (theBar == AffectedStat.MP)
            target.GetComponent<UnitUI>().manaBar.SetSize(target.unit.Stats.curMP / target.unit.Stats.baseMP);
        if (target.gameObject.CompareTag("Hero") && theBar == AffectedStat.RP)
        {
            target.GetComponent<UnitUI>().rageBar.SetSize(target.unit.Stats.curRage / target.unit.Stats.maxRage);
        }
    }

    void CaptureNew(UnitStateMachine catcher, UnitStateMachine target)
    {
        if (Random.Range(0, 100) < 100)
        {
            CapturedPets AddInfo = new ();
            AddInfo.BaseID = target.GetComponent<Enemy>().BaseID;
            AddInfo.UniqueID = System.Guid.NewGuid().ToString();
            AddInfo.Type = target.GetComponent<Enemy>().enemyType;
            AddInfo.Rarity = target.GetComponent<Enemy>().enemyRarity;
            AddInfo.Stats = target.GetComponent<UnitAttributes>().Stats;
            AddInfo.Stats.curHP = target.GetComponent<UnitAttributes>().Stats.baseHP;
            AddInfo.Stats.curMP = target.GetComponent<UnitAttributes>().Stats.baseMP;
            AddInfo.Level = target.GetComponent<UnitLevel>().level;
            AddInfo.Level.NEXT_EXP = target.GetComponent<UnitLevel>().level.requiredExp;

            var ma = target.GetComponent<Abilities>().MagicAttacks;
            foreach (ActiveSkill skill in ma)
            {
                AddInfo.MagicAttacks.Add(skill.ID);
            }

            var ps = target.GetComponent<Abilities>().PassiveSkills;
            foreach (PassiveSkill skill in ps)
            {
                AddInfo.PassiveSkills.Add(skill.ID);
            }

            AddInfo.isDeployable = true;
            AddInfo.active = false;
            var SummonList = catcher.GetComponent<SummonHandler>().SummonList;
            SummonList.Add(AddInfo);

            //RemoveFromLists(target);
            target.currentState = UnitStateMachine.TurnState.DEAD;
            //target.gameObject.SetActive(false);
        }
        else
        {
            Actions.OnDodge(BSM.PerformList[0].AttackersTarget.transform);
        }
    }

    public void MakeSummonsDeploayable(List<GameObject> BattlerHeroes)
    {
        foreach (GameObject hero in BattlerHeroes)
        {
            var summonList = hero.GetComponent<SummonHandler>().SummonList;
            foreach (CapturedPets summon in summonList)
            {
                if (!summon.active)
                    summon.isDeployable = true;
            }
        }
    }

    public void SpawnSummon(UnitStateMachine owner, int summonIndex) //SummonList[index];
    {
        //we want the refference to the BSM.AllySpots
        var AllySpots = BSM.AllySpots;
        //we want to determine if player already has summoned summon
        //one way to do it is to check if there's a summon on the spot that is belonging to that player
        int index = AllySpots.FindIndex(allies => allies.UnitOnSpot == owner.gameObject);
        int ownerSpot = AllySpots[index].Spot;
        bool hasSummon = AllySpots.Any(allies => allies.Spot == ownerSpot + 5);
        if (hasSummon)
        {
            //if he has, then we want to remove summoned summon from the BSM.AllySpots, from HeroesInBattle and 
            //bsm.allyspots
            var findOldSummon = AllySpots.FirstOrDefault(item => item.Spot == ownerSpot + 5);
            GameObject oldSummon = findOldSummon.UnitOnSpot;
            RemoveFromLists(oldSummon.GetComponent<UnitStateMachine>());
            string oldSummonID = oldSummon.name;
            var SummonList = owner.GetComponent<SummonHandler>().SummonList;
            int oldSummonIndex = SummonList.FindIndex(summons => summons.UniqueID == oldSummonID);
            Debug.Log("Old summon ID: " + oldSummonID);
            SummonList[oldSummonIndex].active = false;
            SummonList[oldSummonIndex].isDeployable = false;

            //to remove it from Performlist we actually run the DEAD state of that unit so he can replace himself in enemy choosen targets but I guess we have to run it here or it will otherwise cause some troubles
            Destroy(oldSummon);
        }
        SpawnSummonX(owner, ownerSpot, summonIndex);
    }

    public void SpawnSummonX(UnitStateMachine owner, int spotIndex, int summonIndex) //SummonList[index];
    {
        //then we want to instantiate this summon and fill all the info about it in that game object to ownerSpawnPoint mirrored to SummonSpawn point
        //var go = Instantiate(Summon[name], BSM.summonSpawnPoints[index].position, Quaternion.Euler(0, -45, 0), BSM.summonSpawnPoints[index]);
        //then we want to add the new summon to the BSM.AllySpots.Spot[BSM.AllySpots.owner.Spot.ID+5]

        var info = owner.GetComponent<SummonHandler>().SummonList;
        //int summonIndex = info.FindIndex(status => status.active == true);
        //instantiate the empty prefab
        var thePrefab = HeroDataManager.instance.UnitDatabase.SummonPrefab;
        GameObject NewSummon = Instantiate(thePrefab, BSM.summonSpawnPoints[spotIndex].position, Quaternion.Euler(0, -45, 0), BSM.summonSpawnPoints[spotIndex]);
        //add the unit model
        GameObject summonModel = Extensions.FindModelPrefab(info[summonIndex].BaseID, false);
        var model = Instantiate(summonModel, NewSummon.GetComponent<ModelLoader>().ModelHolder.position, Quaternion.Euler(0, -45, 0), NewSummon.GetComponent<ModelLoader>().ModelHolder);
        NewSummon.GetComponent<ModelLoader>().Model = model;
        //specify the avatar sprite
        NewSummon.GetComponent<UnitUI>().Avatar = Extensions.FindSprite(info[summonIndex].BaseID, false);
        //fill in the information
        NewSummon.name = info[summonIndex].UniqueID;
        NewSummon.GetComponent<UnitAttributes>().Stats = info[summonIndex].Stats;
        NewSummon.GetComponent<UnitAttributes>().Stats.theName = NewSummon.name;
        NewSummon.GetComponent<UnitStateMachine>().BSM = BSM;
        NewSummon.GetComponent<UnitUI>().healthBar.SetSize(NewSummon.GetComponent<UnitAttributes>().Stats.curHP / NewSummon.GetComponent<UnitAttributes>().Stats.baseHP);
        NewSummon.GetComponent<UnitUI>().manaBar.SetSize(NewSummon.GetComponent<UnitAttributes>().Stats.curMP / NewSummon.GetComponent<UnitAttributes>().Stats.baseMP);
        //NewSummon.GetComponent<UnitAttributes>().Stats.displayName = info[summonIndex].Stats.displayName;
        NewSummon.tag = "Summon";
        NewSummon.GetComponent<UnitLevel>().level = info[summonIndex].Level;
        //Debug.Log("Summon level were set in BattleActions, level = " + NewSummon.GetComponent<UnitLevel>().level.currentlevel.ToString());
        NewSummon.GetComponent<Summon>().OwnerName = owner.unit.Stats.theName;
        NewSummon.GetComponent<Summon>().UniqueID = info[summonIndex].UniqueID;
        NewSummon.GetComponent<Summon>().BaseID = info[summonIndex].BaseID;
        NewSummon.GetComponent<Summon>().summonType = info[summonIndex].Type;
        NewSummon.GetComponent<Summon>().summonRarity = info[summonIndex].Rarity;
        //fill in information about skills
        foreach (string maId in info[summonIndex].MagicAttacks)
        {
            ActiveSkill magicAttack = Extensions.FindActiveSkillID(maId);
            if (magicAttack != null)
                NewSummon.GetComponent<Abilities>().MagicAttacks.Add(magicAttack);
        }
        foreach (string passId in info[summonIndex].PassiveSkills)
        {
            PassiveSkill passiveSkill = GameManager.instance.SkillDatabase.PassiveSkills.FirstOrDefault(skill => skill.ID == passId);
            if (passiveSkill != null)
                NewSummon.GetComponent<Abilities>().PassiveSkills.Add(passiveSkill);
        }
        //make active make non-deployable
        info[summonIndex].active = true;
        info[summonIndex].isDeployable = false;
        //add to lists
        BattlePosition SummonInfo = new();
        SummonInfo.UnitOnSpot = NewSummon;
        SummonInfo.Spot = spotIndex + 5;
        BSM.AllySpots.Add(SummonInfo);
        BSM.HeroesInBattle.Add(NewSummon);
        //BSM.AllyButtons();
    }

    public void RemoveFromLists(UnitStateMachine target)
    {
        GameObject unitToRemove = target.gameObject;
        List<BattlePosition> onSpots;

        //target won't perform himself
        BSM.PerformList.RemoveAll(index => index.Attacker == unitToRemove);

        //if target = ally and there are more than him left
        if ((target.CompareTag("Hero") || target.CompareTag("Summon")) && BSM.HeroesInBattle.Count > 0)
        {
            onSpots = BSM.AllySpots;
            BSM.HeroesInBattle.Remove(unitToRemove);
            for (int i = 0; i < BSM.PerformList.Count; i++)
            {
                if (BSM.PerformList[i].AttackersTarget == unitToRemove && BSM.HeroesInBattle.Count > 1)
                {
                    BSM.PerformList[i].AttackersTarget = BSM.HeroesInBattle[Random.Range(0, BSM.HeroesInBattle.Count)];
                    BSM.PerformList[i].Attacker.GetComponent<UnitStateMachine>().ChosenAttackTarget = BSM.PerformList[i].AttackersTarget;
                }
                else if (BSM.PerformList[i].AttackersTarget == unitToRemove && BSM.HeroesInBattle.Count == 1)
                {
                    BSM.PerformList[i].AttackersTarget = BSM.HeroesInBattle[0];
                    BSM.PerformList[i].Attacker.GetComponent<UnitStateMachine>().ChosenAttackTarget = BSM.PerformList[i].AttackersTarget;
                }
            }
            onSpots.RemoveAll(index => index.UnitOnSpot == unitToRemove);
            if (unitToRemove.CompareTag("Hero"))
                BSM.AllyButtons();
        }
        
        else if (target.CompareTag("Enemy") && BSM.EnemiesInBattle.Count > 0)
        {
            onSpots = BSM.EnemySpots;
            BSM.EnemiesInBattle.Remove(unitToRemove);
            for (int i = 0; i < BSM.PerformList.Count; i++)
            {
                if (BSM.PerformList[i].AttackersTarget == unitToRemove && BSM.EnemiesInBattle.Count > 1)
                {
                    BSM.PerformList[i].AttackersTarget = BSM.EnemiesInBattle[Random.Range(0, BSM.EnemiesInBattle.Count)];
                    BSM.PerformList[i].Attacker.GetComponent<UnitStateMachine>().ChosenAttackTarget = BSM.PerformList[i].AttackersTarget;
                    if (BSM.PerformList[i].choosenAttack.ID == "e2a08119519b2ec48bbe851df8d319d9")
                    {
                        BSM.PerformList[i].choosenAttack = Extensions.FindActiveSkillID("010efba02612ef34db18caadaceb37dd");
                    }
                }
                else if (BSM.PerformList[i].AttackersTarget == unitToRemove && BSM.EnemiesInBattle.Count == 1)
                {
                    BSM.PerformList[i].AttackersTarget = BSM.EnemiesInBattle[0];
                    BSM.PerformList[i].Attacker.GetComponent<UnitStateMachine>().ChosenAttackTarget = BSM.PerformList[i].AttackersTarget;
                }
            }
            onSpots.RemoveAll(index => index.UnitOnSpot == unitToRemove);
            BSM.EnemyButtons();
        }

    }
}
