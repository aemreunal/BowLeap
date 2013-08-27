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

public class ScoreController : MonoBehaviour {
	private static readonly int NUM_ROLLS = 21;
	private static readonly int NUM_PINS = 10;
	private static readonly int FARTHEST_POINT_Z = 9;
	private static readonly double ROTATION_THRESHOLD = 0.2;
	
	public GUIText scoreText;
	
	private int currentRoll;
	private int[] scoreBoard;
	private bool[] currentPinStates;
	private bool[] previousPinStates;
	private Transform[] pins;
	private bool levelBegan;

	void Start () {
		DontDestroyOnLoad(this);
		currentRoll = 0;
		levelBegan = false;
		scoreBoard = new int[NUM_ROLLS];
		ResetAllPins();
	}
	
	private void ResetAllPins() {
		ResetCurrentPins();
		ResetPreviousPins();
	}
	
	private void ResetCurrentPins() {
		currentPinStates = new bool[NUM_PINS];
		for(int i = 0; i < NUM_PINS; i++) {
			currentPinStates[i] = true;	
		}
	}
	
	private void ResetPreviousPins() {
		previousPinStates = new bool[NUM_PINS];
		for(int i = 0; i < NUM_PINS; i++) {
			previousPinStates[i] = true;
		}
	}
	
	public void SetPins(Transform[] pins) {
		this.pins = pins;
	}
	
	public void SetScoreTextObject(GUIText scoreText) {
		this.scoreText = scoreText;
	}
	
	public void beginLevel() {
		levelBegan = true;	
	}
	
	public void endLevel() {
		levelBegan = false;
		AddScoreToBoard();	
	}
	
	public void InitiatePins() {
		for(int i = 0; i < NUM_PINS; i++) {
			if(!previousPinStates[i]) {
				Destroy(pins[i].gameObject);
			}
		}
	}
	
	void Update () {
		if(levelBegan) {
			scoreBoard[currentRoll] = 0;
			for(int i = 0; i < NUM_PINS; i++) {
				if(pins[i] != null) {
					Transform pin = pins[i];
					
					if(pinIsDown(pin.rotation.z, pin.rotation.x, pin.position.z)) {
						scoreBoard[currentRoll]++;
						currentPinStates[i] = false;
					}
				}
			}
			SetScoreText();
		}
	}	

	private bool pinIsDown (double zRotation, double xRotation, double zPosition) {
		return zRotation > ROTATION_THRESHOLD || xRotation > ROTATION_THRESHOLD ||
				zRotation < -ROTATION_THRESHOLD ||
				xRotation < -ROTATION_THRESHOLD ||
				zPosition > FARTHEST_POINT_Z;
	}
	
	public void SetScoreText() {
		scoreText.text = "Frame: ";
		if(currentRoll < 18) {
			scoreText.text += ((currentRoll / 2) + 1) + " - Roll: " + ((currentRoll % 2) + 1);
		} else {
			scoreText.text += "10 - Roll: " + (currentRoll - 17);
		}
		scoreText.text += "\nTotal: " + GetTotalScore() +  " - Current Roll Score: " + scoreBoard[currentRoll];
	}
	
	public void AddScoreToBoard() {
		previousPinStates = currentPinStates;
		currentRoll++;
		PrintPinStates();
		CheckPinStates();
		
		if(currentRoll >= NUM_ROLLS) {
			Debug.Log ("Total score of the game: " + GetTotalScore());
			ResetGame();
		}
	}
	
	public void CheckPinStates() {
		if (currentRoll == 20 && (GetScoreForRoll(currentRoll - 2) + GetScoreForRoll(currentRoll - 1)) < 10) {
			currentRoll = 100;
		} else if (!(currentRoll > 18) && currentRoll % 2 == 0) {
			ResetAllPins();	
		} else if (scoreBoard[currentRoll - 1] == 10) {
			if (currentRoll < 18) {
				currentRoll++;
			}
			Debug.Log("Strike!");
			ResetAllPins();
		}
	}
	
	public void ResetGame() {
		Destroy(this);
		Application.LoadLevel("MainMenu");
	}
	
	public int GetTotalScore() {
		int totalScore = 0;

		for (int i = 0; i < NUM_ROLLS; i++) {
			if (GetScoreForRoll(i) == 10 && (i % 2 == 0) && i < 18) {
				if (GetScoreForRoll(i + 2) != 10) {
					// Strike at first roll of the frame, get the strike points
					// and the two proceeding rolls' points as bonus
					totalScore += GetScoreForRoll(i) + GetScoreForRoll(i + 2) + GetScoreForRoll(i + 3);
				} else {
					// Strike at first & second roll of the frame, get the
					// strike points and the proceeding rolls' points as bonus
					if (i < 16) {
						totalScore += GetScoreForRoll(i) + GetScoreForRoll(i + 2) + GetScoreForRoll(i + 4);
					} else {
						totalScore += GetScoreForRoll(i) + GetScoreForRoll(i + 2) + GetScoreForRoll(i + 3);
					}
				}
				i++;
			} else if (GetScoreForRoll(i) != 10 && (i % 2 == 0) && i < 18) {
				if (GetScoreForRoll(i) + GetScoreForRoll(i + 1) == 10) {
					totalScore += GetScoreForRoll(i) + GetScoreForRoll(i + 1) + GetScoreForRoll(i + 2);
				} else {
					totalScore += GetScoreForRoll(i) + GetScoreForRoll(i + 1);
				}
				i++;
			} else {
				totalScore += GetScoreForRoll(i);
			}
		}
		return totalScore;
	}
	
	public void PrintPinStates() {
		string pins = "Pins: ";
		for(int i = 0; i < NUM_PINS; i++) {
			if(currentPinStates[i]) {
				pins += (i + 1) + " ";	
			}
		}
		Debug.Log(pins + "are up.");
	}
	
	private int GetScoreForRoll(int roll) {
		if(roll < NUM_ROLLS) {
			return scoreBoard[roll];	
		} else {
			return 0;
		}
	}
}
