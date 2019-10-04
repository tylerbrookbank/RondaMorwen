using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyEnemyInput : MonoBehaviour {

    private int state = 0;
    private const int idle = 0;
    private const int moveToAttack = 1;
    private const int moveAwayFromTarget = 2;

    private const string horizontal = "Horizontal";
    private const string lightAttack = "LightAttacks";
    private const string strongAttack = "StrongAttack";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool GetAction(string act) {
        bool returnVal = false;

        return returnVal;
    }

    public float GetAxis() {
        float returnVal = 0f;
        return returnVal;
    }

}
