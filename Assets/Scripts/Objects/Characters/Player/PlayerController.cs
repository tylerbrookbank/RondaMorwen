using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character {

	public float moveSpeed;
	public float maxSpeed;
	public float jumpVelocity;
	public float airModifer;
	public float wallSlideSpeed;
	public float wallJumpDistance;
	public float wallJumpVelocityX;
	public float wallJumpVelocityY;
	public float dashDistance;
	public float dashVelocity;

	private bool wallJump;
	private bool jump;
	private bool releaseJump;
	private bool jumpSlowed;//used for jump releasing
	private bool wallSlide;
	private bool dashTrigger;
	private bool dashSet;
	private float xMovement;
	private int airDashCount;
	private bool GetControls;//check to see if currently can control
	private bool touchingWall;

    //Attack varibles
    private bool attackPress;
    private PlayerAttack pAttack;

	//variable used to save a velocity for a set distance
	private float targetVelocity;
	private float targetDistance;
	private float currentDistance;

	//used for buffering movement to make it harder to move off the wall
	private float wallMovementBuffer;
	private float wallMovementBufferMax;
	private EdgeCollider2D wallCollider;

    //used to make sure player is not in interactable zone
    //2097152 is the int layer mask for layer 21
    private const int interactLayer = 2097152;

	private int state;
	private const int onGround = 0;
	private const int inAir = 1;
	private const int inAirJumped = 2;//state in which no longer can jump
	private const int onWall = 3;
	private const int dash = 4;

    private PlayerInventory inventory;

    private bool debug = true;

	protected override void ObjectSetup() {
        base.ObjectSetup();

		wallCollider = (EdgeCollider2D)GetComponentInChildren (typeof(EdgeCollider2D));
		state = onGround;
		facingRight = true;
		wallMovementBufferMax = moveSpeed * 0.25f * Time.deltaTime;
		currentDistance = 0f;
		targetVelocity = 0f;
		targetDistance = 0f;
		dashTrigger = false;
		GetControls = true;

        BoxCollider2D attackBoxCollider2D = null;
        SpriteRenderer attackHitBoxRenderer = null;
        foreach (Transform child in transform) {
            if(child.gameObject.name.Equals("AttackHitBox")) {
                attackHitBoxRenderer = (SpriteRenderer)child.gameObject.GetComponent(typeof(SpriteRenderer));
                attackBoxCollider2D = (BoxCollider2D)child.gameObject.GetComponent(typeof(BoxCollider2D));
                break;
            }
        }
        pAttack = new PlayerAttack(animator, attackBoxCollider2D, attackHitBoxRenderer);

        stats = new Stats();
        stats.SetName("Luthien");
        inventory = new PlayerInventory();
	}

	protected override void UpdateAnimator() {
		animator.SetBool ("grounded", grounded);
		animator.SetBool ("touchingWall", touchingWall);
		animator.SetBool ("wallSlide",wallSlide);
		animator.SetFloat ("velocity", Mathf.Abs(velocity.x));
		animator.SetBool ("wallJump", wallJump);
		animator.SetBool ("dash", dashTrigger);
        animator.SetBool("dead", stats.GetDead());

        animator.SetBool("hurt", stats.GetDamaged());
        stats.SetDamaged(false);

		if (jump && grounded) {
			animator.SetTrigger ("jump");
		} else if (!grounded) {
			animator.ResetTrigger ("jump");
		}
	}

	private void GetInput() {
		jump = false;
		releaseJump = false;
		wallJump = false;
        attackPress = Input.GetButtonDown("Attack");
		maxSpeed = moveSpeed;
		xMovement = Input.GetAxis ("Horizontal");

		if (Input.GetButtonDown ("Jump")) {
			jump = true;
		} else if(Input.GetButtonUp("Jump")) {
			releaseJump = true;
		}
		if (Input.GetButtonDown ("Dash") && !dashTrigger && airDashCount == 0 && inventory.GetDash()) {
			dashTrigger = true;
			dashSet = false;
			if (!grounded) {
				airDashCount++;
			}
		}
	}

	private void GetState() {
		wallSlide = false;
		gravityMultiplier = gravityMultiplierNormal;
		CheckIfTouchingWall ();
		if (dashTrigger) {
			state = dash;
		} else if (grounded) {
			state = onGround;
		} else if(touchingWall) {
			state = onWall;
		} else if (state != inAirJumped) {
			state = inAir;
		}
    }

	private void GetTargetVelocity() {
		if (targetVelocity != 0f) {
			if (Mathf.Abs(currentDistance) < Mathf.Abs(targetDistance)) {
				currentDistance += targetVelocity * Time.deltaTime;
				velocity.x += targetVelocity;
			} else {
				currentDistance = 0f;
				targetDistance = 0f;
				targetVelocity = 0f;
			}
		}
	}

	private void ThrottleVelocity() {
		if (Mathf.Abs (velocity.x) > maxSpeed) {
			velocity.x = velocity.x > 0f ? maxSpeed : -1 * maxSpeed;
		}
	}

	private void CheckIfTouchingWall() {
        touchingWall = false;
		if (wallCollider.IsTouching (contFilter)) {
			touchingWall = true;
		}
	}

	protected override void GetMovement() {
		if (GetControls) {
            CheckWallCollider();
			GetInput ();
			GetState ();

			switch (state) {
			case onGround:
				OnGround ();
				break;
			case inAir:
				InAir ();
				break;
			case inAirJumped:
				InAirJumped ();
				break;
			case onWall:
				OnWall ();
				break;
			case dash:
				Dash ();
				break;
			}

            if(attackPress &&  (state == onGround
                                || state == inAir
                                || state == inAirJumped)) {
                pAttack.Attack(stats.GetLevel(), stats.GetAttack());
            }

            GetTargetVelocity ();
			ThrottleVelocity ();
			CheckDirection ();
            pAttack.Update(facingRight);
            if (debug && Input.GetButtonDown("DebugDeath")) {
                stats.DamageHealth(stats.GetHealth());
                velocity.x = 0f;
            }
        }
	}

	private void OnGround() {
		airDashCount = 0;
        if(bc2d.IsTouchingLayers(interactLayer)) {
            jump = false;
        }
        if (jump) {
			velocity.y += jumpVelocity;
			jumpSlowed = false;
		}
		velocity.x = xMovement * moveSpeed;
	}

	private void InAir() {
		if (!jumpSlowed && velocity.y < 0) {
			jumpSlowed = true;
		}

		if (jump && inventory.GetDoubleJump()) {
			velocity.y = jumpVelocity * airModifer;
			state = inAirJumped;
		} else if (releaseJump && !jumpSlowed) {
			velocity.y *= 0.5f;
			jumpSlowed = true;
		}

		velocity.x = xMovement * moveSpeed;
	}

	private void InAirJumped() {
		velocity.x = xMovement * moveSpeed;
	}

	private void OnWall() {
		bool slideBool = true;
		gravityMultiplier = wallSlideSpeed;
		airDashCount = 0;
		if (wallMovementBuffer != 0f && xMovement == 0f) {
			wallMovementBuffer = 0f;
		}
		if(velocity.y > 0) {
			velocity.y *= 0.9f;
		}
		wallMovementBuffer += xMovement * Time.deltaTime;
		if (facingRight) {
			if (jump) {
				velocity.y = wallJumpVelocityY;
				targetDistance = -1 * wallJumpDistance;
				targetVelocity = -1 * wallJumpVelocityX;
				slideBool = false;
				wallJump = true;
				state = inAir;
			} else if (wallMovementBuffer < -1 * wallMovementBufferMax) {
				velocity.x += xMovement;
				state = inAir;
				slideBool = false;
			}
		} else {
			if (wallMovementBuffer > wallMovementBufferMax) {
				velocity.x += xMovement;
				state = inAir;
				slideBool = false;
			}
			if (jump) {
				velocity.y = wallJumpVelocityY;
				targetDistance = wallJumpDistance;
				targetVelocity = wallJumpVelocityX;
				slideBool = false;
				wallJump = true;
				state = inAir;
			}
		}
		wallSlide = slideBool;
	}

	private void Dash() {
		maxSpeed = dashVelocity;
		if (!dashSet) {
			currentDistance = 0f;
			targetDistance = dashDistance;
			targetVelocity = dashVelocity;
			if (!facingRight) {
				targetDistance *= -1;
				targetVelocity *= -1;
			}
			dashSet = true;
			velocity.y = 0;
		} else if (targetDistance == 0f) {//done dash
			dashTrigger = false;
		} else {
			velocity.y = 0;
		}
	}

    private void CheckWallCollider() {
        wallCollider.enabled = inventory.GetWallSlide();
    }

    public PlayerAttack GetPlayerAttack() {
        return pAttack;
    }

	//Moves player in direction
	//only works with controls are taken from player
	public void MovePlayer(bool rightMovement, float speed) {
		if (!GetControls) {
			if (rightMovement) {
				velocity.x = speed;
			} else {
				velocity.x = speed * -1;
			}
		}
	}

    public bool IsInControl() {
        return GetControls;
    }

	public void SetPostition(float x, float y) {
		if (!GetControls) {
			rb2d.position = new Vector2 (x, y);
		}
	}

    public void TakeControls() {
        GetControls = false;
    }

	//Give controls to player
	public void GiveControls() {
		GetControls = true;
	}

    public PlayerInventory GetInventory() {
        return inventory;
    }

    public bool LevelUp(string stat) {
        bool returnVal = stats.IncreaseStat(stat);
        return returnVal;
    }

}
