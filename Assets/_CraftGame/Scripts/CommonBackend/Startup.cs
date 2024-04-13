using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour
{
	private void Awake()
	{
		StartCoroutine(LoadScenes());
	}
	
	private IEnumerator LoadScenes()
	{
		yield return StartCoroutine(LoadSceneAdd("Ingame UI")); 
		yield return StartCoroutine(LoadSceneAdd("Player")); 
		yield return StartCoroutine(LoadSceneAdd("World")); 
		
		Scene gameWorld = SceneManager.GetSceneByName("World"); 
		SceneManager.SetActiveScene(gameWorld); 
		SceneManager.UnloadSceneAsync("Startup");
	}
	
	private IEnumerator LoadSceneAdd(string sceneName)
	{
		AsyncOperation sceneAsync = new();
		sceneAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		sceneAsync.allowSceneActivation = false;

		while (sceneAsync.progress < 0.9f)
		{
			yield return null;
		}

		sceneAsync.allowSceneActivation = true;

		while (!sceneAsync.isDone)
		{
			yield return null;
		}
	}
}
