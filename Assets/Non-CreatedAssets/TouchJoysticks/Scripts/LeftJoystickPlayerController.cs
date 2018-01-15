using UnityEngine;

public class LeftJoystickPlayerController : MonoBehaviour
{
    public LeftJoystick leftJoystick; // the game object containing the LeftJoystick script
    public Transform rotationTarget; // the game object that will rotate to face the input direction
    private float moveSpeed = 1.0f; // movement speed of the player character
	public float moveSpeedMultiplier = 4.0f;
    public float rotationSpeed = 200.0f; // rotation speed of the player character
    public Animator animator; // the animator controller of the player character
    private Vector3 leftJoystickInput; // holds the input of the Left Joystick
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

        float xMovementLeftJoystick = leftJoystickInput.x; // The horizontal movement from joystick 01
        float zMovementLeftJoystick = leftJoystickInput.y; // The vertical movement from joystick 01	

        // if there is no input on the left joystick
        if (leftJoystickInput == Vector3.zero)
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
				Debug.Log("FOWARD");
				transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
			}

			if(zMovementLeftJoystick < 0) {
				transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
				Debug.Log("BACKWARD");
			}

			if(xMovementLeftJoystick > 0) {
				transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
				Debug.Log("RIGHT");
			}

			if(xMovementLeftJoystick < 0) {
				transform.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime);
				Debug.Log("LEFT");
			}
			Debug.Log(moveSpeed + " - " + rotationSpeed);

			// START HERE -----------------------------------------------------------------------------------------------------------------------------

        }
    }
}