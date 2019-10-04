using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePointMenu : Menu {

    private const int panelCount = 2;
    private const int levelUpIndex = 0;
    private const int cancelIndex = 1;

    protected override void ChildStart() {

        interactPanels = new Image[panelCount];
        currentSelected = levelUpIndex;
        foreach (Transform c in transform) {
            if (c.name.Equals("InteractPanels")) {
                foreach (Transform child in c.transform) {
                    if (child.name.Equals("LevelUpPanel")) {
                        interactPanels[levelUpIndex] = (Image)child.GetComponent(typeof(Image));
                        selectedColor = interactPanels[levelUpIndex].color;
                    }
                    else if (child.name.Equals("CancelPanel")) {
                        interactPanels[cancelIndex] = (Image)child.GetComponent(typeof(Image));
                        unSelectedColor = interactPanels[cancelIndex].color;
                    }
                }
            }
        }

        PlayerController p = (PlayerController)GameObject.Find("Player").GetComponent(typeof(PlayerController));
        Stats s = p.GetStats();
        s.SetCurrentHealth(s.GetHealth());

    }

    protected override void ChildUpdate() {
        CheckSelected();
        if(Input.GetButtonDown("Confirm")) {
            switch(currentSelected) {
                case levelUpIndex:
                    LevelUp();
                    break;
                case cancelIndex:
                    Cancel();
                    break;
            }
        } else if(Input.GetButtonDown("Cancel")) {
            Cancel();
        }
    }

    private void LevelUp() {
        SpawnNewWindow("LevelUpMenu");
    }

    private void Cancel() {
        Destroy(gameObject);
    }

}
