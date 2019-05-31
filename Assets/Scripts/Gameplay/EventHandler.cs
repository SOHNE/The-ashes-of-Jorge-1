using UnityEngine;

public class EventHandler : MonoBehaviour {

    public float minZ, maxZ;
    public GameObject[] enemy;
    public int QttofEnemies;
    public float spawnTime = 3.5f;

    private Transform CharacterSpace;

    private int currentEnemies;

    private GameManager _GM;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {
        _GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        CharacterSpace = GameObject.Find("Others").transform;
    }
    public int id;
    private void Update() {

        /*if (Input.GetKeyDown("r")) {
            GameObject[] en = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < en.Length; i++) {
                Destroy(en[i]);
            }
        }*/

        if (currentEnemies < QttofEnemies) { return; }
        int enemies = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemies <= 0 || (enemies == 1 && FindObjectOfType<Enemy>().HP <= 0)) {
            _GM.Follow();
            //FindObjectOfType<UIManager>().AnimGO();
            Destroy(gameObject);
            gameObject.SetActive(false);
        }

    }
    public void SpawnEnemy() {

        bool positionX = Random.Range(0, 2) == 0 ? true : false;
        Vector3 spawnPosition;
        spawnPosition.z = Random.Range(minZ, maxZ);

        Vector3 spawnPos = new Vector3(transform.position.x, 0, spawnPosition.z);
        spawnPos.x += positionX ? 10 : -10;

        GameObject en = Instantiate(enemy[Random.Range(0, enemy.Length)], spawnPos, Quaternion.identity);
        en.transform.SetParent(CharacterSpace, false);

        currentEnemies++;

        if (currentEnemies < QttofEnemies) {
            Invoke("SpawnEnemy", spawnTime);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) { return; }

        _GM.UnFollow();
        GetComponent<MeshCollider>().enabled = false;
        SpawnEnemy();
    }
}