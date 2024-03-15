using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static BattleStateMachine;
using System.Linq;

public class EnemyStateMachine : MonoBehaviour
{
    //public BaseEnemy enemy;
    public UnitUI ui;
    public UnitLevel UnitLevel;
    public UnitAttributes unit;
    [System.Serializable]
    public class SkillSet
    {
        public ActiveSkill possibleSkill;
        [Range(0,100)]public int skillSpawnChance = 25;
    }

    public class PassiveSkillSet
    {
        public PassiveSkill posPassive;
        [Range(0, 100)] public int skillSpawnChance = 25;
    }

    public List<SkillSet> PossibleSkills = new List<SkillSet>();
    public List<PassiveSkillSet> PossiblePassives = new List<PassiveSkillSet>();

    private List<GameObject> sortBySpeed = new List<GameObject>();
    private List<GameObject> sortByHP = new List<GameObject>();

    //public List<ScriptableObject> Skills = new List<ScriptableObject>();

    private BattleStateMachine BSM;
    public enum TurnState
    {
        PROCESSING,
        CHOOSEACTION,
        WAITING,
        ACTION,
        DEAD
    }

    public TurnState currentState;

    private Vector3 startposition;
    //timeforaction
    private bool actionStarted = false;
    public GameObject HeroToAttack;
    private float animSpeed = 15f;
    private bool isMelee;
    public bool secondAttackRunning = false;
    public bool counterAttack = false;
    private float hitChance;
    private bool isCriticalE = false;
    
    //For testing purpouses
    public bool doubleHit;
    private bool attackTwice = false;
    private int killStreak = 0;
    private int enemyLevel;


    //alive
    private bool alive = true;
    public bool canUseMelee = true;
    public bool canUseMagic = true;
    public bool canAct = true;
    public bool canFlee = true;
    public int expAmount;

    private void OnValidate()
    {
        ui = GetComponent<UnitUI>();
        UnitLevel = GetComponent<UnitLevel>();
        unit.Stats.theName = gameObject.name; 
    }

    private void Awake()
    {
        int curRegions = GameManager.instance.curRegions;
        enemyLevel = GameManager.instance.Regions[curRegions].EnemyLevel;
        UnitLevel.level = new Level(enemyLevel, SetParams);
        SetParams();
        //PopulateSkilllist();
        doubleHit = true;
    }

