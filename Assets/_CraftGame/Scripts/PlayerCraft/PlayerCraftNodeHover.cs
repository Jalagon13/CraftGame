using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCraftNodeHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private PlayerCraftNodeView _playerCraftNodeView;
	[SerializeField] private Transform _toolTip;
	
	private void Start()
	{
		_toolTip.gameObject.SetActive(false);
	}
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		if(_playerCraftNodeView.Discovered)
		{
			_toolTip.gameObject.SetActive(true);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if(_playerCraftNodeView.Discovered)
		{
			_toolTip.gameObject.SetActive(false);
		}
	}
}
