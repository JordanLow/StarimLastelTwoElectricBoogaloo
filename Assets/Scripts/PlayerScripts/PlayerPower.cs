using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerPower : MonoBehaviour
{
    Tilemap vineTilemap;
	Tilemap groundTilemap;
	Tilemap softGroundTilemap;
    Tile vineTile;
	Tilemap grassTilemap;
	RuleTile grassTile;
	PlayerReferences playerReferences;
    [SerializeField] int spriteWidth = 3;
	[SerializeField] int spriteHeight = 2;
	[SerializeField] float growMagnitude = 1f;
	[SerializeField] float dashDuration = 1f;
    //Probably a better way to get this data, just using this for now
    //TODO find better way to get this data

    PlayerResource playerResource;
    PlayerMovement playerMovement;
	Rigidbody2D playerRigidbody2D;
    bool existingVine = false;
	float dashTimer = 0f;
	float gravityScale;

    // Start is called before the first frame update
    void Start()
    {
		playerReferences = GetComponent<PlayerReferences>();
        playerResource = GetComponent<PlayerResource>();
        playerMovement = GetComponent<PlayerMovement>();
		playerRigidbody2D = GetComponent<Rigidbody2D>();
		gravityScale = playerRigidbody2D.gravityScale;
		//Player References
		vineTilemap = playerReferences.vineTilemap;
		groundTilemap = playerReferences.groundTilemap;
		vineTile = playerReferences.vineTile;
		grassTilemap = playerReferences.grassTilemap;
		grassTile = playerReferences.grassTile;
		softGroundTilemap = playerReferences.softGroundTilemap;
    }

	void OnDash()
	{
		playerMovement.isDashing = true;
		playerRigidbody2D.gravityScale = 0;
	}
	void Update()
    {
        if (playerMovement.isMoving() && playerMovement.isGrounded()) 
		{
			//Debug.Log(((int)(spriteWidth/2)).ToString());
			for (int offset = 0 - (int)(spriteWidth/2); offset <= (int)(spriteWidth/2); offset++)
			{
				Vector3Int grassSpawn = new Vector3Int((int)math.floor(transform.localPosition.x) + offset, (int)math.round(transform.localPosition.y) - 1, (int)transform.localPosition.z);
				if (grassSpawnable(grassSpawn))
				{
					grassTilemap.SetTile(grassSpawn, grassTile);
				}
			}
			//Instantiate(grass, grassSpawn, transform.rotation);
			//OnSpawner();
		}
		DashHandler();
    }
	
	//Flourish Dive
	void OnCollisionEnter2D(Collision2D collision)
	{
		//Debug.Log("here");
		if (playerMovement.isGrounded())
		{
			//GrowVines
			for (int offset = (int)(0 - collision.relativeVelocity.y * growMagnitude) ; offset <= (int)(collision.relativeVelocity.y * growMagnitude); offset++)
			{
				Vector3Int grassSpawn = new Vector3Int((int)math.floor(transform.localPosition.x) + offset, (int)math.round(transform.localPosition.y) - 1, (int)transform.localPosition.z);
				if (grassSpawnable(grassSpawn))
				{
					grassTilemap.SetTile(grassSpawn, grassTile);
				}
			}
		}
	}
	void clearVines()
	{
		vineTilemap.ClearAllTiles();
		existingVine = false;
	}

	bool grassSpawnable(Vector3Int coordinates)
	{
		//Checks if tile is a ground tile
		if (groundTilemap.GetTile(coordinates) != null){return false;}
		if (softGroundTilemap.GetTile(coordinates) != null){return false;}
		coordinates.y -= 1;
		//Checks if the tile below is ground
		if (groundTilemap.GetTile(coordinates) == null && softGroundTilemap.GetTile(coordinates) == null){return false;}
		return true;
	}
	
	// Event Listeners
	private void OnEnable()
	{
		EventHandler.OnExitLevel += clearVines;
	}
	private void OnDisable()
	{
		EventHandler.OnExitLevel -= clearVines;
	}
	void DashHandler()
	{
		if (!playerMovement.isDashing){return;}
		if (dashTimer >= dashDuration)
		{
			playerMovement.isDashing = false;
			dashTimer = 0f;
			playerRigidbody2D.gravityScale = gravityScale;
		}
		dashTimer += Time.deltaTime;
	}
}
