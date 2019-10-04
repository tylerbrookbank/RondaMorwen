using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

    private PlayerController player;

	// Use this for initialization
	void Start () {
        player = (PlayerController)GameObject.Find("Player").GetComponent(typeof(PlayerController));
	}
	
	// Update is called once per frame
	void Update () {
        CheckMenuOpen();
	}

    private void CheckMenuOpen() {
        bool check = false;
        Transform t = GameObject.Find("UI").transform;
        foreach(Transform child in t) {
            if(child.name.Contains("Menu")) {
                check = true;
            }
        }
        if(check) {
            player.TakeControls();
            player.Stop();
        }
    }

    public bool IsPlaying() {
        return player.IsInControl();
    }

}
