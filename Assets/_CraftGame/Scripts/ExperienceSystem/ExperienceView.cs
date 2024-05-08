using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceView : MonoBehaviour
{
	[SerializeField] private ExperienceNotifView _notification;
	
	public void Initialize()
	{
		
	}
	
	public void NotifySkillExpGain(SkillCategory skill)
	{
		ExperienceNotifView expNotifView = Instantiate(_notification, transform.position, Quaternion.identity);
		expNotifView.transform.SetParent(transform);
		expNotifView.Initialize(skill);
	}
}
