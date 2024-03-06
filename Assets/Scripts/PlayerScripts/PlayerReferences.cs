using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerReferences : MonoBehaviour
{
    public Tilemap vineTilemap;
	public Tilemap groundTilemap;
    public Tile vineTile;
	public Tilemap grassTilemap;
	public RuleTile grassTile;
    public Tilemap softGroundTilemap;
}
