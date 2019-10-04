using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : Interactable {

    protected override void Interact() {

        GameObject check = GameObject.Find("SavePointMenu");

        PlayerSpawner spawner = (PlayerSpawner)GameObject.Find("PlayerSpawner").GetComponent(typeof(PlayerSpawner));
        spawner.SetSpawnPoint(transform.position);

        if (check == null) {
            Transform canvas = GameObject.Find("UI").transform;
            GameObject obj = (GameObject)Instantiate(Resources.Load("UI/SavePointMenu"), canvas);
            if (obj != null) {
                if (canvas != null) {
                    //obj.transform.SetParent(canvas);
                    obj.transform.localPosition = new Vector3(0, 0, 0);
                    obj.name = "SavePointMenu";
                }
            }
        }

    }

}
