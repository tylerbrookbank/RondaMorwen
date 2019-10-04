using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatHelper : CombatHelper {

    PlayerAttack pAttack;

    protected override void ObjectSetup() {
        
    }

    protected override void ChildUpdate() {
        if(pAttack == null || stats == null) {
            SetUpPlayerAttack();
        }

    }

    protected override void HitBoxHit() {

        Attack a = null;
        Collider2D[] contact = new Collider2D[1];
        EasyEnemyCombatHelper e = null;

        int count = hitBox.GetContacts(contact);
        if (count > 0) {
            e = (EasyEnemyCombatHelper)contact[0].gameObject.GetComponentInParent(typeof(EasyEnemyCombatHelper));
        }

        if (e != null) {
            a = e.GetAttack();
        }

        if (a != null && a.GetAttackKey() != lastAttackKey) {
            lastAttackKey = a.GetAttackKey();
            TakeDamage(a);
        }

    }

    protected override void TakeDamage(Attack a) {
        float damage = a.GetDamage();
        damage -= stats.GetDefence();
        ShowDamageText(damage);
        stats.DamageHealth(damage);
    }

    private void SetUpPlayerAttack() {
        pAttack = ((PlayerController)character).GetPlayerAttack();
        stats = character.GetStats();
    }

    public override Attack GetAttack() {
        float levelMulti = 0.5f;
        float dmg = stats.GetLevel() * levelMulti + stats.GetAttack() * pAttack.GetDamageMultiplier();
        float force = pAttack.GetForce();
        if (!pAttack.GetFacingRight()) force *= -1;
        Attack a = new Attack(stats.GetLevel(), dmg, force, 0.1f, pAttack.GetAttackKey());
        return a;
    }

}
