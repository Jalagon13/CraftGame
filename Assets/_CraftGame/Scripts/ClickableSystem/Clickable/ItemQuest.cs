using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class ItemQuest : MonoBehaviour
{
    [SerializeField] private PlayerObject po;
    [SerializeField] private ItemObject _item;
    [SerializeField] private int _itemAmount;
    [SerializeField] private TextMeshProUGUI _questText, _errorText;
    [TextArea]
    [SerializeField] private string _questCompleteText;
    [SerializeField] private UnityEvent _onComplete;
    private InventoryController _inventory;

    public bool Completed { get; private set; }


    private void Awake()
    {
        _questText.text = $"I need {_itemAmount} {_item.Name}s";
        _errorText.text = "";
        _inventory = po.PlayerInventory;
    }

    public void TryCompleteQuest()
    {
        if (Completed)
        {
            _errorText.text = "You already completed this.";
        }
        else if (_inventory.FindItem(_item).Quantity >= _itemAmount)
        {
            _errorText.text = "Hooray, thank you!";
            Completed = true;
            // Remove items from inventory
            _inventory.RemoveItem(_item, _itemAmount);
            _questText.text = _questCompleteText;
            _onComplete?.Invoke();
        }
        else
        {
            _errorText.color = Color.red;
            _errorText.text = $"You need more {_item.Name}s";
        }
    }

    public void ResetText()
    {
        if (Completed)
        {
            _errorText.color = Color.green;
            _errorText.text = "You already completed this.";
        }
        else
            _errorText.text = "";
    }
}

