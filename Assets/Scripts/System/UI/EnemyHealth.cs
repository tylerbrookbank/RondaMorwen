using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {

    private Image healthBar;
    private Stats enemyStats;
    private Transform enemyTransform;

    void Awake() {
        healthBar = null;
        foreach(Transform child in transform) {
            if(child.name.Equals("EnemyHealthFront")) {
                healthBar = (Image)child.GetComponent(typeof(Image));
            }
        }
    }

    void Update() {
        //enemyTransform will be null if player was killed and
        //also Get damaged the enemy. this causes the health bar 
        //to be spawned with no enemy, due to despawning when player
        //respawns - therefore destroy when null
        if(enemyTransform == null) {
            Destroy(gameObject);
        } else if (enemyStats == null) {
            SetStats();
        } else {
            ResetLocation();
            UpdateHealthBar();
            CheckDeath();
        }
    }

    public void SetEnemyTransform(Transform t) {
        enemyTransform = t;
    }

    private void CheckDeath() {
        if(enemyStats.GetDead()) {
            Destroy(gameObject);
        }
    }

    private void SetStats() {
        if(enemyTransform != null) {
            enemyStats = ((EasyEnemyController)enemyTransform.GetComponent(typeof(EasyEnemyController))).GetStats();
        }
    }

    private void ResetLocation() {
        float y;
        if (enemyTransform.parent != null) {
            y = enemyTransform.localScale.y * enemyTransform.parent.localScale.y;
            y = y / 1.5f;
        }
        else {
            y = enemyTransform.localScale.y / 1.5f;
        }
        Vector3 pos = enemyTransform.position + new Vector3(0, y, 0);
        transform.position = pos;
    }

    private void UpdateHealthBar() {

        float curr = enemyStats.GetCurrentHealth();
        float max = enemyStats.GetHealth();

        float fill = curr / max;
        healthBar.fillAmount = fill;

    }

}
