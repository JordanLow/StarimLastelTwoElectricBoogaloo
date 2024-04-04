using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] Animator FadeAnimator;

    public void Die()
    {
        Debug.Log("Player Dies");
		FadeAnimator.SetTrigger("FadeIn");
        //SceneHandler.instance.ReloadScene();
        gameObject.transform.position = new Vector3(0,0,0);
        FadeAnimator.SetTrigger("FadeOut");
    }
}
