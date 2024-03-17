//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using TMPro;
//using UnityEngine;
//using UnityEngine.Events;
//using Kryz.CharacterStats;
//using System;

//public class HeroStateMachine : MonoBehaviour
//{
//    private BattleStateMachine BSM;
//    //public BaseHero character;
//    public Character character;
//    public UnitUI ui;

//    public enum TurnState
//    {
//        PROCESSING,
//        ADDTOLIST,
//        WAITING,
//        SELECTING,
//        ACTION,
//        DEAD
//    }

//    public TurnState currentState;

//    public GameObject EnemyToAttack;

//    public List<StatusList> StatusList = new List<StatusList>();
//    private List<GameObject> sortBySpeed = new List<GameObject>();
//    private List<GameObject> sortByHP = new List<GameObject>();

//    private bool actionStarted = false;
//    public bool counterAttack = false;

//    private Vector3 startPosition;
//    private float animSpeed = 15f;
//    //dead
//    private bool alive = true;

//    private int critHits;

//    //testing
//    public bool dodgedAtt = false;
    

//    private bool attackNext = false;
//    public bool canUseMelee = true;
//    public bool canUseMagic = true;
//    public bool canAct = true;
//    public bool canFlee = true;
//    //needed for melee / magic animations / character movements
//    public bool isMelee;
//    private float hitChance;

//    private bool isCriticalH = false;

//    private void OnValidate()
//    {
//        ui = GetComponent<UnitUI>();
//        character = GetComponent<Character>();
//    }

//    void Start()
//    {
//        //Set player rage
//        //ui.rageBar.SetRageBarSize(character.unit.Stats.curRage * 100 / character.unit.Stats.maxRage / 100);     
//        startPosition = transform.position;

//        ui.Selector.SetActive(false);
//        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
//        currentState = TurnState.PROCESSING;

//    }

//    void Update()
//    {
//        switch(currentState) 
//        {
//            case (TurnState.PROCESSING):
//            currentState = TurnState.ADDTOLIST;
//            break;

//            case (TurnState.ADDTOLIST):
//                BSM.HeroesToManage.Add(gameObject);
//                currentState = TurnState.WAITING;
//            break;

//            case (TurnState.WAITING):
//                //idle
//            break;

//            case (TurnState.ACTION):
//                StartCoroutine(TimeForAction());
//            break;

//            case (TurnState.DEAD):
//                if(!alive)
//                {
//                    return;
//                }
//                else
//                {
//                    //change tag of character
//                    gameObject.tag = "DeadHero";
//                    //not attackable by enemy
//                    BSM.HeroesInBattle.Remove(gameObject);
//                    //not able to manage the character anymore
//                    BSM.HeroesToManage.Remove(gameObject);
//                    //deactivate the selector
//                    ui.Selector.SetActive(false);
//                    //reset gui
//                    BSM.AttackPanel.SetActive(false);
//                    BSM.EnemySelectPanel.SetActive(false);
//                    //remove character from performlist
//                    if (BSM.HeroesInBattle.Count > 0)
//                    {
//                        for (int i = 0; i < BSM.PerformList.Count; i++)
//                        {
//                            if (i != 0)
//                            {
//                                if (BSM.PerformList[i].Attacker == gameObject)
//                                {
//                                    BSM.PerformList.Remove(BSM.PerformList[i]);
//                                }
//                                else if (BSM.PerformList[i].AttackersTarget == gameObject)
//                                {
//                                    //BSM.PerformList[i].AttackersTarget.Remove(gameObject);
//                                    BSM.PerformList[i].AttackersTarget = BSM.HeroesInBattle[UnityEngine.Random.Range(0, BSM.HeroesInBattle.Count)];
//                                }
//                            }
//                        }
//                    }
//                    //change appearance / play death animation
//                    //gameObject.GetComponent<SpriteRenderer>().color = new Color32(61, 61, 61, 255);
//                    //make not alive
//                    alive = false;
//                    //reset character input
//                    BSM.battleStates = BattleStateMachine.PerformAction.CHECKALIVE; 
//                }
//            break;
//        }
//    }

//    private IEnumerator TimeForAction()
//    {
//        if (actionStarted)
//        {
//            yield break;
//        }
//        actionStarted = true;

//        if (canAct)
//        {
//            //if (BSM.PerformList[0].choosenAttack.attackType == "Melee")
//            //{
//            //    isMelee = true;
//            //}
//            //else
//            //{
//            //    isMelee = false;
//            //}

