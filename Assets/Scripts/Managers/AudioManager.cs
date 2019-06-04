using UnityEngine;

public class AudioManager : MonoBehaviour {
    private void Awake() => GetComponent<Maestria>().NewSoundtrack("Music/joshuaempyre__arcade-music-loop");

}