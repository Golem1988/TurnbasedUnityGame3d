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
    [SerializeField] Transform modelPlaceholder;
    public GameObject ShowSummonButton;
    public GameObject HideSummonButton;
    private GameObject myAv;

    [Header ("Inventory and equipment things")]
    [SerializeField] StatPanel sPanel;
    [SerializeField] ItemTooltip itemTooltip;
    [SerializeField] Image draggableItem;
    [SerializeField] ItemSaveManager itemSaveManager;
    private BaseItemSlot dragItemSlot;
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
        heroStats.Clean();
        //summons
        //summonInterface.Start();
        summonInterface.Owner = Extensions.FindHeroEntry(mainHeroName);
        if (heroStats.HeroPrefab.SummonList.Count > 0)
        {
            summonInterface.EditableSummon = heroStats.HeroPrefab.SummonList[0];
            ShowSummonButton.GetComponent<Button>().interactable = true;
        }
        if (heroStats.HeroPrefab.SummonList.Count == 0)
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
            hero.Stats.curHP = hero.Stats.baseHP;
            hero.Stats.curMP = hero.Stats.baseMP;

            if(hero.SummonList.Count > 0)
            {
                foreach (CapturedPets summon in hero.SummonList)
                {
                    summon.Stats.curHP = summon.Stats.baseHP;
                    summon.Stats.curMP = summon.Stats.baseMP;
                }
            }

        }
        heroStats.UpdateStats();
        summonInterface.UpdateStats();
    }

    public void EnableHeroEditing(int a)
    {
        heroStats.HeroPrefab = heroStats.HeroList[a];
        heroStats.Clean();
        //summons
        summonInterface.Owner = heroStats.HeroPrefab;
        if (heroStats.HeroPrefab.SummonList.Count > 0)
        {
            summonInterface.EditableSummon = heroStats.HeroPrefab.SummonList[0];
            ShowSummonButton.GetComponent<Button>().interactable = true;
        }
        if (heroStats.HeroPrefab.SummonList.Count == 0)
        {
            summonInterface.EditableSummon = null;
            ShowSummonButton.GetComponent<Button>().interactable = false;
        }
        summonInterface.Refresh();
        //skills
        heroSkillPanel.ShowHeroSkills(heroStats.HeroPrefab);
        //equip
        UpdateModel(a);
        for (int i = 0; i < avatarButtons.Length; i++)
        {
            avatarButtons[i].GetComponent<HeroAvatarButton>().glowEffect.SetActive(false);
        }
        avatarButtons[a].GetComponent<HeroAvatarButton>().glowEffect.SetActive(true);
    }

    

    void UpdateModel(int index)
    {
        if (myAv != null)
        {
            Destroy(myAv);
        }
        GameObject summonModel = Extensions.FindModelPrefab(heroStats.HeroList[index].BaseID, true);
        myAv = Instantiate(summonModel, modelPlaceholder.position, Quaternion.Euler(15, 180, 0), modelPlaceholder);
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
            //usableItem.Use(this); this as this in THIS Character class. We need to change that

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
            //if (dragEquipItem != null) dragEquipItem.Equip(this); this as this in THIS Character class. We need to change that
            //if (dropEquipItem != null) dropEquipItem.Unequip(this); this as this in THIS Character class. We need to change that
        }
        if (dragItemSlot is EquipmentSlot)
        {
            //if (dragEquipItem != null) dragEquipItem.Unequip(this); this as this in THIS Character class. We need to change that
            //if (dropEquipItem != null) dropEquipItem.Equip(this); this as this in THIS Character class. We need to change that
        }
        sPanel.UpdateStatValues();
        sPanel.UpdateStatValuesX();

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
            //equippableItem.Unequip(this); this as this in THIS Character class. We need to change that
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
                    //previousItem.Unequip(this); this as this in THIS Character class. We need to change that

                    sPanel.UpdateStatValues();
                    sPanel.UpdateStatValuesX();
                }
                //item.Equip(this); this as this in THIS Character class. We need to change that

                sPanel.UpdateStatValues();
                sPanel.UpdateStatValuesX();
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
            //item.Unequip(this); this as this in THIS Character class. We need to change that

            sPanel.UpdateStatValues();
            sPanel.UpdateStatValuesX();
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
