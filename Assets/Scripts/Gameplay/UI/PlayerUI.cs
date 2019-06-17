using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour {
    #region "Vars"
    public Player player;
    public TextMeshProUGUI LR;
    public TextMeshProUGUI ComboT;
    public float comboTimer;
    private GameManager GM;
    private Animator anim;
    private bool combVisible;
    private ComboManager comboManager;
    #endregion

    private void Awake() {
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        anim = GetComponent<Animator>();
        comboManager = FindObjectOfType<ComboManager>();
    }

    private void Start() => LR.text = GM.lives.ToString();

    private void Update() => ComboUpdater();

    public void ComboUpdater() {
        if (comboTimer >= 3) { return; }

        comboTimer += Time.deltaTime;
        if (!player.combo.Equals(0) && comboTimer >= 3f) { ComboOut(); }
        ComboT.text = string.Format("COMBO:\n\t{0}", player.combo);
    }

    public void UpdateLifes() => LR.text = GM.lives.ToString();

    #region "Animations Events"
    public void AnimGO() => anim.PlayInFixedTime("Go");

    public void ComboIn() {
        if (combVisible) { return; }
        combVisible = !combVisible;

        anim.PlayInFixedTime("Combo");
    }

    public void ComboOut() {
        if (!combVisible) { return; }
        combVisible = !combVisible;

        anim.PlayInFixedTime("ComboOut");

        comboManager.CalcCombo(player.combo);
    }

    public void P_ComboReset() {
        if (player.combo.Equals(0)) { return; }

        comboTimer = 0;
        player.combo = 0;
    }
    #endregion
}