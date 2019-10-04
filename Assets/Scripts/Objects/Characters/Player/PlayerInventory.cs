using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory {

    private bool doubleJump;
    private bool dash;
    private bool wallSlide;
    private bool weaponUpgrade;

    public PlayerInventory() {
        doubleJump = false;
        dash = false;
        wallSlide = false;
        weaponUpgrade = false;
    }

    public bool GetDoubleJump() {
        return doubleJump;
    }

    public void PickUpDoubleJump() {
        doubleJump = true;
    }

    public bool GetDash() {
        return dash;
    }

    public void PickUpDash() {
        dash = true;
    }

    public bool GetWallSlide() {
        return wallSlide;
    }

    public void PickUpWallSlide() {
        wallSlide = true;
    }

    public bool GetWeaponUpgrade() {
        return weaponUpgrade;
    }
    
    public void PickUpWeaponUpgrade() {
        weaponUpgrade = true;
    }

    public void SpendWeaponUpgrade() {
        weaponUpgrade = false;
    }

}
