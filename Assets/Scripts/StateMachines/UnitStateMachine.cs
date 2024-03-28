using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BattleStateMachine;
using System.Linq;
using TMPro;

public class UnitStateMachine : MonoBehaviour
{
    public UnitAttributes unit;
    public UnitUI ui;
    public BattleStateMachine BSM;
    public Abilities abilities;
    public enum TurnState
    {
        PROCESSING,
        CHOICE,
        WAITING,
        ACTION,
        DEAD
    }

    public TurnState currentState;
    public AllowedActions AllowedActions;

    private Vector3 startposition;
    //timeforaction
    private bool actionStarted = false;
    //private bool takingDamage = false;
    public GameObject ChosenAttackTarget;
    private float animSpeed = 15f;
    private bool isMelee;
    public bool secondAttackRunning = false;
    public bool counterAttack = false;
    private float hitChance;

    //For testing purpouses
    public bool doubleHit;
    //private bool attackTwice = false;
    public int killStreak = 0;

    public bool dodgedAtt = false;

    //alive
    private bool alive = true;
    

    private void OnValidate()
    {
        ui = GetComponent<UnitUI>();
        unit = GetComponent<UnitAttributes>();
        abilities = GetComponent<Abilities>();
    }

    private void Awake()
    {
        ui = GetComponent<UnitUI>();
        unit = GetComponent<UnitAttributes>();
        abilities = GetComponent<Abilities>();
        doubleHit = true;
    }

    private void OnEnable()
    {
        //Actions.OnBattleStart += ExecutePassiveSkills;
    }

    private void OnDisable()
    {
        //Actions.OnBattleStart -= ExecutePassiveSkills;
    }

