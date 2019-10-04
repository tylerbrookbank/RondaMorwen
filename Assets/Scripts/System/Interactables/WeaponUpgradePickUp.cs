using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgradePickUp : PickUp {

    protected override void PutInInventory() {
        inventory.PickUpWeaponUpgrade();
    }

}
