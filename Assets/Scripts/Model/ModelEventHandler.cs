using System;
using System.Collections;
using UnityEngine;

public class ModelEventHandler : MonoBehaviour
{
    public bool isVisible = false;

	public Action visibleAction;
	public Action invisibleAction;


    // OnBecameVisible is called when the renderer became visible by any camera
    void OnBecameVisible()
    {
		if (isVisible == false)
		{
			if (visibleAction != null)
			{
				visibleAction();
			}
			isVisible = true;
		}
    }

    // OnBecameInvisible is called when the renderer is no longer visible by any camera
	void OnBecameInvisible()
    {
		if (isVisible == true)
		{
			if (invisibleAction != null)
			{
				invisibleAction();
			}
			isVisible = false;
		}
    }

}