using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAfterTime : MonoBehaviour {
    public float After;
    public GameObject Press;

    private void Start() => StartCoroutine(Wait());

    IEnumerator Wait() {
        yield return new WaitForSeconds(After);
        Press.SetActive(true);
    }
}