    void Start()
    {
        currentState = TurnState.PROCESSING;
        ui.Selector.SetActive(false);
        startposition = transform.position;
        unit = GetComponent<UnitAttributes>();
        ui.audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case (TurnState.PROCESSING):
                if (BSM.battleStates == PerformAction.IDLE)
                {
                    currentState = TurnState.CHOICE;
                }
                break;

            case (TurnState.CHOICE):
                if (gameObject.CompareTag("Enemy"))
                {
                    EnemyActionChoice();
                }
                else
                {
                    BSM.HeroesToManage.Add(gameObject);
                }
                currentState = TurnState.WAITING;
                break;

            case (TurnState.WAITING):
                //idle state
                break;

            case (TurnState.ACTION):
                StartCoroutine(TimeForActionTESTING2());
                break;

            case (TurnState.DEAD):
                if (!alive)
                {
                    return;
                }
                else
                {
                    //not attackable by heroes
                    //remove all inputs
                    BSM.BattleActions.RemoveFromLists(this);
                    //make not alive
                    alive = false;
                    //fade out and make not active
                    StartCoroutine(FadeOut());
                    //check alive
                    BSM.battleStates = PerformAction.CHECKALIVE;
                }
                break;
        }
    }

    private IEnumerator FadeOut()
    {
        if (!gameObject.CompareTag("Hero"))
        {
            if (unit.Stats.HP.CurValue <= 0)
            {
                yield return new WaitForSeconds(1.5f);
            }
            //if (BSM.EnemiesInBattle.Count > 0)
            gameObject.SetActive(false);
        }
    }

    void EnemyActionChoice()
    {
        HandleTurn enemyChoice = new HandleTurn();
        enemyChoice.AttackersName = unit.Stats.theName;
        enemyChoice.attackersSpeed = unit.Stats.Speed.CurValue;
        enemyChoice.Type = "Enemy"; //Why the hell do we need this at all? Check and remove later if not needed
        enemyChoice.Attacker = gameObject;
        BaseClass enemyStats = gameObject.GetComponent<UnitAttributes>().Stats;

        if(Random.Range(0, 100) <= GameManager.instance.skillUseChance)
        {
            //determine if enemy has mana and active skills to use
            if (enemyStats.MP.CurValue > 0 && abilities.MagicAttacks.Count > 0)
            {
                //we have magic attacks
                List<ActiveSkill> magicAttacks = new(abilities.MagicAttacks);
                //can we use them? check the manacost
                //remove all we can't use
                magicAttacks.RemoveAll(attack => attack.CostType != CostType.MP || attack.CostValue > enemyStats.MP.CurValue);
                if (magicAttacks.Count > 1)
                {
                    int num = Random.Range(0, magicAttacks.Count);
                    enemyChoice.choosenAttack = magicAttacks[num];
                }
                else if (magicAttacks.Count == 1)
                {
                    enemyChoice.choosenAttack = magicAttacks[0];
                    //Debug.Log("Enemy chose " + enemyChoice.choosenAttack.SkillName);
                }
                //else
                //{
                //    enemyChoice.choosenAttack = abilities.BasicActions[0];
                //    enemyChoice.AttackersTarget = BSM.HeroesInBattle[Random.Range(0, BSM.HeroesInBattle.Count)];
                //}

                //with choosen attack determine who shall we target with it based on it's target type
                if (enemyChoice.choosenAttack != null && enemyChoice.choosenAttack.TargetType == TargetType.Ally)
                {
                    //Debug.Log("Enemy choosenAttack.TargetType " + enemyChoice.choosenAttack.TargetType.ToString());
                    //Debug.Log("Enemy chose ally ");
                    enemyChoice.AttackersTarget = BSM.EnemiesInBattle[Random.Range(0, BSM.EnemiesInBattle.Count)];
                }
                else if (enemyChoice.choosenAttack != null && enemyChoice.choosenAttack.TargetType == TargetType.Foe)
                {
                    //Debug.Log("Enemy choosenAttack.TargetType " + enemyChoice.choosenAttack.TargetType.ToString());
                    //Debug.Log("Enemy chose hero ");
                    enemyChoice.AttackersTarget = BSM.HeroesInBattle[Random.Range(0, BSM.HeroesInBattle.Count)];
                }

            }
        }

        //if for some reason we still don't have any attack, select basic attack and random hero
        if (enemyChoice.choosenAttack == null)
        {
            enemyChoice.choosenAttack = abilities.BasicActions[0];
            enemyChoice.AttackersTarget = BSM.HeroesInBattle[Random.Range(0, BSM.HeroesInBattle.Count)];
        }

        BSM.CollectActions(enemyChoice);
    }

    private IEnumerator TimeForActionTESTING2()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        var choosenAttack = BSM.PerformList[0].choosenAttack;
        if (BSM.PerformList[0].choosenAttack && BSM.PerformList[0].choosenAttack.ID == "a0b4d18cb0e8c8f4791153f816e5336a")
        {
            //Debug.Log("if (BSM.PerformList[0].choosenAttack && BSM.PerformList[0].choosenAttack.ID == a0b4d18cb0e8c8f4791153f816e5336a triggered");
            var effect = BSM.PerformList[0].choosenAttack.ApplyStatusEffects[0].effect;
            BSM.BuffManager.ApplyStatus(this, effect, 1);
        }
        bool isCritical = false;

        if (AllowedActions.canAct && BSM.PerformList[0].choosenAttack != null)
        {
            if (choosenAttack.CheckCost(this) == true)
            {
                //Debug.Log("BSM.PerformList[0].AttackersTarget = " + BSM.PerformList[0].AttackersTarget.name);
                ActionSettings actionSettings = choosenAttack.CalculateSkill(this, BSM.PerformList[0].AttackersTarget.GetComponent<UnitStateMachine>());
                List<StatusEffectList> statusEffects = new(actionSettings.applyStatusEffects);
                List<UnitStateMachine> endTargets = new(actionSettings.endTargetList);
                if (actionSettings.skillType == SkillType.Melee && AllowedActions.canUseMelee)
                {
                    //check for the passive skills that have anything to do with melee attacks
                    //ExecutePassiveSkill(Trigger.DEAL_MELEE_DAMAGE, actionSettings);
                    //for example double attack passive would probably want to check if our chosen attack is basic attack and if so, apply strikeCount + 1;
                    //another example would be attack that has a chance of applying seal effect OR poison effect on hit, so we would go like: actionSettings.applyStatusEffects.Add(anotherEffect);
                    //we probably also want to add some additional variable to ActionSettings which would determine a list of things that happen if we actually hit the target
                    //for instance we would want to use that for vampirism passive ability that would return hp
                    var PassiveSkills = GetComponent<Abilities>().PassiveSkills;

                    for (int i = 0; i < PassiveSkills.Count; i++)
                    {
                        if (PassiveSkills[i].trigger == Trigger.DEAL_MELEE_DAMAGE && PassiveSkills[i].ApplyStatusEffects.Count > 0)
                        {
                            statusEffects = PassiveSkills[i].ApplyEffect(statusEffects);
                        }
                    }

                    ui.scream.SetActive(true);
                    ui.screamText.text = choosenAttack.SkillName.ToString() + "!";
                    choosenAttack.PayTheCost(this);
                    yield return new WaitForSeconds(0.25f);

                    Vector3 targetPosition;
                    if (gameObject.CompareTag("Enemy"))
                        targetPosition = new Vector3(ChosenAttackTarget.transform.position.x - 0.6f, ChosenAttackTarget.transform.position.y, ChosenAttackTarget.transform.position.z + 0.5f);
                    else
                        targetPosition = new Vector3(ChosenAttackTarget.transform.position.x + 0.6f, ChosenAttackTarget.transform.position.y, ChosenAttackTarget.transform.position.z - 0.5f);
                    //Debug.Log("ChosenAttackTarget name = " + ChosenAttackTarget.name);
                    while (MoveToPosition(targetPosition))
                    {
                        yield return null;
                    }
                    yield return new WaitForSeconds(0.25f);
                    ui.scream.SetActive(false);

                    //Debug.Log("actionSettings.endTargetList.Count = " + endTargets.Count.ToString());
                    for (int i = 0; i < endTargets.Count; i++)
                    {
                        //Debug.Log("actionSettings.strikeCount = " + actionSettings.strikeCount.ToString());
                        for (int j = 0; j < actionSettings.strikeCount; j++)
                        {
                            ui.animator.Play("Attack");
                            ui.audioSource.Play();
                            yield return new WaitForSeconds(0.7f);
                            choosenAttack.PlaySkillVFX(this, endTargets);

                            float TrueDamage;
                            if (actionSettings.canCrit && Random.Range(0, 100) <= unit.Stats.curCRIT)
                            {
                                isCritical = true;
                                TrueDamage = Mathf.Round(actionSettings.TrueDamage * unit.Stats.critDamage);
                            }
                            else
                            {
                                TrueDamage = actionSettings.TrueDamage;
                            }
                            //stop striking the target if it's already dead
                            if (endTargets[i].currentState != TurnState.DEAD)
                                endTargets[i].TakeDamage(this, TrueDamage, actionSettings.isDodgeable, isCritical, actionSettings.ignoreDefense, actionSettings.targetStat, actionSettings.isHeal, statusEffects);
                            //check for the passive skills that have anything to do with targets death whiel attacking it
                            //for example the chase mechanic passive skill, however I am not so sure if we could use that, since it needs to run in coroutine
                            isCritical = false;
                            yield return new WaitForSeconds(0.7f);
                        }
                    }

                    if (unit.Stats.HP.CurValue > 0)
                    {
                        while (MoveToPosition(startposition))
                        {
                            yield return null;
                        }
                    }
                }
                else if (!AllowedActions.canUseMelee)
                {
                    Debug.Log("Unit " + unit.Stats.displayName + " could not use their melee attack due to restriction!");
                }

                if (actionSettings.skillType == SkillType.Magic && AllowedActions.canUseMagic)
                {
                    ui.scream.SetActive(true);
                    ui.screamText.text = choosenAttack.SkillName.ToString() + "!";
                    choosenAttack.PayTheCost(this);
                    yield return new WaitForSeconds(0.7f);
                    ui.scream.SetActive(false);

                    ui.animator.Play("Attack");
                    ui.audioSource.Play();
                    yield return new WaitForSeconds(0.7f);
                    choosenAttack.PlaySkillVFX(this, endTargets);

                    for (int i = 0; i < endTargets.Count; i++)
                    {
                        for (int j = 0; j < actionSettings.strikeCount; j++)
                        {
                            //yield return new WaitForSeconds(0.7f);
                            float TrueDamage;
                            if (actionSettings.canCrit && Random.Range(0, 100) <= unit.Stats.curCRIT)
                            {
                                isCritical = true;
                                TrueDamage = Mathf.Round(actionSettings.TrueDamage * unit.Stats.critDamage);
                            }
                            else
                            {
                                TrueDamage = actionSettings.TrueDamage;
                            }
                            //stop striking the target if it's already dead
                            if (endTargets[i].currentState != TurnState.DEAD)
                                endTargets[i].TakeDamage(this, TrueDamage, actionSettings.isDodgeable, isCritical, actionSettings.ignoreDefense, actionSettings.targetStat, actionSettings.isHeal, statusEffects);
                            isCritical = false;
                        }
                    }
                    yield return new WaitForSeconds(0.7f);
                }
                else if (!AllowedActions.canUseMagic)
                {
                    Debug.Log("Unit " + unit.Stats.displayName + " could not use their magic attack due to restriction!");
                }

            }

        }
        else if (AllowedActions.canAct && BSM.PerformList[0].choosenAction)
        {
            yield return new WaitForSeconds(1f);
            BSM.PerformList[0].choosenAction.Act(this, null, BSM.PerformList[0].index);
            yield return new WaitForSeconds(0.7f);
        }
        else
        {
            Debug.Log("Unit " + unit.Stats.displayName + " could not act due to restriction!");
        }

        //remove this performer from the list in BSM
        //Debug.Log("BSM.PerformList [0] name = " + BSM.PerformList[0].AttackersName);
        BSM.PerformList.RemoveAt(0);

        //end coroutine
        //reset the battle state machine -> set to wait
        //reset this enemy state
        if (gameObject.CompareTag("Enemy"))
        {
            BSM.battleStates = PerformAction.START;
            if (unit.Stats.HP.CurValue > 0 && currentState != TurnState.DEAD)
            {
                //reset this unit state
                currentState = TurnState.PROCESSING;
            }
        }
        else
        {
            if (BSM.battleStates != PerformAction.WIN && BSM.battleStates != PerformAction.LOSE)
            {
                BSM.battleStates = PerformAction.START;
                //reset this unit state
                currentState = TurnState.PROCESSING;
            }
            else
            {
                currentState = TurnState.WAITING;
            }
        }

        actionStarted = false;
    }

    //Move sprite towards target
    private bool MoveToPosition(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    public void TakeDamage(UnitStateMachine attacker, float incomingTrueDamage, bool isDodgeable, bool isCritical, bool ignoresDef, AffectedStat affectedStat, bool isHeal, List<StatusEffectList> statusEffectLists)
    {
        float damageAmount = 0;
        //Calculate if the attack hits
        if (!isHeal)
        {
            hitChance = (attacker.unit.Stats.Hit.CurValue / unit.Stats.Dodge.CurValue) * 100; //(80 / 100) * 100 = 80%    (200 / 100) * 100 = 200
            if (isDodgeable == false)
            {
                hitChance = 100;
            }
            if (Random.Range(0, 100) <= hitChance)
            {
                if (statusEffectLists.Count > 0)
                {
                    Actions.OnRequestStatusApply(this, statusEffectLists);
                }
                if (!ignoresDef)
                {
                    damageAmount = incomingTrueDamage - unit.Stats.DEF.CurValue;
                    if (damageAmount < 0)
                    {
                        damageAmount = 0;
                    }
                }
                if (affectedStat == AffectedStat.HP)
                {
                    ui.animator.Play("Hurt");
                    unit.Stats.HP.CurValue -= damageAmount;
                    //Actions.OnBarChange(this, AffectedStat.HP);
                    if (unit.Stats.HP.CurValue <= 0)
                    {
                        unit.Stats.HP.CurValue = 0;
                        Actions.OnZeroHealth(this);
                        Actions.OnKill(attacker);
                    }
                    ui.healthBar.SetSize(unit.Stats.HP.CurValue / unit.Stats.HP.MaxValue);
                }

                if (affectedStat == AffectedStat.MP)
                {
                    ui.animator.Play("Hurt");
                    unit.Stats.MP.CurValue -= damageAmount;
                    //Actions.OnBarChange(this, AffectedStat.MP);
                    if (unit.Stats.MP.CurValue <= 0)
                    {
                        unit.Stats.MP.CurValue = 0;
                    }
                    ui.manaBar.SetSize(unit.Stats.MP.CurValue / unit.Stats.MP.MaxValue);
                }

                //show popup damage
                Actions.OnDamageReceived(transform, isCritical, damageAmount, false, affectedStat);

                //
                var PassiveSkills = GetComponent<Abilities>().PassiveSkills;

                for (int i = 0; i < PassiveSkills.Count; i++)
                {
                    if (PassiveSkills[i].trigger == Trigger.TAKE_MAGIC_DAMAGE)
                    {
                        //PassiveSkills[i].Activate(this);
                    }
                }
            }
            else
            {
                Actions.OnDodge(gameObject.transform);
                dodgedAtt = true;
            }
        }

        else //if it's healing
        {
            if (statusEffectLists.Count > 0)
            {
                Actions.OnRequestStatusApply(this, statusEffectLists);
            }
            damageAmount = incomingTrueDamage;
            Actions.OnRestore(transform, damageAmount, affectedStat);
        }


        //if (isDodgeable == true && Random.Range(0, 100) <= 100 && !isCounterAttack && unit.Stats.HP.CurValue > 0)
        //{
        //    StartCoroutine(CounterAttack());
        //}
    }

    public void Captured()
    {
        currentState = TurnState.DEAD;
        gameObject.SetActive(false);
    }

    //public IEnumerator TakeTheDamage(UnitStateMachine actor, float incomingTrueDamage, bool isDodgeable, bool isCritical, bool ignoresDef, AffectedStat affectedStat)
    //{
    //    if (takingDamage)
    //    {
    //        yield break;
    //    }
    //    takingDamage = true;
    //    float damageAmount = 0;
    //    //Calculate if the attack hits
    //    hitChance = (actor.unit.Stats.Hit.CurValue / unit.Stats.Dodge.CurValue) * 100; //(80 / 100) * 100 = 80%    (200 / 100) * 100 = 200
    //    if (isDodgeable == false)
    //    {
    //        hitChance = 100;
    //    }
    //    if (Random.Range(0, 100) <= hitChance)
    //    {
    //        if (!ignoresDef)
    //        {
    //            damageAmount = incomingTrueDamage - unit.Stats.DEF.CurValue;
    //            if (damageAmount < 0)
    //            {
    //                damageAmount = 0;
    //            }
    //        }
    //        if (affectedStat == AffectedStat.HP)
    //        {
    //            ui.animator.Play("Hurt");
    //            unit.Stats.HP.CurValue -= damageAmount;
    //            Actions.OnBarChange(this, AffectedStat.HP);
    //            if (unit.Stats.HP.CurValue <= 0)
    //            {
    //                unit.Stats.HP.CurValue = 0;
    //                Actions.OnZeroHealth(this);
    //            }
    //        }
    //        ui.healthBar.SetSize(unit.Stats.HP.CurValue * 100 / unit.Stats.HP.BaseValue / 100);
    //        //show popup damage
    //        Actions.OnDamageReceived(transform, isCritical, damageAmount, false);
    //    }
    //    else
    //    {
    //        Actions.OnDodge(gameObject.transform);
    //        dodgedAtt = true;
    //    }

    //    takingDamage = false;
    //    //if (isDodgeable == true && Random.Range(0, 100) <= 100 && !isCounterAttack && unit.Stats.HP.CurValue > 0)
    //    //{
    //    //    StartCoroutine(CounterAttack());
    //    //}
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
        float minMaxAtk = Mathf.Round(Random.Range(unit.Stats.ATK.MaxValue, unit.Stats.ATK.MaxValue * 0.25f));

        if (Random.Range(0, 100) <= unit.Stats.curCRIT)
        {
            //isCritical = true;
            minMaxAtk = Mathf.Round(minMaxAtk * unit.Stats.critDamage);
        }
        //BSM.PerformList[0].AttackersGameObject.GetComponent<UnitStateMachine>().TakeDamage(minMaxAtk, isCriticalE, unit.Stats.Hit.CurValue, true, counterAttack);
        yield return new WaitForSeconds(0.5f);
        counterAttack = false;
    }

    //Undead mechanic
    //Perk1: While enemy with undead dies, it will rise after X turns with X HP;
    //Drawback: Undead takes 2x damage from holy property / Exorcism passive skill and if killed by holy/exorcism, can't rise
    //Drawback / perk2: Can't be buffed or debuffed / healed neither by enemies or allies unless the skill level isn't maximal
    //If undead: after death remove from PerformList, but don't remove from enemies in Battle so it can be targeted by players
    //If is targeted by player, but hasn't risen at the start of the turn, then switch targets.
    //At this point adding the rise method
    //All those skill related methods with later be moved to corresponding places, at this point it all is for testing purposes.

    //TODO LIST OF SOME SORT
    // 1) Hide mechanic
    //    hidden heroes can't be hit or targeted unless enemy has vision passive skill
    // 2) Exorcism / holy mechanic to counter undead
    // 3) Heal over time (buff that heals ally(allies) at the end or in the beginning of the turn)
    // 4) Mana restoration skill
    // 5) Poison over time. Same as heal over time, but drains %% HP / %% MP at the end of the turn.
    // 6) Ressurect skill. Similar to what RiseUndead method in Enemy state machine does. Cleric / healer skill.
    //

    //public void UndeadRise()
    //{
    //    alive = true;
    //    gameObject.tag = "Enemy";
    //    ui.Selector.SetActive(true);
    //    EnemyActionChoice();
    //}



    //void ExecutePassiveSkills(BattleStateMachine BSM)
    //{
    //    var PassiveSkills = abilities.PassiveSkills;
    //    if (PassiveSkills.Count > 0)
    //    {
    //        for (int i = 0; i < PassiveSkills.Count; i++)
    //        {
    //            //PassiveSkills[i].Activate(this);
    //        }
    //    }
    //}

    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        if (AllowedActions.canAct && BSM.PerformList[0].choosenAttack)
        {
            if (BSM.PerformList[0].choosenAttack.SkillType == SkillType.Melee)
            {
                isMelee = true;
            }
            else
            {
                isMelee = false;
            }
            //Actions.SetBools or something to determine which passive actions and so on can unit use here.
            //Like double attack, and so on

            //attackTwice = false;
            //secondAttackRunning = false;

            ui.scream.SetActive(true);
            ui.screamText.text = BSM.PerformList[0].choosenAttack.SkillName.ToString() + "!";
            yield return new WaitForSeconds(0.25f);

            if (isMelee == true)
            {
                Vector3 targetPosition = new();
                if (gameObject.CompareTag("Enemy"))
                    targetPosition = new Vector3(ChosenAttackTarget.transform.position.x - 0.6f, ChosenAttackTarget.transform.position.y, ChosenAttackTarget.transform.position.z + 0.5f);
                else
                    targetPosition = new Vector3(ChosenAttackTarget.transform.position.x + 0.6f, ChosenAttackTarget.transform.position.y, ChosenAttackTarget.transform.position.z - 0.5f);
                while (MoveToPosition(targetPosition))
                {
                    yield return null;
                }
            }
            //wait a bit till animation of attack plays. Might wanna change later on based on animation.
            yield return new WaitForSeconds(0.25f);
            ui.scream.SetActive(false);

            ui.animator.Play("Attack");
            ui.audioSource.Play();

            yield return new WaitForSeconds(0.7f);
            //DoDamage();
            BSM.PerformList[0].choosenAttack.Activate(this, ChosenAttackTarget.GetComponent<UnitStateMachine>());
            yield return new WaitForSeconds(0.25f);
            //implement all the passive skill things that could be done after damage
            //like double attack or chase
            //also make priority check of those passives
            //
            //  PassivePriority = new List<PassiveSkill>();
            // for (i = 0; i < PassiveSkills.Count; i++){
            //      if (PassiveSkill.trigger.point == afterDamage){
            //          PassivePriority.Add(PassiveSkill[i]);
            //      }
            //  }
            //  PassivePriority.Sort(priority.of.action => acending from 0 to 99);
            //  
            //
            //

            //check for counterattack
            //if (BSM.PerformList[0].AttackersTarget.GetComponent<UnitStateMachine>().counterAttack == true)
            //{
            //    yield return new WaitForSeconds(1.0f);
            //}

            //if (isMelee == false)
            //{
            //    yield return new WaitForSeconds(1f);
            //}

            //Double Hit mechanic testing
            //If target died from first attack, do not attack for the second time
            //If we intend to attack, it has 35% chance to do so
            //if (Random.Range(0, 100) < GameManager.instance.doubleAttackChance && unit.Stats.HP.CurValue > 0)
            //{
            //    attackTwice = true;
            //}

            //if (isMelee && doubleHit && ChosenAttackTarget.GetComponent<Character>().unit.Stats.HP.CurValue > 0 && attackTwice)
            //{
            //    if (ChosenAttackTarget.GetComponent<UnitStateMachine>().dodgedAtt == false)
            //    {
            //        secondAttackRunning = true;
            //        ui.animator.Play("Attack");
            //        ui.audioSource.Play();
            //        //DoDamage();
            //        yield return new WaitForSeconds(0.7f);
            //    }
            //}

            //testing kill streak mechanics
            //after killing one target the killer should choose next one and attack it and do it untill he can't kill the next target
            //if (ChosenAttackTarget.GetComponent<Character>().unit.Stats.HP.CurValue <= 0)
            //{
            //    killStreak++;
            //    Debug.Log("Kill Streak = " + killStreak);
            //}

            if (isMelee && unit.Stats.HP.CurValue > 0)
            {
                while (MoveToPosition(startposition))
                {
                    yield return null;
                }
            }
        }

        if (AllowedActions.canAct && BSM.PerformList[0].choosenAction)
        {
            yield return new WaitForSeconds(1f);
            BSM.PerformList[0].choosenAction.Act(this, null, BSM.PerformList[0].index);
            yield return new WaitForSeconds(0.7f);
        }

        //remove this performer from the list in BSM
        BSM.PerformList.RemoveAt(0);
        //reset the battle state machine -> set to wait

        //end coroutine

        //reset this enemy state
        if (gameObject.CompareTag("Enemy"))
        {
            BSM.battleStates = PerformAction.START;

            if (unit.Stats.HP.CurValue > 0 && currentState != TurnState.DEAD)
            {
                currentState = TurnState.PROCESSING;
            }
        }

        else
        {
            if (BSM.battleStates != PerformAction.WIN && BSM.battleStates != PerformAction.LOSE)
            {
                BSM.battleStates = PerformAction.START;
                //reset this unit state
                currentState = TurnState.PROCESSING;
            }
            else
            {
                currentState = TurnState.WAITING;
            }
        }

        actionStarted = false;
    }

    private IEnumerator TimeForActionTESTING()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        var choosenAttack = BSM.PerformList[0].choosenAttack;

        if (AllowedActions.canAct && BSM.PerformList[0].choosenAttack)
        {
            //Actions.SetBools or something to determine which passive actions and so on can unit use here.
            //Like double attack, and so on

            //attackTwice = false;
            //secondAttackRunning = false;

            ui.scream.SetActive(true);
            ui.screamText.text = choosenAttack.SkillName.ToString() + "!";
            yield return new WaitForSeconds(0.25f);

            if (choosenAttack.SkillType == SkillType.Melee)
            {
                Vector3 targetPosition;
                if (gameObject.CompareTag("Enemy"))
                    targetPosition = new Vector3(ChosenAttackTarget.transform.position.x - 0.6f, ChosenAttackTarget.transform.position.y, ChosenAttackTarget.transform.position.z + 0.5f);
                else
                    targetPosition = new Vector3(ChosenAttackTarget.transform.position.x + 0.6f, ChosenAttackTarget.transform.position.y, ChosenAttackTarget.transform.position.z - 0.5f);
                while (MoveToPosition(targetPosition))
                {
                    yield return null;
                }
            }
            //wait a bit till animation of attack plays. Might wanna change later on based on animation.
            yield return new WaitForSeconds(0.25f);
            ui.scream.SetActive(false);

            bool isCritical = false;

            var Tesst = choosenAttack.CalculateSkill(this, BSM.PerformList[0].AttackersTarget.GetComponent<UnitStateMachine>());

            for (int i = 0; i < Tesst.endTargetList.Count; i++)
            {
                for (int j = 0; j < Tesst.strikeCount; j++)
                {
                    if (choosenAttack.SkillType == SkillType.Melee)
                    {
                        ui.animator.Play("Attack");
                        ui.audioSource.Play();
                        yield return new WaitForSeconds(0.7f);
                    }
                    if (Tesst.canCrit && Random.Range(0, 100) <= unit.Stats.curCRIT)
                    {
                        isCritical = true;
                        Tesst.TrueDamage = Mathf.Round(Tesst.TrueDamage * unit.Stats.critDamage);
                    }
                    //stop striking the target if it's already dead
                    if (Tesst.endTargetList[i].currentState != TurnState.DEAD)
                        Tesst.endTargetList[i].TakeDamage(this, Tesst.TrueDamage, Tesst.isDodgeable, isCritical, Tesst.ignoreDefense, Tesst.targetStat, Tesst.isHeal, Tesst.applyStatusEffects);
                    if (choosenAttack.SkillType == SkillType.Melee)
                    {
                        yield return new WaitForSeconds(0.7f);
                    }
                    //if (targetList[0].dodgedAtt == false)
                    //{
                    //    float HealAmount = trueDamage * 30 / 100; //testing vampirism and restore HP. How much we should heal and how much %% from this.
                    //    Actions.OnRestoreHP(actor.transform, HealAmount);
                    //}
                }
            }
            if (choosenAttack.SkillType != SkillType.Melee)
            {
                ui.animator.Play("Attack");
                ui.audioSource.Play();
                yield return new WaitForSeconds(1.4f);
            }

            //DoDamage();
            //BSM.PerformList[0].choosenAttack.Activate(this, ChosenAttackTarget.GetComponent<UnitStateMachine>());
            //yield return new WaitForSeconds(0.25f);
            //implement all the passive skill things that could be done after damage
            //like double attack or chase
            //also make priority check of those passives
            //
            //  PassivePriority = new List<PassiveSkill>();
            // for (i = 0; i < PassiveSkills.Count; i++){
            //      if (PassiveSkill.trigger.point == afterDamage){
            //          PassivePriority.Add(PassiveSkill[i]);
            //      }
            //  }
            //  PassivePriority.Sort(priority.of.action => acending from 0 to 99);
            //  
            //
            //

            //check for counterattack
            //if (BSM.PerformList[0].AttackersTarget.GetComponent<UnitStateMachine>().counterAttack == true)
            //{
            //    yield return new WaitForSeconds(1.0f);
            //}

            //if (isMelee == false)
            //{
            //    yield return new WaitForSeconds(1f);
            //}

            //Double Hit mechanic testing
            //If target died from first attack, do not attack for the second time
            //If we intend to attack, it has 35% chance to do so
            //if (Random.Range(0, 100) < GameManager.instance.doubleAttackChance && unit.Stats.HP.CurValue > 0)
            //{
            //    attackTwice = true;
            //}

            //if (isMelee && doubleHit && ChosenAttackTarget.GetComponent<Character>().unit.Stats.HP.CurValue > 0 && attackTwice)
            //{
            //    if (ChosenAttackTarget.GetComponent<UnitStateMachine>().dodgedAtt == false)
            //    {
            //        secondAttackRunning = true;
            //        ui.animator.Play("Attack");
            //        ui.audioSource.Play();
            //        //DoDamage();
            //        yield return new WaitForSeconds(0.7f);
            //    }
            //}

            //testing kill streak mechanics
            //after killing one target the killer should choose next one and attack it and do it untill he can't kill the next target
            //if (ChosenAttackTarget.GetComponent<Character>().unit.Stats.HP.CurValue <= 0)
            //{
            //    killStreak++;
            //    Debug.Log("Kill Streak = " + killStreak);
            //}

            if (choosenAttack.SkillType == SkillType.Melee && unit.Stats.HP.CurValue > 0)
            {
                while (MoveToPosition(startposition))
                {
                    yield return null;
                }
            }
        }

        if (AllowedActions.canAct && BSM.PerformList[0].choosenAction)
        {
            yield return new WaitForSeconds(1f);
            BSM.PerformList[0].choosenAction.Act(this, null, BSM.PerformList[0].index);
            yield return new WaitForSeconds(0.7f);
        }

        //remove this performer from the list in BSM
        BSM.PerformList.RemoveAt(0);
        //reset the battle state machine -> set to wait

        //end coroutine

        //reset this enemy state
        if (gameObject.CompareTag("Enemy"))
        {
            BSM.battleStates = PerformAction.START;

            if (unit.Stats.HP.CurValue > 0 && currentState != TurnState.DEAD)
            {
                currentState = TurnState.PROCESSING;
            }
        }

        else
        {
            if (BSM.battleStates != PerformAction.WIN && BSM.battleStates != PerformAction.LOSE)
            {
                BSM.battleStates = PerformAction.START;
                //reset this unit state
                currentState = TurnState.PROCESSING;
            }
            else
            {
                currentState = TurnState.WAITING;
            }
        }

        actionStarted = false;
    }

    //public void ExecutePassiveSkill(Trigger trigger, ActionSettings actionSettings)
    //{
    //    var PassiveSkills = GetComponent<Abilities>().PassiveSkills;

    //    for (int i = 0; i < PassiveSkills.Count; i++)
    //    {
    //        if (PassiveSkills[i].trigger == trigger)
    //        {
    //            PassiveSkills[i].Activate(this, actionSettings);
    //        }
    //    }
    //}

}
