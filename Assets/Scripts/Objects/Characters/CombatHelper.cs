using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatHelper : MonoBehaviour{

    protected Character character;
    protected Stats stats;
    protected float direction;

    //defensive hitBox
    protected BoxCollider2D hitBox;
    //offensive hitbox
    protected BoxCollider2D attackHitBox;
    protected Attack currentAttack;

    protected byte lastAttackKey = 0;

    protected ContactFilter2D hitBoxFilter;

    void Awake() {
        character = (Character)GetComponent(typeof(Character));
        hitBox = null;
        attackHitBox = null;
        foreach (Transform child in transform) {
            if (child.name.Equals("HitBox")) {
                hitBox = (BoxCollider2D)child.GetComponent(typeof(BoxCollider2D));
            }
            else if (child.name.Equals("AttackHitBox")) {
                attackHitBox = (BoxCollider2D)child.GetComponent(typeof(BoxCollider2D));
            }
        }

        if (character == null
            || hitBox == null
            || attackHitBox == null) {
            Debug.Log(gameObject.name + " Get not started correctly. Destroying.");
            Destroy(gameObject);
        }

        hitBoxFilter.useTriggers = true;
        hitBoxFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(hitBox.gameObject.layer));
        hitBoxFilter.useLayerMask = true;

        ObjectSetup();
    }

    void Start() {

    }

    void Update() {
        CheckHitBox();
        ChildUpdate();
        stats.Update();
    }

    protected virtual void ObjectSetup() { }

    protected virtual void ChildUpdate() { }

    protected virtual void TakeDamage(Attack a) { }

    protected virtual void HitBoxHit() { }

    protected void ShowDamageText(float dmg) {
        GameObject obj = (GameObject)Instantiate(Resources.Load("UI/DamageText"));
        if (obj != null) {
            GameObject canvas = GameObject.Find("WorldHUD");

            if (canvas != null) {
                obj.transform.SetParent(canvas.transform);
            }
            else {
                Debug.Log("Failed to find Canvas object");
            }
            DamageText damageText = (DamageText)obj.GetComponent(typeof(DamageText));
            if (damageText != null) {
                damageText.SetDamage(dmg);
                damageText.SetCharacterTransform(transform);
                damageText.SetPosition();
            }
            else {
                Debug.Log("Failed to get DamageText Objext");
            }
        }
        else {
            Debug.Log("Failed to instantiate DamageText Prefab");
        }
    }

    public virtual Attack GetAttack() {
        return new Attack();
    }

    protected virtual void CheckHitBox() {
        if (hitBox != null
            && hitBox.IsTouching(hitBoxFilter)) {
            HitBoxHit();
        }
    }

}
