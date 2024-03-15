using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAction : ScriptableObject
{
    public string ActionName;
    [TextArea(5, 7)] public string Description;

    public virtual void Act(UnitStateMachine actor, UnitStateMachine target, int index)
    {

    }
}
