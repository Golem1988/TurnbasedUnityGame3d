using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Kryz.CharacterStats;

public class StatDisplay : MonoBehaviour
{
    public TMP_Text NameText;
    public TMP_Text ValueText;

    private void OnValidate()
    {
        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>();
        NameText = texts[0];
        ValueText = texts[1];
    }
}
