using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemObject : ScriptableObject
{
	[field: SerializeField] public string Name { get; private set; }
	[field: SerializeField] public Sprite UiDisplay { get; private set; }
	[field: SerializeField] public bool Stackable { get; private set; }
	[field: TextArea]
	[field: SerializeField] public string Description { get; private set; }
	[field: SerializeField] public List<ItemParameter> DefaultParameterList { get; set; }
	
	public abstract void ExecutePrimaryAction(FocusItemController controller);
	public abstract void ExecuteSecondaryAction(FocusItemController controller);
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