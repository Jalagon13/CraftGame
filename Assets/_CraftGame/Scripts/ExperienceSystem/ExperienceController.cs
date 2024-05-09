using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum SkillCategory
{
	None,
	Mining, 
	Woodcutting,
	Gathering,
	Combat,
	Fishing
}

public class ExperienceController : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private int _maxlevel;
	[SerializeField] private AnimationCurve _expPerLevel;
	
	private ExperienceView _experienceView;
	private ExperienceModel _experienceModel;
	public ExperienceModel ExperienceModel => _experienceModel;
	
	private void Awake()
	{
		_po.PlayerExperience = this;
		var categories = new List<SkillCategory>(Enum.GetValues(typeof(SkillCategory)) as SkillCategory[]);
		_experienceModel = new(categories, _maxlevel, _expPerLevel);
		_experienceModel.OnMultiplierUpdated += OnMultiplierUpdated;
		
		GameSignals.CLICKABLE_DESTROYED.AddListener(AddExperience);
	}
	
	private void OnDestroy()
	{
		_experienceModel.OnMultiplierUpdated -= OnMultiplierUpdated;
		GameSignals.CLICKABLE_DESTROYED.RemoveListener(AddExperience);
	}
	
	private void Start()
	{
		_experienceView = FindObjectOfType<ExperienceView>();
	}
	
	private void AddExperience(ISignalParameters parameters)
	{
		if(!parameters.HasParameter("SkillCategory")) return;
		
		SkillCategory skill = (SkillCategory)parameters.GetParameter("SkillCategory");
		
		if(skill != SkillCategory.None)
		{
			IncrementSkillExp(skill);
			NotifyIncrementation(skill);
		}
	}
	
	public void AddToMultiplier(float addedValue)
	{
		_experienceModel.AddToMultiplier(addedValue);
	}
	
	private void OnMultiplierUpdated()
	{
		_experienceView.UpdateMultiplierView(_experienceModel.ExpMultiplier);
	}
	
	private void NotifyIncrementation(SkillCategory skill)
	{
		_experienceView.NotifySkillExpGain(skill);
	}
	
	private void IncrementSkillExp(SkillCategory skill)
	{
		// Communicate with model to increment skill
		_experienceModel.IncrementStoredExp(skill);
	}
}
