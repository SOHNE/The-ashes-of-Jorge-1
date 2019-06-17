using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyKey : MonoBehaviour {
    private void Update() {
        if (!Input.anyKey) { return; }
        GetComponent<Animator>().Play("IN");
    }

    public void FadeIn() => StartCoroutine(Load());

    IEnumerator Load() {
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        op.allowSceneActivation = false;

        do { yield return null; } while (!op.isDone);

        //op.allowSceneActivation = true;
        //print(op.allowSceneActivation);
    }
}