//            ui.scream.SetActive(true);
//            ui.screamText.text = BSM.PerformList[0].choosenAttack.name.ToString() + "!";
//            yield return new WaitForSeconds(0.25f);

//            if (isMelee == true)
//            {
//                Vector3 targetPosition = new Vector3(EnemyToAttack.transform.position.x + 0.6f, EnemyToAttack.transform.position.y, EnemyToAttack.transform.position.z - 0.5f);
//                while (MoveToPosition(targetPosition))
//                {
//                    yield return null;
//                }
//            }
//            //wait a bit till animation of attack plays. Might wanna change later on based on animation.
//            yield return new WaitForSeconds(0.25f);
//            ui.scream.SetActive(false);

//            //test purpouses only
//            ui.animator.Play("Attack");
//            ui.audioSource.Play();
//            //if (BSM.PerformList[0].choosenAttack.attackName == "Capture")
//            //{
//            //    yield return new WaitForSeconds(1.5f);
//            //    if (UnityEngine.Random.Range(0, 100) < 100)
//            //    {
//            //        GameObject enemy = BSM.PerformList[0].AttackersTarget;
//            //        var enemyToCapture = unit.Stats.GetComponent<EnemyStateMachine>().enemy;
//            //        CapturedPets AddInfo = new CapturedPets();
//            //        Debug.Log(enemyToCapture.theName);
//            //        AddInfo.theName = enemyToCapture.theName;
//            //        //AddInfo.thePrefab = unit.Stats.GetComponent<EnemyStateMachine>().enemyPrefabX;
//            //        //AddInfo.strength = enemyToCapture.strength;
//            //        //AddInfo.intellect = enemyToCapture.intellect;
//            //        //AddInfo.dexterity = enemyToCapture.dexterity;
//            //        //AddInfo.agility = enemyToCapture.agility;
//            //        //AddInfo.stamina = enemyToCapture.stamina;
//            //        for (int i = 0; i < enemyToCapture.attacks.Count; i++)
//            //        {
//            //            AddInfo.attacks.Add(enemyToCapture.attacks[i]);
//            //        }
//            //        for (int i = 0; i < enemyToCapture.MagicAttacks.Count; i++)
//            //        {
//            //            AddInfo.MagicAttacks.Add(enemyToCapture.MagicAttacks[i]);
//            //        }
//            //        gameObject.GetComponent<SummonHandler>().SummonList.Add(AddInfo);
//            //        unit.Stats.GetComponent<EnemyStateMachine>().Captured();

//            //    }
//            //    else
//            //    {
//            //        Actions.OnDodge(BSM.PerformList[0].AttackersTarget.transform);
//            //    }

//            //}

//            //do damage
//            //if (BSM.PerformList[0].choosenAttack.isAttack == false && BSM.PerformList[0].choosenAttack.attackName != "Capture")
//            //{
//            //    ApplyBuffsDebuffs();
//            //}
//            //else if (BSM.PerformList[0].choosenAttack.attackName != "Capture")
//            //{
//            //    yield return new WaitForSeconds(0.7f);
//            //    DoDamage();
//            //    yield return new WaitForSeconds(0.25f);

//            //    //check for counterattack
//            //    if (BSM.PerformList[0].AttackersTarget.GetComponent<EnemyStateMachine>().counterAttack == true)
//            //    {
//            //        yield return new WaitForSeconds(1.0f);
//            //    }
//            //}

//            if (isMelee == true && BSM.EnemiesInBattle.Count > 0 && BSM.PerformList[0].AttackersTarget.GetComponent<UnitAttributes>().Stats.curHP <= 0)
//            {
//                StartCoroutine(AttackNextTarget());
//                while (attackNext == true)
//                {
//                    yield return null;
//                }
//            }

//            if (isMelee == false)
//            {
//                yield return new WaitForSeconds(1f);
//            }
//            //animate back to start position
//            if (isMelee && character.unit.Stats.curHP > 0)
//            {
//                Vector3 initialPosition = startPosition;
//                while (MoveToPosition(initialPosition))
//                {
//                    yield return null;
//                }
//            }
//        }
       
//        //remove this performer from the list in BSM
//        BSM.PerformList.RemoveAt(0);
//        //reset the battle state machine -> set to wait
//        if (BSM.battleStates != BattleStateMachine.PerformAction.WIN && BSM.battleStates != BattleStateMachine.PerformAction.LOSE)
//        {
//            BSM.battleStates = BattleStateMachine.PerformAction.START;
//            //reset this unit state
//            currentState = TurnState.PROCESSING;
//        }
//        else
//        {
//            currentState = TurnState.WAITING;
//        }
//        //end coroutine
//        actionStarted = false;
//    }

