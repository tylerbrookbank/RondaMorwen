using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDeadzoneFollow : MonoBehaviour {

	private Transform target;
	private float teatherLengthX;
	private float teatherLengthY;

	private Transform myTransform;

	private Vector2 move;
	private Vector2 targetLocation;
	private Vector2 transformLocation;
	private Camera cam;

	//horizontal 5
	//vertical 2.5

	// Use this for initialization
	void Start () {
        target = GameObject.Find("Player").transform;
		Camera[] cams = Camera.allCameras;
		myTransform = this.transform;
		if (Camera.allCamerasCount == 0) {
			Debug.Log ("No cameras.");
			Application.Quit ();
		} else {
			cam = cams [0];
		}
        teatherLengthX = (transform.localScale.x * transform.parent.localScale.x) / 2;
        teatherLengthY = (transform.localScale.y * transform.parent.localScale.y) / 2;
    }

	void FixedUpdate() {
		Move ();
	}

	// Update is called once per frame
	void Update () {
		GetLocations ();
		VerticalMovement ();
		HorizontalMovement ();
	}

	private void GetLocations() {
		targetLocation.x = target.position.x;
		targetLocation.y = target.position.y;

		transformLocation.x = myTransform.position.x;
		transformLocation.y = myTransform.position.y;
	}

	private void VerticalMovement() {
		float distance;
		distance = targetLocation.y - transformLocation.y;
		move.y = 0;
		if (distance > teatherLengthY) {
			move.y = distance - teatherLengthY;
		} else if (distance < -1 * teatherLengthY) {
			move.y = distance + teatherLengthY;
		}
	}

	private void HorizontalMovement() {
		float distance;
		distance = targetLocation.x - transformLocation.x;
		move.x = 0;
		if (distance > teatherLengthX) {
			move.x = distance - teatherLengthX;
		} else if (distance < -1 * teatherLengthX) {
			move.x = distance + teatherLengthX;
		}
	}

	private void Move() {
		Vector3 movementVector;
		movementVector.x = move.x;
		movementVector.y = move.y;
		movementVector.z = 0;
		cam.transform.position += movementVector;
		//myTransform.position += movementVector;
	}

}
