using UnityEngine;

public class LeftJoystickPlayerController : MonoBehaviour
{
    public LeftJoystick leftJoystick; // the game object containing the LeftJoystick script
	public RightJoystick rightJoystick;
    public Transform rotationTarget; // the game object that will rotate to face the input direction

	public Transform parentCameraTarget;	
	public Transform cameraTarget; // the camera that the player uses to look
	private float xCameraRotationClamp = 45.0f; // clamp the rotation of the looking x angle
	private float yCameraRotationClamp = 45.0f; // clamp the rotation of the looking y angle
	private float xCameraRotationSpeed = 100.0f;
	private float yCameraRotationSpeed = 100.0f;
	private float cameraRotationResetSpeed = 2.0f; // speed of the lerp to return to Vector3.zero

    private float moveSpeed = 1.0f; // movement speed of the player character
	public float moveSpeedMultiplier = 4.0f;
    public float rotationSpeed = 200.0f; // rotation speed of the player character
    public Animator animator; // the animator controller of the player character
    private Vector3 leftJoystickInput; // holds the input of the Left Joystick
	private Vector3 rightJoystickInput; // holds the input of the Right Joystick
    private Rigidbody rigidBody; // rigid body component of the player character


	float apple;

	Quaternion center;
	float maxAngle = 45.0f;

    void Start()
    {
		
        if (transform.GetComponent<Rigidbody>() == null)
        {
            Debug.LogError("A RigidBody component is required on this game object.");
        }
        else
        {
            rigidBody = transform.GetComponent<Rigidbody>();
        }

        if (leftJoystick == null)
        {
            Debug.LogError("The left joystick is not attached.");
        }

		if (rightJoystick == null)
		{
			Debug.LogError("The left joystick is not attached.");
		}


        if (rotationTarget == null)
        {
            Debug.LogError("The target rotation game object is not attached.");
        }

		this.center = parentCameraTarget.rotation;

    }

    void Update()
    {
    }

    void FixedUpdate()
    {

		float tempClamp = parentCameraTarget.transform.localEulerAngles.x;

		//Debug.Log("CURRENT ANGLE -->" + tempClamp);
		
        // get input from both joysticks
		leftJoystickInput = leftJoystick.GetInputDirection();
		rightJoystickInput = rightJoystick.GetInputDirection();

        float xMovementLeftJoystick = leftJoystickInput.x; // The horizontal movement from joystick 01
        float zMovementLeftJoystick = leftJoystickInput.y; // The vertical movement from joystick 01	

		float xMovementRightJoystick = rightJoystickInput.x; // The horizontal movement from joystick 01
		float yMovementRightJoystick = rightJoystickInput.y; // The vertical movement from joystick 01	

        // if there is no input on the left joystick
		if (leftJoystickInput == Vector3.zero || rightJoystickInput == Vector3.zero){
			
            animator.SetBool("isRunning", false);
			//Debug.Log("test");

        }

  
        // if there is only input from the left joystick
        if (leftJoystickInput != Vector3.zero) {
            // calculate the player's direction based on angle
            float tempAngle = Mathf.Atan2(zMovementLeftJoystick, xMovementLeftJoystick);
            xMovementLeftJoystick *= Mathf.Abs(Mathf.Cos(tempAngle));
            zMovementLeftJoystick *= Mathf.Abs(Mathf.Sin(tempAngle));

			//Debug.Log(xMovementLeftJoystick + ", " + zMovementLeftJoystick);
            leftJoystickInput = new Vector3(xMovementLeftJoystick, 0, zMovementLeftJoystick);
            leftJoystickInput = transform.TransformDirection(leftJoystickInput);
            leftJoystickInput *= moveSpeed;

            // rotate the player to face the direction of input
            Vector3 temp = transform.position;
            temp.x += xMovementLeftJoystick;
            temp.z += zMovementLeftJoystick;
            Vector3 lookDirection = temp - transform.position;
            if (lookDirection != Vector3.zero) {
            //    rotationTarget.localRotation = Quaternion.Slerp(rotationTarget.localRotation, Quaternion.LookRotation(lookDirection), rotationSpeed * Time.deltaTime);
            }
            if (animator != null) {
                animator.SetBool("isRunning", true);
            }

            // move the player
            //rigidBody.transform.Translate(leftJoystickInput * Time.fixedDeltaTime);


			// START HERE -----------------------------------------------------------------------------------------------------------------------------

			moveSpeed = Mathf.Abs(zMovementLeftJoystick * moveSpeedMultiplier);
			rotationSpeed = Mathf.Abs(xMovementLeftJoystick * 200);

			if(zMovementLeftJoystick > 0) {
				//Debug.Log("FOWARD");
				transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
			}

			if(zMovementLeftJoystick < 0) {
				transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
				//Debug.Log("BACKWARD");
			}

			if(xMovementLeftJoystick > 0) {
				transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
				//Debug.Log("RIGHT");
			}

			if(xMovementLeftJoystick < 0) {
				transform.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime);
				//Debug.Log("LEFT");
			}

			//Debug.Log(moveSpeed + " - " + rotationSpeed);

			// START HERE -----------------------------------------------------------------------------------------------------------------------------

        }

