using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour {
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI LR;
    [SerializeField] private TextMeshProUGUI ComboT;

    public float comboTimer;

    private GameManager GM;
    private Animator anim;
    private bool combVisible;

    private void Start() {
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        ComboT = GameObject.Find("Combo").GetComponentInChildren<TextMeshProUGUI>();
        anim = GetComponent<Animator>();
    }

    private void Update() {
        LR.text = GM.lives.ToString();

        ComboUpdater();
    }

    public void ComboUpdater() {
        if (comboTimer >= 3) { return; }

        comboTimer += Time.deltaTime;
        if (!player.combo.Equals(0) && comboTimer >= 3f) { ComboOut(); }
        ComboT.text = string.Format("COMBO:\n\t{0}", player.combo);
    }

    #region "Animations Events"
    public void AnimGO() { anim.Play("Go"); }

    public void ComboIn() { if (combVisible) { return; } combVisible = !combVisible; anim.Play("Combo"); }

    public void ComboOut() { if (!combVisible) { return; } combVisible = !combVisible; anim.Play("ComboOut"); }

    public void P_ComboReset() {
        if (player.combo.Equals(0)) { return; }

        comboTimer = 0;
        player.combo = 0;
    }
    #endregion
}