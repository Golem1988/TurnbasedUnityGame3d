using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCharacter : MonoBehaviour
{
    public int heroId;

    public void TransmitNumber()
    {
        GameObject.Find("CharacterCreationManager").GetComponent<CreateCharacter>().ShowCharacter(heroId);
    }
}
