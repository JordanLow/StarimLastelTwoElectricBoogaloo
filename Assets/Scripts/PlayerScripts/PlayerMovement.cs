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
	[SerializeField] float sprintValue = 2f;
	[SerializeField] float phaseDuration = 1f;
	//[SerializeField] float grassSprintValue = 4f;
    [SerializeField] float jumpHeight = 30f;
	[SerializeField] float forestLeapBoost = 2f;
	[SerializeField] float climbSpeed = 1f;
	[SerializeField] int spriteWidth = 3;
	//Currently only works for odd numbers
	BoxCollider2D myFeetCollider;
	PolygonCollider2D myHurtBoxCollider;
	//[SerializeField] GameObject grass; // Type of Grass to spawn
	GameObject currentInteractingGrass;
	
	[SerializeField] Animator animator;
	[SerializeField] RuleTile grassTile;
	[SerializeField] Tilemap grassTilemap;
	[SerializeField] Tilemap groundTilemap;

	bool tryingToClimb = false;
	int direction = 1; // 1: Facing Right, -1: Facing Left
	float phaseTimer = 0;

    Vector2 moveInput;
    Rigidbody2D myRigidBody;
	TilemapCollider2D groundCollider;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
		groundCollider = groundTilemap.GetComponent<TilemapCollider2D>();
		myFeetCollider = GetComponentInChildren<BoxCollider2D>();
		myHurtBoxCollider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
		FlipFacing();
		ClimbVine();
		Phase();
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
		if (Input.GetKey(KeyCode.S))
		{
			//Checking if not on ground or if platform is not soft
			if (!isGrounded() | groundTilemap.GetTile(new Vector3Int((int)math.floor(transform.localPosition.x), (int)math.round(transform.localPosition.y) - 3, (int)transform.localPosition.z)) != null){return;}
			//Phase through soft floor
			groundCollider.enabled = false;
			phaseTimer = 0;
			//Debug.Log("Here");
		}
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
		if (!isGrounded()){
			return false; 
		}
		if (moveInput.x == 0){
			return false;
		}
		bool onGrassTile = (groundTilemap.GetTile(new Vector3Int((int)math.floor(transform.localPosition.x), (int)math.round(transform.localPosition.y) - 2, (int)transform.localPosition.z)) == grassTile);
		if (onGrassTile){
			return false;
		}
		return true;
	}
	
    void Move()
    {
		float move = moveValue;
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
		
        Vector2 playerVelocity = new Vector2(moveInput.x * move, myRigidBody.velocity.y);
		myRigidBody.velocity = playerVelocity;
		if (grassSpawnCheck()) 
		{
			//Debug.Log(((int)(spriteWidth/2)).ToString());
			for (int offset = 0 - (int)(spriteWidth/2); offset <= (int)(spriteWidth/2); offset++)
			{
				Vector3Int grassSpawn = new Vector3Int((int)math.floor(transform.localPosition.x) + offset, (int)math.round(transform.localPosition.y) - 1, (int)transform.localPosition.z);
				bool onGroundTile = groundTilemap.GetTile(new Vector3Int((int)math.floor(transform.localPosition.x) + offset, (int)math.round(transform.localPosition.y) - 2, (int)transform.localPosition.z)) != null;
				if (onGroundTile)
				{
					grassTilemap.SetTile(grassSpawn, grassTile);
				}
			}
			//Instantiate(grass, grassSpawn, transform.rotation);
			//OnSpawner();
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