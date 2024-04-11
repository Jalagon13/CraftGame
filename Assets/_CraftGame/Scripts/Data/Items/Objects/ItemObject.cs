using System;
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

public abstract class ItemObject : ScriptableObject
{
	[field: SerializeField] public string Name { get; private set; }
	[field: SerializeField] public Sprite UiDisplay { get; private set; }
	[field: SerializeField] public bool Stackable { get; private set; }
	[field: TextArea]
	[field: SerializeField] public string Description { get; private set; }
	[field: SerializeField] public List<ItemParameter> DefaultParameterList { get; set; }
	
	// public abstract void ExecutePrimaryAction(FocusSlotControl control);
	// public abstract void ExecuteSecondaryAction(FocusSlotControl control);
	public abstract string GetDescription();
}

[Serializable]
public struct ItemParameter : IEquatable<ItemParameter>
{
	public ItemParameterObject Parameter;
	public float Value;

	public bool Equals(ItemParameter other)
	{
		return other.Parameter == Parameter;
	}
}