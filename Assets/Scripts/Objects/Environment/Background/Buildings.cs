using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildings : MonoBehaviour {

    private Transform cameraTransform;

    private Vector3 currentPos;

    private Vector3 frontStartingPosOffset;
    private Vector3 backStartingPosOffset;

    private Transform frontBuildings;
    private Transform backBuildings;

    public float frontSpeed;
    public float backSpeed;

	// Use this for initialization
	void Start () {

        frontSpeed = 1f;
        backSpeed = frontSpeed / 2f;

        cameraTransform = (GameObject.Find("GameCamera")).transform;
        if(cameraTransform == null) {
            Debug.Log("Cannot find Game Camera... Destroying");
            Destroy(this);
        }

        frontBuildings = (GameObject.Find("FrontRowBuilding")).transform;
        backBuildings = (GameObject.Find("BackRowBuilding")).transform;
        if(frontBuildings == null || backBuildings == null) {
            Debug.Log("Cannot find buildings... Destroying");
            Destroy(this);
        }

        currentPos = cameraTransform.position;

	}
	
	// Update is called once per frame
	void Update () {

        Vector3 position = cameraTransform.position;
        Debug.Log("Position" + position);
        Debug.Log("Camera Pos" + position);
        if (position.x != currentPos.x) {
            Debug.Log("Hey dude");
            MoveBuildings(position);
            currentPos = position;
        }

	}

    void MoveBuildings(Vector3 position) {

        float direction = (position.x - currentPos.x) > 0 ? 1 : -1;

        float frontNewX = frontBuildings.position.x;
        frontNewX += direction * frontSpeed * Time.deltaTime;

        float backNewX = backBuildings.position.x;
        backNewX += direction * backSpeed * Time.deltaTime;

        Vector3 frontNewPos = new Vector3(frontNewX, frontBuildings.position.y, frontBuildings.position.z);
        Vector3 backNewPos = new Vector3(backNewX, backBuildings.position.y, backBuildings.position.z);

        frontBuildings.position = frontNewPos;
        backBuildings.position = backNewPos;

    }

}
