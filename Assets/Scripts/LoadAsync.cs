using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadAsync : MonoBehaviour {
    private void Awake() {
        StartCoroutine(Load());
    }

    IEnumerator Load() {
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        do { yield return null; } while (!op.isDone);
    }
}