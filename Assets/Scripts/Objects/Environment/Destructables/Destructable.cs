using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour {

    protected BoxCollider2D hitBox;
    protected ContactFilter2D hitBoxFilter;

    private Vector3 position;
    private Vector3 currentPosition;
    private float direction;

    protected Stats stats;

    protected byte lastAttackKey = 0;

    void Start() {

        hitBox = null;
        foreach (Transform child in transform) {
            if (child.gameObject.name.Equals("hitBox")) {
                hitBox = (BoxCollider2D)child.GetComponent(typeof(BoxCollider2D));
            }
        }

        hitBoxFilter.useTriggers = true;
        hitBoxFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        hitBoxFilter.useLayerMask = true;
        position = gameObject.transform.position;
        currentPosition = position;

        stats = new Stats();
        ObjectSetup();

    }

	// Update is called once per frame
	void Update () {
        ChildUpdate();
	}

    protected virtual void CheckHitBox() {
        if (hitBox != null
            && hitBox.IsTouching(hitBoxFilter)) {
            HitBoxHit();
        }
    }

    protected virtual void ObjectSetup() { }

    protected virtual void DestroyObject() {

    } 

    protected virtual void ChildUpdate() {
        CheckHitBox();
        CheckPosition();
        CheckDead();
    }

    protected virtual void TakeDamage(Attack a) {

        float damage = a.GetDamage();
        damage -= stats.GetDefence();
        float force = a.GetForce();
        ForcePush(force);
        ShowDamageText(damage);
        stats.DamageHealth(damage);

    }

    private void ForcePush(float f) {
        Vector3 push = new Vector3(f / 8f, 0f, 0f);
        direction = f * -1;
        gameObject.transform.position = position + push;
        currentPosition = gameObject.transform.position;
    }

    protected void CheckDead() {
        if(stats.GetDead()) {
            DestroyObject();
        }
    }

    private void CheckPosition() {
        if (currentPosition != position) {
            float speed = 4f;
            float positionBuffer = 0.01f;
            Vector3 move = new Vector3(direction * speed * Time.deltaTime, 0f, 0f);
            gameObject.transform.position = currentPosition + move;
            currentPosition = gameObject.transform.position;

            if (direction < 0 && currentPosition.x - position.x <= positionBuffer) {
                gameObject.transform.position = position;
                currentPosition = position;
            }
            else if (direction > 0 && position.x - currentPosition.x <= positionBuffer) {
                gameObject.transform.position = position;
                currentPosition = position;
            }
        }
    }

    protected virtual void HitBoxHit() {
        Attack a = null;
        Collider2D[] contact = new Collider2D[1];
        PlayerCombatHelper p = null;

        int count = hitBox.GetContacts(contact);
        if (count > 0) {
            p = (PlayerCombatHelper)contact[0].gameObject.GetComponentInParent(typeof(PlayerCombatHelper));
        }

        if (p != null) {
            a = p.GetAttack();
        }

        if (a != null && a.GetAttackKey() != lastAttackKey) {
            lastAttackKey = a.GetAttackKey();
            TakeDamage(a);
        }
    }

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
}
