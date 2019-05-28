using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public int lives;
    public int characterIndex;

    private static GameManager gameManager;
    private CameraFollow cam;
    private WaveManager wave;

    private int CurrentWave;
    //private int CurrentEnemies;

    private Slider UI_PlayerHP;

    private Vector2 minxy => new Vector2(Camera.main.transform.position.x, 0);
    private Vector2 maxxy => new Vector2(Mathf.Infinity, 0);
    void Awake() {

        if (gameManager == null) {
            gameManager = this;
        } else if (gameManager != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        //

        cam = Camera.main.GetComponent<CameraFollow>();
        
        //UI_PlayerHP = GameObject.Find("P_HP").GetComponent<Slider>();
    }

    public void UnFollow() {
        cam.minXAndY = minxy;
        cam.maxXAndY = minxy;
    }
    public void Follow() {
        cam.maxXAndY = maxxy;
    }

    // Update is called once per frame
    void Update() {
        // UI_PlayerHP.value = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().HP;
        if (lives <= 0 && GameObject.Find("Gameplay")) {
            GameObject.Find("Gameplay").GetComponentInChildren<Canvas>().enabled = false;
            GameObject.Find("GameOver").GetComponentInChildren<Canvas>().enabled = true;
        }
    }
}