//    //Move sprite towards target / back to the initial position
//    private bool MoveToPosition(Vector3 target)
//    {
//        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
//    }

//    public void TakeDamage(float getDamageAmount, bool isCriticalE, float enemyHit, bool isDodgeable, bool isCounterAttack)
//    {
//        hitChance = (enemyHit / character.unit.Stats.curDodge) * 100; //(80 / 100) * 100 = 80%    (200 / 100) * 100 = 200
//        if (isDodgeable == false)
//        {
//            hitChance = 100;
//        }
//        if (UnityEngine.Random.Range(0, 101) <= hitChance) //in 20 outs out of 100 we dodge
//        {
//            dodgedAtt = false;
//            //AttackEffectPlay();
//            ui.animator.Play("Hurt");

//            character.unit.Stats.curHP -= getDamageAmount;
//            if (character.unit.Stats.curHP <= 0)
//            {
//                character.unit.Stats.curHP = 0;
//                currentState = TurnState.DEAD;
//                ui.animator.Play("Die");
//            }

//            //show popup damage
//            //Actions.OnDamageReceived(gameObject.transform, isCriticalE, getDamageAmount, false);
//            //health bar
//            ui.healthBar.SetSize(((character.unit.Stats.curHP * 100) / character.unit.Stats.baseHP) / 100);
   
//        }
//        else
//        {
//            dodgedAtt = true;
//            Actions.OnDodge(gameObject.transform);
//            AddRage(10);
//        }

//        if (!isCounterAttack && character.unit.Stats.curHP > 0 && isDodgeable == true && UnityEngine.Random.Range(0, 100) <= 100)
//        {
//            if (BSM.PerformList[0].Attacker.GetComponent<EnemyStateMachine>().secondAttackRunning == false)
//            {
//                StartCoroutine(CounterAttack());
//            }
//        }

//        AddRage(10);
//    }

//    //do damage
//    //public void DoDamage()
//    //{

//    //    float minMaxAtk = Mathf.Round(UnityEngine.Random.Range(character.unit.Stats.minATK, character.unit.Stats.maxATK));
//    //    float calc_damage = minMaxAtk + BSM.PerformList[0].choosenAttack.attackDamage;

//    //    //testing multiple targets
//    //    if (BSM.PerformList[0].choosenAttack.attackTargets > 1)
//    //    {
//    //        CalcDamageForEachTarget(calc_damage);
//    //        if (critHits >= 1)
//    //        {
//    //            AddRage(10);
//    //            critHits = 0;
//    //        }

//    //    }
//    //    else
//    //    {
//    //        if (UnityEngine.Random.Range(0, 100) <= character.unit.Stats.curCRIT)
//    //        {
//    //            isCriticalH = true;
//    //            calc_damage = Mathf.Round(calc_damage * character.unit.Stats.critDamage);
//    //            AddRage(10);
//    //        }
//    //        BSM.PerformList[0].AttackersTarget.GetComponent<EnemyStateMachine>().TakeDamage(calc_damage, isCriticalH, character.unit.Stats.curHit, isMelee, false);
//    //    }
        
//    //    //mana bar things
//    //    character.unit.Stats.curMP -= BSM.PerformList[0].choosenAttack.attackCost;
//    //    if (character.unit.Stats.curMP <= 0)
//    //    {
//    //        character.unit.Stats.curMP = 0;
//    //    }
//    //    ui.manaBar.SetSize(((character.unit.Stats.curMP * 100) / character.unit.Stats.baseMP) / 100);

//    //    AddRage(10);
//    //    isCriticalH = false;
//    //    //rage bar
        
//    //}

//    private IEnumerator CounterAttack()
//    {
//        if (counterAttack)
//        {
//            yield break;
//        }

//        counterAttack = true;

//        yield return new WaitForSeconds(0.5f);
//        ui.animator.Play("Attack");
//        ui.audioSource.Play();
//        yield return new WaitForSeconds(0.25f);
//        float minMaxAtk = Mathf.Round(UnityEngine.Random.Range(character.unit.Stats.minATK, character.unit.Stats.maxATK));

