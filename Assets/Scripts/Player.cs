using UnityEngine;
using System.Collections;
using Leap;

public class Player : MonoBehaviour {	
	public float keyboardRotateSpeed = 20.0f;
	public float leapRotateSpeed = 50.0f;
	public float ballForce = 12.0f;
	public int sensorSensitivity = 200; // Highest = 200 <-> 400 = Lowest
	public int fingerTipVelocity = 500000; // Highest = 200 <-> 400 = Lowest
	
	public Transform head;
	public Transform ball;
	public Transform pointer;
	public bool useKeyboard = false;
	public bool usePunchingMotion = false;
	
	private bool ballThrown = false;
    private Controller controller;
	private Frame thisFrame;

    void Start () {
		ballThrown = false;
        controller = new Controller();
    }

	void Update () {
		// ---- Keyboard ----
		float horizontal = Input.GetAxis("Horizontal");
		transform.localEulerAngles += new Vector3(0, horizontal, 0) * keyboardRotateSpeed * Time.deltaTime;
		
		if(controller != null) {
			// ---- Leap ----
			thisFrame = controller.Frame();
			float normalX = thisFrame.Fingers.Frontmost.StabilizedTipPosition.x / sensorSensitivity;
			transform.position = new Vector3(normalX, transform.position.y, transform.position.z);
			float fingerDirectionX = thisFrame.Fingers.Frontmost.Direction.x;
			
			if(/* Sol taraf */ !(fingerDirectionX < 0 && transform.rotation.y > 320) || !(fingerDirectionX > 0 && transform.rotation.y < 40)) {
				transform.localEulerAngles += new Vector3(0, fingerDirectionX, 0) * leapRotateSpeed * Time.deltaTime;
			}
		}
			
		if(!ballThrown && ((Input.GetKeyDown(KeyCode.Space)) || (usePunchingMotion && thisFrame.Hands.Count == 2))) {
			ballThrown = true;
			ball.parent.DetachChildren();
			ball.rigidbody.useGravity = true;
			ball.rigidbody.isKinematic = false;
			pointer.transform.renderer.enabled = false;
			ball.rigidbody.AddRelativeForce(0, 0, ballForce, ForceMode.VelocityChange);
		}
	}
}