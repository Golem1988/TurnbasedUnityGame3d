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
            GameManager.instance.Chat.AddToChatOutput("<#de0404>You don't have enough statpoints to destribute!</color>");
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
            GameManager.instance.Chat.AddToChatOutput("<#de0404>You don't have enough statpoints to destribute!</color>");
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
            GameManager.instance.Chat.AddToChatOutput("<#de0404>You don't have enough statpoints to destribute!</color>");
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
            GameManager.instance.Chat.AddToChatOutput("<#de0404>You don't have enough statpoints to destribute!</color>");
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
            GameManager.instance.Chat.AddToChatOutput("<#de0404>You don't have enough statpoints to destribute!</color>");
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

            summonHP.text = EditableSummon.Stats.curHP.ToString() + "/" + EditableSummon.Stats.baseHP.ToString();
            summonMP.text = EditableSummon.Stats.curMP.ToString() + "/" + EditableSummon.Stats.baseMP.ToString();

            summonAtk.text = EditableSummon.Stats.curATK.ToString();
            summonMatk.text = EditableSummon.Stats.curMATK.ToString();
            summonDef.text = EditableSummon.Stats.curDEF.ToString();

            summonDodge.text = EditableSummon.Stats.curDodge.ToString();
            summonHit.text = EditableSummon.Stats.curHit.ToString();
            summonCrit.text = EditableSummon.Stats.curCRIT.ToString();
            summonSpeed.text = EditableSummon.Stats.curSpeed.ToString();

            summonStr.text = EditableSummon.Stats.strength.BaseValue.ToString();
            summonInt.text = EditableSummon.Stats.intellect.BaseValue.ToString();
            summonDex.text = EditableSummon.Stats.dexterity.BaseValue.ToString();
            summonAgi.text = EditableSummon.Stats.agility.BaseValue.ToString();
            summonSta.text = EditableSummon.Stats.stamina.BaseValue.ToString();

            summonStatpts.text = EditableSummon.Stats.unspentStatPoints.ToString();

            summonExp.text = EditableSummon.Level.experience.ToString() + "/" + EditableSummon.Level.requiredExp.ToString();
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
        //Vector3 pos = new Vector3(2f, -91.7f, -45f);
        //GameObject avatar = EditableSummon.GetComponent<UnitUI>().heroAvatar;
        myAv = Instantiate(summonModel, SummonPicture.position, Quaternion.Euler(0, 180, 0), SummonPicture);
        Extensions.SetLayer(myAv, 5);
        //myAv.transform.localScale = new Vector3(150f, 150f, 150f);
        //myAv.transform.SetParent(SummonPicture, false);
        //myAv.transform.
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
            EditableSummon.Stats.intellect.BaseValue += AddedInt;
            EditableSummon.Stats.dexterity.BaseValue += AddedDex;
            EditableSummon.Stats.agility.BaseValue += AddedAgi;
            EditableSummon.Stats.stamina.BaseValue += AddedSta;

            EditableSummon.Stats.unspentStatPoints -= addedStatpts;

            EditableSummon.Stats.baseHP += addedHP;
            EditableSummon.Stats.curHP = EditableSummon.Stats.baseHP;
            EditableSummon.Stats.baseMP += addedMP;
            EditableSummon.Stats.curMP = EditableSummon.Stats.baseMP;


            EditableSummon.Stats.baseATK += addedAtk;
            EditableSummon.Stats.curATK = EditableSummon.Stats.baseATK;
            EditableSummon.Stats.baseMATK += addedMatk;
            EditableSummon.Stats.curMATK = EditableSummon.Stats.baseMATK;
            EditableSummon.Stats.baseDEF += addedDef;
            EditableSummon.Stats.curDEF = EditableSummon.Stats.baseDEF;

            EditableSummon.Stats.baseCRIT += addedCrit;
            EditableSummon.Stats.curCRIT = EditableSummon.Stats.baseCRIT;
            EditableSummon.Stats.baseDodge += addedDodge;
            EditableSummon.Stats.curDodge = EditableSummon.Stats.baseDodge;
            EditableSummon.Stats.baseHit += addedHit;
            EditableSummon.Stats.curHit = EditableSummon.Stats.baseHit;
            EditableSummon.Stats.baseSpeed += addedSpeed;
            EditableSummon.Stats.curSpeed = EditableSummon.Stats.baseSpeed;
            EditableSummon.Stats.minATK = EditableSummon.Stats.curATK;
            EditableSummon.Stats.maxATK = (EditableSummon.Stats.minATK / 100) * 120;

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
            summon.unit.Stats.strength.BaseValue += AddedStr;
            summon.unit.Stats.intellect.BaseValue += AddedInt;
            summon.unit.Stats.dexterity.BaseValue += AddedDex;
            summon.unit.Stats.agility.BaseValue += AddedAgi;
            summon.unit.Stats.stamina.BaseValue += AddedSta;
            summon.unit.Stats.baseHP += addedHP;
            summon.unit.Stats.curHP = summon.unit.Stats.baseHP;
            summon.unit.Stats.baseMP += addedMP;
            summon.unit.Stats.curMP = summon.unit.Stats.baseMP;
            summon.unit.Stats.baseATK += addedAtk;
            summon.unit.Stats.curATK = summon.unit.Stats.baseATK;
            summon.unit.Stats.baseMATK += addedMatk;
            summon.unit.Stats.curMATK = summon.unit.Stats.baseMATK;
            summon.unit.Stats.baseDEF += addedDef;
            summon.unit.Stats.curDEF = summon.unit.Stats.baseDEF;
            summon.unit.Stats.baseCRIT += addedCrit;
            summon.unit.Stats.curCRIT = summon.unit.Stats.baseCRIT;
            summon.unit.Stats.baseDodge += addedDodge;
            summon.unit.Stats.curDodge = summon.unit.Stats.baseDodge;
            summon.unit.Stats.baseHit += addedHit;
            summon.unit.Stats.curHit = summon.unit.Stats.baseHit;
            summon.unit.Stats.baseSpeed += addedSpeed;
            summon.unit.Stats.curSpeed = summon.unit.Stats.baseSpeed;
            summon.unit.Stats.minATK = summon.unit.Stats.curATK;
            summon.unit.Stats.maxATK = Mathf.Round((summon.unit.Stats.minATK / 100) * 120);

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
