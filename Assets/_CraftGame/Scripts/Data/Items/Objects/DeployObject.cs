using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "New Deployable", menuName = "Create Item/New Deployable")]
public class DeployObject : ItemObject
{
	[SerializeField] private GameObject _prefabToDeploy;
	[SerializeField] private AudioClip _deploySound;
	
	public override void ExecutePrimaryAction(FocusItemController controller)
	{
		Vector2 pos = controller.CursorControl.transform.position;
		
		if(controller.IsClear(pos))
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
		Instantiate(_prefabToDeploy, spawnPosition, Quaternion.identity);
		
		MMSoundManagerSoundPlayEvent.Trigger(_deploySound, MMSoundManager.MMSoundManagerTracks.Sfx, default);
	}
	
	public override string GetDescription()
	{
		return string.Empty;
	}
}