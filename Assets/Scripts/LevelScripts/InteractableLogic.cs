using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InteractableLogic : MonoBehaviour
{
    [SerializeField] string interactableType;
    SpriteRenderer selfSpriteRenderer;

    //variables for lever
    Tilemap linkedDoor;
    bool on = false;
    [SerializeField] Sprite leverOn;
    [SerializeField] Sprite leverOff;

    void Start()
    {
        selfSpriteRenderer = GetComponent<SpriteRenderer>();
        //initialization for lever
        if (interactableType == "lever")
        {
            linkedDoor = transform.parent.GetComponentInChildren<Tilemap>();
        }
    }

    public void Interact()
    {
        //Debug.Log("Interacted");
        //interact logic
        if (interactableType == "lever")
        {
            on = !on;
            linkedDoor.gameObject.SetActive(!on);
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
}
