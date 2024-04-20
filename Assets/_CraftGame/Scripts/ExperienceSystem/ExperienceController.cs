using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class ExperienceController : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private MMF_Player _expGainFeedbacks;
	private ExperienceModel _experienceModel = new();
	private ExperienceView _experienceView;
	
	public ExperienceModel ExperienceModel => _experienceModel;
	
	private void Awake()
	{
		_experienceModel = new();
		_experienceModel.ValueIncreased += UpdateView;
		_experienceModel.ValueDecreased += UpdateView;
		_po.PlayerExperience = this;
		
		GameSignals.CLICKABLE_DESTROYED.AddListener(OnClickableDestroyed);
	}
	
	private void OnDestroy()
	{
		_experienceModel.ValueIncreased -= UpdateView;
		_experienceModel.ValueDecreased -= UpdateView;
		
		GameSignals.CLICKABLE_DESTROYED.RemoveListener(OnClickableDestroyed);
	}
	
	private void Start()
	{
		// Future note to self: This may cause some issues when creating a scene loading bootstrap
		_experienceView = FindObjectOfType<ExperienceView>();
		_experienceView.UpdateView(0);
	}
	
	private void UpdateView()
	{
		_experienceView.UpdateView(_experienceModel.CurrentValue);
	}
	
	public void AddExperience(int amount)
	{
		PopupMessage.Create(transform.position, $"+{amount}xp", Color.white, Vector2.up, 1f);
		_expGainFeedbacks?.PlayFeedbacks();
		_experienceModel?.Increment(amount);
	}
	
	public void SubtractExperience(int amount)
	{
		_experienceModel?.Decrement(amount);
	}
	
	private void OnClickableDestroyed(ISignalParameters parameters)
	{
		if(!parameters.HasParameter("Experience")) return;
		
		int xpFromClickable = (int)parameters.GetParameter("Experience");
		AddExperience(xpFromClickable);
	}
}
