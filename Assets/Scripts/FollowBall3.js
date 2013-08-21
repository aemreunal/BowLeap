#pragma strict

/* A simple smooth follow camera,
that follows the “whoToLookAt” target’s forward direction.
From unity script reference (re SmoothDampAngle). */

var whoToLookAt : Transform;
var smoothTime : float = 0.3;
private var yVelocity = 0.0;
var distanceAbove: float = 3.0;
var distanceAway = 5.0;

function LateUpdate( ) {
	if(whoToLookAt.transform.parent == null) {
	    /* Damp angle from current y-angle towards target y-angle */
	    var yAngle : float = Mathf.SmoothDampAngle( 
	                                transform.eulerAngles.y,
	                                whoToLookAt.eulerAngles.y, 
	                                yVelocity, 
	                                smoothTime);
	    /* Position at the target */
	    var newPosition : Vector3 = whoToLookAt.position;
	    /* Then offset by distance behind the new angle */
	    newPosition += Quaternion.Euler(0, yAngle, 0) * Vector3(0, distanceAbove, -distanceAway);
	    /* originally: newPosition += Quaternion.Euler(0, yAngle, 0) * Vector3 (0, 0, -distanceAway); */
	
	    transform.position = newPosition; /* move the camera */
	    transform.LookAt( whoToLookAt );
	}  /* LateUpdate */

}