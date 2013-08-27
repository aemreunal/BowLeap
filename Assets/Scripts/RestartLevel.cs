using UnityEngine;
using System.Collections;

public class RestartLevel : MonoBehaviour {
	public int waitTime = 4;
	
	private bool restartInitiated = false;

	void Update () {
		if(!restartInitiated && transform.position.z > 11) {
			restartInitiated = true;
			StartCoroutine("Restart");
		}
	}
	
	IEnumerator Restart() {
		yield return new WaitForSeconds(waitTime);
		Application.LoadLevel(Application.loadedLevel);
	}
}