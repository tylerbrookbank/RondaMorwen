using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : PhysicsObject {

    protected Stats stats;

    protected ContactFilter2D contFilter;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;

    protected bool facingRight;

    protected override void ObjectSetup() {
        animator = (Animator)GetComponent(typeof(Animator));
        spriteRenderer = (SpriteRenderer)GetComponent(typeof(SpriteRenderer));
        contFilter.useTriggers = true;
        contFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contFilter.useLayerMask = true;
    }

    public Stats GetStats() {
        return stats;
    }

    public bool IsFacingRight() {
        return facingRight;
    }

    protected void CheckDirection() {
        if (velocity.x < 0 && facingRight) {
            spriteRenderer.flipX = true;
            facingRight = !facingRight;
        }
        else if (velocity.x > 0 && !facingRight) {
            spriteRenderer.flipX = false;
            facingRight = !facingRight;
        }
    }

}
