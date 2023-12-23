using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
	public static event Action OnExitLevel;
	
	public void ExitLevel()
	{
		OnExitLevel?.Invoke();
	}
}
