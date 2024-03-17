using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class HeroInfoInterface : MonoBehaviour
{
    [SerializeField] UnitedInfoPanel mainPanel;
    private GameObject myAv;

    public List<CharacterInformation> HeroList = new List<CharacterInformation>();
    //public List<GameObject> buttonObject = new List<GameObject>();

    [Header("Current hero")]
    public CharacterInformation HeroPrefab;
    public int curStatpts;

    //Main information and stat display
    [Header("Display Information")]

    public TMP_Text heroName;
    public TMP_Text heroLevel;

    public TMP_Text heroHP;
    public TMP_Text heroMP;
    public TMP_Text heroAtk;
    public TMP_Text heroMatk;
    public TMP_Text heroDef;

    public TMP_Text heroDodge;
    public TMP_Text heroHit;
    public TMP_Text heroCrit;
    public TMP_Text heroSpeed;

    public TMP_Text heroStr;
    public TMP_Text heroInt;
    public TMP_Text heroDex;
    public TMP_Text heroAgi;
    public TMP_Text heroSta;

    public TMP_Text heroStatpts;

    public TMP_Text heroLoyalty;
    public TMP_Text heroExp;

    //Stat preview display upon pre-levelling stats
    [Header("Stats preview text")]
    public TMP_Text heroHPpre;
    public TMP_Text heroMPpre;
    public TMP_Text heroAtkpre;
    public TMP_Text heroMatkpre;
    public TMP_Text heroDefpre;

    public TMP_Text heroDodgepre;
    public TMP_Text heroHitpre;
    public TMP_Text heroCritpre;
    public TMP_Text heroSpeedpre;

    public TMP_Text heroStrpre;
    public TMP_Text heroIntpre;
    public TMP_Text heroDexpre;
    public TMP_Text heroAgipre;
    public TMP_Text heroStapre;

    public TMP_Text heroStatptsPre;

    //Variables for stat pre-levelling
    [Header("Will be added")]
    public int AddedStr;
    public int AddedInt;
    public int AddedDex;
    public int AddedAgi;
    public int AddedSta;
    
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

    //[Header("Attribute multipliers")]
    //private float hpPerStr;
    //private float atkPerStr;
    //private float mpPerInt;
    //private float atkPerInt;
    //private float spdPerAgi;
    //private float dodgePerAgi;
    //private float hitPerDex;
    //private float atkPerDex;
    //private float hpPerSta;
    //private float defPerSta;
    //private float matkPerInt;
    //private float matkPerStr;


    void OnDisable()
    {
        Clean();
    }

    private void OnEnable()
    {
        if (mainPanel.summonInterface.gameObject.activeSelf == true && mainPanel.ShowSummonButton.activeSelf == true)
        {
            mainPanel.ShowSummonButton.SetActive(false);
            mainPanel.HideSummonButton.SetActive(true);
        }
        Clean();
    }

    private void Awake()
    {

    }

    private void Start()
    {
        //HeroList = HeroData.instance.CharacterInfo;
        //HeroPrefab = HeroList[0];
        //SummonInfoPanel.Owner = HeroPrefab;
        //CreateCharNameButtons();
        UpdateStats();
        //UpdateDisplayStats();
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
        if (HeroPrefab != null)
        {
            curStatpts = HeroPrefab.Stats.unspentStatPoints;

            //heroSlots.text = HeroList.Count.ToString() + "/" + HeroList.Count.ToString();
            heroName.text = HeroPrefab.Stats.theName;
            heroLevel.text = HeroPrefab.Level.currentlevel.ToString();

            heroHP.text = HeroPrefab.Stats.curHP.ToString() + "/" + HeroPrefab.Stats.baseHP.ToString();
            heroMP.text = HeroPrefab.Stats.curMP.ToString() + "/" + HeroPrefab.Stats.baseMP.ToString();

            heroAtk.text = HeroPrefab.Stats.curATK.ToString();
            heroMatk.text = HeroPrefab.Stats.curMATK.ToString();
            heroDef.text = HeroPrefab.Stats.curDEF.ToString();

            heroDodge.text = HeroPrefab.Stats.curDodge.ToString();
            heroHit.text = HeroPrefab.Stats.curHit.ToString();
            heroCrit.text = HeroPrefab.Stats.curCRIT.ToString();
            heroSpeed.text = HeroPrefab.Stats.curSpeed.ToString();

            heroStr.text = HeroPrefab.Stats.strength.BaseValue.ToString();
            heroInt.text = HeroPrefab.Stats.intellect.BaseValue.ToString();
            heroDex.text = HeroPrefab.Stats.dexterity.BaseValue.ToString();
            heroAgi.text = HeroPrefab.Stats.agility.BaseValue.ToString();
            heroSta.text = HeroPrefab.Stats.stamina.BaseValue.ToString();

            heroStatpts.text = HeroPrefab.Stats.unspentStatPoints.ToString();

            //heroExp.text = HeroPrefab.Level.expNow.ToString() + "/" + HeroPrefab.Level.displayExp.ToString();
        }
    }

    void DestroyAvatar()
    {
        if (myAv != null)
            Destroy(myAv);
    }

    public void UpdateDisplayStats()
    {
        if (addedHP > 0)
            heroHPpre.text = "+" + addedHP.ToString();
        else
            heroHPpre.text = "";

        if (addedMP > 0)
            heroMPpre.text = "+" + addedMP.ToString();
        else
            heroMPpre.text = "";

        if (addedAtk > 0)
            heroAtkpre.text = "+" + addedAtk.ToString();
        else
            heroAtkpre.text = "";

        if (addedMatk > 0)
            heroMatkpre.text = "+" + addedMatk.ToString();
        else
            heroMatkpre.text = "";

        if (addedDef > 0)
            heroDefpre.text = "+" + addedDef.ToString();
        else
            heroDefpre.text = "";

        if (addedDodge > 0)
            heroDodgepre.text = "+" + addedDodge.ToString();
        else
            heroDodgepre.text = "";

        if (addedHit > 0)
            heroHitpre.text = "+" + addedHit.ToString();
        else
            heroHitpre.text = "";

        if (addedCrit > 0)
            heroCritpre.text = "+" + addedCrit.ToString();
        else
            heroCritpre.text = "";

        if (addedSpeed > 0)
            heroSpeedpre.text = "+" + addedSpeed.ToString();
        else
            heroSpeedpre.text = "";

        if (AddedStr > 0)
            heroStrpre.text = "+" + AddedStr.ToString();
        else
            heroStrpre.text = "";

        if (AddedInt > 0)
            heroIntpre.text = "+" + AddedInt.ToString();
        else
            heroIntpre.text = "";

        if (AddedDex > 0)
            heroDexpre.text = "+" + AddedDex.ToString();
        else
            heroDexpre.text = "";

        if (AddedAgi > 0)
            heroAgipre.text = "+" + AddedAgi.ToString();
        else
            heroAgipre.text = "";

        if (AddedSta > 0)
            heroStapre.text = "+" + AddedSta.ToString();
        else
            heroStapre.text = "";

        if (addedStatpts > 0)
            heroStatptsPre.text = "-" + addedStatpts.ToString();
        else
            heroStatptsPre.text = "";

    }

    public void FireStatIncrease()
    {
        if (HeroPrefab != null)
        {
            HeroPrefab.Stats.strength.BaseValue += AddedStr;
            HeroPrefab.Stats.intellect.BaseValue += AddedInt;
            HeroPrefab.Stats.dexterity.BaseValue += AddedDex;
            HeroPrefab.Stats.agility.BaseValue += AddedAgi;
            HeroPrefab.Stats.stamina.BaseValue += AddedSta;

            HeroPrefab.Stats.unspentStatPoints -= addedStatpts;

            HeroPrefab.Stats.baseHP += addedHP;
            HeroPrefab.Stats.curHP = HeroPrefab.Stats.baseHP;
            HeroPrefab.Stats.baseMP += addedMP;
            HeroPrefab.Stats.curMP = HeroPrefab.Stats.baseMP;

            HeroPrefab.Stats.baseATK += addedAtk;
            HeroPrefab.Stats.curATK = HeroPrefab.Stats.baseATK;
            HeroPrefab.Stats.baseMATK += addedMatk;
            HeroPrefab.Stats.curMATK = HeroPrefab.Stats.baseMATK;
            HeroPrefab.Stats.baseDEF += addedDef;
            HeroPrefab.Stats.curDEF = HeroPrefab.Stats.baseDEF;

            HeroPrefab.Stats.baseCRIT += addedCrit;
            HeroPrefab.Stats.curCRIT = HeroPrefab.Stats.baseCRIT;
            HeroPrefab.Stats.baseDodge += addedDodge;
            HeroPrefab.Stats.curDodge = HeroPrefab.Stats.baseDodge;
            HeroPrefab.Stats.baseHit += addedHit;
            HeroPrefab.Stats.curHit = HeroPrefab.Stats.baseHit;
            HeroPrefab.Stats.baseSpeed += addedSpeed;
            HeroPrefab.Stats.curSpeed = HeroPrefab.Stats.baseSpeed;
            HeroPrefab.Stats.minATK = HeroPrefab.Stats.curATK;
            HeroPrefab.Stats.maxATK = (HeroPrefab.Stats.minATK / 100) * 120;
            //HeroData.instance.SaveCharData();
            Clean();
        }
    }

    void ExecuteStatIncrease(UnitAttributes hero)
    {
        if (hero != null)
        {
            hero.Stats.strength.BaseValue += AddedStr;
            hero.Stats.intellect.BaseValue += AddedInt;
            hero.Stats.dexterity.BaseValue += AddedDex;
            hero.Stats.agility.BaseValue += AddedAgi;
            hero.Stats.stamina.BaseValue += AddedSta;
            hero.Stats.baseHP += addedHP;
            hero.Stats.curHP = hero.Stats.baseHP;
            hero.Stats.baseMP += addedMP;
            hero.Stats.curMP = hero.Stats.baseMP;
            hero.Stats.baseATK += addedAtk;
            hero.Stats.curATK = hero.Stats.baseATK;
            hero.Stats.baseMATK += addedMatk;
            hero.Stats.curMATK = hero.Stats.baseMATK;
            hero.Stats.baseDEF += addedDef;
            hero.Stats.curDEF = hero.Stats.baseDEF;
            hero.Stats.baseCRIT += addedCrit;
            hero.Stats.curCRIT = hero.Stats.baseCRIT;
            hero.Stats.baseDodge += addedDodge;
            hero.Stats.curDodge = hero.Stats.baseDodge;
            hero.Stats.baseHit += addedHit;
            hero.Stats.curHit = hero.Stats.baseHit;
            hero.Stats.baseSpeed += addedSpeed;
            hero.Stats.curSpeed = hero.Stats.baseSpeed;
            hero.Stats.minATK = hero.Stats.curATK;
            hero.Stats.maxATK = Mathf.Round((hero.Stats.minATK / 100) * 120);


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

    void CalculateStatBonus()
    {
        //Calculate HP based on Stats
        addedHP = Mathf.Round(AddedStr * HeroDataManager.instance.UnitDatabase.hpPerStr) + (AddedSta * HeroDataManager.instance.UnitDatabase.hpPerSta);
        //Calculate MP based on stats
        addedMP = Mathf.Round(AddedInt * HeroDataManager.instance.UnitDatabase.mpPerInt);
        //Calculate Attack based on stats
        addedAtk = Mathf.Round((AddedStr * HeroDataManager.instance.UnitDatabase.atkPerStr) + (AddedInt * HeroDataManager.instance.UnitDatabase.atkPerInt) + (AddedDex * HeroDataManager.instance.UnitDatabase.atkPerDex));
        //Calculate magic Attack based on stats
        addedMatk = Mathf.Round((AddedStr * HeroDataManager.instance.UnitDatabase.matkPerStr) + (AddedInt * HeroDataManager.instance.UnitDatabase.matkPerInt));
        //Calculate HIT based on stats
        addedHit = Mathf.Round(AddedDex * HeroDataManager.instance.UnitDatabase.hitPerDex);
        //Calculate dodge based on stats
        addedDodge = Mathf.Round(AddedAgi * HeroDataManager.instance.UnitDatabase.dodgePerAgi);
        //calculate def based on stats
        addedDef = Mathf.Round((AddedSta * HeroDataManager.instance.UnitDatabase.defPerSta));
        //calculate critrate based on stats
        addedCrit = 0;
        //calculate speed based on stats
        addedSpeed = Mathf.Round(AddedAgi * HeroDataManager.instance.UnitDatabase.spdPerAgi);
    }

    public void CalculateBonus(UnitAttributes hero)
    {
        var Str = 1;
        var Int = 1;
        var Dex = 1;
        var Agi = 1;
        var Sta = 1;
        //Calculate HP based on Stats
        addedHP = Mathf.Round(Str * HeroDataManager.instance.UnitDatabase.hpPerStr) + (Sta * HeroDataManager.instance.UnitDatabase.hpPerSta);
        //Calculate MP based on stats
        addedMP = Mathf.Round(Int * HeroDataManager.instance.UnitDatabase.mpPerInt);
        //Calculate Attack based on stats
        addedAtk = Mathf.Round((Str * HeroDataManager.instance.UnitDatabase.atkPerStr) + (Int * HeroDataManager.instance.UnitDatabase.atkPerInt) + (AddedDex * HeroDataManager.instance.UnitDatabase.atkPerDex));
        //Calculate magic Attack based on stats
        addedMatk = Mathf.Round((Str * HeroDataManager.instance.UnitDatabase.matkPerStr) + (Int * HeroDataManager.instance.UnitDatabase.matkPerInt));
        //Calculate HIT based on stats
        addedHit = Mathf.Round(Dex * HeroDataManager.instance.UnitDatabase.hitPerDex);
        //Calculate dodge based on stats
        addedDodge = Mathf.Round(Agi * HeroDataManager.instance.UnitDatabase.dodgePerAgi);
        //calculate def based on stats
        addedDef = Mathf.Round((Sta * HeroDataManager.instance.UnitDatabase.defPerSta));
        //calculate critrate based on stats
        addedCrit = 0;
        //calculate speed based on stats
        addedSpeed = Mathf.Round(Agi * HeroDataManager.instance.UnitDatabase.spdPerAgi);
        //
        AddedStr = Str;
        AddedInt = Int;
        AddedDex = Dex;
        AddedAgi = Agi;
        AddedSta = Sta;

        ExecuteStatIncrease(hero);
    }


}
