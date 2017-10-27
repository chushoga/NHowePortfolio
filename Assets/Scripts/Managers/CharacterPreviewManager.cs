using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPreviewManager : MonoBehaviour {
	// rotation speed for the character
	public float rotSpeed = 0.0f;

	// ANIMATIONS
	public List<GameObject> gm; // drop all the modles/with their animations in the list
	private Animator anim; // this is for accessing the Animator component of the current model

	// UI
	public GameObject actionButtonPrefab; // the action button prefab
	private GameObject actionPanel; // content panel of the scroll view

	void Start(){
		// Instantiate the first model in the list as default and set as "model" game object
		GameObject model = Instantiate(gm[0], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

		// Set the models postion to this transform.
		//---------------------------------------------------
		// TODO: see if this is really necessary!!!!
		//---------------------------------------------------
		model.transform.SetParent(gameObject.transform, false);
		//---------------------------------------------------

		// Find the action panel
		actionPanel = GameObject.Find("ActionPanelContent");

		// Check if the action panel actually exists
		if(actionPanel != null){

			// get the animator component of the model and set it to a variable
			anim = model.GetComponent<Animator>();

			//Debug.Log(anim.runtimeAnimatorController.animationClips.Length);

			// loop through the clips found in the animator.
			//---------------------------------------------------
			// TODO: Consider changing this to a for loop
			// try the following once the code works ok. put the 
			// foreach conents into the for loop.
			//---------------------------------------------------
			/*
			for(int i = 0; i < anim.runtimeAnimatorController.animationClips.Length; i++){
				Debug.Log(anim.runtimeAnimatorController.animationClips[i].name);
			}
*/
			//---------------------------------------------------
			int i = 0;
			foreach(AnimationClip ac in anim.runtimeAnimatorController.animationClips){

				// Name of animation clip
				string animationName = anim.runtimeAnimatorController.animationClips[i].name;

				// Instantiate the action button prefab.
				GameObject a = (GameObject)Instantiate(actionButtonPrefab);

				// Set the parent for the button
				a.transform.SetParent(actionPanel.transform, false);

				// Get and set the button text
				Text txt = a.GetComponentInChildren<Text>();
				txt.text = anim.runtimeAnimatorController.animationClips[i].name;

				// Add an onClick listener to the button to play the animationName.
				a.GetComponent<Button>().onClick.AddListener(() => PlayAnimationButton(animationName));

				i++;
			}

		} else {
			Debug.Log("There is no actionPanel in the GUI for the actions.");
		}


	}

	// Update is called once per frame
	void Update(){
		transform.Rotate(Vector3.up * Time.deltaTime * rotSpeed);
	}

	// Play the animation
	void PlayAnimationButton(string animName){
		anim.Play(animName);
	}

	// Start he model rotation
	public void StartRotating(){
		rotSpeed = 20.0f;
	}

	// Stop the model rotation
	public void StopRotating(){
		rotSpeed = 0;
	}

}
