using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceModel
{
	private List<Category> _skillCategories = new();
	
	public class Category
	{
		public SkillCategory Skill;
		private int _currentAmount;
		
		public Category(SkillCategory skill)
		{
			Skill = skill;
			_currentAmount = 0;
		}
		
		public void Increment()
		{
			_currentAmount++;
		}
	}
	
	public ExperienceModel(List<SkillCategory> categories)
	{
		foreach(SkillCategory skill in categories)
		{
			// Create an internal category
			Category skillCategory = new(skill);
			_skillCategories.Add(skillCategory);
		}
	}
	
	public void IncrementSkill(SkillCategory skill)
	{
		Category category = GetSkillCategory(skill);
		
		if(category != null)
		{
			category.Increment();
		}
		else
		{
			Debug.LogError($"Skill: {skill} could not be found");
		}
	}
	
	private Category GetSkillCategory(SkillCategory skill)
	{
		foreach (Category item in _skillCategories)
		{
			if(item.Skill == skill)
			{
				return item;
			}
		}
		
		return null;
	}
}
