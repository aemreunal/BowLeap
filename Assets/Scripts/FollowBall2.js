#pragma strict

/* Simple camera 2 that stays near the “whoToLookAt” target. */

var whoToLookAt : Transform; 
var cameraStopTrigger : Transform;

var smoothTime : float = 0.3;
var distanceAbove: float = 2.0;
var distanceAway = 3.0; 
var cameraStopDistance : double = 7.5;
 
private var yVelocity = 0.0;
private var following : boolean = true;

function LateUpdate( ) {
	if(following && whoToLookAt.transform.parent == null) {
	    /* transform the camera so it is a distance away from target */
	   	transform.position = whoToLookAt.position + Vector3(0, distanceAbove, -distanceAway);
	
	    /* Look at the target */
	    transform.LookAt( whoToLookAt );
    }
    
    if(whoToLookAt.position.z > cameraStopDistance) {
  		following = false;
    }
}  /* LateUpdate */