#pragma strict

var waitTime : int = 3;
var restartInitiated : boolean = false;

function Update () {
	// Next frame/roll
	if(!restartInitiated && transform.position.z > 11) {
		restartInitiated = true;
		NextLevel();
	}
}

function NextLevel() {
	yield WaitForSeconds(waitTime);
	Application.LoadLevel(Application.loadedLevel);
}