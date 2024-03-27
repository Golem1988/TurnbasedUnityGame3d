using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UnitedInfoPanel : MonoBehaviour
{
    [SerializeField] HeroInfoInterface heroStats;
    public SummonInfoInterface summonInterface;
    [SerializeField] EquipmentPanel EquipmentPanel;
    [SerializeField] Inventory Inventory;
    [SerializeField] SkillPanel heroSkillPanel;
    [SerializeField] GameObject[] avatarButtons;
    [SerializeField] Transform heroModelPlace;
    public GameObject ShowSummonButton;
    public GameObject HideSummonButton;
    public GameObject myAv;

    [Header ("Inventory and equipment things")]
    [SerializeField] StatPanel sPanel;
    [SerializeField] ItemTooltip itemTooltip;
    [SerializeField] Image draggableItem;
    [SerializeField] ItemSaveManager itemSaveManager;
    private BaseItemSlot dragItemSlot;
    private CharacterInformation hero;
    //private List<CharacterInformation> HeroList = new ();

    private void Awake()
    {

        //sPanel.SetStats(unit.Stats.strength, unit.Stats.agility, unit.Stats.intellect, unit.Stats.dexterity, unit.Stats.stamina);
        //sPanel.UpdateStatValues();
        //sPanel.UpdateStatValuesX();

        // Setup Events:
        // Right Click
        Inventory.OnRightClickEvent += Equip;
        EquipmentPanel.OnRightClickEvent += Unequip;
        // Pointer Enter
        Inventory.OnPointerEnterEvent += ShowTooltip;
        EquipmentPanel.OnPointerEnterEvent += ShowTooltip;
        //craftingWindow.OnPointerEnterEvent += ShowTooltip;
        // Pointer Exit
        Inventory.OnPointerExitEvent += HideTooltip;
        EquipmentPanel.OnPointerExitEvent += HideTooltip;
        //craftingWindow.OnPointerExitEvent += HideTooltip;
        // Begin Drag
        Inventory.OnBeginDragEvent += BeginDrag;
        EquipmentPanel.OnBeginDragEvent += BeginDrag;
        // End Drag
        Inventory.OnEndDragEvent += EndDrag;
        EquipmentPanel.OnEndDragEvent += EndDrag;
        // Drag
        Inventory.OnDragEvent += Drag;
        EquipmentPanel.OnDragEvent += Drag;
        // Drop
        Inventory.OnDropEvent += Drop;
        EquipmentPanel.OnDropEvent += Drop;
        //dropItemArea.OnDropEvent += DropItemOutsideUI;


    }

    private void Start()
    {
        heroStats.HeroList = HeroDataManager.instance.CharacterInfo;
        
        string mainHeroName = heroStats.HeroList.FirstOrDefault(name => name.isMainCharacter)?.Name;
        //stats
        heroStats.HeroPrefab = Extensions.FindHeroEntry(mainHeroName);
        hero = heroStats.HeroPrefab;
        //sPanel.characterX = hero;
        heroStats.Clean();
        //equipment
        EquipmentPanel.EditableHero = hero;
        EquipmentPanel.DisplayItems();
        //summons
        //summonInterface.Start();
        summonInterface.Owner = Extensions.FindHeroEntry(mainHeroName);
        if (hero.SummonList.Count > 0)
        {
            summonInterface.EditableSummon = hero.SummonList[0];
            ShowSummonButton.GetComponent<Button>().interactable = true;
        }
        if (hero.SummonList.Count == 0)
        {
            summonInterface.EditableSummon = null;
            ShowSummonButton.GetComponent<Button>().interactable = false;
        }
        summonInterface.Refresh();

        //heroSkillPanel.ShowHeroSkills(heroStats.HeroList[0]);
        int index = heroStats.HeroList.FindIndex(hero => hero.isMainCharacter);
        heroSkillPanel.ShowHeroSkills(heroStats.HeroList[index]);
        UpdateModel(index);
        ButtonActions();
        avatarButtons[index].GetComponent<Button>().interactable = false;
        EnableHeroEditing(index);
        if (itemSaveManager != null)
        {
            LoadCharacter();
            //gameManager.Chat.AddToChatOutput("Character items loaded...");
        }

        sPanel.SetStats(hero.Stats.HP, hero.Stats.MP, hero.Stats.strength, hero.Stats.intellect, hero.Stats.dexterity, hero.Stats.agility, hero.Stats.stamina, hero.Stats.ATK, hero.Stats.MATK, hero.Stats.DEF, hero.Stats.Dodge, hero.Stats.Hit, hero.Stats.Speed);
        sPanel.UpdateStatValues();
        //sPanel.UpdateStatValuesX(hero);
    }

    public void LoadCharacter()
    {
        //statPanel.CalculateBonus(gameObject, data.strength, data.intellect, data.dexterity, data.agility, data.stamina);

        //itemSaveManager.LoadEquipment(this);
        //itemSaveManager.LoadInventory(this);
    }


    private void OnValidate()
    {
        foreach (var btn in avatarButtons)
        {
            btn.GetComponent<HeroAvatarButton>().infoPanel = this;
        }
    }

    public void ButtonActions()
    {
        for (int i = 0; i < avatarButtons.Length; i++)
        {
            if (heroStats.HeroList[i].isUnlocked)
            {
                avatarButtons[i].GetComponent<Button>().interactable = true;
                avatarButtons[i].GetComponent<HeroAvatarButton>().avatarImage.color = new Color32(255, 255, 255, 255);
            }
            else
            {
                avatarButtons[i].GetComponent<Button>().interactable = false;
                avatarButtons[i].GetComponent<HeroAvatarButton>().avatarImage.color = new Color32(255, 255, 255, 50);
            }    
        }

    }

    public void HealEveryone()
    {
        foreach (CharacterInformation hero in heroStats.HeroList)
        {
            hero.Stats.HP.CurValue = hero.Stats.HP.Value;
            hero.Stats.MP.CurValue = hero.Stats.MP.Value;

            if(hero.SummonList.Count > 0)
            {
                foreach (CapturedPets summon in hero.SummonList)
                {
                    summon.Stats.HP.CurValue = summon.Stats.HP.Value;
                    summon.Stats.MP.CurValue = summon.Stats.MP.Value;
                }
            }

        }
        heroStats.UpdateStats();
        summonInterface.UpdateStats();
        sPanel.SetStats(hero.Stats.HP, hero.Stats.MP, hero.Stats.strength, hero.Stats.intellect, hero.Stats.dexterity, hero.Stats.agility, hero.Stats.stamina, hero.Stats.ATK, hero.Stats.MATK, hero.Stats.DEF, hero.Stats.Dodge, hero.Stats.Hit, hero.Stats.Speed);
        sPanel.UpdateStatValues();
    }

    public void EnableHeroEditing(int a)
    {
        heroStats.HeroPrefab = heroStats.HeroList[a];
        hero = heroStats.HeroPrefab;

        heroStats.Clean();
        //summons
        summonInterface.Owner = hero;
        if (hero.SummonList.Count > 0)
        {
            summonInterface.EditableSummon = hero.SummonList[0];
            ShowSummonButton.GetComponent<Button>().interactable = true;
        }
        if (hero.SummonList.Count == 0)
        {
            summonInterface.EditableSummon = null;
            ShowSummonButton.GetComponent<Button>().interactable = false;
        }
        summonInterface.Refresh();
        //skills
        heroSkillPanel.ShowHeroSkills(hero);
        //equip
        UpdateModel(a);
        for (int i = 0; i < avatarButtons.Length; i++)
        {
            avatarButtons[i].GetComponent<HeroAvatarButton>().glowEffect.SetActive(false);
        }
        avatarButtons[a].GetComponent<HeroAvatarButton>().glowEffect.SetActive(true);
        sPanel.SetStats(hero.Stats.HP, hero.Stats.MP, hero.Stats.strength, hero.Stats.intellect, hero.Stats.dexterity, hero.Stats.agility, hero.Stats.stamina, hero.Stats.ATK, hero.Stats.MATK, hero.Stats.DEF, hero.Stats.Dodge, hero.Stats.Hit, hero.Stats.Speed);
        sPanel.UpdateStatValues();
        EquipmentPanel.EditableHero = hero;
        EquipmentPanel.DisplayItems();
    }

    

    void UpdateModel(int index)
    {
        if (myAv != null)
        {
            Destroy(myAv);
        }
        GameObject heroModel = Extensions.FindModelPrefab(heroStats.HeroList[index].BaseID, true);
        myAv = Instantiate(heroModel, heroModelPlace.position, Quaternion.Euler(15, 180, 0), heroModelPlace);
        EquipmentPanel.heroModel = myAv.transform;
        //myAv.transform.rotation =  Quaternion.Euler(15, 0, 0);
        Extensions.SetLayer(myAv, 5);
    }

    //void DestroyAvatar()
    //{
    //    if (myAv != null)
    //        Destroy(myAv);
    //}

    //private void OnValidate()
    //{
    //    HeroList = HeroData.instance.CharacterInfo;
    //    CreateCharButtons();
    //}

    private void Equip(BaseItemSlot itemSlot)
    {
        EquippableItem equippableItem = itemSlot.Item as EquippableItem;
        if (equippableItem != null)
        {
            Equip(equippableItem);
        }
    }

    private void Unequip(BaseItemSlot itemSlot)
    {
        EquippableItem equippableItem = itemSlot.Item as EquippableItem;
        if (equippableItem != null)
        {
            Unequip(equippableItem);
        }
    }

    private void InventoryRightClick(BaseItemSlot itemSlot)
    {
        if (itemSlot.Item is EquippableItem)
        {
            Equip((EquippableItem)itemSlot.Item);
        }
        else if (itemSlot.Item is UsableItem)
        {
            UsableItem usableItem = (UsableItem)itemSlot.Item;
            usableItem.Use(hero); //this as this in THIS Character class. We need to change that

            if (usableItem.IsConsumable)
            {
                itemSlot.Amount--;
                usableItem.Destroy();
            }
        }
    }

    private void EquipmentPanelRightClick(BaseItemSlot itemSlot)
    {
        if (itemSlot.Item is EquippableItem)
        {
            Unequip((EquippableItem)itemSlot.Item);
        }
    }

    private void ShowTooltip(BaseItemSlot itemSlot)
    {
        EquippableItem equippableItem = itemSlot.Item as EquippableItem;
        if (equippableItem != null)
        {
            itemTooltip.ShowTooltip(equippableItem);
        }
    }

    private void HideTooltip(BaseItemSlot itemSlot)
    {
        if (itemTooltip.gameObject.activeSelf)
        {
            itemTooltip.HideTooltip();
        }
    }

    private void BeginDrag(BaseItemSlot itemSlot)
    {
        if (itemSlot.Item != null)
        {
            dragItemSlot = itemSlot;
            draggableItem.sprite = itemSlot.Item.Icon;
            draggableItem.transform.position = Input.mousePosition;
            draggableItem.gameObject.SetActive(true);
        }
    }

    private void Drag(BaseItemSlot itemSlot)
    {
        draggableItem.transform.position = Input.mousePosition;
    }

    private void EndDrag(BaseItemSlot itemSlot)
    {
        dragItemSlot = null;
        draggableItem.gameObject.SetActive(false);
    }

    private void Drop(BaseItemSlot dropItemSlot)
    {
        if (dragItemSlot == null) return;

        if (dropItemSlot.CanAddStack(dragItemSlot.Item))
        {
            AddStacks(dropItemSlot);
        }
        else if (dropItemSlot.CanReceiveItem(dragItemSlot.Item) && dragItemSlot.CanReceiveItem(dropItemSlot.Item))
        {
            SwapItems(dropItemSlot);
        }
    }

    private void AddStacks(BaseItemSlot dropItemSlot)
    {
        int numAddableStacks = dropItemSlot.Item.MaximumStacks - dropItemSlot.Amount;
        int stacksToAdd = Mathf.Min(numAddableStacks, dragItemSlot.Amount);

        dropItemSlot.Amount += stacksToAdd;
        dragItemSlot.Amount -= stacksToAdd;
    }

    private void SwapItems(BaseItemSlot dropItemSlot)
    {
        EquippableItem dragEquipItem = dragItemSlot.Item as EquippableItem;
        EquippableItem dropEquipItem = dropItemSlot.Item as EquippableItem;

        if (dropItemSlot is EquipmentSlot)
        {
            if (dragEquipItem != null) dragEquipItem.Equip(hero); //this as this in THIS Character class. We need to change that
            if (dropEquipItem != null) dropEquipItem.Unequip(hero); //this as this in THIS Character class. We need to change that
        }
        if (dragItemSlot is EquipmentSlot)
        {
            if (dragEquipItem != null) dragEquipItem.Unequip(hero); //this as this in THIS Character class. We need to change that
            if (dropEquipItem != null) dropEquipItem.Equip(hero); //this as this in THIS Character class. We need to change that
        }
        sPanel.SetStats(hero.Stats.HP, hero.Stats.MP, hero.Stats.strength, hero.Stats.intellect, hero.Stats.dexterity, hero.Stats.agility, hero.Stats.stamina, hero.Stats.ATK, hero.Stats.MATK, hero.Stats.DEF, hero.Stats.Dodge, hero.Stats.Hit, hero.Stats.Speed);
        sPanel.UpdateStatValues();
        sPanel.UpdateStatValuesX(hero);

        Item draggedItem = dragItemSlot.Item;
        int draggedItemAmount = dragItemSlot.Amount;

        dragItemSlot.Item = dropItemSlot.Item;
        dragItemSlot.Amount = dropItemSlot.Amount;

        dropItemSlot.Item = draggedItem;
        dropItemSlot.Amount = draggedItemAmount;
    }

    //private void DropItemOutsideUI()
    //{
    //    if (dragItemSlot == null) return;

    //    reallyDropItemDialog.Show();
    //    BaseItemSlot slot = dragItemSlot;
    //    reallyDropItemDialog.OnYesEvent += () => DestroyItemInSlot(slot);
    //}

    private void DestroyItemInSlot(BaseItemSlot itemSlot)
    {
        // If the item is equiped, unequip first
        if (itemSlot is EquipmentSlot)
        {
            EquippableItem equippableItem = (EquippableItem)itemSlot.Item;
            equippableItem.Unequip(hero);
        }

        itemSlot.Item.Destroy();
        itemSlot.Item = null;
    }

    public void Equip(EquippableItem item)
    {
        if (Inventory.RemoveItem(item))
        {
            EquippableItem previousItem;
            if (EquipmentPanel.AddItem(item, out previousItem))
            {
                if (previousItem != null)
                {
                    Inventory.AddItem(previousItem);
                    previousItem.Unequip(hero); //this as this in THIS Character class. We need to change that
                    sPanel.SetStats(hero.Stats.HP, hero.Stats.MP, hero.Stats.ATK, hero.Stats.MATK, hero.Stats.DEF, hero.Stats.Dodge, hero.Stats.Hit, hero.Stats.Speed);
                    sPanel.UpdateStatValues();
                    sPanel.UpdateStatValuesX(hero);
                }
                item.Equip(hero); //this as this in THIS Character class. We need to change that
                sPanel.SetStats(hero.Stats.HP, hero.Stats.MP, hero.Stats.strength, hero.Stats.intellect, hero.Stats.dexterity, hero.Stats.agility, hero.Stats.stamina, hero.Stats.ATK, hero.Stats.MATK, hero.Stats.DEF, hero.Stats.Dodge, hero.Stats.Hit, hero.Stats.Speed);
                sPanel.UpdateStatValues();
                sPanel.UpdateStatValuesX(hero);
            }
            else
            {
                Inventory.AddItem(item);
            }
        }
    }

    public void Unequip(EquippableItem item)
    {
        if (Inventory.CanAddItem(item) && EquipmentPanel.RemoveItem(item))
        {
            item.Unequip(hero); //this as this in THIS Character class. We need to change that
            sPanel.SetStats(hero.Stats.HP, hero.Stats.MP, hero.Stats.strength, hero.Stats.intellect, hero.Stats.dexterity, hero.Stats.agility, hero.Stats.stamina, hero.Stats.ATK, hero.Stats.MATK, hero.Stats.DEF, hero.Stats.Dodge, hero.Stats.Hit, hero.Stats.Speed);
            sPanel.UpdateStatValues();
            sPanel.UpdateStatValuesX(hero);
            Inventory.AddItem(item);
        }
    }

    private ItemContainer openItemContainer;

    private void TransferToItemContainer(BaseItemSlot itemSlot)
    {
        Item item = itemSlot.Item;
        if (item != null && openItemContainer.CanAddItem(item))
        {
            Inventory.RemoveItem(item);
            openItemContainer.AddItem(item);
        }
    }

    private void TransferToInventory(BaseItemSlot itemSlot)
    {
        Item item = itemSlot.Item;
        if (item != null && Inventory.CanAddItem(item))
        {
            openItemContainer.RemoveItem(item);
            Inventory.AddItem(item);
        }
    }

    public void OpenItemContainer(ItemContainer itemContainer)
    {
        openItemContainer = itemContainer;

        Inventory.OnRightClickEvent -= InventoryRightClick;
        Inventory.OnRightClickEvent += TransferToItemContainer;

        itemContainer.OnRightClickEvent += TransferToInventory;

        itemContainer.OnPointerEnterEvent += ShowTooltip;
        itemContainer.OnPointerExitEvent += HideTooltip;
        itemContainer.OnBeginDragEvent += BeginDrag;
        itemContainer.OnEndDragEvent += EndDrag;
        itemContainer.OnDragEvent += Drag;
        itemContainer.OnDropEvent += Drop;
    }

    public void CloseItemContainer(ItemContainer itemContainer)
    {
        openItemContainer = null;

        Inventory.OnRightClickEvent += InventoryRightClick;
        Inventory.OnRightClickEvent -= TransferToItemContainer;

        itemContainer.OnRightClickEvent -= TransferToInventory;

        itemContainer.OnPointerEnterEvent -= ShowTooltip;
        itemContainer.OnPointerExitEvent -= HideTooltip;
        itemContainer.OnBeginDragEvent -= BeginDrag;
        itemContainer.OnEndDragEvent -= EndDrag;
        itemContainer.OnDragEvent -= Drag;
        itemContainer.OnDropEvent -= Drop;
    }

}

