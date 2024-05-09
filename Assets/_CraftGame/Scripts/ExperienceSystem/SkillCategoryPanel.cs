using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCategoryPanel : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _skillText;
	[SerializeField] private TextMeshProUGUI _levelText;
	[SerializeField] private Image _fillImage;
	private LevelSystem _levelSystem;
	private Category _category;
	
	public void Initialize(Category category)
	{
		string skillName = category.Skill.ToString();
		_category = category;
		_levelSystem = category.LevelSystem;
		_skillText.text = $"{skillName}";
		_levelText.text = $"Lv.{category.LevelSystem.CurrentLevel}";
		
		StartCoroutine(ExpSequence());
	}
	
	private IEnumerator ExpSequence()
	{
		UpdateTime();
		
		int storedExp = _category.StoredExp;
		for (int i = 0; i < storedExp; i++)
		{
			_levelSystem.GainExperience(1);
			Debug.Log(_category.Skill + " " + _levelSystem.CurrentExp);
			UpdateTime();
			yield return new WaitForSeconds(0.2f);
		}
		_category.StoredExp = 0;
	}
	
	public void UpdateTime()
	{
		int currentLevel = _levelSystem.CurrentLevel;
		int currentExp = _levelSystem.CurrentExp;
		
		int currentLevelAmount = _levelSystem.ExpPerLevel[currentLevel];
		int nextLevelAmount = _levelSystem.ExpPerLevel[currentLevel + 1];
		int expRelativeToNextLvl = currentExp - currentLevelAmount;
		Debug.Log("currentExp " + currentExp);
		Debug.Log("next level " + nextLevelAmount);
		_fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, nextLevelAmount, currentExp - currentLevelAmount));
	}
}
