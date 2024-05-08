using System;
using System.Collections;
using System.Collections.Generic;
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
	private ExperienceView _experienceView;
	private ExperienceModel _experienceModel;
	
	private void Awake()
	{
		_experienceModel = new(new List<SkillCategory>(Enum.GetValues(typeof(SkillCategory)) as SkillCategory[]));
		
		GameSignals.CLICKABLE_DESTROYED.AddListener(AddExperience);
	}
	
	private void OnDestroy()
	{
		GameSignals.CLICKABLE_DESTROYED.RemoveListener(AddExperience);
	}
	
	private void Start()
	{
		_experienceView = FindObjectOfType<ExperienceView>();
	}
	
	private void AddExperience(ISignalParameters parameters)
	{
		if(!parameters.HasParameter("SkillCategory"));
		
		SkillCategory skill = (SkillCategory)parameters.GetParameter("SkillCategory");
		
		if(skill != SkillCategory.None)
		{
			IncrementSkillExp(skill);
			NotifyIncrementation(skill);
		}
	}
	
	private void NotifyIncrementation(SkillCategory skill)
	{
		_experienceView.NotifySkillExpGain(skill);
	}
	
	private void IncrementSkillExp(SkillCategory skill)
	{
		// Communicate with model to increment skill
		_experienceModel.IncrementSkill(skill);
	}
}
