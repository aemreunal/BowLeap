using UnityEngine;
using System.Collections;
using System;

public class Score : MonoBehaviour {
	private static readonly int NUM_PINS = 10;
	private static readonly int FARTHEST_POINT_Z = 9;
	private static readonly double ROTATION_THRESHOLD = 0.2;
	
	public GUIText scoreText;
	public Transform bowlingPinsObject;
	
	//private bool previousScoreExists = false;
	private ScoreStorage scoreStorage;
	private int currentScore = 0;
	//private int totalScore = 0;
	
	void Start() {
		/*
		if(PlayerPrefs.HasKey("Frame1_1")) {
			previousScoreExists = true;
		}
		*/
		try {
			scoreStorage = (ScoreStorage) GameObject.Find("ScoreStorage").GetComponent("ScoreStorage");
		} catch (Exception e) {
			Debug.Log("Couldn't retrieve the score storage object!");	
		}
	}
	
	// Update is called once per frame
	void Update () {
		currentScore = 0;
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
		
		/*
		if(previousScoreExists) {
			scoreText.text = "Previous: " + PlayerPrefs.GetInt("Frame1_1") +  " - Current Score: " + currentScore;
		} else {
			scoreText.text = "Score: " + currentScore;
		}
		*/
		scoreText.text = "Score: " + currentScore;
		if(scoreStorage != null) {
			scoreText.text = "Total: " + scoreStorage.GetTotalScore() +  " - Current " + scoreText.text;
		}
	}
	
	void OnDestroy() {
		/*
		if(previousScoreExists) {
			PlayerPrefs.DeleteKey("Frame1_1");
		} else {
			PlayerPrefs.SetInt("Frame1_1", currentScore);
		}
		PlayerPrefs.Save();
		*/
		if(scoreStorage != null) {
			scoreStorage.AddScoreToBoard(currentScore);
		}
	}
}
