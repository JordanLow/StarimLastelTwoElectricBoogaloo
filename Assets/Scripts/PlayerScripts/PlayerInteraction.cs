using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    IInteractable currentInteractable;

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("hi");
        //currentInteractable = other.gameObject.GetComponent<InteractableLogic>();
        if (other.gameObject.tag == "Interactable")
        {
            //Debug.Log("Inside");
            currentInteractable = other.gameObject.GetComponent<IInteractable>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        currentInteractable = null;
    }

    void OnInteract(InputValue value)
    {
		//Debug.Log(currentInteractable);
        if (currentInteractable == null){return;}
        currentInteractable.Interact();
    }
}
