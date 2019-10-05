using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildings : MonoBehaviour {

    private Transform cameraTransform;

    private Vector3 currentPos;

    [SerializeField]
    private Transform frontBuildings;
    [SerializeField]
    private Transform backBuildings;

    public float frontSpeed;
    public float backSpeed;

    Vector3 frontBackOffset;

    bool hitLeftBound;
    bool hitRightBound;

	// Use this for initialization
	void Start () {

        frontSpeed = 1f;
        backSpeed = frontSpeed / 2f;

        cameraTransform = (GameObject.Find("GameCamera")).transform;
        if(cameraTransform == null) {
            Debug.Log("Cannot find Game Camera... Destroying");
            Destroy(this);
        }

        foreach(Transform child in transform) {
            if(child.name.Contains("Front")) {
                frontBuildings = child;
            } else if(child.name.Contains("Back")) {
                backBuildings = child;
            }
        }
        if(frontBuildings == null || backBuildings == null) {
            Debug.Log("Cannot find buildings... Destroying");
            Destroy(this);
        }

        currentPos = cameraTransform.position;

        hitLeftBound = false;
        hitRightBound = false;

        frontBackOffset = new Vector3();

        GetOffsets();

    }
	
    void CheckBounds() {
        //Front: (-7.7, -3.3, 12.8)	Back: (-10.8, -2.3, 13.7)
        //back bound Front: (-36.6, -3.3, 12.8)	Back: (-39.7, -2.3, 13.7)
        Vector3 frontLeftBound = new Vector3(-20.7f, -3.3f, 12.8f);
        Vector3 backLeftBound = new Vector3(-20.8f, -2.3f, 13.7f);
        Vector3 frontRightBound = new Vector3(-70.6f, -3.3f, 12.8f);
        Vector3 backRightBound = new Vector3(-70.7f, -2.3f, 13.7f);

        Vector3 cameraPos = cameraTransform.position;
        Vector3 frontPos = frontBuildings.position;
        Vector3 backPos = backBuildings.position;

        Vector3 frontOffset = new Vector3(frontPos.x - cameraPos.x, frontPos.y - cameraPos.y, frontPos.z - cameraPos.z);
        Vector3 backOffset = new Vector3(backPos.x - cameraPos.x, backPos.y - cameraPos.y, backPos.z - cameraPos.z);

        Vector3 frontNewPos = frontPos;
        Vector3 backNewPos = backPos;
        bool frontChangePos = false;
        bool backChangePos = false;

        //y offset should always be the same for now
        if (frontOffset.y != frontLeftBound.y) {
            frontChangePos = true;
            frontNewPos.y = frontLeftBound.y + cameraPos.y;
        }

        //y offset should always be the same for now
        if (backOffset.y != backLeftBound.y) {
            backChangePos = true;
            backNewPos.y = backLeftBound.y + cameraPos.y;
        }

        if (frontOffset.x > frontLeftBound.x) {
            frontChangePos = true;
            frontNewPos.x = frontLeftBound.x + cameraPos.x;
            hitLeftBound = true;
        } else if(frontOffset.x < frontRightBound.x) {
            frontChangePos = true;
            frontNewPos.x = frontRightBound.x + cameraPos.x;
            hitRightBound = true;
        }

        if (backOffset.x > backLeftBound.x) {
            backChangePos = true;
            backNewPos.x = backLeftBound.x + cameraPos.x;
            hitLeftBound = true;
        } else if (backOffset.x < backRightBound.x) {
            backChangePos = true;
            backNewPos.x = backRightBound.x + cameraPos.x;
            hitRightBound = true;
        }

        frontBuildings.position = frontChangePos ? frontNewPos : frontPos;
        backBuildings.position = backChangePos ? backNewPos : backPos;

        if(hitLeftBound || hitRightBound) {
            Vector3 fPos = frontBuildings.position;
            Vector3 bPos = backBuildings.position;
            float x = fPos.x - bPos.x;
            float y = fPos.y - bPos.y;
            float z = fPos.z - bPos.z;

            frontBackOffset = new Vector3(x,y,z);

        }

    }

    void SetLocation() {

        Vector3 frontStartingPosOffset = new Vector3(-21.3f, -3.3f, 12.8f);
        Vector3 backStartingPosOffset = new Vector3(-24.4f, -2.3f, 13.7f);

        float frontX = cameraTransform.position.x + frontStartingPosOffset.x;
        float frontY = cameraTransform.position.y + frontStartingPosOffset.y;
        float frontZ = cameraTransform.position.z + frontStartingPosOffset.z;

        float backX = cameraTransform.position.x + backStartingPosOffset.x;
        float backY = cameraTransform.position.y + backStartingPosOffset.y;
        float backZ = cameraTransform.position.z + backStartingPosOffset.z;

        Vector3 frontPosition = new Vector3(frontX, frontY, frontZ);
        Vector3 backPosition = new Vector3(backX, backY, backZ);

        frontBuildings.position = frontPosition;
        backBuildings.position = backPosition;

    }

    //Leaving this here it was only to get the offset amounts.....
    void GetOffsets() {

        //Front Pos: (-37.0, -3.3, 2.8) Back Pos: (-40.1, -2.3, 3.7)

        Vector3 frontStartingPosOffset = new Vector3(-37.0f, -3.3f, 2.8f);
        Vector3 backStartingPosOffset = new Vector3(-40.1f, -2.3f, 3.7f);

        float frontX = frontStartingPosOffset.x - cameraTransform.position.x;
        float frontY = frontStartingPosOffset.y - cameraTransform.position.y;
        float frontZ = frontStartingPosOffset.z - cameraTransform.position.z;

        float backX = backStartingPosOffset.x - cameraTransform.position.x;
        float backY = backStartingPosOffset.y - cameraTransform.position.y;
        float backZ = backStartingPosOffset.z - cameraTransform.position.z;

        Vector3 front = new Vector3(frontX,frontY,frontZ);
        Vector3 back = new Vector3(backX, backY, backZ);

       // Debug.Log("Front: " + front + "\tBack: "+back);

    }

	// Update is called once per frame
	void Update () {

        //Debug.Log("Front Pos: " + frontBuildings.position + " Back Pos: " + backBuildings.position);
        CheckBounds();

        Vector3 position = cameraTransform.position;
        if (position.x != currentPos.x) {
            MoveBuildings(position);
            currentPos = position;
        }

	}

    void BoundHitMove() {

        Vector3 frontPos = frontBuildings.position;
        Vector3 backPos = backBuildings.position;

        float x = frontBackOffset.x + backPos.x;
        float y = frontBackOffset.y + backPos.y;
        float z = frontBackOffset.z + backPos.z;

        Vector3 newPos = new Vector3(frontBackOffset.x + backPos.x, frontBackOffset.y + backPos.y, frontBackOffset.z + backPos.z);

        frontBuildings.position = newPos;

    }

    void MoveBuildings(Vector3 position) {
        
        float direction = (position.x - currentPos.x) > 0 ? 1 : -1;

        if(hitLeftBound || hitRightBound) {
            if (hitLeftBound && direction > 0) {
                hitLeftBound = false;
            }
            else if (hitRightBound && direction < 0) {
                hitRightBound = false;
            }
            else {
                BoundHitMove();
                return;
            }
        }

        float frontNewX = frontBuildings.position.x;
        frontNewX += direction * frontSpeed * Time.deltaTime;

        //Debug.Log(frontBuildings.position.x + " " + frontNewX);

        float backNewX = backBuildings.position.x;
        backNewX += direction * backSpeed * Time.deltaTime;

        Vector3 frontNewPos = new Vector3(frontNewX, frontBuildings.position.y, frontBuildings.position.z);
        Vector3 backNewPos = new Vector3(backNewX, backBuildings.position.y, backBuildings.position.z);

        frontBuildings.position = frontNewPos;
        backBuildings.position = backNewPos;

    }

}
