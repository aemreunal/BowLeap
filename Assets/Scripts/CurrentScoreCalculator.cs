using UnityEngine;
using System.Collections;
using System;

public class CurrentScoreCalculator : MonoBehaviour {
	private static readonly int NUM_PINS = 10;
	private static readonly int FARTHEST_POINT_Z = 9;
	private static readonly double ROTATION_THRESHOLD = 0.2;
	
	public GUIText scoreText;
	public Transform pin1;
	public Transform pin2;
	public Transform pin3;
	public Transform pin4;
	public Transform pin5;
	public Transform pin6;
	public Transform pin7;
	public Transform pin8;
	public Transform pin9;
	public Transform pin10;
	
	private ScoreStorage scoreStorage;
	private int currentScore = 0;
	private Transform[] pins;
	private bool[] pinIsUp;
	
	void Start() {
		addPinsToArray();
		getScoreStorage();
		initiatePinStates();
	}

	private void addPinsToArray () {
		pins = new Transform[NUM_PINS];
		pins[0] = pin1;
		pins[1] = pin2;
		pins[2] = pin3;
		pins[3] = pin4;
		pins[4] = pin5;
		pins[5] = pin6;
		pins[6] = pin7;
		pins[7] = pin8;
		pins[8] = pin9;
		pins[9] = pin10;
	}

	private void getScoreStorage() {
		try {
			scoreStorage = (ScoreStorage) GameObject.Find("ScoreStorage").GetComponent("ScoreStorage");
		} catch (Exception e) {
			Debug.Log("Couldn't retrieve the score storage object!");	
			Debug.Log(e.StackTrace);
		}
	}
	
	private void initiatePinStates() {
		pinIsUp = scoreStorage.GetPreviousPinStates();
		for(int i = 0; i < NUM_PINS; i++) {
			if(!pinIsUp[i]) {
				Destroy(pins[i].gameObject);
			}
		}
	}
	
	void Update () {
		currentScore = 0;
		for(int i = 0; i < NUM_PINS; i++) {
			if(pins[i] != null) {
				Transform pin = pins[i];
				
				if(pinIsDown(pin.rotation.z, pin.rotation.x, pin.position.z)) {
					currentScore++;
					pinIsUp[i] = false;
				}
			}
		}
		
		SetScoreText();
	}

	public void SetScoreText() {
		scoreText.text = "Score: " + currentScore;
		if(scoreStorage != null) {
			scoreText.text = "Total: " + scoreStorage.GetTotalScore() +  " - Current " + scoreText.text;
		}
	}

	private bool pinIsDown (double zRotation, double xRotation, double zPosition) {
		return zRotation > ROTATION_THRESHOLD || xRotation > ROTATION_THRESHOLD ||
				zRotation < -ROTATION_THRESHOLD ||
				xRotation < -ROTATION_THRESHOLD ||
				zPosition > FARTHEST_POINT_Z;
	}
	
	void OnDestroy() {
		if(scoreStorage != null) {
			scoreStorage.AddScoreToBoard(currentScore, pinIsUp);
		}
		if(pinIsUp != null) { 
			PrintPinStates ();
		}
	}

	private void PrintPinStates () {
		Debug.Log("Pins:");
		for(int i = 0; i < NUM_PINS; i++) {
			if(pinIsUp[i]) {
				Debug.Log(i + 1);	
			}
		}
		Debug.Log("are up.");
	}
}
