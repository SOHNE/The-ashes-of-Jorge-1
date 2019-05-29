using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour {
    public Player player;
    public TextMeshProUGUI LR;

    private GameManager GM;
    private Animator anim;
    private bool combVisible;


    private void Start() {
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        anim = GetComponent<Animator>();
    }

    private void Update() {
        LR.text = GM.lives.ToString();
    }

    #region "Animations Events"
    public void AnimGO() { anim.Play("Go"); }

    public void ComboIn() { if (combVisible) { return; } combVisible = !combVisible; anim.Play("Combo"); }

    public void ComboOut() { if (!combVisible) { return; } combVisible = !combVisible; anim.Play("ComboOut"); }

    public void P_ComboReset() {
        if (player.combo.Equals(0)) { return; }

        player.comboTimer = 0;
        player.combo = 0;
    }
    #endregion
}
