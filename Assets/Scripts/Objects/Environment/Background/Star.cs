using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {

    private bool isDimming;

    private float max;
    private float min;

    private Vector3 location;
    private SpriteRenderer halo;

	// Use this for initialization
	void Start () {

        int num = Random.Range(0,99);
        float randPercent = num / 100f;
        isDimming = num % 2 == 0;

        location = transform.localPosition;

        halo = (SpriteRenderer)GetComponentInChildren(typeof(SpriteRenderer));

        if(halo == null) {
            Debug.Log("Star failed to start");
            Destroy(gameObject);
        }

        max = Random.Range(0, 99);
        max /= 100;
        min = 0.1f;

        float innerScale = max * randPercent;
        innerScale = min + innerScale;

        Color c = new Color(halo.color.r, halo.color.g, halo.color.b, innerScale);
        halo.color = c;
    }
	
	// Update is called once per frame
	void Update () {
        Flicker();
        CheckPosition();
	}

    private void CheckPosition() {
        transform.localPosition = location;
    }

    private void Flicker() {
        float increase = Time.deltaTime;
        increase = isDimming ? increase * -1 : increase;
        float h = 0.5f * increase;
        h = halo.color.a + h;
        Color c = new Color(halo.color.r, halo.color.g, halo.color.b, h);
        halo.color = c;
        if(halo.color.a <= min || halo.color.a >= max) {
            isDimming = !isDimming;
        }
    }

}
