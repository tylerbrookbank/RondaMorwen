using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyEnemyAttack : CharacterAttack {

    private int attackType = 0;
    private const int punch = 0;
    private const int kick = 1;

    private const float punchMulti = 15f;
    private const float kickMulti = 30f;
    private const float punchForce = 1f;
    private const float kickForce = 10f;

    private float attackTimer;
    private float attackTime;
    private float hitBoxTime;
    private float timeBetweenAttacks;

    private const float firstAttackTime = 1f;
    private const float secondAttackTime = 0.75f;

    //these are used for attack telegraphing
    private SpriteRenderer characterRenderer;
    private const float attackStartTime = 1f;
    private bool attackStart;
    private Color baseColor;

    public EasyEnemyAttack(Animator animator, BoxCollider2D hitBox, SpriteRenderer renderer) {

        attackTime = 0.25f;
        hitBoxTime = 0.5f;
        isAttacking = false;
        attackStart = false;

        this.animator = animator;
        this.hitBox = hitBox;
        this.renderer = renderer;
        hitBox.enabled = false;
        renderer.enabled = false;
        hitBoxTransform = hitBox.gameObject.transform;
        Vector3 position = new Vector3(hitBoxXLocation, 0, 0);
        hitBoxTransform.localPosition = position;

        characterRenderer = (SpriteRenderer)animator.gameObject.GetComponent(typeof(SpriteRenderer));
        if(characterRenderer == null) {
            Debug.Log("EasyEnemyAttack: characterRenderer didnt load.");
            characterRenderer = new SpriteRenderer();
        }
        baseColor = characterRenderer.color;

        attackTimer = secondAttackTime;
        timeBetweenAttacks = secondAttackTime;

    }

    public void Update(bool facingRight) {
        this.facingRight = facingRight;
        attackTimer += Time.deltaTime;
        TelegraphAttack();
        CheckOrination();
        CheckAttacking();

        animator.SetBool("attack", isAttacking);
        animator.SetInteger("attackType", attackType);
    }

    public void CheckAttacking() {
        if(isAttacking && attackTimer >= attackTime) {
            isAttacking = false;
        }
        if(attackTimer >= hitBoxTime) {
            SetHitBox(false);
        }
    }

    private void TelegraphAttack() {

        if(attackStart && attackTimer >= attackStartTime) {
            attackTimer = 0f;
            isAttacking = true;
            attackStart = false;
            SetHitBox(true);
            characterRenderer.color = baseColor;
            attackKey++;
        } else if(attackStart) {
            attackTimer += Time.deltaTime;
        }
    }

    public void Attack() {
        if(!attackStart && !isAttacking
            && (attackTimer >= timeBetweenAttacks)) {
            attackTimer = 0f;
            attackStart = true;
            int randAttack = Random.Range(0,100);
            int randTime = Random.Range(0,100);
            randAttack %= 2;
            randTime %= 2;
            switch (randAttack) {
                case punch:
                    attackType = punch;
                    break;
                case kick:
                    attackType = kick;
                    break;
            }

            Color c = baseColor;
            c.r *= 100;
            characterRenderer.color = c;

        }
    }

    private void SetHitBox(bool onOff) {
        hitBox.enabled = onOff;
        renderer.enabled = onOff;
    }

    public void StopAttack() {
        characterRenderer.color = baseColor;
        attackTimer = 0f;
        isAttacking = false;
        attackStart = false;
        renderer.enabled = false;
        hitBox.enabled = false;
    }

    public int GetAttackType() {
        return attackType;
    }

    public override float GetDamageMultiplier() {
        float returnVal=0f;
        switch (attackType) {
            case punch:
                returnVal = punchMulti;
                break;
            case kick:
                returnVal = kickMulti;
                break;
        }
        return returnVal;
    }

    public override float GetForce() {
        float returnVal = 0f;
        switch (attackType) {
            case punch:
                returnVal = punchForce;
                break;
            case kick:
                returnVal = kickForce;
                break;
        }
        return returnVal;
    }

}
