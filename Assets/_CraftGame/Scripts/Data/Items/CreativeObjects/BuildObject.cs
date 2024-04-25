using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "New Build Object", menuName = "Create Item/New Build Object")]
public class BuildObject : ItemObject
{
	[SerializeField] private RuleTileExtended _wallTile;
	[SerializeField] private RuleTileExtended _floorTile;
	
	public override void ExecutePrimaryAction(FocusItemController controller)
	{
		var pos = Vector3Int.FloorToInt(controller.CursorControl.gameObject.transform.position);
		
		bool wallTmHasTile = controller.WallTm.Tilemap.HasTile(Vector3Int.FloorToInt(controller.CursorControl.gameObject.transform.position));
		
		if(controller.IsClear(new(pos.x, pos.y)) && !wallTmHasTile)
		{
			controller.WallTm.Tilemap.SetTile(pos, _wallTile);
			controller.PlayerObject.PlayerInventory.RemoveItem(this, 1); // Note to future self: This implementation is bugged and will need fixing later
			MMSoundManagerSoundPlayEvent.Trigger(_wallTile.PlaceSound, MMSoundManager.MMSoundManagerTracks.Sfx, default, pitch:UnityEngine.Random.Range(0.9f, 1.1f));
		}
	}

	public override void ExecuteSecondaryAction(FocusItemController controller)
	{
		var pos = Vector3Int.FloorToInt(controller.CursorControl.gameObject.transform.position);
		
		bool floorTmHasTile = controller.FloorTm.Tilemap.HasTile(Vector3Int.FloorToInt(controller.CursorControl.gameObject.transform.position));
		
		if(!floorTmHasTile)
		{
			controller.FloorTm.Tilemap.SetTile(pos, _floorTile);
			controller.PlayerObject.PlayerInventory.RemoveItem(this, 1); // Note to future self: This implementation is bugged and will need fixing later
			MMSoundManagerSoundPlayEvent.Trigger(_floorTile.PlaceSound, MMSoundManager.MMSoundManagerTracks.Sfx, default, pitch:UnityEngine.Random.Range(0.9f, 1.1f));
		}
	}

	public override string GetDescription()
	{
		return string.Empty;
	}
}
