using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.Events;

public class BattleStateMachine : MonoBehaviour
{
    /*
     Fight structure:
          1) PreBattle
          2) BATTLE(loop)
              2.0) Input >
              2.1) pre-turn >
              2.2) turn >
                  2.2.1) win -> PostBattle
                  2.2.2) lose -> PostBattle
              2.3) post-turn -> Input
          3) PostBattle
    */

    public BuffManager BuffManager;
    public BattleActions BattleActions;

    public enum BattlePhases
    {
        PREBATTLE,
        PLAYERINPUT,
        PREFIGHT,//starts pre-action coroutine
        FIGHT,
        POSTFIGHT //starts post-fight coroutine
    }
    public BattlePhases battlePhases;

    public UnityEvent preTurn;
    public UnityEvent postTurn;

    private bool prebattleStarted = false;
    private bool postbattleStarted = false;
    private bool postFightStarted = false;
    private bool preFightStarted = false;

    public enum PerformAction
    {
        IDLE,
        START,
        TAKEACTION,
        PERFORMACTION,
        CHECKALIVE,
        WIN,
        LOSE
    }
    public PerformAction battleStates;

    public enum HeroGUI
    {
        NOTACTIVE,
        ACTIVATE,
        WAITING,
        INPUT1, //select attack
        INPUT2, //select target
        DONE
    }
    public HeroGUI HeroInput;

    public List<HandleTurn> PerformList = new List<HandleTurn>();
    public List<GameObject> HeroesInBattle = new();
    public List<BattlePosition> EnemySpots = new();
    public List<BattlePosition> AllySpots = new();
    public List<GameObject> EnemiesInBattle = new();
    public List<GameObject> HeroesToManage = new();
    private List<GameObject> BattlerHeroes = new();

    private List<GameObject> atkBtns = new List<GameObject>();

    //enemy buttons
    public TargetSelectPanel enemyPanel;
    public TargetSelectPanel allyPanel;

    //spawnpoints
    public List<Transform> spawnPoints = new List<Transform>();
    public List<Transform> heroSpawnPoints = new List<Transform>();
    public List<Transform> summonSpawnPoints = new List<Transform>();
    [SerializeField] List<Button> BattleSpeedButtons = new();
    public bool test = true;

    //Turn things
    public int ChoicesMade = 0;
    public int CurrentTurn = 1;

    private float currentCount = 0f;
    public float startingCount = 30f;

    //autobattle?
    public bool autoBattle = false;
    public int autobattleTurns;

    private bool countdownTrigger = false;

    public HandleTurn HeroChoice;

    public Transform Spacer;
    public Transform allySpacer;
    public Transform summonSpacer;
    public Transform MassVFXenemy;
    public Transform MassVFXplayer;

    public GameObject AttackPanel;
    public GameObject EnemySelectPanel;
    public GameObject AllySelectPanel;

    public GameObject MagicPanelGrid;
    public GameObject AutoBattlePanel;
    public GameObject SummonSelectPanel;

    [SerializeField] private TMP_Text turnText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private GameObject fightText;
    [SerializeField] private Transform battleCanvas;
    [SerializeField] private TMP_Text autoText;


    //magic attack
    public Transform actionSpacer;
    public Transform magicSpacer;
    public GameObject actionButton;
    public GameObject summonButton;


    //exp calculation for won battle
    private int expMultiplier = 1; //global exp multiplier (in case of world exp increase events)
    public float expTotal; //sum of total enemy exp
    private int getExp; //how much exp will each character get
    private int xLevel; //sum of enemy levels in battle
    private int averageEnemyLvl;

    //timescale
    private float fixedDeltaTime;
    private float timeModifier;

    void Awake()
    {
        timeModifier = GameManager.instance.fightSpeed;
        DisableSpeedButton(timeModifier);
        SpawnActors();
        AutobattleSetup();
        fixedDeltaTime = Time.fixedDeltaTime;
        Debug.Log("Awake ended");
    }

    void Start()
    {
        test = false;
        getExp = 0;
        CollectExp(); //make a sum of exp enemies will be giving in case of victory
        //Set starting enums to idle etc
        HeroInput = HeroGUI.NOTACTIVE;
        battleStates = PerformAction.IDLE;
        battlePhases = BattlePhases.PREBATTLE;
        //countdown set count
        currentCount = startingCount;

        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(false);
        MagicPanelGrid.SetActive(false);

        EnemyButtons();
        AllyButtons();
    }

