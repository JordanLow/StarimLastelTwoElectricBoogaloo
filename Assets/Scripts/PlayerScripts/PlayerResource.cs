using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResource : MonoBehaviour
{
    public bool powerUpVine {get;set;}
    // Start is called before the first frame update
    void Start()
    {
        resetPowerUps();
    }
	
	void resetPowerUps()
	{
		powerUpVine = false;
	}
	
	// Event Listeners
	private void OnEnable()
	{
		EventHandler.OnExitLevel += resetPowerUps;
	}
	private void OnDisable()
	{
		EventHandler.OnExitLevel -= resetPowerUps;
	}
}
