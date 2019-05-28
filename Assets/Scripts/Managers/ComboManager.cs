using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboManager : MonoBehaviour {

    private TextMeshProUGUI txt;
    private Animator anim;

    private void Awake() { txt = GetComponentInChildren<TextMeshProUGUI>(); anim = GetComponent<Animator>(); }

    public void ComboUp(int total) => txt.text = string.Format(@"Combo:\n\t{0}", total);
    public void ComboIn() => anim.Play("ComboIN");
    public void ComboOut() => anim.Play("ComboOUT");

}