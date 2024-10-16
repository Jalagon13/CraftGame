using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// I need Craft Amount, Craft Timer Remaining Seconds, Craft Timer Remaining Seconds fraction for Fill Image
public class ProgressView : MonoBehaviour
{
	[SerializeField] private Image _fillImage;
	[SerializeField] private Image _outputImage;
	[SerializeField] private TextMeshProUGUI _craftAmountText;
	[SerializeField] private TextMeshProUGUI _secondsLeftText;
	
	private CraftingModel _craftingModel;
	
	private void Start()
	{
		LoopThroughChildElements(false);
	}
	
	private void Update()
	{
		if(_craftingModel == null || _craftingModel.CraftingTimer == null) return;
		
		if(_craftingModel.CraftingTimer.RemainingSeconds > 0)
		{
			// Update Fill Image Percentage
			TimerFillImage();
		}
	}
	
	public void TimerFillImage()
	{
		float remainingSeconds = _craftingModel.CraftingTimer.RemainingSeconds;
		float craftingTimer = _craftingModel.CurrentCraftingRecipe.CraftingTimer;
		
		_fillImage.fillAmount = 1 - Mathf.Clamp01(Mathf.InverseLerp(0, craftingTimer, remainingSeconds));
		
		float f = Mathf.Round(remainingSeconds * 10.0f) * 0.1f;
		_secondsLeftText.text = $"{f}s";
	}
	
	public void Initialize(CraftingModel craftingModel)
	{
		_craftingModel = craftingModel;
	}
	
	public void StartProcessViewUI(CraftingModel craftingModel)
	{
		_craftingModel = craftingModel;
		_craftingModel.OnCraftingDone += StopProcessViewUI;
		_craftingModel.OnIterationDone += UpdateProgressView;
		
		UpdateProgressView();
		LoopThroughChildElements(true);
	}
	
	public void UpdateProgressView()
	{
		if(_craftingModel == null || _craftingModel.CurrentCraftingRecipe == null) return;
		
		_craftAmountText.text = $"x{_craftingModel.CraftCounter}";
		_outputImage.sprite = _craftingModel.CurrentCraftingRecipe.OutputItem.UiDisplay;
	}
	
	public void StopProcessViewUI()
	{
		_craftingModel.OnCraftingDone -= StopProcessViewUI;
		_craftingModel.OnIterationDone -= UpdateProgressView;
		
		LoopThroughChildElements(false);
	}
	
	public void EnableProgressViewUI(bool enabled)
	{
		if(enabled)
			UpdateProgressView();
			
		LoopThroughChildElements(enabled);
	}
	
	private void LoopThroughChildElements(bool enabled)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform childTransform = transform.GetChild(i);
			childTransform.gameObject.SetActive(enabled);
		}
	}
}
