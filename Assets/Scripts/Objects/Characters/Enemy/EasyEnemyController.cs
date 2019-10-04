using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyEnemyController : Character {
    
    private BoxCollider2D hitBox;
    private Collider2D aggroTrigger;

    private GameState game;
    private Transform target;

    private bool inAttackRange;
    private EasyEnemyAttack eAttack;

    private bool spawnedBits;

    protected override void ObjectSetup() {

        base.ObjectSetup();

        spawnedBits = false;

        game = (GameState)GameObject.Find("GameState").GetComponent(typeof(GameState));

        hitBox = null;
        aggroTrigger = null;
        foreach(Transform child in transform) {
            if(child.name.Equals("HitBox")) {
                hitBox = (BoxCollider2D)child.GetComponent(typeof(BoxCollider2D));
            } else if(child.name.Equals("AggroTrigger")) {
                aggroTrigger = (CircleCollider2D)child.GetComponent(typeof(CircleCollider2D));
            }
        }

        target = GameObject.Find("Player").transform;

        facingRight = true;
        stats = new Stats();

        if(animator == null) {
            Debug.Log("EasyEnemy: animator not created. Destroying.");
            Destroy(gameObject);
        }

        if(hitBox == null) {
            Debug.Log("EasyEnemy: hitBox not created. Destroying.");
            Destroy(gameObject);
        }

        if (aggroTrigger == null) {
            Debug.Log("EasyEnemy: AggroTrigger not created. Destroying.");
            Destroy(gameObject);
        }

        if(game == null) {
            Debug.Log("EasyEnemy: Game State not loaded. Destorying.");
            Destroy(gameObject);
        }

        inAttackRange = false;
        BoxCollider2D attackBoxCollider2D = null;
        SpriteRenderer attackHitBoxRenderer = null;
        foreach (Transform child in transform) {
            if (child.gameObject.name.Equals("AttackHitBox")) {
                attackHitBoxRenderer = (SpriteRenderer)child.gameObject.GetComponent(typeof(SpriteRenderer));
                attackBoxCollider2D = (BoxCollider2D)child.gameObject.GetComponent(typeof(BoxCollider2D));
                break;
            }
        }
        eAttack = new EasyEnemyAttack(animator, attackBoxCollider2D, attackHitBoxRenderer);
        stats.AddBits(100);
    }

    private void OnDestroy() {
        if (stats.GetDead()) {
            PlayerController player = (PlayerController)GameObject.Find("Player").GetComponent(typeof(PlayerController));
            Stats pStats = player.GetStats();
            pStats.AddBits(stats.GetBits());
        }
    }

    private void ExplodeBits() {
        GameObject obj = (GameObject)Instantiate(Resources.Load("BitExplosion"), transform.position, transform.rotation);
        //obj.transform.position = transform.position;
    }

    protected override void UpdateAnimator() {
        animator.SetBool("grounded", grounded);
        animator.SetBool("dead", stats.GetDead());
        animator.SetBool("stun", stats.GetStun());
    }

    protected override void GetMovement() {

        if (game.IsPlaying()) {
            if (stats.GetDead()) {
                Dead();
            } else {
                if (!stats.GetStun()) {
                    Move();
                    Attack();
                    CheckDirection();
                } else {
                    velocity.x = 0f;
                    eAttack.StopAttack();
                }
            }
            eAttack.Update(facingRight);
        }

    }

    private void Attack() {
        if(inAttackRange) {
            eAttack.Attack();
        }
    }

    private void Move() {
        if(aggroTrigger.IsTouching(contFilter)) {
            float attackRange = 1.5f;
            float speed = 4f;
            Vector3 targetPosition = target.position;
            float distanceFromTarget = targetPosition.x - transform.position.x;
            if(Mathf.Abs(distanceFromTarget) > attackRange) {
                inAttackRange = false;
                velocity.x = speed;
                if(distanceFromTarget < 0) {
                    velocity.x *= -1;
                }
            } else {
                inAttackRange = true;
                velocity.x = 0f;
            }
        }
    }

    private void Dead() {
        gravityMultiplier = 0.1f;
        velocity.x = 0f;
        hitBox.enabled = false;
        aggroTrigger.enabled = false;
        bc2d.enabled = false;
        DeathAnim();
        if (!spawnedBits) {
            ExplodeBits();
            spawnedBits = true;
        }
    }

    private void DeathAnim() {
        Quaternion q = transform.localRotation;
        float rotationSpeed = 1f;
        //facingRight = false;
        float dir = facingRight ? 1 : -1;
        q.z += rotationSpeed * Time.deltaTime * dir;
        transform.localRotation = q;
    }

    public EasyEnemyAttack GetEnemyAttack() {
        return eAttack;
    }

}
