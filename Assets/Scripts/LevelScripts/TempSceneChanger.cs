using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSceneChanger : MonoBehaviour
{
    [SerializeField] int destinationScene;
	
	void OnTriggerEnter2D(Collider2D other)
	{
		SceneHandler.instance.LoadScene(destinationScene);
	}
}
