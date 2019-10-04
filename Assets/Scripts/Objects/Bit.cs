using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bit : PhysicsObject {

    private Transform target;
    private bool go;

    protected override void ObjectSetup() {
        gravityMultiplier = 0f;
        bc2d.enabled = false;
        go = false;
        target = GameObject.Find("Player").transform;
    }

    protected override void GetMovement() {
        if(go) {
            MoveTowardTarget();
            CheckDistace();
        }
    }
   
    private void MoveTowardTarget() {
        float ySpeed = 5f;
        float xSpeed = 1.5f;
        float pull = 0.1f;
        float push = 4f;
        float x = target.position.x - transform.position.x;
        if ((x > 0 && velocity.x < 0) || (x < 0 && velocity.x > 0)) {
            velocity.x += xSpeed * x * pull;
        } else {
            velocity.x = xSpeed * x * push;
        }
        float y = target.position.y - transform.position.y;
        if ((y > 0 && velocity.y < 0) || (y < 0 && velocity.y > 0)) {
            velocity.y += ySpeed * y * pull;
        }
        else {
            velocity.y = xSpeed * y * push;
        }
    }

    private void CheckDistace() {
        float minDis = 0.15f;
        Vector3 v = target.position - transform.position;
        float mag = v.magnitude;
        Debug.Log(mag);
        if (mag <= minDis) {
            Destroy(gameObject);
        }
    }

    public void SetVelocity(Vector3 v) {
        velocity = v;
        go = true;
    }

}
