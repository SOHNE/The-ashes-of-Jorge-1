using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    [Header("Canvas")]
    public GameObject gameplay;
    public GameObject gameover, theEnd;
    [Header("Reset")]
    public GameObject events;
    [Header("Player Related")]
    public int lives;
    private static GameManager gameManager;
    private CameraFollow cam;
    private GameObject player;
    public GameObject ch, pd; // character, player death
    private Vector2 minxy => new Vector2(Camera.main.transform.position.x, 0);
    private Vector2 maxxy => new Vector2(Mathf.Infinity, 0);
    public ComboManager comboManager;
    public int TotalTime => (int)GetComponentInChildren<Timer>()._Timer;
    public int MaxCombo => (int)GetComponentInChildren<ComboManager>().MaxCombo;
    public int TotalDeaths => (int)GetComponentInChildren<ComboManager>().TotalDeaths;

    private void Awake() {

        if (gameManager == null) {
            gameManager = this;
        } else if (gameManager != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        cam = Camera.main.GetComponent<CameraFollow>();

        player = GameObject.FindGameObjectWithTag("Player");
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

    /// <summary>
    /// Change the gameplay scrren between Gameplay to GameOver.
    /// Pass 0 for Defeat, any else for Win
    /// </summary>
    public void ChangeScreen(int mode) {
        gameplay.SetActive(false);

        if (mode.Equals(0)) {
            gameover.SetActive(true);
            pd.transform.position = Camera.main.WorldToScreenPoint(player.transform.position + new Vector3(0, .5f, 0));
            pd.transform.rotation = player.transform.rotation;
            pd.SetActive(true);
            player.SetActive(false);
        } else {
            theEnd.SetActive(true);
        }

    }

    public void DisableElements() {
        ch.SetActive(false);
        events.SetActive(false);
        //foreach (Transform child in events.transform) { child.GetComponent<MeshCollider>().enabled = true; }
    }
}