using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadAsync : MonoBehaviour {
    // private AsyncOperation op;

    private void Start() => StartCoroutine(Load());

    IEnumerator Load() {
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        //op.allowSceneActivation = false;

        do {
            yield return null;
        } while (!op.isDone);
    }
}