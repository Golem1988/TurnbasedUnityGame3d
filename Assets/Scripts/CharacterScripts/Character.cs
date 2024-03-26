using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Kryz.CharacterStats;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable]
public class Character : MonoBehaviour
{
    private GameManager gameManager;
    public UnitAttributes unit;
    public Inventory Inventory;
    public EquipmentPanel EquipmentPanel;
    public HeroInfoInterface statPanel;
    //[SerializeField] StatPanel sPanel;
    //[SerializeField] ItemTooltip itemTooltip;
    //[SerializeField] Image draggableItem;
    public SummonHandler summonHandler;
    public UnitLevel UnitLevel;

    //[SerializeField] ItemSaveManager itemSaveManager;

    //private BaseItemSlot dragItemSlot;


    //[Header("Secondary Attributes")]
    //public float maxRage = 150f;
    //public float curRage = 0f;

    public int curLvl;

    private void Awake()
    {
        gameManager = GameManager.instance;

    }
    //    //sPanel.SetStats(unit.Stats.strength.BaseValue, unit.Stats.agility.BaseValue, unit.Stats.intellect.BaseValue, unit.Stats.dexterity.BaseValue, unit.Stats.stamina.BaseValue);
    //    //sPanel.UpdateStatValues();
    //    //sPanel.UpdateStatValuesX();

    //    // Setup Events:
    //    // Right Click
    //    Inventory.OnRightClickEvent += Equip;
    //    EquipmentPanel.OnRightClickEvent += Unequip;
    //    // Pointer Enter
    //    Inventory.OnPointerEnterEvent += ShowTooltip;
    //    EquipmentPanel.OnPointerEnterEvent += ShowTooltip;
    //    //craftingWindow.OnPointerEnterEvent += ShowTooltip;
    //    // Pointer Exit
    //    Inventory.OnPointerExitEvent += HideTooltip;
    //    EquipmentPanel.OnPointerExitEvent += HideTooltip;
    //    //craftingWindow.OnPointerExitEvent += HideTooltip;
    //    // Begin Drag
    //    Inventory.OnBeginDragEvent += BeginDrag;
    //    EquipmentPanel.OnBeginDragEvent += BeginDrag;
    //    // End Drag
    //    Inventory.OnEndDragEvent += EndDrag;
    //    EquipmentPanel.OnEndDragEvent += EndDrag;
    //    // Drag
    //    Inventory.OnDragEvent += Drag;
    //    EquipmentPanel.OnDragEvent += Drag;
    //    // Drop
    //    Inventory.OnDropEvent += Drop;
    //    EquipmentPanel.OnDropEvent += Drop;
    //    //dropItemArea.OnDropEvent += DropItemOutsideUI;


    //}

    private void Start()
    {
        //itemSaveManager = gameManager.ItemSaveManager;
        //charName = GetComponent<UnitAttributes>().Stats.displayName;

        summonHandler = gameObject.GetComponent<SummonHandler>();
        statPanel = gameManager.heroInfoInterface;
        //var leveltest = UnitLevel.level.currentlevel;

        //testing level adjustments
        int index = HeroDataManager.instance.CharacterInfo.FindIndex(hero => hero.Name == unit.Stats.theName);
        var unitLevel = HeroDataManager.instance.CharacterInfo[index].Level;

        UnitLevel.level = unitLevel;

        curLvl = UnitLevel.level.currentlevel;
        Debug.Log("curLvl = " + curLvl.ToString());
        Debug.Log("experience = " + UnitLevel.level.experience.ToString());
        UnitLevel.level = new Level(curLvl);
        Debug.Log("curLvl = " + curLvl.ToString());
        Debug.Log("experience = " + UnitLevel.level.experience.ToString());
        UnitLevel.level.experience = unitLevel.totalExperience;
        UnitLevel.level.totalExperience = unitLevel.totalExperience;
        UnitLevel.level.CUR_EXP = unitLevel.CUR_EXP;
        UnitLevel.level.NEXT_EXP = unitLevel.NEXT_EXP;
        Debug.Log("experience = " + UnitLevel.level.experience.ToString());
        //hero.Stats.displayNameText.text = hero.Stats.theName.ToString();
    }

