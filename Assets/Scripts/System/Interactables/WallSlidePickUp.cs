using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlidePickUp : PickUp {

    protected override void PutInInventory() {
        inventory.PickUpWallSlide();
    }

}
