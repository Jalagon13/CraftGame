using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "New Settler Object", menuName = "Create Item/New Settler Object")]
public class SettlerObject : ItemObject
{
	[SerializeField] private GameObject _settlerToDelpoy;
	[SerializeField] private AudioClip _deploySound;
	
	public override void ExecutePrimaryAction(FocusItemController controller)
	{
		Vector2 pos = controller.CursorControl.transform.position;
		bool onSpawnFloor = controller.SpawnFloorTilemap.Tilemap.HasTile(Vector3Int.FloorToInt(pos));
		bool inValidHouse = controller.InValidHouse(); // TO DO Later
		
		if(controller.IsClear(pos) && onSpawnFloor && inValidHouse)
		{
			PlaceDownPrefab(pos);
			controller.PlayerObject.PlayerInventory.RemoveItem(this, 1); // Note to future self: This implementation is bugged and will need fixing later
		}
	}

	public override void ExecuteSecondaryAction(FocusItemController controller)
	{
		
	}
	
	private void PlaceDownPrefab(Vector2 position)
	{
		Vector2 spawnPosition = new(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
		Instantiate(_settlerToDelpoy, spawnPosition, Quaternion.identity);
		
		MMSoundManagerSoundPlayEvent.Trigger(_deploySound, MMSoundManager.MMSoundManagerTracks.Sfx, default);
	}

	public override string GetDescription()
	{
		return string.Empty;
	}
}
