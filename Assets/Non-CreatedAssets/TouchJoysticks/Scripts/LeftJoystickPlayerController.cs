using UnityEngine;

public class LeftJoystickPlayerController : MonoBehaviour
{
    public LeftJoystick leftJoystick; // the game object containing the LeftJoystick script
	public RightJoystick rightJoystick;
    public Transform rotationTarget; // the game object that will rotate to face the input direction

	public Transform cameraTarget; // the camera that the player uses to look
	private float xCameraRotationClamp = 45.0f; // clamp the rotation of the looking x angle
	private float yCameraRotationClamp = 45.0f; // clamp the rotation of the looking y angle
	private float cameraRotationSpeed = 200.0f;
	private float cameraRotationResetSpeed = 2.0f; // speed of the lerp to return to Vector3.zero

    private float moveSpeed = 1.0f; // movement speed of the player character
	public float moveSpeedMultiplier = 4.0f;
    public float rotationSpeed = 200.0f; // rotation speed of the player character
    public Animator animator; // the animator controller of the player character
    private Vector3 leftJoystickInput; // holds the input of the Left Joystick
	private Vector3 rightJoystickInput; // holds the input of the Right Joystick
    private Rigidbody rigidBody; // rigid body component of the player character

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
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        // get input from both joysticks
		leftJoystickInput = leftJoystick.GetInputDirection();
		rightJoystickInput = rightJoystick.GetInputDirection();

        float xMovementLeftJoystick = leftJoystickInput.x; // The horizontal movement from joystick 01
        float zMovementLeftJoystick = leftJoystickInput.y; // The vertical movement from joystick 01	

		float xMovementRightJoystick = rightJoystickInput.x; // The horizontal movement from joystick 01
		float yMovementRightJoystick = rightJoystickInput.y; // The vertical movement from joystick 01	

        // if there is no input on the left joystick
		if (leftJoystickInput == Vector3.zero || rightJoystickInput == Vector3.zero)
        {
            animator.SetBool("isRunning", false);
        }

  
        // if there is only input from the left joystick
        if (leftJoystickInput != Vector3.zero)
        {
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
            if (lookDirection != Vector3.zero)
            {
            //    rotationTarget.localRotation = Quaternion.Slerp(rotationTarget.localRotation, Quaternion.LookRotation(lookDirection), rotationSpeed * Time.deltaTime);
            }
            if (animator != null)
            {
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

			Debug.Log(xMovementRightJoystick + ", " + yMovementRightJoystick);
			//rightJoystickInput = new Vector3(xMovementRightJoystick, 0, yMovementRightJoystick);
			//rightJoystickInput = transform.TransformDirection(rightJoystickInput);
			//rightJoystickInput *= moveSpeed;

			// rotate the player to face the direction of input
			Vector3 tempRight = cameraTarget.transform.position;
			tempRight.x += xMovementRightJoystick;
			tempRight.y += yMovementRightJoystick;

		
			Vector3 lookDirectionRight = tempRight - cameraTarget.transform.position;

			// START HERE !!!!!!!!!!!!!!!!!! TRY PUTTING THE COMMENTED OUT LINE IN AND SEE IF IT WORKS
			//Debug.Log("CURRENT Y ROTATION: " + cameraTarget.transform.localEulerAngles.y + " | CLAMP: " + yCameraRotationClamp);
			//Debug.Log(temp.x + ", " + temp.y);
			Debug.Log(tempRight.x + ", " + tempRight.y + " => " + lookDirectionRight);

			if(cameraTarget.transform.localRotation.eulerAngles.y >= yCameraRotationClamp) {
				
				//Debug.Log("over Clamp!! STOP" + cameraTarget.transform.localRotation.eulerAngles.y);

			} else {
				
				//Debug.Log("ROTATE!" + cameraTarget.transform.localRotation.eulerAngles.y);

			}


			cameraRotationSpeed = Mathf.Abs(xMovementRightJoystick * 200);


			if(xMovementRightJoystick > 0) {
				//cameraTarget.transform.Rotate(Vector3.zero * cameraRotationSpeed * Time.deltaTime);
// CONTINUE HERE ----------------------------------------------------------------------------------------------------
				// think about putting this in here
				float speed = 3.0f;
				float xRot = speed * xMovementRightJoystick;
				float yRot = speed * yMovementRightJoystick;

				cameraTarget.transform.Rotate(xRot, yRot, 0.0f);
// CONTINUE HERE ----------------------------------------------------------------------------------------------------
				//Debug.Log("RIGHT");
			}

			if(xMovementRightJoystick < 0) {
				cameraTarget.transform.Rotate(-Vector3.up * cameraRotationSpeed * Time.deltaTime);
				//Debug.Log("LEFT");
			}

			if(yMovementRightJoystick > 0) {
				//cameraTarget.transform.Rotate(Vector3.left * cameraRotationSpeed * Time.deltaTime);
				//Debug.Log("RIGHT");
			}

			if(yMovementRightJoystick < 0) {
				//cameraTarget.transform.Rotate(-Vector3.left * cameraRotationSpeed * Time.deltaTime);
				//Debug.Log("LEFT");
			}

			/*
			cameraTarget.transform.eulerAngles = new Vector3(
				cameraTarget.transform.eulerAngles.x - tempRight.y * Time.deltaTime * 100.0f,
				cameraTarget.transform.eulerAngles.y + tempRight.x * Time.deltaTime * 100.0f,
				0f);
			*/
			//cameraTarget.localRotation = Quaternion.Slerp(cameraTarget.localRotation, Quaternion.LookRotation(lookDirectionRight), cameraRotationSpeed * Time.deltaTime);
			//cameraTarget.transform.Rotate(tempRight.x, tempRight.y, 0);


			
		} else {
			// if the joystick is released then snap back to 0,0
			cameraTarget.localRotation = Quaternion.Slerp(cameraTarget.localRotation, Quaternion.LookRotation(Vector3.zero), Time.deltaTime * cameraRotationResetSpeed);
		}

    }
}