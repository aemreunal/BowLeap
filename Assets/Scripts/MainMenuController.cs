using UnityEngine;
using System.Collections;
using Leap;

public class MainMenuController : MonoBehaviour {	
	public float leapRotateSpeed = 50.0f;
	public int sensorSensitivity = 200; // Highest = 200 <-> 400 = Lowest
	
	public Transform head;
	
    private Controller controller;
	private Frame frame;

    void Start () {
        controller = new Controller();
    }

	void Update () {
		frame = controller.Frame();
		transform.localEulerAngles += new Vector3(-frame.Fingers.Frontmost.Direction.y, frame.Fingers.Frontmost.Direction.x, 0) * leapRotateSpeed * Time.deltaTime;
		
		if(Input.GetKeyDown(KeyCode.Space)) {
			Application.LoadLevel("MainBowling");
		}
	}
}