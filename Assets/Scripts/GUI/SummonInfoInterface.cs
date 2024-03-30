using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class SummonInfoInterface : MonoBehaviour
{
    //[SerializeField] GameObject SummonInfoPanelGameObject;
    [SerializeField] UnitedInfoPanel mainPanel;
    [SerializeField] private Transform SummonListDisplay;
    [SerializeField] private Transform SummonPicture;
    [SerializeField] private GameObject button;
    [SerializeField] SkillPanel summonSkillPanel;
    [SerializeField] GameObject cover;
    public GameObject myAv;
    //private GameObject avatar;
    public CharacterInformation Owner; //HeroData.CharacterInfo
    public int MaxSummonSlots;
    public List<CapturedPets> SummonList = new ();
    public List<GameObject> buttonObject = new List<GameObject>();
    [SerializeField] GameObject owner;
    [SerializeField] TMP_Text[] textItems;
    [SerializeField] GameObject makeMainButton;
    [SerializeField] GameObject restButton;
    [SerializeField] GameObject expellButton;
    private string typeColorCode;

    [Header("Current summon")]
    public CapturedPets EditableSummon;
    
    //Main information and stat display
    [Header("Display Information")]
    public TMP_Text summonSlots;

    public TMP_Text SummonName;
    public TMP_Text summonLevel;
    public TMP_Text summonType;

    public TMP_Text summonHP;
    public TMP_Text summonMP;
    public TMP_Text summonAtk;
    public TMP_Text summonMatk;
    public TMP_Text summonDef;

    public TMP_Text summonDodge;
    public TMP_Text summonHit;
    public TMP_Text summonCrit;
    public TMP_Text summonSpeed;

    public TMP_Text summonStr;
    public TMP_Text summonInt;
    public TMP_Text summonDex;
    public TMP_Text summonAgi;
    public TMP_Text summonSta;

    public TMP_Text summonStatpts;

    public TMP_Text summonLoyalty;
    public TMP_Text summonExp;

    //Stat preview display upon pre-levelling stats
    [Header("Stats preview text")]
    public TMP_Text summonHPpre;
    public TMP_Text summonMPpre;
    public TMP_Text summonAtkpre;
    public TMP_Text summonMatkpre;
    public TMP_Text summonDefpre;

    public TMP_Text summonDodgepre;
    public TMP_Text summonHitpre;
    public TMP_Text summonCritpre;
    public TMP_Text summonSpeedpre;

    public TMP_Text summonStrpre;
    public TMP_Text summonIntpre;
    public TMP_Text summonDexpre;
    public TMP_Text summonAgipre;
    public TMP_Text summonStapre;

    public TMP_Text summonStatptsPre;

    //Variables for stat pre-levelling
    [Header("Will be added")]
    public int AddedStr;
    public int AddedInt;
    public int AddedDex;
    public int AddedAgi;
    public int AddedSta;

    public int curStatpts;

    private int addedStatpts;
    
    private float addedHP;
    private float addedMP;
    private float addedAtk;
    private float addedMatk;
    private float addedDef;
    
    private float addedCrit;
    private float addedDodge;
    private float addedHit;
    private float addedSpeed;

    public float typeMultiplier = 1f;

    [Header("Attribute multipliers")]
    private float hpPerStr;
    private float atkPerStr;
    private float mpPerInt;
    private float atkPerInt;
    private float spdPerAgi;
    private float dodgePerAgi;
    private float hitPerDex;
    private float atkPerDex;
    private float hpPerSta;
    private float defPerSta;
    private float matkPerInt;
    private float matkPerStr;


    private void Awake()
    {

    }

    void OnDisable()
    {
        //Clean();
        //Actions.OnHeroEdit += EnableSummonEditing;
        if (mainPanel.HideSummonButton.activeSelf == true)
        {
            mainPanel.ShowSummonButton.SetActive(true);
            mainPanel.HideSummonButton.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (mainPanel.ShowSummonButton.activeSelf == true)
        {
            mainPanel.ShowSummonButton.SetActive(false);
            mainPanel.HideSummonButton.SetActive(true);
        }
        //Clean();
        //UpdateAvatar();
    }

    public void Start()
    {
        //Owner = HeroData.instance.CharacterInfo[0];
        string mainHeroName = HeroDataManager.instance.CharacterInfo.FirstOrDefault(name => name.isMainCharacter)?.Name;
        Owner = Extensions.FindHeroEntry(mainHeroName);
        SummonList = Owner.SummonList;
        if (SummonList.Count > 0)
        {
            EditableSummon = SummonList[0];
        }
        else
            EditableSummon = null;
        ////EditableSummon = SummonList[0];
        MaxSummonSlots = Owner.MaxSummonSlots;
        //CreateSummonNameButtons();
        //UpdateStats();
        //UpdateDisplayStats();

        //if (EditableSummon.Type == EnemyType.NORMAL || EditableSummon.Type == EnemyType.BABY)
        //    typeMultiplier = 1f;
        //if (EditableSummon.Type == EnemyType.MUTANT)
        //    typeMultiplier = 1.5f;

        //hpPerStr = Mathf.Round(EditableSummon.Stats.hpPerStr * typeMultiplier);
        //atkPerStr = Mathf.Round(EditableSummon.Stats.atkPerStr * typeMultiplier);
        //mpPerInt = Mathf.Round(EditableSummon.Stats.mpPerInt * typeMultiplier);
        //atkPerInt = Mathf.Round(EditableSummon.Stats.atkPerInt * typeMultiplier);
        //spdPerAgi = Mathf.Round(EditableSummon.Stats.spdPerAgi * typeMultiplier);
        //dodgePerAgi = Mathf.Round(EditableSummon.Stats.dodgePerAgi * typeMultiplier);
        //hitPerDex = Mathf.Round(EditableSummon.Stats.hitPerDex * typeMultiplier);
        //atkPerDex = Mathf.Round(EditableSummon.Stats.atkPerDex * typeMultiplier);
        //hpPerSta = Mathf.Round(EditableSummon.Stats.hpPerSta * typeMultiplier);
        //defPerSta = Mathf.Round(EditableSummon.Stats.defPerSta * typeMultiplier);
        //matkPerInt = Mathf.Round(EditableSummon.Stats.matkPerInt * typeMultiplier);
        //matkPerStr = Mathf.Round(EditableSummon.Stats.matkPerStr * typeMultiplier);
        Refresh();
    }

    public void CreateSummonNameButtons()
    {
        for (int i = 0; i < SummonList.Count; i++)
        {
            GameObject Btn = Instantiate(button);
            Btn.GetComponentInChildren<TextMeshProUGUI>().text = SummonList[i].Stats.displayName;
            if(i == 0)
            {
                Btn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            }
            Btn.GetComponent<SummonOrderInList>().listOrder = i;
            Btn.GetComponent<SummonOrderInList>().summonInterface = this;
            Btn.transform.SetParent(SummonListDisplay, false);
            buttonObject.Add(Btn);
        }
    }

    //
    public void Buttons()
    {
        //makeMainButton.GetComponent<Button>().interactable = false;
        //restButton.GetComponent<Button>().interactable = false;
        //expellButton.GetComponent<Button>().interactable = false;
        if (EditableSummon != null)
        {
            if (EditableSummon.active)
            {
                makeMainButton.GetComponent<Button>().interactable = false;
                restButton.GetComponent<Button>().interactable = true;
                expellButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                makeMainButton.GetComponent<Button>().interactable = true;
                restButton.GetComponent<Button>().interactable = false;
                expellButton.GetComponent<Button>().interactable = true;
            }
        }

        if (SummonList.Count == 0 || EditableSummon == null)
        {
            makeMainButton.GetComponent<Button>().interactable = false;
            restButton.GetComponent<Button>().interactable = false;
            expellButton.GetComponent<Button>().interactable = false;
        }
        
        //if (!EditableSummon.isDeployable)
        //{
        //    makeMainButton.GetComponent<Button>().interactable = false;
        //    restButton.GetComponent<Button>().interactable = false;
        //    expellButton.GetComponent<Button>().interactable = true;
        //}

    }

    //Set the character to work with

    public void EnableSummonEditing(int a)
    {
        for (int i = 0; i < buttonObject.Count; i++)
        {
            if(i != a)
            buttonObject[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 203, 25, 255);
        }
        EditableSummon = SummonList[a];
        Clean();
        summonSkillPanel.ShowSummonSkills(EditableSummon);
        Buttons();
        SetColorCodes();
        //UpdateAvatar();
    }

    //Code related to adding / removing / setting and displaying the character stats

    public void IncrStr()
    {
        if (curStatpts > 0)
        {
            AddedStr++;
            curStatpts--;
            addedStatpts++;
            CalculateStatBonus();
            UpdateDisplayStats();
        }
        else
        {
            GameManager.instance.Chat.AddToChatOutput("<#de0404>You don't have enough statpoints to distribute!</color>");
        }
    }

    public void DecrStr()
    {
        if(AddedStr > 0)
        {
            AddedStr--;
            curStatpts++;
            addedStatpts--;
            CalculateStatBonus();
            UpdateDisplayStats();
        }
        else
        {
            GameManager.instance.Chat.AddToChatOutput("<#de0404>You can't decrease your stat any lower!</color>");
        }

    }


    public void IncrInt()
    {
        if (curStatpts > 0)
        {
            AddedInt++;
            curStatpts--;
            addedStatpts++;
            CalculateStatBonus();
            UpdateDisplayStats();
        }
        else
        {
            GameManager.instance.Chat.AddToChatOutput("<#de0404>You don't have enough statpoints to distribute!</color>");
        }
    }

    public void DecrInt()
    {
        if (AddedInt > 0)
        {
            AddedInt--;
            curStatpts++;
            addedStatpts--;
            CalculateStatBonus();
            UpdateDisplayStats();
        }
        else
        {
            GameManager.instance.Chat.AddToChatOutput("<#de0404>You can't decrease your stat any lower!</color>");
        }
    }


    public void IncrDex()
    {
        if (curStatpts > 0)
        {
            AddedDex++;
            curStatpts--;
            addedStatpts++;
            CalculateStatBonus();
            UpdateDisplayStats();
        }
        else
        {
            GameManager.instance.Chat.AddToChatOutput("<#de0404>You don't have enough statpoints to distribute!</color>");
        }
    }

    public void DecrDex()
    {
        if (AddedDex > 0)
        {
            AddedDex--;
            curStatpts++;
            addedStatpts--;
            CalculateStatBonus();
            UpdateDisplayStats();
        }
        else
        {
            GameManager.instance.Chat.AddToChatOutput("<#de0404>You can't decrease your stat any lower!</color>");
        }
    }

    public void IncrAgi()
    {
        if (curStatpts > 0)
        {
            AddedAgi++;
            curStatpts--;
            addedStatpts++;
            CalculateStatBonus();
            UpdateDisplayStats();
        }
        else
        {
            GameManager.instance.Chat.AddToChatOutput("<#de0404>You don't have enough statpoints to distribute!</color>");
        }
    }

    public void DecrAgi()
    {
        if (AddedAgi > 0)
        {
            AddedAgi--;
            curStatpts++;
            addedStatpts--;
            CalculateStatBonus();
            UpdateDisplayStats();
        }
        else
        {
            GameManager.instance.Chat.AddToChatOutput("<#de0404>You can't decrease your stat any lower!</color>");
        }
    }

    public void IncrSta()
    {
        if (curStatpts > 0)
        {
            AddedSta++;
            curStatpts--;
            addedStatpts++;
            CalculateStatBonus();
            UpdateDisplayStats();
        }
        else
        {
            GameManager.instance.Chat.AddToChatOutput("<#de0404>You don't have enough statpoints to distribute!</color>");
        }
    }

    public void DecrSta()
    {
        if (AddedSta > 0)
        {
            AddedSta--;
            curStatpts++;
            addedStatpts--;
            CalculateStatBonus();
            UpdateDisplayStats();
        }
        else
        {
            GameManager.instance.Chat.AddToChatOutput("<#de0404>You can't decrease your stat any lower!</color>");
        }
    }

    public void UpdateStats()
    {
        summonSlots.text = SummonList.Count.ToString() + "/" + MaxSummonSlots.ToString();
        if (EditableSummon != null)
        {
            cover.SetActive(false);
            curStatpts = EditableSummon.Stats.unspentStatPoints;

            summonType.text = EditableSummon.Type.ToString();
            if (EditableSummon.Type == EnemyType.NORMAL)
            {
                summonType.text = "WILD";
                summonType.color = Color.white;
            }
            if (EditableSummon.Type == EnemyType.BABY)
            {
                summonType.color = Color.magenta;
            }
            if (EditableSummon.Type == EnemyType.MUTANT)
            {
                summonType.color = new Color32(155, 0, 155, 255); // purple
            }
            SummonName.text = EditableSummon.Stats.displayName.ToString();
            summonLevel.text = EditableSummon.Level.currentlevel.ToString();

            summonHP.text = EditableSummon.Stats.HP.CurValue.ToString() + "/" + EditableSummon.Stats.HP.MaxValue.ToString();
            summonMP.text = EditableSummon.Stats.MP.CurValue.ToString() + "/" + EditableSummon.Stats.MP.MaxValue.ToString();

            summonAtk.text = EditableSummon.Stats.ATK.CurValue.ToString();
            summonMatk.text = EditableSummon.Stats.MATK.CurValue.ToString();
            summonDef.text = EditableSummon.Stats.DEF.CurValue.ToString();

            summonDodge.text = EditableSummon.Stats.Dodge.CurValue.ToString();
            summonHit.text = EditableSummon.Stats.Hit.CurValue.ToString();
            summonCrit.text = EditableSummon.Stats.curCRIT.ToString();
            summonSpeed.text = EditableSummon.Stats.Speed.CurValue.ToString();

            summonStr.text = EditableSummon.Stats.strength.BaseValue.ToString();
            summonInt.text = EditableSummon.Stats.intellect.BaseValue.ToString();
            summonDex.text = EditableSummon.Stats.dexterity.BaseValue.ToString();
            summonAgi.text = EditableSummon.Stats.agility.BaseValue.ToString();
            summonSta.text = EditableSummon.Stats.stamina.BaseValue.ToString();

            summonStatpts.text = EditableSummon.Stats.unspentStatPoints.ToString();
            
            summonExp.text = EditableSummon.Level.CUR_EXP.ToString() + "/" + EditableSummon.Level.NEXT_EXP.ToString();
            UpdateAvatar();
        }
        else
        {
            ShowNothing();
        }
    }

    void UpdateAvatar()
    {
        if (myAv != null)
        {
            Destroy(myAv);
        }
        GameObject summonModel = Extensions.FindModelPrefab(EditableSummon.BaseID, false);
        myAv = Instantiate(summonModel, SummonPicture.position, Quaternion.Euler(0, 160, 0), SummonPicture);
        Extensions.SetLayer(myAv, 5);
    }

    void DestroyAvatar()
    {
        if (myAv != null)
            Destroy(myAv);
    }

    public void UpdateDisplayStats()
    {
        if (addedHP > 0)
            summonHPpre.text = "+" + addedHP.ToString();
        else
            summonHPpre.text = "";

        if (addedMP > 0)
            summonMPpre.text = "+" + addedMP.ToString();
        else
            summonMPpre.text = "";

        if (addedAtk > 0)
            summonAtkpre.text = "+" + addedAtk.ToString();
        else
            summonAtkpre.text = "";

        if (addedMatk > 0)
            summonMatkpre.text = "+" + addedMatk.ToString();
        else
            summonMatkpre.text = "";

        if (addedDef > 0)
            summonDefpre.text = "+" + addedDef.ToString();
        else
            summonDefpre.text = "";

        if (addedDodge > 0)
            summonDodgepre.text = "+" + addedDodge.ToString();
        else
            summonDodgepre.text = "";

        if (addedHit > 0)
            summonHitpre.text = "+" + addedHit.ToString();
        else
            summonHitpre.text = "";

        if (addedCrit > 0)
            summonCritpre.text = "+" + addedCrit.ToString();
        else
            summonCritpre.text = "";

        if (addedSpeed > 0)
            summonSpeedpre.text = "+" + addedSpeed.ToString();
        else
            summonSpeedpre.text = "";

        if (AddedStr > 0)
            summonStrpre.text = "+" + AddedStr.ToString();
        else
            summonStrpre.text = "";

        if (AddedInt > 0)
            summonIntpre.text = "+" + AddedInt.ToString();
        else
            summonIntpre.text = "";

        if (AddedDex > 0)
            summonDexpre.text = "+" + AddedDex.ToString();
        else
            summonDexpre.text = "";

        if (AddedAgi > 0)
            summonAgipre.text = "+" + AddedAgi.ToString();
        else
            summonAgipre.text = "";

        if (AddedSta > 0)
            summonStapre.text = "+" + AddedSta.ToString();
        else
            summonStapre.text = "";

        if (addedStatpts > 0)
            summonStatptsPre.text = "-" + addedStatpts.ToString();
        else
            summonStatptsPre.text = "";

    }

    public void FireStatIncrease()
    {
        if (EditableSummon != null)
        {
            EditableSummon.Stats.strength.BaseValue += AddedStr;
            EditableSummon.Stats.strength.CurValue += AddedStr;
            EditableSummon.Stats.strength.MaxValue += AddedStr;
            EditableSummon.Stats.intellect.BaseValue += AddedInt;
            EditableSummon.Stats.intellect.CurValue += AddedInt;
            EditableSummon.Stats.intellect.MaxValue += AddedInt;
            EditableSummon.Stats.dexterity.BaseValue += AddedDex;
            EditableSummon.Stats.dexterity.CurValue += AddedDex;
            EditableSummon.Stats.dexterity.MaxValue += AddedDex;
            EditableSummon.Stats.agility.BaseValue += AddedAgi;
            EditableSummon.Stats.agility.CurValue += AddedAgi;
            EditableSummon.Stats.agility.MaxValue += AddedAgi;
            EditableSummon.Stats.stamina.BaseValue += AddedSta;
            EditableSummon.Stats.stamina.CurValue += AddedSta;
            EditableSummon.Stats.stamina.MaxValue += AddedSta;

            EditableSummon.Stats.unspentStatPoints -= addedStatpts;

            EditableSummon.Stats.HP.MaxValue += addedHP;
            EditableSummon.Stats.HP.BaseValue += addedHP;
            EditableSummon.Stats.HP.CurValue = EditableSummon.Stats.HP.MaxValue;

            EditableSummon.Stats.MP.MaxValue += addedMP;
            EditableSummon.Stats.MP.BaseValue += addedMP;
            EditableSummon.Stats.MP.CurValue = EditableSummon.Stats.MP.MaxValue;

            EditableSummon.Stats.ATK.BaseValue += addedAtk;
            EditableSummon.Stats.ATK.MaxValue += addedAtk;
            EditableSummon.Stats.ATK.CurValue = EditableSummon.Stats.ATK.MaxValue;

            EditableSummon.Stats.MATK.BaseValue += addedMatk;
            EditableSummon.Stats.MATK.MaxValue += addedMatk;
            EditableSummon.Stats.MATK.CurValue = EditableSummon.Stats.MATK.MaxValue;

            EditableSummon.Stats.DEF.BaseValue += addedDef;
            EditableSummon.Stats.DEF.MaxValue += addedDef;
            EditableSummon.Stats.DEF.CurValue = EditableSummon.Stats.DEF.MaxValue;

            EditableSummon.Stats.baseCRIT += addedCrit;
            EditableSummon.Stats.curCRIT = EditableSummon.Stats.baseCRIT;

            EditableSummon.Stats.Dodge.BaseValue += addedDodge;
            EditableSummon.Stats.Dodge.MaxValue += addedDodge;
            EditableSummon.Stats.Dodge.CurValue = EditableSummon.Stats.Dodge.MaxValue;

            EditableSummon.Stats.Hit.BaseValue += addedHit;
            EditableSummon.Stats.Hit.MaxValue += addedHit;
            EditableSummon.Stats.Hit.CurValue = EditableSummon.Stats.Hit.MaxValue;

            EditableSummon.Stats.Speed.BaseValue += addedSpeed;
            EditableSummon.Stats.Speed.MaxValue += addedSpeed;
            EditableSummon.Stats.Speed.CurValue = EditableSummon.Stats.Speed.MaxValue;

            //EditableSummon.Stats.minATK = EditableSummon.Stats.ATK.CurValue;
            //EditableSummon.Stats.maxATK = Mathf.Round((EditableSummon.Stats.minATK / 100) * 120);

            //EditableSummon.Stats.strength.BaseValue += AddedStr;
            //EditableSummon.Stats.intellect.BaseValue += AddedInt;
            //EditableSummon.Stats.dexterity.BaseValue += AddedDex;
            //EditableSummon.Stats.agility.BaseValue += AddedAgi;
            //EditableSummon.Stats.stamina.BaseValue += AddedSta;

            //EditableSummon.Stats.unspentStatPoints -= addedStatpts;

            //EditableSummon.Stats.HP.MaxValue += addedHP;
            //EditableSummon.Stats.HP.CurValue = EditableSummon.Stats.HP.MaxValue;
            //EditableSummon.Stats.MP.MaxValue += addedMP;
            //EditableSummon.Stats.MP.CurValue = EditableSummon.Stats.MP.MaxValue;


            //EditableSummon.Stats.ATK.BaseValue += addedAtk;
            //EditableSummon.Stats.ATK.CurValue = EditableSummon.Stats.ATK.BaseValue;
            //EditableSummon.Stats.MATK.BaseValue += addedMatk;
            //EditableSummon.Stats.MATK.CurValue = EditableSummon.Stats.MATK.BaseValue;
            //EditableSummon.Stats.DEF.BaseValue += addedDef;
            //EditableSummon.Stats.DEF.CurValue = EditableSummon.Stats.DEF.BaseValue;

            //EditableSummon.Stats.baseCRIT += addedCrit;
            //EditableSummon.Stats.curCRIT = EditableSummon.Stats.baseCRIT;
            //EditableSummon.Stats.Dodge.BaseValue += addedDodge;
            //EditableSummon.Stats.Dodge.CurValue = EditableSummon.Stats.Dodge.BaseValue;
            //EditableSummon.Stats.Hit.BaseValue += addedHit;
            //EditableSummon.Stats.Hit.CurValue = EditableSummon.Stats.Hit.BaseValue;
            //EditableSummon.Stats.Speed.BaseValue += addedSpeed;
            //EditableSummon.Stats.Speed.CurValue = EditableSummon.Stats.Speed.BaseValue;
            //EditableSummon.Stats.minATK = EditableSummon.Stats.ATK.CurValue;
            //EditableSummon.Stats.maxATK = (EditableSummon.Stats.minATK / 100) * 120;

            Actions.OnMainHeroSummonChange(Owner, EditableSummon);

            //owner.GetComponent<SummonHandler>().SavePetData(owner.GetComponent<UnitAttributes>().Stats.displayName);

            Clean();
        }

    }

    public void Clean()
    {
        AddedStr = 0;
        AddedInt = 0;
        AddedDex = 0;
        AddedAgi = 0;
        AddedSta = 0;

        addedStatpts = 0;
        addedHP = 0;
        addedMP = 0;
        addedAtk = 0;
        addedMatk = 0;
        addedDef = 0;
        addedCrit = 0;
        addedDodge = 0;
        addedHit = 0;
        addedSpeed = 0;

        DestroyAvatar();
        UpdateStats();
        UpdateDisplayStats();

    }

    public void ShowNothing()
    {
        foreach (var item in textItems)
        {
            item.text = "";
        }
        DestroyAvatar();
        summonType.text = "";
        summonSkillPanel.ShowSummonSkills(EditableSummon);
        cover.SetActive(true);
    }

    void CalculateStatBonus()
    {
        //Calculate HP based on Stats
        addedHP = Mathf.Round(AddedStr * hpPerStr) + (AddedSta * hpPerSta);
        //Calculate MP based on stats
        addedMP = Mathf.Round(AddedInt * mpPerInt);
        //Calculate Attack based on stats
        addedAtk = Mathf.Round((AddedStr * atkPerStr) + (AddedInt * atkPerInt) + (AddedDex * atkPerDex));
        //Calculate magic Attack based on stats
        addedMatk = Mathf.Round((AddedStr * matkPerStr) + (AddedInt * matkPerInt));
        //Calculate HIT based on stats
        addedHit = Mathf.Round(AddedDex * hitPerDex);
        //Calculate dodge based on stats
        addedDodge = Mathf.Round(AddedAgi * dodgePerAgi);
        //calculate def based on stats
        addedDef = Mathf.Round((AddedSta * defPerSta));
        //calculate critrate based on stats
        addedCrit = 0;
        //calculate speed based on stats
        addedSpeed = Mathf.Round(AddedAgi * spdPerAgi);
    }

    public void CalculateBonus(Summon summon)
    {
        SetMultipliers(summon);
        var Str = 1;
        var Int = 1;
        var Dex = 1;
        var Agi = 1;
        var Sta = 1;
        //Calculate HP based on Stats
        addedHP = Mathf.Round(Str * hpPerStr) + (Sta * hpPerSta);
        //Calculate MP based on stats
        addedMP = Mathf.Round(Int * mpPerInt);
        //Calculate Attack based on stats
        addedAtk = Mathf.Round((Str * atkPerStr) + (Int * atkPerInt) + (AddedDex * atkPerDex));
        //Calculate magic Attack based on stats
        addedMatk = Mathf.Round((Str * matkPerStr) + (Int * matkPerInt));
        //Calculate HIT based on stats
        addedHit = Mathf.Round(Dex * hitPerDex);
        //Calculate dodge based on stats
        addedDodge = Mathf.Round(Agi * dodgePerAgi);
        //calculate def based on stats
        addedDef = Mathf.Round((Sta * defPerSta));
        //calculate critrate based on stats
        addedCrit = 0;
        //calculate speed based on stats
        addedSpeed = Mathf.Round(Agi * spdPerAgi);
        //
        AddedStr = Str;
        AddedInt = Int;
        AddedDex = Dex;
        AddedAgi = Agi;
        AddedSta = Sta;
        //
        ExecuteStatIncrease(summon);
    }

    void ExecuteStatIncrease(Summon summon)
    {
        if (summon != null)
        {
            //summon.unit.Stats.strength.BaseValue += AddedStr;
            //summon.unit.Stats.intellect.BaseValue += AddedInt;
            //summon.unit.Stats.dexterity.BaseValue += AddedDex;
            //summon.unit.Stats.agility.BaseValue += AddedAgi;
            //summon.unit.Stats.stamina.BaseValue += AddedSta;
            //summon.unit.Stats.HP.MaxValue += addedHP;
            //summon.unit.Stats.HP.CurValue = summon.unit.Stats.HP.MaxValue;
            //summon.unit.Stats.MP.MaxValue += addedMP;
            //summon.unit.Stats.MP.CurValue = summon.unit.Stats.MP.MaxValue;
            //summon.unit.Stats.ATK.BaseValue += addedAtk;
            //summon.unit.Stats.ATK.CurValue = summon.unit.Stats.ATK.BaseValue;
            //summon.unit.Stats.MATK.BaseValue += addedMatk;
            //summon.unit.Stats.MATK.CurValue = summon.unit.Stats.MATK.BaseValue;
            //summon.unit.Stats.DEF.BaseValue += addedDef;
            //summon.unit.Stats.DEF.CurValue = summon.unit.Stats.DEF.BaseValue;
            //summon.unit.Stats.baseCRIT += addedCrit;
            //summon.unit.Stats.curCRIT = summon.unit.Stats.baseCRIT;
            //summon.unit.Stats.Dodge.BaseValue += addedDodge;
            //summon.unit.Stats.Dodge.CurValue = summon.unit.Stats.Dodge.BaseValue;
            //summon.unit.Stats.Hit.BaseValue += addedHit;
            //summon.unit.Stats.Hit.CurValue = summon.unit.Stats.Hit.BaseValue;
            //summon.unit.Stats.Speed.BaseValue += addedSpeed;
            //summon.unit.Stats.Speed.CurValue = summon.unit.Stats.Speed.BaseValue;
            //summon.unit.Stats.minATK = summon.unit.Stats.ATK.CurValue;
            //summon.unit.Stats.maxATK = Mathf.Round((summon.unit.Stats.minATK / 100) * 120);

            summon.unit.Stats.strength.BaseValue += AddedStr;
            summon.unit.Stats.strength.CurValue += AddedStr;
            summon.unit.Stats.strength.MaxValue += AddedStr;
            summon.unit.Stats.intellect.BaseValue += AddedInt;
            summon.unit.Stats.intellect.CurValue += AddedInt;
            summon.unit.Stats.intellect.MaxValue += AddedInt;
            summon.unit.Stats.dexterity.BaseValue += AddedDex;
            summon.unit.Stats.dexterity.CurValue += AddedDex;
            summon.unit.Stats.dexterity.MaxValue += AddedDex;
            summon.unit.Stats.agility.BaseValue += AddedAgi;
            summon.unit.Stats.agility.CurValue += AddedAgi;
            summon.unit.Stats.agility.MaxValue += AddedAgi;
            summon.unit.Stats.stamina.BaseValue += AddedSta;
            summon.unit.Stats.stamina.CurValue += AddedSta;
            summon.unit.Stats.stamina.MaxValue += AddedSta;



            summon.unit.Stats.HP.MaxValue += addedHP;
            summon.unit.Stats.HP.BaseValue += addedHP;
            summon.unit.Stats.HP.CurValue = summon.unit.Stats.HP.MaxValue;

            summon.unit.Stats.MP.MaxValue += addedMP;
            summon.unit.Stats.MP.BaseValue += addedMP;
            summon.unit.Stats.MP.CurValue = summon.unit.Stats.MP.MaxValue;

            summon.unit.Stats.ATK.BaseValue += addedAtk;
            summon.unit.Stats.ATK.MaxValue += addedAtk;
            summon.unit.Stats.ATK.CurValue = summon.unit.Stats.ATK.MaxValue;

            summon.unit.Stats.MATK.BaseValue += addedMatk;
            summon.unit.Stats.MATK.MaxValue += addedMatk;
            summon.unit.Stats.MATK.CurValue = summon.unit.Stats.MATK.MaxValue;

            summon.unit.Stats.DEF.BaseValue += addedDef;
            summon.unit.Stats.DEF.MaxValue += addedDef;
            summon.unit.Stats.DEF.CurValue = summon.unit.Stats.DEF.MaxValue;

            summon.unit.Stats.baseCRIT += addedCrit;
            summon.unit.Stats.curCRIT = summon.unit.Stats.baseCRIT;

            summon.unit.Stats.Dodge.BaseValue += addedDodge;
            summon.unit.Stats.Dodge.MaxValue += addedDodge;
            summon.unit.Stats.Dodge.CurValue = summon.unit.Stats.Dodge.MaxValue;

            summon.unit.Stats.Hit.BaseValue += addedHit;
            summon.unit.Stats.Hit.MaxValue += addedHit;
            summon.unit.Stats.Hit.CurValue = summon.unit.Stats.Hit.MaxValue;

            summon.unit.Stats.Speed.BaseValue += addedSpeed;
            summon.unit.Stats.Speed.MaxValue += addedSpeed;
            summon.unit.Stats.Speed.CurValue = summon.unit.Stats.Speed.MaxValue;

            //summon.unit.Stats.minATK = summon.unit.Stats.ATK.CurValue;
            //summon.unit.Stats.maxATK = Mathf.Round((summon.unit.Stats.minATK / 100) * 120);

            //string mainHeroName = HeroDataManager.instance.CharacterInfo.FirstOrDefault(hero => hero.isMainCharacter)?.Name;
            //if (summon.ownerName == mainHeroName)
            //{
            //    int index = HeroDataManager.instance.CharacterInfo.FindIndex(hero => hero.Name == summon.ownerName);
            //    Debug.Log("index" + index.ToString());
            //    var entrya = Extensions.FindSummonEntry(summon.uniqueID, index);
            //    Debug.Log("entrya" + entrya.Stats.displayName.ToString());
            //    var _onwer = HeroDataManager.instance.CharacterInfo.FirstOrDefault(hero => hero.isMainCharacter);
            //    Debug.Log("entrya" + _onwer.Stats.displayName.ToString());
            //    //Actions.OnMainHeroSummonChange(_onwer, entrya);
            //}

            AddedStr = 0;
            AddedInt = 0;
            AddedDex = 0;
            AddedAgi = 0;
            AddedSta = 0;

            addedStatpts = 0;
            addedHP = 0;
            addedMP = 0;
            addedAtk = 0;
            addedMatk = 0;
            addedDef = 0;
            addedCrit = 0;
            addedDodge = 0;
            addedHit = 0;
            addedSpeed = 0;
        }
    }

    public void SetAsActive()
    {
        foreach (CapturedPets summon in SummonList)
        {
            summon.active = false;
            summon.isDeployable = true;
        }
        EditableSummon.active = true;
        EditableSummon.isDeployable = false;
        //owner.GetComponent<SummonHandler>().SavePetData(owner.GetComponent<UnitAttributes>().Stats.displayName);
        //Owner.SummonList[]; //HeroData.CharacterInfo
        GameManager.instance.Chat.AddToChatOutput("<" + typeColorCode + ">" + EditableSummon.Stats.displayName + "</color> is set to active!");
        Actions.OnMainHeroSummonChange(Owner, EditableSummon);
        
        Buttons();
    }

    public void SetToRest()
    {
        foreach (CapturedPets summon in SummonList)
        {
            summon.active = false;
            summon.isDeployable = true;
        }
        GameManager.instance.Chat.AddToChatOutput("<" + typeColorCode + ">" + EditableSummon.Stats.displayName + "</color> is set to rest!");
        Actions.OnMainHeroSummonChange(Owner, EditableSummon);

        Buttons();
    }

    public void Expell()
    {
        if (!EditableSummon.active)
        {
            GameManager.instance.Chat.AddToChatOutput("<" + typeColorCode + ">" + EditableSummon.Stats.displayName + "</color> were expelled!");
            SummonList.Remove(EditableSummon);
            Extensions.DestroyAllChildren(SummonListDisplay);
            buttonObject.Clear();
            if (Owner.SummonList.Count > 0)
                EditableSummon = SummonList[0];
            else
                EditableSummon = null;
            CreateSummonNameButtons();
            UpdateStats();
            UpdateDisplayStats();
            Buttons();
            
        }
        else
        {
            GameManager.instance.Chat.AddToChatOutput("<#de0404>You can't expell active summon!</color>");
        }
        //owner.GetComponent<SummonHandler>().SavePetData(owner.GetComponent<UnitAttributes>().Stats.displayName);
    }

    public void Refresh()
    {
        Extensions.DestroyAllChildren(SummonListDisplay);
        buttonObject.Clear();
        SummonList = Owner.SummonList;
        if (SummonList.Count > 0)
        {
            EditableSummon = SummonList[0];
        }
        else
        {
            EditableSummon = null;
        }
        UpdateStats();
        CreateSummonNameButtons();
        MaxSummonSlots = Owner.MaxSummonSlots;
        UpdateDisplayStats();
        Buttons();
        if (EditableSummon != null)
        {
            summonSkillPanel.ShowSummonSkills(EditableSummon);
            if (EditableSummon.Type == EnemyType.NORMAL)
            {
                typeMultiplier = HeroDataManager.instance.UnitDatabase.NormalSummonStatMultiplier;
                typeColorCode = "#FFFFFF";
            }

            if (EditableSummon.Type == EnemyType.BABY)
            {
                typeMultiplier = HeroDataManager.instance.UnitDatabase.BabySummonStatMultiplier;
                typeColorCode = "#ff03ff";
            }

            if (EditableSummon.Type == EnemyType.MUTANT)
            {
                typeMultiplier = HeroDataManager.instance.UnitDatabase.MutantSummonStatMultiplier;
                typeColorCode = "#9B009B";
            }
                
            ExecuteMultipliers(typeMultiplier);
        }

    }

    void SetColorCodes()
    {
        if (EditableSummon.Type == EnemyType.NORMAL)
        {
            typeColorCode = "#FFFFFF";
        }

        if (EditableSummon.Type == EnemyType.BABY)
        {
            typeColorCode = "#ff03ff";
        }

        if (EditableSummon.Type == EnemyType.MUTANT)
        {
            typeColorCode = "#9B009B";
        }
    }

    void SetMultipliers(Summon summon)
    {
        if (summon.summonType == EnemyType.NORMAL)
        {
            typeMultiplier = HeroDataManager.instance.UnitDatabase.NormalSummonStatMultiplier;
            typeColorCode = "#FFFFFF";
        }

        if (summon.summonType == EnemyType.BABY)
        {
            typeMultiplier = HeroDataManager.instance.UnitDatabase.BabySummonStatMultiplier;
            typeColorCode = "#ff03ff";
        }

        if (summon.summonType == EnemyType.MUTANT)
        {
            typeMultiplier = HeroDataManager.instance.UnitDatabase.MutantSummonStatMultiplier;
            typeColorCode = "#9B009B";
        }

        ExecuteMultipliers(typeMultiplier);
    }

    void ExecuteMultipliers(float typeMultiplier)
    {
         hpPerStr = Mathf.Round(HeroDataManager.instance.UnitDatabase.hpPerStr * typeMultiplier);
        atkPerStr = Mathf.Round(HeroDataManager.instance.UnitDatabase.atkPerStr * typeMultiplier);
         mpPerInt = Mathf.Round(HeroDataManager.instance.UnitDatabase.mpPerInt * typeMultiplier);
        atkPerInt = Mathf.Round(HeroDataManager.instance.UnitDatabase.atkPerInt * typeMultiplier);
        spdPerAgi = Mathf.Round(HeroDataManager.instance.UnitDatabase.spdPerAgi * typeMultiplier);
      dodgePerAgi = Mathf.Round(HeroDataManager.instance.UnitDatabase.dodgePerAgi * typeMultiplier);
        hitPerDex = Mathf.Round(HeroDataManager.instance.UnitDatabase.hitPerDex * typeMultiplier);
        atkPerDex = Mathf.Round(HeroDataManager.instance.UnitDatabase.atkPerDex * typeMultiplier);
         hpPerSta = Mathf.Round(HeroDataManager.instance.UnitDatabase.hpPerSta * typeMultiplier);
        defPerSta = Mathf.Round(HeroDataManager.instance.UnitDatabase.defPerSta * typeMultiplier);
       matkPerInt = Mathf.Round(HeroDataManager.instance.UnitDatabase.matkPerInt * typeMultiplier);
       matkPerStr = Mathf.Round(HeroDataManager.instance.UnitDatabase.matkPerStr * typeMultiplier);
    }

}
