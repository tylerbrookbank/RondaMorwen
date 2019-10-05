using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitCollection : Destructable {
    
    public int bitAmount;

    private GameObject spawner;

    protected override void ObjectSetup() {

    }

    protected override void ChildUpdate() {
        base.ChildUpdate();
    }

    //use this to destory the spawner so it doesnt respawn
    //if destroyed
    public void SetSpawnerObject(GameObject spawner) {
        this.spawner = spawner;
    }

    public void SetBitAmount(int bitAmount) {
        this.bitAmount = bitAmount;
    }

    protected override void DestroyObject() {
        base.DestroyObject();
        Stats playerStats = ((Character)GameObject.Find("Player").GetComponent(typeof(Character))).GetStats();
        if (playerStats == null) {
            Debug.Log("Could Not Find Player Stats");
            Destroy(gameObject);
        }
        playerStats.AddBits(bitAmount);
        Destroy(gameObject);
        Destroy(spawner);
    }

}
