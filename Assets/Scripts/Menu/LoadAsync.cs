using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadAsync : MonoBehaviour {
    [SerializeField] private string NextScene = string.Empty;

    private void Start() { if(!NextScene.Equals(string.Empty)) { StartCoroutine(Load(NextScene)); } }

    public void Do(string sceneName) => StartCoroutine(Load(sceneName));

    IEnumerator Load(string sceneName) {

        AsyncOperation op = SceneManager.LoadSceneAsync(SceneManager.GetSceneByName(sceneName).buildIndex);
        //op.allowSceneActivation = false;

        do { yield return null; } while (!op.isDone);
    }
}