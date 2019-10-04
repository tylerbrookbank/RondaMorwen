using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : CharacterAttack{

    private bool debugging = false;

    private float deltaAttack;//time since last attack
    private float attackTime;//time it takes for attack
    private float attackChainTime;//max time between last attack and new attack for attack chain
    private float hitBoxTime;//max time hitbox can stay on

    //Attack Types
    private int attackType;
    private const int noAttack = 0;
    private const int punch = 1;
    private const int heavyPunch = 2;
    private const int kick = 3;

    //damage multipliers and force constants
    private const float punchForce = 3f;
    private const float heavyPunchForce = 5f;
    private const float kickForce = 15f;
    private const float punchMulti = 15f;
    private const float heavyPunchMulti = 20f;
    private const float kickMulti = 30f;

    //constructor that takes in an animator
    //the animator, box collider and sprite renderer are nessessary for object so 
    //no default constructor
	public PlayerAttack(Animator animator, BoxCollider2D hitBox, SpriteRenderer renderer) {

        attackTime = 0.25f;
        hitBoxTime = 0.5f;
        attackChainTime = 1.5f;
        //set deltaTime to attack time so an attack can be performed right away
        deltaAttack = attackTime;
        isAttacking = false;
        attackType = noAttack;
        attackKey = 0;

        this.animator = animator;
        this.hitBox = hitBox;
        this.renderer = renderer;
        hitBox.enabled = false;
        renderer.enabled = false;
        hitBoxTransform = hitBox.gameObject.transform;
        Vector3 position = new Vector3(hitBoxXLocation, 0 , 0);
        hitBoxTransform.localPosition = position;

        debugging = true;
    }

    //function that updates object state, should get called every frame
    //passes in facinigRight boolean for orintation of hitBox
    public void Update(bool facingRight) {
        deltaAttack += Time.deltaTime;
        this.facingRight = facingRight;
        CheckAttacking();
        CheckOrination();
        animator.SetBool("attacking", isAttacking);
        animator.SetInteger("attackType", attackType);
    }

    //trys to process an attack if not currently attacking
    //parameters are level of character and attack stat of character
    public bool Attack(int level, int attack) {
        bool returnVal = false;
        if(!isAttacking) {
            attackKey++;//increment the attack key
            deltaAttack = 0f;
            isAttacking = true;
            attackType = deltaAttack >= attackChainTime ? punch : attackType + 1;
            attackType = attackType > 3 ? punch : attackType;
            SetHitBox(true);
            returnVal = true;
        }
        return returnVal;
    }

    public override byte GetAttackKey() {
        return attackKey;
    }

    public override float GetForce() {
        float force = 1f;
        switch(attackType) {
            case punch:
                force = punchForce;
                break;
            case heavyPunch:
                force = heavyPunchForce;
                break;
            case kick:
                force = kickForce;
                break;
        }
        return force;
    }

    public override float GetDamageMultiplier() {
        float dmgMult = 1f;
        switch(attackType) {
            case punch:
                dmgMult = punchMulti;
                break;
            case heavyPunch:
                dmgMult = heavyPunchMulti;
                break;
            case kick:
                dmgMult = kickMulti;
                break;
        }
        return dmgMult;
    }

    private void SetHitBox(bool onOff) {
        hitBox.enabled = onOff;
        if (debugging) {//turn on renderer if debugging
            renderer.enabled = onOff;
        }
    }

    //checks to see if the state of object is currently
    //attacking or not
    private void CheckAttacking() {
        //if attacking and attack timer has run out
        //stop attacking
        if(isAttacking && deltaAttack >= attackTime) {
            isAttacking = false;
        }
        if(deltaAttack >= hitBoxTime) {
            SetHitBox(false);
        }
        if(deltaAttack > attackChainTime) {
            attackType = noAttack;
        }
    }

}
