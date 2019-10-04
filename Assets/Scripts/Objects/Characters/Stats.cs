using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats {

    private string name;
    private int level;
    private int attack;
    private int defence;
    private int stunDef;
    private float health;
    private float currentHealth;

    //exp
    private int bits;
    private int nextCost;

    private bool stunned;
    private bool damaged;

    private float stunTimer;
    private float stunTime;

    public Stats() {
        level = 1;
        attack = 1;
        defence = 1;
        health = 100;
        currentHealth = 100;
        stunDef = 10;
        stunTime = 0.75f;
        nextCost = 100;
    }

    public void Update() {
        CheckStun();
    }

    private void CheckStun() {
        if(stunned) {
            if(stunTimer >= stunTime) {
                stunned = false;
            }
            stunTimer += Time.deltaTime;
        }
    }

    public bool GetDead() {
        bool returnVal = false;
        if (currentHealth == 0)
            returnVal = true;
        return returnVal;
    }

    public string getName() {
        return name;
    }

    public void SetName(string name) {
        this.name = name;
    }

    public int GetLevel() {
        return level;
    }

    public int GetAttack() {
        return attack;
    }

    public int GetDefence() {
        return defence;
    }

    public float GetHealth() {
        return health;
    }

    public float GetCurrentHealth() {
        return currentHealth;
    }

    public void SetCurrentHealth(float hp) {
        currentHealth = hp > health ? health : hp;
    }

    public float DamageHealth(float dmg) {

        currentHealth -= dmg;
        currentHealth = currentHealth > 0 ? currentHealth : 0;
        damaged = true;

        return currentHealth;
    }

    public void Stun(float force) {
        if(force > stunDef) {
            stunned = true;
            stunTimer = 0f;
        }
    }

    public bool GetStun() {
        return stunned;
    }

    /*increases stat based on string given
     *returns false if input stat is invalid
     *true otherwise*/
    public bool IncreaseStat(string stat) {
        bool returnVal = true;
        if (stat.Equals("attack")) {
            level++;
            attack++;
        } else if (stat.Equals("defence")) {
            level++;
            defence++;
        } else if (stat.Equals("hp")) {
            level++;
            health += 50;
        } else { 
            returnVal = false;
        }
        return returnVal;
    }

    public bool GetDamaged() {
        return damaged;
    }

    public void SetDamaged(bool dmg) {
        damaged = dmg;
    }

    public void AddBits(int b) {
        bits += b;
    }

    public int GetBits() {
        return bits;
    }

    public int GetNextCost() {
        return nextCost;
    }

}
