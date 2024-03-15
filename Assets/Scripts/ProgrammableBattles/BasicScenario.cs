using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Programmable Battles/New Behaviour")]
public class BasicScenario : ScriptableObject
{
    public List<Scenario> entry = new();
}