		// if there is only input from the right joystick
		if(rightJoystickInput != Vector3.zero) {
			
			// calculate the player's direction based on angle
			float tempAngleRight = Mathf.Atan2(yMovementRightJoystick, xMovementRightJoystick);
			xMovementRightJoystick *= Mathf.Abs(Mathf.Cos(tempAngleRight));
			yMovementRightJoystick *= Mathf.Abs(Mathf.Sin(tempAngleRight));

			//Debug.Log(xMovementRightJoystick + ", " + yMovementRightJoystick);
			rightJoystickInput = new Vector3(xMovementRightJoystick, yMovementRightJoystick, 0);
			rightJoystickInput = parentCameraTarget.transform.TransformDirection(rightJoystickInput);
			rightJoystickInput *= moveSpeed;

			//Debug.Log("rightJoystickInput:" + rightJoystickInput);

			// rotate the player to face the direction of input
			Vector3 tempRight = cameraTarget.transform.position;
			tempRight.x += xMovementRightJoystick;
			tempRight.y += yMovementRightJoystick;
			tempRight.z = 0f;

			//Debug.Log("lookDirection:" + tempRight);

			Vector3 lookDirectionRight = tempRight - cameraTarget.transform.position;

			//Debug.Log("lookDirection:" + lookDirectionRight);

			xCameraRotationSpeed = Mathf.Abs(xMovementRightJoystick * 200);
			yCameraRotationSpeed = Mathf.Abs(yMovementRightJoystick * 200);

			float rotToPos = Mathf.Abs(yMovementRightJoystick * 360f);
			float rotToNeg = 360f - (Mathf.Abs(yMovementRightJoystick * 360f));

			//Debug.Log("POS: " + rotToPos + ", NEG: " + rotToNeg);


			//apple += yMovementRightJoystick * yCameraRotationSpeed * Time.deltaTime;
			//parentCameraTarget.localRotation = Quaternion.Euler(-apple, 0f, 0f);	
			Debug.Log(Input.GetAxis("Mouse Y"));
			float rotationYInput = yMovementRightJoystick * 500f * Time.deltaTime;
			Quaternion yQuaternion = Quaternion.AngleAxis(rotationYInput, -Vector3.right);
			Quaternion temp = parentCameraTarget.localRotation * yQuaternion;
			if(Quaternion.Angle(center, temp)<this.maxAngle) parentCameraTarget.localRotation = temp;


			//parentCameraTarget.localRotation = Quaternion.Slerp(parentCameraTarget.localRotation, Quaternion.LookRotation(new Vector3(0f, 1f, 0f)), Time.deltaTime * cameraRotationResetSpeed);
			//Quaternion rot = parentCameraTarget.transform.localRotation;
			//rot.eulerAngles = new Vector3 (apple, 0f, 0f);
			//parentCameraTarget.transform.localRotation = rot;
			//parentCameraTarget.transform.localEulerAngles = Quaternion.Euler(0f,apple,0f);
			//Debug.Log(yMovementRightJoystick + "--> " + apple);

			/*
			//GLOBAL VAR
			bool rotDirection = true;
			//------------------------

			if(yxMovementRightJoystick > 0) {
				rotDirection = true;	
			}

			if(yMovementRightJoystick < 0) {
				rotDirection = false;
			}

			if(rotDirection == true){

			} else {

			}
				

			*/

			if(xMovementRightJoystick > 0) {
				//Debug.Log("RIGHT → " + cameraTarget.transform);
				//cameraTarget.transform.Rotate(new Vector3(0f, 1f, 0f) * xCameraRotationSpeed * Time.deltaTime);
			}

			if(xMovementRightJoystick < 0) {
				//Debug.Log("← LEFT" + cameraTarget.transform);
				//cameraTarget.transform.Rotate(new Vector3(0f, -1f, 0f) * xCameraRotationSpeed * Time.deltaTime);
			}

			if(yMovementRightJoystick > 0) {
				//Debug.Log(yMovementRightJoystick + "UP ↑ " + tempClamp);

				if(tempClamp > 315f || tempClamp < 45f) {
					//parentCameraTarget.transform.Rotate(new Vector3(-1f, 0f, 0f) * yCameraRotationSpeed * Time.deltaTime);
				} 
			}

			if(yMovementRightJoystick < 0) {
				//Debug.Log("DOWN ↓ " + tempClamp);
				// Check if the joystick is not floored to release the y lock
				if(yMovementRightJoystick > -0.98f){
					if(tempClamp > 315f || tempClamp < 45f) {
						//parentCameraTarget.transform.Rotate(new Vector3(1f, 0f, 0f) * yCameraRotationSpeed * Time.deltaTime);
					}
				} 
			}

			
		} else if(leftJoystickInput != Vector3.zero && rightJoystickInput == Vector3.zero) {
			// if the joystick is released then snap back to 0,0
			// UNCOMMENT TO SNAP THE CAMERA BACK TO STARTING POINT
			parentCameraTarget.localRotation = Quaternion.Slerp(parentCameraTarget.localRotation, Quaternion.LookRotation(Vector3.zero), Time.deltaTime * cameraRotationResetSpeed);
			//cameraTarget.localRotation = Quaternion.Slerp(cameraTarget.localRotation, Quaternion.LookRotation(Vector3.zero), Time.deltaTime * cameraRotationResetSpeed);
		} else {
			
			//parentCameraTarget.localRotation = Quaternion.Slerp(parentCameraTarget.localRotation, Quaternion.LookRotation(Vector3.zero), Time.deltaTime * cameraRotationResetSpeed);
		
		}

    }
}