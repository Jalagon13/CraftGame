using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Reference", menuName = "New Reference/Player Reference")]
public class PlayerObject : ScriptableObject
{
	private Vector2 _playerPosition;
	private Vector2 _mousePosition;
	private InventoryController _playerInventory;
	private ExperienceController _experienceController;
	private bool _someUiActive;
	
	public Vector2 Position { get { return _playerPosition; } set { _playerPosition = value; } }
	public Vector2 MousePosition { get { return _mousePosition; } set { _mousePosition = value; } }
	public InventoryController PlayerInventory { get { return _playerInventory; } set { _playerInventory = value; } }
	public ExperienceController PlayerExperience { get { return _experienceController; } set { _experienceController = value; } }
	public bool SomeUiActive { get { return _someUiActive;} set { _someUiActive = value;}}
}
