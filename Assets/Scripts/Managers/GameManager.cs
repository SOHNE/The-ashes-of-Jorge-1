using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    [Header("Canvas")]
    public GameObject gameover, gameplay;
    [Space]
    public int lives;
    private static GameManager gameManager;
    private CameraFollow cam;
    private GameObject player, ch;
    private Vector2 minxy => new Vector2(Camera.main.transform.position.x, 0);
    private Vector2 maxxy => new Vector2(Mathf.Infinity, 0);
    public ComboManager comboManager;
    public int TotalTime => (int)GetComponentInChildren<Timer>()._Timer;
    public int MaxCombo => (int)GetComponentInChildren<ComboManager>().MaxCombo;
    public int TotalCombo => (int)GetComponentInChildren<ComboManager>().TotalCombo;

    private void Awake() {

        if (gameManager == null) {
            gameManager = this;
        } else if (gameManager != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        cam = Camera.main.GetComponent<CameraFollow>();

        player = GameObject.FindGameObjectWithTag("Player");
        ch = GameObject.Find("----- Characters");
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

    public void ChangeScreen() {
        gameplay.SetActive(false);
        gameover.SetActive(true);

        GameObject pd = GameObject.Find("PlayerDeath");

        pd.transform.position = Camera.main.WorldToScreenPoint(player.transform.position + new Vector3(0, .5f, 0));
        pd.transform.rotation = player.transform.rotation;

        player.SetActive(false);
    }

    public void DisableElements() {
        Destroy(GameObject.Find("[ Events ]"));
        ch.SetActive(false);
    }
}