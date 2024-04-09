using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
	[SerializeField] private float _attractSpeed;
	[SerializeField] private float _initialAttractDelay = 0.65f;
	private PlayerObject _po;
	private ItemObject _item;

	private bool _canAttract;

	public bool CanAttract { get { return _canAttract; } }
	
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(_initialAttractDelay);
		_canAttract = true;
	}
	
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out CollectTag collectTag))
		{
			if (_canAttract)
				transform.root.position = Vector2.MoveTowards(transform.root.position, collision.transform.root.position, _attractSpeed * Time.deltaTime);
			else if (_po != null && _item != null /* && _po.Inventory.CanAddItem(_item) */)
			{
				_canAttract = true;
			}
		}
	}
	
	public void DisableCanAttract(PlayerObject pr, ItemObject item)
	{
		_po = pr;
		_item = item;
		_canAttract = false;
	}
}
