using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpPickUp : PickUp {

    protected override void PutInInventory() {
        inventory.PickUpDoubleJump();
    }

}