    // Update is called once per frame
    void Update()
    {
        switch (battlePhases)
        {
            case (BattlePhases.PREBATTLE):
                StartCoroutine(PreBattleActions());
                break;

            case (BattlePhases.PLAYERINPUT):
                //wait for player input, countdown etc
                //countdownTrigger = false;
                //countdown thingies
                if (currentCount > 1)
                {
                    currentCount -= 1 * Time.deltaTime;
                    countdownText.text = currentCount.ToString("0");
                    turnText.text = "Upcoming Turn: " + CurrentTurn;
                }
                else
                {
                    countdownText.text = "";
                    countdownTrigger = true;
                }

                if (ChoicesMade >= HeroesInBattle.Count)
                {
                    countdownText.text = "";
                    battlePhases = BattlePhases.PREFIGHT;
                }
                break;

            case (BattlePhases.PREFIGHT):
                StartCoroutine(PreFightActions());
                break;
            case (BattlePhases.FIGHT):
                //idle and show that there's current turn running
                break;

            case (BattlePhases.POSTFIGHT):
                StartCoroutine(PostFightActions());
                break;
        }

        switch (battleStates)
        {
            case (PerformAction.IDLE):
                //pre-battle waiting / just idling
                break;
            case (PerformAction.START):
                //Battle things
                if (battlePhases == BattlePhases.FIGHT && PerformList.Count == 0)
                {
                    battleStates = PerformAction.IDLE;
                    battlePhases = BattlePhases.POSTFIGHT;
                }

                if (PerformList.Count >= 1)
                {
                    turnText.text = "Current Turn: " + CurrentTurn;
                    countdownText.text = "";
                    battleStates = PerformAction.TAKEACTION;
                }

                break;

            case (PerformAction.TAKEACTION):
                GameObject performer = GameObject.Find(PerformList[0].AttackersName);
                UnitStateMachine USM = performer.GetComponent<UnitStateMachine>();
                if (PerformList[0].Attacker != null)
                {
                    USM.ChosenAttackTarget = PerformList[0].AttackersTarget;
                    USM.currentState = UnitStateMachine.TurnState.ACTION;
                }

                battleStates = PerformAction.PERFORMACTION;
                break;

            case (PerformAction.PERFORMACTION):
                //idle
                break;

            case (PerformAction.CHECKALIVE):
                if (HeroesInBattle.Count < 1)
                {
                    battleStates = PerformAction.LOSE;
                    //lose battle
                }
                else if (EnemiesInBattle.Count < 1)
                {
                    battleStates = PerformAction.WIN;
                    //win battle
                }
                else
                {
                    //call function
                    ClearAttackPanel();
                    battleStates = PerformAction.PERFORMACTION;
                }
                break;

            case (PerformAction.LOSE):
                StartCoroutine(PostBattleActions(false));
                break;

            case (PerformAction.WIN):
                StartCoroutine(PostBattleActions(true));
                break;

        }

        switch (HeroInput)
        {
            case (HeroGUI.NOTACTIVE):
                //just idle before battle
                break;
            case (HeroGUI.ACTIVATE):
                if (HeroesToManage.Count >= 1 && ChoicesMade < HeroesInBattle.Count)
                {
                    HeroesToManage[0].GetComponent<UnitUI>().Selector.SetActive(true);
                    HeroChoice = new HandleTurn();

                    HeroInput = HeroGUI.WAITING;
                    if (autoBattle == true || countdownTrigger == true)
                    {
                        AutoSelect();
                    }
                    else
                    {
                        AttackPanel.SetActive(true);
                        //populate action buttons
                        CreateActionButtons(HeroesToManage[0]);
                    }
                }
                break;
            case (HeroGUI.WAITING):
                //waiting for input
                if (countdownTrigger == true)
                {
                    AutoSelect();
                }
                break;
            case (HeroGUI.DONE):
                HeroInputDone();
                break;
        }

        if (battlePhases == BattlePhases.PREBATTLE || battlePhases == BattlePhases.PLAYERINPUT || battleStates == PerformAction.WIN || battleStates == PerformAction.LOSE)
        {
            ShowSpeedButtons(false);
            Time.timeScale = 1f;
            Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
        }
        else
        {
            ShowSpeedButtons(true);
            Time.timeScale = timeModifier;
            Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
        }

    }

