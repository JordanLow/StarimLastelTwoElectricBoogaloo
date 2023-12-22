using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
	public static SceneHandler instance;
	
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

	public void ReloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
