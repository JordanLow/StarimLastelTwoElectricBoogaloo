using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{
    Animator anim;
	bool open = false;
	
	void Start() {
		anim = gameObject.GetComponent<Animator>();
	}

    public void toggle() {
		// Track and toggle internal state
		// If Door needs to open, play Door Open animation
		open = !open;
		if (open) {
			anim.SetTrigger("Open");
		} else {
			anim.SetTrigger("Close");
		}// If Door needs to close, play Door Close animation
		// Animation itself handles hitboxes etc.
		// Lockout lever until door finish opening?? Should this be here or on lever itself??
	}
}
