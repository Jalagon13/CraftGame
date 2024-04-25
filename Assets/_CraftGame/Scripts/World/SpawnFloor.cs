using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnFloor : MonoBehaviour
{
	[SerializeField] private TilemapObject _spawnFloorTm;
	[SerializeField] private float _respawnTimer;
	[SerializeField] private Clickable _lootBox;
	[SerializeField] private LootTable _lootBoxContent;
	[Space()]
	[SerializeField] private List<RscSpawnSetting> _rscSpawnSettings;

	private List<Vector2> _spawnPositions = new();
	private Stack<string> _clearedClickables = new();
	
	[Serializable]
	public class RscSpawnSetting
	{
		public Clickable ClickableToSpawn;
		public int SpawnAmount;
	}
	
	private void OnEnable()
	{
		_spawnFloorTm.DynamicTilemap.Tilemap = GetComponent<Tilemap>();
		
		// Gather all spawn tile positions
		BoundsInt bounds = _spawnFloorTm.Tilemap.cellBounds;
		
		foreach (var position in bounds.allPositionsWithin)
		{
			if(_spawnFloorTm.Tilemap.HasTile(position))
			{
				_spawnPositions.Add(new Vector2(position.x, position.y));
			}
		}
		
		RefreshResources();
		StartCoroutine(ResourceSpawner());
		SpawnLootBox();
		
		GameSignals.CLICKABLE_DESTROYED.AddListener(RegisterClearedClickable);
	}
	
	private void OnDisable()
	{
		StopAllCoroutines();
		
		GameSignals.CLICKABLE_DESTROYED.RemoveListener(RegisterClearedClickable);
	}
	
	private void SpawnLootBox()
	{
		tryAgain:
		var randPos = GetRandomPosition();
		Vector3Int pos = new Vector3Int((int)randPos.x, (int)randPos.y, 0);
		
		if(IsValidSpawnPosition(pos))
		{
			var clickable = SpawnClickable(_lootBox, pos);
			clickable.OverrideLootTable(_lootBoxContent);
		}
		else
		{
			goto tryAgain;
		}
	}
	
	private void RegisterClearedClickable(ISignalParameters parameters)
	{
		Clickable clickable = (Clickable)parameters.GetParameter("Clickable");
		_clearedClickables.Push(clickable.Name);
	}
	
	private IEnumerator ResourceSpawner()
	{
		yield return new WaitForSeconds(_respawnTimer);
		
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
				tryAgain:
				var randPos = GetRandomPosition();
				Vector3Int pos = new Vector3Int((int)randPos.x, (int)randPos.y, 0);
				
				if(IsValidSpawnPosition(pos))
				{
					SpawnClickable(entry.ClickableToSpawn, pos);
				}
				else
				{
					goto tryAgain;
				}
			}
		}
	}
	
	private Clickable SpawnClickable(Clickable clickable, Vector3Int spawnPos)
	{
		var r = Instantiate(clickable, spawnPos, Quaternion.identity);
		
		r.transform.SetParent(transform);
			
		return r;
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
			if(col.CompareTag("Clickable") || col.TryGetComponent(out IInteractable interactable)) 
			{
				return false;
			}
		}

		return true;
	}
	
	private Vector2 GetRandomPosition()
	{
		int randomIndex = UnityEngine.Random.Range(0, _spawnPositions.Count);
		Vector2 randomPosition = _spawnPositions[randomIndex];
		
		return randomPosition;
	}
}
