using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitPool : Interactable {

    private int bits = 1000;

    protected override void ChildStart() {
        
    }

    protected override void ChildUpdate() {

    }

    protected override void Interact() {
        pStats.AddBits(bits);
        HideButton();
        Destroy(gameObject);
    }

    public void SetBits(int b) {
        bits = b;
    }

}
