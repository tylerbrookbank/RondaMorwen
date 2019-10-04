using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour {

    private Transform pTransform;
    private Stats pStats;
    private PlayerController player;
    private Image screenCover;
    private Text deathText;

    private int state = 0;
    private const int wait = 0;
    private const int killed = 1;
    private const int spawn = 2;
    private const int screenOn = 3;
    private const int screenOff = 4;

    private Vector3 spawnPoint;
    private Quaternion defaultRotation;
    private float timer;

	// Use this for initialization
	void Start () {

        pTransform = (GameObject.Find("Player")).transform;
        if(pTransform == null) Debug.Log("pTransform is null");

        player = (PlayerController)pTransform.GetComponent(typeof(PlayerController));
        if (player == null) Debug.Log("player is null");

        pStats = player.GetStats();
        if (pStats == null) Debug.Log("pStats is null");

        screenCover = (Image)GameObject.Find("ScreenCover").GetComponent(typeof(Image));
        if (screenCover == null) Debug.Log("screenCover is null");

        deathText = (Text)GameObject.Find("DeathText").GetComponent(typeof(Text));
        if (deathText == null) Debug.Log("deathText is null");

        spawnPoint = new Vector3(0f, 0f, 0f);
        defaultRotation = deathText.transform.rotation;

	}
	
	// Update is called once per frame
	void Update () {
        GetStats();
        StateMachine();
	}

    public void GetStats() {
        pStats = player.GetStats();
        if (pStats == null) Debug.Log("pStats is null");
    }

    public void SetSpawnPoint(Vector3 spawnP) {
        spawnPoint = spawnP;
    }

    private void StateMachine() {
        switch(state) {
            case wait:
                CheckState();
                break;
            case killed:
                Killed();
                break;
            case spawn:
                Spawn();
                break;
            case screenOff:
                TurnOffScreenCover();
                break;
            case screenOn:
                TurnOnScreenCover();
                break;
        }
    }

    private void CheckState() {
        if(pStats.GetDead()) {
            state = killed;
            AlignScreen();
            timer = 0f;
            StopPlayer();
        }
    }

    private void StopPlayer() {
        player.TakeControls();
    }

    private void StartPlayer() {
        player.GiveControls();
    }

    private void Killed() {
        float maxTime = 1f;
        if (timer >= maxTime) {
            timer = 0f;
            state = screenOn;
        }
        timer += Time.deltaTime;
    }

    private void TurnOnScreenCover() {
        float a = screenCover.color.a;
        float speed = 1f;
        a += speed * Time.deltaTime;

        Color c1 = new Color(screenCover.color.r,screenCover.color.g, screenCover.color.b, a);
        Color c2 = new Color(deathText.color.r, deathText.color.g, deathText.color.b, a);

        screenCover.color = c1;
        deathText.color = c2;

        if(a >= 1f) {
            timer = 0f;
            state = spawn;

            GameObject obj = (GameObject)GameObject.Find("BitPool");
            if(obj != null) {
                Destroy(obj);
            }

            obj = (GameObject)Instantiate(Resources.Load("Interactables/BitPool"));
            if(obj != null) {
                BitPool bp = (BitPool)obj.GetComponent(typeof(BitPool));
                bp.SetBits(player.GetStats().GetBits());
                player.GetStats().AddBits(-1 * player.GetStats().GetBits());
                bp.transform.position = player.transform.position;
                bp.name = "BitPool";
            }

        }
    }

    private void TurnOffScreenCover() {
        WaveText();
        float a = screenCover.color.a;
        float speed = 1f;
        a -= speed * Time.deltaTime;

        Color c1 = new Color(screenCover.color.r, screenCover.color.g, screenCover.color.b, a);
        Color c2 = new Color(deathText.color.r, deathText.color.g, deathText.color.b, a);

        screenCover.color = c1;
        deathText.color = c2;

        if (a <= 0f) {
            state = wait;
            timer = 0f;
            StartPlayer();
        }
    }

    private void AlignScreen() {
        deathText.transform.rotation = defaultRotation;
        deathText.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
    }

    private void Spawn() {
        if(pTransform.position != spawnPoint)
            pTransform.position = spawnPoint;
        WaveText();
        ContinueFlag();
        if(Input.GetButtonDown("Confirm")) {
            state = screenOff;
            ContinueFlagOff();
            pStats.SetCurrentHealth(pStats.GetHealth());
        }
    }

    private void ContinueFlagOff() {
        Image flag = (Image)deathText.GetComponentInChildren(typeof(Image));
        Color c = new Color(flag.color.r, flag.color.g, flag.color.b, 0);
        flag.color = c;
    }

    private void ContinueFlag() {
        Image flag = (Image)deathText.GetComponentInChildren(typeof(Image));

        if(timer >= 1) {
            float a;
            if (flag.color.a == 1) {
                a = 0;
            } else {
                a = 1;
            }
            Color c = new Color(flag.color.r, flag.color.g, flag.color.b, a);
            flag.color = c;
            timer = 0f;
        }
        timer += Time.deltaTime;
    }

    private void WaveText() {

    }

}
