using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour {

    private BoxCollider2D levelTrigger;
    private ContactFilter2D contactFilter;

    private bool loaded;
    public string levelName;

	// Use this for initialization
	void Start () {

        levelTrigger = null;
        foreach(Transform child in transform) {
            if(child.name.Equals("LevelTrigger")) {
                levelTrigger = (BoxCollider2D)child.GetComponent(typeof(BoxCollider2D));
                contactFilter.layerMask = Physics2D.GetLayerCollisionMask(child.gameObject.layer);
            }
        }
        if(levelTrigger == null) {
            Debug.Log(gameObject.name + " could not load correctly, check level structure. Level wwill not work correctly.");
            levelTrigger = new BoxCollider2D();
        }

        loaded = false;

        contactFilter.useTriggers = true;
        contactFilter.useLayerMask = true;

	}
	
	// Update is called once per frame
	void Update () {
		if(!loaded && levelTrigger.IsTouching(contactFilter)) {
            LoadLevel();
        } else if(loaded && !levelTrigger.IsTouching(contactFilter)) {
            UnloadLevel();
        }
	}

    private void LoadLevel() {
        loaded = true;
        Debug.Log("Level Loaded");
        foreach(Transform child in transform) {
            if(child.name.Equals("EESpawnPoint")) {
                GameObject obj = (GameObject)Instantiate(Resources.Load("Characters/EasyEnemy"));
                if(obj != null) {
                    obj.transform.position = child.position;
                    obj.name = levelName + "EasyEnemy";
                } else {
                    Debug.Log("Failed to load enemey");
                }
            }
        }
    }

    private void UnloadLevel() {
        loaded = false;
        Debug.Log("Level UnLoaded");
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (GameObject o in obj) {
            if(o.name.Equals(levelName + "EasyEnemy")) {
                Destroy(o);
            }
        }
    }

}
