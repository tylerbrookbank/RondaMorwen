using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPickUp : PickUp {

    protected override void PutInInventory() {
        inventory.PickUpDash();
    }

}