    /*
    private void OnDestroy()
    {
        if (itemSaveManager != null)
        {
            itemSaveManager.SaveEquipment(this);
            itemSaveManager.SaveInventory(this);
            SaveSystem.SaveCharacter(this);   
        }
        summonHandler.SavePetData(charName);
        if (gameManager)
            gameManager.Chat.AddToChatOutput("Character data saved...");
    }

    
    private void OnValidate()
    {
        unit = GetComponent<UnitStateMachine>().unit;
        UnitLevel = GetComponent<UnitLevel>();
        //hero.Stats.theName = gameObject.name;
        if (itemTooltip == null)
            itemTooltip = FindObjectOfType<ItemTooltip>();
    }
    
    private void Update()
    {
        //exp add by hand
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UnitLevel.level.AddExp(100);
            statPanel.UpdateStats();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            LoadCharacter();
        }

        //save manually
        if (Input.GetKeyDown(KeyCode.L))
        {
            OnDestroy();
        }

    }
    */
    public void OnLevelUp()
    {
        gameManager.Chat.AddToChatOutput("Hero" + unit.Stats.theName + " leveled up from level " + curLvl + " to level " + UnitLevel.level.currentlevel + "!");
        int levelsUpped = UnitLevel.level.currentlevel - curLvl;
        for (int i = 0; i < levelsUpped; i++)
        {
            unit.Stats.unspentStatPoints += HeroDataManager.instance.UnitDatabase.statpointsPerLevel;
            statPanel.CalculateBonus(unit);
        }
        curLvl = UnitLevel.level.currentlevel;
        //writing data to HeroData
        //int index = Extensions.FindHeroIndex(unit.Stats.theName);
        int index = HeroDataManager.instance.CharacterInfo.FindIndex(hero => hero.Name == unit.Stats.theName);
        HeroDataManager.instance.CharacterInfo[index].Stats = unit.Stats;
        HeroDataManager.instance.CharacterInfo[index].Level = UnitLevel.level;
    }

    //private void Equip(BaseItemSlot itemSlot)
    //{
    //    EquippableItem equippableItem = itemSlot.Item as EquippableItem;
    //    if (equippableItem != null)
    //    {
    //        Equip(equippableItem);
    //    }
    //}

    //private void Unequip(BaseItemSlot itemSlot)
    //{
    //    EquippableItem equippableItem = itemSlot.Item as EquippableItem;
    //    if (equippableItem != null)
    //    {
    //        Unequip(equippableItem);
    //    }
    //}

    //private void InventoryRightClick(BaseItemSlot itemSlot)
    //{
    //    if (itemSlot.Item is EquippableItem)
    //    {
    //        Equip((EquippableItem)itemSlot.Item);
    //    }
    //    else if (itemSlot.Item is UsableItem)
    //    {
    //        UsableItem usableItem = (UsableItem)itemSlot.Item;
    //        //usableItem.Use(this);

    //        if (usableItem.IsConsumable)
    //        {
    //            itemSlot.Amount--;
    //            usableItem.Destroy();
    //        }
    //    }
    //}

    //private void EquipmentPanelRightClick(BaseItemSlot itemSlot)
    //{
    //    if (itemSlot.Item is EquippableItem)
    //    {
    //        Unequip((EquippableItem)itemSlot.Item);
    //    }
    //}

    //private void ShowTooltip(BaseItemSlot itemSlot)
    //{
    //    EquippableItem equippableItem = itemSlot.Item as EquippableItem;
    //    if (equippableItem != null)
    //    {
    //        itemTooltip.ShowTooltip(equippableItem);
    //    }
    //}

    //private void HideTooltip(BaseItemSlot itemSlot)
    //{
    //    if (itemTooltip.gameObject.activeSelf)
    //    {
    //        itemTooltip.HideTooltip();
    //    }
    //}

    //private void BeginDrag(BaseItemSlot itemSlot)
    //{
    //    if (itemSlot.Item != null)
    //    {
    //        dragItemSlot = itemSlot;
    //        draggableItem.sprite = itemSlot.Item.Icon;
    //        draggableItem.transform.position = Input.mousePosition;
    //        draggableItem.gameObject.SetActive(true);
    //    }
    //}

    //private void Drag(BaseItemSlot itemSlot)
    //{
    //    draggableItem.transform.position = Input.mousePosition;
    //}

    //private void EndDrag(BaseItemSlot itemSlot)
    //{
    //    dragItemSlot = null;
    //    draggableItem.gameObject.SetActive(false);
    //}

    //private void Drop(BaseItemSlot dropItemSlot)
    //{
    //    if (dragItemSlot == null) return;

    //    if (dropItemSlot.CanAddStack(dragItemSlot.Item))
    //    {
    //        AddStacks(dropItemSlot);
    //    }
    //    else if (dropItemSlot.CanReceiveItem(dragItemSlot.Item) && dragItemSlot.CanReceiveItem(dropItemSlot.Item))
    //    {
    //        SwapItems(dropItemSlot);
    //    }
    //}

    //private void AddStacks(BaseItemSlot dropItemSlot)
    //{
    //    int numAddableStacks = dropItemSlot.Item.MaximumStacks - dropItemSlot.Amount;
    //    int stacksToAdd = Mathf.Min(numAddableStacks, dragItemSlot.Amount);

    //    dropItemSlot.Amount += stacksToAdd;
    //    dragItemSlot.Amount -= stacksToAdd;
    //}

    //private void SwapItems(BaseItemSlot dropItemSlot)
    //{
    //    EquippableItem dragEquipItem = dragItemSlot.Item as EquippableItem;
    //    EquippableItem dropEquipItem = dropItemSlot.Item as EquippableItem;

