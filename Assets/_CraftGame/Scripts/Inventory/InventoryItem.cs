using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
	public ItemObject Item;
	public int Quantity;
	
	public bool HasItem()
	{
		return Item != null;
	}
}