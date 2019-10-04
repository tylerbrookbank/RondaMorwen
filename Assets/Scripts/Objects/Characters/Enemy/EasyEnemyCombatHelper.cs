using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyEnemyCombatHelper : CombatHelper {

    protected Vector3 currentPosition;
    protected Vector3 position;

    //this is used by EnemyHealthBar
    //EnemyHealthBar when enabled tries to find the 
    //Enemy that wants to turn their health bar on
    private bool healthBarOn;
    private EasyEnemyAttack eAttack;

    protected override void ObjectSetup() {

        position = gameObject.transform.position;
        currentPosition = position;
        healthBarOn = false;

    }

    protected override void ChildUpdate() {
        if(stats == null || eAttack == null) {
            SetUpCombatHelper();
        }
        CheckPosition();
        CheckStun();
    }

    //if stunned make sure they cant attack or be hurt
    private void CheckStun() {
        if(stats.GetStun()) {
            eAttack.StopAttack();
        }
    }

    private void SetUpCombatHelper() {
        stats = character.GetStats();
        eAttack = ((EasyEnemyController)character).GetEnemyAttack();
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

    protected override void TakeDamage(Attack a) {
        float damage = a.GetDamage();
        damage -= stats.GetDefence();
        float force = a.GetForce();
        ForcePush(force);
        ShowDamageText(damage);
        if(!healthBarOn)
            SpawnHealthBar();
        stats.DamageHealth(damage);
    }

    private void SpawnHealthBar() {
        healthBarOn = true;
        GameObject obj = (GameObject)Instantiate(Resources.Load("UI/EnemyHealth"));
        if (obj != null) {
            GameObject canvas = GameObject.Find("WorldHUD");
            EnemyHealth healthBar = (EnemyHealth)obj.GetComponent(typeof(EnemyHealth));
            healthBar.SetEnemyTransform(transform);

            if (canvas != null) {
                obj.transform.SetParent(canvas.transform);
            }
        }
    }

    private void ForcePush(float f) {
        stats.Stun(Mathf.Abs(f));
        Vector3 push = new Vector3(f, 0f, 0f);
        direction = f * -1;

        //position = gameObject.transform.position;
        //currentPosition = position;

        //gameObject.transform.position = position + push;
        //currentPosition = gameObject.transform.position;
    }

    protected override void HitBoxHit() {
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

    public override Attack GetAttack() {
        float levelMulti = 0.5f;
        float dmg = stats.GetLevel() * levelMulti + stats.GetAttack() * eAttack.GetDamageMultiplier();
        float force = eAttack.GetForce();
        if (!eAttack.GetFacingRight()) force *= -1;
        Attack a = new Attack(stats.GetLevel(), dmg, force, 0.1f, eAttack.GetAttackKey());
        return a;
    }

}
