using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingWindow : MonoBehaviour {

    private Color minColor;
    private Color maxColor;

    private float minR;
    private float minG;
    private float minB;
    private float dimAmount = 0.25f;

    private const float blueR = 0f;
    private const float blueG = 0.22f;
    private const float blueB = 0.58f;

    private const float pinkR = 0.4f;
    private const float pinkG = 0.0f;
    private const float pinkB = 0.4f;

    private const float whiteR = 0.4f;
    private const float whiteG = 0.4f;
    private const float whiteB = 0.1f;

    private int color;
    private const int pink = 0;
    private const int blue = 1;
    private const int white = 2;

    private bool dimming;

	// Use this for initialization
	void Start () {
		
        float num = Random.Range(0,99);
        dimming = num % 2 == 0;
        if(num < 33) {
            color = white;
        } else if(num < 66) {
            color = pink;
        } else {
            color = blue;
        }
        switch(color) {
            case pink:
                minR = pinkR;
                minG = pinkG;
                minB = pinkB;
                break;
            case white:
                minR = whiteR;
                minG = whiteG;
                minB = whiteB;
                break;
            case blue:
                minR = blueR;
                minG = blueG;
                minB = blueB;
                break;

        }
        num = num / 100;
        SetColor(num);

	}
	
	// Update is called once per frame
	void Update () {
        Flicker();
	}

    private void Flicker() {
        float add = dimAmount * Time.deltaTime;
        add = dimming ? add*-1 : add;
        foreach (Transform t in transform) {
            if (t.name.Contains("Window")) {
                SpriteRenderer sp = (SpriteRenderer)t.GetComponent(typeof(SpriteRenderer));
                if (color == blue) {
                    sp.color = new Color(0, sp.color.g + add, sp.color.b + add);
                } else {
                    sp.color = new Color(sp.color.r + add, sp.color.g + add, sp.color.b + add);
                }
                if((sp.color.g <= minG && dimming) || (sp.color.g >= (minG + dimAmount) && !dimming)) {
                    dimming = !dimming;
                }
            }
        }
    }

    private void SetColor(float num) {
        float add = dimAmount * num;
        foreach (Transform t in transform) {
            if(t.name.Contains("Window")) {
                SpriteRenderer sp = (SpriteRenderer)t.GetComponent(typeof(SpriteRenderer));
                if (color == blue) {
                    sp.color = new Color(0, minG + add, minB + add);
                }
                else {
                    sp.color = new Color(minR + add, minG + add, minB + add);
                }
            }
        }
    }

}
