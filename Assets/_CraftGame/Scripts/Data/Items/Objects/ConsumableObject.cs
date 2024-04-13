using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Create Item/New Consumable")]
public class ConsumableObject : ItemObject
{
	[SerializeField] private int _energyValue;
	[SerializeField] private AudioClip _consumeSound;

	public override void ExecutePrimaryAction(FocusItemController controller)
	{
		
	}

	public override void ExecuteSecondaryAction(FocusItemController controller)
	{
		MMSoundManagerSoundPlayEvent.Trigger(_consumeSound, MMSoundManager.MMSoundManagerTracks.Sfx, default);
		
		Signal signal = GameSignals.ON_CONSUME;
		signal.ClearParameters();
		signal.AddParameter("ConsumeItem", this);
		signal.AddParameter("EnergyValue", _energyValue);
		signal.Dispatch();
	}

	public override string GetDescription()
	{
		return string.Empty;
	}
}
