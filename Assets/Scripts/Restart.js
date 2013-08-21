#pragma strict

var waitTime : int = 3;
var restartInitiated : boolean = false;

function Update () {
	if(!restartInitiated && transform.position.z > 11) {
		restartInitiated = true;
		RestartLevel();
	}	
}

function RestartLevel() {
	yield WaitForSeconds(waitTime);
	Application.LoadLevel(Application.loadedLevel);
}