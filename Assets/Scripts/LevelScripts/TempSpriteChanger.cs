using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSpriteChanger : MonoBehaviour
{
	[SerializeField] Sprite MC1;
	[SerializeField] Sprite MC2;
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.gameObject.tag == "Player") 
		{
			if (other.transform.GetComponent<SpriteRenderer>().sprite == MC1)
			{
				other.transform.GetComponent<SpriteRenderer>().sprite = MC2;
			} else if (other.transform.GetComponent<SpriteRenderer>().sprite == MC2)
			{
				other.transform.GetComponent<SpriteRenderer>().sprite = MC1;
			}
		}
	}
}
