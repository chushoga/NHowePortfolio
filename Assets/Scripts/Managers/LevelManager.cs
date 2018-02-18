using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	private float fadeSpeed = 1f; // fading speed
	private Canvas canvas;
	private Transform coverImageGO; // the black overlay image
	private Image coverImage; // the black overlay image

	void Awake(){
		Application.targetFrameRate = 60;
	}

	void Start(){
		canvas = GetComponentInChildren<Canvas>();
		coverImageGO = canvas.transform.Find("Image");
		coverImage = coverImageGO.GetComponentInChildren<Image>();

		// start by enableing the image: the default is hidden so we can see scene in the editor.
		coverImage.enabled = true;

		// start with fading in.
		CrossAlphaWithCallBack(coverImage, 0f, fadeSpeed, delegate {
			coverImage.enabled = false;
		});
	}

	// on scene load option fade out to black.
	public void LoadScene(string sceneName) {

		Debug.Log("Start loading the scene" + sceneName);

		 CrossAlphaWithCallBack(coverImage, 1f, fadeSpeed, delegate {
			SceneManager.LoadScene(sceneName);			
		});

	}

	private void CrossAlphaWithCallBack(Image img, float alpha, float duration, System.Action action){
		StartCoroutine(CrossFadeAlphaCOR(img, alpha, duration, action));
	}

	IEnumerator CrossFadeAlphaCOR(Image img, float alpha, float duration, System.Action action){

		img.enabled = true;

		Color currentColor = img.color;

		Color visibleColor = img.color;
		visibleColor.a = alpha;

		float counter = 0;

		while(counter < duration){
			counter += Time.deltaTime;
			img.color = Color.Lerp(currentColor, visibleColor, counter / duration);
			yield return null;
		}
		action.Invoke();
	}

}