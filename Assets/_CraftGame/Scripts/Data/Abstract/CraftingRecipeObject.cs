using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipeObject : ScriptableObject
{
	public ItemObject OutputItem;
	public int OutputAmount;
	public float CraftingTimer;
	public bool OneTimeCraftable;
	public List<InventoryItem> ResourceList = new();
}
