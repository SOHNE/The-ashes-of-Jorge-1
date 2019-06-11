using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {

    private float time;
    public float _Timer => time;
    public TextMeshProUGUI Clock;
    private CharacterBase pl;

    private void Start() => pl = GameObject.FindObjectOfType<Player>();

    void Update() {
        if (!SceneManager.GetActiveScene().name.Equals("Gameplay") || pl.IsDead) { return; }
        time = Time.timeSinceLevelLoad;
        Clock.text = string.Format("{0}", (int)time);
    }
}