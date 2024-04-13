using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolType
{
	None,
	Axe,
	Pickaxe,
	Sword,
	Hammer
}

[CreateAssetMenu(fileName = "New Tool", menuName = "Create Item/New Tool")]
public class ToolObject : ItemObject
{
	[SerializeField] private ToolType _type;
	
	public ToolType ToolType => _type;
	
	public override string GetDescription()
	{
		return string.Empty;
	}
}
