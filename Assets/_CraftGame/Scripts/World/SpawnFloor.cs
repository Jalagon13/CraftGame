using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnFloor : MonoBehaviour
{
	[SerializeField] private List<RscSpawnSetting> _rscSpawnSettings;

	private Tilemap _spawnTilemap;
	private List<Vector2> _spawnPositions = new();
	private Stack<string> _clearedClickables = new();
	
	[Serializable]
	public class RscSpawnSetting
	{
		public Clickable ClickableToSpawn;
		public int SpawnAmount;
		public float DistancePreference;
	}
	
	private void OnEnable()
	{
		_spawnTilemap = GetComponent<Tilemap>();
		
		// Gather all spawn tile positions
		BoundsInt bounds = _spawnTilemap.cellBounds;
		
		foreach (var position in bounds.allPositionsWithin)
		{
			if(_spawnTilemap.HasTile(position))
			{
				_spawnPositions.Add(new Vector2(position.x, position.y));
			}
		}
		
		RefreshResources();
		StartCoroutine(ResourceSpawner());
		
		GameSignals.CLICKABLE_DESTROYED.AddListener(RegisterClearedClickable);
	}
	
	private void OnDisable()
	{
		StopAllCoroutines();
		
		GameSignals.CLICKABLE_DESTROYED.RemoveListener(RegisterClearedClickable);
	}
	
	private void RegisterClearedClickable(ISignalParameters parameters)
	{
		Clickable clickable = (Clickable)parameters.GetParameter("Clickable");
		_clearedClickables.Push(clickable.Name);
	}
	
	private IEnumerator ResourceSpawner()
	{
		yield return new WaitForSeconds(5f);
		
		if(_clearedClickables.Count > 0)
			{
				string rscToSpawnName = _clearedClickables.Peek();
				
				var randPos = GetRandomPosition();
				Vector3Int pos = new Vector3Int((int)randPos.x, (int)randPos.y, 0);
				
				if(IsValidSpawnPosition(pos))
				{
					foreach(RscSpawnSetting entry in _rscSpawnSettings)
					{
						if(rscToSpawnName == entry.ClickableToSpawn.Name)
						{
							SpawnClickable(entry.ClickableToSpawn, pos);
							_clearedClickables.Pop();
							Debug.Log("Rsc Re-generated");
						}
					}
				}
			}
			
			StartCoroutine(ResourceSpawner());
	}
	
	
	
	private void RefreshResources()
	{
		// Destroy all resources
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
		
		// spawn resources
		foreach(RscSpawnSetting entry in _rscSpawnSettings)
		{
			for (int i = 0; i < entry.SpawnAmount; i++)
			{
				var randPos = GetRandomPosition();
				Vector3Int pos = new Vector3Int((int)randPos.x, (int)randPos.y, 0);
				
				if(IsNearbyResources(new Vector2(pos.x, pos.y), entry.DistancePreference)) 
					continue;
				
				if(IsValidSpawnPosition(pos))
				{
					SpawnClickable(entry.ClickableToSpawn, pos);
				}
			}
		}
	}
	
	private void SpawnClickable(Clickable clickable, Vector3Int spawnPos)
	{
		var r = Instantiate(clickable, spawnPos, Quaternion.identity);
		r.transform.SetParent(transform);
	}
	
	private bool IsValidSpawnPosition(Vector3Int pos)
	{
		if(/* _floorTm.Tilemap.HasTile(pos) || _wallTm.Tilemap.HasTile(pos) ||  */!IsClear(pos))
		{
			return false;
		}
		
		return true;
	}
	
	public bool IsClear(Vector3Int pos)
	{
		var colliders = Physics2D.OverlapBoxAll(new Vector2(pos.x, pos.y) + new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), 0);

		foreach(Collider2D col in colliders)
		{
			if(/* col.gameObject.layer == 3 ||  */col.CompareTag("Clickable")/*  || col.TryGetComponent(out FeetTag ft) */) 
			{
				return false;
			}
		}

		return true;
	}
	
	private bool IsNearbyResources(Vector2 pos, float radiusToCheck)
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(pos + new Vector2(0.5f, 0.5f), radiusToCheck);
		
		foreach(Collider2D col in colliders)
		{
			if(/* col.gameObject.layer == 3 ||  */col.CompareTag("Clickable")/*  || col.TryGetComponent(out FeetTag ft) */) 
			{
				return true;
			}
		}
		
		return false;
	}
	
	private Vector2 GetRandomPosition()
	{
		int randomIndex = UnityEngine.Random.Range(0, _spawnPositions.Count);
		Vector2 randomPosition = _spawnPositions[randomIndex];
		
		return randomPosition;
	}
}