    void Start()
    {
        //TMP_Text enemyName = unit.Stats.displayNameText;
        //enemyName.text = unit.Stats.theName.ToString();

        currentState = TurnState.PROCESSING;
        ui.Selector.SetActive (false);
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        startposition = transform.position;

        ui.animator = GetComponent<Animator>();
        ui.audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case (TurnState.PROCESSING):
                if(BSM.battleStates == PerformAction.IDLE)
                {
                    currentState = TurnState.CHOOSEACTION;
                }
                break;

            case (TurnState.CHOOSEACTION):
                //ChooseAction();
                currentState = TurnState.WAITING;
                break;

            case (TurnState.WAITING):
                //idle state
                break;

            case (TurnState.ACTION):
                StartCoroutine(TimeForAction());
                break;

            case (TurnState.DEAD):
                if(!alive)
                {
                    return;
                }
                else
                {
                    //change tag of enemy
                    gameObject.tag = "DeadEnemy";
                    //not attackable by heroes
                    BSM.EnemiesInBattle.Remove(gameObject);
                    //disable the selector
                    ui.Selector.SetActive (false);
                    ui.healthBar.gameObject.SetActive(false);
                    ui.manaBar.gameObject.SetActive(false);
                    //remove all inputs
                    if (BSM.EnemiesInBattle.Count > 0)
                    {
                        for (int i = 0; i < BSM.PerformList.Count; i++)
                        {
                            if (i != 0)
                            {
                                if (BSM.PerformList[i].Attacker == gameObject)
                                {
                                    BSM.PerformList.Remove(BSM.PerformList[i]);
                                }
                                //if someone has them as a target and it's single target atttack, we will replace it to someone random
                                else if (BSM.PerformList[i].AttackersTarget == gameObject) //Look in all the lists of attack targets.
                                {
                                    BSM.PerformList[i].AttackersTarget = BSM.EnemiesInBattle[Random.Range(0, BSM.EnemiesInBattle.Count)];
                                }
                            }
                        }

                    }
                    //change the color to gray / play death animation
                    //gameObject.GetComponent<SpriteRenderer>().color = new Color32(61, 61, 61, 255);
                    //make not alive
                    alive = false;
                    //reset enemy buttons
                    BSM.EnemyButtons();
                    //check alive
                    BSM.battleStates = PerformAction.CHECKALIVE;
                    //fade out and make not active
                    StartCoroutine(FadeOut());
                }
                break;
        }
    }

    private IEnumerator FadeOut()
    {
        //SpriteRenderer rend;
        //rend = GetComponent<SpriteRenderer>();
        //for (float f = 1f; f >= -0.02f; f -= 0.02f)
        //{
        //    Color c = rend.material.color;
        //    c.a = f;
        //    rend.material.color = c;
        //    yield return new WaitForSeconds(0.02f);
        //}
        if(unit.Stats.curHP <= 0)
        {
            yield return new WaitForSeconds(1.5f);
        }
        //if (BSM.EnemiesInBattle.Count > 0)
            gameObject.SetActive(false);
    }

    //void ChooseAction()
    //{
    //    HandleTurn myAttack = new HandleTurn();
    //    myAttack.AttackersName = unit.Stats.theName;
    //    myAttack.attackersSpeed = unit.Stats.curSpeed;
    //    myAttack.Type = "Enemy";
    //    myAttack.Attacker = gameObject;
    //    //Target choice: Randomly choose the target from list. Editable for later.
    //    myAttack.AttackersTarget = BSM.HeroesInBattle[Random.Range(0, BSM.HeroesInBattle.Count)];
        
    //    //determine which skills does enemy have:
    //    if (unit.Stats.MagicAttacks.Count > 0)
    //    {
    //        //we have magic attacks
    //        //can we use them? check the manacost
    //        for (int i = 0; i < unit.Stats.MagicAttacks.Count; i++)
    //        {
    //            if (unit.Stats.MagicAttacks[i].costType == CostType.MP && unit.Stats.curMP >= unit.Stats.MagicAttacks[i].costValue)
    //            {
    //                int num = Random.Range(0, unit.Stats.MagicAttacks.Count-1);
    //                myAttack.choosenAttack = unit.Stats.MagicAttacks[num];
    //            }
    //        }
    //        //if we have not enough mana for any of magic attacks
    //        if (myAttack.choosenAttack == null)
    //        {
    //            myAttack.choosenAttack = unit.Stats.BasicActions[0];
    //        }
    //    }
    //    else
    //    {
    //        //choose basic attack
    //        myAttack.choosenAttack = unit.Stats.BasicActions[0];
    //    }

    //    if (myAttack.choosenAttack.skillType == SkillType.Melee)
    //    {
    //        isMelee = true;
    //    }
    //    else
    //    {
    //        isMelee = false;
    //    }

    //    BSM.CollectActions(myAttack);
    //}


    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;
        attackTwice = false;
        secondAttackRunning = false;

        //animate the enemy near the character to attack

        if (canAct)
        {
            ui.scream.SetActive(true);
            ui.screamText.text = BSM.PerformList[0].choosenAttack.name.ToString() + "!";
            yield return new WaitForSeconds(0.25f);

            if (isMelee == true)
            {
                Vector3 heroPosition = new Vector3(HeroToAttack.transform.position.x - 0.6f, HeroToAttack.transform.position.y, HeroToAttack.transform.position.z + 0.5f);
                while (MoveTowardsEnemy(heroPosition))
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
            yield return new WaitForSeconds(0.25f);
            //check for counterattack
            if (BSM.PerformList[0].AttackersTarget.GetComponent<HeroStateMachine>().counterAttack == true)
            {
                yield return new WaitForSeconds(1.0f);
            }

            if (isMelee == false)
            {
                yield return new WaitForSeconds(1f);
            }

            //Double Hit mechanic testing
            //If target died from first attack, do not attack for the second time
            //If we intend to attack, it has 35% chance to do so
            if (Random.Range(0, 100) < GameManager.instance.doubleAttackChance && unit.Stats.curHP > 0)
            {
                attackTwice = true;
            }

            if (isMelee && doubleHit && HeroToAttack.GetComponent<Character>().unit.Stats.curHP > 0 && attackTwice)
            {
                if (HeroToAttack.GetComponent<HeroStateMachine>().dodgedAtt == false)
                {
                    secondAttackRunning = true;
                    ui.animator.Play("Attack");
                    ui.audioSource.Play();
                    //DoDamage();
                    yield return new WaitForSeconds(0.7f);
                }
            }

            //testing kill streak mechanics
            //after killing one target the killer should choose next one and attack it and do it untill he can't kill the next target
            if (HeroToAttack.GetComponent<Character>().unit.Stats.curHP <= 0)
            {
                killStreak++;
                Debug.Log("Kill Streak = " + killStreak);
            }

            if (isMelee && unit.Stats.curHP > 0)
            {
                //animate back to start position
                Vector3 firstPosition = startposition;
                while (MoveTowardsStart(firstPosition))
                {
                    yield return null;
                }
            }
        }
        
        //remove this performer from the list in BSM
        BSM.PerformList.RemoveAt(0);
        //reset the battle state machine -> set to wait
        BSM.battleStates = PerformAction.START;
        //end coroutine
        actionStarted = false;
        //reset this enemy state

        if(unit.Stats.curHP > 0 && currentState != TurnState.DEAD)
        {
            currentState = TurnState.PROCESSING;
        }
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

  //  void DoDamage()
  //  {
  //      //dodge / hit calculations for all attack types (magic perfect hit to be added later)
  //      //get enemy hit value get target dodge value
  //      //hit / dodge x 100 = chance to hit call it hitChance
  //      //if hit > dodge, just proceed if hit < dodge random range (1, 100) < hitChance =  
  //      //
  //      //do damage
  //      float minMaxAtk = Mathf.Round(Random.Range(unit.Stats.minATK, unit.Stats.maxATK));
  //      //float calc_damage = unit.Stats.curATK + BSM.PerformList[0].choosenAttack.attackDamage;
  //      float calc_damage = minMaxAtk + BSM.PerformList[0].choosenAttack.attackDamage;
  //      //critical strikes

		//if (BSM.PerformList [0].choosenAttack.attackTargets > 1) {
		//	CalcDamageForEachTarget (calc_damage);
		//} else {
		//	if (Random.Range (0, 100) <= unit.Stats.curCRIT) {
		//		isCriticalE = true;
		//		calc_damage = Mathf.Round (calc_damage * unit.Stats.critDamage);
		//	}

		//	//add damage formula later on
		//	float opponentDef = HeroToAttack.GetComponent<Character>().unit.Stats.curDEF;
		//	calc_damage -= opponentDef;
		//	if (calc_damage < 0) {
		//		calc_damage = 0;
		//	}

		//	HeroToAttack.GetComponent<HeroStateMachine>().TakeDamage (calc_damage, isCriticalE, unit.Stats.curHit, isMelee, false);
		//	if (HeroToAttack.GetComponent<HeroStateMachine>().dodgedAtt == false) {
		//		calc_damage = Mathf.Round( calc_damage * 30 / 100); //testing vampirism and restore HP. How much we should heal and how much %% from this.
  //              Actions.OnRestoreHP(transform, calc_damage);
		//	}
		//}

		//unit.Stats.curMP -= BSM.PerformList[0].choosenAttack.attackCost;
		//if (unit.Stats.curMP <= 0)
		//{
		//	unit.Stats.curMP = 0;
		//}
  //      ui.manaBar.SetSize(((unit.Stats.curMP * 100) / unit.Stats.baseMP) / 100);
    
  //      isCriticalE = false;
  //  }

    public void TakeDamage(float getDamageAmount, bool isCriticalH, float heroHit, bool isDodgeable, bool isCounterAttack)
    {
        //Calculate if the attack hits
        hitChance = (heroHit / unit.Stats.curDodge) * 100; //(80 / 100) * 100 = 80%    (200 / 100) * 100 = 200
        if (isDodgeable == false)
        {
            hitChance = 100;
        }
        if (Random.Range(0, 101) <= hitChance)
        {
            //AttackEffectPlay();
            ui.animator.Play("Hurt");

            unit.Stats.curHP -= getDamageAmount;
            if (unit.Stats.curHP <= 0)
            {
                unit.Stats.curHP = 0;
                //passive ressurect skill
                SelfRessurect(GameManager.instance.selfRessurrectChance, 50);
            }

            //show popup damage
            //Actions.OnDamageReceived(transform, isCriticalE, getDamageAmount, false);

            //update health bar
            ui.healthBar.SetSize(((unit.Stats.curHP * 100) / unit.Stats.baseHP) / 100);
        }
        else
        {
            Actions.OnDodge(transform);
        }

        if (isDodgeable == true && Random.Range(0, 100) <= 100 && !isCounterAttack && unit.Stats.curHP > 0)
        {
            StartCoroutine(CounterAttack());
        }
    }

   
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
        float minMaxAtk = Mathf.Round(Random.Range(unit.Stats.minATK, unit.Stats.maxATK));

        if (Random.Range(0, 100) <= unit.Stats.curCRIT)
        {
            isCriticalE = true;
            minMaxAtk = Mathf.Round(minMaxAtk * unit.Stats.critDamage);
        }
        BSM.PerformList[0].Attacker.GetComponent<HeroStateMachine>().TakeDamage(minMaxAtk, isCriticalE, unit.Stats.curHit, true, counterAttack);
        yield return new WaitForSeconds(0.5f);
        counterAttack = false;
    }

    //private void AttackEffectPlay()
    //{
    //    Transform MagicVFX = BSM.GetComponent<BattleStateMachine>().PerformList[0].choosenAttack.attackVFX;
    //    if (MagicVFX != null)
    //    {
    //        Instantiate(MagicVFX, transform.position, Quaternion.identity, transform);
    //    }
    //}

    private void SetParams()
    {
        //level 1 stats = 5x each. Then for each level we get 5 statpoints + 1 for each stat
        //for randomness

        var levelBasedStat = 5 + (1 * enemyLevel);

        unit.Stats.strength.BaseValue = Random.Range(levelBasedStat, levelBasedStat + enemyLevel + Random.Range(0, 3));
        unit.Stats.intellect.BaseValue = Random.Range(levelBasedStat, levelBasedStat + enemyLevel + Random.Range(0, 3));
        unit.Stats.dexterity.BaseValue = Random.Range(levelBasedStat, levelBasedStat + enemyLevel + Random.Range(0, 3));
        unit.Stats.agility.BaseValue = Random.Range(levelBasedStat, levelBasedStat + enemyLevel + Random.Range(0, 3));
        unit.Stats.stamina.BaseValue = Random.Range(levelBasedStat, levelBasedStat + enemyLevel + Random.Range(0, 3));

        //Calculate HP based on Stats
        unit.Stats.baseHP = Mathf.Round(unit.Stats.strength.BaseValue * HeroDataManager.instance.UnitDatabase.hpPerStr) + (unit.Stats.stamina.BaseValue * HeroDataManager.instance.UnitDatabase.hpPerSta);
        unit.Stats.curHP = unit.Stats.baseHP;

        //Calculate MP based on stats
        unit.Stats.baseMP = Mathf.Round(unit.Stats.intellect.BaseValue * HeroDataManager.instance.UnitDatabase.mpPerInt);
        unit.Stats.curMP = unit.Stats.baseMP;

        //Calculate Attack based on stats
        unit.Stats.baseATK = Mathf.Round((unit.Stats.strength.BaseValue * HeroDataManager.instance.UnitDatabase.atkPerStr) + (unit.Stats.intellect.BaseValue * HeroDataManager.instance.UnitDatabase.atkPerInt));
        unit.Stats.curATK = unit.Stats.baseATK;

        unit.Stats.maxATK = unit.Stats.baseATK + Random.Range(10, 50);
        unit.Stats.minATK = unit.Stats.baseATK;

        //Calculate HIT based on stats
        unit.Stats.baseHit = Mathf.Round(unit.Stats.dexterity.BaseValue * HeroDataManager.instance.UnitDatabase.hitPerDex);
        unit.Stats.curHit = unit.Stats.baseHit;

        //Calculate dodge based on stats
        unit.Stats.baseDodge = Mathf.Round(unit.Stats.agility.BaseValue * HeroDataManager.instance.UnitDatabase.dodgePerAgi);
        unit.Stats.curDodge = unit.Stats.baseDodge;

        //calculate def based on stats
        unit.Stats.baseDEF = Mathf.Round(unit.Stats.stamina.BaseValue * HeroDataManager.instance.UnitDatabase.defPerSta);
        unit.Stats.curDEF = unit.Stats.baseDEF;

        //calculate critrate based on stats
        //unit.Stats.curCRIT = unit.Stats.baseCRIT;

        //calculate speed based on stats
        unit.Stats.baseSpeed = Mathf.Round(unit.Stats.agility.BaseValue * HeroDataManager.instance.UnitDatabase.spdPerAgi);
        unit.Stats.curSpeed = unit.Stats.baseSpeed;

        expAmount = unit.Stats.strength.BaseValue + unit.Stats.intellect.BaseValue + unit.Stats.dexterity.BaseValue + unit.Stats.agility.BaseValue + unit.Stats.stamina.BaseValue;
    }

    public void SelfRessurect(int resChance, int resHP)
    {
        if (Random.Range(0, 100) <= resChance)
        {
            unit.Stats.curHP = Mathf.Round((unit.Stats.baseHP / 100) * resHP);
            //Actions.OnDamageReceived(transform, false, unit.Stats.curHP, true);
            //Instantiate(ui.RessurectVFX, transform.position, Quaternion.identity, transform);
        }
        else
        {
            ui.animator.Play("Die");
            currentState = TurnState.DEAD;
        }
    }


    //Undead mechanic
    //Perk1: While enemy with undead dies, it will rise after X turns with X HP;
    //Drawback: Undead takes 2x damage from holy property / Exorcism passive skill and if killed by holy/exorcism, can't rise
    //Drawback / perk2: Can't be buffed or debuffed / healed neither by enemies or allies unless the skill level isn't maximal
    //If undead: after death remove from PerformList, but don't remove from enemies in Battle so it can be targeted by players
    //If is targeted by player, but hasn't risen at the start of the turn, then switch targets.
    //At this point adding the rise method
    //All those skill related methods with later be moved to corresponding places, at this point it all is for testing purposes.

    public void UndeadRise()
    {
        alive = true;
        gameObject.tag = "Enemy";
        ui.Selector.SetActive(true);
        //ChooseAction();
    }

    public void Captured()
    {
        currentState = TurnState.DEAD;
    }

    //void CalcDamageForEachTarget(float calc_damage)
    //{
    //    if (BSM.HeroesInBattle.Count >= BSM.PerformList[0].choosenAttack.attackTargets)
    //    {
    //        sortBySpeed = new List<GameObject>();
    //        //add all enemies to the list 
    //        foreach (GameObject en in BSM.HeroesInBattle)
    //        {
    //            sortBySpeed.Add(en);
    //        }
    //        //remove enemy that already is in the list by default
    //        sortBySpeed.Remove(BSM.PerformList[0].AttackersTarget);
    //        //sort enemies in the list by the speed, then reverse, so we attack enemies with the highest speed
    //        sortBySpeed = sortBySpeed.OrderBy(x => x.GetComponent<Character>().unit.Stats.curSpeed).ToList();
    //        sortBySpeed.Reverse();
    //        //add speedy enemies to the list
    //        for (int j = 0; j < (BSM.PerformList[0].choosenAttack.attackTargets - 1); j++)
    //        {
    //            if (Random.Range(0, 100) <= unit.Stats.curCRIT)
    //            {
    //                isCriticalE = true;
    //                calc_damage = Mathf.Round(calc_damage * unit.Stats.critDamage);
    //            }
    //            float opponentDef = BSM.PerformList[0].AttackersTarget.GetComponent<Character>().unit.Stats.curDEF;
    //            calc_damage -= opponentDef;
    //            if (calc_damage < 0)
    //            {
    //                calc_damage = 0;
    //            }
    //            sortBySpeed[j].GetComponent<HeroStateMachine>().TakeDamage(calc_damage, isCriticalE, unit.Stats.curHit, isMelee, false);
    //        }
    //        BSM.PerformList[0].AttackersTarget.GetComponent<HeroStateMachine>().TakeDamage(calc_damage, isCriticalE, unit.Stats.curHit, isMelee, false);

    //    }
    //    else if (BSM.PerformList[0].choosenAttack.attackTargets > BSM.HeroesInBattle.Count)
    //    {
    //        for (int j = 0; j < BSM.HeroesInBattle.Count; j++)
    //        {
    //            if (Random.Range(0, 100) <= unit.Stats.curCRIT)
    //            {
    //                isCriticalE = true;
    //                calc_damage = Mathf.Round(calc_damage * unit.Stats.critDamage);
    //            }
    //            float opponentDef = BSM.PerformList[0].AttackersTarget.GetComponent<Character>().unit.Stats.curDEF;
    //            calc_damage -= opponentDef;
    //            if (calc_damage < 0)
    //            {
    //                calc_damage = 0;
    //            }
    //            BSM.HeroesInBattle[j].GetComponent<HeroStateMachine>().TakeDamage(calc_damage, isCriticalE, unit.Stats.curHit, isMelee, false); 
    //        }
    //    }
    //}

}

