using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public GameObject gameover;
    public GameObject gameplay;
    [Space]
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

        cam = Camera.main.GetComponent<CameraFollow>();
        
        //UI_PlayerHP = GameObject.Find("P_HP").GetComponent<Slider>();
    }

    public void UnFollow() {
        cam.minXAndY = minxy;
        cam.maxXAndY = minxy;
        cam.IsBlock = true;
    }
    public void Follow() {
        cam.maxXAndY = maxxy;
        cam.IsBlock = false;
    }

    // Update is called once per frame
    void Update() {

        // UI_PlayerHP.value = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().HP;
        if (lives == 0 && GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().IsDead) {
            gameplay.SetActive(false);
            gameover.SetActive(true);

            GameObject.Find("PlayerDeath").transform.position = Camera.main.WorldToScreenPoint(GameObject.FindGameObjectWithTag("Player").transform.position);
            GameObject.Find("PlayerDeath").transform.rotation = GameObject.FindGameObjectWithTag("Player").transform.rotation;
        }
    }
}