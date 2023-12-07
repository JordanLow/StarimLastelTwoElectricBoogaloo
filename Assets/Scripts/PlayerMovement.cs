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
	[SerializeField] GameObject grass; // Type of Grass to spawn

	bool onSpawner = false; // If floor is already marked for grass growth
	int direction = 1; // 1: Facing Right, -1: Facing Left

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
		float jump = jumpSpeed;
		if (onGrass()) {jump *= 2;} // Forest Leap
		if (value.isPressed)
        {
            myRigidBody.velocity += new Vector2(0f, jump);
        }
	}
	
	public bool isGrounded()
	{
		return myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
	}
	
	bool onGrass()
	{
		return myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Grass"));
	}
	
	bool grassSpawnCheck() // Check if current ground is valid for grass growth
	{
		return !Input.GetKey(KeyCode.LeftShift) && isGrounded() && moveInput.x != 0 && !myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Grass")) && !onSpawner;
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
		FlipFacing();
		if (grassSpawnCheck()) 
		{
			Vector3 grassSpawn = new Vector3(Mathf.Round(transform.position.x * 2) / 2, Mathf.Round(transform.position.y * 2)  /  2, transform.position.z);
			Instantiate(grass, grassSpawn, transform.rotation);
			OnSpawner();
		}
    }
	
	void FlipFacing()
    {
		if (!isGrounded()) {return;}
		bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
		if (playerHasHorizontalSpeed)
		{
			transform.localScale = new Vector2((Mathf.Sign(myRigidBody.velocity.x)) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
			direction = ((int)Mathf.Sign(myRigidBody.velocity.x));
		}
    }
	
	public void OnSpawner() {
		onSpawner = true;
	}
	
	public void OffSpawner() {
		onSpawner = false;
	}
}