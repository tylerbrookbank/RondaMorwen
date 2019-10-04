using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    protected Image[] interactPanels;
    protected Color selectedColor;
    protected Color unSelectedColor;

    protected int currentSelected;

    protected bool selected;

    void Awake() {
        selected = false;
        ChildStart();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        ChildUpdate();
	}

    protected virtual void CheckSelected() {

        if(!selected) {
            float selDir = Input.GetAxis("Vertical");
            if(selDir != 0) {
                int lastSelected = currentSelected;
                currentSelected = selDir > 0 ? currentSelected - 1 : currentSelected + 1;
                currentSelected = currentSelected < 0 ? interactPanels.Length - 1 : currentSelected;
                currentSelected = currentSelected == interactPanels.Length ? 0 : currentSelected;

                interactPanels[lastSelected].color = unSelectedColor;
                interactPanels[currentSelected].color = selectedColor;
                foreach (Transform c in interactPanels[lastSelected].transform) {
                    if (c.name.Contains("Selected")) {
                        ((Image)c.GetComponent(typeof(Image))).enabled = false;
                    }
                }
                foreach (Transform c in interactPanels[currentSelected].transform) {
                    if (c.name.Contains("Selected")) {
                        ((Image)c.GetComponent(typeof(Image))).enabled = true;
                    }
                }
                selected = true;
            }
        } else {
            if(Input.GetAxis("Vertical") == 0) {
                selected = false;
            }
        }

    }

    protected virtual void ChildStart() { }

    protected virtual void ChildUpdate() { }

    protected void SpawnNewWindow(string name) {
        Transform canvas = GameObject.Find("UI").transform;
        if (canvas != null) {
            GameObject obj = (GameObject)Instantiate(Resources.Load("UI/"+name), canvas);
            if (obj != null) {
                obj.transform.localPosition = new Vector3(0, 0, 0);
                obj.name = "SavePointMenu";
                Destroy(gameObject);
            } else {
                Debug.Log("Menu - SpawnNewWidnow: could not find window " + name + ".");
            }
        } else {
            Debug.Log("Menu - SpawnNewWidnow: Could not find UI.");
        }
    }

    void OnDestroy() {
        PlayerController p = (PlayerController)GameObject.Find("Player").GetComponent(typeof(PlayerController));
        p.GiveControls();
    }

}
