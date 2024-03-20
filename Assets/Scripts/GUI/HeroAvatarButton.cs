using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroAvatarButton : MonoBehaviour
{
    public int listOrder;
    public UnitedInfoPanel infoPanel;
    public Image avatarImage;
    public GameObject glowEffect;

    public void TransmitNumber()
    {
        infoPanel.EnableHeroEditing(listOrder);

    }

}
