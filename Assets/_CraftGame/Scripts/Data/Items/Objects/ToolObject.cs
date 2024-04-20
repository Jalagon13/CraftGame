using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolType
{
	None,
	Axe,
	Pickaxe,
	Sword,
	Shovel,
	Hammer
}

[CreateAssetMenu(fileName = "New Tool", menuName = "Create Item/New Tool")]
public class ToolObject : ItemObject
{
	[SerializeField] private ToolType _type;
	
	public ToolType ToolType => _type;

	public override void ExecutePrimaryAction(FocusItemController controller)
	{
		
	}

	public override void ExecuteSecondaryAction(FocusItemController controller)
	{
		
	}

	public override string GetDescription()
	{
		return string.Empty;
	}
}
