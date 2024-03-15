using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SummonStateMachine : MonoBehaviour
{
    private BattleStateMachine BSM;
    public BaseSummon Summon;
    public UnitUI ui;

    public enum TurnState
    {
        PROCESSING,
        ADDTOLIST,
        WAITING,
        SELECTING,
        ACTION,
        DEAD
    }

    public TurnState currentState;

    //IeNumerator
    public GameObject EnemyToAttack;

    public List<StatusList> StatusList = new ();
    private List<GameObject> sortBySpeed = new ();
    private List<GameObject> sortByHP = new ();

    private bool actionStarted = false;
    public bool counterAttack = false;

    private Vector3 startPosition;
    private float animSpeed = 15f;
    //dead
    private bool alive = true;
    public bool canUseMelee = true;
    public bool canUseMagic = true;
    public bool canAct = true;
    public bool canFlee = true;

    private int critHits;

    //testing
    public bool dodgedAtt = false;

    private bool attackNext = false;
    //needed for melee / magic animations / character movements
    public bool isMelee;
    private float hitChance;

    private bool isCriticalH = false;

    private void OnValidate()
    {
        ui = GetComponent<UnitUI>();
    }

    void Start()
    {
        ui = GetComponent<UnitUI>();
        startPosition = transform.position;
        ui.Selector.SetActive(false);
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        currentState = TurnState.WAITING;
    }

    void Update()
    {
        switch (currentState)
        {
            case (TurnState.PROCESSING):
                currentState = TurnState.ADDTOLIST;
                break;

            case (TurnState.ADDTOLIST):
                BSM.HeroesToManage.Add(gameObject);
                currentState = TurnState.WAITING;
                break;

            case (TurnState.WAITING):
                //idle
                break;

            case (TurnState.ACTION):
                StartCoroutine(TimeForAction());
                break;

            case (TurnState.DEAD):
                if (!alive)
                {
                    return;
                }
                else
                {
                    //change tag of character
                    gameObject.tag = "DeadSummon";
                    //not attackable by enemy
                    BSM.HeroesInBattle.Remove(gameObject);
                    //not able to manage the character anymore
                    BSM.HeroesToManage.Remove(gameObject);
                    //deactivate the selector
                    ui.Selector.SetActive(false);
                    //reset gui
                    //I don't think that this has any use already because of changed turn flow system, check this later on.
                    BSM.AttackPanel.SetActive(false);
                    BSM.EnemySelectPanel.SetActive(false);
                    //remove character from performlist
                    if (BSM.HeroesInBattle.Count > 0)
                    {
                        for (int i = 0; i < BSM.PerformList.Count; i++)
                        {
                            if (i != 0)
                            {
                                if (BSM.PerformList[i].Attacker == gameObject)
                                {
                                    BSM.PerformList.Remove(BSM.PerformList[i]);
                                }
                                else if (BSM.PerformList[i].AttackersTarget == gameObject)
                                {
                                    //BSM.PerformList[i].AttackersTarget.Remove(gameObject);
                                    BSM.PerformList[i].AttackersTarget = BSM.HeroesInBattle[Random.Range(0, BSM.HeroesInBattle.Count)];
                                }
                            }
                        }
                    }
                    //change appearance / play death animation
                    //gameObject.GetComponent<SpriteRenderer>().color = new Color32(61, 61, 61, 255);
                    //make not alive
                    alive = false;
                    //reset character input
                    BSM.battleStates = BattleStateMachine.PerformAction.CHECKALIVE;
                }
                break;
        }
    }

    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        //if (BSM.PerformList[0].choosenAttack.attackType == "Melee")
        //{
        //    isMelee = true;
        //}
        //else
        //{
        //    isMelee = false;
        //}

        ui.scream.SetActive(true);
        ui.screamText.text = BSM.PerformList[0].choosenAttack.name.ToString() + "!";
        yield return new WaitForSeconds(0.25f);

        if (isMelee == true)
        {
            Vector3 enemyPosition = new Vector3(EnemyToAttack.transform.position.x + 0.6f, EnemyToAttack.transform.position.y, EnemyToAttack.transform.position.z - 0.5f);
            while (MoveTowardsEnemy(enemyPosition))
            {
                yield return null;
            }
        }
        //wait a bit till animation of attack plays. Might wanna change later on based on animation.
        yield return new WaitForSeconds(0.25f);
        ui.scream.SetActive(false);

        //test purpouses only
        ui.animator.Play("Attack");
        ui.audioSource.Play();

        //do damage
        //if (BSM.PerformList[0].choosenAttack.isAttack == false)
        //{
        //    ApplyBuffsDebuffs();
        //}

        yield return new WaitForSeconds(0.7f);
        //DoDamage();
        yield return new WaitForSeconds(0.25f);

            //check for counterattack
        if (BSM.PerformList[0].AttackersTarget.GetComponent<EnemyStateMachine>().counterAttack == true)
        {
            yield return new WaitForSeconds(1.0f);
        }

        if (isMelee == true && BSM.EnemiesInBattle.Count > 0 && BSM.PerformList[0].AttackersTarget.GetComponent<UnitAttributes>().Stats.curHP <= 0)
        {
            StartCoroutine(AttackNextTarget());
            while (attackNext == true)
            {
                yield return null;
            }
        }

        if (isMelee == false)
        {
            yield return new WaitForSeconds(1f);
        }
        //animate back to start position
        if (isMelee && Summon.curHP > 0)
        {
            Vector3 firstPosition = startPosition;
            while (MoveTowardsStart(firstPosition))
            {
                yield return null;
            }
        }
        //remove this performer from the list in BSM
        BSM.PerformList.RemoveAt(0);
        //reset the battle state machine -> set to wait
        if (BSM.battleStates != BattleStateMachine.PerformAction.WIN && BSM.battleStates != BattleStateMachine.PerformAction.LOSE)
        {
            BSM.battleStates = BattleStateMachine.PerformAction.START;
            //reset this enemy state
            currentState = TurnState.PROCESSING;
        }
        else
        {
            currentState = TurnState.WAITING;
        }
        //end coroutine
        actionStarted = false;
    }

    //Move sprite towards target
    private bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    //return the sprite towards starting position on battlefield
    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    public void TakeDamage(float getDamageAmount, bool isCriticalE, float enemyHit, bool isDodgeable, bool isCounterAttack)
    {
        hitChance = (enemyHit / Summon.curDodge) * 100; //(80 / 100) * 100 = 80%    (200 / 100) * 100 = 200
        if (isDodgeable == false)
        {
            hitChance = 100;
        }
        if (Random.Range(0, 101) <= hitChance) //in 20 outs out of 100 we dodge
        {
            dodgedAtt = false;
            //AttackEffectPlay();
            ui.animator.Play("Hurt");
            Summon.curHP -= getDamageAmount;
            //Actions.OnDamageReceived(gameObject.transform, isCriticalE, getDamageAmount, false);
            if (Summon.curHP <= 0)
            {
                Summon.curHP = 0;
                ui.animator.Play("Die");
                currentState = TurnState.DEAD;  
            }

            //health bar
            ui.healthBar.SetSize(((Summon.curHP * 100) / Summon.baseHP) / 100);
        }
        else
        {
            dodgedAtt = true;
            Actions.OnDodge(gameObject.transform);
        }

        if (!isCounterAttack && Summon.curHP > 0 && isDodgeable == true && Random.Range(0, 100) <= 100)
        {
            if (BSM.PerformList[0].Attacker.GetComponent<EnemyStateMachine>().secondAttackRunning == false)
            {
                StartCoroutine(CounterAttack());
            }
        }
    }

    //do damage
    //public void DoDamage()
    //{
    //    float minMaxAtk = Mathf.Round(Random.Range(Summon.minATK, Summon.maxATK));
    //    float calc_damage = minMaxAtk + BSM.PerformList[0].choosenAttack.attackDamage;

    //    //testing multiple targets
    //    if (BSM.PerformList[0].choosenAttack.attackTargets > 1)
    //    {
    //        CalcDamageForEachTarget(calc_damage);
    //        if (critHits >= 1)
    //        {
    //            critHits = 0;
    //        }
    //    }
    //    else
    //    {
    //        if (Random.Range(0, 100) <= Summon.curCRIT)
    //        {
    //            isCriticalH = true;
    //            calc_damage = Mathf.Round(calc_damage * Summon.critDamage);
    //        }
    //        BSM.PerformList[0].AttackersTarget.GetComponent<EnemyStateMachine>().TakeDamage(calc_damage, isCriticalH, Summon.curHit, isMelee, false);
    //    }

    //    //mana bar things
    //    Summon.curMP -= BSM.PerformList[0].choosenAttack.attackCost;
    //    if (Summon.curMP <= 0)
    //    {
    //        Summon.curMP = 0;
    //    }
    //    ui.manaBar.SetSize(((Summon.curMP * 100) / Summon.baseMP) / 100);

    //    isCriticalH = false;

    //}

    private IEnumerator CounterAttack()
    {
        if (counterAttack)
        {
            yield break;
        }

        counterAttack = true;

        yield return new WaitForSeconds(0.5f);
        ui.animator.Play("Attack");
        ui.audioSource.Play();
        yield return new WaitForSeconds(0.25f);
        float minMaxAtk = Mathf.Round(Random.Range(Summon.minATK, Summon.maxATK));

        if (Random.Range(0, 100) <= Summon.curCRIT)
        {
            isCriticalH = true;
            minMaxAtk = Mathf.Round(minMaxAtk * Summon.critDamage);
        }
        BSM.PerformList[0].Attacker.GetComponent<EnemyStateMachine>().TakeDamage(minMaxAtk, isCriticalH, Summon.curHit, true, counterAttack);
        yield return new WaitForSeconds(0.5f);
        counterAttack = false;
    }

    //public void ApplyBuffsDebuffs()
    //{
    //    //Buffs and debuffs can't crit
    //    float minMaxAtk = Mathf.Round(Random.Range(Summon.minATK, Summon.maxATK));
    //    float calc_buff = minMaxAtk + BSM.PerformList[0].choosenAttack.attackDamage;
    //    //actualy code
    //    bool isHeal = true;

    //    int count = BSM.PerformList[0].choosenAttack.attackTargets;
    //    if (BSM.HeroesInBattle.Count < count)
    //    {
    //        count = BSM.HeroesInBattle.Count;
    //    }

    //    sortByHP = new List<GameObject>();
    //    //add all heroes to the list 
    //    foreach (GameObject en in BSM.HeroesInBattle)
    //    {
    //        sortByHP.Add(en);
    //    }
    //    //remove character that already is in the list by default
    //    sortByHP.Remove(BSM.PerformList[0].AttackersTarget);
    //    //sort heroes in the list by the HP, then reverse, so we heal allies with the lowest HP
    //    sortByHP = sortByHP.OrderBy(x => x.GetComponent<BaseSummon>().curHP).ToList();
    //    sortByHP.Reverse();

    //    BSM.PerformList[0].AttackersTarget.GetComponent<HeroStateMachine>().AcceptBuffsDebuffs(calc_buff, isHeal);
    //    for (int i = 0; i < count; i++)
    //    {
    //        sortByHP[i].GetComponent<HeroStateMachine>().AcceptBuffsDebuffs(calc_buff, isHeal);
    //    }

    //    //mana bar things
    //    Summon.curMP -= BSM.PerformList[0].choosenAttack.attackCost;
    //    if (Summon.curMP <= 0)
    //    {
    //        Summon.curMP = 0;
    //    }
    //    ui.manaBar.SetSize(((Summon.curMP * 100) / Summon.baseMP) / 100);
    //}

    public void AcceptBuffsDebuffs(float buff_value, bool isHeal)
    {
        if (isHeal)
        {
            //Actions.OnRestoreHP(gameObject.transform, buff_value);
        }
    }

    //public void ReceiveStatusEffect()
    //{
    //    BaseBuff[] Effects = BSM.PerformList[0].choosenAttack.statusEffects;
    //    StatusList statParams = new StatusList();
    //    for (int i = 0; i < Effects.Length; i++)
    //    {
    //        statParams.buffObject = Effects[i];
    //        statParams.duration = Effects[i].Duration;
    //    }
    //    StatusList.Add(statParams);
    //}

    //void CalcDamageForEachTarget(float calc_damage)
    //{
    //    if (BSM.EnemiesInBattle.Count >= BSM.PerformList[0].choosenAttack.attackTargets)
    //    {
    //        sortBySpeed = new List<GameObject>();
    //        //add all enemies to the list 
    //        foreach (GameObject en in BSM.EnemiesInBattle)
    //        {
    //            sortBySpeed.Add(en);
    //        }
    //        //remove enemy that already is in the list by default
    //        sortBySpeed.Remove(BSM.PerformList[0].AttackersTarget);
    //        //sort enemies in the list by the speed, then reverse, so we attack enemies with the highest speed
    //        sortBySpeed = sortBySpeed.OrderBy(x => x.GetComponent<EnemyStateMachine>().unit.Stats.curSpeed).ToList();
    //        sortBySpeed.Reverse();
    //        //add speedy enemies to the list
    //        for (int j = 0; j < (BSM.PerformList[0].choosenAttack.attackTargets - 1); j++)
    //        {
    //            if (Random.Range(0, 100) <= Summon.curCRIT)
    //            {
    //                isCriticalH = true;
    //                calc_damage = Mathf.Round(calc_damage * Summon.critDamage);
    //                critHits++;
    //            }
    //            float opponentDef = BSM.PerformList[0].AttackersTarget.GetComponent<EnemyStateMachine>().unit.Stats.curDEF;
    //            calc_damage -= opponentDef;
    //            if (calc_damage < 0)
    //            {
    //                calc_damage = 0;
    //            }
    //            sortBySpeed[j].GetComponent<EnemyStateMachine>().TakeDamage(calc_damage, isCriticalH, Summon.curHit, isMelee, false);
    //        }
    //        BSM.PerformList[0].AttackersTarget.GetComponent<EnemyStateMachine>().TakeDamage(calc_damage, isCriticalH, Summon.curHit, isMelee, false);

    //    }
    //    else if (BSM.PerformList[0].choosenAttack.attackTargets > BSM.EnemiesInBattle.Count)
    //    {
    //        for (int j = 0; j < BSM.EnemiesInBattle.Count; j++)
    //        {
    //            if (Random.Range(0, 100) <= Summon.curCRIT)
    //            {
    //                isCriticalH = true;
    //                calc_damage = Mathf.Round(calc_damage * Summon.critDamage);
    //                critHits++;
    //            }
    //            float opponentDef = BSM.PerformList[0].AttackersTarget.GetComponent<EnemyStateMachine>().unit.Stats.curDEF;
    //            calc_damage -= opponentDef;
    //            if (calc_damage < 0)
    //            {
    //                calc_damage = 0;
    //            }
    //            BSM.EnemiesInBattle[j].GetComponent<EnemyStateMachine>().TakeDamage(calc_damage, isCriticalH, Summon.curHit, isMelee, false); ;
    //        }
    //    }
    //}


    //If we killed a target, chase next one. 
    private IEnumerator AttackNextTarget()
    {
        if (attackNext)
        {
            yield break;
        }

        attackNext = true;
        GameObject NewEnemyToAttack = BSM.EnemiesInBattle[Random.Range(0, BSM.EnemiesInBattle.Count)];
        BSM.PerformList[0].AttackersTarget = NewEnemyToAttack;
        Vector3 newEnemyPosition = new Vector3(NewEnemyToAttack.transform.position.x + 0.6f, NewEnemyToAttack.transform.position.y, NewEnemyToAttack.transform.position.z - 0.5f);
        while (MoveTowardsEnemy(newEnemyPosition))
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.25f);

        ui.animator.Play("Attack");
        ui.audioSource.Play();
        yield return new WaitForSeconds(0.7f);
        //DoDamage();
        yield return new WaitForSeconds(0.25f);
        //check for counterattack
        if (BSM.PerformList[0].AttackersTarget.GetComponent<EnemyStateMachine>().counterAttack == true)
        {
            yield return new WaitForSeconds(1.0f);
        }
        attackNext = false;
    }

    //private void AttackEffectPlay()
    //{
    //    Transform MagicVFX = BSM.GetComponent<BattleStateMachine>().PerformList[0].choosenAttack.attackVFX;
    //    if (MagicVFX != null)
    //    {
    //        Instantiate(MagicVFX, transform.position, Quaternion.identity, transform);
    //    }
    //}
}
