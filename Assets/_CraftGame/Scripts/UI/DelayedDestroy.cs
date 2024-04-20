using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedDestroy : MonoBehaviour
{
	[SerializeField] private float _disappearTimer;
	
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(_disappearTimer);
		Destroy(gameObject);
	}
}
