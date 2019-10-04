using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {

	public float gravityMultiplierNormal = 2f;
	public float gravityMultiplier = 2f;

	protected Rigidbody2D rb2d;
    protected BoxCollider2D bc2d;
	protected Vector2 velocity;
	protected ContactFilter2D contactFilter;
	protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

	protected float minMovement = 0.001f;
	protected float shellRadius = 0.01f;
	protected bool grounded;//am i touching ground
	 
    void Awake() {
        rb2d = (Rigidbody2D)GetComponent(typeof(Rigidbody2D));
        bc2d = (BoxCollider2D)GetComponent(typeof(BoxCollider2D));
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
        ObjectSetup();
    }

	// Use this for initialization
	void Start () {
		
	}

	void FixedUpdate() {

		Vector2 move;
		Vector2 deltaPosition;

		velocity += Physics2D.gravity * Time.deltaTime * gravityMultiplier;
		deltaPosition = velocity * Time.deltaTime;

		move = Vector2.right * deltaPosition.x;
		Movement (move, false);

		grounded = false;
		move = Vector2.up * deltaPosition.y;
		Movement (move, true);

	}

	// Update is called once per frame
	void Update () {
		GetMovement ();
		UpdateAnimator ();
	}

	protected void Movement(Vector2 move, bool YMove) {

		float distance = move.magnitude;

		if (distance > minMovement) {
			int count = bc2d.Cast (move, contactFilter, hitBuffer, distance + shellRadius);
            
			for(int i=0; i < count; i++) {

				Vector2 currentNormal = hitBuffer [i].normal;
				float projection;
				float modifiedDistance;

				if (currentNormal.y == 1) {
					grounded = true;
					if (YMove) {
						currentNormal.x = 0;
					}
				} 

				projection = Vector2.Dot (velocity, currentNormal);
				if (projection < 0) {
					velocity -= projection * currentNormal;
				}

				modifiedDistance = hitBuffer [i].distance - shellRadius;
				distance = modifiedDistance < distance ? modifiedDistance : distance;
			}

		}

		rb2d.position += move.normalized * distance;

	}

    public void Stop() {
        velocity.x = 0;
    }

	//Use this for setups so children classes can rewrite if needed
	protected virtual void ObjectSetup() {
		
	}

	//used for updating animator for gravity and other flags related to movement
	protected virtual void UpdateAnimator() {

	}

	protected virtual void GetMovement() {

	}

}