    void SpawnActors()
    {
        //Spawn enemies
        for (int i = 0; i < GameManager.instance.enemysToBattle.Count; i++)
        {
            //instantiate the empty prefab
            GameObject NewEnemy = Instantiate(HeroDataManager.instance.UnitDatabase.EnemyPrefab, spawnPoints[i].position, Quaternion.Euler(0, 135, 0));
            //fill the prefab with information like model and avatar
            EnemyUnit enemyUnit = GameManager.instance.enemysToBattle[i];
            GameObject enemyModel = Extensions.FindModelPrefab(enemyUnit.ID, false);
            var model = Instantiate(enemyModel, NewEnemy.GetComponent<ModelLoader>().ModelHolder.position, Quaternion.Euler(0, 135, 0), NewEnemy.GetComponent<ModelLoader>().ModelHolder);
            NewEnemy.GetComponent<ModelLoader>().Model = model;
            NewEnemy.GetComponent<UnitUI>().Avatar = Extensions.FindSprite(enemyUnit.ID, false);
            NewEnemy.GetComponent<UnitAttributes>().Stats.theName = enemyUnit.DisplayName;
            //NewEnemy.GetComponent<UnitAttributes>().Stats.displayName = enemyUnit.DisplayName;
            NewEnemy.GetComponent<UnitStateMachine>().BSM = this;
            NewEnemy.name = NewEnemy.GetComponent<UnitAttributes>().Stats.theName + " " + (i + 1);
            EnemyTypeSet(NewEnemy);
            PopulateSkilllist(enemyUnit, NewEnemy);
            NewEnemy.GetComponent<Enemy>().BaseID = enemyUnit.ID;
            EnemiesInBattle.Add(NewEnemy);
            BattlePosition Info = new();
            Info.UnitOnSpot = NewEnemy;
            Info.Spot = i;
            EnemySpots.Add(Info);
        }

        //SPAWN HEROES NEW

        var CharacterInfo = HeroDataManager.instance.CharacterInfo;
        int spawnIndex = -1;
        for (int i = 0; i < CharacterInfo.Count; i++)
        {
            if (CharacterInfo[i].isActive)
            {
                spawnIndex++;
                GameObject heroModel = Extensions.FindModelPrefab(CharacterInfo[i].BaseID, true);
                GameObject NewHero = Instantiate(HeroDataManager.instance.UnitDatabase.HeroPrefab, heroSpawnPoints[spawnIndex].position, Quaternion.Euler(0, -45, 0));
                //NewHero.GetComponent<Character>().SetLevel();
                //NewHero.GetComponent<UnitLevel>().level = CharacterInfo[i].Level;
                NewHero.GetComponent<UnitAttributes>().Stats = CharacterInfo[i].Stats;
                NewHero.GetComponent<UnitStateMachine>().BSM = this;
                //NewHero.GetComponent<UnitAttributes>().Stats.displayName = CharacterInfo[i].Stats.displayName;
                NewHero.GetComponent<SummonHandler>().SummonList = CharacterInfo[i].SummonList;
                var magicAttacks = CharacterInfo[i].MagicAttacks;
                var passiveSkills = CharacterInfo[i].PassiveSkills;
                //add skills
                if (magicAttacks.Count > 0)
                {
                    foreach (string skillID in magicAttacks)
                    {
                        NewHero.GetComponent<Abilities>().MagicAttacks.Add(Extensions.FindActiveSkillID(skillID));
                    }
                }
                if (passiveSkills.Count > 0)
                {
                    foreach (string skillID in passiveSkills)
                    {
                        NewHero.GetComponent<Abilities>().PassiveSkills.Add(Extensions.FindPassiveSkillID(skillID));
                    }
                }
                NewHero.name = CharacterInfo[i].Stats.theName;
                NewHero.GetComponent<UnitUI>().Avatar = Extensions.FindSprite(CharacterInfo[i].BaseID, true);
                var Stats = NewHero.GetComponent<UnitAttributes>().Stats;
                NewHero.GetComponent<UnitUI>().healthBar.SetSize(Stats.curHP / Stats.baseHP);
                NewHero.GetComponent<UnitUI>().manaBar.SetSize(Stats.curMP / Stats.baseMP);
                //NewHero.GetComponent<UnitUI>().InventoryEquipmentCanvas.gameObject.SetActive(false);
                var holder = NewHero.GetComponent<ModelLoader>();
                var model = Instantiate(heroModel, holder.ModelHolder.position, Quaternion.Euler(0, -45, 0), holder.ModelHolder);
                holder.Model = model;
                HeroesInBattle.Add(NewHero);
                BattlePosition Info = new();
                Info.UnitOnSpot = NewHero;
                Info.Spot = spawnIndex;
                AllySpots.Add(Info);

                BattlerHeroes.Add(NewHero);

                //instantiate hero summons
                if (NewHero.GetComponent<SummonHandler>().SummonList.Count > 0)
                {
                    UnitStateMachine owner = NewHero.GetComponent<UnitStateMachine>();
                    int summonIndex = NewHero.GetComponent<SummonHandler>().SummonList.FindIndex(status => status.active == true);
                    if (summonIndex >= 0)
                        gameObject.GetComponent<BattleActions>().SpawnSummonX(owner, spawnIndex, summonIndex);
                }
            }
        }
    }

    public void EnemyTypeSet(GameObject NewEnemy)
    {
        float rate = UnityEngine.Random.Range(0f, 100f);
        string typeTxt = "";
        EnemyType typeType = EnemyType.NORMAL;
        int eLvl = 1;
        int curRegions = GameManager.instance.curRegions;
        if (rate <= GameManager.instance.babyChance)
        {
            float mutantRate = UnityEngine.Random.Range(0f, 100f);
            if (mutantRate <= GameManager.instance.mutantChance)
            {
                //typeTxt = "Mutant";
                typeType = EnemyType.MUTANT;
                NewEnemy.GetComponent<UnitUI>().displayNameText.color = new Color32(155, 0, 155, 255); // purple
            }
            else
            {
                //typeTxt = "Baby";
                typeType = EnemyType.BABY;
                NewEnemy.GetComponent<UnitUI>().displayNameText.color = Color.magenta;
            }
            typeTxt = "Baby";
        }
        else if (rate <= GameManager.instance.eliteChance)
        {
            typeTxt = "Elite";
            typeType = EnemyType.ELITE;
            NewEnemy.GetComponent<UnitUI>().displayNameText.color = new Color32(255, 90, 0, 255); // orange
            eLvl = GameManager.instance.Regions[curRegions].EnemyLevel + 5;
        }
        else
            eLvl = GameManager.instance.Regions[curRegions].EnemyLevel;
        
        if (typeTxt != "")
            NewEnemy.GetComponent<UnitAttributes>().Stats.displayName = typeTxt + " " + NewEnemy.GetComponent<UnitAttributes>().Stats.theName;
        else
            NewEnemy.GetComponent<UnitAttributes>().Stats.displayName = NewEnemy.GetComponent<UnitAttributes>().Stats.theName;
        NewEnemy.GetComponent<UnitAttributes>().Stats.theName = NewEnemy.name;
        NewEnemy.GetComponent<Enemy>().enemyType = typeType;
        NewEnemy.GetComponent<Enemy>().SetEnemyLevelX(eLvl);
    }
    public void PopulateSkilllist(EnemyUnit enemyData, GameObject NewEnemy) //Take skills from the list of possible skills, calculate chances and spawn in according lists
    {
        if (enemyData.ActiveSkills.Count > 0)
        {
            for (int i = 0; i < enemyData.ActiveSkills.Count; i++)
            {
                float chance = enemyData.ActiveSkills[i].skillSpawnChance;
                if (NewEnemy.GetComponent<Enemy>().enemyType == EnemyType.ELITE)
                {
                    chance = 100f;
                }
                if (UnityEngine.Random.Range(0, 100) <= chance)
                {
                    ActiveSkill NewSkill = enemyData.ActiveSkills[i].possibleSkill;
                    NewEnemy.GetComponent<Abilities>().MagicAttacks.Add(NewSkill);
                }
            }
        }

        if (enemyData.PassiveSkills.Count > 0)
        {
            for (int i = 0; i < enemyData.PassiveSkills.Count; i++)
            {
                float chance = enemyData.PassiveSkills[i].skillSpawnChance;
                if (NewEnemy.GetComponent<Enemy>().enemyType == EnemyType.ELITE)
                {
                    chance = 100f;
                }
                if (UnityEngine.Random.Range(0, 100) <= chance)
                {
                    PassiveSkill NewSkill = enemyData.PassiveSkills[i].posPassive;
                    NewEnemy.GetComponent<Abilities>().PassiveSkills.Add(NewSkill);
                }
            }
        }
    }

