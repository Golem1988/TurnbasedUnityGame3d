using UnityEngine;
using System;
using System.Collections.Generic;

public class EquipmentPanel : MonoBehaviour
{
	public EquipmentSlot[] EquipmentSlots;
	public Transform heroModel;
	[SerializeField] Transform equipmentSlotsParent;
	[Header("Current hero")]
	public CharacterInformation EditableHero;

	public event Action<BaseItemSlot> OnPointerEnterEvent;
	public event Action<BaseItemSlot> OnPointerExitEvent;
	public event Action<BaseItemSlot> OnRightClickEvent;
	public event Action<BaseItemSlot> OnBeginDragEvent;
	public event Action<BaseItemSlot> OnEndDragEvent;
	public event Action<BaseItemSlot> OnDragEvent;
	public event Action<BaseItemSlot> OnDropEvent;

	private void Start()
	{
		for (int i = 0; i < EquipmentSlots.Length; i++)
		{
			EquipmentSlots[i].OnPointerEnterEvent += slot => OnPointerEnterEvent(slot);
			EquipmentSlots[i].OnPointerExitEvent += slot => OnPointerExitEvent(slot);
			EquipmentSlots[i].OnRightClickEvent += slot => OnRightClickEvent(slot);
			EquipmentSlots[i].OnBeginDragEvent += slot => OnBeginDragEvent(slot);
			EquipmentSlots[i].OnEndDragEvent += slot => OnEndDragEvent(slot);
			EquipmentSlots[i].OnDragEvent += slot => OnDragEvent(slot);
			EquipmentSlots[i].OnDropEvent += slot => OnDropEvent(slot);
		}

	}

	private void OnValidate()
	{
		EquipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
	}

	public bool AddItem(EquippableItem item, out EquippableItem previousItem)
	{
		for (int i = 0; i < EquipmentSlots.Length; i++)
		{
			if (EquipmentSlots[i].EquipmentType == item.EquipmentType)
			{
				previousItem = (EquippableItem)EquipmentSlots[i].Item;
				EquipmentSlots[i].Item = item;
				EquipmentSlots[i].Amount = 1;
				return true;
			}
		}
		previousItem = null;
		return false;
	}

	public bool RemoveItem(EquippableItem item)
	{
		for (int i = 0; i < EquipmentSlots.Length; i++)
		{
			if (EquipmentSlots[i].Item == item)
			{
				EquipmentSlots[i].Item = null;
				EquipmentSlots[i].Amount = 0;
				return true;
			}
		}
		return false;
	}

	public void DisplayItems()
    {
		EquippableItem item;

		var itemList = EditableHero.Equipment;
		List<string> equippedItemsX = new List<string>();
		equippedItemsX.Add(itemList.WeaponID);
		equippedItemsX.Add(itemList.WingsID);
		equippedItemsX.Add(itemList.BootsID);
		equippedItemsX.Add(itemList.BeltID);
		equippedItemsX.Add(itemList.HelmetID);
		equippedItemsX.Add(itemList.RingID);
		equippedItemsX.Add(itemList.PendantID);
		equippedItemsX.Add(itemList.ArmorID);


		foreach (EquipmentSlot slot in EquipmentSlots)
		{
			slot.Item = null;
			slot.Amount = 0;
		}

		foreach (var itemID in equippedItemsX)
        {
			if (itemID != "")
			{
				item = Extensions.FindItemByID(itemID);

				foreach (EquipmentSlot slot in EquipmentSlots)
                {
					if (item.EquipmentType == slot.EquipmentType)
                    {
						slot.Item = item;
						slot.Amount = 1;
                    }

                }

				if (item.EquipmentType == EquipmentType.Wings)
				{
					if (item.Model)
						ShowWings(item.Model);
				}
			}
		}

	}

	void ShowWings(GameObject wings)
    {
		//var heroId = EditableHero.BaseID;
		Transform wingSpot = heroModel.GetComponent<GameObjectContainer>().objectArray[5].gameObject.transform;
		var wingInstance = Instantiate(wings, wingSpot.position, Quaternion.identity, wingSpot);
		Extensions.SetLayer(wingInstance, 5);
	}
}
