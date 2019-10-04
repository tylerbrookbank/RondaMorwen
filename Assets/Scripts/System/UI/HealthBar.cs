using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    private PlayerController player;
    private Stats pStats;

    private Image healthBar;
    private Image healthBarBack;

	// Use this for initialization
	void Start () {

        player = (PlayerController)(GameObject.Find("Player").GetComponent(typeof(PlayerController)));
        if(player == null) {
            Debug.Log("HealthBar: Could not find player. Destroying.");
            Destroy(gameObject);
        }

        healthBar = null;
        healthBarBack = null;
        foreach(Transform child in transform) {
            if(child.name.Equals("HealthBarFront")) {
                healthBar = (Image)child.GetComponent(typeof(Image));
            } else if(child.name.Equals("HealthBarBack")) {
                healthBarBack = (Image)child.GetComponent(typeof(Image));
            }
        }
        if(healthBar == null || healthBarBack == null) {
            Debug.Log("HealthBar: Could not set up HealthBar. Destroying.");
            Destroy(gameObject);
        }

	}
	
	// Update is called once per frame
	void OnGUI() {
		if(pStats == null) {
            pStats = player.GetStats();
        } else {
            CheckHealthBarVisability();
            UpdateHealthBar();
        }
	}

    private void CheckHealthBarVisability() {
        healthBar.enabled = player.IsInControl();
        healthBarBack.enabled = player.IsInControl();
    }

    private void UpdateHealthBar() {

        float fill = pStats.GetCurrentHealth() / pStats.GetHealth();
        healthBar.fillAmount = fill;

    }

}
