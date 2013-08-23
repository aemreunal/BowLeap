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
		//float vertical = Input.GetAxis("Vertical");
		transform.localEulerAngles += new Vector3(/*-vertical*/ 0, horizontal, 0) * keyboardRotateSpeed * Time.deltaTime;
		//transform.Rotate(new Vector3(-vertical, horizontal, 0) * rotateSpeed * Time.deltaTime);
		
		// ---- Leap ----
		thisFrame = controller.Frame();
    	//Debug.Log(frame.Fingers.Frontmost.Direction.AngleTo(Leap.Vector.ZAxis));
		//player.transform.TransformDirection(frame.Fingers.Frontmost.Direction.x, frame.Fingers.Frontmost.Direction.y, frame.Fingers.Frontmost.Direction.z);	
		//Debug.Log(frame.Fingers.Frontmost.TipPosition.x);
		float normalX = thisFrame.Fingers.Frontmost.StabilizedTipPosition.x / sensorSensitivity;
		//float normalZ = frame.Fingers.Frontmost.TipPosition.z / 360;
		//transform.position.Set(frame.Fingers.Frontmost.TipPosition.x, frame.Fingers.Frontmost.TipPosition.y, frame.Fingers.Frontmost.TipPosition.z);
		transform.position = new Vector3(normalX, transform.position.y, transform.position.z);
		
		float fingerDirectionX = thisFrame.Fingers.Frontmost.Direction.x;
		if(/* Sol taraf */ !(fingerDirectionX < 0 && transform.rotation.y > 320) || !(fingerDirectionX > 0 && transform.rotation.y < 40)) {
			//Debug.Log(fingerDirectionX);
			transform.localEulerAngles += new Vector3(/*-vertical*/ 0, fingerDirectionX, 0) * leapRotateSpeed * Time.deltaTime;
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