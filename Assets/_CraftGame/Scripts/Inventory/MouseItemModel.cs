using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseItemModel
{
	private InventoryItem _mouseInventoryItem;
	
	public InventoryItem MouseInventoryItem { get { return _mouseInventoryItem; } set { _mouseInventoryItem = value;}}
	
	public void OverrideMouseItem(InventoryItem item)
	{
		_mouseInventoryItem = item;
	}
}
