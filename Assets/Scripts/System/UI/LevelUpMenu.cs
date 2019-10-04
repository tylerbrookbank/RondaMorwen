using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpMenu : Menu {

    private const int panelCount = 3;
    private const int strIndex = 2;
    private const int defIndex = 1;
    private const int hpIndex = 0;

    private PlayerController player;

    protected override void ChildStart() {
        interactPanels = new UnityEngine.UI.Image[panelCount];
        currentSelected = hpIndex;
        foreach(Transform c in transform) {
            if(c.name.Equals("InteractPanels")) {
                foreach(Transform child in c) {
                    if(child.name.Equals("StrengthPanel")) {
                        interactPanels[strIndex] = (Image)child.GetComponent(typeof(Image));
                        unSelectedColor = interactPanels[strIndex].color;
                    } else if(child.name.Equals("DefencePanel")) {
                        interactPanels[defIndex] = (Image)child.GetComponent(typeof(Image));
                    } else if(child.name.Equals("HpPanel")) {
                        interactPanels[hpIndex] = (Image)child.GetComponent(typeof(Image));
                        selectedColor = interactPanels[hpIndex].color;
                    }
                }
            }
        }
        player = (PlayerController)GameObject.Find("Player").GetComponent(typeof(PlayerController));
        if(player == null) {
            Debug.Log("LevelUpMenu: Could not find player.");
            Destroy(gameObject);
        }
    }

    protected override void ChildUpdate() {
        RefreshGUI();
        CheckSelected();
        if(Input.GetButtonDown("Cancel")) {
            SpawnNewWindow("SavePointMenu");
        } else if(Input.GetButtonDown("Confirm")) {
            LevelUp();
        }
    }

    private void LevelUp() {
        string levelUp = "";
        if (CheckCost()) {
            switch(currentSelected) {
                case hpIndex:
                    levelUp = "hp";
                    break;
                case strIndex:
                    levelUp = "attack";
                    break;
                case defIndex:
                    levelUp = "defence";
                    break;
            }
            player.GetStats().AddBits(-1 * player.GetStats().GetNextCost());
            player.GetStats().IncreaseStat(levelUp);
        } else {
            //spawn a nsf notice
        }
    }

    private bool CheckCost() {
        bool returnVal = true;
        if(player.GetStats().GetBits() < player.GetStats().GetNextCost()) {
            returnVal = false;
        }
        return returnVal;
    }

    private void RefreshGUI() {
        Color sc = new Color(0, 0.4f, 0.9f);
        Color c = new Color(0.8f, 0.8f, 0.8f);
        Color nsf = new Color(0.5f, 0, 0);
        foreach(Transform t in transform) {
            if(t.name.Equals("InteractPanels")) {
                foreach(Transform child in t) {
                    if(child.name.Equals("HpPanel")) {
                        Text text = (Text)child.GetComponentInChildren(typeof(Text));
                        if(currentSelected == hpIndex) {
                            text.text = "HP: " + (player.GetStats().GetHealth() + 50);
                            text.color = sc;
                        } else {
                            text.text = "HP: " + player.GetStats().GetHealth();
                            text.color = c;
                        }
                    } else if(child.name.Equals("StrengthPanel")) {
                        Text text = (Text)child.GetComponentInChildren(typeof(Text));
                        if (currentSelected == strIndex) {
                            text.text = "STR: " + (player.GetStats().GetAttack() + 1);
                            text.color = sc;
                        } else {
                            text.text = "STR: " + player.GetStats().GetAttack();
                            text.color = c;
                        }
                    } else if(child.name.Equals("DefencePanel")) {
                        Text text = (Text)child.GetComponentInChildren(typeof(Text));
                        if (currentSelected == defIndex) {
                            text.text = "DEF: " + (player.GetStats().GetDefence() + 1);
                            text.color = sc;
                        } else {
                            text.text = "DEF: " + player.GetStats().GetDefence();
                            text.color = c;
                        }
                    }
                }
            } else if(t.name.Equals("DisplayPanels")) {
                foreach (Transform child in t) {
                    if (child.name.Equals("LevelPlate")) {
                        Text text = (Text)child.GetComponentInChildren(typeof(Text));
                        text.text = "Level: " + player.GetStats().GetLevel();
                    }
                    else if (child.name.Equals("CurrentBitsPlate")) {
                        Text text = (Text)child.GetComponentInChildren(typeof(Text));
                        text.text = "Bits: " + player.GetStats().GetBits();
                        if (!CheckCost()) {
                            text.color = nsf;
                        } else {
                            text.color = c;
                        }
                    } else if(child.name.Equals("ReqBitsPlate")) {
                        Text text = (Text)child.GetComponentInChildren(typeof(Text));
                        text.text = "Cost: " + player.GetStats().GetNextCost();
                    }
                }
            }
        }
    }

}
