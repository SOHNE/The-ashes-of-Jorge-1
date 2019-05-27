using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {
    private WaveManager WM;
    private GameManager GM;

    private GameObject EH;

    private bool EndWave = false;

    private Animator Go_anim;

    public float minZ, maxZ;
    public GameObject[] enemy;
    private int QttofEnemies;
    public float spawnTime;

    private int currentEnemies;

    // Start is called before the first frame update
    void Awake() {
        WM = GameObject.FindGameObjectWithTag("WaveController").GetComponent<WaveManager>();
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        EH = GameObject.FindGameObjectWithTag("EventHandler");

        Go_anim = GameObject.Find("[ Go ]").GetComponent<Animator>();

    }

    private void Update() {
        /*if (Input.GetKeyDown("r")) {
            GameObject[] en = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < en.Length; i++) {
                Destroy(en[i]);
            }
        }*/

        if (!EndWave) { return; }

        QttofEnemies = EH.GetComponent<EventHandler>().QttofEnemies;
        if (currentEnemies >= QttofEnemies) {
            int enemies = FindObjectsOfType<Enemy>().Length;
            if (enemies <= 0) {
                Go_anim.Play("Go");
                GM.Follow();
                EndWave = !EndWave;
            }
        }
    }

    public void SpawnEnemy() {
        Random.InitState(Random.Range(2,7));
        bool positionX = Random.Range(0, 2) == 0 ? true : false;
        Vector3 spawnPosition;
        spawnPosition.z = Random.Range(minZ, maxZ);

        if (positionX) {
            spawnPosition = new Vector3(EH.transform.position.x + 5, 0, spawnPosition.z);
        } else {
            spawnPosition = new Vector3(EH.transform.position.x - 5, 0, spawnPosition.z);
        }

        Instantiate(enemy[Random.Range(0, enemy.Length)], spawnPosition, Quaternion.identity);
        currentEnemies++;

        if (currentEnemies < QttofEnemies) {
            Invoke("SpawnEnemy", spawnTime);
        }
    }
}
