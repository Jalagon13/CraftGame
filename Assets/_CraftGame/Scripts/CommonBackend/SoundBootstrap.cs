using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class SoundBootstrap : MonoBehaviour
{
	[SerializeField] private AudioClip _bgMusic;
	
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(1f);
		
		MMSoundManagerSoundPlayEvent.Trigger(_bgMusic, MMSoundManager.MMSoundManagerTracks.Music, default, loop: true, volume: 0.5f);
	}
}
