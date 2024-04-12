using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private float _initialCollectDelay = 0.5f;
	[SerializeField] private MMF_Player _pickUpFeedback;

	private InventoryItem _inventoryItem;
	private SpriteRenderer _sr;
	private bool _canCollect;
	private bool _collected;
	private Attractor _attractor;
	
	private void Awake()
	{
		_sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
		_attractor = gameObject.GetComponentInChildren<Attractor>();
	}
	
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(_initialCollectDelay);
		_canCollect = true;
	}
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out CollectTag collectTag) && _canCollect && !_collected)
		{
			_pickUpFeedback?.PlayFeedbacks();
			_po.PlayerInventory.CollectItem(_inventoryItem);
			Destroy(gameObject);
		}
	}
	
	public void Initialize(InventoryItem inventoryItem)
	{
		_inventoryItem = inventoryItem;
		_sr.sprite = _inventoryItem.Item.UiDisplay;
		
		gameObject.name = $"[Item] {_inventoryItem.Item.Name}";
	}
}
