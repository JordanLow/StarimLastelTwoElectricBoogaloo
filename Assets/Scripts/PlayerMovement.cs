using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveValue = 1f;
    [SerializeField] float jumpSpeed = 5f;
	[SerializeField] BoxCollider2D myFeetCollider;
	[SerializeField] CapsuleCollider2D myHurtBoxCollider;
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
	
	void OnJump(InputValue value)
	{
		if (!isGrounded()) {return;};
		if (value.isPressed)
        {
            myRigidBody.velocity += new Vector2(0f, jumpSpeed);
        }
	}
	
	public bool isGrounded()
	{
		return myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
	}
	
    void Move()
    {
		float sprint = 1f;
		if (Input.GetKey(KeyCode.LeftShift)) 
		{
				sprint = 2f;
		}
        Vector2 playerVelocity = new Vector2(moveInput.x * moveValue * sprint, myRigidBody.velocity.y);
		myRigidBody.velocity = playerVelocity;
    }
}