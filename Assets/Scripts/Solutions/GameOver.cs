using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour {
    [Header("Objects")]
    public TextMeshProUGUI stats;
    private string Base => "Tempo Total:  \t{0}\nInimigos mortos:\t{1}\nCombo maximo:\t{2}\n";

    private void Awake() {

        GameManager gm = FindObjectOfType<GameManager>();
        stats.text = string.Format(Base, gm.TotalTime, gm.TotalDeaths, gm.MaxCombo);
    }
}