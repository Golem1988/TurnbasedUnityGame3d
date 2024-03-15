using TMPro;
using UnityEngine;

public class SummonOrderInList : MonoBehaviour
{
    public int listOrder;
    public TMP_Text summonname;
    public SummonInfoInterface summonInterface;

    public void TransmitNumber()
    {
        summonInterface.EnableSummonEditing(listOrder);
    }

    public void SetColor()
    {
        summonname.color = Color.white;
    }
}

