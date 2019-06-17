using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheEnd : MonoBehaviour {
    private bool Started;

    private void Update() {
        if (!Input.anyKey || Started) { return; }
        StartCoroutine(Load());
    }

    private IEnumerator Load() {
        Started = true;
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        do { yield return null; } while (!op.isDone);
    }
}