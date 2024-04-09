using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
	[SerializeField] private PlayerObject _pr;
	[SerializeField] private float _initialCollectDelay = 0.5f;
	[SerializeField] private MMF_Player _pickUpFeedback;

	private ItemObject _item;
	private List<ItemParameter> _currentParameters;
	private SpriteRenderer _sr;
	private int _currentStack;
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
			Debug.Log("Item Collected");
			// maybe put item add functionality here
		}
	}
	
	public void Initialize(ItemObject item, int currentStack, List<ItemParameter> currentParameters)
	{
		_item = item;
		_currentStack = currentStack;
		_currentParameters = currentParameters;
		_sr.sprite = item.UiDisplay;
		
		gameObject.name = $"[Item] {item.Name}";
	}
	
	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.TryGetComponent(out CollectTag collectTag) && _canCollect && !_collected)
		{
			if (_attractor.CanAttract)
			{
				// var leftover = _pr.Inventory.AddItem(_item, _currentStack, _currentParameters);

				// if (leftover == 0)
				// {
				// 	_collected = true;
				// 	Destroy(gameObject);
				// }
				// else
				// {
				// 	_currentStack = leftover;
				// 	_attractor.DisableCanAttract(_pr, _item);
				// }
			}
		}
	}
}
