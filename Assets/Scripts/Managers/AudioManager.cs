using UnityEngine;

public class AudioManager : MonoBehaviour {
    private void Awake() => GetComponent<Maestria>().NewSoundtrack("Music/Rei do Gado");

}