    public void CollectActions(HandleTurn input)
    {
        PerformList.Add(input);
    }

    void CreateActionButtons(GameObject PlayerOrSummon)
    {

        GameObject AttackButton = Instantiate(actionButton);
        TextMeshProUGUI AttackButtonText = AttackButton.GetComponent<ActionButton>().ActionText;
        AttackButtonText.text = "Attack";
        AttackButton.GetComponent<Button>().onClick.AddListener(() => Input1());
        AttackButton.transform.SetParent(actionSpacer, false);
        atkBtns.Add(AttackButton);

        GameObject MagicAttackButton = Instantiate(actionButton);
        TextMeshProUGUI MagicAttackButtonText = MagicAttackButton.GetComponent<ActionButton>().ActionText;
        MagicAttackButtonText.text = "Magic";
        MagicAttackButton.GetComponent<Button>().onClick.AddListener(() => Input3());
        MagicAttackButton.transform.SetParent(actionSpacer, false);
        atkBtns.Add(MagicAttackButton);


        //Flee button allowing to flee the battle
        GameObject FleeButton = Instantiate(actionButton);
        TextMeshProUGUI FleeButtonText = FleeButton.GetComponent<ActionButton>().ActionText;
        FleeButtonText.text = "Flee";
        FleeButton.GetComponent<Button>().onClick.AddListener(() => Input5());
        FleeButton.transform.SetParent(actionSpacer, false);
        atkBtns.Add(FleeButton);

        //Defense button
        //Defense system: If in defence stance, take only XX % damage. Attack noone.
        GameObject DefendButton = Instantiate(actionButton);
        TextMeshProUGUI DefendButtonText = DefendButton.GetComponent<ActionButton>().ActionText;
        DefendButtonText.text = "Defend";
        DefendButton.GetComponent<Button>().onClick.AddListener(() => Input7());
        DefendButton.transform.SetParent(actionSpacer, false);
        atkBtns.Add(DefendButton);

        if (PlayerOrSummon.CompareTag("Hero"))
        {
            GameObject CaptureButton = Instantiate(actionButton);
            TextMeshProUGUI CaptureButtonText = CaptureButton.GetComponent<ActionButton>().ActionText;
            CaptureButtonText.text = "Capture";
            CaptureButton.GetComponent<Button>().onClick.AddListener(() => Input8());
            CaptureButton.transform.SetParent(actionSpacer, false);
            atkBtns.Add(CaptureButton);

            GameObject SpawnSummonButton = Instantiate(actionButton);
            TextMeshProUGUI SpawnSummonButtonText = SpawnSummonButton.GetComponent<ActionButton>().ActionText;
            SpawnSummonButtonText.text = "Spawn";
            SpawnSummonButton.GetComponent<Button>().onClick.AddListener(() => Input9(PlayerOrSummon));
            SpawnSummonButton.transform.SetParent(actionSpacer, false);
            atkBtns.Add(SpawnSummonButton);
            if (PlayerOrSummon.GetComponent<SummonHandler>().SummonList.Count == 0 || !PlayerOrSummon.GetComponent<SummonHandler>().SummonList.Any(summon => summon.isDeployable))
                SpawnSummonButton.GetComponent<Button>().interactable = false;

            //Autobattle enable button
            GameObject AutoSelectButton = Instantiate(actionButton);
            TextMeshProUGUI AutoSelectButtonText = AutoSelectButton.GetComponent<ActionButton>().ActionText;
            AutoSelectButtonText.text = "Auto";
            AutoSelectButton.GetComponent<Button>().onClick.AddListener(() => ToggleAutoBattle(true));
            AutoSelectButton.transform.SetParent(actionSpacer, false);
            atkBtns.Add(AutoSelectButton);
        }

        if (HeroesToManage[0].GetComponent<Abilities>().MagicAttacks.Count > 0)
        {
            var skillList = HeroesToManage[0].GetComponent<Abilities>().MagicAttacks;
            var skillSlots = MagicPanelGrid.GetComponent<MagicPanel>().attackButtons;
            for (int i = 0; i < skillSlots.Length; i++)
            {
                skillSlots[i].GetComponent<Button>().interactable = false;
                skillSlots[i].skillIconObject.gameObject.SetActive(false);
                skillSlots[i].magicAttackToPerform = null;
                skillSlots[i].isAssigned = false;
            }
            for (int i = 0; i < skillList.Count; i++)
            {
                skillSlots[i].skillIconObject.sprite = skillList[i].SkillIcon;
                skillSlots[i].magicAttackToPerform = skillList[i];
                skillSlots[i].isAssigned = true;
                skillSlots[i].skillIconObject.gameObject.SetActive(true);
                skillSlots[i].GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            MagicAttackButton.GetComponent<Button>().interactable = false;
        }
    }

    public void EnemyButtons()
    {
        //clear all buttons first
        var targetPanel = enemyPanel.GetComponent<TargetSelectPanel>();

        for (int i = 0; i < targetPanel.enemyButtons.Length; i++)
        {
            targetPanel.enemyButtons[i].EnemyPrefab = null;
            targetPanel.enemyButtons[i].enemyIconObject.sprite = null;
            targetPanel.enemyButtons[i].isAssigned = false;
            targetPanel.enemyButtons[i].enemyIconObject.gameObject.SetActive(false);
            targetPanel.enemyButtons[i].GetComponent<Button>().interactable = false;
        }

        //create buttons for each enemy
        for (int i = 0; i < EnemySpots.Count; i++)
        {
            targetPanel.enemyButtons[EnemySpots[i].Spot].EnemyPrefab = EnemySpots[i].UnitOnSpot;
            targetPanel.enemyButtons[EnemySpots[i].Spot].enemyIconObject.sprite = EnemySpots[i].UnitOnSpot.GetComponent<UnitUI>().Avatar;
            targetPanel.enemyButtons[EnemySpots[i].Spot].isAssigned = true;
            targetPanel.enemyButtons[EnemySpots[i].Spot].enemyIconObject.gameObject.SetActive(true);
            //if (EnemySpots[i].UnitOnSpot.GetComponent<Enemy>().enemyType != EnemyType.ELITE)
                targetPanel.enemyButtons[EnemySpots[i].Spot].GetComponent<Button>().interactable = true;
        }
    }


    public void AllyButtons()
    {
        //clear all buttons first
        var targetPanel = allyPanel.GetComponent<TargetSelectPanel>();

        for (int i = 0; i < targetPanel.allyButtons.Length; i++)
        {
            targetPanel.allyButtons[i].AllyPrefab = null;
            targetPanel.allyButtons[i].allyIconObject.sprite = null;
            targetPanel.allyButtons[i].isAssigned = false;
            targetPanel.allyButtons[i].allyIconObject.gameObject.SetActive(false);
            targetPanel.allyButtons[i].GetComponent<Button>().interactable = false;
        }

        //create buttons for each enemy
        for (int i = 0; i < AllySpots.Count; i++)
        {
            targetPanel.allyButtons[AllySpots[i].Spot].AllyPrefab = AllySpots[i].UnitOnSpot;
            targetPanel.allyButtons[AllySpots[i].Spot].allyIconObject.sprite = AllySpots[i].UnitOnSpot.GetComponent<UnitUI>().Avatar;
            targetPanel.allyButtons[AllySpots[i].Spot].isAssigned = true;
            targetPanel.allyButtons[AllySpots[i].Spot].allyIconObject.gameObject.SetActive(true);
            targetPanel.allyButtons[AllySpots[i].Spot].GetComponent<Button>().interactable = true;
        }
    }

    public void SummonButtons(GameObject PlayerOrSummon)
    {
        //destroy all existing buttons
        Extensions.DestroyAllChildren(summonSpacer);

        var SummonList = PlayerOrSummon.GetComponent<SummonHandler>().SummonList;
        //instantiate the buttons based on how many pets there are in the list
        for (int i = 0; i < SummonList.Count; i++)
        {
            if (SummonList[i].isDeployable && !SummonList[i].active)
            {
                var button = Instantiate(summonButton, summonSpacer.position, Quaternion.identity, summonSpacer);
                var sButton = button.GetComponent<SummonSelectButton>();
                sButton.levelText.text = SummonList[i].Level.currentlevel.ToString();
                sButton.summonIcon.sprite = Extensions.FindSprite(SummonList[i].BaseID, false);
                sButton.typeText.text = SummonList[i].Type.ToString();
                if (SummonList[i].Type == EnemyType.BABY)
                    sButton.typeText.color = Color.magenta;
                if (SummonList[i].Type == EnemyType.MUTANT)
                    sButton.typeText.color = new Color32(124, 0, 124, 255);
                sButton.hpText.text = (SummonList[i].Stats.curHP.ToString() + "/" + SummonList[i].Stats.baseHP.ToString());
                sButton.index = i;
                button.GetComponent<Button>().onClick.AddListener(() => Input10(sButton.index));
                button.GetComponent<Button>().interactable = true;
            }
        }
        //remove the one that's already on battlefield

        //make inactive the ones that should be inactive

    }


    public void Input1() //attack button
    {
        HeroChoice.AttackersName = HeroesToManage[0].name; //might be changed
        HeroChoice.attackersSpeed = HeroesToManage[0].GetComponent<UnitAttributes>().Stats.curSpeed;
        HeroChoice.Attacker = HeroesToManage[0];
        HeroChoice.Type = "Hero";
        HeroChoice.choosenAttack = HeroesToManage[0].GetComponent<Abilities>().BasicActions[0];
        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(true);
    }


    public void Input2(GameObject choosenEnemy) //select enemy / target
    {
        HeroChoice.AttackersTarget = choosenEnemy; //add initial target to list
        HeroInput = HeroGUI.DONE;
    }

    public void Input3() //switching to magic attacks
    {
        AttackPanel.SetActive(false);
        MagicPanelGrid.SetActive(true);
    }

    public void Input4(ActiveSkill choosenMagic) //chosen magic attack
    {
        HeroChoice.AttackersName = HeroesToManage[0].name; //might be changed
        HeroChoice.attackersSpeed = HeroesToManage[0].GetComponent<UnitAttributes>().Stats.curSpeed;
        HeroChoice.Attacker = HeroesToManage[0];
        HeroChoice.Type = "Hero";
        HeroChoice.choosenAttack = choosenMagic;
        MagicPanelGrid.SetActive(false);
        //MagicPanel.SetActive(false);
        if (choosenMagic.IsAttack == true)
        {
            EnemySelectPanel.SetActive(true);
        }
        else
        {
            AllySelectPanel.SetActive(true);
        }
    }


    public void Input5() //fleeing from battle and clearing the current battle information
    {
        for (int i = 0; i < HeroesInBattle.Count; i++)
        {
            HeroesInBattle[i].GetComponent<UnitStateMachine>().currentState = UnitStateMachine.TurnState.WAITING;
        }

        Debug.Log("Fleed from battle");
        GameManager.instance.LoadSceneAfterBattle();
        GameManager.instance.gameState = GameStates.WORLD_STATE;
        GameManager.instance.enemysToBattle.Clear();
    }

    public void Input6(GameObject choosenAlly) //select enemy / target
    {
        HeroChoice.AttackersTarget = choosenAlly; //add initial target to list
        HeroInput = HeroGUI.DONE;
    }


    //defense mechanic
    public void Input7()
    {
        HeroChoice.AttackersName = HeroesToManage[0].name; //might be changed
        HeroChoice.attackersSpeed = 9999;
        HeroChoice.Attacker = HeroesToManage[0];
        HeroChoice.Type = "Hero";
        if (HeroesToManage[0].CompareTag("Hero"))
            HeroChoice.choosenAttack = HeroesToManage[0].GetComponent<Abilities>().BasicActions[2];
        if (HeroesToManage[0].CompareTag("Summon"))
            HeroChoice.choosenAttack = HeroesToManage[0].GetComponent<Abilities>().BasicActions[1];
        HeroChoice.index = -1;
        HeroInput = HeroGUI.DONE;
    }


    //capture mechanic
    public void Input8()
    {
        HeroChoice.AttackersName = HeroesToManage[0].name; //might be changed
        HeroChoice.attackersSpeed = HeroesToManage[0].GetComponent<UnitAttributes>().Stats.curSpeed;
        HeroChoice.Attacker = HeroesToManage[0];
        HeroChoice.Type = "Hero";
        HeroChoice.choosenAttack = HeroesToManage[0].GetComponent<Abilities>().BasicActions[1];
        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(true);//should be changed later, we have to add "capturable enemies" buttons, since not every enemy should / could be captured
    }

    //call summon mechanic
    public void Input9(GameObject player)
    {
        HeroChoice.AttackersName = HeroesToManage[0].name; //might be changed
        HeroChoice.attackersSpeed = HeroesToManage[0].GetComponent<UnitAttributes>().Stats.curSpeed;
        HeroChoice.Attacker = HeroesToManage[0];
        HeroChoice.Type = "Hero";
        HeroChoice.choosenAttack = null;
        HeroChoice.choosenAction = HeroesToManage[0].GetComponent<Abilities>().BasicActs[0]; //summon spawn scriptable object in list
        AttackPanel.SetActive(false);
        SummonButtons(player);
        SummonSelectPanel.SetActive(true);
    }

    public void Input10(int index)
    {
        HeroChoice.AttackersTarget = null; //add initial target to list
        HeroChoice.index = index;
        HeroInput = HeroGUI.DONE;
    }

    void HeroInputDone()
    {
        PerformList.Add(HeroChoice);
        ChoicesMade++;
        //cleanup attack panel
        ClearAttackPanel();

        HeroesToManage[0].transform.Find("NameCanvasNew").transform.Find("Selector").gameObject.SetActive(false);
        HeroesToManage.RemoveAt(0);
        HeroInput = HeroGUI.ACTIVATE;
    }

    void ClearAttackPanel()
    {
        EnemySelectPanel.SetActive(false);
        AllySelectPanel.SetActive(false);
        AttackPanel.SetActive(false);
        MagicPanelGrid.SetActive(false);
        SummonSelectPanel.SetActive(false);

        foreach (GameObject atkBtn in atkBtns)
        {
            Destroy(atkBtn);
        }
        atkBtns.Clear();
    }

    public void AutoSelect()
    {
        //HeroChoise = new HandleTurn();
        BaseClass heroe = HeroesToManage[0].GetComponent<UnitAttributes>().Stats;
        Abilities abilities = HeroesToManage[0].GetComponent<Abilities>();
        HeroChoice.AttackersName = HeroesToManage[0].name; //might be changed
        HeroChoice.attackersSpeed = HeroesToManage[0].GetComponent<UnitAttributes>().Stats.curSpeed;
        HeroChoice.Attacker = HeroesToManage[0];
        HeroChoice.Type = "Hero";
        if (countdownTrigger == true)
        {
            HeroChoice.choosenAttack = abilities.BasicActions[0]; //0 is assigned to basic attack 1 is assigned to capture at this point
        }

        if (countdownTrigger == false && heroe.curMP > 0 && abilities.MagicAttacks.Count > 0)
        {
            List<ActiveSkill> magicAttacks = new(abilities.MagicAttacks);
            //remove all we can't use
            magicAttacks.RemoveAll(attack => attack.CostType != CostType.MP || attack.CostValue > heroe.curMP);
            //now select either attacking skill or healing skill based on information
            if (magicAttacks.Count > 1)
            {
                int num = UnityEngine.Random.Range(0, magicAttacks.Count);
                HeroChoice.choosenAttack = magicAttacks[num];
            }
            else if (magicAttacks.Count == 1)
            {
                HeroChoice.choosenAttack = magicAttacks[0];
            }

            if (HeroChoice.choosenAttack != null && HeroChoice.choosenAttack.TargetType == TargetType.Ally)
            {
                HeroChoice.AttackersTarget = HeroesInBattle[UnityEngine.Random.Range(0, HeroesInBattle.Count)];
            }
            else if (HeroChoice.choosenAttack != null && HeroChoice.choosenAttack.TargetType == TargetType.Foe)
            {
                HeroChoice.AttackersTarget = EnemiesInBattle[UnityEngine.Random.Range(0, EnemiesInBattle.Count)];
            }
        }
        else
        {
            HeroChoice.choosenAttack = abilities.BasicActions[0];
            HeroChoice.AttackersTarget = EnemiesInBattle[UnityEngine.Random.Range(0, EnemiesInBattle.Count)];
        }
        if (HeroChoice.choosenAttack == null)
        {
            HeroChoice.choosenAttack = abilities.BasicActions[0];
            HeroChoice.AttackersTarget = EnemiesInBattle[UnityEngine.Random.Range(0, EnemiesInBattle.Count)];
        }
        HeroInput = HeroGUI.DONE;
    }

    public void ToggleAutoBattle(bool once)
    {
        if (once)
        {
            autoBattle = true;
            AutoBattlePanel.SetActive(true);
        }
        else
        {
            autoBattle = !autoBattle;
        }
        if (autoBattle)
        {
            GameManager.instance.autoBattle = true;
            autobattleTurns = GameManager.instance.autoBattleTurns;
            autoText.text = "Autobattle: ON" + Environment.NewLine + "Turns left: " + autobattleTurns;
            if (HeroInput == HeroGUI.WAITING)
            {
                AutoSelect();
            }
        }
        else
        {
            GameManager.instance.autoBattle = false;
            autoText.text = "Autobattle: OFF";
        }
    }

    void InstantiateFightText()
    {
        GameObject FightText = Instantiate(fightText);
        FightText.transform.SetParent(battleCanvas, false);
    }

    void AutobattleSetup()
    {
        autobattleTurns = GameManager.instance.remainingAutobattleTurns;
        if (GameManager.instance.autoBattle == true)
        {
            if (autobattleTurns > 0)
            {
                autoBattle = true;
                AutoBattlePanel.SetActive(true);
                autoText.text = "Autobattle: ON" + Environment.NewLine + "Turns left: " + autobattleTurns;
                Debug.Log("Autobattle is on, remaining autobattle turns: " + autobattleTurns);
            }
        }
        else
        {
            autoText.text = "Autobattle: OFF";
        }
    }

    void AutobattleControl()
    {
        autobattleTurns--;
        GameManager.instance.remainingAutobattleTurns--;
        autoText.text = "Autobattle: ON" + Environment.NewLine + "Turns left: " + autobattleTurns;
        if (autobattleTurns <= 0)
        {
            autoBattle = false;
            autoText.text = "Autobattle: OFF";
        }
    }

    //Set up some things that are to be made before battle even begins
    //basically it's just for the pre-battle cooldown purpouse at this point
    private IEnumerator PreBattleActions()
    {
        if (prebattleStarted)
        {
            yield break;
        }

        prebattleStarted = true;

        //Actions.OnBattleStart(this);
        
        for (int i = 0; i < EnemiesInBattle.Count; i++)
        {
            EnemiesInBattle[i].GetComponentInChildren<Animator>().Play("Show");
        }

        for (int i = 0; i < HeroesInBattle.Count; i++)
        {
            HeroesInBattle[i].GetComponentInChildren<Animator>().Play("Show");
        }


        yield return new WaitForSeconds(GameManager.instance.preFightCooldown);

        //start the fight
        HeroInput = HeroGUI.ACTIVATE;
        battlePhases = BattlePhases.PLAYERINPUT;

        prebattleStarted = false;
    }


    //At this point PreFight makes very little sense actually
    private IEnumerator PreFightActions()
    {
        if (preFightStarted)
        {
            yield break;
        }

        preFightStarted = true;
        Actions.OnTurnStart(this); //invoke preturn actions, in this case buffs trigger etc
        Time.timeScale = timeModifier;
        Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
        InstantiateFightText();
        //Order actors in performlist based on their speed 
        UpdatePerformList();

        yield return new WaitForSeconds(0.5f);

        //apply all the effects that apply just right before the fight starts (like +-HP at the start of the turn)
        //put heroes with special attacks / skills waiting in front of the PerformList
        //(like skills that require activation and will perform first at the start of the next turn)
        //BuffManager.ExecuteBuffManager();


        yield return new WaitForSeconds(0.5f);

        currentCount = startingCount;
        countdownTrigger = false;

        battleStates = PerformAction.START;
        battlePhases = BattlePhases.FIGHT;

        preFightStarted = false;
    }

    //Actions at the end of turn
    private IEnumerator PostFightActions()
    {
        if (postFightStarted)
        {
            yield break;
        }

        postFightStarted = true;
        Actions.OnTurnEnd(this);

        CurrentTurn++;

        yield return new WaitForSeconds(0.5f);

        ChoicesMade = 0;

        //decrease autobattle turns
        if (autoBattle == true)
        {
            AutobattleControl();
        }
        yield return new WaitForSeconds(0.5f);
        //And start a new turn actions
        Time.timeScale = 1;
        Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;

        if (EnemiesInBattle.Count > 0)
            battlePhases = BattlePhases.PLAYERINPUT;

        postFightStarted = false;
    }


    //we will use this later on when we will be using speed buffs
    //and will trigger this method if some will get their speed modified mid-turn
    //for no we will use it at the beginning of each turns
    public void UpdatePerformList()
    {
        PerformList = PerformList.OrderBy(x => x.attackersSpeed).ToList();
        PerformList.Reverse();
    }

    //win / lose / etc
    private IEnumerator PostBattleActions(bool battleWin)
    {
        if (postbattleStarted)
        {
            yield break;
        }

        postbattleStarted = true;

        if (battleWin == true)
        {
            Debug.Log("You won the battle");
            for (int i = 0; i < HeroesInBattle.Count; i++)
            {
                GetExperience(i);
                HeroesInBattle[i].GetComponent<UnitStateMachine>().currentState = UnitStateMachine.TurnState.WAITING;
            }
        }
        else
        {
            Debug.Log("You lost the battle");
            for (int i = 0; i < HeroesInBattle.Count; i++)
            {
                HeroesInBattle[i].GetComponent<UnitStateMachine>().currentState = UnitStateMachine.TurnState.WAITING;
            }
        }

        //Here goes saving

        yield return new WaitForSeconds(GameManager.instance.postFightCooldown);
        for (int i = 0; i < HeroesInBattle.Count; i++)
        {
            HeroesInBattle[i].SetActive(false);
        }
        Time.timeScale = 1;
        Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
        BattleActions.MakeSummonsDeploayable(BattlerHeroes);
        //HeroDataManager.instance.SaveCharData();
        //yield return new WaitForSeconds(0.25f);
        GameManager.instance.LoadSceneAfterBattle();
        GameManager.instance.gameState = GameStates.WORLD_STATE;
        GameManager.instance.enemysToBattle.Clear();

        //postbattleStarted = false;
    }

    private void GetExperience(int i)
    {
        UnitLevel unitLevel = HeroesInBattle[i].GetComponent<UnitLevel>();
        CalculateExp(unitLevel.level.currentlevel);
        var unitAttributes = HeroesInBattle[i].GetComponent<UnitAttributes>();
        Debug.Log("unit attributes belong to = " + unitAttributes.gameObject.name);
        unitLevel.level.AddExp(getExp, unitAttributes);
        Debug.Log("Unit " + HeroesInBattle[i].GetComponent<UnitAttributes>().Stats.displayName + " gained " + getExp + "EXP.");
        GameManager.instance.Chat.AddToChatOutput(HeroesInBattle[i].GetComponent<UnitAttributes>().Stats.displayName + " gained " + getExp + "EXP.");

        if (HeroesInBattle[i].CompareTag("Hero"))
        {
            var name = HeroesInBattle[i].name;
            int index = HeroDataManager.instance.CharacterInfo.FindIndex(hero => hero.Name == name);
            HeroDataManager.instance.CharacterInfo[index].Level = unitLevel.level;
        }
        else if (HeroesInBattle[i].CompareTag("Summon"))
        {
            var ownerName = HeroesInBattle[i].GetComponent<Summon>().OwnerName;
            Debug.Log("ownerName = " + ownerName);
            int index = HeroDataManager.instance.CharacterInfo.FindIndex(hero => hero.Name == ownerName);
            Debug.Log("ownerindex = " + index.ToString());
            int summonIndex = HeroDataManager.instance.CharacterInfo[index].SummonList.FindIndex(summon => summon.UniqueID == HeroesInBattle[i].GetComponent<Summon>().UniqueID);
            Debug.Log("summonIndex = " + summonIndex.ToString());
            HeroDataManager.instance.CharacterInfo[index].SummonList[summonIndex].Level = unitLevel.level;
        }
    }

    public void SpeedUpTime(float modifier)
    {
        Debug.Log(modifier);
        timeModifier = modifier;
        GameManager.instance.fightSpeed = modifier;
    }

    void CalculateExp(int yLevel)
    {
        Debug.Log("Average level: " + averageEnemyLvl + "expTotal: " + expTotal);
        getExp = (int)Mathf.Round((expTotal * averageEnemyLvl) / (HeroesInBattle.Count * yLevel) * expMultiplier);

    }

    void CollectExp()
    {
        for (int i = 0; i < EnemiesInBattle.Count; i++)
        {
            xLevel += EnemiesInBattle[i].GetComponent<UnitLevel>().level.currentlevel;
            expTotal += EnemiesInBattle[i].GetComponent<Enemy>().expAmount;
            averageEnemyLvl = (int)Mathf.Round(xLevel / EnemiesInBattle.Count);
        }
        Debug.Log("Average level: " + averageEnemyLvl + "expTotal: " + expTotal);
    }

    void DisableSpeedButton(float speed)
    {
        if (speed == 1f)
        {
            BattleSpeedButtons[0].interactable = false;
        }
        if (speed == 2f)
        {
            BattleSpeedButtons[1].interactable = false;
        }
        if (speed == 4f)
        {
            BattleSpeedButtons[2].interactable = false;
        }
    }

    void ShowSpeedButtons(bool yesno)
    {
         BattleSpeedButtons[0].interactable = yesno;
         BattleSpeedButtons[1].interactable = yesno;
         BattleSpeedButtons[2].interactable = yesno;
    }

}
