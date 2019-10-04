using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack {

    private int level;
    private float dmg;
    private float force;
    private byte attackKey;
    private float critChance;

    public Attack() { }

    public Attack(int l, float d, float f, float cc, byte a) {
        level = l;
        dmg = d;
        force = f;
        critChance = cc;
        attackKey = a;
    }

    public float GetCritChance() {
        return critChance;
    }

    public void SetCritChance(float cc) {
        critChance = cc;
    }

    public int GetLevel() {
        return level;
    }

    public void SetLevel(int l) {
        level = l;
    }

    public float GetDamage() {
        return dmg;
    }

    public void SetDamage(float d) {
        dmg = d;
    }

    public float GetForce() {
        return force;
    }

    public void SetForce(float f) {
        force = f;
    }   

    public byte GetAttackKey() {
        return attackKey;
    }

    public void SetAttackKey(byte a) {
        attackKey = a;
    }

}

