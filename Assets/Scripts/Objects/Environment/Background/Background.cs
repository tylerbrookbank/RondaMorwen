using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    private int maxStars;

    private float maxY;
    private float maxX;

	// Use this for initialization
	void Start () {
        maxStars = 10000;
        maxY = 5.4f;
        maxX = 9.6f;
        SpawnStars();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void SpawnStars() {
        for(int i=0; i<maxStars; i++) {
            Transform obj = ((GameObject)Instantiate(Resources.Load("Background/Star"), gameObject.transform)).transform;
            float x = Random.Range(-1*maxX, maxX);
            float y = Random.Range(-1 * maxY, maxY);
            Vector3 l = new Vector3(x, y, 0);
            obj.localPosition = l;
        }
    }

}
