using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

public class ExperienceController : SerializedMonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private MMF_Player _expGainFeedbacks;
	[SerializeField] private Dictionary<int, int> _xpRp = new();
	
	private ExperienceModel _experienceModel = new();
	private ExperienceView _experienceView;
	private int _expandIndex = 0;
	private int _currentRp;
	
	public ExperienceModel ExperienceModel => _experienceModel;
	
	private void Awake()
	{
		_experienceModel = new();
		_experienceModel.ValueIncreased += UpdateView;
		_experienceModel.ValueDecreased += UpdateView;
		_po.PlayerExperience = this;
		
		GameSignals.CLICKABLE_DESTROYED.AddListener(OnClickableDestroyed);
		GameSignals.ON_EXPAND.AddListener(OnExpand);
		GameSignals.ON_QUEST_COMPLETE.AddListener(OnQuestComplete);
	}
	
	private void OnDestroy()
	{
		_experienceModel.ValueIncreased -= UpdateView;
		_experienceModel.ValueDecreased -= UpdateView;
		
		GameSignals.CLICKABLE_DESTROYED.RemoveListener(OnClickableDestroyed);
		GameSignals.ON_EXPAND.RemoveListener(OnExpand);
		GameSignals.ON_QUEST_COMPLETE.RemoveListener(OnQuestComplete);
	}
	
	private void Start()
	{
		// Future note to self: This may cause some issues when creating a scene loading bootstrap
		_experienceView = FindObjectOfType<ExperienceView>();
		_experienceView.UpdateView(_experienceModel.CurrentValue, _xpRp.ElementAt(_expandIndex).Key);
		_experienceView.UpdateFriendship(_currentRp, _xpRp.ElementAt(_expandIndex).Value);
	}
	
	private void UpdateView()
	{
		_experienceView.UpdateView(_experienceModel.CurrentValue, _xpRp.ElementAt(_expandIndex).Key);
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
	
	private void OnQuestComplete(ISignalParameters parameters)
	{
		_currentRp++;
		_experienceView.UpdateFriendship(_currentRp, _xpRp.ElementAt(_expandIndex).Value);
	}
	
	private void OnExpand(ISignalParameters parameters)
	{
		SubtractExperience(_xpRp.ElementAt(_expandIndex).Key);
		_expandIndex++;
		_currentRp = 0;
		Debug.Log(_expandIndex);
		Debug.Log(_xpRp.ElementAt(_expandIndex).Key);
		_experienceView.UpdateView(_experienceModel.CurrentValue, _xpRp.ElementAt(_expandIndex).Key);
		_experienceView.UpdateFriendship(0, _xpRp.ElementAt(_expandIndex).Value);
		
		// Make add fancy game feel here!
	}
	
	private void OnClickableDestroyed(ISignalParameters parameters)
	{
		if(!parameters.HasParameter("Experience")) return;
		
		int xpFromClickable = (int)parameters.GetParameter("Experience");
		AddExperience(xpFromClickable);
	}
}
