using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Research Recipe", menuName = "Create Recipe/New Research Recipe")]
public class ResearchRecipeObject : ScriptableObject
{
	public int XpQuota;
	public List<ResearchRecipeObject> RecipeObjectsUnlockedOnComplete;
}
