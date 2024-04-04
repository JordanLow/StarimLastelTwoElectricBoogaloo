using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using System;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveValue = 1f;
	public float dashValue = 2f;
	[SerializeField] float phaseDuration = 2f;
	[SerializeField] float grassMoveValue = 4f;
    [SerializeField] float jumpHeight = 30f;
	[SerializeField] float forestLeapBoost = 2f;
	//[SerializeField] float climbSpeed = 1f;
	BoxCollider2D myFeetCollider;
	PolygonCollider2D myHurtBoxCollider;
	//[SerializeField] GameObject grass; // Type of Grass to spawn
	GameObject currentInteractingGrass;
	
	//[SerializeField] Animator animator;
	Tilemap softGroundTilemap;

	//bool tryingToClimb = false;
	int direction = 1; // 1: Facing Right, -1: Facing Left
	float phaseTimer = 0;

    Vector2 moveInput;
    Rigidbody2D myRigidBody;
	TilemapCollider2D groundCollider;
	PlayerReferences playerReferences;
	public bool isDashing {get; set;}

    // Start is called before the first frame update
    void Start()
    {
		playerReferences = GetComponent<PlayerReferences>();
        myRigidBody = GetComponent<Rigidbody2D>();
		myFeetCollider = GetComponentInChildren<BoxCollider2D>();
		myHurtBoxCollider = GetComponent<PolygonCollider2D>();
		//Drawing from references
		softGroundTilemap = playerReferences.softGroundTilemap;
		groundCollider = softGroundTilemap.GetComponent<TilemapCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
		FlipFacing();
		Phase();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
		//Debug.Log(moveInput.x.ToString());
    }
	
	void OnJump(InputValue value)
	{
		if (Input.GetKey(KeyCode.S))
		{
			//Checking if not on ground or if platform is not soft
			if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("SoftGround"))){return;}
			//Phase through soft floor
			groundCollider.enabled = false;
			phaseTimer = 0;
			//Debug.Log("Here");
		}
		if (!isGrounded()) {return;};
		float jump = jumpHeight;
		if (onGrass()) {jump *= forestLeapBoost;} // Forest Leap
		if (value.isPressed)
        {
            myRigidBody.velocity += new Vector2(0f, jump);
        }
	}
	
	public bool isMoving()
	{
		bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
		return playerHasHorizontalSpeed;
	}
	public bool isGrounded()
	{
		return myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) | myFeetCollider.IsTouchingLayers(LayerMask.GetMask("SoftGround"));
	}
	
	bool onGrass()
	{
		return myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Grass"));
	}
	
    void Move()
    {
		float move = moveValue;
		if (onGrass())
		{
			move = grassMoveValue;
		}
		Vector2 playerVelocity = new Vector2(moveInput.x * move, myRigidBody.velocity.y);
		if (isDashing)
		{
			playerVelocity = new Vector2(transform.localScale.x * dashValue, 0);
		}
		myRigidBody.velocity = playerVelocity;
		//TODO IMPLEMENT THIS, CURRENTLY COMMENTED OUT BECAUSE CLOGGING TERMINAL
		//animator.SetBool("Walking", true);
		/*
		if (Input.GetKey(KeyCode.LeftShift)) 
		{
			//animator.SetBool("Sprinting", true);
			animator.SetBool("Walking", false);
			move = sprintValue;
			if (onGrass())
			{
				move = grassSprintValue;
			}
		} else {
			//animator.SetBool("Sprinting", false);
			animator.SetBool("Walking", true);
		}
		*/
		/*if (moveInput.x == 0f) {
			//animator.SetBool("Sprinting", false);
			animator.SetBool("Walking", false);
		}
		if (!isGrounded()) {
			animator.SetBool("Airborne", true);
		} else {
			animator.SetBool("Airborne", false);
		}
		*/
		// Refactor our of Unity Animator State Machine when possible, this bit here sucks. Set params by ID as interim measure if needed
    }
	
	void FlipFacing()
    {
		if (isMoving())
		{
			transform.localScale = new Vector2((Mathf.Sign(myRigidBody.velocity.x)) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
			direction = ((int)Mathf.Sign(myRigidBody.velocity.x));
		}
    }
	/*
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Grass" && Input.GetKey(KeyCode.LeftShift))
		{
			Destroy(other.transform.parent.gameObject);
		}
	}
	*/
	void Phase()
	{
		if (groundCollider.enabled){return;}
		if (phaseTimer >= phaseDuration)
		{
			groundCollider.enabled = true;
		}
		phaseTimer += Time.deltaTime;
	}
}