using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using Unity.Mathematics;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] int moveValue = 1;
    Rigidbody2D myRigidBody;
    CapsuleCollider2D enemyVision;
    Animator enemyAnimator;
    Tilemap grassTilemap;
    EnemyReferences enemyReferences;
    bool converted = false;

    void Start()
    {
        enemyReferences = GetComponent<EnemyReferences>();
        enemyAnimator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        enemyVision = GetComponent<CapsuleCollider2D>();
        grassTilemap = enemyReferences.grassTilemap;
    }

    void Update()
    {
        myRigidBody.velocity = new Vector2(-moveValue, 0);
        //Movement
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (!enemyVision.IsTouchingLayers(LayerMask.GetMask("Ground")) && !enemyVision.IsTouchingLayers(LayerMask.GetMask("SoftGround")))
        {
            moveValue = -moveValue;
            //Flips the movement Direction
            FlipEnemyFacing();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player" && !converted)
        {
            if (other.transform.GetComponent<PlayerMovement>().isDashing)
            {
                ConvertEnemy();
            }
            else
            {
                other.transform.GetComponent<PlayerDeath>().Die();
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag == "Grass" && !converted)
        {
            RemoveGrass();
        }
    }
    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-Math.Sign(myRigidBody.velocity.x), 1f);
        //Flips the x scale of sprite, changing its facing.
    }

    void RemoveGrass()
    {
        Vector3Int interactedGrass = new Vector3Int((int)math.floor(transform.localPosition.x - transform.localScale.x), (int)math.round(transform.localPosition.y) - 1, (int)transform.localPosition.z);
        grassTilemap.SetTile(interactedGrass, null);
    }
    void ConvertEnemy()
    {
        enemyAnimator.SetBool("isConverted", true);
        converted = true;
    }
}
