using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "New Quest")]
public class QuestObject : ScriptableObject
{
	public ItemObject ItemNeeded;
	public int QuantityQuota;
	public int XpReward;
}
