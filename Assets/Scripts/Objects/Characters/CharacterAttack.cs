using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack {
    
    //index of current attack
    //used to pass to enemy hit boxes to ensure
    //each attack only does damage a single time
    protected byte attackKey;
    protected bool facingRight;

    protected Transform hitBoxTransform;
    protected BoxCollider2D hitBox;
    protected SpriteRenderer renderer;
    protected bool isAttacking;

    protected Animator animator;//animator for character animations

    protected float hitBoxXLocation = 0.5f;

    public virtual float GetDamageMultiplier() {
        return 0f;
    }

    public virtual float GetForce() {
        return 0f;
    }

    public virtual byte GetAttackKey() {
        return attackKey;
    }

    public bool GetFacingRight() {
        return facingRight;
    }

    protected virtual void CheckOrination() {
        if (isAttacking) {
            if (facingRight && hitBoxTransform.localPosition.x < 0) {
                Vector3 position = new Vector3(hitBoxXLocation, 0, 0);
                hitBoxTransform.localPosition = position;
            }
            else if (!facingRight && hitBoxTransform.localPosition.x > 0) {
                Vector3 position = new Vector3(hitBoxXLocation * -1, 0, 0);
                hitBoxTransform.localPosition = position;
            }
        }
    }

}
