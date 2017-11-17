using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPreviewManager : MonoBehaviour {

	// OTHER
	public float rotationSpeed = 300f;	// rotation speed for the character

	private Vector3 prevPos; // for rotating character on mouse drag
	private Vector3 newPos; // for rotating character on mouse drag

	// ANIMATIONS
	public List<GameObject> gm; // drop all the modles/with their animations in the list
	private Animator anim; // this is for accessing the Animator component of the current model

	// UI
	public GameObject actionButtonPrefab; // the action button prefab
	public GameObject actorButtonPrefab; // the actor button prefab
	public GameObject GUI3DButtonCamera; // camera for 3d preveiw button

	private GameObject actionPanelContent; // content panel of the scroll view
	private GameObject modelPanelContent; // content of modle panel with scroll view

	private GameObject infoPanel; // information about the model
	private GameObject infoPanelContent; // info panel content with scroll view

	void Start(){
		
		// Find the action and model panel in the GUI
		modelPanelContent = GameObject.Find("ModelPanelContent");
		actionPanelContent = GameObject.Find("ActionPanelContent");
		infoPanelContent = GameObject.Find("InfoPanelContent");
		infoPanel = GameObject.Find("InfoPanel");

		// Instantiate buttons for the model selection buttons
		if (modelPanelContent != null) {
			PopulateModelPreview();
		}

		// Check if the action and panels actually exists
		if(actionPanelContent != null && infoPanelContent != null){
			LoadModel(1);
		}

		if(infoPanel == null) {
			Debug.Log("no info panel detected");
		}

	}

	// Update is called once per frame
	void Update(){
		
	}

	// Play the animation
	void PlayAnimationButton(string animName){
		anim.Play(animName);
	}

	public void LoadModel(int actor){

		// first clear the main spawn point before adding.
		foreach(Transform child in transform){
			Destroy(child.gameObject);
		}
		// first clear any previous actions in the action panel
		foreach(Transform child in actionPanelContent.transform) {
			Destroy(child.gameObject);
		}

		// Instantiate the first model in the list as default and set as "model" game object
		GameObject model = Instantiate(gm[actor], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

		// Set the models postion to this transform.
		model.transform.SetParent(gameObject.transform, false);

		// get the animator component of the model and set it to a variable
		anim = model.GetComponent<Animator>();

		int i = 0;
		foreach(AnimationClip ac in anim.runtimeAnimatorController.animationClips){

			// Name of animation clip
			string animationName = anim.runtimeAnimatorController.animationClips[i].name;

			// Instantiate the action button prefab.
			GameObject a = (GameObject)Instantiate(actionButtonPrefab);

			// Set the parent for the button
			a.transform.SetParent(actionPanelContent.transform, false);

			// Get and set the button text
			Text txt = a.GetComponentInChildren<Text>();
			txt.text = anim.runtimeAnimatorController.animationClips[i].name;

			// Add an onClick listener to the button to play the animationName.
			a.GetComponent<Button>().onClick.AddListener(() => PlayAnimationButton(animationName));

			i++;
		}

		// Display the details info for the model
		// add the model script if it does not exist before getting dtails or it will throw error.
		if(model.GetComponentInChildren<ModelDetails>() == null){
			model.AddComponent<ModelDetails>();
		}
		Text infoContent = infoPanelContent.GetComponentInChildren<Text>();
		infoContent.text = ""; // first clear anything that might still be there
		infoContent.text = model.GetComponent<ModelDetails>().details; // add the info there

		// Reset the scrollbar position
		ScrollRect sr = infoPanel.GetComponentInChildren<ScrollRect>();
		sr.verticalNormalizedPosition = 1f;

	}

	// Populate Models Menu
	public void PopulateModelPreview(){
		for (int i = 0; i < gm.Count; i++) {

			// --------------------------------------------------------------------------
			// RENDER TEXTURE
			// --------------------------------------------------------------------------

			RenderTexture rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
			rt.Create();
			rt.name = "RenderTexture" + i;

			// --------------------------------------------------------------------------
			// CREATE SPAWN FOR MODEL PREVIEW MODELS/CAMERAS
			// --------------------------------------------------------------------------

			float spawnOffset = -i + (-i + -5f); // spawn offset down and extra spacing

			Vector3 spawnPos = new Vector3(0f,spawnOffset,0f); // set the new postion
			Vector3 cameraSpawnPos = new Vector3(2.06f,spawnOffset + 0.5f, -3f); // camera spawn postion

			GameObject spawnPoint = new GameObject(); // create new empty GameObject
			spawnPoint.transform.position = spawnPos; // set its spawn position incremented
			spawnPoint.transform.localRotation *= Quaternion.Euler(0,180,0); // flip the spawn 180
			spawnPoint.name = gm[i].name + "Anchor"; // give the spawn point a new name

			// --------------------------------------------------------------------------
			// INSTANTIATE Model/Camera
			// --------------------------------------------------------------------------
			GameObject go = (GameObject)Instantiate(gm[i]); // instantiate model
			go.transform.SetParent(spawnPoint.transform, false); // set parent to spawnPoint anchor

			GameObject buttonCamera = (GameObject)Instantiate(GUI3DButtonCamera); // instantiate button camera
			buttonCamera.transform.SetParent(spawnPoint.transform, false);
			buttonCamera.transform.position = cameraSpawnPos; // set position to  camera spawn position.
			buttonCamera.transform.localRotation *= Quaternion.Euler(0f, -215f, 0f); // set the rotation of the camera at an angle.
			Camera guiCam = buttonCamera.GetComponent<Camera>();

			guiCam.targetTexture = rt; // set the camera texture to the render texture we created before

			// Instantiate the action button prefab.
			GameObject actorButton = (GameObject)Instantiate(actorButtonPrefab);
			actorButton.transform.SetParent(modelPanelContent.transform, false); // Set the parent for the button
			Text txt = actorButton.GetComponentInChildren<Text>(); // get the button text component
			txt.text = gm[i].name.Replace ("Demo", ""); // set the text to the model name and Remove the affex "Demo"

			// set the raw image to the render texture created before.
			RawImage rawImg = actorButton.GetComponentInChildren<RawImage>();
			rawImg.texture = rt;

			// TODO: add the on click event to switch between models.
			// Move the start function items to a new function that can be called
			// when the user clicks the button. Call it LoadModel(clicked id?);
			// pass in what array position of the modle that was clicked.

			int tempInt = i; // the click listener needs this temp in to work correclty. Just adding i to the load model does not work.
			actorButton.GetComponent<Button>().onClick.AddListener(() => LoadModel(tempInt));
		}
	}


	// Set inital mouse touch positions
	void OnMouseDown(){

		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		pos = Input.mousePosition;

		prevPos = pos;
		newPos = pos;

	}

	// Rotate the model on mouse drag
	void OnMouseDrag(){
		
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		pos = Input.mousePosition;
		newPos = pos;

		if(prevPos.x > newPos.x) {
			transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
		} else if(prevPos.x < newPos.x) {
			transform.Rotate(-Vector3.up * Time.deltaTime * rotationSpeed);
		}
		prevPos = newPos;
	}
}
