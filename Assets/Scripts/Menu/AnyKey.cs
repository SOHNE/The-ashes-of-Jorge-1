using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AnyKey : MonoBehaviour {

    void Update() {
        if (!Input.anyKey || Input.GetMouseButton(0)) { return; }
        GetComponent<Animator>().Play("IN");
    }


    public void FadeIn(){
        StartCoroutine(Load());
    }

    IEnumerator Load() {
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        do { yield return null; } while (!op.isDone);
    }
}