    //    if (dropItemSlot is EquipmentSlot)
    //    {
    //        //if (dragEquipItem != null) dragEquipItem.Equip(this);
    //        //if (dropEquipItem != null) dropEquipItem.Unequip(this);
    //    }
    //    if (dragItemSlot is EquipmentSlot)
    //    {
    //        //if (dragEquipItem != null) dragEquipItem.Unequip(this);
    //        //if (dropEquipItem != null) dropEquipItem.Equip(this);
    //    }
    //    sPanel.UpdateStatValues();
    //    sPanel.UpdateStatValuesX();

    //    Item draggedItem = dragItemSlot.Item;
    //    int draggedItemAmount = dragItemSlot.Amount;

    //    dragItemSlot.Item = dropItemSlot.Item;
    //    dragItemSlot.Amount = dropItemSlot.Amount;

    //    dropItemSlot.Item = draggedItem;
    //    dropItemSlot.Amount = draggedItemAmount;
    //}

    ////private void DropItemOutsideUI()
    ////{
    ////    if (dragItemSlot == null) return;

    ////    reallyDropItemDialog.Show();
    ////    BaseItemSlot slot = dragItemSlot;
    ////    reallyDropItemDialog.OnYesEvent += () => DestroyItemInSlot(slot);
    ////}

    //private void DestroyItemInSlot(BaseItemSlot itemSlot)
    //{
    //    // If the item is equiped, unequip first
    //    if (itemSlot is EquipmentSlot)
    //    {
    //        EquippableItem equippableItem = (EquippableItem)itemSlot.Item;
    //        //equippableItem.Unequip(this);
    //    }

    //    itemSlot.Item.Destroy();
    //    itemSlot.Item = null;
    //}

    //public void Equip(EquippableItem item)
    //{
    //    if (Inventory.RemoveItem(item))
    //    {
    //        EquippableItem previousItem;
    //        if (EquipmentPanel.AddItem(item, out previousItem))
    //        {
    //            if (previousItem != null)
    //            {
    //                Inventory.AddItem(previousItem);
    //                //previousItem.Unequip(this);

    //                sPanel.UpdateStatValues();
    //                sPanel.UpdateStatValuesX();
    //            }
    //            //item.Equip(this);

    //            sPanel.UpdateStatValues();
    //            sPanel.UpdateStatValuesX();
    //        }
    //        else
    //        {
    //            Inventory.AddItem(item);
    //        }
    //    }
    //}

    //public void Unequip(EquippableItem item)
    //{
    //    if (Inventory.CanAddItem(item) && EquipmentPanel.RemoveItem(item))
    //    {
    //        //item.Unequip(this);

    //        sPanel.UpdateStatValues();
    //        sPanel.UpdateStatValuesX();
    //        Inventory.AddItem(item);
    //    }
    //}

    //private ItemContainer openItemContainer;

    //private void TransferToItemContainer(BaseItemSlot itemSlot)
    //{
    //    Item item = itemSlot.Item;
    //    if (item != null && openItemContainer.CanAddItem(item))
    //    {
    //        Inventory.RemoveItem(item);
    //        openItemContainer.AddItem(item);
    //    }
    //}

    //private void TransferToInventory(BaseItemSlot itemSlot)
    //{
    //    Item item = itemSlot.Item;
    //    if (item != null && Inventory.CanAddItem(item))
    //    {
    //        openItemContainer.RemoveItem(item);
    //        Inventory.AddItem(item);
    //    }
    //}

    //public void OpenItemContainer(ItemContainer itemContainer)
    //{
    //    openItemContainer = itemContainer;

    //    Inventory.OnRightClickEvent -= InventoryRightClick;
    //    Inventory.OnRightClickEvent += TransferToItemContainer;

    //    itemContainer.OnRightClickEvent += TransferToInventory;

    //    itemContainer.OnPointerEnterEvent += ShowTooltip;
    //    itemContainer.OnPointerExitEvent += HideTooltip;
    //    itemContainer.OnBeginDragEvent += BeginDrag;
    //    itemContainer.OnEndDragEvent += EndDrag;
    //    itemContainer.OnDragEvent += Drag;
    //    itemContainer.OnDropEvent += Drop;
    //}

    //public void CloseItemContainer(ItemContainer itemContainer)
    //{
    //    openItemContainer = null;

    //    Inventory.OnRightClickEvent += InventoryRightClick;
    //    Inventory.OnRightClickEvent -= TransferToItemContainer;

    //    itemContainer.OnRightClickEvent -= TransferToInventory;

    //    itemContainer.OnPointerEnterEvent -= ShowTooltip;
    //    itemContainer.OnPointerExitEvent -= HideTooltip;
    //    itemContainer.OnBeginDragEvent -= BeginDrag;
    //    itemContainer.OnEndDragEvent -= EndDrag;
    //    itemContainer.OnDragEvent -= Drag;
    //    itemContainer.OnDropEvent -= Drop;
    //}
    
    //public void UpdateStatValues()
    //{
    //    sPanel.UpdateStatValues();
    //    sPanel.UpdateStatValuesX();
    //}
    
}
