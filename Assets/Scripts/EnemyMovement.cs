using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] int moveValue = 1;
    Rigidbody2D myRigidBody;
    BoxCollider2D enemyVision;
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        enemyVision = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        myRigidBody.velocity = new Vector2(moveValue, 0);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (!enemyVision.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            moveValue = -moveValue;
            FlipEnemyFacing();
        }
    }
    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-Math.Sign(myRigidBody.velocity.x), 1f);
    }
}
