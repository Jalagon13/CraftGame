using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	[SerializeField] private GameObject _itemBasePrefab;
	
	public void SpawnItem(Vector2 worldPos, InventoryItem inventoryItem, bool playAudio = true)
	{
		GameObject newItemGo = Instantiate(_itemBasePrefab, worldPos, Quaternion.identity);
		ItemBehavior newItem = newItemGo.GetComponent<ItemBehavior>();

		if (playAudio)
		{
			// MMSoundManagerSoundPlayEvent.Trigger(_popSound, MMSoundManager.MMSoundManagerTracks.UI, default);
		}
		
		if (!inventoryItem.Item.Stackable || inventoryItem.Quantity <= 0)
		{
				inventoryItem.Quantity = 1;
		}
		
		newItem.Initialize(inventoryItem);
	}
}
