using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceController : MonoBehaviour
{
	private ExperienceView _experienceView;
	
	private int _currentExperience;
	
	private void Awake()
	{
		GameSignals.CLICKABLE_DESTROYED.AddListener(OnClickableDestroyed);
	}
	
	private void OnDestroy()
	{
		GameSignals.CLICKABLE_DESTROYED.RemoveListener(OnClickableDestroyed);
	}
	
	private void Start()
	{
		// Future note to self: This may cause some issues when creating a scene loading bootstrap
		_experienceView = FindObjectOfType<ExperienceView>();
	}
	
	private void OnClickableDestroyed(ISignalParameters parameters){};
}
