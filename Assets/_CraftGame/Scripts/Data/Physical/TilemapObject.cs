using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "[TMR] ", menuName = "New Reference/Tilemap Reference")]
public class TilemapObject : ScriptableObject
{
	[SerializeField] private DynamicTilemap _dynamicTm;
	public Tilemap Tilemap { get { return DynamicTilemap.Tilemap; } }
	public DynamicTilemap DynamicTilemap { get { return _dynamicTm; } set { _dynamicTm = value; } }
}
