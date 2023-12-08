using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerPower : MonoBehaviour
{
    [SerializeField] Tilemap wallTilemap;
    [SerializeField] Tilemap vineTilemap;
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
    void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        if (input.y < 0)
        {
            //powerup used
            //Debug.Log("PowerUp Used");
            if (playerResource.powerUpVine && playerMovement.isGrounded())
            {
                Vector3Int vineStart;
                vineStart = new Vector3Int((int)math.floor(transform.localPosition.x), (int)math.round(transform.localPosition.y), (int)transform.localPosition.z);
                //Finds Vine start location based on player character position & facing direction
                int curVineHeight = 0;
                //Counter for wall and vine height
                Vector3Int currentReferenceWallTile = new Vector3Int(vineStart.x + (int)transform.localScale.x, vineStart.y, vineStart.z);
                //Checking if there is a wall for the vine to grow on
                if (existingVine && wallTilemap.GetTile(currentReferenceWallTile) != null)
                {
                    vineTilemap.ClearAllTiles();
                    existingVine = false;
                }
                while (wallTilemap.GetTile(currentReferenceWallTile) != null) // Checking if the wall is at the top
                {
                    Vector3Int currentVineToPlace = new Vector3Int(vineStart.x, vineStart.y + curVineHeight, vineStart.z);
                    //Setting the vine to place at the current counter
                    vineTilemap.SetTile(currentVineToPlace,vineTile);
                    //Vine Growing
                    curVineHeight++;
                    currentReferenceWallTile = new Vector3Int(vineStart.x + (int)transform.localScale.x, vineStart.y + curVineHeight, vineStart.z);
                    //Iterating
                    existingVine = true;
                    //Only sets to true if vine grows;
                }
            }
        }
    }
}
