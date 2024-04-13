using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionTotem : MonoBehaviour
{
	private bool _selected;
	
	#region Select Methods
	private void Selected()
	{
		_selected = true;
	}
	
	private void UnSelected()
	{
		_selected = false;
	}
	private void OnMouseOver()
	{
		Selected();
	}

	private void OnMouseExit()
	{
		UnSelected();
	}
	
	private void OnTriggerStay2D(Collider2D other)
	{
		if(other.TryGetComponent(out CursorControl cc))
		{
			Selected();
		}
	}
	
	private void OnTriggerExit2D(Collider2D other)
	{
		if(other.TryGetComponent(out CursorControl cc))
		{
			UnSelected();
		}
	}
	#endregion
}
