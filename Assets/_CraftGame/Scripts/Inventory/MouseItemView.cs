using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseItemView : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	
	private void Update()
	{
		UpdatePosition();
	}
	
	private void UpdatePosition()
	{
		transform.position = Camera.main.WorldToScreenPoint(_po.MousePosition);
	}
	
	public void Initialize()
	{
		
	}
}
