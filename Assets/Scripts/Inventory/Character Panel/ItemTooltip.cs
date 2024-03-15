using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] TMP_Text ItemNameText;
    [SerializeField] TMP_Text ItemSlotText;
    [SerializeField] TMP_Text ItemLevelText;
    [SerializeField] TMP_Text ItemStatsText;
    [SerializeField] Image ItemIcon;

    private void Awake()
    {
        gameObject.SetActive(false);
    }
    public void ShowTooltip(EquippableItem item)
    {
        ItemNameText.text = item.ItemName;
        ItemSlotText.text = item.EquipmentType.ToString();
        ItemLevelText.text = item.RequiredLevel.ToString();
        ItemIcon.sprite = item.Icon;
        //ItemDescriptionText.text = item.GetDescription();
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
