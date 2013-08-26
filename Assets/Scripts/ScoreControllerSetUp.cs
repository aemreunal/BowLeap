using UnityEngine;
using System.Collections;
using System;

public class ScoreControllerSetUp : MonoBehaviour {
	private static readonly int NUM_PINS = 10;
	
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
	
	private ScoreController scoreController;
	private Transform[] pins;
	
	void Start() {
		AddPinsToArray();
		GetScoreController();
		if(scoreController != null) {
			SendObjectsToScoreController();
			scoreController.InitiatePins();
			scoreController.beginLevel();
		}
	}

	private void GetScoreController() {
		try {
			scoreController = (ScoreController) GameObject.Find("ScoreController").GetComponent("ScoreController");
		} catch (Exception e) {
			Debug.Log("Couldn't retrieve the score controller object!");	
			Debug.Log(e.StackTrace);
		}
	}

	private void AddPinsToArray() {
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
	
	public void SendObjectsToScoreController() {
		scoreController.SetPins(pins);
		scoreController.SetScoreTextObject(this.scoreText);
	}
	
	void OnDestroy() {
		if(scoreController != null) {
			scoreController.endLevel();
		}
	}
}
