using UnityEngine;
using System.Collections;

public class FollowBall : MonoBehaviour {
	public Transform whoToLookAt;
	
	private float distanceAbove = 2.0f;
	private float distanceAway = 2.5f; 
	private double cameraStopDistance = 7.5;
 
	private bool following = true;

	void LateUpdate () {
		if(following && whoToLookAt.transform.parent == null) {
		    /* transform the camera so it is a distance away from target */
		   	transform.position = whoToLookAt.position + new Vector3(0, distanceAbove, -distanceAway);
		
		    /* Look at the target */
		    transform.LookAt(whoToLookAt);
	    }
	    
	    if(whoToLookAt.position.z > cameraStopDistance) {
	  		following = false;
	    }
	}
}