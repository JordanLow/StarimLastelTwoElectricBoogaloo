using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] int moveValue = 1;
    Vector2 moveInput;
    Rigidbody2D myRigidBody;
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }   
    void Move()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveValue, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
    }
}
