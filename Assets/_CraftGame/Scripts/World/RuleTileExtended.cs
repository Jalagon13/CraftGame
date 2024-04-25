using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "New Extended Rule Tile")]
	public class RuleTileExtended : RuleTile
	{
		public ItemObject Item;
		public AudioClip HitSound;
		public AudioClip PlaceSound;
		public AudioClip DestroySound;
		public int MaxHitPoints;
		public ToolType HitToolType;
	}
