using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    protected PlayerController player;
    protected Stats pStats;

    protected CircleCollider2D interactTrigger;
    protected ContactFilter2D contactFilter;
    protected bool buttonShowing;

    protected GameObject button;

	// Use this for initialization
	void Start () {

        player = (PlayerController)GameObject.Find("Player").GetComponent(typeof(PlayerController));
        interactTrigger = (CircleCollider2D)GetComponent(typeof(CircleCollider2D));
        contactFilter.useTriggers = true;
        contactFilter.layerMask = Physics2D.GetLayerCollisionMask(gameObject.layer);
        contactFilter.useLayerMask = true;
        buttonShowing = false;
        ChildStart();

	}

	// Update is called once per frame
	void LateUpdate () {
        if(pStats == null) {
            SetUpStats();
        }

        if (interactTrigger.IsTouching(contactFilter) && !buttonShowing) {
            ShowInterActButton();
        } else if(buttonShowing && !interactTrigger.IsTouching(contactFilter)) {
            HideButton();
        }

        if (buttonShowing && Input.GetButtonDown("Confirm")) {
            Interact();
        }
        ChildUpdate();
    }

    protected void SetUpStats() {
        pStats = player.GetStats();
    }

    protected virtual void ChildStart() { }

    protected virtual void ChildUpdate() { }

    protected virtual void Interact() { }

    protected void HideButton() {
        Destroy(button);
        buttonShowing = false;
    }

    protected void ShowInterActButton() {
        Transform canvas = GameObject.Find("WorldHUD").transform;
        if(canvas != null) {
            button = (GameObject)Instantiate(Resources.Load("UI/InteractButton"));
            button.transform.SetParent(canvas);
            Vector3 pos = transform.position;
            pos.y += transform.localScale.y / 1.8f;
            pos.z = -5;
            button.transform.position = pos;
            buttonShowing = true;
        }
    }

}
