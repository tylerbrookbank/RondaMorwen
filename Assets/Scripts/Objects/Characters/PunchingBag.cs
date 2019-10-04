using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PunchingBag : MonoBehaviour {

    private BoxCollider2D hitBox;

    private Vector3 position;
    private Vector3 currentPosition;
    private float direction;

    private Stats stats;

    //used to tell what attack hit last
    //to ensure a single attack does damage once
    private byte lastAttackKey;

    ContactFilter2D hitBoxFilter;


	// Use this for initialization
	void Start () {

        hitBox = null;
        foreach(Transform child in transform) {
            if(child.gameObject.name.Equals("hitBox")) {
                hitBox = (BoxCollider2D)child.GetComponent(typeof(BoxCollider2D));
            }
        }

        hitBoxFilter.useTriggers = true;
        hitBoxFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        hitBoxFilter.useLayerMask = true;
        position = gameObject.transform.position;
        currentPosition = position;

        stats = new Stats();

    }
	
	// Update is called once per frame
	void Update () {
        CheckHitBox();
        CheckPosition();
	}

    private void CheckHitBox() {
        if(hitBox != null
            && hitBox.IsTouching(hitBoxFilter)) {
            HitBoxHit();
        }
    }

    private void HitBoxHit() {

        Attack a = null;
        Collider2D []contact = new Collider2D[1];
        PlayerCombatHelper p = null;

        int count = hitBox.GetContacts(contact);
        Debug.Log("count is " + count);
        if (count > 0) {
            p = (PlayerCombatHelper)contact[0].gameObject.GetComponentInParent(typeof(PlayerCombatHelper));
        }

        if(p != null) {
            a = p.GetAttack();
        }

        if(a !=null && a.GetAttackKey() != lastAttackKey) {
            lastAttackKey = a.GetAttackKey();
            TakeDamage(a);
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
            } else if (direction > 0 && position.x - currentPosition.x <= positionBuffer) {
                gameObject.transform.position = position;
                currentPosition = position;
            }
        }
    }

    private void ForcePush(float f) {
        Vector3 push = new Vector3(f, 0f, 0f);
        direction = f * -1;
        gameObject.transform.position = position + push;
        currentPosition = gameObject.transform.position;
    }

    private void TakeDamage(Attack a) {
        float damage = a.GetDamage();
        damage -= stats.GetDefence();
        float force = a.GetForce();
        ForcePush(force);
        ShowDamageText(damage);
    }

    private void ShowDamageText(float dmg) {
        GameObject obj = (GameObject) Instantiate(Resources.Load("DamageText"));
        if(obj != null) {
            GameObject canvas = GameObject.Find("Canvas");

            float y;
            if(transform.parent != null) {
                y = transform.localScale.y * transform.parent.localScale.y;
                y = y / 1.8f;
            } else {
                y = transform.localScale.y / 1.8f;
            }
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, y, 0));

            if(canvas != null) {
                obj.transform.SetParent(canvas.transform);
            } else {
                Debug.Log("Failed to find Canvas object");
            }
            DamageText damageText = (DamageText)obj.GetComponent(typeof(DamageText));
            if(damageText != null) {
                damageText.SetDamage(dmg);
                damageText.SetPosition();
            } else {
                Debug.Log("Failed to get DamageText Objext");
            }
        } else {
            Debug.Log("Failed to instantiate DamageText Prefab");
        }
    }

}
