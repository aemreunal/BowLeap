using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
	private static readonly int NUM_PINS = 10;
	private static readonly int FARTHEST_POINT_Z = 9;
	private static readonly double ROTATION_THRESHOLD = 0.2;
	
	public GUIText scoreText;
	public Transform bowlingPinsObject;
	
	// Update is called once per frame
	void Update () {
		int currentScore = 0;
		for(int i = 0; i < NUM_PINS; i++) {
			Transform child = bowlingPinsObject.GetChild(i);
			double zRotation = child.rotation.z;
			double xRotation = child.rotation.x;
			
			if(zRotation > ROTATION_THRESHOLD ||
				xRotation > ROTATION_THRESHOLD ||
				zRotation < -ROTATION_THRESHOLD ||
				xRotation < -ROTATION_THRESHOLD ||
				child.position.z > FARTHEST_POINT_Z) {
				currentScore++;	
			}
		}
		scoreText.text = "Score: " + currentScore;
	}	
	
}
