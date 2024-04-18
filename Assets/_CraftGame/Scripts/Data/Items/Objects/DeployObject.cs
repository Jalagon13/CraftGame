using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Deployable", menuName = "Create Item/New Deployable")]
public class DeployObject : ItemObject
{
	[SerializeField] private GameObject _prefabToDeploy;
	[SerializeField] private AudioClip _deploySound;
	
	public override void ExecutePrimaryAction(FocusItemController controller)
	{
		
	}

	public override void ExecuteSecondaryAction(FocusItemController controller)
	{
		
	}

	public override string GetDescription()
	{
		return string.Empty;
	}
}