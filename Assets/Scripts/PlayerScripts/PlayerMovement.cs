using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveValue = 1f;
	[SerializeField] float sprintValue = 2f;
	[SerializeField] float grassSprintValue = 4f;
    [SerializeField] float jumpHeight = 30f;
	[SerializeField] float forestLeapBoost = 2f;
	[SerializeField] float climbSpeed = 1f;
	[SerializeField] BoxCollider2D myFeetCollider;
	[SerializeField] CapsuleCollider2D myHurtBoxCollider;
	[SerializeField] GameObject grass; // Type of Grass to spawn
	GameObject currentInteractingGrass;
	
	[SerializeField] Animator animator;

	bool onSpawner = false; // If floor is already marked for grass growth
	bool tryingToClimb = false;
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
		FlipFacing();
		ClimbVine();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
	
	void OnJump(InputValue value)
	{
		if (isOnVine())
		{
			if (value.isPressed)
			{
				tryingToClimb = true;
			}
			else
			{
				tryingToClimb = false;
			}
			return;
		}
		tryingToClimb = false;
		//To ensure climbing always stops when space is released
		//Climbing Vine Interactions
		if (!isGrounded()) {return;};
		float jump = jumpHeight;
		if (onGrass()) {jump *= forestLeapBoost;} // Forest Leap
		if (value.isPressed)
        {
            myRigidBody.velocity += new Vector2(0f, jump);
        }
	}
	
	public bool isGrounded()
	{
		return myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
	}
	
	public bool isOnVine()
	{
		return myHurtBoxCollider.IsTouchingLayers(LayerMask.GetMask("Vine"));
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
		float move = moveValue;
		if (Input.GetKey(KeyCode.LeftShift)) 
		{
			animator.SetBool("Sprinting", true);
			animator.SetBool("Walking", false);
			move = sprintValue;
			if (onGrass())
			{
				move = grassSprintValue;
			}
		} else {
			animator.SetBool("Sprinting", false);
			animator.SetBool("Walking", true);
		}
		
		if (moveInput.x == 0f) {
			animator.SetBool("Sprinting", false);
			animator.SetBool("Walking", false);
		}
		if (!isGrounded()) {
			animator.SetBool("Airborne", true);
		} else {
			animator.SetBool("Airborne", false);
		}
		// Refactor our of Unity Animator State Machine when possible, this bit here sucks. Set params by ID as interim measure if needed
		
        Vector2 playerVelocity = new Vector2(moveInput.x * move, myRigidBody.velocity.y);
		myRigidBody.velocity = playerVelocity;
		if (grassSpawnCheck()) 
		{
			Vector3 grassSpawn = new Vector3(Mathf.Round(transform.position.x * 2) / 2, Mathf.Round(transform.position.y * 2)  /  2, transform.position.z);
			Instantiate(grass, grassSpawn, transform.rotation);
			OnSpawner();
		}
    }
	
	void FlipFacing()
    {
		//if (!isGrounded()) {return;} this is not meant to be here
		bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
		if (playerHasHorizontalSpeed)
		{
			transform.localScale = new Vector2((Mathf.Sign(myRigidBody.velocity.x)) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
			direction = ((int)Mathf.Sign(myRigidBody.velocity.x));
		}
    }
	void ClimbVine()
	{
		if (!tryingToClimb){return;}
		//dont climb if button not pressed
		if (!isOnVine()){return;}
		//dont climb if not on a vine
		Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, climbSpeed);
		myRigidBody.velocity = climbVelocity;
	}
	
	public void OnSpawner() {
		onSpawner = true;
	}
	
	public void OffSpawner() {
		onSpawner = false;
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Grass" && Input.GetKey(KeyCode.LeftShift))
		{
			Destroy(other.transform.parent.gameObject);
		}
	}
}