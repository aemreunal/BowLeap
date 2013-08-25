using UnityEngine;
using System.Collections;

/*
 * score[0] = Frame 1 : 1
 * score[1] = Frame 1 : 2
 * 
 * score[2] = Frame 2 : 1
 * score[3] = Frame 2 : 2
 * 
 * score[4] = Frame 3 : 1
 * score[5] = Frame 3 : 2
 * 
 * score[6] = Frame 4 : 1
 * score[7] = Frame 4 : 2
 * 
 * score[8] = Frame 5 : 1
 * score[9] = Frame 5 : 2
 * 
 * score[10] = Frame 6 : 1
 * score[11] = Frame 6 : 2
 * 
 * score[12] = Frame 7 : 1
 * score[13] = Frame 7 : 2
 * 
 * score[14] = Frame 8 : 1
 * score[15] = Frame 8 : 2
 * 
 * score[16] = Frame 9 : 1
 * score[17] = Frame 9 : 2
 * 
 * score[18] = Frame 10 : 1
 * score[19] = Frame 10 : 2
 * score[20] = Frame 10 : 3
 * 
 */

public class ScoreStorage : MonoBehaviour {
	private static readonly int NUM_ROLLS = 21;
	private static readonly int NUM_PINS = 10;
	
	private int currentRoll;
	private int[] scoreBoard;
	private bool[] previousPinStates;

	void Start () {
		DontDestroyOnLoad(this);
		currentRoll = 0;
		scoreBoard = new int[NUM_ROLLS];
		InitiatePinStates();
	}
	
	void InitiatePinStates() {
		previousPinStates = new bool[NUM_PINS];
		ResetPinStates();
	}
	
	public void AddScoreToBoard(int score, bool[] pinStates) {
		scoreBoard[currentRoll] = score;
		previousPinStates = pinStates;
		currentRoll++;
		CheckPinStates();
		
		if(currentRoll == NUM_ROLLS) {
			Debug.Log ("Total score for 21 rolls: " + GetTotalScore());
			ResetScore();
		}
	}
	
	public void ResetScore() {
		currentRoll = 0;
		for(int i = 0; i < scoreBoard.Length; i++) {
			scoreBoard[i] = 0;	
		}
		Destroy(this);
		Application.LoadLevel("MainMenu");
	}
	
	public void CheckPinStates() {
		if(!(currentRoll > 18) && currentRoll % 2 == 0) {
			ResetPinStates();	
		} else {
			foreach(bool pinIsUp in previousPinStates) {
				if(pinIsUp)	{
					return;	
				}
			}
			ResetPinStates();
		}
	}
	
	private void ResetPinStates() {
		for(int i = 0; i < NUM_PINS; i++) {
			previousPinStates[i] = true;	
		}
	}
	
	public bool[] GetPreviousPinStates() {
		return previousPinStates;	
	}
	
	public int GetTotalScore() {
		int totalScore = 0;
		
		for(int i = 0; i < scoreBoard.Length; i++) {
			totalScore += scoreBoard[i];	
		}
		
		return totalScore;
	}
}
