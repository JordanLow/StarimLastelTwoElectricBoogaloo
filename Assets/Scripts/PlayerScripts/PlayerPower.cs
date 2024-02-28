using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerPower : MonoBehaviour
{
    [SerializeField] Tilemap vineTilemap;
	[SerializeField] Tilemap groundTilemap;
    [SerializeField] Tile vineTile;
    //Probably a better way to get this data, just using this for now
    //TODO find better way to get this data

    PlayerResource playerResource;
    PlayerMovement playerMovement;
    bool existingVine = false;

    // Start is called before the first frame update
    void Start()
    {
        playerResource = GetComponent<PlayerResource>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void OnPowerUp(InputValue value)
    {
		//powerup used
		//Debug.Log("PowerUp Used");
		if (playerResource.powerUpVine && playerMovement.isGrounded())
		{
			Vector3Int vineStart;
			vineStart = new Vector3Int((int)math.floor(transform.localPosition.x), (int)math.round(transform.localPosition.y), (int)transform.localPosition.z);
			//Finds Vine start location based on player character position & facing direction
			
			Vector3Int currentReferenceWallTile = new Vector3Int(vineStart.x + (int)transform.localScale.x, vineStart.y, vineStart.z);
			Vector3Int currentVineToPlace = new Vector3Int(vineStart.x, vineStart.y, vineStart.z);
			//Checking if there is a wall for the vine to grow on
			
			if (existingVine && groundTilemap.GetTile(currentReferenceWallTile) != null)
			{
				clearVines();
			}

			while (groundTilemap.GetTile(currentReferenceWallTile) != null && groundTilemap.GetTile(currentVineToPlace) == null) // Checking if the wall ends or theres a ceiling
			{
				vineTilemap.SetTile(currentVineToPlace, vineTile);
				//Vine Growing
				
				currentReferenceWallTile += new Vector3Int(0, 1, 0);
				currentVineToPlace += new Vector3Int(0, 1, 0);
				//Iterating
				
				existingVine = true;
				//Only sets to true if vine grows;
			}
		}
	}
	
	void clearVines()
	{
		vineTilemap.ClearAllTiles();
		existingVine = false;
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
}
