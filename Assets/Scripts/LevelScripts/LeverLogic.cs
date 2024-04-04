using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LeverLogic : MonoBehaviour, IInteractable
{
    SpriteRenderer selfSpriteRenderer;
	[SerializeField] float lockout = 1f;
	float timer = 0f;

    [SerializeField] GameObject linkedDoor;
    bool on = false;
    [SerializeField] Sprite leverOn;
    [SerializeField] Sprite leverOff;
	
	void Start() {
		selfSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
	}
	
	void Update() {
		if (timer > 0) {
			timer -= Time.deltaTime;
			if (timer <= 0f) {
				timer = 0;
			}
		}
	}
	
    public void Interact()
    {
		if (timer > 0) return;
		timer = lockout;
        Debug.Log("Interacted");
        //interact logic (polymorphic for future use)
		on = !on;
		linkedDoor.GetComponent<DoorLogic>().toggle();
		// Make and play the door animation
		// Door turning off its collision is a property of the door itself.
		if (on)
		{
			selfSpriteRenderer.sprite = leverOn;
		}
		else
		{
			selfSpriteRenderer.sprite = leverOff;
		}
    }
}
