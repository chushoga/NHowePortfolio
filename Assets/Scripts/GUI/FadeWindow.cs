using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeWindow : MonoBehaviour {

	public float fadeSpeed = 1.5f; // fading speed
	//public bool sceneStarting = true; // is the screen still fading?
	private Image coverImage; // the black overlay image
	private Color col = Color.black;

	// Use this for initialization
	void Start () {
		coverImage = GetComponentInChildren<Image>(); // grab the image attached to this canvas
		coverImage.enabled = false; // set it to disabled to start
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void FadeIn(float speed){
		col.a = 1f;
		coverImage.color = col;
		coverImage.CrossFadeAlpha(0f, speed, false);
	}

	public void FadeOut(float speed){
		col.a = 0f;
		coverImage.color = col;
		coverImage.CrossFadeAlpha(1f, speed, false);
	}

}
