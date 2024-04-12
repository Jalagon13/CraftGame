using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	[SerializeField] private GameObject _itemBasePrefab;
	
	public void SpawnItem(Vector2 worldPos, ItemObject item, int stack, List<ItemParameter> parameterList = null, bool playAudio = true)
	{
		GameObject newItemGo = Instantiate(_itemBasePrefab, worldPos, Quaternion.identity);
		ItemBehavior newItem = newItemGo.GetComponent<ItemBehavior>();

		if (playAudio)
		{
			// MMSoundManagerSoundPlayEvent.Trigger(_popSound, MMSoundManager.MMSoundManagerTracks.UI, default);
		}
		
		if (!item.Stackable || stack <= 0)
				stack = 1;

		if (item.DefaultParameterList.Count > 0)
			newItem.Initialize(item, stack, item.DefaultParameterList);
		else
			newItem.Initialize(item, stack, parameterList);
	}
}