//        if (UnityEngine.Random.Range(0, 100) <= character.unit.Stats.curCRIT)
//        {
//            isCriticalH = true;
//            minMaxAtk = Mathf.Round(minMaxAtk * character.unit.Stats.critDamage);
//            AddRage(10);
//        }
//        BSM.PerformList[0].Attacker.GetComponent<EnemyStateMachine>().TakeDamage(minMaxAtk, isCriticalH, character.unit.Stats.curHit, true, counterAttack);
//        AddRage(10);
//        yield return new WaitForSeconds(0.5f);
//        counterAttack = false;
//    }

//    //public void ApplyBuffsDebuffs()
//    //{
//    //    //Buffs and debuffs can't crit
//    //    float minMaxAtk = Mathf.Round(UnityEngine.Random.Range(character.unit.Stats.minATK, character.unit.Stats.maxATK));
//    //    float calc_buff = minMaxAtk + BSM.PerformList[0].choosenAttack.attackDamage;
//    //    //actualy code
//    //    bool isHeal = true;

//    //    int count = BSM.PerformList[0].choosenAttack.attackTargets;
//    //    if(BSM.HeroesInBattle.Count < count)
//    //    {
//    //        count = BSM.HeroesInBattle.Count;
//    //    }

//    //    sortByHP = new List<GameObject>();
//    //    //add all heroes to the list 
//    //    foreach (GameObject en in BSM.HeroesInBattle)
//    //    {
//    //        sortByHP.Add(en);
//    //    }
//    //    //remove character that already is in the list by default
//    //    sortByHP.Remove(BSM.PerformList[0].AttackersTarget);
//    //    //sort heroes in the list by the HP, then reverse, so we heal allies with the lowest HP
//    //    sortByHP = sortByHP.OrderBy(x => x.GetComponent<Character>().unit.Stats.curHP).ToList();
//    //    sortByHP.Reverse();

//    //    BSM.PerformList[0].AttackersTarget.GetComponent<HeroStateMachine>().AcceptBuffsDebuffs(calc_buff, isHeal);
//    //    for (int i = 0; i < count; i++)
//    //    {
//    //        sortByHP[i].GetComponent<HeroStateMachine>().AcceptBuffsDebuffs(calc_buff, isHeal);
//    //    }

//    //    //mana bar things
//    //    character.unit.Stats.curMP -= BSM.PerformList[0].choosenAttack.attackCost;
//    //    if (character.unit.Stats.curMP <= 0)
//    //    {
//    //        character.unit.Stats.curMP = 0;
//    //    }
//    //    ui.manaBar.SetSize(((character.unit.Stats.curMP * 100) / character.unit.Stats.baseMP) / 100);

//    //    AddRage(10);
//    //}

//    public void AcceptBuffsDebuffs(float buff_value, bool isHeal)
//    {
//        if (isHeal)
//        {
//            //Actions.OnRestoreHP(gameObject.transform, buff_value);
//        }
//    }

//    //public void ReceiveStatusEffect()
//    //{
//    //    BaseBuff[] Effects = BSM.PerformList[0].choosenAttack.statusEffects;
//    //    StatusList statParams = new StatusList();
//    //    for (int i = 0; i < Effects.Length; i++)
//    //    {
//    //        statParams.buffObject = Effects[i];
//    //        statParams.duration = Effects[i].Duration;
//    //    }
//    //    StatusList.Add(statParams);
//    //}

//    //Add rage based on the events like attack, critical attack, damage taken, dodge, etc.
//    void AddRage(int rageAmount)
//    {
//        if (character != null)
//        {
//            character.curRage += rageAmount;
//            if (character.curRage >= character.maxRage)
//            {
//                character.curRage = character.maxRage;
//            }
//            ui.rageBar.SetSize(character.curRage * 100 / character.maxRage / 100);
//        }
//    }

