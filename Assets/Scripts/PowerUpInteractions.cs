using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpInteractions : MonoBehaviour
{
    [SerializeField] string powerType = "Vine"; //future use for diff powerups
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("PowerUp");
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerResource>().powerUpVine = true; //give power to player
            Destroy(transform.gameObject); //removes powerup ball
        }
    }
}
