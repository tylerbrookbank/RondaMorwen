using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour {

    private Text damageText;
    private Transform characterTransform;

    void Awake() {

        damageText = (Text)GetComponent(typeof(Text));
        if (damageText == null) {
            Debug.Log("DamageText not attached to Text field, destroying.");
            Destroy(gameObject);
        }

    }

	// Use this for initialization
	void Start () {

        damageText = (Text)GetComponent(typeof(Text));
        if (damageText == null) {
            Debug.Log("DamageText not attached to Text field, destroying.");
            Destroy(gameObject);
        }

	}
	
	// Update is called once per frame
	void Update () {
        UpdatePosition();
        Float();
        Fade();
	}

    public void SetDamage(float damage) {
        if (damageText != null)
            damageText.text = "" + damage;
        else
            Debug.Log("Huh");
    }

    public void SetCharacterTransform(Transform t) {
        characterTransform = t;
    }

    public void SetPosition() {
        Vector3 pos = characterTransform.position;
        float y = 0;
        if(characterTransform.parent != null) {
            y = characterTransform.localScale.y * characterTransform.parent.localScale.y;
            y = y / 1.8f;
        } else {
            y = characterTransform.localScale.y / 1.8f;
        }
        transform.position = pos + new Vector3(0,y,0);
        //transform.position = pos;
    }

    private void UpdatePosition() {
        if (characterTransform != null) {
            Vector3 pos = transform.position;
            pos.x = characterTransform.position.x;
            transform.position = pos;
        }
    }

    private void Float() {
        Vector3 newPos = transform.position;
        float speed = 1f;
        newPos.y += speed * Time.deltaTime;
        transform.position = newPos;
    }

    private void Fade() {
        float fadeTime = 1f;
        float a = damageText.color.a - fadeTime * Time.deltaTime;
        Color c = new Color(damageText.color.r, damageText.color.g, damageText.color.b, a);
        damageText.color = c;
        if(damageText.color.a <= 0) {
            Destroy(gameObject);
        }
    }

}
