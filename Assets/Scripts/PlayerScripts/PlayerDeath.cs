using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public void Die()
    {
        Debug.Log("Player Dies");
        SceneHandler.instance.ReloadScene();
    }
}
