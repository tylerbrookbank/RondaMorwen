using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Interactable {

    protected PlayerInventory inventory;

    protected override void ChildUpdate() {
        if(inventory == null) {
            inventory = player.GetInventory();
        }
    }

    protected override void Interact() {
        PutInInventory();
        buttonShowing = false;
        HideButton();
        DestroyThisObject();
    }

    protected virtual void PutInInventory() { }

    protected void DestroyThisObject() {
        Destroy(gameObject);
    }

}
