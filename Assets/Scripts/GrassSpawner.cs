using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawner : MonoBehaviour
{
	void Awake() 
	{
		transform.parent.Find("Grass").gameObject.SetActive(false);
	}

    void OnTriggerExit2D(Collider2D other) // Grow grass on marked spot and destroy marker (self)
    {
		if (other.transform.gameObject.tag == "Player") 
		{
			transform.parent.Find("Grass").gameObject.SetActive(true);
			other.GetComponent<PlayerMovement>().OffSpawner();
			Object.Destroy(this.gameObject);
		}
	}
}
