using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitExplosion : MonoBehaviour {

	void Awake() {
        SpawnBits();
        Destroy(gameObject);
    }

    private void SpawnBits() {
        float speed = 25f;
        int numBits = 359;
        for(int i = 0; i < numBits; i+=36) {
            Bit b = (Bit)((GameObject)Instantiate(Resources.Load("Bit"))).GetComponent(typeof(Bit));
            float x = Mathf.Cos(i);
            float y = Mathf.Abs(Mathf.Sin(i));
            Vector3 vel = new Vector3(x,y,0);
            vel *= speed;
            b.transform.position = transform.position;
            b.SetVelocity(vel);
        }
    }

}
