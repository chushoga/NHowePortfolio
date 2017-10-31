using UnityEngine;
using System.Collections;

// Does not need a GUI text element.
// Attach to an empty GameObject
// Will show milliseconds
public class FPSCounter : MonoBehaviour
{
	float deltaTime = 0.0f;

	void Update(){

		// update time
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

	}

	void OnGUI(){

		// set width and height to the current screen width and height
		int w = Screen.width;
		int	h = Screen.height;

		// create a new GUIStyle
		GUIStyle style = new GUIStyle();

		// create new rect for the FPS counter
		Rect rect = new Rect(0, 0, w, h * 2 / 100);

		// set the position and style for the FPS
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);

		// set milliseconds
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;

		// set the format for the string
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);

		// print the lable.
		GUI.Label(rect, text, style);
	}
}