using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
	private static EventHandler instance;
	void Awake() 
	{
		DontDestroyOnLoad(this);
		
		if (instance == null)
		{
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}
	// Singleton
	
	public static event Action OnExitLevel;
	
	public void ExitLevel()
	{
		OnExitLevel?.Invoke();
	}
}
