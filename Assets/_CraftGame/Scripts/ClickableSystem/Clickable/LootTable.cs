using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class LootTable
{
	public List<Loot> Table;
	
	public void SpawnLoot(Vector2 spawnPos)
	{
		foreach (var loot in Loot())
		{
			// Spawn Loot here.
			InventoryItem inventoryItem = new()
			{
				Item = loot.Key,
				Quantity = loot.Value	
			};
			
			GameManager.Instance.SpawnItem(spawnPos, inventoryItem);
		}
	}
	
	public Dictionary<ItemObject, int> Loot()
	{
		Dictionary<ItemObject, int> lootReturn = new();

		foreach (Loot loot in Table)
		{
			if (Random.Range(0, 100) < loot.Chance)
			{
				int dropAmount = Random.Range(loot.Min, loot.Max + 1);
				lootReturn.Add(loot.Item, dropAmount);
			}
		}

		return lootReturn;
	}
}

[Serializable]
public class Loot 
{
	public ItemObject Item;
	public int Min;
	public int Max;
	[Range(0.0f, 100.0f)]
	public float Chance;
}
