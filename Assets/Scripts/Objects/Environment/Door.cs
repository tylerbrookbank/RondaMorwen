using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	public float openSpeed;

	private AudioSource doorSound;
	private CircleCollider2D doorTrigger;
	private Rigidbody2D door;

	private float maxY;
	private float minY;

	private bool openDoor;

	private Vector2 closedPosition;
	private ContactFilter2D contactFilter;

	// Use this for initialization
	void Start () {

		doorTrigger = (CircleCollider2D)GetComponent (typeof(CircleCollider2D));

		foreach (Transform child in transform) {
			if (child.name == "DoorSound")
				doorSound = (AudioSource)child.GetComponent (typeof(AudioSource));
			else if (child.name == "DoorObject")
				door = (Rigidbody2D)child.GetComponent (typeof(Rigidbody2D));
		}
			
		maxY = 3f;
		minY = 0f;

		contactFilter.useTriggers = true;
		contactFilter.SetLayerMask (Physics2D.GetLayerCollisionMask (gameObject.layer)); 
		contactFilter.useLayerMask = true;

		openDoor = false;

		closedPosition = door.position;

	}
	
	// Update is called once per frame
	void Update () {

		if (doorTrigger.IsTouching (contactFilter)) {
			OpenDoor ();
		} else {
			CloseDoor ();
		}

	}

	private void OpenDoor() {
		
		float localY = door.position.y - closedPosition.y;

		if (!openDoor) {
			doorSound.pitch = 0.4f;
			doorSound.Play ();
			openDoor = true;
		}

		if (localY < maxY) {
			door.position += Vector2.up * ((maxY - localY) * openSpeed * Time.deltaTime);
		}

	}

	private void CloseDoor() {

		float localY = door.position.y - closedPosition.y;

		if (openDoor) {
			//doorSound.pitch = 0.3f;
			//doorSound.Play ();
			openDoor = false;
		}

		if (localY > minY) {
			door.position += Vector2.down * ((localY - minY) * openSpeed * Time.deltaTime);
		}

	}

}
