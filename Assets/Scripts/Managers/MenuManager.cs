using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public GameObject menu, tuto;
    private AsyncOperation op;

    private void Update() {
        if (!Input.anyKeyDown || !menu.activeInHierarchy) { return; }
        GetComponent<Animator>().PlayInFixedTime("IN");
    }

    public void FadeIn() {
        if (!menu.activeInHierarchy) { return; }
        
        menu.SetActive(false);
        tuto.SetActive(true);
        GetComponent<Animator>().PlayInFixedTime("OUT");

        StartCoroutine(Wait());
        StartCoroutine(Load());
    }

    IEnumerator Load() {
        op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        op.allowSceneActivation = false;

        do { yield return null; } while (!op.isDone);
    }

    IEnumerator Wait() {
        yield return new WaitForSeconds(5f);
        GetComponent<Animator>().PlayInFixedTime("IN");
        yield return new WaitForSeconds(1.25f);
        op.allowSceneActivation = true;
    }
}