using UnityEngine;

public class EventHandler : MonoBehaviour {
    public Transform Player;
    public Maestria Maestro;
    public float minZ, maxZ;
    public GameObject[] enemy;
    public int QttofEnemies;
    public float spawnTime = 3.5f;
    private Transform CharacterSpace;
    private int currentEnemies;
    private int Pos = 12;
    private SystemUI SUI;
    [Header("BossWave")]
    public GameObject Boss;
    public GameObject BossUI;
    private GameManager _GM;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {
        _GM = FindObjectOfType<GameManager>();
        SUI = GetComponentInParent<SystemUI>();
        CharacterSpace = GameObject.Find("Others").transform;
        Pos *= Random.Range(0, 2) == 0 ? -1 : 1;
    }

    private void Update() {
        if (currentEnemies < QttofEnemies) { return; }
        Desblo();
    }

    public void Desblo() {
        int enemies = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemies <= 0 || (enemies == 1 && FindObjectOfType<Enemy>().IsDead)) {
            _GM.Follow();
            SUI.AnimGO();
            gameObject.SetActive(false);
        }
    }

    public void SpawnEnemy() {

        Vector3 spawnPosition;
        spawnPosition.z = Random.Range(minZ, maxZ);

        Vector3 spawnPos = new Vector3(transform.position.x, 0, spawnPosition.z);

        spawnPos.x += Pos;
        Pos *= -1;

        GameObject en = Instantiate(enemy[Random.Range(0, enemy.Length)], spawnPos, Quaternion.identity);
        en.transform.SetParent(CharacterSpace, false);

        currentEnemies++;

        if (currentEnemies < QttofEnemies) {
            Invoke("SpawnEnemy", spawnTime);
        }
    }

    public void SpawnBoss() {

        Vector3 spawnPos = new Vector3(transform.position.x, 0, Player.position.z);

        spawnPos.x += 2.75f;

        GameObject en = Instantiate(Boss, spawnPos, Quaternion.identity);
        en.transform.SetParent(CharacterSpace, false);
        Maestro.ResetLoops();
        Maestro.NewSoundtrack("Music/Boss");
        en.GetComponent<Matthew>().BossWave = gameObject.GetComponent<EventHandler>();

        currentEnemies++;


    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) { return; }

        _GM.UnFollow();
        GetComponent<MeshCollider>().enabled = false;
        if (!Boss) {
            SpawnEnemy();
        } else {
            SpawnBoss();
        }
    }
}