using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //
    //public HeroData HeroData;
    public SkillDatabase SkillDatabase;
    //public UnitDatabase UnitDatabase;
    public ItemSaveManager ItemSaveManager;
    public GameObject InventoryEquipmentCanvas;
    public SummonInfoInterface summonInfoInterface;
    public HeroInfoInterface heroInfoInterface;
    public ChatController Chat;
    [SerializeField] GameObject OuterWorldUI;
    [SerializeField] GameObject ChatCanvas;
    public GameObject PlayerFollowCamera;
    public GameObject Storage;
    //public Transform respawnpoint;
    //class random monster

    public int curRegions;
    [Range(0, 30)] public int encounterRate = 10;

    public List<RegionData> Regions = new List<RegionData>();
    //public int enemyLevel;

    [Header("Some testing settings")]
    [Range(0, 100)] public float counterAttackChance = 20;
    [Range(0, 100)] public int selfRessurrectChance = 20;
    [Range(0, 100)] public float doubleAttackChance = 35;
    [Range(0, 10)] public float preFightCooldown = 2.5f;
    [Range(0, 10)] public float postFightCooldown = 1.5f;
    [Range(0, 100)] public int battleExpMultiplier = 1;
    [Header("Enemy settings")]
    [Range(0.1f, 100)] public float babyChance = 50f;
    [Range(0.1f, 100)] public float mutantChance = 50f;
    [Range(0.1f, 100)] public float eliteChance = 80f;
    [Range(0.1f, 100)] public float skillUseChance = 35f;

    //Hero
    public GameObject PlayerPrefab;
    public GameObject PlayerPrefabDungeon;
    public GameObject Hero;


    //Battlers
    //autobattle basic attacks
    public bool autoBattle = false;
    public int autoBattleTurns = 50;
    public int remainingAutobattleTurns = 50;
    public float fightSpeed = 1;
    //battle phases etc things


    //Positions
    public Vector3 nextHeroPosition;
    public Vector3 lastHeroPosition; //battle

    //scenes
    public string sceneToLoad;
    public string lastScene; //battle

    //bools
    public bool isWalking = false;
    public bool canGetEncounter = false;
    public bool gotAttacked = false;

    //Battle
    public List<EnemyUnit> enemysToBattle = new ();
    public int enemyAmount;
    public int heroAmount;
    public GameStates gameState;

    //testing dungeon set up
    public bool isDungeonBattle;
    public int battleExp;

    void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // This method will be called whenever a new scene is loaded

    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        
        Chat.AddToChatOutput("Welcome to the " + Application.productName + "!");
        Chat.AddToChatOutput("If you are interested in working on this project together, please contact me at <#f200ff>golem80@gmail.com</color>");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);

        if (scene.name != "CharacterCreation" && scene.name != "MainScreen")
        {
            if (Hero == null && scene.name != "BattleScene" && scene.name != "DungeonNew")
            {
                Hero = Instantiate(PlayerPrefab, transform.position, Quaternion.identity);
                Hero.name = "Player";
                //Hero.GetComponent<ControllableCharacter>().heroID = Extensions.FindMainCharacterID();
                Hero.SetActive(true);
                OuterWorldUI.SetActive(true);
            }
            if (Hero == null && scene.name != "BattleScene" && scene.name == "DungeonNew")
            {
                Hero = GameObject.FindWithTag("Player");
                //Hero.name = "Player";
                //Hero.GetComponent<ControllableCharacter>().heroID = Extensions.FindMainCharacterID();
                //Hero.SetActive(true);
                OuterWorldUI.SetActive(true);
            }
            ChatCanvas.SetActive(true);
        }

        else
        {
            ChatCanvas.SetActive(false);
            OuterWorldUI.SetActive(false);
        }

    }


    void Update()
    {
        switch (gameState)
        {
            case (GameStates.WORLD_STATE):
                if (isWalking)
                {
                    RandomEncounter();
                }

                if (gotAttacked)
                {
                    gameState = GameStates.BATTLE_STATE;
                }
                break;

            case (GameStates.TOWN_STATE):
                //do the things that are meant for town
                break;
            case (GameStates.DUNGEON_STATE):
                //do the things that are meant for dungeon
                break;

            case (GameStates.BATTLE_STATE):
                // load battle scene
                StartBattle();
                //go to idle
                gameState = GameStates.IDLE;
                break;

            case (GameStates.IDLE):

                break;
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadSceneAfterBattle()
    {
        SceneManager.LoadScene(lastScene);
        //Hero.SetActive(true);
        //OuterWorldUI.SetActive(true);
    }

    void RandomEncounter() //will be that switch to get into battle
    {
        if (isWalking && canGetEncounter)
        {
            int eRate = Mathf.Abs(encounterRate - 31);
            if (Random.Range(0, (eRate * 300)) < 10)
            {
                //Debug.Log("We got encounter!");
                gotAttacked = true;
            }
        }

    }

    public void StartBattle()
    {
        //set the amount of enemys we can encounter
        enemyAmount = Random.Range(1, Regions[curRegions].maxAmountEnemys + 1);
        //which enemys we can encounter
        for (int i = 0; i < enemyAmount; i++)
        {
            enemysToBattle.Add(Regions[curRegions].possibleEnemys[Random.Range(0, Regions[curRegions].possibleEnemys.Count)]);
        }
        lastHeroPosition = Hero.transform.position;
        nextHeroPosition = lastHeroPosition;
        lastScene = SceneManager.GetActiveScene().name;
        //Load level
        SceneManager.LoadScene(Regions[curRegions].battleScene);
        //reset player character
        isWalking = false;
        gotAttacked = false;
        canGetEncounter = false;
        //Hero.SetActive(false);
        OuterWorldUI.SetActive(false);
    }

    public void StartDungeonBattle(BattleSetupSO battleSetup)
    {
        isDungeonBattle = true;
        battleExp = battleSetup.Experience;
        //set the amount of enemys we encounter
        enemyAmount = battleSetup.EnemyUnits.Length;
        //which enemys we can encounter
        for (int i = 0; i < enemyAmount; i++)
        {
            enemysToBattle.Add(battleSetup.EnemyUnits[i].EnemyUnit);
        }
        lastHeroPosition = Hero.GetComponent<PlayerControls>().dest + new Vector3(0, 0.5f, 0);
        nextHeroPosition = lastHeroPosition;
        lastScene = SceneManager.GetActiveScene().name;
        //Load level
        SceneManager.LoadScene("BattleScene");
        //reset player character
        OuterWorldUI.SetActive(false);
    }

}