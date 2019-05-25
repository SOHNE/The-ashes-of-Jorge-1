using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour {

    private GameManager gm;
    private Player player;
    private TextMeshProUGUI P_NAME;
    public TextMeshProUGUI E_NAME;
    private Slider P_HP;
    public Slider E_HP;
    public GameObject EnemyUI;
    private HealthBar P_HB;
    public GameObject VOID;
    private Animator anim;
    public float EnemyUITime = 4f;
    private float EnemyTimer;

    private bool combVisible;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        P_HB = GameObject.Find("HPB").GetComponent<HealthBar>();

        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update() {
        GameObject.Find("Lifes").GetComponent<TextMeshProUGUI>().text = gm.lives.ToString();
    }

    public void EnemyUpdate(int maxHealth, int currentHealth, string name = "Enemy") {
        EnemyUI.SetActive(true);
        VOID.SetActive(false);

        E_HP.maxValue = maxHealth;
        E_HP.value = currentHealth;
        E_NAME.text = name;
        EnemyTimer = 0;
    }

    #region "Animations Events"
    public void AnimGO() { anim.Play("Go"); }

    public void ComboIn() { if (combVisible) { return; } combVisible = true; anim.Play("Combo"); }

    public void ComboOut() { if (!combVisible) { return; } combVisible = false; anim.Play("ComboOut"); }

    public void SoundGO() {
        AudioSource tmp = GetComponent<AudioSource>();
        tmp.clip = Resources.Load<AudioClip>("SFX/gogogo");
        tmp.loop = false;
        tmp.Play();
    }
    #endregion

    #region "Player HealthBar"
    /// <summary>
    /// Função chamada no Animator do UI
    /// </summary>
    public void P_ComboReset() {
        if (player.combo.Equals(0)) { return; }

        player.comboTimer = 0;
        player.combo = 0;
    }
    #endregion
}