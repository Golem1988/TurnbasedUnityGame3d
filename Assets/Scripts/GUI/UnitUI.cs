using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitUI : MonoBehaviour
{
    public TMP_Text displayNameText;
    public GameObject levelText;
    public TextMeshProUGUI screamText;
    public GameObject Selector;
    public GameObject FloatingText;
    public Transform BuffPanel;
    public GameObject scream;
    public HealthBar healthBar;
    public ManaBar manaBar;
    public RageBar rageBar;
    public Animator animator;
    public AudioSource audioSource;
    //public GameObject ObjectParent;
    //public GameObject ModelHolder;
    public Sprite heroAvatar;
    //public GameObject InventoryEquipmentCanvas;
    public Canvas NameCanvas;

    private string type;

    private void OnEnable()
    {
        Actions.OnUnitDeath += HideUI;
        //InventoryEquipmentCanvas = GameManager.instance.InventoryEquipmentCanvas;
    }

private void OnDisable()
    {
        Actions.OnUnitDeath -= HideUI;
    }

    //private void OnValidate()
    //{
    //    LoadUI();
    //}

    private void Awake()
    {
        
    }


    void Start()
    {
        //LoadUI();
        if (GetComponent<ModelLoader>().Model)
            animator = GetComponent<ModelLoader>().Model.GetComponent<Animator>();
        if (gameObject.CompareTag("Hero"))
            rageBar.SetSize(gameObject.GetComponent<Character>().curRage * 100 / gameObject.GetComponent<Character>().maxRage / 100);
        displayNameText.text = gameObject.GetComponent<UnitAttributes>().Stats.displayName;
    }

    public void LoadUI()
    {
        Selector = NameCanvas.transform.Find("Selector").gameObject;
        displayNameText = NameCanvas.transform.Find("DisplayNameText").GetComponent<TextMeshProUGUI>();
        displayNameText.text = gameObject.GetComponent<UnitAttributes>().Stats.displayName;
        healthBar = NameCanvas.transform.Find("HealthBar").GetComponent<HealthBar>();
        manaBar = NameCanvas.transform.Find("ManaBar").GetComponent<ManaBar>();
        BuffPanel = NameCanvas.transform.Find("BuffPanel");
        if (gameObject.CompareTag("Hero"))
        {
            rageBar = NameCanvas.transform.Find("RageBar").GetComponent<RageBar>();
        }

        scream = NameCanvas.transform.Find("Scream").gameObject;
        screamText = NameCanvas.transform.Find("Scream").transform.GetComponentInChildren<TextMeshProUGUI>();

        audioSource = GetComponent<AudioSource>();
        Selector.SetActive(false);
    }

    void HideUI(UnitStateMachine unit)
    {
        if (unit.gameObject == gameObject)
        {
            Selector.SetActive(false);
            healthBar.gameObject.SetActive(false);
            manaBar.gameObject.SetActive(false);
            BuffPanel.gameObject.SetActive(false);
            if (rageBar)
                rageBar.gameObject.SetActive(false);
            if (levelText)
                levelText.gameObject.SetActive(false);
        }
    }
}