//    //void CalcDamageForEachTarget(float calc_damage)
//    //{
//    //    if (BSM.EnemiesInBattle.Count >= BSM.PerformList[0].choosenAttack.attackTargets)
//    //    {
//    //        sortBySpeed = new List<GameObject>();
//    //        //add all enemies to the list 
//    //        foreach (GameObject en in BSM.EnemiesInBattle)
//    //        {
//    //            sortBySpeed.Add(en);
//    //        }
//    //        //remove enemy that already is in the list by default
//    //        sortBySpeed.Remove(BSM.PerformList[0].AttackersTarget);
//    //        //sort enemies in the list by the speed, then reverse, so we attack enemies with the highest speed
//    //        sortBySpeed = sortBySpeed.OrderBy(x => x.GetComponent<EnemyStateMachine>().unit.Stats.curSpeed).ToList();
//    //        sortBySpeed.Reverse();
//    //        //add speedy enemies to the list
//    //        for (int j = 0; j < (BSM.PerformList[0].choosenAttack.attackTargets - 1); j++)
//    //        {
//    //            if (UnityEngine.Random.Range(0, 100) <= character.unit.Stats.curCRIT)
//    //            {
//    //                isCriticalH = true;
//    //                calc_damage = Mathf.Round(calc_damage * character.unit.Stats.critDamage);
//    //                critHits++;
//    //            }
//    //            float opponentDef = BSM.PerformList[0].AttackersTarget.GetComponent<EnemyStateMachine>().unit.Stats.curDEF;
//    //            calc_damage -= opponentDef;
//    //            if (calc_damage < 0)
//    //            {
//    //                calc_damage = 0;
//    //            }
//    //            sortBySpeed[j].GetComponent<EnemyStateMachine>().TakeDamage(calc_damage, isCriticalH, character.unit.Stats.curHit, isMelee, false);
//    //        }
//    //        BSM.PerformList[0].AttackersTarget.GetComponent<EnemyStateMachine>().TakeDamage(calc_damage, isCriticalH, character.unit.Stats.curHit, isMelee, false);

//    //    }
//    //    else if (BSM.PerformList[0].choosenAttack.attackTargets > BSM.EnemiesInBattle.Count)
//    //    {
//    //        for (int j = 0; j < BSM.EnemiesInBattle.Count; j++)
//    //        {
//    //            if (UnityEngine.Random.Range(0, 100) <= character.unit.Stats.curCRIT)
//    //            {
//    //                isCriticalH = true;
//    //                calc_damage = Mathf.Round(calc_damage * character.unit.Stats.critDamage);
//    //                critHits++;
//    //            }
//    //            float opponentDef = BSM.PerformList[0].AttackersTarget.GetComponent<EnemyStateMachine>().unit.Stats.curDEF;
//    //            calc_damage -= opponentDef;
//    //            if (calc_damage < 0)
//    //            {
//    //                calc_damage = 0;
//    //            }
//    //            BSM.EnemiesInBattle[j].GetComponent<EnemyStateMachine>().TakeDamage(calc_damage, isCriticalH, character.unit.Stats.curHit, isMelee, false); ;
//    //        }
//    //    }
//    //}


//    //If we killed a target, chase next one. 
//    private IEnumerator AttackNextTarget()
//    {
//        if (attackNext)
//        {
//            yield break;
//        }

//        attackNext = true;
//        GameObject NewEnemyToAttack = BSM.EnemiesInBattle[UnityEngine.Random.Range(0, BSM.EnemiesInBattle.Count)];
//        BSM.PerformList[0].AttackersTarget = NewEnemyToAttack;
//        Vector3 nextEnemyPosition = new Vector3(NewEnemyToAttack.transform.position.x + 0.6f, NewEnemyToAttack.transform.position.y, NewEnemyToAttack.transform.position.z - 0.5f);
//        while (MoveToPosition(nextEnemyPosition))
//        {
//            yield return null;
//        }
//        yield return new WaitForSeconds(0.25f);

//        ui.animator.Play("Attack");
//        ui.audioSource.Play();
//        yield return new WaitForSeconds(0.7f);
//        //DoDamage();
//        yield return new WaitForSeconds(0.25f);
//        //check for counterattack
//        if (BSM.PerformList[0].AttackersTarget.GetComponent<EnemyStateMachine>().counterAttack == true)
//        {
//            yield return new WaitForSeconds(1.0f);
//        }
//        attackNext = false;
//    }

//    //private void AttackEffectPlay()
//    //{
//    //    Transform MagicVFX = BSM.GetComponent<BattleStateMachine>().PerformList[0].choosenAttack.attackVFX;
//    //    if (MagicVFX != null)
//    //    {
//    //        Instantiate(MagicVFX, transform.position, Quaternion.identity, transform);
//    //    }

//    //}

//    //TODO LIST OF SOME SORT
//    // 1) Hide mechanic
//    //    hidden heroes can't be hit or targeted unless enemy has vision passive skill
//    // 2) Exorcism / holy mechanic to counter undead
//    // 3) Heal over time (buff that heals ally(allies) at the end or in the beginning of the turn)
//    // 4) Mana restoration skill
//    // 5) Poison over time. Same as heal over time, but drains %% HP / %% MP at the end of the turn.
//    // 6) Ressurect skill. Similar to what RiseUndead method in Enemy state machine does. Cleric / healer skill.
//    //

//}
