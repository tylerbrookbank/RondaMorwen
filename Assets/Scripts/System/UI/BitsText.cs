using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BitsText : MonoBehaviour {

    private PlayerController player;
    private Stats pStats;

    private Text bits;
    private Text newBits;

    private int oldBits;
    private int addBits;

    private bool addingBits;

	// Use this for initialization
	void Start () {
        pStats = null;
        bits = null;
        newBits = null;
        foreach(Transform child in transform) {
            if(child.name.Equals("Bits")) {
                bits = (Text)child.GetComponent(typeof(Text));
            } else if(child.name.Equals("NewBits")){
                newBits = (Text)child.GetComponent(typeof(Text));
            }
        }
        player = (PlayerController)GameObject.Find("Player").GetComponent(typeof(PlayerController));
        if(bits == null || newBits == null || player == null) {
            Debug.Log("BitText couldnt load, Destroying");
            Destroy(gameObject);
        }
        newBits.enabled = false;
	}
	
	// Update is called once per frame
	void OnGUI() {
		if(pStats == null) {
            SetUpStats();
        } else {
            UpdateBits();
        }
        CheckVisability();
	}

    private void CheckVisability() {
        bits.enabled = player.IsInControl();
        if(newBits.enabled) {
            newBits.enabled = player.IsInControl();
        }
    }

    private void UpdateBits() {
        addBits = pStats.GetBits() - oldBits;
        if(addBits > 0) {
            oldBits++;
            newBits.enabled = true;
            newBits.text = "+" + addBits;
            bits.text = "" + oldBits;
        } else {
            newBits.enabled = false;
            oldBits = pStats.GetBits();
            bits.text = "" + oldBits;
        }
    }

    private void SetUpStats() {
        pStats = ((PlayerController)GameObject.Find("Player").GetComponent(typeof(PlayerController))).GetStats();
        bits.text = "" + pStats.GetBits();
        oldBits = pStats.GetBits();
    }

}
