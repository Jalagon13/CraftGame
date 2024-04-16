using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CraftingView : MonoBehaviour
{
	private bool _uiActive;
	public bool UiActive { get { return _uiActive; } 
		set 
		{ 
			_uiActive = value;
			LoopThroughChildElements(value);
		} 
	}
	
	private void LoopThroughChildElements(bool enabled)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform childTransform = transform.GetChild(i);
			childTransform.gameObject.SetActive(enabled);
		}
	}
}
