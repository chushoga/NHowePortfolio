using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPreviewManager : MonoBehaviour {

	// OTHER
	public float rotSpeed = 0.0f;	// rotation speed for the character
	public float baseAngle = 0.0f; // base angle for drag rotation

	// ANIMATIONS
	public List<GameObject> gm; // drop all the modles/with their animations in the list
	private Animator anim; // this is for accessing the Animator component of the current model

	// UI
	public GameObject actionButtonPrefab; // the action button prefab
	public GameObject actorButtonPrefab; // the actor button prefab
	public GameObject GUI3DButtonCamera; // camera for 3d preveiw button
	private GameObject actionPanel; // content panel of the scroll view
	private GameObject modelPanel; // modle panels with scroll view

	void Start(){
		
		// Find the action and model panel in the GUI
		modelPanel = GameObject.Find("ModelPanelContent");
		actionPanel = GameObject.Find("ActionPanelContent");

		// Instantiate buttons for the model selection buttons
		if (modelPanel != null) {
			PopulateModelPreview();
		}

		// Check if the action panel actually exists
		if(actionPanel != null){
			LoadModel(1);
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

	public void LoadModel(int actor){

		// first clear the main spawn point before adding.
		foreach(Transform child in transform){
			Destroy(child.gameObject);
		}
		// first clear any previous actions in the action panel
		foreach(Transform child in actionPanel.transform) {
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
			a.transform.SetParent(actionPanel.transform, false);

			// Get and set the button text
			Text txt = a.GetComponentInChildren<Text>();
			txt.text = anim.runtimeAnimatorController.animationClips[i].name;

			// Add an onClick listener to the button to play the animationName.
			a.GetComponent<Button>().onClick.AddListener(() => PlayAnimationButton(animationName));

			i++;
		}

		// NOTE: Dissabled for testing click drag rotation.
		//StartRotating(); // start rotating the model
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
			actorButton.transform.SetParent(modelPanel.transform, false); // Set the parent for the button
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

	// Rotate the model on mouse down

	void OnMouseDown(){
		Debug.Log("MOUSE DOWN");
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		pos = Input.mousePosition - pos;
		baseAngle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
		baseAngle -= Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;
	}


	void OnMouseDrag(){
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		pos = Input.mousePosition - pos;
		float ang = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg - baseAngle;
		transform.rotation = Quaternion.AngleAxis(ang, Vector3.up);

		float rotationSpeed = 0.5f;
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = Camera.main.transform.position.y - transform.position.y;
		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
		float angle = -Mathf.Atan2(transform.position.z - mouseWorldPos.z, transform.position.x - mouseWorldPos.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, angle, 0), rotationSpeed * Time.deltaTime);
	}
}
