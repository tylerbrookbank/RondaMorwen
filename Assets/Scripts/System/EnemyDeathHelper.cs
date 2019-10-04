using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathHelper : MonoBehaviour {

    private SpriteRenderer sprite;
    private Stats stats;

    // Use this for initialization
    void Start() {
        sprite = (SpriteRenderer)GetComponent(typeof(SpriteRenderer));
    }

    // Update is called once per frame
    void Update() {
        if (stats == null) {
            GetStats();
        } else {
            GetStats();
            CheckDeath();
        }
    }

    private void CheckDeath() {
        if(stats.GetDead()) {
            DespawnEnemy();
        }
    }

    private void DespawnEnemy() {
        float a = sprite.color.a;
        a -= Time.deltaTime;
        Color c = new Color(sprite.color.r, sprite.color.g, sprite.color.b, a);
        sprite.color = c;
        if(a <= 0f) {
            Destroy(gameObject);
        }
    }

    private void GetStats() {
        stats = ((EasyEnemyController)GetComponent(typeof(EasyEnemyController))).GetStats();
    }

}

