using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoneTransfer : MonoBehaviour {

	public Transform despawnPoint;
	public Transform spawnPoint;
	public Transform transitionEndPoint;
	public BoxCollider2D spawnTargetTransitionBox;

	private PlayerController controller;
	private BoxCollider2D boxCollider;
	private Vector2 myPosition;
	private Vector2 playerPosition;
	private Vector2 despawnPointLocation;
	private Vector2 spawnPointLocation;
	private Vector2 transitionEndPointLocal;
	private ContactFilter2D contactFilter;
	private const float moveSpeed = 8f;
	private bool waiting;
	private float currentTime;
	private float waitTime = 1.5f;

	//used for percentage calculations for screen cover during transitions
	private float totalDistance;
	private Image screenCover;

	private bool inTransition;
	private bool spawned;

	// Use this for initialization
	void Start () {
		GameObject obj = GameObject.Find ("ScreenCover");
		boxCollider = (BoxCollider2D)GetComponent(typeof(BoxCollider2D));
		controller = (PlayerController)GameObject.Find ("Player").GetComponent (typeof(PlayerController));
		screenCover = (Image)obj.GetComponent (typeof(Image));
		myPosition.x = transform.position.x;
		myPosition.y = transform.position.y;
		despawnPointLocation.x = despawnPoint.position.x;
		despawnPointLocation.y = despawnPoint.position.y;
		transitionEndPointLocal.x = transitionEndPoint.position.x;
		transitionEndPointLocal.y = transitionEndPoint.position.y;
		contactFilter.useTriggers = true;
		contactFilter.SetLayerMask (Physics2D.GetLayerCollisionMask(gameObject.layer));
		contactFilter.useLayerMask = true;
		inTransition = false;
		spawned = false;
		waiting = false;
		currentTime = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		GetPositionOfPlayer ();
		if (inTransition) {
			if (!spawned) {
				MovePlayer (despawnPointLocation, moveSpeed);
			} else if (waiting) {
				Wait ();
			} else {
				MovePlayer (transitionEndPointLocal, moveSpeed);
			}
		} else {
			CheckTrigger ();
		}
	}

	//simulate loading
	//might change in future for actual loading
	private void Wait() {
		controller.MovePlayer (false, 0);
		currentTime += Time.deltaTime;
		if (currentTime > waitTime) {
			waiting = false;
			currentTime = 0f;
			CalculateTotalDistance (transitionEndPointLocal);
		}
	}

	private void CalculateTotalDistance(Vector2 target) {
		totalDistance = Mathf.Abs (target.x - playerPosition.x);
	}
		
	private void HandleScreenCover(Vector2 target) {
		float percentage;
		float buffer = 0.2f;
		if (spawned) {
			percentage = (Mathf.Abs (target.x - playerPosition.x) / totalDistance);
		} else {
			percentage = 1 - (Mathf.Abs (target.x - playerPosition.x) / totalDistance);
		}
		percentage += buffer;
		screenCover.color = new Color (screenCover.color.r,
			screenCover.color.g,
			screenCover.color.b,
			percentage);
	}

	private void MovePlayer(Vector2 target, float Speed) {
		float buffer = 0.2f;//used to buffer direction
		float direction = target.x - playerPosition.x + buffer;
		bool right = direction > 0 ? true : false;
		if (right) {
			if (playerPosition.x < target.x) {
				controller.MovePlayer (right, moveSpeed);
				HandleScreenCover (target);
			} else {
				if (spawned) {
					controller.GiveControls ();
					spawned = false;
					inTransition = false;
					spawnTargetTransitionBox.enabled = true;
					TurnOffScreenCover ();
				} else {
					SpawnPlayer ();
				}
			}
		} else {
			if (playerPosition.x > target.x) {
				controller.MovePlayer (right, moveSpeed);
				HandleScreenCover (target);
			} else {
				if (spawned) {
					controller.GiveControls ();
					spawned = false;
					inTransition = false;
					spawnTargetTransitionBox.enabled = true;
					TurnOffScreenCover ();
				} else {
					SpawnPlayer ();
				}
			}
		}
	}

	private void TurnOffScreenCover() {
		screenCover.color = new Color (screenCover.color.r,
			screenCover.color.g,
			screenCover.color.b,
			0);
	}

	private void SpawnPlayer() {
		controller.SetPostition (spawnPoint.position.x, spawnPoint.position.y);
		spawned = true;
		waiting = true;
	}

	private void CheckTrigger() {
		if (boxCollider.IsTouching (contactFilter)) {
			controller.TakeControls ();
			inTransition = true;
			spawned = false;
			spawnTargetTransitionBox.enabled = false;
			CalculateTotalDistance (despawnPointLocation);
		}
	}

	private void GetPositionOfPlayer() {
		playerPosition.x = controller.transform.position.x;
		playerPosition.y = controller.transform.position.y;
	}

}
