using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingControl : MonoBehaviour
{
	[SerializeField] private float _swingCd;
	
	private readonly int _hashRightHit = Animator.StringToHash("[ANM] RightSwing");
	private readonly int _hashUpHit = Animator.StringToHash("[ANM] UpSwing");
	private readonly int _hashLeftHit = Animator.StringToHash("[ANM] LeftSwing");
	private readonly int _hashDownHit = Animator.StringToHash("[ANM] DownSwing");
	private readonly int _hashIdle = Animator.StringToHash("[ANM] Idle");
	private Animator _animator;
	private Camera _camera;
	private Timer _swingTimer;
	private float _defaultAnimSpeed = 1;
	
	private void Awake()
	{
		_animator = GetComponent<Animator>();
		_animator.speed = _defaultAnimSpeed;
		_swingTimer = new(_swingCd);
	}
	
	private IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		_camera = Camera.main;
	}
}
