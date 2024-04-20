using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
	[TextArea]
	[SerializeField] private string[] _text;
	[SerializeField] private TextMeshProUGUI _dialogueText;
	[SerializeField] private GameObject _promptObject, _tutorialObject;
	[SerializeField] private Button _nextButton, _backButton;
	[SerializeField] private UnityEvent _reachedLastDialogue;

	private int _counter = 0;

	private void Awake()
	{ 
		if (_dialogueText != null)
		{
			_dialogueText.text = _text[0];
		}
	}

	private IEnumerator Start()
	{
		_tutorialObject.transform.localScale = Vector3.one;
		yield return new WaitForSeconds(0.5f);
		_tutorialObject.transform.localScale = new(1.15f, 1.15f, 1.15f);
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(Start());
	}

	public void StopTutorialIndicator()
	{
		StopAllCoroutines();
		Destroy(_tutorialObject);
	}

	public void NextText()
	{
		_counter++;
		if (_counter >= _text.Length - 1)
		{
			_nextButton.interactable = false;
			_reachedLastDialogue?.Invoke();
		}
		else if (!_backButton.interactable)
			_backButton.interactable = true;

		_dialogueText.text = _text[_counter];
	}

	public void PreviousText()
	{
		_counter--;
		if (_counter <= 0)
		{
			_backButton.interactable = false;
		}
		else if (!_nextButton.interactable)
			_nextButton.interactable = true;

		if (_counter < 0)
			_counter = 0;
		_dialogueText.text = _text[_counter];
	}

	// called when text box is exited so dialogue can be cycled through again
	public void ResetText()
	{
		_counter = 0;
		_dialogueText.text = _text[0];
		_nextButton.interactable = true;
		_backButton.interactable = false;
	}

    #region Select Methods
    private void OnMouseOver()
    {
		_tutorialObject.SetActive(true);
    }

    private void OnMouseExit()
    {
		_tutorialObject.SetActive(false);
    }

    private void OnMouseDown()
    {
		_promptObject.SetActive(true);
    }
    /*
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out CursorControl cc))
        {
            Selected();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out CursorControl cc))
        {
            UnSelected();
        }
    }*/
    #endregion

}
