using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftNodeView : MonoBehaviour
{
	[SerializeField] private Image _outputImage;
	private ItemObject _outputItem;
	private int _index;
	
	public void Initialize(ItemObject outputItem, int index)
	{
		_outputItem = outputItem;
		_outputImage.sprite = _outputItem.UiDisplay;
		_index = index;
	}
	
	public void SelectThisCraftNode()
	{
		Signal signal = GameSignals.ON_CRAFT_NODE_CLICKED;
		signal.ClearParameters();
		signal.AddParameter("RecipeIndex", _index);
		signal.Dispatch();
	}
}
