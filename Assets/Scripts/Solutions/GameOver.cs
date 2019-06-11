using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {
    [Header("Objects")]
    public Text time;
    public Text combo;

    private void Awake() {
        GameManager gm = FindObjectOfType<GameManager>();

        time.text = string.Format("{0}", gm.TotalTime);
        combo.text = string.Format("{0}", gm.MaxCombo);
    }
}