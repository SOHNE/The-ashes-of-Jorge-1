using UnityEngine;

public class SystemUI : MonoBehaviour {
    public GameObject Gos;
    private Animator anim;
    private AudioSource aud;
    private AudioClip ac;
    private void Awake() {
        anim = Gos.GetComponent<Animator>();

        aud = GetComponentInChildren<AudioSource>();
        aud.clip = Resources.Load<AudioClip>("SFX/gogogo");
    }

    public void AnimGO() { aud.Play(); anim.PlayInFixedTime("Go"); }

}