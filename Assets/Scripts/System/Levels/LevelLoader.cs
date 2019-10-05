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
        Debug.Log(levelName + " Level Loaded");
        foreach(Transform child in transform) {
            if (child.name.Equals("EESpawnPoint")) {
                GameObject obj = (GameObject)Instantiate(Resources.Load("Characters/EasyEnemy"));
                if (obj != null) {
                    obj.transform.position = child.position;
                    obj.name = levelName + "EasyEnemy";
                } else {
                    Debug.Log("Failed to load enemey");
                }
            } else if (child.name.Contains("BitCollectionSpawner")) {
                SpawnBitCollection(child, child.name);
            } else if (child.name.Contains("BackgroundSpawner_")) {
                SpawnBackground(child, child.name);
            }
        }
    }

    private void SpawnBitCollection(Transform child, string name) {
        int startIndex = name.IndexOf("_") + 1;
        string type = name.Substring(startIndex);
        switch (type) {
            case "100": 
                {
                    GameObject obj = (GameObject)Instantiate(Resources.Load("Interactables/BitCollection"));
                    if (obj != null) {
                        SetUpBitCollection(obj, child, 100);
                    }
                    else {
                        Debug.Log("Failed to Load " + child.name);
                    }
                }
                break;
            case "500": 
                {
                    GameObject obj = (GameObject)Instantiate(Resources.Load("Interactables/BitCollection"));
                    if (obj != null) {
                        SetUpBitCollection(obj, child, 500);
                    }
                    else {
                        Debug.Log("Failed to Load " + child.name);
                    }
                }
                break;
            case "1000": 
                {
                    GameObject obj = (GameObject)Instantiate(Resources.Load("Interactables/BitCollection"));
                    if (obj != null) {
                        SetUpBitCollection(obj, child, 1000);
                    }
                    else {
                        Debug.Log("Failed to Load " + child.name);
                    }
                }
                break;
        }
    }

    private void SetUpBitCollection(GameObject obj, Transform child, int bitAmount) {
        BitCollection bc = (BitCollection)obj.GetComponent(typeof(BitCollection));
        if (bc != null) {
            bc.SetBitAmount(bitAmount);
            bc.SetSpawnerObject(child.gameObject);
        }
        else { 
            Debug.Log("couldn't find bit collection script");
        }
        obj.transform.position = child.position;
        obj.name = levelName + " Bit Collection " + bitAmount;
    }

    private void SpawnBackground(Transform child, string name) {

        int startIndex = name.IndexOf("_") + 1;
        string type = name.Substring(startIndex);
        switch (type) {
            case "Buildings":
                GameObject obj = (GameObject)Instantiate(Resources.Load("Background/Buildings"));
                if(obj != null) {
                    obj.name = levelName + "Buildings";
                } else {
                    Debug.Log("Failed to Load Buildings");
                }
                break;
        }

    }

    private void UnloadLevel() {
        loaded = false;
        Debug.Log(levelName + " Level UnLoaded");
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (GameObject o in obj) {
            if(o.name.Equals(levelName + "EasyEnemy")
                || o.name.Equals(levelName + "Buildings")
                || o.name.Contains(levelName + " Bit Collection")) {
                Destroy(o);
            }
        }
    }

}
