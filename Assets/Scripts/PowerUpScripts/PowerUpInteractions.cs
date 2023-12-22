using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpInteractions : MonoBehaviour
{
    [SerializeField] string powerType = "Vine"; //future use for diff powerups
	bool active = true;
	
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("PowerUp");
        if (other.tag == "Player" && active)
        {
            other.GetComponent<PlayerResource>().powerUpVine = true; //give power to player
			
            GetComponent<Renderer>().enabled = false; 
			active = false;
			//disables and hides powerup ball
        }
    }
	
	void respawn()
	{
		GetComponent<Renderer>().enabled = true;
		active = true;
	}
	
	// Event Listeners
	private void OnEnable()
	{
		EventHandler.OnExitLevel += respawn;
	}
	private void OnDisable()
	{
		EventHandler.OnExitLevel -= respawn;
	}
}
