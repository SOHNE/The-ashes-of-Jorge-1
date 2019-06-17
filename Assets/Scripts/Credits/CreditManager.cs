using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditManager : MonoBehaviour {
    private bool loading;
    void Awake() {
        Maestria maestro = FindObjectOfType<Maestria>();
        maestro.NewSoundtrack("Music/Credits");
    }

    private void Update() {
        if (!Input.anyKey || loading) { return; }
        StartCoroutine(Load());
    }

    IEnumerator Load() {
        loading = true;
        AsyncOperation op = SceneManager.LoadSceneAsync(0);

        do { yield return null; } while (!op.isDone);
    }
}