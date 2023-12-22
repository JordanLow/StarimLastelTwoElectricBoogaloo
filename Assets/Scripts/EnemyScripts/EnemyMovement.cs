using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] int moveValue = 1;
    Rigidbody2D myRigidBody;
    BoxCollider2D enemyVision;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        enemyVision = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        myRigidBody.velocity = new Vector2(-moveValue, 0);
        //Movement
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (!enemyVision.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            moveValue = -moveValue;
            //Flips the movement Direction
            FlipEnemyFacing();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Grass")
        {
            Destroy(other.transform.parent.gameObject);
            //Destroys Grass object since collider is in child
        }
		if (enemyVision.IsTouchingLayers(LayerMask.GetMask("Wall")))
        {
            moveValue = -moveValue;
            //Flips the movement Direction
            FlipEnemyFacing();
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.GetComponent<PlayerDeath>().Die();
            //Kills Player
        }
    }
    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-Math.Sign(myRigidBody.velocity.x), 1f);
        //Flips the x scale of sprite, changing its facing.
    }
}
